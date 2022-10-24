using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents
{
    public class UIImagePlus : UIImage
    {
        public UIImagePlus(Texture2D texture) : base(texture) { }

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
