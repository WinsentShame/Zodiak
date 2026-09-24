namespace Zodiak;

public sealed class skybox : module
{
    public override string USAGE => lang_manager.get("skybox.usage");
    public override string DESCRIPTION => lang_manager.get("skybox.module.desc");

    public skybox() : base(category.Visual, "Skybox", "", "", true)
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_render_sky.ACTIVE = true;
        level_renderer_camera_render_sky.set_hide(true);
        level_renderer_camera_render_sky.set_time_of_day(true);

        if (sky_cubemap.load())
            chat_response.send(lang_manager.get("skybox.module.enable"));
        else
            chat_response.send(lang_manager.get("skybox.module.error"));
    }

    public override void on_disable()
    {
        level_renderer_camera_render_sky.ACTIVE = false;
        level_renderer_camera_render_sky.set_hide(false);
        level_renderer_camera_render_sky.set_time_of_day(false);
        sky_cubemap.unload();
        chat_response.send(lang_manager.get("skybox.module.disable"));
    }
}