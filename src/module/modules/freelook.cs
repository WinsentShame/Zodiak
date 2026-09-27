namespace Zodiak;

public sealed class freelook : module
{
    public override string DESCRIPTION => lang_manager.get("freelook.module.desc");
    public override string USAGE => lang_manager.get("freelook.usage");

    public static bool ACTIVE;
    public static nint LOCAL_PLAYER;

    public static float CAM_PITCH;
    public static float CAM_YAW;
    public static float FROZEN_PITCH;
    public static float FROZEN_YAW;

    private const int PERSPECTIVE_THIRD_BACK = 1;

    private static int saved_perspective;

    public freelook() : base(category.Visual, "FreeLook", "", "", true)
    {
    }

    public override void on_enable()
    {
        nint player = local_player.get();
        if (!local_player.is_valid(player))
        {
            chat_response.send("no player");
            return;
        }

        LOCAL_PLAYER = player;

        FROZEN_PITCH = CAM_PITCH = entity_rotation.get_pitch(player);
        FROZEN_YAW = CAM_YAW = entity_rotation.get_yaw(player);

        saved_perspective = game_options.get_perspective();
        game_options.set_perspective_allowed(PERSPECTIVE_THIRD_BACK);

        ACTIVE = true;
        chat_response.send(lang_manager.get("freelook.module.enable"));
    }

    public override void on_disable()
    {
        game_options.set_perspective_allowed(saved_perspective);

        ACTIVE = false;
        chat_response.send(lang_manager.get("freelook.module.disable"));
    }

    public static void tick()
    {
        if (!ACTIVE) return;

        nint player = local_player.get();
        if (player != 0)
            LOCAL_PLAYER = player;
        else
            ACTIVE = false;
    }
}