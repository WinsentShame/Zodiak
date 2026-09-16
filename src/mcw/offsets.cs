namespace Zodiak;

public static class OFFSETS
{
    public static class FUNC
    {
        public const nint CLIENTINSTANCE_ONTICK = 0x11B2D0;
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
        public const nint MOUSEDEVICE_FEED = 0x718F40;
        public const nint INPUTHANDLER_HANDLEBUTTONEVENT = 0x715C40;
        public const nint MOVEINPUTHANDLER_TICK = 0x443DC0;
        public const nint CLIENTINSTANCE_GRABMOUSE = 0x11BE90;
        public const nint MINECRAFTGAME_RELEASEMOUSE = 0x13D920;
        public const nint CLIENTINSTANCE_TICKBUILDACTION = 0x11C350;
        public const nint MINECRAFTSCREENMODEL_SENDCHATMESSAGE = 0x399C70;
        public const nint GUIDATA_DISPLAYCLIENTMESSAGE = 0x1CDD30;
        public const nint LEVELRENDERERCAMERA_SETUPFOG = 0x5AFE00;

        public const nint LEVELRENDERERCAMERA_RENDERSKY = 0x5ACE40;
        public const nint LEVELRENDERERCAMERA_RENDERSUNORMOON = 0x5AD330;
        public const nint LEVELRENDERERCAMERA_RENDERSTARS = 0x5AD1D0;

        public const nint TEXTUREPTR_CTOR = 0x73F2B0;
        public const nint TEXTUREGROUP_REMOVEREF = 0x44C160;

        public const nint MATRIXSTACK_PUSH = 0x730000;

        public const nint TESSELLATOR_BEGIN = 0x5D0660;
        public const nint TESSELLATOR_COLOUR = 0x5D0890;
        public const nint TESSELLATOR_VERTEXUV = 0x5D0960;
        public const nint TESSELLATOR_DRAW2 = 0x5D19E0;

        public const nint G_TESSELLATOR = 0x1925550;
        public const nint G_SKYMATRIXSTACK = 0x192AED0;
        public const nint G_SKYCOLOUR = 0x192AE08;
    }

    public static class DATA
    {
        public const nint LOCALPLAYER_VTABLE = 0x1724CD8;
        public const nint GUIDATA_GUISCALE = 0x16EC0A4;
        public const nint G_CAMERAPOS = 0x19436F8;
        public const nint BASEENTITYRENDERER_TEXTBILLBOARDRETURN = 0x5C8EC8;
        public const nint LEVELRENDERERPLAYER_SETUPCAMERAPERSPRETURN = 0x5BA3F2;
        public const nint TEXTUREPTR_SIZE = 0x58;
        public const nint TEXTUREGROUP_REGISTRY = 0x28;
    }

    public static class FIELD
    {

        public const nint MINECRAFTGAME_GUIDATA = 0x170;
        public const nint MINECRAFTGAME_FONT = 0x88;
        public const nint ENTITY_POS = 0x88;
        public const nint ENTITY_POSOLD = 0x94;
        public const nint ENTITY_POSEXTRAP = 0xA0;
        public const nint ENTITY_ROT = 0xB8;

        public const nint LEVELRENDERERCAMERA_FOGCOLOUR = 0x3C8;
        public const nint LEVELRENDERERCAMERA_SUNMATERIAL = 0x308;

        public const nint CLIENTINSTANCE_TEXTURECONTAINER = 0x30;
        public const nint CLIENTINSTANCE_TEXTUREGROUP = 0x80;
    }
}