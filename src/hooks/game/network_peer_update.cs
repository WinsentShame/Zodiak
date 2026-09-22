using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class network_peer_update : hook_group
{
    private static network_peer_update_sig original;

    protected override string NAME => "network_peer_update";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.RAKNETNETWORKPEER_UPDATE;
    protected override void store_original(nint ptr) => original = (network_peer_update_sig)ptr;

    protected override nint detour_ptr()
    {
        network_peer_update_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self,
                                nint a2, nint a3, nint a4, nint a5, nint a6, nint a7,
                                nint a8, nint a9, nint a10, nint a11, nint a12, nint a13, nint a14)
    {
        if (self != 0 && context.NETWORK_PEER != self)
            context.NETWORK_PEER = self;

        original(self, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
    }
}