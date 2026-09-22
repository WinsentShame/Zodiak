using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_camera_render_sky : hook_group
{
    private const int fog_colour_size = 12;

    public static bool ACTIVE;

    private static render_sky_sig original;
    private static render_sun_moon_sig sun_moon;
    private static render_stars_sig stars;
    private static nint base_address;

    private static byte_patch? hide_sky;
    private static nint tod_address;
    private static byte[] tod_original = Array.Empty<byte>();

    protected override string NAME => "level_renderer_camera_render_sky";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSKY;
    protected override void store_original(nint ptr) => original = (render_sky_sig)ptr;

    protected override nint detour_ptr()
    {
        render_sky_sig fn = &detour;
        return (nint)fn;
    }

    protected override void on_installed()
    {
        base_address = native_interop.get_module_handle_w(null);

        sun_moon = (render_sun_moon_sig)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSUNORMOON);
        stars = (render_stars_sig)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSTARS);

        hide_sky = new byte_patch(signatures.HIDE_SKY, 6);
    }

    public static void set_hide(bool hide)
    {
        if (hide_sky == null || base_address == 0) return;
        if (hide) hide_sky.apply(base_address);
        else hide_sky.revert();
    }

    public static void set_time_of_day(bool on)
    {
        if (base_address == 0) return;

        if (on)
        {
            if (tod_address == 0)
                tod_address = memory.find_pattern(base_address, signatures.TIME_OF_DAY);
            if (tod_address == 0 || tod_original.Length != 0) return;
            tod_original = memory.patch(tod_address, instructions.TIME_OF_DAY_MOVSS);
        }
        else
        {
            if (tod_original.Length == 0) return;
            memory.patch(tod_address, tod_original);
            tod_original = Array.Empty<byte>();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, float a, float b)
    {
        if (!ACTIVE || !sky_cubemap.refresh())
        {
            set_hide(false);
            original(self, a, b);
            return;
        }

        set_hide(true);

        var fog = fog_slot(self);
        var fog_saved = fog.read();
        fog.write(0f, 0f, 0f);

        original(self, a, b);

        fog.write(fog_saved);

        sun_moon(self, b, 1);
        sun_moon(self, b, 0);
        stars(self, b, a);

        draw_cubemap_bright(self);
    }

    private static void draw_cubemap_bright(nint self)
    {
        var fog = fog_slot(self);
        var fog_saved = fog.read();
        fog.write(1f, 1f, 1f);

        nint sky_ptr = base_address + OFFSETS.FUNC.G_SKYCOLOUR;
        var sky = fog_slot(sky_ptr);
        var sky_saved = sky.read();
        sky.write(1f, 1f, 1f);

        sky_cubemap.draw(self);

        sky.write(sky_saved);
        fog.write(fog_saved);
    }

    private static fog_ref fog_slot(nint addr)
    {
        if (!memory.is_readable(addr, fog_colour_size)) return new fog_ref(null);
        return new fog_ref((float*)addr);
    }

    private readonly unsafe struct fog_ref
    {
        private readonly float* ptr;

        public fog_ref(float* p) { ptr = p; }

        public bool valid => ptr != null;

        public (float r, float g, float b) read()
            => valid ? (ptr[0], ptr[1], ptr[2]) : (0f, 0f, 0f);

        public void write((float r, float g, float b) c)
        {
            if (!valid) return;
            ptr[0] = c.r;
            ptr[1] = c.g;
            ptr[2] = c.b;
        }

        public void write(float r, float g, float b)
        {
            if (!valid) return;
            ptr[0] = r;
            ptr[1] = g;
            ptr[2] = b;
        }
    }
}