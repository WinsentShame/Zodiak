namespace Zodiak;

public static class OFFSETS
{
    public static class FUNC
    {
        public const nint CLIENTINSTANCE_ONTICK = 0x11B2D0;
        public const nint CLIENTINSTANCE_LEAVEGAME = 0x119C80;
        public const nint MINECRAFTGAME_UPDATEGRAPHICS = 0x131D00;
        public const nint MINECRAFTSCREENMODEL_SENDCHATMESSAGE = 0x399C70;
        public const nint GUIDATA_DISPLAYCLIENTMESSAGE = 0x1CDD30;

        public const nint LEVELRENDERERCAMERA_SETUPFOG = 0x5AFE00;
        public const nint LEVELRENDERERCAMERA_RENDERSKY = 0x5ACE40;
        public const nint LEVELRENDERERCAMERA_RENDERSUNORMOON = 0x5AD330;
        public const nint LEVELRENDERERCAMERA_RENDERSTARS = 0x5AD1D0;

        public const nint TEXTUREPTR_CTOR = 0x73F2B0;
        public const nint TEXTUREGROUP_REMOVEREF = 0x44C160;
        public const nint RAKNETNETWORKPEER_UPDATE = 0x7917B0;

        public const nint SCREENRENDERER_SINGLETON = 0x1D9620;
        public const nint SCREENRENDERER_FILL = 0x1DAF10;
        public const nint FONT_DRAWCACHED = 0x1C8B60;

        public const nint MATRIX_SCALE = 0x15D560;
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
        public const nint TEXTUREGROUP_REGISTRY = 0x28;
    }

    public static class FIELD
    {
        public const nint MINECRAFTGAME_GUIDATA = 0x170;
        public const nint MINECRAFTGAME_FONT = 0x88;

        public const nint LEVELRENDERERCAMERA_FOGCOLOUR = 0x3C8;
        public const nint LEVELRENDERERCAMERA_SUNMATERIAL = 0x308;

        public const nint CLIENTINSTANCE_TEXTURECONTAINER = 0x30;
        public const nint CLIENTINSTANCE_TEXTUREGROUP = 0x80;

        public const nint RAKNETNETWORKPEER_AVERAGE_PING = 228;
        public const nint RAKNETNETWORKPEER_LAST_PING = 232;
    }
}