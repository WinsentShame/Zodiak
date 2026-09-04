using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class leave_game
{
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    delegate void client_instance_leave_game_delegate(IntPtr self, IntPtr a2);

    static readonly IntPtr base_address = native.GetModuleHandle(null);
    static readonly IntPtr client_leave_addr = base_address + (int)offsets.CLIENT_INSTANCE_LEAVE_GAME;

    static IntPtr original_client_leave_ptr;
    static client_instance_leave_game_delegate original_client_leave = null!;

    public static void install()
    {
        logger.info("leave_game", "installing hook...");
        void* detour = (delegate* unmanaged[Thiscall]<IntPtr, IntPtr, void>)&client_leave_game_hook;
        void* orig = null;
        if (native.MH_CreateHook((void*)client_leave_addr, detour, &orig) == 0)
        {
            original_client_leave_ptr = (IntPtr)orig;
            original_client_leave = Marshal.GetDelegateForFunctionPointer<client_instance_leave_game_delegate>(original_client_leave_ptr);
            native.MH_EnableHook((void*)client_leave_addr);
            logger.info("leave_game", "hook installed");
        }
        else logger.error("leave_game", "failed");
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    static void client_leave_game_hook(IntPtr self, IntPtr a2)
    {
        logger.info("leave_game", $"called self=0x{self:X}");
        original_client_leave(self, a2);
        logger.info("leave_game", "finished");
    }
}