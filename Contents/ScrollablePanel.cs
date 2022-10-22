using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using log4net;

// holy shit terraria
namespace BPConstructs.Contents
{
    internal class ScrollablePanel : UIPanelPlus
    {
        UIScrollbar _scrollbar;
        bool _isScrollbarAttached;
        static UIList _bpList;

        public ScrollablePanel()
        {
            _bpList = new UIList
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(-30f, 1f),
            };
            this.Append(_bpList);
            _scrollbar = new UIScrollbar
            {
                Height = new StyleDimension(0f, 1f),
                Top = new StyleDimension(0f, 0f),
                VAlign = 1f,
                HAlign = 1f,
            };
            _scrollbar.Left.Set(0f, 0);
            _scrollbar.SetView(100f, 1000f);
            _scrollbar.Height.Set(-1f, 1f);
            this.Append(_scrollbar);
            _bpList.SetScrollbar(_scrollbar);
        }

        public static bool AddBlueprint(string name, Tile[,] blueprint)
        {
            LogManager.GetLogger("BPConstructs").Info("AddBlueprint was called");
            // LogManager.GetLogger("BPConstructs").Info("tiles: " + String.Join(Environment.NewLine, tiles));

            BPContainerPanel itemPanel = new BPContainerPanel(blueprint);
            _bpList.Add(itemPanel);
            return true;
        }
    }
}
