namespace Zodiak;

public static class chat_commands
{
    public static void dispatch(string raw)
    {
        string[] parts = raw.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0) { help.show(); return; }

        module? m = module_manager.find(parts[0]);
        if (m == null) { chat_response.error($"Енто что ваще бля: {parts[0]}"); return; }

        m.on_command(parts.Skip(1).ToArray());
    }
}