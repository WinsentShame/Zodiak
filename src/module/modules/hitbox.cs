namespace Zodiak;

public sealed class hitbox : module
{
    public override string DESCRIPTION => lang_manager.get("hitbox.module.desc");
    public override string USAGE => lang_manager.get("hitbox.usage");

    public static bool ACTIVE;
    public static float WIDTH_MULT = 1.5f;
    public static float HEIGHT_MULT = 1.5f;

    public hitbox() : base(category.Player, "Hitbox", "", "", true)
    {
    }

    public override void on_enable()
    {
        ACTIVE = true;
        chat_response.send(lang_manager.get("hitbox.module.enable", WIDTH_MULT, HEIGHT_MULT));
    }

    public override void on_disable()
    {
        ACTIVE = false;
        hitbox_state.restore_all();
        chat_response.send(lang_manager.get("hitbox.module.disable"));
    }

    public override void on_command(string[] args)
    {
        if (args.Length == 0)
        {
            ENABLED = !ENABLED;
            return;
        }

        if (args[0].Equals("size", StringComparison.OrdinalIgnoreCase))
        {
            if (!args.try_float(1, out float value))
            {
                chat_response.send(lang_manager.get("hitbox.usage"));
                return;
            }

            value = Math.Clamp(value, 1.0f, 5.0f);
            WIDTH_MULT = value;
            HEIGHT_MULT = value;

            if (ENABLED)
            {
                ENABLED = false;
                ENABLED = true;
            }

            chat_response.send(lang_manager.get("hitbox.module.size.set", value));
            return;
        }

        chat_response.send(lang_manager.get("hitbox.usage"));
    }
}