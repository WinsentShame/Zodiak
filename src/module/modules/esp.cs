namespace Zodiak;

public sealed class esp : module
{
    public override string DESCRIPTION => lang_manager.get("esp.module.desc");
    public override string USAGE => lang_manager.get("esp.usage");

    public static bool ACTIVE;

    public esp() : base(category.Visual, "Esp", "", "", true)
    {
    }

    public override void on_enable()
    {
        ACTIVE = true;
        entity_cache.clear();
        chat_response.send(lang_manager.get("esp.module.enable"));
    }

    public override void on_disable()
    {
        ACTIVE = false;
        entity_cache.clear();
        chat_response.send(lang_manager.get("esp.module.disable"));
    }
}