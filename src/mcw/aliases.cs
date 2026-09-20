using unsafe on_tick_sig = delegate* unmanaged[Stdcall]<nint, int, int, void>;
using unsafe update_graphics_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;
using unsafe render_sky_sig = delegate* unmanaged[Stdcall]<nint, float, float, void>;
using unsafe leave_game_sig = delegate* unmanaged[Stdcall]<nint, nint, long>;
using unsafe send_chat_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;
using unsafe font_draw_cached_sig = delegate* unmanaged[Stdcall]<nint, nint, float, float, nint, byte, byte, nint, int, byte, void>;
using unsafe network_peer_update_sig = delegate* unmanaged[Stdcall]<
    nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>;

namespace Zodiak;