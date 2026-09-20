namespace Zodiak;

public sealed class help : module
{
    public help() : base(category.Misc, "Help", "Список доступных команд.")
    {
    }

    public override void on_command(string[] args) => show();

    public static void show()
    {
        chat_response.def("§aZodiak§f commands§7:");

        foreach (category cat in Enum.GetValues<category>())
        {
            bool header = false;

            foreach (var m in module_manager.ALL)
            {
                if (m.CATEGORY != cat) continue;

                if (!header)
                {
                    chat_response.line($"§f{cat}:");
                    header = true;
                }

                chat_response.line($".{m.NAME.ToLower()} §7- {m.DESCRIPTION}");
            }
        }
    }
}