using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using log4net;

namespace BPConstructs.Contents
{
    internal class BPContainerPanel : UIPanel
    {
        Tile[,] _tiles;
        ThumbnailImage thumbnail;

        public BPContainerPanel(Tile[,] tiles)
        {
            Width.Set(100f, 0f);
            Height.Set(100f, 0f);
            _tiles = tiles;
            thumbnail = new ThumbnailImage(tiles, new Vector2(100, 100));
            this.Append(thumbnail);
        }

        public Tile[,] tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            LogManager.GetLogger("BPConstructs").Info("Left Pixels: " + base.Left.Pixels);

            base.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
