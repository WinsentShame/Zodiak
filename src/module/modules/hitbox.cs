namespace Zodiak;

public sealed class hitboxes : module
{
    public override string DESCRIPTION => lang_manager.get("hitboxes.module.desc");
    public override string USAGE => lang_manager.get("hitboxes.usage");

    public static bool ACTIVE;

    public hitboxes() : base(category.Visual, "Hitboxes", "", "", true)
    {
    }

    public override void on_enable()
    {
        ACTIVE = true;
        entity_cache.clear();
        chat_response.send(lang_manager.get("hitboxes.module.enable"));
    }

    public override void on_disable()
    {
        ACTIVE = false;
        entity_cache.clear();
        chat_response.send(lang_manager.get("hitboxes.module.disable"));
    }
}