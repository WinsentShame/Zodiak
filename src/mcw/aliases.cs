using unsafe on_tick_sig = delegate* unmanaged[Stdcall]<nint, int, int, void>;
using unsafe update_graphics_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;
using unsafe render_sky_sig = delegate* unmanaged[Stdcall]<nint, float, float, void>;
using unsafe leave_game_sig = delegate* unmanaged[Stdcall]<nint, nint, long>;
using unsafe send_chat_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;

namespace Zodiak;