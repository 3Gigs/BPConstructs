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
        Vector2 screenPos;
        Point startScreenTile;
        Point lastScreenTile;
        private bool isMouseDown;
        private bool isMouseUp;
        // Fix UI Scaling from affecting DrawRect

        public override void OnInitialize()
        {
            isMouseDown = false;
            isMouseUp = false;
            startTile = new Point(-1, -1);
            lastMouseTile = new Point(-1, -1);

            screenPos = Main.screenPosition;
            screenPos += new Vector2(-50f);
            screenPos = screenPos.ToTileCoordinates().ToVector2() * 16f;
            startScreenTile = Main.MouseWorld.ToTileCoordinates();
            startScreenTile.X -= (int)screenPos.X / 16;
            startScreenTile.Y -= (int)screenPos.Y / 16;

            base.OnInitialize();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            PlayerInput.SetZoom_World();

            Point mouseTileCoord = (Main.MouseWorld).ToTileCoordinates();

            DrawBlueLaserGrid(spriteBatch);

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

                LogManager.GetLogger("startTile" + startTile);

                if (startScreenTile.X == -1 && startScreenTile.Y == -1)
                {
                    startScreenTile = Main.MouseWorld.ToTileCoordinates();
                    startScreenTile.X -= (int)screenPos.X / 16;
                    startScreenTile.Y -= (int)screenPos.Y / 16;

                    lastScreenTile = Main.MouseWorld.ToTileCoordinates();
                    lastScreenTile.X -= (int)screenPos.X / 16;
                    lastScreenTile.Y -= (int)screenPos.Y / 16;
                }
                else
                {
                    lastScreenTile = Main.MouseWorld.ToTileCoordinates();
                    lastScreenTile.X -= (int)screenPos.X / 16;
                    lastScreenTile.Y -= (int)screenPos.Y / 16;
                }


                Point upperLeftTile = new Point(
                    Math.Min(lastScreenTile.X, startScreenTile.X), 
                    Math.Min(lastScreenTile.Y, startScreenTile.Y));
                Point bottomRightTile = new Point(
                    Math.Max(lastScreenTile.X, startScreenTile.X),
                    Math.Max(lastScreenTile.Y, startScreenTile.Y));

                LogManager.GetLogger("startScreenTile: " + startScreenTile);
                LogManager.GetLogger("lastScreenTile: " + lastScreenTile);

                DrawCloneLaser(spriteBatch, screenPos, upperLeftTile, bottomRightTile);
            }
            else
            {
                screenPos = Main.screenPosition;
                screenPos += new Vector2(-50f);
                screenPos = screenPos.ToTileCoordinates().ToVector2() * 16f;
                startScreenTile = new Point(-1, -1);
                lastScreenTile = Main.MouseWorld.ToTileCoordinates();
                lastScreenTile.X -= (int)screenPos.X / 16;
                lastScreenTile.Y -= (int)screenPos.Y / 16;

                DrawCloneLaser(
                    spriteBatch,
                    screenPos,
                    lastScreenTile, 
                    lastScreenTile);
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

            spriteBatch.End();
            spriteBatch.Begin();
            base.Draw(spriteBatch);
        }

        private void DrawBlueLaserGrid(SpriteBatch spriteBatch)
        {
            float num = Main.player[Main.myPlayer].velocity.Length();
            num = Vector2.Distance(Main.player[Main.myPlayer].position, Main.player[Main.myPlayer].shadowPos[2]);
            float num2 = 6f;
            Texture2D value = TextureAssets.Extra[68].Value;
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
            Rectangle value2 = new Rectangle(0, 18, 18, 18);
            vec -= Vector2.One;

            //LogManager.GetLogger("BPConstructs").Info("Vec: " + (vec / 16f));
            //LogManager.GetLogger("BPConstructs").Info("Vec Tile: " + Framing.GetTileSafely(vec / 16f));
            //LogManager.GetLogger("BPConstructs").Info("Point: " + (point.ToVector2() * 16));
            //LogManager.GetLogger("BPConstructs").Info("Point Tile: " + Framing.GetTileSafely(point.ToVector2() * 16));

            Vector2 zero = Vector2.Zero;
            value2.X = 2;
            value2.Width = 16;
            zero.X = 2f;
            value2.Y = 2;
            value2.Height = 16;
            zero.Y = 2f;

            // Draws the blue laser grid
            for (int i = 0; i < num3; i++)
            {
                for (int j = 0; j < num4; j++)
                {
                    Color color3 = color;

                    spriteBatch.Draw(value, Main.ReverseGravitySupport(new Vector2(i, j) * 16f - Main.screenPosition + vec + zero, 16f), value2, color3, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        private void DrawCloneLaser(SpriteBatch spriteBatch, Vector2 screenPos, Point startTilePos, Point lastTilePos)
        {
            Texture2D texture = TextureAssets.Extra[68].Value;

            Color red = new Color(1f, 0.3f, 0.3f, 0.5f); 

            for (int x = startTilePos.X; x <= lastTilePos.X; x++)
            {
                for(int y = startTilePos.Y; y <= lastTilePos.Y; y++)
                {
                    spriteBatch.Draw(texture, Main.ReverseGravitySupport(new Vector2(x, y) * 16f - Main.screenPosition + screenPos, 16f), new Rectangle(0, 0, 18, 18), red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
            isMouseDown = Main.mouseLeft;
            isMouseUp = Main.mouseLeftRelease;

            //LogManager.GetLogger("BPConstructs").Info("DrawRect mouse: " + Main.MouseWorld.ToTileCoordinates().ToString());

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            base.Draw(spriteBatch);
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
