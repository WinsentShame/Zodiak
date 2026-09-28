using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_camera_render_level : hook_group
{
    private static render_level_sig original;
    private static render_entities_sig original_render_entities;

    protected override string NAME => "level_renderer_camera_render_level";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVEL_RENDERER_CAMERA_RENDER_LEVEL;
    protected override void store_original(nint ptr) => original = (render_level_sig)ptr;

    protected override nint detour_ptr()
    {
        render_level_sig fn = &detour;
        return (nint)fn;
    }

    protected override void on_installed()
    {
        nint ba = native_interop.get_module_handle_w(null);
        original_render_entities = (render_entities_sig)
            (ba + OFFSETS.FUNC.LEVEL_RENDERER_CAMERA_RENDER_ENTITIES);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint a2, nint a3, nint a4)
    {
        if (original == null) return;

        original(self, a2, a3, a4);

        if (wallhack_state.ACTIVE && original_render_entities != null)
        {
            wallhack_state.SECOND_PASS = true;
            wallhack_state.IN_ENTITY_PASS = true;
            try
            {
                original_render_entities(self, wallhack_state.SAVED_PARTIAL);
            }
            finally
            {
                wallhack_state.IN_ENTITY_PASS = false;
                wallhack_state.SECOND_PASS = false;
            }
        }
    }
}