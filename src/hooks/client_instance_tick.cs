using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class client_instance_on_tick
{
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    delegate void client_instance_on_tick_delegate(IntPtr self, int a2, int a3);

    static readonly IntPtr base_address = native.GetModuleHandle(null);
    static readonly IntPtr on_tick_addr = base_address + (int)offsets.CLIENT_INSTANCE_ON_TICK;

    static IntPtr original_ptr;
    static client_instance_on_tick_delegate original = null!;

    public static void install()
    {
        void* detour = (delegate* unmanaged[Thiscall]<IntPtr, int, int, void>)&on_tick_hook;
        void* orig = null;

        if (native.MH_CreateHook((void*)on_tick_addr, detour, &orig) == 0)
        {
            original_ptr = (IntPtr)orig;
            original = Marshal.GetDelegateForFunctionPointer<client_instance_on_tick_delegate>(original_ptr);
            native.MH_EnableHook((void*)on_tick_addr);
            logger.info("client_instance_on_tick", "hook installed");
        }
        else
        {
            logger.error("client_instance_on_tick", "failed to install hook");
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    static void on_tick_hook(IntPtr self, int a2, int a3)
    {
        context.client_instance = self;
        original(self, a2, a3);
    }
}