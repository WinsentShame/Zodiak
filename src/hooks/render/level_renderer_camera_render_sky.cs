using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class level_renderer_camera_render_sky
{
    private const string HIDE_SKY_SIGNATURE = "0F 85 ?? ?? ?? ?? 48 8D 54 24 30 E8 ?? ?? ?? ?? 90 48 8B 5C 24 30";
    private const int FOG_COLOUR_SIZE = 12;
    private const int HIDE_SKY_SIZE = 6;

    public static bool active;

    private static nint original;
    private static nint base_address; 
    private static byte_patch? hide_sky;

    public static bool install()
    {
        base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) { return false; }

        hide_sky = new byte_patch(HIDE_SKY_SIGNATURE, HIDE_SKY_SIZE);

        nint address = base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSKY;
        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, float, float, void>)&hook;

        int st = native_interop.mh_create_hook(address, detour_ptr, out nint orig);
        if (st != 0) {  return false; }

        original = orig;

        st = native_interop.mh_enable_hook(address);
        if (st != 0) {return false; }

        return true;
    }

    public static void set_hide(bool hide)
    {
        if (hide_sky == null) return;
        if (hide) hide_sky.apply(base_address);
        else hide_sky.revert();
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void hook(nint self, float a, float b)
    {
        try
        {
            var _original = (delegate* unmanaged[Stdcall]<nint, float, float, void>)original;

            if (!active || !sky_cubemap.refresh())
            {
                set_hide(false);
                _original(self, a, b);
                return;
            }

            set_hide(true);

            float* fog = get_fog(self);
            var fog_saved = save_fog(fog);
            set_fog(fog, 0f, 0f, 0f);

            _original(self, a, b);

            restore_fog(fog, fog_saved);

            draw_sun_and_stars(self, a, b);

            draw_cubemap_bright(self);
        }
        catch 
        {
            active = false;
        }
    }

    private static float* get_fog(nint self)
    {
        nint slot = self + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR;
        if (!memory.is_readable(slot, FOG_COLOUR_SIZE)) return null;
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
        var sun_moon = (delegate* unmanaged[Stdcall]<nint, float, byte, void>)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSUNORMOON);
        var starts = (delegate* unmanaged[Stdcall]<nint, float, float, void>)
            (base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERSTARS);

        sun_moon(self, b, 1);
        sun_moon(self, b, 0);
        starts(self, b, a);
    }

    private static void draw_cubemap_bright(nint self)
    {
        float* fog = get_fog(self);
        var fog_saved = save_fog(fog);
        set_fog(fog, 1f, 1f, 1f);

        nint sky_ptr = base_address + OFFSETS.FUNC.G_SKYCOLOUR;
        bool have_sky = memory.is_readable(sky_ptr, FOG_COLOUR_SIZE);
        float* sky = have_sky ? (float*)sky_ptr : null;
        var sky_saved = save_fog(sky);
        set_fog(sky, 1f, 1f, 1f);

        sky_cubemap.draw(self);

        restore_fog(sky, sky_saved);
        restore_fog(fog, fog_saved);
    }
}