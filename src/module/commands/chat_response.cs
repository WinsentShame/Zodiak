namespace Zodiak;

public static unsafe class chat_response
{
    private const int MSVC_STRING_SIZE = 0x20; // какашка

    private static delegate* unmanaged[Stdcall]<nint, nint, void> display;

    public static void install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return;

        display = (delegate* unmanaged[Stdcall]<nint, nint, void>)
            (base_address + OFFSETS.FUNC.GUIDATA_DISPLAYCLIENTMESSAGE);
    }

    public static void send(string message)
    {
        if (display == null) return;

        nint game = minecraft_game.pointer;
        if (game == 0) return;

        nint gui_data = *(nint*)(game + OFFSETS.FIELD.MINECRAFTGAME_GUIDATA);
        if (gui_data == 0) return;

        byte* buffer = stackalloc byte[MSVC_STRING_SIZE];
        msvc_string.write((nint)buffer, message);
        try
        {
            display(gui_data, (nint)buffer);
        }
        finally
        {
            msvc_string.free((nint)buffer);
        }
    }

    public static void info(string msg) => send($"§f[ §aZodiak§f | §bINFO §f] §7{msg} ");
    public static void success(string msg) => send($"§f[ §aZodiak§f | §2SUCCESS §f] §7{msg} ");
    public static void error(string msg) => send($"§f[ §aZodiak§f | §4ERROR §f] §7{msg} ");
    public static void line(string msg) => send($"  §7{msg} ");
    public static void def(string msg) => send(msg);
}