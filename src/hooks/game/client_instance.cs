using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe on_tick_sig = delegate* unmanaged[Stdcall]<nint, int, int, void>;

namespace Zodiak;

public static unsafe class client_instance
{
    private static on_tick_sig ORIGINAL;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        nint address = base_address + OFFSETS.FUNC.CLIENTINSTANCE_ONTICK;
        on_tick_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (on_tick_sig)original_ptr;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, int a2, int a3)
    {
        if (context.CLIENT_INSTANCE == 0)
            context.CLIENT_INSTANCE = self;

        if (ORIGINAL != null)
            ORIGINAL(self, a2, a3);
    }
}