using System; 
using Sokol;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Numerics;
using static Sokol.SApp;
using static Sokol.SG;
using static Sokol.SGlue;
using static Sokol.SG.sg_vertex_format;
using static Sokol.SG.sg_index_type;
using static Sokol.SG.sg_cull_mode;
using static Sokol.SG.sg_compare_func;
using static Sokol.Utils;
using System.Diagnostics;
using static Sokol.SLog;
using static Sokol.SDebugUI;
using Imgui;
using static Sokol.SImgui;
using static Imgui.ImguiNative;

public static unsafe class Sokolnettest2App
{
    struct _state
    {
        public sg_pass_action pass_action;
    }

    static _state state = new _state();



    //[UnmanagedCallersOnly]
    //private static unsafe void Init()
    //{
    //    const int max_vertices = 131072;
    //    sg_setup(new sg_desc { environment = sglue_environment() });
    //    simgui_setup(new simgui_desc_t { max_vertices = max_vertices, logger = { func = &slog_func } });//max_vertices = 262144 - It's requare 32-bit indices, so it's mean sokol should be compiled
    //    //multiply by 3 since https://github.com/floooh/sokol/blob/28f9d8d44d92dab8536791a9f7d13d7e911a2b39/util/sokol_imgui.h#L2401
    //    metrics.CollectRenderCapacityMetrics(max_vertices, max_vertices * 3);

    //    pass_action = default;
    //    pass_action.colors[0].load_action = sg_load_action.SG_LOADACTION_CLEAR;
    //    pass_action.colors[0].clear_value = new sg_color { r = 0.1f, g = 0.1f, b = 0.1f, a = 1.0f };

    //    inputSource = new InputSystem();
    //    ChangeActiveScene(AllScenes.Scenes[selectedSceneIdx]);
    //}

    [UnmanagedCallersOnly]
    private static unsafe void Init()
    {
        sg_setup(new sg_desc(){environment = sglue_environment(), logger = {func = &slog_func,}});
        simgui_setup(new simgui_desc_t{logger = {func = &slog_func}});
        state.pass_action = default;
        state.pass_action.colors[0].load_action = sg_load_action.SG_LOADACTION_CLEAR;
        state.pass_action.colors[0].clear_value = new sg_color { r = 0.25f, g = 0.5f, b = 0.75f, a = 1.0f };
    }


    [UnmanagedCallersOnly]
    private static unsafe void Frame()
    {
        double deltaTime = sapp_frame_duration();

        float g =  state.pass_action.colors[0].clear_value.g + 0.01f;
        state.pass_action.colors[0].clear_value.g = (g > 1.0f) ? 0.0f : g;


        float dpi = sapp_dpi_scale();
        float w = sapp_widthf() / dpi;
        float h = sapp_heightf() / dpi;

        simgui_new_frame(new simgui_frame_desc_t
        {
            width = sapp_width(),
            height = sapp_height(),
            delta_time = (float)deltaTime,
            dpi_scale = dpi,//may wrong for android, see https://github.com/elix22/Sokol.NET/blob/35fc8551403bed0c855c36d42375a707c61f3a37/examples/cimgui/Source/cimgui-app.cs#L105
        });
        sg_begin_pass(new sg_pass { action = state.pass_action, swapchain = sglue_swapchain() });

        byte showMetrics = 1;
        igSetNextWindowPos(new Vector2(20, 20), ImGuiCond.FirstUseEver, Vector2.Zero);
        igShowMetricsWindow(ref showMetrics);
        //byte showDemo = 1;
        //igShowDemoWindow(ref showDemo);

        igSetNextWindowPos(new Vector2(360, 20), ImGuiCond.FirstUseEver, Vector2.Zero);
        igSetNextWindowSize(new Vector2(300, 150), ImGuiCond.FirstUseEver);

        byte open = 1;
        if (igBegin("Debug PanelNew2", ref open, 0))
        {
            igText("Sokol.NET Web & Touch Test");
            igText($"DPI Scale: {dpi:F2}");
            igText($"Resolution: {sapp_width()}x{sapp_height()}");
            if (igButton("Reset Color Speed", Vector2.Zero))
            {
                state.pass_action.colors[0].clear_value.g = 0.0f;
            }
        }
        igEnd();

        simgui_render();

        sg_end_pass();
        sg_commit();
    }


    [UnmanagedCallersOnly]
    private static unsafe void Event(sapp_event* e)
    {
        simgui_handle_event(*e);

        ImGuiIO* io = igGetIO_Nil();
        bool hasMouse = io->WantCaptureMouse == 0;
        bool hasKeyboard = io->WantCaptureKeyboard == 0;

        static Vector2 getPos(float x, float y) => new Vector2(x, y) / sapp_dpi_scale();
        static Vector2 mousePos(sapp_event* e) => getPos(e->mouse_x, e->mouse_y);
        static Vector2 touchPos(sapp_event* e) => getPos(e->touches[0].pos_x, e->touches[0].pos_y);

        //MouseButton mouseBtn;
        //const MouseButton touchMouseButton = MouseButton.Left;

        switch (e->type)
        {
            case sapp_event_type.SAPP_EVENTTYPE_KEY_DOWN:
                //if (hasKeyboard)
                //inputSource.OnKeyDown(e->key_code.ToString());
                break;

            case sapp_event_type.SAPP_EVENTTYPE_KEY_UP:
                //if (hasKeyboard)
                //inputSource.OnKeyUp(e->key_code.ToString());
                break;

            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_DOWN:
                //if (hasMouse && ToBalanceMouseButton(e->mouse_button, out mouseBtn))
                //inputSource.OnMouseDown(mouseBtn, mousePos(e), activeScene.World, activeScene.View);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_MOVE:
                //if (hasMouse)
                //inputSource.OnMouseMove(mousePos(e), activeScene.World, activeScene.View);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_UP:
                //if (hasMouse && ToBalanceMouseButton(e->mouse_button, out var btn))
                //inputSource.OnMouseUp(btn, mousePos(e), activeScene.World, activeScene.View);
                break;

            case sapp_event_type.SAPP_EVENTTYPE_TOUCHES_BEGAN:
                //if (hasMouse && e->num_touches > 0)
                //inputSource.OnMouseDown(touchMouseButton, touchPos(e), activeScene.World, activeScene.View);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_TOUCHES_MOVED:
                //if (hasMouse && e->num_touches > 0)
                //inputSource.OnMouseMove(touchPos(e), activeScene.World, activeScene.View);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_TOUCHES_ENDED:
            case sapp_event_type.SAPP_EVENTTYPE_TOUCHES_CANCELLED:
                //if (hasMouse && e->num_touches > 0)
                //inputSource.OnMouseUp(touchMouseButton, touchPos(e), activeScene.World, activeScene.View);
                break;
        }
    }

    [UnmanagedCallersOnly]
    static void Cleanup()
    {
        sg_shutdown();

        // Force a complete shutdown if debugging
        if (Debugger.IsAttached)
        {
            Environment.Exit(0);
        }
    }

    public static SApp.sapp_desc sokol_main()
    {
        return new SApp.sapp_desc()
        {
            init_cb = &Init,
            frame_cb = &Frame,
            event_cb = &Event,
            cleanup_cb = &Cleanup,
            width = 0,
            height = 0,
            sample_count = 4,
            window_title = "Template (sokol-app)",
            high_dpi = true,
            icon = { sokol_default = true },
            logger = {
                func = &slog_func,
            }
        };
    }

}
