using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class player_renderer_render : hook_group
{
    private static player_renderer_render_sig original;

    protected override string NAME => "player_renderer_render";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.PLAYERRENDERER_RENDER;
    protected override void store_original(nint ptr) => original = (player_renderer_render_sig)ptr;

    protected override nint detour_ptr()
    {
        player_renderer_render_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint self, nint entity, nint pos, nint a4, float partial)
    {
        if (original == null) return 0;

        nint result = original(self, entity, pos, a4, partial);

        if (hitboxes.ACTIVE && entity != 0 && entity_check.is_real_player(entity))
        {
            nint local = local_player.get();
            if (entity != local)
                entity_cache.push(entity);
        }

        return result;
    }
}