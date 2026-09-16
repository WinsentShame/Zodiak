namespace Zodiak;

public static unsafe class memory
{
    private const uint PAGE_EXECUTE_READWRITE = 0x40;
    private const uint PAGE_NOACCESS = 0x01;
    private const uint PAGE_GUARD = 0x100;
    private const uint MEM_COMMIT = 0x1000;

    public static nint rva(nint baseAddr, nint offset) => baseAddr + offset;

    public static T read<T>(nint addr) where T : unmanaged => *(T*)addr;

    public static void write<T>(nint addr, T value) where T : unmanaged => *(T*)addr = value;

    public static bool isReadable(nint addr, nuint size)
    {
        if (addr == 0) return false;
        if (native_interop.VirtualQuery(addr, out var mbi,
                (nuint)sizeof(native_interop.MEMORY_BASIC_INFORMATION)) == 0)
            return false;

        if ((mbi.Protect & PAGE_NOACCESS) != 0 || (mbi.Protect & PAGE_GUARD) != 0)
            return false;

        nint end = addr + (nint)size;
        nint regionEnd = mbi.BaseAddress + (nint)mbi.RegionSize;
        return end <= regionEnd;
    }

    public static byte[] patch(nint addr, byte[] newBytes)
    {
        byte[] original = new byte[newBytes.Length];
        for (int i = 0; i < newBytes.Length; i++)
            original[i] = *(byte*)(addr + i);

        if (!native_interop.VirtualProtect(addr, (nuint)newBytes.Length, PAGE_EXECUTE_READWRITE, out uint old))
            return Array.Empty<byte>();

        for (int i = 0; i < newBytes.Length; i++)
            *(byte*)(addr + i) = newBytes[i];

        native_interop.VirtualProtect(addr, (nuint)newBytes.Length, old, out _);
        return original;
    }

    public static nint findPattern(nint baseAddr, string pattern)
    {
        string[] parts = pattern.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int len = parts.Length;
        if (len == 0) return 0;

        byte?[] bytes = new byte?[len];
        for (int i = 0; i < len; i++)
            bytes[i] = parts[i] == "??" ? null : Convert.ToByte(parts[i], 16);

        nint addr = baseAddr;
        while (true)
        {
            if (native_interop.VirtualQuery(addr, out var mbi,
                    (nuint)sizeof(native_interop.MEMORY_BASIC_INFORMATION)) == 0)
                break;

            bool readable = mbi.State == MEM_COMMIT
                            && (mbi.Protect & PAGE_NOACCESS) == 0
                            && (mbi.Protect & PAGE_GUARD) == 0;

            if (readable)
            {
                nint start = mbi.BaseAddress;
                nint end = mbi.BaseAddress + (nint)mbi.RegionSize - len;
                for (nint p = start; p < end; p++)
                {
                    bool match = true;
                    for (int i = 0; i < len; i++)
                    {
                        if (bytes[i].HasValue && *(byte*)(p + i) != bytes[i]!.Value)
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match) return p;
                }
            }

            addr = mbi.BaseAddress + (nint)mbi.RegionSize;
        }
        return 0;
    }
}