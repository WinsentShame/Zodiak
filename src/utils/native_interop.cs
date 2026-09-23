using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class native_interop
{
    [DllImport("minhook", EntryPoint = "MH_Initialize", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_initialize();

    [DllImport("minhook", EntryPoint = "MH_Uninitialize", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_uninitialize();

    [DllImport("minhook", EntryPoint = "MH_CreateHook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_create_hook(nint Target, nint Detour, out nint Original);

    [DllImport("minhook", EntryPoint = "MH_EnableHook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_enable_hook(nint Target);

    [DllImport("minhook", EntryPoint = "MH_DisableHook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_disable_hook(nint Target);

    [DllImport("minhook", EntryPoint = "MH_RemoveHook", CallingConvention = CallingConvention.Cdecl)]
    public static extern int mh_remove_hook(nint Target);

    [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern nint get_module_handle_w(string? ModuleName);

    [DllImport("kernel32.dll", EntryPoint = "VirtualProtect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool virtual_protect(nint Addr, nuint Size, uint NewProtect, out uint OldProtect);

    [DllImport("kernel32.dll", EntryPoint = "VirtualQuery", SetLastError = true)]
    public static extern nuint virtual_query(nint Address, out memory_basic_information Buffer, nuint Length);

    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow", SetLastError = true)]
    public static extern nint get_foreground_window();

    [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true)]
    public static extern uint get_window_thread_process_id(nint hwnd, out uint pid);

    [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", SetLastError = true)]
    public static extern uint get_current_thread_id();

    [DllImport("user32.dll", EntryPoint = "AttachThreadInput", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool attach_thread_input(uint id_attach, uint id_attach_to,
                                                   [MarshalAs(UnmanagedType.Bool)] bool attach);

    [DllImport("user32.dll", EntryPoint = "GetAsyncKeyState", SetLastError = true)]
    public static extern short get_async_key_state(int vk);

    [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool enum_windows(delegate* unmanaged[Stdcall]<nint, nint, byte> callback, nint lparam);

    [DllImport("user32.dll", EntryPoint = "EnumChildWindows", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool enum_child_windows(nint parent, delegate* unmanaged[Stdcall]<nint, nint, byte> callback, nint lparam);

    [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int get_class_name_w(nint hwnd, System.Text.StringBuilder buffer, int max_count);

    [DllImport("user32.dll", EntryPoint = "IsWindow", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool is_window(nint hwnd);

    [DllImport("kernel32.dll", EntryPoint = "CreateToolhelp32Snapshot", SetLastError = true)]
    public static extern nint create_toolhelp32_snapshot(uint flags, uint pid);

    [DllImport("kernel32.dll", EntryPoint = "Thread32First", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool thread32_first(nint snapshot, ref THREADENTRY32 entry);

    [DllImport("kernel32.dll", EntryPoint = "Thread32Next", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool thread32_next(nint snapshot, ref THREADENTRY32 entry);

    [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool close_handle(nint handle);

    [StructLayout(LayoutKind.Sequential)]
    public struct THREADENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ThreadID;
        public uint th32OwnerProcessID;
        public int tpBasePri;
        public int tpDeltaPri;
        public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct memory_basic_information
    {
        public nint BaseAddress;
        public nint AllocationBase;
        public uint AllocationProtect;
        public nuint RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }
}