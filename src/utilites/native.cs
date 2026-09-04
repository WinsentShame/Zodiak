using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class native
{
    [DllImport("minhook")]
    public static extern int MH_Initialize();

    [DllImport("minhook")]
    public static extern int MH_CreateHook(void* pTarget, void* myMethod, void** pOriginal);

    [DllImport("minhook")]
    public static extern void MH_EnableHook(void* pTarget);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    [DllImport("psapi.dll", SetLastError = true)]
    public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, uint cb);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X, Y; }

    [StructLayout(LayoutKind.Sequential)]
    public struct MODULEINFO
    {
        public IntPtr lpBaseOfDll;
        public uint SizeOfImage;
        public IntPtr EntryPoint;
    }

    public static int get_module_size(IntPtr module)
    {
        MODULEINFO info;
        if (GetModuleInformation(System.Diagnostics.Process.GetCurrentProcess().Handle, module, out info, (uint)Marshal.SizeOf<MODULEINFO>()))
            return (int)info.SizeOfImage;
        return 0;
    }
}