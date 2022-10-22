using Terraria;
using Terraria.GameContent.UI.Elements;

namespace BPConstructs.Contents
{
    internal class ScrollablePanel : UIPanelPlus
    {
        UIScrollbar _scrollbar;
        bool _isScrollbarAttached;

        public ScrollablePanel()
        {
            _scrollbar = new UIScrollbar();
            _scrollbar.Left.Set(440, 0);
            this.Append(_scrollbar);
        }
    }
}
