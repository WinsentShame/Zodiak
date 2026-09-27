using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_camera_render_entities : hook_group
{
    private static render_entities_sig original;

    protected override string NAME => "level_renderer_camera_render_entities";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERENTITIES;
    protected override void store_original(nint ptr) => original = (render_entities_sig)ptr;

    protected override nint detour_ptr()
    {
        render_entities_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint self, float partial)
    {
        if (original == null) return 0;

        wallhack_state.SAVED_PARTIAL = partial;

        bool prev = wallhack_state.IN_ENTITY_PASS;
        if (wallhack_state.ACTIVE) wallhack_state.IN_ENTITY_PASS = true;

        try { return original(self, partial); }
        finally { wallhack_state.IN_ENTITY_PASS = prev; }
    }
}