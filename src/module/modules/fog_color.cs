namespace Zodiak;

public sealed class fog_color : module
{
    public override string DESCRIPTION => lang_manager.get("fogcolor.module.desc");

    public fog_color() : base(category.Visual, "FogColor", "")
    { }

    public override void on_enable()
    {
        level_renderer_camera_setup_fog.ACTIVE = true;
        chat_response.send(lang_manager.get("fogcolor.module.enable",
            level_renderer_camera_setup_fog.R,
            level_renderer_camera_setup_fog.G,
            level_renderer_camera_setup_fog.B));
    }

    public override void on_disable()
    {
        level_renderer_camera_setup_fog.ACTIVE = false;
        chat_response.send(lang_manager.get("fogcolor.module.disable"));
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
            chat_response.send(lang_manager.get("fogcolor.module.color",
                level_renderer_camera_setup_fog.R,
                level_renderer_camera_setup_fog.G,
                level_renderer_camera_setup_fog.B));
            return;
        }

        chat_response.send(lang_manager.get("fogcolor.module.usage"));
    }
}