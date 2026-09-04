using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class leave_game
{
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    delegate void client_instance_leave_game_delegate(IntPtr self, IntPtr a2);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    delegate void minecraft_game_leave_game_delegate(IntPtr self, IntPtr a2);

    static readonly IntPtr base_address = native.GetModuleHandle(null);
    static readonly IntPtr client_leave_addr = base_address + (int)offsets.CLIENT_INSTANCE_LEAVE_GAME;
    static readonly IntPtr game_leave_addr = base_address + (int)offsets.MINECRAFT_GAME_LEAVE_GAME;

    static IntPtr original_client_leave_ptr;
    static IntPtr original_game_leave_ptr;
    static client_instance_leave_game_delegate original_client_leave = null!;
    static minecraft_game_leave_game_delegate original_game_leave = null!;

    public static void install()
    {
        logger.info("leave_game", "installing hooks...");

        void* detourClient = (delegate* unmanaged[Thiscall]<IntPtr, IntPtr, void>)&client_leave_game_hook;
        void* detourGame = (delegate* unmanaged[Thiscall]<IntPtr, IntPtr, void>)&game_leave_game_hook;

        void* origClient = null;
        void* origGame = null;

        bool clientOk = native.MH_CreateHook((void*)client_leave_addr, detourClient, &origClient) == 0;
        bool gameOk = native.MH_CreateHook((void*)game_leave_addr, detourGame, &origGame) == 0;

        if (clientOk && gameOk)
        {
            original_client_leave_ptr = (IntPtr)origClient;
            original_game_leave_ptr = (IntPtr)origGame;
            original_client_leave = Marshal.GetDelegateForFunctionPointer<client_instance_leave_game_delegate>(original_client_leave_ptr);
            original_game_leave = Marshal.GetDelegateForFunctionPointer<minecraft_game_leave_game_delegate>(original_game_leave_ptr);

            native.MH_EnableHook((void*)client_leave_addr);
            native.MH_EnableHook((void*)game_leave_addr);

            logger.info("leave_game", "hooks installed");
        }
        else
        {
            logger.error("leave_game", $"failed to install hooks: client={clientOk}, game={gameOk}");
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    static void client_leave_game_hook(IntPtr self, IntPtr a2)
    {
        logger.info("leave_game", $"client_leave_game called, self=0x{self:X}, a2=0x{a2:X}");
        original_client_leave(self, a2);
        logger.info("leave_game", "client_leave_game finished");
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    static void game_leave_game_hook(IntPtr self, IntPtr a2)
    {
        logger.info("leave_game", $"game_leave_game called, self=0x{self:X}, a2=0x{a2:X}");
        original_game_leave(self, a2);
        logger.info("leave_game", "game_leave_game finished");
    }
}