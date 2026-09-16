using System.Runtime.InteropServices;
using System.Text;

namespace Zodiak;

[StructLayout(LayoutKind.Sequential)]
public struct color4
{
    public float r, g, b, a;

    public color4(float r, float g, float b, float a)
    {
        this.r = r; this.g = g; this.b = b; this.a = a;
    }
}

public static unsafe class draw
{
    private static nint _base;
    private static delegate* unmanaged[Stdcall]<nint> _ssSingleton;
    private static delegate* unmanaged[Stdcall]<nint, float, float, float, float, color4*, void> _ssFill;
    private static delegate* unmanaged[Stdcall]<nint, nint, float, float, color4*, byte, byte, nint, int, byte, void> _fontDraw;

    public static bool Install()
    {
        _base = native_interop.GetModuleHandleW(null);
        if (_base == 0) { logger.error("draw", "no base"); return false; }

        _ssSingleton = (delegate* unmanaged[Stdcall]<nint>)(_base + OFFSETS.FUNC.SCREENRENDERER_SINGLETON);
        _ssFill = (delegate* unmanaged[Stdcall]<nint, float, float, float, float, color4*, void>)(_base + OFFSETS.FUNC.SCREENRENDERER_FILL);
        _fontDraw = (delegate* unmanaged[Stdcall]<nint, nint, float, float, color4*, byte, byte, nint, int, byte, void>)(_base + OFFSETS.FUNC.FONT_DRAWCACHED);

        logger.info("draw", "installed");
        return true;
    }

    public static bool Ready => _base != 0;

    public static void Rect(float x0, float y0, float x1, float y1, in color4 col)
    {
        nint r = _ssSingleton();
        if (r == 0) return;
        fixed (color4* p = &col)
            _ssFill(r, x0, y0, x1, y1, p);
    }

    public static void Outline(float x0, float y0, float x1, float y1, float t, in color4 col)
    {
        Rect(x0, y0, x1, y0 + t, col);
        Rect(x0, y1 - t, x1, y1, col);
        Rect(x0, y0, x0 + t, y1, col);
        Rect(x1 - t, y0, x1, y1, col);
    }

    public static void GradientV(float x0, float y0, float x1, float y1, in color4 top, in color4 bottom, int steps = 16)
    {
        float h = (y1 - y0) / steps;
        for (int i = 0; i < steps; i++)
        {
            float t = (i + 0.5f) / steps;
            color4 c = new color4(
                top.r + (bottom.r - top.r) * t,
                top.g + (bottom.g - top.g) * t,
                top.b + (bottom.b - top.b) * t,
                top.a + (bottom.a - top.a) * t);
            Rect(x0, y0 + i * h, x1, y0 + i * h + h + 0.5f, c);
        }
    }

    public static void Text(nint font, string s, float x, float y, in color4 col)
    {
        if (font == 0 || s.Length == 0) return;

        byte[] raw = Encoding.UTF8.GetBytes(s);
        int len = raw.Length;

        byte* str = stackalloc byte[32];
        for (int i = 0; i < 32; i++) str[i] = 0;

        if (len < 16)
        {
            for (int i = 0; i < len; i++) str[i] = raw[i];
            *(long*)(str + 16) = len;
            *(long*)(str + 24) = 15;
            fixed (color4* p = &col)
                _fontDraw(font, (nint)str, x, y, p, 0, 0, 0, 0, 0);
        }
        else
        {
            nint heap = Marshal.AllocHGlobal(len + 1);
            for (int i = 0; i < len; i++) *(byte*)(heap + i) = raw[i];
            *(byte*)(heap + len) = 0;
            *(nint*)str = heap;
            *(long*)(str + 16) = len;
            *(long*)(str + 24) = len;
            fixed (color4* p = &col)
                _fontDraw(font, (nint)str, x, y, p, 0, 0, 0, 0, 0);
            Marshal.FreeHGlobal(heap);
        }
    }

    public static float TextWidth(string s, float scale = 1f) => s.Length * 6.5f * scale;

    public static nint FontOf(nint minecraftGame)
    {
        if (minecraftGame == 0) return 0;
        if (!memory.isReadable(minecraftGame + OFFSETS.FIELD.MINECRAFTGAME_FONT, 8)) return 0;
        return *(nint*)(minecraftGame + OFFSETS.FIELD.MINECRAFTGAME_FONT);
    }

    public static float GuiScale()
    {
        nint ptr = _base + OFFSETS.DATA.GUIDATA_GUISCALE;
        if (memory.isReadable(ptr, 4))
        {
            float s = *(float*)ptr;
            if (s >= 0.5f && s <= 8f) return s;
        }
        return 2f;
    }

    public static bool ScreenSize(out float w, out float h)
    {
        w = h = 0f;
        nint hwnd = native_interop.GetForegroundWindow();
        if (hwnd == 0) return false;
        if (!native_interop.GetClientRect(hwnd, out native_interop.RECT rc)) return false;

        float gs = GuiScale();
        w = (rc.R - rc.L) / gs;
        h = (rc.B - rc.T) / gs;
        return w > 1f && h > 1f;
    }

    public static void CursorLogical(out float x, out float y)
    {
        x = y = 0f;
        nint hwnd = native_interop.GetForegroundWindow();
        native_interop.GetCursorPos(out native_interop.POINT p);
        native_interop.ScreenToClient(hwnd, ref p);
        float gs = GuiScale();
        x = p.X / gs;
        y = p.Y / gs;
    }
}