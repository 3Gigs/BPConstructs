using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents
{
    internal class BPPaginatedGrid<T> : UIPanelPlus where T : UIElement
    {
        List<T> _elements;
        UIGrid _grid;
        float availableWidth, availibleHeight;
        readonly int maxPanels = 7;

        public BPPaginatedGrid()
        {
            Width = new StyleDimension(0f, 1f);
            Height = new StyleDimension(0f, 1f);
            _elements = new List<T>();
            _grid = new UIGrid()
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f)
            };
            availableWidth = base.GetInnerDimensions().Width;
            availibleHeight = base.GetInnerDimensions().Height;
            Append(_grid);
        }

        public void AddElement(T element)
        {
            _elements.Add(element);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            _grid.Clear();
            for (int i = 0; i < maxPanels; i++)
            {
                if (i < _elements.Count)
                {
                    _grid.Add(_elements[i]);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
