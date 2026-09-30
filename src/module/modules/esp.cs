namespace Zodiak;

public sealed class esp : module
{
    public override string DESCRIPTION => lang_manager.get("esp.module.desc");
    public override string USAGE => lang_manager.get("esp.usage");

    public static bool ACTIVE;
    public static bool PLAYER_MODE = true;
    public static bool CHEST_MODE = true;

    public esp() : base(category.Visual, "Esp", "", "", true)
    {
    }

    public override void on_enable()
    {
        ACTIVE = true;
        PLAYER_MODE = true;
        CHEST_MODE = true;
        entity_cache.clear();
        chest_cache.clear();
        chat_response.send(lang_manager.get("esp.module.enable"));
    }

    public override void on_disable()
    {
        ACTIVE = false;
        PLAYER_MODE = false;
        CHEST_MODE = false;
        entity_cache.clear();
        chest_cache.clear();
        chat_response.send(lang_manager.get("esp.module.disable"));
    }

    public override void on_command(string[] args)
    {
        if (args.Length == 0)
        {
            ENABLED = !ENABLED;
            return;
        }

        if (args[0].Equals("player", StringComparison.OrdinalIgnoreCase))
        {
            PLAYER_MODE = !PLAYER_MODE;
            if (!ENABLED) ENABLED = true;
            chat_response.send(lang_manager.get("esp.module.player", PLAYER_MODE ? "on" : "off"));
            return;
        }

        if (args[0].Equals("chest", StringComparison.OrdinalIgnoreCase))
        {
            CHEST_MODE = !CHEST_MODE;
            if (!ENABLED) ENABLED = true;
            chat_response.send(lang_manager.get("esp.module.chest", CHEST_MODE ? "on" : "off"));
            return;
        }

        chat_response.send(lang_manager.get("esp.usage"));
    }
}