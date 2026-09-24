namespace Zodiak;

public sealed class wallhack : module
{
    public override string DESCRIPTION => lang_manager.get("wallhack.module.desc");
    public override string USAGE => lang_manager.get("wallhack.usage");

    public wallhack() : base(category.Visual, "Wallhack", "", "", true)
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_render_entities.ACTIVE = true;
        chat_response.send(lang_manager.get("wallhack.module.enable"));
    }

    public override void on_disable()
    {
        level_renderer_camera_render_entities.ACTIVE = false;
        chat_response.send(lang_manager.get("wallhack.module.disable"));
    }
}