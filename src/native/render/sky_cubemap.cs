using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class sky_cubemap
{
    private static nint faces;
    private static nint group;
    private static bool ready;

    public static string STATUS { get; private set; } = lang_manager.get("skycubemap.render.default.status");

    public static bool load()
    {
        if (ready) return true;

        nint texture_group = get_texture_group();
        if (texture_group == 0)
        {
            STATUS = "waiting for texture group";
            return false;
        }

        release_faces();

        faces = Marshal.AllocHGlobal(sky_cube.FACE_COUNT * sky_cube.FACE_SIZE);
        if (faces == 0)
        {
            STATUS = "alloc failed";
            return false;
        }

        nint ba = native_interop.get_module_handle_w(null);
        var ctor = (delegate* unmanaged[Stdcall]<nint, nint, nint, int, nint>)
            (ba + OFFSETS.FUNC.TEXTUREPTR_CTOR);

        for (int face = 0; face < sky_cube.FACE_COUNT; face++)
        {
            nint location = Marshal.AllocHGlobal(0x48);
            for (int i = 0; i < 0x48; i++) *(byte*)(location + i) = 0;

            msvc_string.write(location + 0x00, $"textures/environment/overworld_cubemap/cubemap_{face}");
            *(int*)(location + 0x20) = 0;
            msvc_string.write(location + 0x28, "");

            nint slot = faces + sky_cube.FACE_SIZE * face;
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
        for (int face = 0; face < sky_cube.FACE_COUNT; face++)
        {
            nint slot = faces + sky_cube.FACE_SIZE * face;
            if (*(nint*)slot == 0) return false;
        }
        return true;
    }

    private static void release_faces()
    {
        if (faces == 0) return;

        nint ba = native_interop.get_module_handle_w(null);
        var remove = (delegate* unmanaged[Stdcall]<nint, nint, void>)
            (ba + OFFSETS.FUNC.TEXTUREGROUP_REMOVEREF);

        for (int face = 0; face < sky_cube.FACE_COUNT; face++)
        {
            nint slot = faces + sky_cube.FACE_SIZE * face;
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

        nint slot = client + OFFSETS.FIELD.CLIENT_INSTANCE_TEXTURE_CONTAINER;
        if (!memory.is_readable(slot, 8)) return 0;
        nint container = *(nint*)slot;
        if (container == 0) return 0;

        nint group_slot = container + OFFSETS.FIELD.CLIENT_INSTANCE_TEXTURE_GROUP;
        if (!memory.is_readable(group_slot, 8)) return 0;
        return *(nint*)group_slot;
    }

    public static void draw(nint camera)
    {
        if (!ready || camera == 0) return;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return;

        if (!tessellator.resolve() || !tessellator.valid()) return;

        nint material = camera + OFFSETS.FIELD.LEVEL_RENDERER_CAMERA_SUN_MATERIAL;
        nint stack_address = base_address + OFFSETS.FUNC.G_SKYMATRIXSTACK;

        if (!memory.is_readable(stack_address, 0x20)) return;

        var push = (matrixstack_push_sig)(base_address + OFFSETS.FUNC.MATRIXSTACK_PUSH);
        var scale = (matrix_scale_sig)(base_address + OFFSETS.FUNC.MATRIX_SCALE);

        byte* guard = stackalloc byte[16];
        for (int i = 0; i < 16; i++) guard[i] = 0;

        push(stack_address, (nint)guard);

        nint matrix = *(nint*)(guard + 8);
        if (matrix == 0) return;

        scale(matrix, sky_cube.PACK_CUBE_SCALE, sky_cube.PACK_CUBE_SCALE, sky_cube.PACK_CUBE_SCALE);

        for (int face = 0; face < sky_cube.FACE_COUNT; face++)
        {
            nint texture = faces + sky_cube.FACE_SIZE * face;

            tessellator.reset_state();

            tessellator.begin(1, 8);
            tessellator.colour(255, 255, 255, 255);

            for (int pass = 0; pass < 2; pass++)
            {
                for (int step = 0; step < 4; step++)
                {
                    int idx = pass == 0 ? step : 3 - step;
                    sky_cube.corner(face, idx, out float x, out float y, out float z);
                    tessellator.vertex_uv(x, y, z, sky_cube.U[idx], sky_cube.V[idx]);
                }
            }

            tessellator.draw2(material, texture);
        }

        byte* stack = (byte*)stack_address;
        stack[0x18] = 1;
        *(nint*)(stack + 8) -= 0x40;
    }
}