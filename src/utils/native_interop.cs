using System.Runtime.InteropServices;

namespace Zodiak;

public static class native_interop
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