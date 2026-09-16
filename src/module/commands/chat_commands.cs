namespace Zodiak;

public static class chat_commands
{
    public static void dispatch(string Raw)
    {
        string[] Parts = Raw.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (Parts.Length == 0) { help.show(); return; }

        module? M = module_manager.find(Parts[0]);
        if (M == null) { chat_response.error($"Енто что ваще бля: {Parts[0]}"); return; }

        M.on_command(Parts.Skip(1).ToArray());
    }
}