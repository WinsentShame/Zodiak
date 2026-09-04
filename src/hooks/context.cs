namespace Zodiak;

public static class context
{
    public static IntPtr client_instance = IntPtr.Zero;
    public static IntPtr local_player = IntPtr.Zero;
    public static IntPtr game_mode = IntPtr.Zero;
    public static IntPtr level = IntPtr.Zero;
    public static IntPtr screen_context = IntPtr.Zero;
    public static bool in_world = false;

    public static void reset() => in_world = false;
}