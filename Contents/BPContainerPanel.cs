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

        // private void CreateThumbnail(Tile[,] tiles, Vector2 startPos, SpriteBatch sb)
        // {
        //     int thumbnailWidth = 100;
        //     int thumbnailHeight = 100;
        //     int tileWidth = tiles.GetLength(0);
        //     int tileHeight = tiles.GetLength(1);
        //     float scale = 1.0F;
        //     Vector2 offset = new Vector2();

        //     if (tileWidth > thumbnailWidth || tileHeight > thumbnailHeight)
        //     {
        //         if (tileHeight > tileWidth)
        //         {
        //             scale = (float)thumbnailWidth / tileHeight;
        //             offset.X = (thumbnailWidth - tileWidth * scale);
        //         }
        //         else
        //         {
        //             scale = (float)thumbnailWidth / tileWidth;
        //             offset.Y = (thumbnailHeight - tileHeight * scale) / 2;
        //         }
        //     }

        //     CopyModeUI.DrawPreview(sb, tiles, startPos, 0.2f);
        // }
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
