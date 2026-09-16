using System.Runtime.InteropServices;

namespace Zodiak;

public static class native_interop
{
    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_Initialize();

    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_Uninitialize();

    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_CreateHook(nint pTarget, nint pDetour, out nint ppOriginal);

    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_EnableHook(nint pTarget);

    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_DisableHook(nint pTarget);

    [DllImport("minhook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_RemoveHook(nint pTarget);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern nint GetModuleHandleW(string? lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualProtect(nint addr, nuint size, uint newProtect, out uint oldProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nuint VirtualQuery(nint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, nuint dwLength);

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public nint BaseAddress;
        public nint AllocationBase;
        public uint AllocationProtect;
        public nuint RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    [DllImport("user32.dll")] public static extern short GetAsyncKeyState(int vKey);
    [DllImport("user32.dll")] public static extern nint GetForegroundWindow();
    [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT p);
    [DllImport("user32.dll")] public static extern bool ScreenToClient(nint hWnd, ref POINT p);
    [DllImport("user32.dll")] public static extern bool GetClientRect(nint hWnd, out RECT rc);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X, Y; }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT { public int L, T, R, B; }
}