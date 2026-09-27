namespace Zodiak;

public static class OFFSETS
{
    public static class FUNC
    {
        public const nint LOCALPLAYER_NORMALTICK = 0x4AA600;

        public const nint REMOTEPLAYER_NORMALTICK = 0x4B3D10;

        public const nint INGAMEPLAYSCREEN_RENDER = 0x3528C0;

        public const nint CLIENTINSTANCE_ONTICK = 0x11B2D0;
        public const nint CLIENTINSTANCE_LEAVEGAME = 0x119C80;

        public const nint MINECRAFTGAME_UPDATEGRAPHICS = 0x131D00;
        public const nint MINECRAFTSCREENMODEL_SENDCHATMESSAGE = 0x399C70;

        public const nint GUIDATA_DISPLAYCLIENTMESSAGE = 0x1CDD30;

        public const nint LEVELRENDERERCAMERA_SETUPFOG = 0x5AFE00;
        public const nint LEVELRENDERERCAMERA_RENDERSKY = 0x5ACE40;
        public const nint LEVELRENDERERCAMERA_RENDERSUNORMOON = 0x5AD330;
        public const nint LEVELRENDERERCAMERA_RENDERSTARS = 0x5AD1D0;
        public const nint LEVELRENDERERCAMERA_RENDERENTITIES = 0x5AE250;

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
        public const nint LEVELRENDERER_RENDERLEVEL = 0x5A9120;
        public const nint TESSELLATOR_DRAW = 0x5D1840;
        public const nint RENDER_CTX = 0x1913B40;
        public const nint SHADER_COLOR = 0x192AE08;
        public const nint SHADER_COLOR_SET = 0x192AE18;

        public const nint G_TESSELLATOR = 0x1925550;
        public const nint G_SKYMATRIXSTACK = 0x192AED0;
        public const nint G_SKYCOLOUR = 0x192AE08;

        public const nint MINECRAFTGAME_GETSCREENNAME = 0x138A70;

        public const nint MCE_RENDERCONTEXT_CREATEDEPTHSTATE = 0x726EF0;
        public const nint MCE_RENDERCONTEXT_APPLYDEPTHSTATE = 0x7271B0;

        public const nint LEVELRENDERERCAMERA_RENDERLEVEL = 0x5B07D0;

        public const nint ENTITY_TURN = 0x9C2020;
        public const nint LEVELRENDERERPLAYER_MOVECAMERATOPLAYER = 0x5BA7E0;

        public const nint OPTIONS_GETPLAYERVIEWPERSPECTIVE = 0x488420;
        public const nint OPTIONS_SETPLAYERVIEWPERSPECTIVE = 0x4883A0;
        public const nint MINECRAFTGAME_GETOPTIONS = 0x137BD0;

        public const nint G_CAMERA_POS = 0x19436F8;
        public const nint ENTITYRENDERDISPATCHER_RENDER = 0x55D640;
        public const nint TESSELLATOR_VERTEX = 0x5D0A90;
        public const nint TESSELLATOR_END = 0x5D14F0;

        public const nint G_RENDER_ORIGIN = 0x19436E8;
    }

    public static class DATA
    {
        public const nint TEXTUREGROUP_REGISTRY = 0x28;

        public const nint LOCALPLAYER_VTABLE = 0x1724CD8;
        public const nint PLAYER_VTABLE = 0x1766348;
    }

    public static class FIELD
    {
        public const nint MINECRAFTGAME_GUIDATA = 0x170;
        public const nint MINECRAFTGAME_FONT = 0x88;

        public const nint LEVELRENDERERCAMERA_FOGCOLOUR = 0x3C8;
        public const nint LEVELRENDERERCAMERA_SUNMATERIAL = 0x308;

        public const nint CLIENTINSTANCE_TEXTURECONTAINER = 0x30;
        public const nint CLIENTINSTANCE_TEXTUREGROUP = 0x80;
        public const nint CLIENTINSTANCE_CAMERATARGET = 0x50;
        public const nint CLIENTINSTANCE_LOCALPLAYER = 0x60;

        public const nint RAKNETNETWORKPEER_LAST_PING = 232;

        public const nint ENTITY_POS_X = 0x88;
        public const nint ENTITY_POS_Y = 0x8C;
        public const nint ENTITY_POS_Z = 0x90;
        public const nint ENTITY_POS_OLD_X = 0x94;
        public const nint ENTITY_POS_OLD_Y = 0x98;
        public const nint ENTITY_POS_OLD_Z = 0x9C;

        public const nint ENTITY_ROT_YAW = 0xB8;
        public const nint ENTITY_ROT_PITCH = 0xBC;
        public const nint ENTITY_ROT_OLD_YAW = 0xC0;
        public const nint ENTITY_ROT_OLD_PITCH = 0xC4;
    }
}