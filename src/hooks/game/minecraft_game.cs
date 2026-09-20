using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe update_graphics_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;

namespace Zodiak;

public static unsafe class minecraft_game
{
    private static update_graphics_sig ORIGINAL;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        nint address = base_address + OFFSETS.FUNC.MINECRAFTGAME_UPDATEGRAPHICS;

        update_graphics_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (update_graphics_sig)original_ptr;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint a2)
    {
        ORIGINAL(self, a2);

        if (context.MINECRAFT_GAME == 0)
            context.MINECRAFT_GAME = self;

        watermark.tick();
    }
}