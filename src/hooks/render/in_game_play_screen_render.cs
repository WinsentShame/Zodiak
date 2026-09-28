using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class in_game_play_screen_render : hook_group
{
    private static in_game_play_render_sig original;

    protected override string NAME => "in_game_play_screen_render";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.IN_GAME_PLAY_SCREEN_RENDER;
    protected override void store_original(nint ptr) => original = (in_game_play_render_sig)ptr;

    protected override nint detour_ptr()
    {
        in_game_play_render_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint screen_context)
    {
        if (original == null) return;

        original(self, screen_context);

        if (hitboxes.ACTIVE)
            hitbox_renderer.draw(wallhack_state.SAVED_PARTIAL);
    }
}