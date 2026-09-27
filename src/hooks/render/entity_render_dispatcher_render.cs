using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class entity_render_dispatcher_render : hook_group
{
    private static entity_render_disp_sig original;

    protected override string NAME => "entity_render_dispatcher_render";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.ENTITYRENDERDISPATCHER_RENDER;
    protected override void store_original(nint ptr) => original = (entity_render_disp_sig)ptr;

    protected override nint detour_ptr()
    {
        entity_render_disp_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint self, nint entity, nint pos, nint a4, int a5)
    {
        if (original == null) return 0;

        nint result = original(self, entity, pos, a4, a5);

        if (hitboxes.ACTIVE && entity != 0 && entity_type.is_player(entity))
        {
            nint player = local_player.get();
            if (entity != player)
                entity_cache.push(entity);
        }

        return result;
    }
}