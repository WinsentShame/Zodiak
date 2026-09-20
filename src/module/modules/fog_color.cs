namespace Zodiak;

public sealed class fog_color : module
{
    public fog_color() : base(category.Visual, "FogColor", "Пользовательский цвет тумана.")
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_setup_fog.ACTIVE = true;
        chat_response.success($"Fog color включен ({level_renderer_camera_setup_fog.R:F2}, {level_renderer_camera_setup_fog.G:F2}, {level_renderer_camera_setup_fog.B:F2}).");
    }

    public override void on_disable()
    {
        level_renderer_camera_setup_fog.ACTIVE = false;
        chat_response.success("Fog color выключен.");
    }

    public override void on_command(string[] args)
    {
        if (args.Length == 0)
        {
            ENABLED = !ENABLED;
            return;
        }

        if (args.try_color(out float r, out float g, out float b))
        {
            level_renderer_camera_setup_fog.R = Math.Clamp(r, 0f, 1f);
            level_renderer_camera_setup_fog.G = Math.Clamp(g, 0f, 1f);
            level_renderer_camera_setup_fog.B = Math.Clamp(b, 0f, 1f);
            level_renderer_camera_setup_fog.ACTIVE = true;
            ENABLED = true;
            chat_response.info($"Fog color ({level_renderer_camera_setup_fog.R:F2}, {level_renderer_camera_setup_fog.G:F2}, {level_renderer_camera_setup_fog.B:F2})");
            return;
        }

        chat_response.error("Используй: .fogcolor <r> <g> <b>");
    }
}