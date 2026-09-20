using System.Runtime.InteropServices;

namespace Zodiak;

public static class program
{
    [UnmanagedCallersOnly(EntryPoint = "DllProcessAttach")]
    public static void on_dll_process_attach(nint HModule)
    {
        logger.init();
        int st = native_interop.mh_initialize();
        if (st != 0) return;

        minecraft_game.install();
        client_instance.install();
        chat_response.install();
        draw.install();    
        network_peer.install();
        chat_hook.install();
        level_renderer_camera_setup_fog.install();
        level_renderer_camera_render_sky.install();

        module_manager.register(new help());
        module_manager.register(new fog_color());
        module_manager.register(new skybox());
        module_manager.register(new watermark());   
    }
}