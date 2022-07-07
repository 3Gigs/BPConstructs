using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.Graphics;
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
        private Vector2 mouseUIDiff;

        public override void OnInitialize()
        {
            isMouseDown = false;
            isMouseUp = false;
            startTile = new Point(-1, -1);
            lastMouseTile = new Point(-1, -1);
            mouseUIDiff = (Main.MouseScreen * Main.UIScale - Main.MouseScreen);

            base.OnInitialize();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            DrawRect(spriteBatch, isMouseDown);
            base.Draw(spriteBatch);
        }

        public void DrawRect(SpriteBatch spriteBatch, bool fill)
        {
            Color color = new Color(255, 230, 26);
            Rectangle rect = new Rectangle(0, 0, 1, 1);
            Point upperLeftTile;
            Point bottomRightTile;
            
            if(isMouseDown)
            {
                upperLeftTile = new Point(
                    Math.Min(startTile.X, lastMouseTile.X), 
                    Math.Min(startTile.Y, lastMouseTile.Y));
                bottomRightTile = new Point(
                    Math.Max(startTile.X, lastMouseTile.X) + 1,
                    Math.Max(startTile.Y, lastMouseTile.Y) + 1);
            }
            else
            {
                upperLeftTile = (Main.MouseWorld + mouseUIDiff).ToTileCoordinates();
                bottomRightTile = new Point(upperLeftTile.X + 1, upperLeftTile.Y + 1);
            }

            LogManager.GetLogger("BPConstructs").Info("DrawRect upperLeftTile: " + upperLeftTile.ToString() + " lowerRightTile" + bottomRightTile.ToString());
            LogManager.GetLogger("BPConstructs").Info("DrawRect mouse: " + Main.MouseWorld.ToTileCoordinates().ToString());


            Vector2 upperLeftScreen = upperLeftTile.ToVector2() * 16f;
            // Vector2 bottomRightScreen = bottomRight * 16f;
            upperLeftScreen -= Main.screenPosition;
            Point offset = bottomRightTile - upperLeftTile;

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
                color, 
                0f, 
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
                Point upperLeftTile = new Point(
                    Math.Min(startTile.X, lastMouseTile.X),
                    Math.Min(startTile.Y, lastMouseTile.Y));
                Point lowerRightTile = new Point(
                    Math.Max(startTile.X, lastMouseTile.X),
                    Math.Max(startTile.Y, lastMouseTile.Y));

                Tile[,] clonedTiles = new Tile[
                    lowerRightTile.X - upperLeftTile.X + 1,
                    lowerRightTile.Y - upperLeftTile.Y + 1];
 
                for (int x = 0; x <= lowerRightTile.X - upperLeftTile.X; x++)
                {
                    for (int y = 0; y <= lowerRightTile.Y - upperLeftTile.Y; y++)
                    {
                        clonedTiles[x, y] = new Tile();
                    }
                }
                
                for (int i = 0; i <= lowerRightTile.X - upperLeftTile.X; i++)
                {
                    string output = "";
                    for (int j = 0; j <= lowerRightTile.Y - upperLeftTile.Y; j++)
                    {
                        if(WorldGen.InWorld(upperLeftTile.X + i, upperLeftTile.Y + j))
                        {
                            clonedTiles[i, j] = Framing.GetTileSafely(new Point(upperLeftTile.X + i, upperLeftTile.Y + j));
                            output += (upperLeftTile.ToString()) + " ";
                        }
                    }
                    LogManager.GetLogger("BPConstructs").Info(output);
                }
                //LogManager.GetLogger("BPConstructs").Info("cloneTiles upperLeftTile: " + upperLeftTile.ToString() + " lowerRightTile" + lowerRightTile.ToString());
                //LogManager.GetLogger("BPConstructs").Info("cloneTiles mouse: " + Main.MouseWorld.ToTileCoordinates().ToString());

                return clonedTiles;
            }
            return new Tile[0, 0];
        }

        public override void Update(GameTime gameTime)
        {
            isMouseDown = Main.mouseLeft ? true : false;
            isMouseUp = Main.mouseLeftRelease ? true : false;
            mouseUIDiff = (Main.MouseScreen * Main.UIScale - Main.MouseScreen);
            Point mouseTileCoord = (Main.MouseWorld + mouseUIDiff).ToTileCoordinates();

            if (isMouseDown)
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
            if (isMouseUp)
            {
                Tile[,] tiles = CloneTiles();
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    string output = "";
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        output += TileID.Search.GetName(tiles[i, j].TileType);
                    }
                    LogManager.GetLogger("BPConstructs").Info(output);
                }
                startTile = mouseTileCoord;
                lastMouseTile = mouseTileCoord;
            }

            //LogManager.GetLogger("BPConstructs").Info("ScreenPos: " + Main.screenPosition);
            LogManager.GetLogger("BPConstructs").Info("zoomDiff: ");
            LogManager.GetLogger("BPConstructs").Info("screenPos" + (Main.MouseScreen.ToTileCoordinates()));
            LogManager.GetLogger("BPConstructs").Info("xdddd poS" + (Main.MouseScreen * Main.UIScale - Main.MouseScreen));


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
