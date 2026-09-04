using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class patcher
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

    [DllImport("kernel32.dll")]
    static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

    const uint PAGE_EXECUTE_READWRITE = 0x40;

    private struct PatchInfo
    {
        public IntPtr address;
        public byte[] original;
        public uint oldProtect;
    }

    private static PatchInfo[] patches = new PatchInfo[3];
    private static int patch_count = 0;

    public static IntPtr find_pattern(string signature)
    {
        var tokens = signature.Split(' ');
        byte[] pattern = new byte[tokens.Length];
        bool[] mask = new bool[tokens.Length];

        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i] == "?")
            {
                mask[i] = false;
                pattern[i] = 0;
            }
            else
            {
                mask[i] = true;
                pattern[i] = Convert.ToByte(tokens[i], 16);
            }
        }

        IntPtr baseAddr = native.GetModuleHandle(null);
        int moduleSize = native.get_module_size(baseAddr);
        if (moduleSize == 0) return IntPtr.Zero;

        byte* start = (byte*)baseAddr;
        for (int i = 0; i < moduleSize - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (mask[j] && start[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
                return (IntPtr)(start + i);
        }
        return IntPtr.Zero;
    }

    public static bool apply(string signature, int nop_count)
    {
        IntPtr addr = find_pattern(signature);
        if (addr == IntPtr.Zero)
        {
            logger.warn("patcher", "pattern not found: " + signature);
            return false;
        }
        logger.info("patcher", $"pattern found at 0x{addr:X}");

        byte[] original = new byte[nop_count];
        Marshal.Copy(addr, original, 0, nop_count);

        uint oldProtect;
        VirtualProtect(addr, (UIntPtr)nop_count, PAGE_EXECUTE_READWRITE, out oldProtect);

        for (int i = 0; i < nop_count; i++)
            *(byte*)(addr + i) = 0x90;

        VirtualProtect(addr, (UIntPtr)nop_count, oldProtect, out _);
        FlushInstructionCache(System.Diagnostics.Process.GetCurrentProcess().Handle, addr, (UIntPtr)nop_count);

        if (patch_count < patches.Length)
        {
            patches[patch_count].address = addr;
            patches[patch_count].original = original;
            patches[patch_count].oldProtect = oldProtect;
            patch_count++;
        }

        return true;
    }

    public static void revert_all()
    {
        for (int i = 0; i < patch_count; i++)
        {
            if (patches[i].address != IntPtr.Zero)
            {
                uint oldProtect;
                VirtualProtect(patches[i].address, (UIntPtr)patches[i].original.Length, PAGE_EXECUTE_READWRITE, out oldProtect);
                Marshal.Copy(patches[i].original, 0, patches[i].address, patches[i].original.Length);
                VirtualProtect(patches[i].address, (UIntPtr)patches[i].original.Length, patches[i].oldProtect, out _);
                FlushInstructionCache(System.Diagnostics.Process.GetCurrentProcess().Handle, patches[i].address, (UIntPtr)patches[i].original.Length);
            }
        }
        patch_count = 0;
    }
}