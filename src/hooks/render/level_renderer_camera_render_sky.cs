using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe render_sky_sig = delegate* unmanaged[Stdcall]<nint, float, float, void>;
using unsafe render_sun_moon_sig = delegate* unmanaged[Stdcall]<nint, float, byte, void>;
using unsafe render_stars_sig = delegate* unmanaged[Stdcall]<nint, float, float, void>;

namespace Zodiak;

public static unsafe class level_renderer_camera_render_sky
{
    private const int fog_colour_size = 12;

    public static bool ACTIVE;

    private static render_sky_sig ORIGINAL;
    private static nint base_address;
    private static byte_patch? hide_sky;
    private static nint tod_address;
    private static byte[] tod_original = Array.Empty<byte>();

    public static bool install()
    {
        base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        hide_sky = new byte_patch(signatures.HIDE_SKY, 6);

        nint address = base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSKY;
        render_sky_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (render_sky_sig)original_ptr;
        return true;
    }

    public static void set_hide(bool hide)
    {
        if (hide_sky == null) return;
        if (hide) hide_sky.apply(base_address);
        else hide_sky.revert();
    }

    public static void set_time_of_day(bool on)
    {
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
        try
        {
            if (!ACTIVE || !sky_cubemap.refresh())
            {
                set_hide(false);
                ORIGINAL(self, a, b);
                return;
            }

            set_hide(true);

            float* fog = get_fog(self);
            var fog_saved = save_fog(fog);
            set_fog(fog, 0f, 0f, 0f);

            ORIGINAL(self, a, b);

            restore_fog(fog, fog_saved);

            draw_sun_and_stars(self, a, b);
            draw_cubemap_bright(self);
        }
        catch
        {
            ACTIVE = false;
        }
    }

    private static float* get_fog(nint self)
    {
        nint slot = self + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR;
        if (!memory.is_readable(slot, fog_colour_size)) return null;
        return (float*)slot;
    }

    private static (float R, float G, float B)? save_fog(float* fog)
    {
        if (fog == null) return null;
        return (fog[0], fog[1], fog[2]);
    }

    private static void restore_fog(float* fog, (float R, float G, float B)? saved)
    {
        if (fog == null || saved == null) return;
        fog[0] = saved.Value.R;
        fog[1] = saved.Value.G;
        fog[2] = saved.Value.B;
    }

    private static void set_fog(float* fog, float r, float g, float b)
    {
        if (fog == null) return;
        fog[0] = r;
        fog[1] = g;
        fog[2] = b;
    }

    private static void draw_sun_and_stars(nint self, float a, float b)
    {
        var sun_moon = (render_sun_moon_sig)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSUNORMOON);
        var stars = (render_stars_sig)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSTARS);

        sun_moon(self, b, 1);
        sun_moon(self, b, 0);
        stars(self, b, a);
    }

    private static void draw_cubemap_bright(nint self)
    {
        float* fog = get_fog(self);
        var fog_saved = save_fog(fog);
        set_fog(fog, 1f, 1f, 1f);

        nint sky_ptr = base_address + OFFSETS.FUNC.G_SKYCOLOUR;
        bool have_sky = memory.is_readable(sky_ptr, fog_colour_size);
        float* sky = have_sky ? (float*)sky_ptr : null;
        var sky_saved = save_fog(sky);
        set_fog(sky, 1f, 1f, 1f);

        sky_cubemap.draw(self);

        restore_fog(sky, sky_saved);
        restore_fog(fog, fog_saved);
    }
}