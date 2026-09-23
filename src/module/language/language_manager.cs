namespace Zodiak;

public static class lang_manager
{
    public static language CURRENT { get; private set; } = language.Russian;

    private const string project_name = "§7[§f§l Zodiak §r§7]§r ";

    private static readonly Dictionary<string, (string ru, string en)> table = new()
    {
        ["lang.module.desc"] = ("Языковые изменения.", "Language changes."),
        ["lang.changed"] = (project_name + "Язык изменен с {0} на {1}.", project_name + "Language changed from {0} to {1}."),
        ["lang.already"] = (project_name + "Текущий язык уже {0}.", project_name + "Language is already {0}."),
        ["lang.usage"] = (project_name + "Используй: .lang [ru|en]", project_name + "Usage: .lang [ru|en]"),

        ["chat.error"] = (project_name + "Прости, я не знаю эту команду.", project_name + "Sorry, I don't know that command."),

        ["binds.module.desc"] = ("Управление биндами клавиш.", "Keybind management."),
        ["binds.module.usage"] = ("Используй: .binds <module> <key>", "Usage: .binds <module> <key>"),
        ["binds.module.notfound"] = ("Модуль не найден: {0}", "Module not found: {0}"),
        ["binds.module.cleared"] = ("{0}: бинд очищен", "{0}: bind cleared"),
        ["binds.module.set"] = ("{0} → {1}", "{0} → {1}"),
        ["binds.module.line"] = ("§7{0} §f→ §b{1}", "§7{0} §f→ §b{1}"),
        ["binds.module.header"] = ("Текущие бинды:", "Current keybindings:"),
        ["binds.module.empty"] = (" Бинды отсутствуют.", " No keybindings set."),
        ["binds.module.allcleared"] = ("Все бинды очищены", "All binds cleared"),
        ["binds.module.key.error"] = ("Клавиша не найдена: {0}", "Key not found: {0}"),
        ["binds.module.key.unknown"] = ("Клавиша не найдена", "Key not found"),

        ["help.module.desc"] = ("Список доступных команд.", "List of available commands."),
        ["help.module.zodiak.commands"] = ("§l§fZodiak§r - действующие команды:", "§l§fZodiak§r - active commands:"),

        ["skycubemap.render.default.status"] = ("Не загружено.", "Not loaded."),
        ["skycubemap.render.default.wait"] = ("", ""),
        ["skycubemap.render.default.ready"] = ("", ""),
        ["skycubemap.render.default.notloaded"] = ("", ""),

        ["skybox.module.desc"] = ("Кубическая карта неба из ресурс пака.", "A skybox from the resource pack."),
        ["skybox.module.enable"] = (project_name + "Skybox включен.", project_name + "Skybox enabled."),
        ["skybox.module.disable"] = (project_name + "Skybox выключен.", project_name + "Skybox disabled."),
        ["skybox.module.error"] = (project_name + "Skybox: ", project_name + "Skybox: "),

        ["fogcolor.module.desc"] = ("Пользовательский цвет тумана.", "Custom fog color."),
        ["fogcolor.module.enable"] = ("Fog color включен ({0:F2}, {1:F2}, {2:F2}).",
                                        "Fog color enabled ({0:F2}, {1:F2}, {2:F2})."),
        ["fogcolor.module.color"] = ("Цвет тумана ({0:F2}, {1:F2}, {2:F2})",
                                        "Fog color ({0:F2}, {1:F2}, {2:F2})"),
        ["fogcolor.module.disable"] = (project_name + "Fog color выключен.",
                                        project_name + "Fog color disabled."),
        ["fogcolor.module.usage"] = (project_name + "Используй: .fogcolor <r> <g> <b>",
                                        project_name + "Usage: .fogcolor <r> <g> <b>"),

        ["watermark.module.desc"] = ("Дополнительная информация.", "Additional information."),
    };

    public static string get(string key, params object[] args)
    {
        if (!table.TryGetValue(key, out var entry)) return key;

        string template = CURRENT == language.Russian ? entry.ru : entry.en;

        return args.Length > 0
            ? string.Format(System.Globalization.CultureInfo.InvariantCulture, template, args)
            : template;
    }

    public static void set(language target) => CURRENT = target;
    public static void toggle() => CURRENT = CURRENT == language.Russian ? language.English : language.Russian;

    public static string native_name(language l)
        => l == language.Russian ? "Русский" : "English";
}