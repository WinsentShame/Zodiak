namespace Zodiak;

public static class gui
{
    public static bool Visible = true;

    private static float _x = 200f, _y = 100f;
    private const float W = 300f;
    private const float H = 170f;
    private const float TITLE_H = 14f;
    private const float SIDEBAR_W = 78f;
    private const float ROW_H = 26f;
    private const float ROW_PAD = 3f;
    private const float PAD = 6f;

    private static int _tab = 0;
    private static bool _dragging = false;
    private static float _dragDx, _dragDy;
    private static bool _prevInsert = false;
    private static bool _prevMouse = false;

    private static readonly string[] _tabNames = { "Combat", "Player", "Movement", "Misc" };
    private static readonly string[] _tabIcons = { "C", "P", "M", "X" };

    private const int VK_INSERT = 0x2D;
    private const int VK_LBUTTON = 0x01;

    private static readonly color4 _cBg = new(0.04f, 0.04f, 0.06f, 0.97f);
    private static readonly color4 _cTitle = new(0.06f, 0.06f, 0.09f, 0.98f);
    private static readonly color4 _cSidebar = new(0.02f, 0.02f, 0.04f, 0.98f);
    private static readonly color4 _cRowBg = new(0.07f, 0.07f, 0.10f, 1.00f);
    private static readonly color4 _cRowHover = new(0.09f, 0.09f, 0.13f, 1.00f);
    private static readonly color4 _cRowOn = new(0.55f, 0.12f, 0.20f, 1.00f);
    private static readonly color4 _cWhite = new(0.95f, 0.95f, 0.98f, 1.00f);
    private static readonly color4 _cDim = new(0.55f, 0.55f, 0.60f, 1.00f);
    private static readonly color4 _cFaint = new(0.35f, 0.35f, 0.40f, 1.00f);
    private static readonly color4 _cAccent = new(1.00f, 0.20f, 0.30f, 1.00f);
    private static readonly color4 _cToggleBg = new(0.15f, 0.15f, 0.20f, 1.00f);
    private static readonly color4 _cToggleOn = new(1.00f, 0.20f, 0.30f, 1.00f);
    private static readonly color4 _cKnob = new(0.95f, 0.95f, 0.98f, 1.00f);
    private static readonly color4 _cTabText = new(0.55f, 0.55f, 0.60f, 1.00f);
    private static readonly color4 _cTabActive = new(0.10f, 0.08f, 0.14f, 1.00f);
    private static readonly color4 _cTabHover = new(0.07f, 0.07f, 0.10f, 1.00f);

    public static bool Install()
    {
        overlay.Register(Render);
        logger.info("gui", "installed");
        return true;
    }

    private static int RowsInTab()
    {
        var modules = module_manager.all;
        int n = 0;
        var currentTab = (category)_tab;
        for (int i = 0; i < modules.Count; i++)
            if (modules[i].category == currentTab) n++;
        return n;
    }

    private static void Update()
    {
        if (!draw.Ready) return;

        bool insert = (native_interop.GetAsyncKeyState(VK_INSERT) & 0x8000) != 0;
        if (insert && !_prevInsert)
        {
            Visible = !Visible;
            logger.info("gui", Visible ? "shown" : "hidden");
        }
        _prevInsert = insert;

        if (!Visible) { _dragging = false; _prevMouse = false; return; }

        draw.CursorLogical(out float mx, out float my);

        bool mouseDown = (native_interop.GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
        bool mousePressed = mouseDown && !_prevMouse;

        if (mousePressed) OnClick(mx, my);
        else if (mouseDown && _dragging) { _x = mx - _dragDx; _y = my - _dragDy; }
        else if (!mouseDown) _dragging = false;

        _prevMouse = mouseDown;
    }

    private static void OnClick(float mx, float my)
    {
        float sideTop = _y + TITLE_H;

        if (mx >= _x && mx <= _x + SIDEBAR_W && my >= sideTop && my <= _y + H)
        {
            float tabStart = sideTop + 12f;
            int t = (int)((my - tabStart) / 26f);
            if (t >= 0 && t < _tabNames.Length && my >= tabStart + t * 26f && my <= tabStart + (t + 1) * 26f)
            {
                _tab = t;
                return;
            }
        }

        if (mx >= _x + SIDEBAR_W && mx <= _x + W && my >= sideTop && my <= _y + H)
        {
            float contentX = _x + SIDEBAR_W + PAD;
            float contentW = W - SIDEBAR_W - PAD * 2f;
            float rowStart = sideTop + 20f;
            var modules = module_manager.all;
            var currentTab = (category)_tab;
            int idx = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                var m = modules[i];
                if (m.category != currentTab) continue;

                float ry0 = rowStart + idx * (ROW_H + ROW_PAD);
                idx++;

                if (my >= ry0 && my <= ry0 + ROW_H && mx >= contentX && mx <= contentX + contentW)
                {
                    m.enabled = !m.enabled;
                    return;
                }
            }
        }

        if (mx >= _x && mx <= _x + W && my >= _y && my <= _y + TITLE_H)
        {
            _dragging = true;
            _dragDx = mx - _x;
            _dragDy = my - _y;
        }
    }

