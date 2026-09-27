using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class mce_rendercontext_apply_depth_state : hook_group
{
    private static apply_depth_state_sig original;

    protected override string NAME => "mce_rendercontext_apply_depth_state";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.MCE_RENDERCONTEXT_APPLYDEPTHSTATE;
    protected override void store_original(nint ptr) => original = (apply_depth_state_sig)ptr;

    protected override nint detour_ptr()
    {
        apply_depth_state_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint state, nint ctx, nint force)
    {
        if (original == null) return 0;

        if (!wallhack_state.ACTIVE
            || !wallhack_state.IN_ENTITY_PASS
            || state == 0
            || wallhack_state.CREATING_NO_DEPTH)
            return original(state, ctx, force);

        if (ctx != 0 && wallhack_state.CACHED_CTX == 0)
            wallhack_state.CACHED_CTX = ctx;

        if (!wallhack_state.NO_DEPTH_READY)
            wallhack_state.ensure_no_depth_state();

        if (!wallhack_state.NO_DEPTH_READY)
            return original(state, ctx, force);

        int saved_depth = *(int*)state;
        nint saved_obj = *(nint*)(state + 24);

        *(int*)state = 0;
        *(nint*)(state + 24) = wallhack_state.NO_DEPTH_STATE;

        try
        {
            return original(state, ctx, force);
        }
        finally
        {
            *(int*)state = saved_depth;
            *(nint*)(state + 24) = saved_obj;
        }
    }
}