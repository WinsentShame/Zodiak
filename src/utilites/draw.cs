namespace Zodiak;
public static unsafe class draw
{
    static IntPtr game_window;
    public static void set_window(IntPtr hWnd) => game_window = hWnd;
}