namespace Zodiak;

public static unsafe class chat_response
{
    private static display_client_message_sig display;
    private static bool resolved;

    public static bool resolve()
    {
        if (resolved) return true;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0)
            return false; 


        display = (display_client_message_sig)
            (base_address + OFFSETS.FUNC.GUIDATA_DISPLAYCLIENTMESSAGE);

        resolved = display != null;
        return resolved;
    }

    public static void send(string message)
    {
        if (!resolved) return;

        nint game = context.MINECRAFT_GAME;
        if (game == 0) return;

        nint gui_data = *(nint*)(game + OFFSETS.FIELD.MINECRAFTGAME_GUIDATA);
        if (gui_data == 0) return;

        byte* buffer = stackalloc byte[msvc_string.SIZE];
        msvc_string.write((nint)buffer, message);

        try { display(gui_data, (nint)buffer); }
        finally { msvc_string.free((nint)buffer); }
    }
}