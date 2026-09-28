using System.Runtime.InteropServices;

namespace Zodiak;

public static class program
{
    [UnmanagedCallersOnly(EntryPoint = "DllProcessAttach")]
    public static void on_dll_process_attach(nint h_module)
    {
        logger.init();
        int st = native_interop.mh_initialize();
        if (st != 0) return;

        new minecraft_game_update_graphics().install();
        new client_instance_on_tick().install();
        new client_intance_leave_game().install();
        new network_peer_update().install();
        new minecraft_screen_model_send_chat_message().install();
        new level_renderer_camera_setup_fog().install();
        new level_renderer_camera_render_sky().install();
        new entity_turn().install();
        new level_renderer_player_move_camera_to_player().install();
        new options_set_player_view_perspective().install();
        new level_renderer_camera_render_level().install();
        new mce_rendercontext_create_depth_state().install();
        new mce_rendercontext_apply_depth_state().install();
        new in_game_play_screen_render().install();
        new level_renderer_camera_render_entities().install();
        new player_renderer_render().install();
        new in_game_play_screen_render().install();

        tessellator.resolve();
        chat_response.resolve();
        font_draw_cached.resolve();
        minecraft_game_get_screen.resolve();


        module_manager.register(new help());
        module_manager.register(new fog_color());
        module_manager.register(new skybox());
        module_manager.register(new watermark());
        module_manager.register(new wallhack());
        module_manager.register(new freelook());
        module_manager.register(new esp());
        module_manager.register(new binds());
        module_manager.register(new lang());
    }
}