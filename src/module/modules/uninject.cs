namespace Zodiak;

public sealed class uninject : module
{
    public override string DESCRIPTION => lang_manager.get("uninject.module.desc");
    public override string USAGE => lang_manager.get("uninject.usage");

    private static bool running;

    public uninject() : base(category.Misc, "Uninject", "", "", false)
    {
    }

    public override void on_command(string[] args)
    {
        if (running) return;
        running = true;

        chat_response.send(lang_manager.get("uninject.module.bye"));

        module_manager.disable_all();
        hook_registry.uninstall_all();
        level_renderer_camera_render_sky.restore_patches();

        chat_response.send(lang_manager.get("uninject.module.done"));
    }
}