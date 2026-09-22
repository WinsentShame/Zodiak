using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class sky_cubemap
{
    private const int face_count = 6;
    private const int face_size = 0x58;
    private const float pack_cube_scale = 800.0f;

    private static nint faces;
    private static nint group;
    private static bool ready;

    public static string STATUS { get; private set; } = "not loaded";

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
        if (ready) return true;

        nint texture_group = get_texture_group();
        if (texture_group == 0)
        {
            STATUS = "Waiting texture group";
            return false;
        }

        release_faces();

        faces = Marshal.AllocHGlobal(face_count * face_size);

        nint base_addr = native_interop.get_module_handle_w(null);
        var ctor = (delegate* unmanaged[Stdcall]<nint, nint, nint, int, nint>)
            (base_addr + OFFSETS.FUNC.TEXTUREPTR_CTOR);

        for (int face = 0; face < face_count; face++)
        {
            nint location = Marshal.AllocHGlobal(0x48);
            for (int i = 0; i < 0x48; i++) *(byte*)(location + i) = 0;

            msvc_string.write(location + 0x00, $"textures/environment/overworld_cubemap/cubemap_{face}");
            *(int*)(location + 0x20) = 0;
            msvc_string.write(location + 0x28, "");

            nint slot = faces + face_size * face;
            ctor(slot, texture_group, location, 0);

            msvc_string.free(location + 0x28);
            msvc_string.free(location + 0x00);
            Marshal.FreeHGlobal(location);
        }

        group = texture_group;
        ready = true;
        STATUS = "ready";
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
        STATUS = "not loaded";
    }

    public static bool faces_valid()
    {
        if (!ready || faces == 0) return false;
        for (int face = 0; face < face_count; face++)
        {
            nint slot = faces + face_size * face;
            nint group_ptr = *(nint*)slot;
            if (group_ptr == 0) return false;
        }
        return true;
    }

    private static void release_faces()
    {
        if (faces == 0) return;

        nint base_addr = native_interop.get_module_handle_w(null);
        var remove = (delegate* unmanaged[Stdcall]<nint, nint, void>)
            (base_addr + OFFSETS.FUNC.TEXTUREGROUP_REMOVEREF);

        for (int face = 0; face < face_count; face++)
        {
            nint slot = faces + face_size * face;
            nint group_ptr = *(nint*)slot;
            if (group_ptr == 0) continue;

            nint registry = group_ptr + OFFSETS.DATA.TEXTUREGROUP_REGISTRY;
            nint slot_holder = slot;
            remove(registry, (nint)(&slot_holder));
        }
    }

    private static nint get_texture_group()
    {
        nint client = context.CLIENT_INSTANCE;
        if (client == 0) return 0;

        nint slot = client + OFFSETS.FIELD.CLIENTINSTANCE_TEXTURECONTAINER;
        if (!memory.is_readable(slot, 8)) return 0;
        nint container = *(nint*)slot;
        if (container == 0) return 0;

        nint group_slot = container + OFFSETS.FIELD.CLIENTINSTANCE_TEXTUREGROUP;
        if (!memory.is_readable(group_slot, 8)) return 0;
        nint group_out = *(nint*)group_slot;
        return group_out;
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

        scale(matrix, pack_cube_scale, pack_cube_scale, pack_cube_scale);

        for (int face = 0; face < face_count; face++)
        {
            nint texture = faces + face_size * face;

            byte* state = (byte*)tess_address;
            state[0x170] = 0;
            state[0x125] = 0;

            begin(tess_address, 1, 8);
            colour(tess_address, 255, 255, 255, 255);

            for (int pass = 0; pass < 2; pass++)
            {
                for (int step = 0; step < 4; step++)
                {
                    int idx = pass == 0 ? step : 3 - step;
                    float x, y, z;
                    corner(face, idx, out x, out y, out z);
                    vertex_uv(tess_address, x, y, z, u[idx], v[idx]);
                }
            }

            draw2(tess_address, material, texture);
        }

        byte* stack = (byte*)stack_address;
        stack[0x18] = 1;
        *(nint*)(stack + 8) -= 0x40;
    }

    private static void corner(int face, int idx, out float x, out float y, out float z)
    {
        float u_local = u[idx] * 2f - 1f;
        float v_local = 1f - v[idx] * 2f;
        x = normals[face, 0] + rights[face, 0] * u_local + ups[face, 0] * v_local;
        y = normals[face, 1] + rights[face, 1] * u_local + ups[face, 1] * v_local;
        z = normals[face, 2] + rights[face, 2] * u_local + ups[face, 2] * v_local;
    }
}