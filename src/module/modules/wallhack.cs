namespace Zodiak;

public sealed class wallhack : module
{
    public override string DESCRIPTION => lang_manager.get("wallhack.module.desc");
    public override string USAGE => lang_manager.get("wallhack.usage");

    public wallhack() : base(category.Visual, "Wallhack", "", "", true)
    {
    }

    public override void on_enable()
    {
        wallhack_state.ACTIVE = true;
        chat_response.send(lang_manager.get("wallhack.module.enable"));
    }

    public override void on_disable()
    {
        wallhack_state.ACTIVE = false;
        chat_response.send(lang_manager.get("wallhack.module.disable"));
    }
}