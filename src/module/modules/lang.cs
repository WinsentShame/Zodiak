namespace Zodiak;

public sealed class lang : module
{
    public override string DESCRIPTION => lang_manager.get("lang.module.desc");

    public lang() : base(category.Misc, "Lang", "")
    { }

    public override void on_command(string[] args)
    {
        language previous = lang_manager.CURRENT;

        if (args.Length > 0)
        {
            if (!args.try_language(0, out language target))
            {
                chat_response.send(lang_manager.get("lang.usage"));
                return;
            }

            lang_manager.set(target);
        }
        else
        {
            lang_manager.toggle();
        }

        if (previous == lang_manager.CURRENT)
        {
            chat_response.send(lang_manager.get("lang.already",
                lang_manager.native_name(lang_manager.CURRENT)));
            return;
        }

        string from = lang_manager.native_name(previous);
        string to = lang_manager.native_name(lang_manager.CURRENT);
        chat_response.send(lang_manager.get("lang.changed", from, to));
    }
}