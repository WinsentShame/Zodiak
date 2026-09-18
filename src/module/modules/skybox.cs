namespace Zodiak;

public sealed class skybox : module
{
    public skybox() : base(category.Visual, "Skybox", "Кубическая карта неба из ресурс пака.")
    {
    }

    public override void on_enable()
    {
        level_renderer_camera_render_sky.active = true;
        level_renderer_camera_render_sky.set_hide(true);   

        if (sky_cubemap.load())
            chat_response.success("Skybox включен.");
        else
            chat_response.error($"Skybox: {sky_cubemap.status}");
    }

    public override void on_disable()
    {
        level_renderer_camera_render_sky.active = false;
        level_renderer_camera_render_sky.set_hide(false); 
        sky_cubemap.unload();
        chat_response.info("Skybox выключен.");
    }
}