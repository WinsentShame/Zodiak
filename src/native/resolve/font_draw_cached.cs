namespace Zodiak;

public static unsafe class font_draw_cached
{
    private static font_draw_cached_sig draw_cached;
    private static bool resolved;

    public static bool resolve()
    {
        if (resolved) return true;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0)
        {
            logger.error("font_draw_cached", "no base address");
            return false;
        }

        draw_cached = (font_draw_cached_sig)
            (base_address + OFFSETS.FUNC.FONT_DRAWCACHED);

        resolved = draw_cached != null;

        return resolved;
    }

    public static void text(string message, float x, float y,
                            float r, float g, float b, float a)
    {
        if (!resolved) return;
        if (context.MINECRAFT_GAME == 0) return;

        nint font = *(nint*)(context.MINECRAFT_GAME + OFFSETS.FIELD.MINECRAFTGAME_FONT);
        if (font == 0) return;

        byte* str_buf = stackalloc byte[msvc_string.SIZE];
        msvc_string.write((nint)str_buf, message);

        float* colour = stackalloc float[4];
        colour[0] = r;
        colour[1] = g;
        colour[2] = b;
        colour[3] = a;

        try
        {
            draw_cached(font, (nint)str_buf, x, y, (nint)colour,
                        0, 0, 0, -1, 0);
        }
        finally
        {
            msvc_string.free((nint)str_buf);
        }
    }
}