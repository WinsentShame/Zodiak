using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class leave_game
{
    private const long LeaveGameRva = 0x119C80;

    private static delegate* unmanaged[Stdcall]<nint, nint, long> _original;
    private static nint _target;

    public static bool IsInstalled { get; private set; }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    public static long Detour(nint self, nint flag)
    {
        try
        {
            logger.info("leave_game", $"called self=0x{self:X} flag={flag}");
            //esp.Clear();
        }
        catch { }

        if (_original == null) return 0;
        return _original(self, flag);
    }

    public static bool Install()
    {
        if (IsInstalled) return true;

        nint baseAddr = native_interop.GetModuleHandleW(null);
        if (baseAddr == 0) { logger.error("leave_game", "no base"); return false; }

        _target = baseAddr + (nint)LeaveGameRva;
        logger.info("leave_game", $"base=0x{baseAddr:X}, target=0x{_target:X}");

        nint detourPtr = (nint)(delegate* unmanaged[Stdcall]<nint, nint, long>)&Detour;

        int status = native_interop.MH_CreateHook(_target, detourPtr, out nint originalPtr);
        if (status != 0) { logger.error("leave_game", $"create: {status}"); return false; }

        _original = (delegate* unmanaged[Stdcall]<nint, nint, long>)originalPtr;

        status = native_interop.MH_EnableHook(_target);
        if (status != 0)
        {
            logger.error("leave_game", $"enable: {status}");
            native_interop.MH_RemoveHook(_target);
            _original = null;
            return false;
        }

        IsInstalled = true;
        logger.info("leave_game", "hook installed");
        return true;
    }

    public static void Uninstall()
    {
        if (!IsInstalled) return;
        native_interop.MH_DisableHook(_target);
        native_interop.MH_RemoveHook(_target);
        _target = 0;
        _original = null;
        IsInstalled = false;
        logger.info("leave_game", "hook removed");
    }
}