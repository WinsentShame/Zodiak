namespace Zodiak;

public sealed class help : module
{
    public override string DESCRIPTION => lang_manager.get("help.module.desc");

    public help() : base(category.Misc, "Help", "")
    { }

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
                    chat_response.send($" §f{cat}:");
                    header = true;
                }

                chat_response.send($"  .{m.NAME.ToLower()} §7- {m.DESCRIPTION}");
            }
        }
    }
}