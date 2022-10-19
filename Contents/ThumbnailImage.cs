using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents
{
    public class ThumbnailImage : UIElement
    {
        public Tile[,] tiles;
        TexInfo[,] textures;
        float scale;
        Vector2 offset = new Vector2();

        public ThumbnailImage(Tile[,] _tiles, Vector2 _thumbSize)
        {
            tiles = _tiles;
            textures = CreateThumbnail(_tiles);
            int tileWidth = tiles.GetLength(0) * 16;
            int tileHeight = tiles.GetLength(1) * 16;
            scale = 1f;
            if (tileWidth > _thumbSize.X || tileHeight > _thumbSize.Y)
            {
                if (tileHeight > tileWidth)
                {
                    scale = (float)_thumbSize.X / tileHeight;
                    offset.X = (_thumbSize.X - tileWidth * scale) / 2;
                }
                else
                {
                    scale = (float)_thumbSize.X / tileWidth;
                    offset.Y = (_thumbSize.Y - tileHeight * scale) / 2;
                }
            }

            offset /= scale;
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

        void DrawPreview(SpriteBatch sb)
        {
            int width = textures.GetLength(0);
            int height = textures.GetLength(1);
            Color color = Color.White;
            color.A = 160;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    TexInfo texture = textures[x, y];
                    if (texture.wallFrame != null)
                    {
                        Rectangle value = new Rectangle((int)texture.wallFrame?.X, (int)texture.wallFrame?.Y, 32, 32);
                        Vector2 pos = new Vector2(100, 100) + new Vector2(x * 16, y * 16);
                        sb.Draw(texture.texture, pos * this.scale, value, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
                    }
                    else if (texture.tileFrame != null)
                    {
                        Rectangle value = new Rectangle((int)texture.tileFrame?.X, (int)texture.tileFrame?.Y, 16, 16);
                        Vector2 pos = new Vector2(100, 100) + new Vector2(x * 16, y * 16);
                        sb.Draw(texture.texture, pos * this.scale, value, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
                    }
                }
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            this.DrawPreview(sb);
        }
    }
}
