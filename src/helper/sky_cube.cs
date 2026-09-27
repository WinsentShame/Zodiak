namespace Zodiak;

public static class sky_cube
{
    public const int FACE_COUNT = 6;
    public const int FACE_SIZE = 0x58;
    public const float PACK_CUBE_SCALE = 800.0f;

    public static readonly float[,] NORMALS = new float[6, 3]
    {
        { 0, 0, -1 }, { 1, 0, 0 }, { 0, 0, 1 },
        { -1, 0, 0 }, { 0, 1, 0 }, { 0, -1, 0 }
    };

    public static readonly float[,] RIGHTS = new float[6, 3]
    {
        { 1, 0, 0 }, { 0, 0, 1 }, { -1, 0, 0 },
        { 0, 0, -1 }, { 1, 0, 0 }, { 1, 0, 0 }
    };

    public static readonly float[,] UPS = new float[6, 3]
    {
        { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 },
        { 0, 1, 0 }, { 0, 0, 1 }, { 0, 0, -1 }
    };

    public static readonly float[] U = { 0f, 1f, 1f, 0f };
    public static readonly float[] V = { 0f, 0f, 1f, 1f };

    public static void corner(int face, int idx, out float x, out float y, out float z)
    {
        float u_local = U[idx] * 2f - 1f;
        float v_local = 1f - V[idx] * 2f;
        x = NORMALS[face, 0] + RIGHTS[face, 0] * u_local + UPS[face, 0] * v_local;
        y = NORMALS[face, 1] + RIGHTS[face, 1] * u_local + UPS[face, 1] * v_local;
        z = NORMALS[face, 2] + RIGHTS[face, 2] * u_local + UPS[face, 2] * v_local;
    }
}