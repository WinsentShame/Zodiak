using System.Diagnostics;

namespace Zodiak;

public sealed class watermark : module
{
    public override string USAGE => lang_manager.get("watermark.module.usage");
    public override string DESCRIPTION => lang_manager.get("watermark.module.desc");

    private const float pos_x = 4f;
    private const float pos_y = 4f;
    private const float accent_w = 2f;
    private const float box_w = 165f;
    private const float box_h = 16f;
    private const float text_pad_x = 8f;
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

        update_fps();
        render();
    }

    private static void update_fps()
    {
        double now = clock.Elapsed.TotalSeconds;
        double delta = now - last_time;
        last_time = now;

        if (delta <= 0.0 || delta > 1.0) return;

        float instant = (float)(1.0 / delta);

        if (fps <= 0f) fps = instant;
        else fps += (instant - fps) * 0.1f;
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
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return;

        var singleton = (screen_renderer_singleton_sig)
            (base_address + OFFSETS.FUNC.SCREENRENDERER_SINGLETON);
        var fill = (screen_renderer_fill_sig)
            (base_address + OFFSETS.FUNC.SCREENRENDERER_FILL);

        nint renderer = singleton();
        if (renderer == 0) return;

        int ping = get_ping();
        string ping_text = ping < 0 ? "  -- ms" : $"{ping,4} ms";
        string text = $"Zodiak | {(int)fps,4} fps | {ping_text}";

        float* colour = stackalloc float[4];

        colour[0] = 0.05f; colour[1] = 0.05f; colour[2] = 0.05f; colour[3] = 0.75f;
        fill(renderer, pos_x, pos_y, pos_x + box_w, pos_y + box_h, (nint)colour);

        colour[0] = 0.42f; colour[1] = 0.55f; colour[2] = 1.00f; colour[3] = 1.00f;
        fill(renderer, pos_x, pos_y, pos_x + accent_w, pos_y + box_h, (nint)colour);

        font_draw_cached.text(text, pos_x + text_pad_x, pos_y + text_pad_y, 1f, 1f, 1f, 1f);
    }
}