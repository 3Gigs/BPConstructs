using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader.UI.Elements;
using log4net;

// holy shit terraria
namespace BPConstructs.Contents
{
    internal class ScrollablePanel : UIPanel
    {
        UIScrollbar _scrollbar;
        bool _isScrollbarAttached;
        static UIGrid _bpList;

        public ScrollablePanel()
        {
            _bpList = new UIGrid
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                VAlign = 1,
                HAlign = 0,
            };
            this.Append(_bpList);
            _scrollbar = new UIScrollbar
            {
                Height = new StyleDimension(0f, 0.825f),
                Top = new StyleDimension(0f, 0f),
                VAlign = 0f,
                HAlign = 1f,
            };
            _scrollbar.SetView(100f, 1000f);
            _scrollbar.Left.Set(0f, 0);
            _scrollbar.Height.Set(-1f, 1f);
            _bpList.SetScrollbar(_scrollbar);
            this.Append(_scrollbar);
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
