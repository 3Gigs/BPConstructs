using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using log4net;
using Terraria.GameInput;

namespace BPConstructs.Contents
{
    internal class UIPanelPlus : UIPanel
    {
        public Vector2 position;
        public Vector2 offset;

        public UIPanelPlus()
        {
            position = new Vector2(base.Left.Pixels, base.Top.Pixels);
            offset = new Vector2(base.Width.Pixels, base.Height.Pixels);
        }

        public virtual bool IsMouseInside()
        {
            if (Main.mouseX > Main.screenWidth || Main.mouseX < 0 || Main.mouseY > Main.screenHeight || Main.mouseY < 0)
                return false;

            Vector2 vector = position;

            return (float)Main.mouseX >= vector.X && (float)Main.mouseX <= vector.X + base.Width.Pixels && (float)Main.mouseY >= vector.Y && (float)Main.mouseY <= vector.Y + base.Height.Pixels;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();
            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
