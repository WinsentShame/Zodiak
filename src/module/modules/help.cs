namespace Zodiak;

public sealed class help : module
{
    public override string USAGE => lang_manager.get("help.module.usage");
    public override string DESCRIPTION => lang_manager.get("help.module.desc");

    public help() : base(category.Misc, "Help", "", "", false)
    {
    }

    public override void on_command(string[] args) => show();

    public static void show()
    {
        chat_response.send(lang_manager.get("help.module.zodiak.commands"));

        foreach (category cat in Enum.GetValues<category>())
        {
            bool header = false;

            foreach (var m in module_manager.ALL)
            {
                if (m.CATEGORY != cat) continue;

                if (!header)
                {
                    chat_response.send($"§f{cat}:");
                    header = true;
                }

                string label = lang_manager.get("help.module.usage.label");
                string line = $"  .{m.NAME.ToLower()} - {m.DESCRIPTION} " +
                               $"{label} {m.USAGE}";

                chat_response.send(line);
            }
        }
    }
}