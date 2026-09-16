namespace Zodiak;

public static class OFFSETS
{
    public static class FUNC
    {
        public const nint CLIENTINSTANCE_LEAVEGAME = 0x119C80;
        public const nint MINECRAFTGAME_UPDATEGRAPHICS = 0x131D00;
        public const nint PLAYER_NORMALTICK = 0xA437C0;
        public const nint LEVEL_TICK = 0xB9F660;
        public const nint SCREENRENDERER_SINGLETON = 0x1D9620;
        public const nint SCREENRENDERER_FILL = 0x1DAF10;
        public const nint FONT_DRAWCACHED = 0x1C8B60;
        public const nint ENTITYRENDERER_RENDERTEXT = 0x561E70;
        public const nint ENTITYRENDERER_GETOFFSET = 0x562050;
        public const nint MATRIX_PERSPECTIVE = 0x349A40;
        public const nint OPTIONS_GETPLAYERVIEWPERSPECTIVE = 0x488420;
        public const nint MATRIX_SCALE = 0x15D560;
    }

    public static class DATA
    {
        public const nint LOCALPLAYER_VTABLE = 0x1724CD8;
        public const nint GUIDATA_GUISCALE = 0x16EC0A4;
        public const nint G_CAMERAPOS = 0x19436F8;
        public const nint BASEENTITYRENDERER_TEXTBILLBOARDRETURN = 0x5C8EC8;
        public const nint LEVELRENDERERPLAYER_SETUPCAMERAPERSPRETURN = 0x5BA3F2;
    }

    public static class FIELD
    {
        public const nint MINECRAFTGAME_FONT = 0x88;
        public const nint ENTITY_POS = 0x88;
        public const nint ENTITY_POSOLD = 0x94;
        public const nint ENTITY_POSEXTRAP = 0xA0;
        public const nint ENTITY_ROT = 0xB8;
    }
}