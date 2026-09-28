namespace Zodiak;

public static unsafe class entity_check
{
    public const nint ENTITY_TYPE_ID = 0x14C;
    public const nint ENTITY_NAME = 0x1258;
    public const int ROTATION_SIZE = 16;

    public static bool is_player(nint entity)
    {
        if (entity == 0) return false;
        if (!memory.is_readable(entity + ENTITY_TYPE_ID, 4)) return false;

        int type = *(int*)(entity + ENTITY_TYPE_ID);
        return type == (int)entity_id.Player;
    }

    public static bool is_type(nint entity, entity_id id)
    {
        if (entity == 0) return false;
        if (!memory.is_readable(entity + ENTITY_TYPE_ID, 4)) return false;

        return *(int*)(entity + ENTITY_TYPE_ID) == (int)id;
    }

    public static entity_id get_type(nint entity)
    {
        if (entity == 0) return entity_id.Unknown;
        if (!memory.is_readable(entity + ENTITY_TYPE_ID, 4)) return entity_id.Unknown;

        return (entity_id)(*(int*)(entity + ENTITY_TYPE_ID));
    }

    public static string? get_name(nint entity)
    {
        if (entity == 0) return null;
        if (!memory.is_readable(entity + ENTITY_NAME, msvc_string.SIZE)) return null;

        return msvc_string.read(entity + ENTITY_NAME);
    }

    public static bool is_real_player(nint entity)
    {
        if (!is_player(entity)) return false;

        string? name = get_name(entity);
        if (string.IsNullOrEmpty(name)) return false;

        return true;
    }

    public static bool is_valid(nint entity)
        => entity != 0 && memory.is_readable(entity + OFFSETS.FIELD.ENTITY_ROT_YAW, ROTATION_SIZE);

    public static float get_yaw(nint entity)
        => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_YAW);

    public static float get_pitch(nint entity)
        => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_PITCH);

    public static float get_old_yaw(nint entity)
        => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_YAW);

    public static float get_old_pitch(nint entity)
        => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_PITCH);

    public static void write_rotation(nint entity, float yaw, float pitch)
    {
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_YAW) = yaw;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_PITCH) = pitch;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_YAW) = yaw;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_PITCH) = pitch;
    }
}
public enum entity_id
{
    Unknown = 0,
    ExplodingDynamite = 2,
    Chicken = 5,
    Cow = 6,
    MushroomCow = 7,
    Pig = 8,
    Sheep = 9,
    Bat = 10,
    Wolf = 11,
    EnderDragon = 12,
    PolarBear = 13,
    Village = 14,
    Zombie = 16,
    ZombiePig = 17,
    Ghast = 19,
    Blaze = 20,
    Skeleton = 21,
    Silverfish = 23,
    Creeper = 24,
    Enderman = 26,
    Arrow = 27,
    ShulkerBullet = 28,
    Bobber = 29,
    Player = 30,
    Egg = 31,
    Snowball = 32,
    EnderPearl = 33,
    Vial = 34,
    ScatteringBubble = 35,
    Boat = 39,
    Octopus = 40,
    Fireball = 41,
    MiniFireball = 42,
    DragonFireball = 43,
    Nan = 44,
    ZombieVillage = 45,
    Exp = 46,
    Lightning = 47,
    IronGolem = 48,
    Ocelot = 49,
    Snowman = 50,
    Shulker = 51,
    ExpBottle = 52,
    Rabbit = 53,
    Witch = 54,
    Llama = 56,
    Cameraman = 57,
    Nan2 = 60,
    AncientGuardian = 62,
    UnknownWanderer = 63,
    Wither = 65,
    WitherShell = 66,
    DesertZombie = 68,
    Stray = 69,
    Skeleton1 = 70,
    EyeEnder = 71,
    EnderCrystal = 72,
    Endermite = 73,
    Evoker = 74,
    Vex = 76,
    Vindicator = 77
}