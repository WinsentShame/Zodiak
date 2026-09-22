using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class minecraft_game_update_graphics : hook_group
{
    private static update_graphics_sig original;

    protected override string NAME => "minecraft_game_update_graphics";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.MINECRAFTGAME_UPDATEGRAPHICS;
    protected override void store_original(nint ptr) => original = (update_graphics_sig)ptr;

    protected override nint detour_ptr()
    {
        update_graphics_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint a2)
    {
        original(self, a2);

        if (self != 0 && context.MINECRAFT_GAME != self)
            context.MINECRAFT_GAME = self;

        watermark.tick();
    }
}