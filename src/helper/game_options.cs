namespace Zodiak;

public static unsafe class game_options
{
    public static bool ALLOW_INTERNAL;

    public static nint get()
    {
        if (context.MINECRAFT_GAME == 0) return 0;

        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return 0;

        var fn = (get_options_sig)(ba + OFFSETS.FUNC.MINECRAFT_GAME_GET_OPTIONS);
        return fn(context.MINECRAFT_GAME);
    }

    public static int get_perspective()
    {
        nint options = get();
        if (options == 0) return 0;

        nint ba = native_interop.get_module_handle_w(null);
        var fn = (get_perspective_sig)(ba + OFFSETS.FUNC.OPTIONS_GET_PLAYER_VIEW_PERSPECTIVE);
        return fn(options);
    }

    public static void set_perspective_allowed(int value)
    {
        nint options = get();
        if (options == 0) return;

        nint ba = native_interop.get_module_handle_w(null);
        var fn = (set_perspective_sig)(ba + OFFSETS.FUNC.OPTIONS_SET_PLAYER_VIEW_PERSPECTIVE);

        bool prev = ALLOW_INTERNAL;
        ALLOW_INTERNAL = true;
        try { fn(options, value); }
        finally { ALLOW_INTERNAL = prev; }
    }
}