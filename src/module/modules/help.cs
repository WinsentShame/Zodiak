namespace Zodiak;

public sealed class help : module
{
    public help() : base(category.Misc, "Help", "Список доступных команд.")
    {
    }

    public override void on_command(string[] Args) => show();

    public static void show()
    {
        chat_response.def("§aZodiak§f commands§7:");

        foreach (category Cat in Enum.GetValues<category>())
        {
            bool Header = false;

            foreach (var M in module_manager.all)
            {
                if (M.category != Cat) continue;

                if (!Header)
                {
                    chat_response.line($"§f{Cat}:");
                    Header = true;
                }

                chat_response.line($".{M.name.ToLower()} §7- {M.description}");
            }
        }
    }
}