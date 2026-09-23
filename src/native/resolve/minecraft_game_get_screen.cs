namespace Zodiak;

public static unsafe class minecraft_game_get_screen
{
    private static get_screen_name_sig get_name;

    public static bool resolve()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        get_name = (get_screen_name_sig)
            (base_address + OFFSETS.FUNC.MINECRAFTGAME_GETSCREENNAME);

        return get_name != null;
    }

    public static string current_name()
    {
        if (get_name == null || context.MINECRAFT_GAME == 0) return "";

        byte* buf = stackalloc byte[msvc_string.SIZE];
        for (int i = 0; i < msvc_string.SIZE; i++) buf[i] = 0;

        get_name(context.MINECRAFT_GAME, (nint)buf);

        return msvc_string.read((nint)buf) ?? "";
    }

    public static bool is_hud() => current_name() == "hud_screen";
}