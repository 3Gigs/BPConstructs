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
using Terraria.GameInput;

namespace BPConstructs.Contents
{
    internal class CopyMode : UIElement
    {
        private Point startTile;
        private Point lastMouseTile;
        private bool isMouseDown;
        private bool isMouseUp;
        // Fix UI Scaling from affecting DrawRect
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
            PlayerInput.SetZoom_World();
            DrawInterface_3_LaserRuler();
            spriteBatch.End();
            spriteBatch.Begin();
            base.Draw(spriteBatch);
        }

        public void DrawRect(SpriteBatch spriteBatch, bool fill)
        {
            Color color = new Color(255, 230, 26);
            Rectangle rect = new Rectangle(0, 0, 1, 1);
            Point upperLeftTile;
            Point bottomRightTile;

            if (isMouseDown)
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




            Vector2 upperLeftScreen = upperLeftTile.ToVector2() * 16f;
            // Vector2 bottomRightScreen = bottomRight * 16f;
            //upperLeftScreen -= Main.screenPosition;
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

            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitX * -2f,  
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(2f, offset.Y * 16f), 
                SpriteEffects.None, 
                0f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitX * offset.X * 16f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(2f, offset.Y * 16f), 
                SpriteEffects.None, 
                0f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitY * -2f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(offset.X * 16f, 2f), 
                SpriteEffects.None, 
                0f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                upperLeftScreen + Vector2.UnitY * offset.Y * 16f, 
                new Microsoft.Xna.Framework.Rectangle?(rect), 
                color, 
                0f, 
                Vector2.Zero, 
                new Vector2(offset.X * 16f, 2f), 
                SpriteEffects.None, 
                0f);
        }
        private static void DrawInterface_3_LaserRuler()
        {
            float num = Main.player[Main.myPlayer].velocity.Length();
            num = Vector2.Distance(Main.player[Main.myPlayer].position, Main.player[Main.myPlayer].shadowPos[2]);
            float num2 = 6f;
            Texture2D value = TextureAssets.MagicPixel.Value;
            float scale = MathHelper.Lerp(0.2f, 0.7f, MathHelper.Clamp(1f - num / num2, 0f, 1f));
            Vector2 vec = Main.screenPosition;
            vec += new Vector2(-50f);
            vec = vec.ToTileCoordinates().ToVector2() * 16f;
            int num3 = (Main.screenWidth + 100) / 16;
            int num4 = (Main.screenHeight + 100) / 16;
            Point point = Main.MouseWorld.ToTileCoordinates();
            point.X -= (int)vec.X / 16;
            point.Y -= (int)vec.Y / 16;
            Color color = new Color(0.24f, 0.8f, 0.9f, 0.5f) * 0.4f * scale;
            Color color2 = new Color(1f, 0.8f, 0.9f, 0.5f) * 0.5f * scale;
            Rectangle value2 = new Rectangle(0, 18, 18, 18);
            vec -= Vector2.One;

            LogManager.GetLogger("BPConstructs").Info("Vec: " + vec);
            LogManager.GetLogger("BPConstructs").Info("Point: " + point);

            for (int i = 0; i < num3; i++)
            {
                for (int j = 0; j < num4; j++)
                {
                    Color color3 = color;
                    Vector2 zero = Vector2.Zero;
                    if (i != point.X && j != point.Y)
                    {
                        if (i != point.X + 1)
                        {
                            value2.X = 0;
                            value2.Width = 16;
                        }
                        else
                        {
                            value2.X = 2;
                            value2.Width = 14;
                            zero.X = 2f;
                        }
                        if (j != point.Y + 1)
                        {
                            value2.Y = 18;
                            value2.Height = 16;
                        }
                        else
                        {
                            value2.Y = 2;
                            value2.Height = 14;
                            zero.Y = 2f;
                        }
                        Main.spriteBatch.Draw(value, Main.ReverseGravitySupport(new Vector2(i, j) * 16f - Main.screenPosition + vec + zero, 16f), value2, color3, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            value2 = new Rectangle(0, 0, 16, 18);
            for (int k = 0; k < num3; k++)
            {
                if (k == point.X)
                {
                    Main.spriteBatch.Draw(value, Main.ReverseGravitySupport(new Vector2(k, point.Y) * 16f - Main.screenPosition + vec, 16f), new Rectangle(0, 0, 16, 16), color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(value, Main.ReverseGravitySupport(new Vector2(k, point.Y) * 16f - Main.screenPosition + vec, 16f), value2, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            value2 = new Rectangle(0, 0, 18, 16);
            for (int l = 0; l < num4; l++)
            {
                if (l != point.Y)
                {
                    Main.spriteBatch.Draw(value, Main.ReverseGravitySupport(new Vector2(point.X, l) * 16f - Main.screenPosition + vec, 16f), value2, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
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

            LogManager.GetLogger("BPConstructs").Info("DrawRect mouse: " + Main.MouseWorld.ToTileCoordinates().ToString());

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