    private static void Render(nint minecraftGame)
    {
        Update();

        if (!Visible || !draw.Ready || minecraftGame == 0) return;

        nint font = draw.FontOf(minecraftGame);
        if (font == 0) return;

        draw.CursorLogical(out float mx, out float my);

        draw.Rect(_x, _y, _x + W, _y + H, _cBg);

        draw.Rect(_x, _y, _x + W, _y + TITLE_H, _cTitle);
        draw.Rect(_x, _y + TITLE_H - 1f, _x + W, _y + TITLE_H, _cFaint);

        draw.Text(font, "Zodiak", _x + 5f, _y + 3f, _cAccent);

        draw.Rect(_x, _y + TITLE_H, _x + SIDEBAR_W, _y + H, _cSidebar);
        draw.Rect(_x + SIDEBAR_W - 1f, _y + TITLE_H, _x + SIDEBAR_W, _y + H, _cFaint);

        float sideTop = _y + TITLE_H;
        float tabStart = sideTop + 12f;
        var currentTab = (category)_tab;

        for (int i = 0; i < _tabNames.Length; i++)
        {
            float ty0 = tabStart + i * 26f;
            float ty1 = ty0 + 24f;
            bool active = i == _tab;

            bool hover = mx >= _x && mx <= _x + SIDEBAR_W && my >= ty0 && my <= ty1;

            if (active)
                draw.Rect(_x + 4f, ty0, _x + SIDEBAR_W - 4f, ty1, _cTabActive);
            else if (hover)
                draw.Rect(_x + 4f, ty0, _x + SIDEBAR_W - 4f, ty1, _cTabHover);

            if (active)
                draw.Rect(_x + 4f, ty0 + 5f, _x + 7f, ty1 - 5f, _cAccent);

            float tx = _x + 16f;
            float textY = ty0 + (24f - 10f) * 0.5f;

            draw.Text(font, _tabIcons[i], tx, textY, active ? _cAccent : _cFaint);
            draw.Text(font, _tabNames[i], tx + 12f, textY, active ? _cWhite : _cTabText);
        }

        int rowCount = RowsInTab();
        if (rowCount == 0)
        {
            draw.Text(font, "no modules", _x + SIDEBAR_W + PAD + 12f, sideTop + 30f, _cFaint);
            return;
        }

        float contentX = _x + SIDEBAR_W + PAD;
        float contentW = W - SIDEBAR_W - PAD * 2f;
        float rowStart = sideTop + 20f;

        var modules = module_manager.all;
        int idx = 0;
        for (int i = 0; i < modules.Count; i++)
        {
            var m = modules[i];
            if (m.category != currentTab) continue;

            float ry0 = rowStart + idx * (ROW_H + ROW_PAD);
            float ry1 = ry0 + ROW_H;
            idx++;

            bool hover = mx >= contentX && mx <= contentX + contentW && my >= ry0 && my <= ry1;

            color4 rowCol;
            if (m.enabled) rowCol = _cRowOn;
            else if (hover) rowCol = _cRowHover;
            else rowCol = _cRowBg;

            draw.Rect(contentX, ry0, contentX + contentW, ry1, rowCol);

            var descCol = m.enabled ? new color4(0.90f, 0.85f, 0.88f, 1f) : _cDim;

            draw.Text(font, m.name, contentX + 8f, ry0 + 3f, _cWhite);
            draw.Text(font, m.description, contentX + 8f, ry0 + 14f, descCol);

            float tgW = 24f;
            float tgH = 12f;
            float tgX = contentX + contentW - tgW - 8f;
            float tgY = ry0 + (ROW_H - tgH) * 0.5f;

            draw.Rect(tgX, tgY, tgX + tgW, tgY + tgH,
                m.enabled ? _cToggleOn : _cToggleBg);

            float knobW = tgH - 4f;
            float knobX = m.enabled ? tgX + tgW - knobW - 2f : tgX + 2f;
            draw.Rect(knobX, tgY + 2f, knobX + knobW, tgY + 2f + knobW, _cKnob);
        }

        draw.Text(font, $"INSERT  |  {overlay.Fps:F0} fps",
            _x + SIDEBAR_W + PAD + 8f, _y + H - 12f, _cFaint);
    }
}