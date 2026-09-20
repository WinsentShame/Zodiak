using unsafe font_draw_cached_sig = delegate* unmanaged[Stdcall]<
    nint, nint, float, float, nint,
    byte, byte, nint, int, byte, void>;


namespace Zodiak;

public static unsafe class draw
{
    private static font_draw_cached_sig DRAW_CACHED;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        DRAW_CACHED = (font_draw_cached_sig)
            (base_address + OFFSETS.FUNC.FONT_DRAWCACHED);

        return true;
    }

    public static unsafe void text(string message, float x, float y,
                                    float r, float g, float b, float a)
    {
        if (DRAW_CACHED == null) return;
        if (context.MINECRAFT_GAME == 0) return;

        nint font = *(nint*)(context.MINECRAFT_GAME + OFFSETS.FIELD.MINECRAFTGAME_FONT);
        if (font == 0) return;

        byte* str_buf = stackalloc byte[0x20];
        msvc_string.write((nint)str_buf, message);

        float* colour = stackalloc float[4];
        colour[0] = r;
        colour[1] = g;
        colour[2] = b;
        colour[3] = a;

        try
        {
            DRAW_CACHED(font, (nint)str_buf, x, y, (nint)colour,
                        0, 0, 0, -1, 0);
        }
        finally
        {
            msvc_string.free((nint)str_buf);
        }
    }
}