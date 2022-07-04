using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPConstructs.Utils;
using log4net;

namespace BPConstructs.Contents
{
    internal class CopyMode : UIElement
    {
        private Point startTile;
        private Point lastMouseTile;
        private bool isMouseDown;
        private bool isMouseUp;

        public override void OnInitialize()
        {
            isMouseDown = false;
            isMouseUp = false;
            startTile = new Point(-1, -1);
            lastMouseTile = new Point(-1, -1);

            base.OnInitialize();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawRect(spriteBatch, isMouseDown);
        }

        public void DrawRect(SpriteBatch spriteBatch, bool fill)
        {
            Color color = new Color(255, 230, 26);
            Rectangle rect = new Rectangle(0, 0, 1, 1);
            Point upperLeft;
            Point bottomRight;
            
            if(isMouseDown)
            {
                upperLeft = new Point(
                    Math.Min(startTile.X, lastMouseTile.X), 
                    Math.Min(startTile.Y, lastMouseTile.Y));
                bottomRight = new Point(
                    Math.Max(startTile.X, lastMouseTile.X) + 1,
                    Math.Max(startTile.Y, lastMouseTile.Y) + 1);
            }
            else
            {
                upperLeft = Main.MouseWorld.ToTileCoordinates();
                bottomRight = new Point(upperLeft.X + 1, upperLeft.Y + 1);
            }

            Vector2 upperLeftScreen = upperLeft.ToVector2() * 16f;
            // Vector2 bottomRightScreen = bottomRight * 16f;
            upperLeftScreen -= Main.screenPosition;
            // bottomRightScreen -= Main.screenPosition;
            Point offset = bottomRight - upperLeft;

            if(fill)
                spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value, 
                    upperLeftScreen, 
                    new Rectangle?(rect), 
                    color * 0.6f, 
                    0f, 
                    Vector2.Zero, 
                    16f * offset.ToVector2(), 
                    SpriteEffects.None, 
                    0f);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitX * -2f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(2f, offset.Y * 16f), 
                SpriteEffects.None, 
                0f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitX * offset.X * 16f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(2f, offset.Y * 16f), 
                SpriteEffects.None, 
                0f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitY * -2f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(offset.X * 16f, 2f), 
                SpriteEffects.None, 
                0f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitY * offset.Y * 16f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 0f, 
                Vector2.Zero, 
                new Vector2(offset.X * 16f, 2f), 
                SpriteEffects.None, 
                0f);

        }

        public Tile[,] CloneTiles()
        {
            if (startTile.X != -1 && startTile.Y != -1
                && lastMouseTile.X != -1 && lastMouseTile.Y != -1
                && startTile != lastMouseTile)
            {
                Point upperLeft = new Point(
                    Math.Min(startTile.X, lastMouseTile.X),
                    Math.Min(startTile.Y, lastMouseTile.Y));
                Point lowerRight = new Point(
                    Math.Max(startTile.X, lastMouseTile.X),
                    Math.Max(startTile.Y, lastMouseTile.Y));

                Tile[,] clonedTiles = new Tile[
                    lowerRight.X - upperLeft.X + 1,
                    lowerRight.Y - upperLeft.Y + 1];

                for (int x = 0; x <= lowerRight.X - upperLeft.X; x++)
                {
                    for (int y = 0; y <= lowerRight.Y - upperLeft.Y; y++)
                    {
                        clonedTiles[x, y] = new Tile();
                    }
                }

                for (int i = 0; i <= lowerRight.X - upperLeft.X; i++)
                {
                    for (int j = 0; j <= lowerRight.Y - upperLeft.Y; j++)
                    {
                        if(WorldGen.InWorld(upperLeft.X + i, upperLeft.Y + j))
                        {
                            // clonedTiles[i, j] = Main.tile[minX + i, minY + j];
                            LogManager.GetLogger("BPConstructs").Info(upperLeft.X + i);
                        }
                    }
                }

                return clonedTiles;
            }
            return new Tile[0, 0];
        }

        public override void Update(GameTime gameTime)
        {
            isMouseDown = Main.mouseLeft ? true : false;
            isMouseUp = Main.mouseLeftRelease ? true : false;
            Point mouseTileCoord = Main.MouseWorld.ToTileCoordinates();

             if (isMouseUp)
            {
                Tile[,] tiles = CloneTiles();
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    string output = "";
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        output += tiles[i, j].TileType;
                    }
                    //LogManager.GetLogger("BPConstructs").Info(output);
                }
                startTile = mouseTileCoord;
                lastMouseTile = mouseTileCoord;
            }
            else if (isMouseDown)
            {
                if (startTile.X == -1)
                {
                    startTile = mouseTileCoord;
                    lastMouseTile = new Point(-1, -1);
                }
                else
                {
                    lastMouseTile = mouseTileCoord;
                }
            }
            else
            {
                startTile = mouseTileCoord;
                lastMouseTile = mouseTileCoord;
            }

            base.Update(gameTime);
        }
    }
    internal class CopyModeIcon : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = new Color(255, 255, 255);

            spriteBatch.Draw(
                ModContent.Request<Texture2D>("BPConstructs/Contents/ArchitectMode").Value,
                new Vector2(Main.screenWidth - 30, Main.screenHeight + 40) / 2f,
                color);
        }
    }
    internal class CopyModeUI : DraggablePanel
    {
        public CopyModeUI()
        {
            base.Width.Set(250f, 0f);
            base.Height.Set(250f, 0f);
        }
    }
    internal class ArchitectUI : UIState
    {
        CopyMode copyMode;
        CopyModeIcon btn;
        CopyModeUI copyModeUI;

        public ArchitectUI()
        {
            copyMode = new CopyMode();
            btn = new CopyModeIcon();
            copyModeUI = new CopyModeUI();
        }

        public override void Update(GameTime gameTime)
        {
            if (copyMode != null && btn != null && copyModeUI != null)
            {
                Player player = Main.LocalPlayer;
                BPCPlayer modPlayer = player.GetModPlayer<BPCPlayer>();
                if (modPlayer.architectMode == true)
                {
                    Append(btn);
                    Append(copyMode);
                    Append(copyModeUI);
                    btn.Update(gameTime);
                    copyMode.Update(gameTime);
                    copyModeUI.Update(gameTime);
                    modPlayer.architectMode = false;
                }
                else
                {
                    RemoveChild(btn);
                    RemoveChild(copyMode);
                    RemoveChild(copyModeUI);
                }
            }
        }
    }
}
