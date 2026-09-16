using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class overlay
{
    private static nint _base;
    private static delegate* unmanaged[Stdcall]<nint, nint, void> _original;

    private static readonly List<Action<nint>> _renderers = new();

    private static readonly Stopwatch _sw = Stopwatch.StartNew();
    private static int _frames;
    private static double _lastFpsUpdate;

    public static float Fps { get; private set; } = 60f;

    public static bool Install()
    {
        _base = native_interop.GetModuleHandleW(null);
        if (_base == 0) { logger.error("overlay", "no base"); return false; }

        nint addr = _base + OFFSETS.FUNC.MINECRAFTGAME_UPDATEGRAPHICS;
        nint detour = (nint)(delegate* unmanaged[Stdcall]<nint, nint, void>)&Detour;

        int st = native_interop.MH_CreateHook(addr, detour, out nint orig);
        if (st != 0) { logger.error("overlay", $"create: {st}"); return false; }

        _original = (delegate* unmanaged[Stdcall]<nint, nint, void>)orig;

        st = native_interop.MH_EnableHook(addr);
        if (st != 0) { logger.error("overlay", $"enable: {st}"); return false; }

        logger.info("overlay", "installed");
        return true;
    }

    public static void Register(Action<nint> renderer)
    {
        _renderers.Add(renderer);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void Detour(nint self, nint a2)
    {
        _original(self, a2);

        try
        {
            _frames++;
            double now = _sw.Elapsed.TotalSeconds;
            if (now - _lastFpsUpdate >= 1.0)
            {
                Fps = (float)(_frames / (now - _lastFpsUpdate));
                _frames = 0;
                _lastFpsUpdate = now;
            }

            module_manager.update_all();

            for (int i = 0; i < _renderers.Count; i++)
            {
                try { _renderers[i](self); }
                catch (Exception ex) { logger.error("overlay", $"renderer: {ex.Message}"); }
            }
        }
        catch { }
    }
}