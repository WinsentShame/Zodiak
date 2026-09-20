using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe network_peer_update_sig = delegate* unmanaged[Stdcall]<
    nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>;

namespace Zodiak;

public static unsafe class network_peer
{
    private static network_peer_update_sig ORIGINAL;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        nint address = base_address + OFFSETS.FUNC.RAKNETNETWORKPEER_UPDATE;
        network_peer_update_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (network_peer_update_sig)original_ptr;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self,
                                nint a2, nint a3, nint a4, nint a5, nint a6, nint a7,
                                nint a8, nint a9, nint a10, nint a11, nint a12, nint a13, nint a14)
    {
        context.NETWORK_PEER = self;

        ORIGINAL(self, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
    }
}