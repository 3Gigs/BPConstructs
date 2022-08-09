using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using BPConstructs.Contents;

namespace BPConstructs.Utils
{
    internal class DraggablePanel : UIPanelPlus
    {
        public bool isDragging;
        private Vector2 offset;

        public DraggablePanel()
        {
            isDragging = false;
            offset = new Vector2(0, 0);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            DragStart(evt);
            base.MouseDown(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            DragStop(evt);
            base.MouseUp(evt);
        }

        void DragStart(UIMouseEvent evt)
        {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            isDragging = true;
        }
        void DragStop(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            base.Left.Set(end.X - offset.X, 0f);
            base.Top.Set(end.Y - offset.Y, 0f);
            isDragging = false;

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;

            if (isDragging)
            {
                Left = new StyleDimension(Main.mouseX - offset.X, 0f);
                Top = new StyleDimension(Main.mouseY - offset.Y, 0f);

                Recalculate();
            }

            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Terraria.Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Terraria.Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }
}
