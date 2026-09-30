namespace Zodiak;

public static class OFFSETS
{
    public static class FUNC
    {
        public const nint CLIENT_INSTANCE_LEAVE_GAME = 0x119C80;
        public const nint CLIENT_INSTANCE_ON_TICK = 0x11B2D0;

        public const nint ENTITY_RENDER_DISPATCHER_RENDER = 0x55D640;
        public const nint ENTITY_TURN = 0x9C2020;

        public const nint FONT_DRAWCACHED = 0x1C8B60;

        public const nint G_RENDER_ORIGIN = 0x19436E8;
        public const nint G_SKYCOLOUR = 0x192AE08;
        public const nint G_SKYMATRIXSTACK = 0x192AED0;
        public const nint G_TESSELLATOR = 0x1925550;

        public const nint GUI_DATA_GUI_SCALE = 0x16EC0A4;
        public const nint GUI_DATA_DISPLAY_CLIENT_MESSAGE = 0x1CDD30;

        public const nint IN_GAME_PLAY_SCREEN_RENDER = 0x3528C0;

        public const nint LEVEL_RENDERER_CAMERA_RENDER_ENTITIES = 0x5AE250;
        public const nint LEVEL_RENDERER_CAMERA_RENDER_LEVEL = 0x5B07D0;
        public const nint LEVEL_RENDERER_CAMERA_RENDER_SKY = 0x5ACE40;
        public const nint LEVEL_RENDERER_CAMERA_RENDER_STARS = 0x5AD1D0;
        public const nint LEVEL_RENDERER_CAMERA_RENDER_SUN_OR_MOON = 0x5AD330;
        public const nint LEVEL_RENDERER_CAMERA_SETUP_FOG = 0x5AFE00;
        public const nint LEVEL_RENDERER_PLAYER_MOVE_CAMERA_TO_PLAYER = 0x5BA7E0;

        public const nint MATRIX_SCALE = 0x15D560;
        public const nint MATRIXSTACK_PUSH = 0x730000;

        public const nint MCE_RENDERCONTEXT_APPLYDEPTHSTATE = 0x7271B0;
        public const nint MCE_RENDERCONTEXT_CREATEDEPTHSTATE = 0x726EF0;

        public const nint MINECRAFT_GAME_GET_OPTIONS = 0x137BD0;
        public const nint MINECRAFT_GAME_GET_SCREEN_NAME = 0x138A70;
        public const nint MINECRAFT_GAME_UPDATE_GRAPHICS = 0x131D00;
        public const nint MINECRAFT_SCREEN_MODEL_SEND_CHAT_MESSAGE = 0x399C70;

        public const nint OPTIONS_GET_PLAYER_VIEW_PERSPECTIVE = 0x488420;
        public const nint OPTIONS_SET_PLAYER_VIEW_PERSPECTIVE = 0x4883A0;

        public const nint PLAYERRENDERER_RENDER = 0x581910;

        public const nint RAKNETNETWORKPEER_UPDATE = 0x7917B0;

        public const nint RENDER_CTX = 0x1913B40;

        public const nint SCREENRENDERER_FILL = 0x1DAF10;
        public const nint SCREENRENDERER_SINGLETON = 0x1D9620;

        public const nint SHADER_COLOR = 0x192AE08;
        public const nint SHADER_COLOR_SET = 0x192AE18;

        public const nint TESSELLATOR_BEGIN = 0x5D0660;
        public const nint TESSELLATOR_COLOUR = 0x5D0890;
        public const nint TESSELLATOR_DRAW = 0x5D1840;
        public const nint TESSELLATOR_DRAW2 = 0x5D19E0;
        public const nint TESSELLATOR_END = 0x5D14F0;
        public const nint TESSELLATOR_VERTEX = 0x5D0A90;
        public const nint TESSELLATOR_VERTEXUV = 0x5D0960;

        public const nint TEXTUREGROUP_REMOVEREF = 0x44C160;
        public const nint TEXTUREPTR_CTOR = 0x73F2B0;

        public const nint CHEST_RENDERER_RENDER = 0x53F750;
        public const nint BLOCK_SOURCE_GET_BLOCK_ID = 0xB73250;
    }

    public static class DATA
    {
        public const nint TEXTUREGROUP_REGISTRY = 0x28;
    }

    public static class FIELD
    {
        public const nint CLIENT_INSTANCE_CAMERA_TARGET = 0x50;
        public const nint CLIENT_INSTANCE_LOCAL_PLAYER = 0x60;
        public const nint CLIENT_INSTANCE_TEXTURE_CONTAINER = 0x30;
        public const nint CLIENT_INSTANCE_TEXTURE_GROUP = 0x80;

        public const nint ENTITY_POS_OLD_X = 0x94;
        public const nint ENTITY_POS_OLD_Y = 0x98;
        public const nint ENTITY_POS_OLD_Z = 0x9C;
        public const nint ENTITY_POS_X = 0x88;
        public const nint ENTITY_POS_Y = 0x8C;
        public const nint ENTITY_POS_Z = 0x90;

        public const nint ENTITY_ROT_OLD_PITCH = 0xC4;
        public const nint ENTITY_ROT_OLD_YAW = 0xC0;
        public const nint ENTITY_ROT_PITCH = 0xBC;
        public const nint ENTITY_ROT_YAW = 0xB8;

        public const nint ENTITY_HITBOX_WIDTH = 0x198;
        public const nint ENTITY_HITBOX_HEIGHT = 0x19C;

        public const nint LEVEL_RENDERER_CAMERA_FOG_COLOUR = 0x3C8;
        public const nint LEVEL_RENDERER_CAMERA_SUN_MATERIAL = 0x308;

        public const nint MINECRAFT_GAME_FONT = 0x88;
        public const nint MINECRAFT_GAME_GUIDATA = 0x170;
        public const nint MINECRAFT_GAME_SCREEN_WIDTH = 0x4C;
        public const nint MINECRAFT_GAME_SCREEN_HEIGHT = 0x50;

        public const nint RAKNET_NETWORK_PEER_LAST_PING = 232;
        public const nint ENTITY_BLOCK_SOURCE = 0xD8;
    }
}