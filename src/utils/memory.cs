namespace Zodiak;

public static unsafe class memory
{
    private const uint PAGE_EXECUTE_READWRITE = 0x40;
    private const uint PAGE_NOACCESS = 0x01;
    private const uint PAGE_GUARD = 0x100;
    private const uint MEM_COMMIT = 0x1000;

    private const int PATTERN_HEX_BASE = 16;
    private const string PATTERN_WILDCARD = "??";

    public static nint rva(nint base_address, nint offset) => base_address + offset;

    public static T read<T>(nint addr) where T : unmanaged => *(T*)addr;

    public static void write<T>(nint addr, T value) where T : unmanaged => *(T*)addr = value;

    public static bool is_readable(nint addr, nuint size)
    {
        if (addr == 0) return false;
        if (native_interop.virtual_query(addr, out var mbi,
                (nuint)sizeof(native_interop.memory_basic_information)) == 0)
            return false;

        if ((mbi.Protect & PAGE_NOACCESS) != 0 || (mbi.Protect & PAGE_GUARD) != 0)
            return false;

        nint end = addr + (nint)size;
        nint region_end = mbi.BaseAddress + (nint)mbi.RegionSize;
        return end <= region_end;
    }

    public static byte[] patch(nint addr, byte[] new_bytes)
    {
        byte[] original = new byte[new_bytes.Length];
        for (int i = 0; i < new_bytes.Length; i++)
            original[i] = *(byte*)(addr + i);

        if (!native_interop.virtual_protect(addr, (nuint)new_bytes.Length,
                PAGE_EXECUTE_READWRITE, out uint old))
            return Array.Empty<byte>();

        for (int i = 0; i < new_bytes.Length; i++)
            *(byte*)(addr + i) = new_bytes[i];

        native_interop.virtual_protect(addr, (nuint)new_bytes.Length, old, out _);
        return original;
    }

    public static nint find_pattern(nint base_address, string pattern)
    {
        string[] parts = pattern.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int len = parts.Length;
        if (len == 0) return 0;

        byte?[] bytes = new byte?[len];
        for (int i = 0; i < len; i++)
            bytes[i] = parts[i] == PATTERN_WILDCARD
                ? null
                : Convert.ToByte(parts[i], PATTERN_HEX_BASE);

        nint addr = base_address;
        while (true)
        {
            if (native_interop.virtual_query(addr, out var mbi,
                    (nuint)sizeof(native_interop.memory_basic_information)) == 0)
                break;

            bool readable = mbi.State == MEM_COMMIT
                            && (mbi.Protect & PAGE_NOACCESS) == 0
                            && (mbi.Protect & PAGE_GUARD) == 0;

            if (readable)
            {
                nint start = mbi.BaseAddress;
                nint end = mbi.BaseAddress + (nint)mbi.RegionSize - len;

                for (nint p = start; p < end; p++)
                    if (matches_at(p, bytes)) return p;
            }

            addr = mbi.BaseAddress + (nint)mbi.RegionSize;
        }
        return 0;
    }

    private static bool matches_at(nint addr, byte?[] pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i].HasValue && *(byte*)(addr + i) != pattern[i]!.Value)
                return false;
        }
        return true;
    }
}

public sealed class byte_patch
{
    private const byte NOP = 0x90;

    private readonly string _pattern;
    private readonly int _size;

    private nint _addr;
    private byte[] _original = Array.Empty<byte>();

    public bool Applied { get; private set; }

    public byte_patch(string pattern, int size)
    {
        _pattern = pattern;
        _size = size;
    }

    public bool apply(nint base_address)
    {
        if (Applied) return true;

        if (_addr == 0)
        {
            _addr = memory.find_pattern(base_address, _pattern);
            if (_addr == 0)
               return false;
            
        }

        byte[] nops = new byte[_size];
        for (int i = 0; i < _size; i++) nops[i] = NOP;

        _original = memory.patch(_addr, nops);
        if (_original.Length == 0) return false;

        Applied = true;
        return true;
    }

    public void revert()
    {
        if (!Applied) return;
        memory.patch(_addr, _original);
        Applied = false;
    }
}