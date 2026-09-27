using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class mce_rendercontext_create_depth_state : hook_group
{
    protected override string NAME => "mce_rendercontext_create_depth_state";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.MCE_RENDERCONTEXT_CREATEDEPTHSTATE;
    protected override void store_original(nint ptr) => wallhack_state.ORIGINAL_CREATE_DEPTH = (create_depth_state_sig)ptr;

    protected override nint detour_ptr()
    {
        create_depth_state_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint state, nint ctx, nint packet)
    {
        if (wallhack_state.ORIGINAL_CREATE_DEPTH == null) return 0;
        return wallhack_state.ORIGINAL_CREATE_DEPTH(state, ctx, packet);
    }
}