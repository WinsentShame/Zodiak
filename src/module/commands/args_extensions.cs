namespace Zodiak;

public static class args_extensions
{
    public static bool try_float(this string[] args, int index, out float value)
    {
        value = 0f;
        return index < args.Length && float.TryParse(args[index], out value);
    }

    public static bool try_int(this string[] args, int index, out int value)
    {
        value = 0;
        return index < args.Length && int.TryParse(args[index], out value);
    }

    public static bool try_floats(this string[] args, out float[] values)
    {
        values = new float[args.Length];
        for (int i = 0; i < args.Length; i++)
            if (!float.TryParse(args[i], out values[i])) return false;
        return true;
    }

    public static bool try_ints(this string[] args, out int[] values)
    {
        values = new int[args.Length];
        for (int i = 0; i < args.Length; i++)
            if (!int.TryParse(args[i], out values[i])) return false;
        return true;
    }

    public static bool try_color(this string[] args, out float r, out float g, out float b)
    {
        r = g = b = 0f;
        if (args.Length < 3) return false;
        return float.TryParse(args[0], out r)
            && float.TryParse(args[1], out g)
            && float.TryParse(args[2], out b);
    }
}