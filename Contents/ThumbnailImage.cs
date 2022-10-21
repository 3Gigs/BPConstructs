using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents
{
    public class ThumbnailImage : UIElement
    {
        public Tile[,] tiles;
        TexInfo[,] textures;
        Texture2D makeThumbnailTexture;
        Vector2 thumbSize;

        public ThumbnailImage(Tile[,] _tiles, Vector2 _thumbSize)
        {
            tiles = _tiles;
            textures = CreateThumbnail(_tiles);
            thumbSize = _thumbSize;
            makeThumbnailTexture = this.MakeThumbnail();
        }

        struct TexInfo
        {
            Texture2D _texture;
            Vector2? _wallFrame;
            Vector2? _tileFrame;

            public TexInfo(Texture2D texture, Vector2? wallFrame, Vector2? tileFrame)
            {
                _texture = texture;
                _wallFrame = wallFrame;
                _tileFrame = tileFrame;
            }

            public Texture2D texture
            {
                get { return _texture; }
            }

            public Vector2? wallFrame
            {
                get { return _wallFrame; }
            }

            public Vector2? tileFrame
            {
                get { return _tileFrame; }
            }
        }


        TexInfo[,] CreateThumbnail(Tile[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            TexInfo[,] textures = new TexInfo[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tile tile = tiles[x, y];

                    if (tile.HasTile)
                    {
                        Main.instance.LoadTiles(tile.TileType);
                        Texture2D texture = TextureAssets.Tile[tile.TileType].Value;
                        textures[x, y] = new TexInfo(texture, null, new Vector2(tile.TileFrameX, tile.TileFrameY));
                    }
                    else if (tile.WallType > 0)
                    {
                        Main.instance.LoadWall(tile.WallType);
                        Texture2D texture = TextureAssets.Wall[tile.WallType].Value;
                        int wallFrame = Main.wallFrame[tile.WallType] * 180;

                        textures[x, y] = new TexInfo(texture, new Vector2(tile.WallFrameX, tile.WallFrameY + wallFrame), null);

                    }
                }
            }

            return textures;
        }


        public Texture2D MakeThumbnail()
        {
            int desiredWidth = 100;
            int desiredHeight = 100;

            int actualWidth = textures.GetLength(0) * 16;
            int actualHeight = textures.GetLength(1) * 16;

            float scale = 1;
            Vector2 offset = new Vector2();
            if (actualWidth > desiredWidth || actualHeight > desiredHeight)
            {
                if (actualHeight > actualWidth)
                {
                    scale = (float)desiredWidth / actualHeight;
                    offset.X = (desiredWidth - actualWidth * scale) / 2;
                }
                else
                {
                    scale = (float)desiredWidth / actualWidth;
                    offset.Y = (desiredHeight - actualHeight * scale) / 2;
                }
            }
            offset = offset / scale;

            Main.spriteBatch.End();
            RenderTarget2D renderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, desiredWidth, desiredHeight);
            Main.instance.GraphicsDevice.SetRenderTarget(renderTarget);
            Main.instance.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin();

            CopyModeUI.DrawPreview(Main.spriteBatch, tiles, offset, scale);

            Main.spriteBatch.End();
            Main.instance.GraphicsDevice.SetRenderTarget(null);
            Main.spriteBatch.Begin();

            Texture2D mergedTexture = new Texture2D(Main.instance.GraphicsDevice, desiredWidth, desiredHeight);
            Color[] content = new Color[desiredWidth * desiredHeight];
            renderTarget.GetData<Color>(content);
            mergedTexture.SetData<Color>(content);
            return mergedTexture;
        }

        void DrawPreview(SpriteBatch sb, Vector2 startPos, float scale = 1f)
        {
            int width = textures.GetLength(0);
            int height = textures.GetLength(1);
            Color color = Color.White;
            color.A = 160;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    TexInfo texture = textures[x, y];
                    if (texture.wallFrame != null)
                    {
                        Rectangle value = new Rectangle((int)texture.wallFrame?.X, (int)texture.wallFrame?.Y, 32, 32);
                        Vector2 pos = startPos + new Vector2(x * 16, y * 16);
                        sb.Draw(texture.texture, pos * scale, value, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                    else if (texture.tileFrame != null)
                    {
                        Rectangle value = new Rectangle((int)texture.tileFrame?.X, (int)texture.tileFrame?.Y, 16, 16);
                        Vector2 pos = startPos + new Vector2(x * 16, y * 16);
                        sb.Draw(texture.texture, pos * scale, value, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                }
            sb.End();
            sb.Begin();
        }

        public override void Draw(SpriteBatch sb)
        {
            int tileWidth = tiles.GetLength(0) * 16;
            int tileHeight = tiles.GetLength(1) * 16;
            Vector2 size = new Vector2(tileWidth, tileHeight);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            CalculatedStyle pos = base.GetDimensions();
            sb.Draw(makeThumbnailTexture, base.GetDimensions().Position(), Color.White);

            log4net.LogManager.GetLogger("BPConstructs").Info("Calculated pos x: " + (pos.X * (1 + Main.UIScale)));
            log4net.LogManager.GetLogger("BPConstructs").Info("UIScale: " + Main.UIScale);

            sb.End();
            sb.Begin();
        }
    }
}
