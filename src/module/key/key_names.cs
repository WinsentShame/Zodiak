namespace Zodiak;

public static class key_names
{
    private static readonly Dictionary<int, string> names = new()
    {
        { 0x08, "Backspace" }, { 0x09, "Tab" }, { 0x0D, "Enter" }, { 0x1B, "Escape" },
        { 0x20, "Space" }, { 0x21, "PageUp" }, { 0x22, "PageDown" }, { 0x23, "End" },
        { 0x24, "Home" }, { 0x25, "Left" }, { 0x26, "Up" }, { 0x27, "Right" },
        { 0x28, "Down" }, { 0x2D, "Insert" }, { 0x2E, "Delete" },
        { 0x14, "CapsLock" },
        { 0x60, "Num0" }, { 0x61, "Num1" }, { 0x62, "Num2" }, { 0x63, "Num3" },
        { 0x64, "Num4" }, { 0x65, "Num5" }, { 0x66, "Num6" }, { 0x67, "Num7" },
        { 0x68, "Num8" }, { 0x69, "Num9" },
    };

    static key_names()
    {
        for (int i = 0; i < 26; i++) names[0x41 + i] = ((char)('A' + i)).ToString();
        for (int i = 0; i < 10; i++) names[0x30 + i] = ((char)('0' + i)).ToString();
        for (int i = 1; i <= 24; i++) names[0x70 + i - 1] = "F" + i;
    }

    public static string get(int vk)
        => names.TryGetValue(vk, out var name) ? name : $"VK_{vk:X2}";

    public static int from_string(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        foreach (var kv in names)
            if (string.Equals(kv.Value, s, StringComparison.OrdinalIgnoreCase))
                return kv.Key;
        return 0;
    }
}