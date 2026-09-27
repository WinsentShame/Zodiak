namespace Zodiak;

public static unsafe class wallhack_state
{
    public static bool ACTIVE;
    public static bool IN_ENTITY_PASS;
    public static bool CREATING_NO_DEPTH;
    public static bool NO_DEPTH_READY;
    public static bool SECOND_PASS;

    public static nint NO_DEPTH_STATE;
    public static nint CACHED_CTX;
    public static float SAVED_PARTIAL = 1.0f;

    public static create_depth_state_sig ORIGINAL_CREATE_DEPTH;

    public static void ensure_no_depth_state()
    {
        if (NO_DEPTH_READY) return;
        if (CACHED_CTX == 0) return;
        if (ORIGINAL_CREATE_DEPTH == null) return;

        CREATING_NO_DEPTH = true;
        try
        {
            byte* saved_cache = stackalloc byte[24];
            for (int i = 0; i < 24; i++) saved_cache[i] = *(byte*)(CACHED_CTX + 8 + i);
            byte saved_flag = *(byte*)(CACHED_CTX + 117);

            *(byte*)(CACHED_CTX + 117) = 1;

            byte* packet = stackalloc byte[20];
            for (int i = 0; i < 20; i++) packet[i] = 0;
            packet[0] = 1;
            packet[1] = 0;
            packet[2] = 2;
            packet[3] = 2;
            packet[4] = 1; packet[5] = 1; packet[6] = 1; packet[7] = 1;
            packet[8] = 1; packet[9] = 1; packet[10] = 1;
            packet[11] = 1;
            packet[12] = 0xFF;
            packet[16] = 0xFF;

            byte* state_buf = stackalloc byte[32];
            for (int i = 0; i < 32; i++) state_buf[i] = 0;

            ORIGINAL_CREATE_DEPTH((nint)state_buf, CACHED_CTX, (nint)packet);

            nint obj = *(nint*)(state_buf + 24);
            if (obj != 0)
            {
                NO_DEPTH_STATE = obj;
                NO_DEPTH_READY = true;
            }

            for (int i = 0; i < 24; i++) *(byte*)(CACHED_CTX + 8 + i) = saved_cache[i];
            *(byte*)(CACHED_CTX + 117) = saved_flag;
        }
        finally
        {
            CREATING_NO_DEPTH = false;
        }
    }
}