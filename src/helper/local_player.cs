namespace Zodiak;

public static unsafe class local_player
{
    public static nint get()
    {
        nint client = context.CLIENT_INSTANCE;
        if (client == 0) return 0;

        nint player = *(nint*)(client + OFFSETS.FIELD.CLIENT_INSTANCE_CAMERA_TARGET);
        if (player == 0)
            player = *(nint*)(client + OFFSETS.FIELD.CLIENT_INSTANCE_LOCAL_PLAYER);
        return player;
    }

    public static bool is_valid(nint player)
        => player != 0 && memory.is_readable(player + OFFSETS.FIELD.ENTITY_ROT_OLD_PITCH, 4);
}