using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class sky_cubemap
{
    private const int FACE_COUNT = 6;
    private const int FACE_SIZE = 0x58;
    private const float PACK_CUBE_SCALE = 800.0f;

    private static nint faces;
    private static nint group;
    private static bool ready;
    private static bool failed;
    private static bool warned;
    private static bool drew;

    public static string status { get; private set; } = "not loaded";

    private static readonly float[,] normals = new float[6, 3]
    {
        { 0, 0, -1 }, { 1, 0, 0 }, { 0, 0, 1 },
        { -1, 0, 0 }, { 0, 1, 0 }, { 0, -1, 0 }
    };

    private static readonly float[,] rights = new float[6, 3]
    {
        { 1, 0, 0 }, { 0, 0, 1 }, { -1, 0, 0 },
        { 0, 0, -1 }, { 1, 0, 0 }, { 1, 0, 0 }
    };

    private static readonly float[,] ups = new float[6, 3]
    {
        { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 },
        { 0, 1, 0 }, { 0, 0, 1 }, { 0, 0, -1 }
    };

    private static readonly float[] u = { 0f, 1f, 1f, 0f };
    private static readonly float[] v = { 0f, 0f, 1f, 1f };

    public static bool load()
    {
        if (ready) { return true; }
        if (failed) { return false; }

        nint Group = get_texture_group();

        if (Group == 0)
        {
            status = "waiting for texture group";
            return false;
        }

        release_faces();

        faces = Marshal.AllocHGlobal(FACE_COUNT * FACE_SIZE);

        nint Base = native_interop.get_module_handle_w(null);
        var Ctor = (delegate* unmanaged[Stdcall]<nint, nint, nint, int, nint>)
            (Base + OFFSETS.FUNC.TEXTUREPTR_CTOR);

        for (int Face = 0; Face < FACE_COUNT; Face++)
        {

            nint Location = Marshal.AllocHGlobal(0x48);
            for (int i = 0; i < 0x48; i++) *(byte*)(Location + i) = 0;

            msvc_string.write(Location + 0x00, $"textures/environment/overworld_cubemap/cubemap_{Face}");
            *(int*)(Location + 0x20) = 0;
            msvc_string.write(Location + 0x28, "");

            nint Slot = faces + FACE_SIZE * Face;
            Ctor(Slot, Group, Location, 0);

            msvc_string.free(Location + 0x28);
            msvc_string.free(Location + 0x00);
            Marshal.FreeHGlobal(Location);
        }

        group = Group;
        ready = true;
        status = "ready";
        return true;
    }

    public static bool refresh()
    {
        if (ready && group == get_texture_group() && faces_valid())
            return true;
        unload();
        return load();
    }

    public static void unload()
    {
        release_faces();
        if (faces != 0) { Marshal.FreeHGlobal(faces); faces = 0; }
        ready = false;
        group = 0;
        status = "not loaded";
    }

    public static bool faces_valid()
    {
        if (!ready || faces == 0) return false;
        for (int Face = 0; Face < FACE_COUNT; Face++)
        {
            nint Slot = faces + FACE_SIZE * Face;
            nint Group = *(nint*)Slot;
            if (Group == 0) return false;
        }
        return true;
    }

    private static void release_faces()
    {
        if (faces == 0) return;

        nint base_add = native_interop.get_module_handle_w(null);
        var remove = (delegate* unmanaged[Stdcall]<nint, nint, void>)
            (base_add + OFFSETS.FUNC.TEXTUREGROUP_REMOVEREF);

        for (int Face = 0; Face < FACE_COUNT; Face++)
        {
            nint slot = faces + FACE_SIZE * Face;
            nint group_ptr = *(nint*)slot;
            if (group_ptr == 0) continue;

            nint registry = group_ptr + OFFSETS.DATA.TEXTUREGROUP_REGISTRY;
            nint slot_holder = slot;
            remove(registry, (nint)(&slot_holder));
        }
    }

    private static nint get_texture_group()
    {
        nint client = client_instance.Pointer;
        if (client == 0) return 0;

        nint slot = client + OFFSETS.FIELD.CLIENTINSTANCE_TEXTURECONTAINER;
        if (!memory.is_readable(slot, 8)) { return 0; }
        nint conatiner = *(nint*)slot;
        if (conatiner == 0) return 0;

        nint group_slot = conatiner + OFFSETS.FIELD.CLIENTINSTANCE_TEXTUREGROUP;
        if (!memory.is_readable(group_slot, 8)) { return 0; }
        nint group = *(nint*)group_slot;
        return group;
    }
    public static void draw(nint camera)
    {
        if (!ready || camera == 0) return;

        nint base_address = native_interop.get_module_handle_w(null);

        nint material = camera + OFFSETS.FIELD.LEVELRENDERERCAMERA_SUNMATERIAL;
        nint tess_address = base_address + OFFSETS.FUNC.G_TESSELLATOR;
        nint stack_address = base_address + OFFSETS.FUNC.G_SKYMATRIXSTACK;

        if (!memory.is_readable(tess_address, 0x140)) return;
        if (!memory.is_readable(stack_address, 0x20)) return;

        var push = (delegate* unmanaged[Stdcall]<nint, nint, void>)(base_address + OFFSETS.FUNC.MATRIXSTACK_PUSH);
        var scale = (delegate* unmanaged[Stdcall]<nint, float, float, float, void>)(base_address + OFFSETS.FUNC.MATRIX_SCALE);
        var begin = (delegate* unmanaged[Stdcall]<nint, byte, int, void>)(base_address + OFFSETS.FUNC.TESSELLATOR_BEGIN);
        var colour = (delegate* unmanaged[Stdcall]<nint, byte, byte, byte, byte, void>)(base_address + OFFSETS.FUNC.TESSELLATOR_COLOUR);
        var vertex_uv = (delegate* unmanaged[Stdcall]<nint, float, float, float, float, float, void>)(base_address + OFFSETS.FUNC.TESSELLATOR_VERTEXUV);
        var draw2 = (delegate* unmanaged[Stdcall]<nint, nint, nint, void>)(base_address + OFFSETS.FUNC.TESSELLATOR_DRAW2);

        byte* guard = stackalloc byte[16];
        for (int i = 0; i < 16; i++) guard[i] = 0;

        push(stack_address, (nint)guard);

        nint matrix = *(nint*)(guard + 8);
        if (matrix == 0) return;

        scale(matrix, PACK_CUBE_SCALE, PACK_CUBE_SCALE, PACK_CUBE_SCALE);

        for (int face = 0; face < FACE_COUNT; face++)
        {
            nint Texture = faces + FACE_SIZE * face;

            byte* state = (byte*)tess_address;
            state[0x170] = 0;
            state[0x125] = 0;

            begin(tess_address, 1, 8);
            colour(tess_address, 255, 255, 255, 255);

            for (int pass = 0; pass < 2; pass++)
            {
                for (int step = 0; step < 4; step++)
                {
                    int I = pass == 0 ? step : 3 - step;
                    float X, Y, Z;
                    corner(face, I, out X, out Y, out Z);
                    vertex_uv(tess_address, X, Y, Z, u[I], v[I]);
                }
            }

            draw2(tess_address, material, Texture);
        }

        byte* SB = (byte*)stack_address;
        SB[0x18] = 1;
        *(nint*)(SB + 8) -= 0x40;
    }
    private static void corner(int Face, int I, out float X, out float Y, out float Z)
    {
        float U = u[I] * 2f - 1f;
        float V = 1f - v[I] * 2f;
        X = normals[Face, 0] + rights[Face, 0] * U + ups[Face, 0] * V;
        Y = normals[Face, 1] + rights[Face, 1] * U + ups[Face, 1] * V;
        Z = normals[Face, 2] + rights[Face, 2] * U + ups[Face, 2] * V;
    }
}