namespace Zodiak;

public sealed class fog_color : module
{
    public fog_color() : base(category.Visual, "FogColor", "Пользовательский цвет тумана.")
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_setup_fog.active = true;
        chat_response.success($"Fog color включен ({level_renderer_camera_setup_fog.r:F2}, {level_renderer_camera_setup_fog.g:F2}, {level_renderer_camera_setup_fog.b:F2}).");
    }

    public override void on_disable()
    {
        level_renderer_camera_setup_fog.active = false;
        chat_response.success("Fog color выключен.");
    }

    public override void on_command(string[] args)
    {
        if (args.Length == 0)
        {
            enabled = !enabled;
            return;
        }

        if (args.try_color(out float r, out float g, out float b))
        {
            level_renderer_camera_setup_fog.r = Math.Clamp(r, 0f, 1f);
            level_renderer_camera_setup_fog.g = Math.Clamp(g, 0f, 1f);
            level_renderer_camera_setup_fog.b = Math.Clamp(b, 0f, 1f);
            level_renderer_camera_setup_fog.active = true;
            enabled = true;
            chat_response.info($"Fog color ({level_renderer_camera_setup_fog.r:F2}, {level_renderer_camera_setup_fog.g:F2}, {level_renderer_camera_setup_fog.b:F2})");
            return;
        }

        chat_response.error("Используй: .fogcolor <r> <g> <b>");
    }
}