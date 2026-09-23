using System.Collections.Generic;

namespace Zodiak;

public sealed class keybind
{
    public int VK { get; set; }
    public bool CTRL { get; set; }
    public bool SHIFT { get; set; }
    public bool ALT { get; set; }

    public bool IS_EMPTY => VK == 0;

    public bool matches()
    {
        if (IS_EMPTY) return false;
        if (!thread_input.was_pressed(VK)) return false;
        return true;
    }

    public override string ToString()
    {
        if (IS_EMPTY) return "none";

        var parts = new List<string>();
        if (CTRL) parts.Add("Ctrl");
        if (SHIFT) parts.Add("Shift");
        if (ALT) parts.Add("Alt");
        parts.Add(key_names.get(VK));
        return string.Join("+", parts);
    }
}