namespace Zodiak;

using System.Diagnostics;

public sealed class watermark : module
{
    public override string USAGE => lang_manager.get("watermark.module.usage");
    public override string DESCRIPTION => lang_manager.get("watermark.module.desc");

    private const float box_w = 150f;
    private const float box_h = 16f;
    private const float accent_w = 2f;
    private const float margin = 6f;
    private const float text_pad_x = 9f;
    private const float text_pad_y = 4f;

    private static readonly Stopwatch clock = Stopwatch.StartNew();
    private static double last_time;
    private static float fps;

    public watermark() : base(category.Visual, "Watermark", "", "", true)
    {
        ENABLED = true;
    }

    public static unsafe void tick()
    {
        module? m = module_manager.find("Watermark");
        if (m == null || !m.ENABLED) return;

        nint player = local_player.get();
        if (!local_player.is_valid(player)) return;

        update_fps();
        render();
    }

    private static void update_fps()
    {
        double now = clock.Elapsed.TotalSeconds;
        double delta = now - last_time;
        last_time = now;

        if (delta < 0.0002) return;
        if (delta > 1.0) return;

        float instant = (float)(1.0 / delta);
        if (instant < 1f || instant > 100000f) return;

        if (fps <= 0f) fps = instant;
        else fps += (instant - fps) * 0.02f;
    }

    private static unsafe int get_ping()
    {
        if (context.NETWORK_PEER == 0) return -1;

        int raw = *(int*)(context.NETWORK_PEER + OFFSETS.FIELD.RAKNET_NETWORK_PEER_LAST_PING);
        if (raw < 0 || raw > 1000) return -1;
        return raw;
    }

    private static unsafe void render()
    {
        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return;

        nint game = context.MINECRAFT_GAME;
        if (game == 0) return;

        if (!memory.is_readable(game + OFFSETS.FIELD.MINECRAFT_GAME_SCREEN_WIDTH, 8)) return;

        int screen_w = *(int*)(game + OFFSETS.FIELD.MINECRAFT_GAME_SCREEN_WIDTH);
        int screen_h = *(int*)(game + OFFSETS.FIELD.MINECRAFT_GAME_SCREEN_HEIGHT);
        if (screen_w <= 0 || screen_h <= 0) return;

        float gui_scale = *(float*)(ba + OFFSETS.FUNC.GUI_DATA_GUI_SCALE);
        if (gui_scale <= 0f) gui_scale = 1f;

        float ui_w = screen_w / gui_scale;
        float ui_h = screen_h / gui_scale;

        float x0 = ui_w - box_w - margin;
        float y0 = ui_h - box_h - margin;
        float x1 = x0 + box_w;
        float y1 = y0 + box_h;

        var singleton = (screen_renderer_singleton_sig)
            (ba + OFFSETS.FUNC.SCREENRENDERER_SINGLETON);
        var fill = (screen_renderer_fill_sig)
            (ba + OFFSETS.FUNC.SCREENRENDERER_FILL);



        nint renderer = singleton();
        if (renderer == 0) return;

        int ping = get_ping();

        int fps_int = 0;
        if (!float.IsNaN(fps) && !float.IsInfinity(fps) && fps > 0f && fps < 10000f)
            fps_int = (int)fps;

        string fps_str = fps_int.ToString().PadLeft(4);
        string ping_str = ping < 0 ? "  --" : ping.ToString().PadLeft(4);
        string text = $"Zodiak | {fps_str} fps | {ping_str} ms";

        float* colour = stackalloc float[4];

        colour[0] = 0.05f; colour[1] = 0.05f; colour[2] = 0.05f; colour[3] = 0.80f;
        fill(renderer, x0, y0, x1, y1, (nint)colour);

        colour[0] = 0.42f; colour[1] = 0.55f; colour[2] = 1.00f; colour[3] = 1.00f;
        fill(renderer, x0, y0, x0 + accent_w, y1, (nint)colour);

        float text_x = x0 + text_pad_x;
        float text_y = y0 + text_pad_y;

        font_draw_cached.text(text, text_x + 1f, text_y + 1f, 0f, 0f, 0f, 1f);
        font_draw_cached.text(text, text_x, text_y, 1f, 1f, 1f, 1f);
    }
}