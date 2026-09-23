using System.Diagnostics;

namespace Zodiak;

public sealed class watermark : module
{
    public override string DESCRIPTION => lang_manager.get("watermark.module.desc");

    private static watermark? instance;

    private static readonly Stopwatch clock = Stopwatch.StartNew();
    private static int last_good_ping;
    private static double last_time;
    private static float fps = 60f;

    public watermark() : base(category.Visual, "Watermark", "")
    {
        instance = this;
        ENABLED = true;
    }

    private static void update_fps()
    {
        double now = clock.Elapsed.TotalSeconds;
        double delta = now - last_time;
        last_time = now;

        if (delta > 0 && delta < 1.0)
            fps += ((float)(1.0 / delta) - fps) * 0.08f;
    }

    private static unsafe int get_ping()
    {
        if (context.NETWORK_PEER == 0) return 0;

        int raw = *(int*)(context.NETWORK_PEER + OFFSETS.FIELD.RAKNETNETWORKPEER_LAST_PING);

        if (raw < 0 || raw > 2000)
            return last_good_ping;

        last_good_ping = raw;
        return raw;
    }

    public static unsafe void render()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return;

        var singleton = (screen_renderer_singleton_sig)
            (base_address + OFFSETS.FUNC.SCREENRENDERER_SINGLETON);
        var fill = (screen_renderer_fill_sig)
            (base_address + OFFSETS.FUNC.SCREENRENDERER_FILL);

        nint renderer = singleton();
        if (renderer == 0) return;

        float* colour = stackalloc float[4];

        colour[0] = 0.0f; colour[1] = 0.0f; colour[2] = 0.0f; colour[3] = 0.7f;
        fill(renderer, 5f, 5f, 145f, 20f, (nint)colour);

        colour[0] = 0.42f; colour[1] = 0.55f; colour[2] = 1.00f; colour[3] = 1.00f;
        fill(renderer, 5f, 5f, 6f, 20f, (nint)colour);

        int ping = get_ping();
        string text = $"Zodiak | {(int)fps,4} fps | {ping,3} ms";

        font_draw_cached.text(text, 14f, 9f, 1f, 1f, 1f, 1f);
    }

    public static void tick()
    {
        if (instance == null || !instance.ENABLED) return;
        update_fps();
        render();
    }
}