namespace Zodiak;

public static unsafe class entity_rotation
{
    public const int SIZE = 16;

    public static bool is_valid(nint entity)
        => entity != 0 && memory.is_readable(entity + OFFSETS.FIELD.ENTITY_ROT_YAW, SIZE);

    public static float get_yaw(nint entity) => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_YAW);
    public static float get_pitch(nint entity) => *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_PITCH);

    public static void write(nint entity, float yaw, float pitch)
    {
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_YAW) = yaw;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_PITCH) = pitch;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_YAW) = yaw;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_ROT_OLD_PITCH) = pitch;
    }
}