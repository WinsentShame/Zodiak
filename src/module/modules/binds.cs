namespace Zodiak;

public sealed class binds : module
{
    public override string USAGE => lang_manager.get("binds.module.usage");
    public override string DESCRIPTION => lang_manager.get("binds.module.desc");

    public binds() : base(category.Misc, "Binds", "", "", false)
    {
    }

    public override void on_command(string[] args)
    {
        if (args.Length == 0) { list_binds(); return; }

        if (args[0].Equals("list", StringComparison.OrdinalIgnoreCase)) { list_binds(); return; }
        if (args[0].Equals("clear", StringComparison.OrdinalIgnoreCase)) { clear_all(); return; }

        if (args.Length < 2)
        {
            chat_response.send(lang_manager.get("binds.module.usage"));
            return;
        }

        var target = module_manager.find(args[0]);
        if (target == null)
        {
            chat_response.send(lang_manager.get("binds.module.notfound", args[0]));
            return;
        }

        if (!target.BINDABLE)
        {
            chat_response.send(lang_manager.get("binds.module.notbindable", target.NAME));
            return;
        }

        if (args[1].Equals("none", StringComparison.OrdinalIgnoreCase)
            || args[1].Equals("clear", StringComparison.OrdinalIgnoreCase))
        {
            target.BIND.VK = 0;
            target.BIND.CTRL = target.BIND.SHIFT = target.BIND.ALT = false;
            chat_response.send(lang_manager.get("binds.module.cleared", target.NAME));
            return;
        }

        if (!args.try_keybind(1, out int vk, out bool ctrl, out bool shift, out bool alt))
        {
            chat_response.send(lang_manager.get("binds.module.key.error", args[1]));
            return;
        }

        target.BIND.VK = vk;
        target.BIND.CTRL = ctrl;
        target.BIND.SHIFT = shift;
        target.BIND.ALT = alt;

        chat_response.send(lang_manager.get("binds.module.set", target.NAME, target.BIND));
    }

    private static void list_binds()
    {
        chat_response.send(lang_manager.get("binds.module.header"));

        bool any = false;
        var modules = module_manager.ALL;
        for (int i = 0; i < modules.Count; i++)
        {
            var m = modules[i];
            if (m.BIND.IS_EMPTY) continue;

            any = true;
            chat_response.send(lang_manager.get("binds.module.line",
                m.NAME.ToLower(), m.BIND.ToString()));
        }

        if (!any)
            chat_response.send(lang_manager.get("binds.module.empty"));
    }

    private static void clear_all()
    {
        var modules = module_manager.ALL;
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].BIND.VK = 0;
            modules[i].BIND.CTRL = modules[i].BIND.SHIFT = modules[i].BIND.ALT = false;
        }
        chat_response.send(lang_manager.get("binds.module.allcleared"));
    }
}