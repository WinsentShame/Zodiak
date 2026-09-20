namespace Zodiak;

public sealed class skybox : module
{
    public skybox() : base(category.Visual, "Skybox", "Кубическая карта неба из ресурс пака.")
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_render_sky.ACTIVE = true;
        level_renderer_camera_render_sky.set_hide(true);
        level_renderer_camera_render_sky.set_time_of_day(true);

        if (sky_cubemap.load())
            chat_response.success("Skybox включен.");
        else
            chat_response.error($"Skybox: {sky_cubemap.STATUS}");
    }

    public override void on_disable()
    {
        level_renderer_camera_render_sky.ACTIVE = false;
        level_renderer_camera_render_sky.set_hide(false);
        level_renderer_camera_render_sky.set_time_of_day(false);
        sky_cubemap.unload();
        chat_response.info("Skybox выключен.");
    }
}