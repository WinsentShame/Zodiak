namespace Zodiak;

public static unsafe class fog
{
    public const int SIZE = 16;

    public static fog_ref slot(nint addr)
    {
        if (!memory.is_readable(addr, SIZE)) return new fog_ref(null);
        return new fog_ref((float*)addr);
    }

    public static fog_ref camera(nint camera)
        => slot(camera + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR);

    public static fog_ref sky_colour(nint base_address)
        => slot(base_address + OFFSETS.FUNC.G_SKYCOLOUR);
}

public readonly unsafe struct fog_ref
{
    private readonly float* ptr;

    public fog_ref(float* p) { ptr = p; }

    public bool valid => ptr != null;

    public (float r, float g, float b, float a) read()
        => valid ? (ptr[0], ptr[1], ptr[2], ptr[3]) : (0f, 0f, 0f, 0f);

    public void write((float r, float g, float b, float a) c)
    {
        if (!valid) return;
        ptr[0] = c.r;
        ptr[1] = c.g;
        ptr[2] = c.b;
        ptr[3] = c.a;
    }

    public void write(float r, float g, float b, float a)
    {
        if (!valid) return;
        ptr[0] = r;
        ptr[1] = g;
        ptr[2] = b;
        ptr[3] = a;
    }
}