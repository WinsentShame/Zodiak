namespace Zodiak;

public static class keybind_manager
{
    public static void tick()
    {
        thread_input.attach();

        if (!minecraft_game_get_screen.is_hud()) return;

        var modules = module_manager.ALL;
        for (int i = 0; i < modules.Count; i++)
        {
            var m = modules[i];
            if (m.BIND.IS_EMPTY) continue;
            if (!m.BIND.matches()) continue;
            m.ENABLED = !m.ENABLED;
        }
    }
}