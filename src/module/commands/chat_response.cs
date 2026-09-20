namespace Zodiak;

public static unsafe class chat_response
{
    private const int msvc_string_size = 0x20;

    private static delegate* unmanaged[Stdcall]<nint, nint, void> DISPLAY;

    public static void install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return;

        DISPLAY = (delegate* unmanaged[Stdcall]<nint, nint, void>)
            (base_address + OFFSETS.FUNC.GUIDATA_DISPLAYCLIENTMESSAGE);
    }

    public static void send(string message)
    {
        if (DISPLAY == null) return;

        nint game = context.MINECRAFT_GAME;
        if (game == 0) return;

        nint gui_data = *(nint*)(game + OFFSETS.FIELD.MINECRAFTGAME_GUIDATA);
        if (gui_data == 0) return;

        byte* buffer = stackalloc byte[msvc_string_size];
        msvc_string.write((nint)buffer, message);
        try
        {
            DISPLAY(gui_data, (nint)buffer);
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