namespace Zodiak;

public static class lang_manager
{
    public static language CURRENT { get; private set; } = language.Russian;

    private const string project_name = "§7[§f§l Zodiak §r§7]§r ";

    private static readonly Dictionary<string, (string ru, string en)> table = new()
    {
        ["lang.module.desc"] = ("Языковые изменения.", "Language changes."),
        ["lang.module.usage"] = (".lang / .lang ru / .lang en", ".lang / .lang ru / .lang en"),
        ["lang.module.changed"] = (project_name + "Язык изменен с {0} на {1}.", project_name + "Language changed from {0} to {1}."),
        ["lang.module.already"] = (project_name + "Текущий язык уже {0}.", project_name + "Language is already {0}."),

        ["chat.error"] = (project_name + "Прости, я не знаю эту команду.", project_name + "Sorry, I don't know that command."),

        ["binds.module.desc"] = ("Управление биндами клавиш.", "Keybind management."),
        ["binds.module.usage"] = (".binds / .binds <module> <key>", ".binds / .binds <module> <key>"),
        ["binds.module.empty"] = (" Бинды отсутствуют.", " No keybindings set."),
        ["binds.module.notfound"] = (project_name + "Модуль не найден: {0}", "Module not found: {0}"),
        ["binds.module.cleared"] = (project_name + "{0}: бинд очищен", "{0}: bind cleared"),
        ["binds.module.set"] = (project_name + "{0} → {1}", "{0} → {1}"),
        ["binds.module.line"] = (project_name + "§7{0} §f→ §b{1}", "§7{0} §f→ §b{1}"),
        ["binds.module.header"] = (project_name + "Текущие бинды:", project_name + "Current keybindings:"),
        ["binds.module.allcleared"] = (project_name + "Все бинды очищены.", project_name + "All binds cleared."),
        ["binds.module.key.error"] = (project_name + "Клавиша не найдена: {0}", project_name + "Key not found: {0}"),
        ["binds.module.key.unknown"] = (project_name + "Клавиша не найдена.", project_name + "Key not found."),
        ["binds.module.notbindable"] = (project_name + "Невозможно забиндить '{0}' модуль.", project_name + "{0} cannot be bound"),

        ["help.module.desc"] = ("Список доступных команд.", "List of available commands."),
        ["help.module.usage"] = (".help", ".help"),
        ["help.module.usage.label"] = ("Использование:", "Usage:"),
        ["help.module.zodiak.commands"] = ("§l§fZodiak§r - действующие команды:", "§l§fZodiak§r - active commands:"),

        ["skycubemap.render.default.status"] = ("Не загружено.", "Not loaded."),
        ["skycubemap.render.default.wait"] = ("", ""),
        ["skycubemap.render.default.ready"] = ("", ""),
        ["skycubemap.render.default.notloaded"] = ("", ""),

        ["skybox.module.desc"] = ("Кубическая карта неба из ресурс пака.", "A skybox from the resource pack."),
        ["skybox.module.usage"] = (".skybox", ".skybox"),
        ["skybox.module.enable"] = (project_name + "Skybox включен.", project_name + "Skybox enabled."),
        ["skybox.module.disable"] = (project_name + "Skybox выключен.", project_name + "Skybox disabled."),
        ["skybox.module.error"] = (project_name + "Skybox: ", project_name + "Skybox: "),

        ["fogcolor.module.desc"] = ("Пользовательский цвет тумана.", "Custom fog color."),
        ["fogcolor.module.usage"] = (".fogcolor / .fogcolor <r> <g> <b>", ".fogcolor / .fogcolor <r> <g> <b>"),
        ["fogcolor.module.enable"] = (project_name + "Fog color включен ({0:F2}, {1:F2}, {2:F2}).", project_name + "Fog color enabled ({0:F2}, {1:F2}, {2:F2})."),
        ["fogcolor.module.color"] = (project_name + "Цвет тумана ({0:F2}, {1:F2}, {2:F2})", project_name + "Fog color ({0:F2}, {1:F2}, {2:F2})"),
        ["fogcolor.module.disable"] = (project_name + "Fog color выключен.", project_name + project_name + "Fog color disabled."),

        ["wallhack.module.desc"] = ("Игроки видны сквозь стены.", "Players are visible through walls."),
        ["wallhack.usage"] = (".wallhack", ".wallhack"),
        ["wallhack.module.enable"] = (project_name + " Wallhack включен.", project_name + " Wallhack enabled."),
        ["wallhack.module.disable"] = (project_name + " Wallhack выключен.", project_name + " Wallhack disabled."),

        ["watermark.module.desc"] = ("Дополнительная информация.", "Additional information."),
        ["watermark.module.usage"] = (".watermark", ".watermark"),
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