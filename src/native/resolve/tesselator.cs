namespace Zodiak;

public static unsafe class tessellator
{
    private static nint instance;
    private static bool resolved;

    private static tessellator_begin_sig begin_fn;
    private static tessellator_vertex_sig vertex_fn;
    private static tessellator_vertexuv_sig vertexuv_fn;
    private static tessellator_colour_sig colour_fn;
    private static tessellator_end_sig end_fn;
    private static tessellator_draw_sig draw_fn;
    private static tessellator_draw2_sig draw2_fn;

    public static bool resolve()
    {
        if (resolved) return instance != 0;

        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return false;

        instance = ba + OFFSETS.FUNC.G_TESSELLATOR;

        begin_fn = (tessellator_begin_sig)(ba + OFFSETS.FUNC.TESSELLATOR_BEGIN);
        vertex_fn = (tessellator_vertex_sig)(ba + OFFSETS.FUNC.TESSELLATOR_VERTEX);
        vertexuv_fn = (tessellator_vertexuv_sig)(ba + OFFSETS.FUNC.TESSELLATOR_VERTEXUV);
        colour_fn = (tessellator_colour_sig)(ba + OFFSETS.FUNC.TESSELLATOR_COLOUR);
        end_fn = (tessellator_end_sig)(ba + OFFSETS.FUNC.TESSELLATOR_END);
        draw_fn = (tessellator_draw_sig)(ba + OFFSETS.FUNC.TESSELLATOR_DRAW);
        draw2_fn = (tessellator_draw2_sig)(ba + OFFSETS.FUNC.TESSELLATOR_DRAW2);

        resolved = instance != 0;
        return resolved;
    }

    public static bool valid()
        => instance != 0 && memory.is_readable(instance, 0x140);

    public static nint ptr() => instance;

    public static bool needs_flush()
    {
        if (instance == 0) return false;
        byte* tess = (byte*)instance;
        return tess[0x170] != 0 || tess[0x125] != 0 || *(int*)(tess + 0x168) != 0;
    }

    public static void reset_state()
    {
        if (instance == 0) return;
        byte* tess = (byte*)instance;
        tess[0x170] = 0;
        tess[0x125] = 0;
    }

    public static void begin(byte mode, int hint)
        => begin_fn(instance, mode, hint);

    public static void vertex(float x, float y, float z)
        => vertex_fn(instance, x, y, z);

    public static void vertex_uv(float x, float y, float z, float u, float v)
        => vertexuv_fn(instance, x, y, z, u, v);

    public static void colour(byte r, byte g, byte b, byte a)
        => colour_fn(instance, r, g, b, a);

    public static void draw(nint ctx, long flags)
        => draw_fn(instance, ctx, flags);

    public static void draw2(nint material, nint texture)
        => draw2_fn(instance, material, texture);

    public static void end_flush()
    {
        if (instance == 0) return;

        byte* buf = stackalloc byte[0x200];
        for (int i = 0; i < 0x200; i++) buf[i] = 0;

        end_fn(instance, (nint)buf, 0, 0);
    }
}