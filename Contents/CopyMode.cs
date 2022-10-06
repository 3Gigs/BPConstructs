using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPConstructs.Utils;
using log4net;
using Terraria.GameInput;
using Terraria.Localization;

namespace BPConstructs.Contents
{
    /**
     * Contains the CopyMode functionality
     */
    internal class CopyMode : UIElement
    {
        public static bool forceNoDraw;
        private Point startTile;
        private Point lastMouseTile;
        private Vector2 screenPos;
        private Point startScreenTile;
        private Point lastScreenTile;
        private bool isMouseDown;
        private bool isMouseUp;
        private bool isJustMouseDown;
        public Tile[,] clonedTiles;
        // Fix UI Scaling from affecting DrawRect

        private Point upperLeftTile;
        private Point bottomRightTile;

        public bool isScreenPosInUI(Vector2 screenPos)
        {
            return upperLeftTile.ToWorldCoordinates().X < screenPos.X &&
                   upperLeftTile.ToWorldCoordinates().Y < screenPos.Y &&
                   bottomRightTile.ToWorldCoordinates().X > screenPos.X &&
                   bottomRightTile.ToWorldCoordinates().Y > screenPos.Y;
        }

        public CopyMode()
        {
            isMouseDown = false;
            isMouseUp = false;
            isJustMouseDown = false;
            startTile = new Point(-1, -1);
            lastMouseTile = new Point(-1, -1);
            clonedTiles = new Tile[0, 0];

            screenPos = Main.screenPosition;
            screenPos += new Vector2(-50f);
            screenPos = screenPos.ToTileCoordinates().ToVector2() * 16f;
            startScreenTile = Main.MouseWorld.ToTileCoordinates();
            startScreenTile.X -= (int)screenPos.X / 16;
            startScreenTile.Y -= (int)screenPos.Y / 16;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            PlayerInput.SetZoom_World();

            Point mouseTileCoord = (Main.MouseWorld).ToTileCoordinates();

            DrawBlueLaserGrid(spriteBatch);

            if (Main.mouseLeft && !Main.LocalPlayer.mouseInterface && !forceNoDraw)
            {
                isMouseDown = true;
                isJustMouseDown = true;
                isMouseUp = false;
            }
            else
            {
                isMouseUp = true;
                isMouseDown = false;
            }

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


                upperLeftTile = new Point(
                    Math.Min(lastScreenTile.X, startScreenTile.X),
                    Math.Min(lastScreenTile.Y, startScreenTile.Y));
                bottomRightTile = new Point(
                    Math.Max(lastScreenTile.X, startScreenTile.X),
                    Math.Max(lastScreenTile.Y, startScreenTile.Y));

                LogManager.GetLogger("startScreenTile: " + startScreenTile);
                LogManager.GetLogger("lastScreenTile: " + lastScreenTile);

                DrawCloneLaser(spriteBatch, screenPos, upperLeftTile, bottomRightTile);
            }
            else if (isMouseUp && isJustMouseDown) // Just released mouse
            {
                Tile[,] tiles = CloneTiles();
                CopyModeUI.AddBlueprint(new Random().Next(100000).ToString(), tiles);
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    string output = "";
                    for (int x = 0; x < tiles.GetLength(0); x++)
                    {
                        output += TileID.Search.GetName(tiles[x, y].TileType) + " ";
                    }
                    LogManager.GetLogger("BPConstructs").Info(output);
                }
                startTile = mouseTileCoord;
                lastMouseTile = mouseTileCoord;

                isJustMouseDown = false;
            }
            else if (isMouseUp && !isJustMouseDown && !Main.LocalPlayer.mouseInterface && !forceNoDraw) // If is mouse up
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

                startTile = mouseTileCoord;
                lastMouseTile = mouseTileCoord;
            }

            spriteBatch.End();
            spriteBatch.Begin();
            base.Draw(spriteBatch);
        }

        // Draws the blue laser grid
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

            Vector2 zero = Vector2.Zero;
            value2.X = 2;
            value2.Width = 16;
            zero.X = 2f;
            value2.Y = 2;
            value2.Height = 16;
            zero.Y = 2f;

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
                for (int y = startTilePos.Y; y <= lastTilePos.Y; y++)
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
                        if (WorldGen.InWorld(upperLeftTile.X + i, upperLeftTile.Y + j))
                        {
                            clonedTiles[i, j] = Framing.GetTileSafely(new Point(upperLeftTile.X + i, upperLeftTile.Y + j));
                            output += (upperLeftTile.ToString()) + " ";
                        }
                    }
                    LogManager.GetLogger("BPConstructs").Info(output);
                }

                return clonedTiles;
            }
            else
            {
                LogManager.GetLogger("BPConstructs").Error("Invalid CloneTile!");
            }

            return new Tile[0, 0];
        }

        private static void DrawPreview(SpriteBatch sb, Tile[,] tiles, Vector2 startPos, float scale)
        {
            Color color = Color.White;
            color.A = 160;
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tile tile = tiles[x, y];

                    if (tile.WallType > 0)
                    {
                        Main.instance.LoadWall(tile.WallType);
                        Texture2D textureWall;

                        textureWall = TextureAssets.Wall[tile.WallType].Value;

                        int wallFrame = Main.wallFrame[tile.WallType] * 180;
                        Rectangle value = new Rectangle(tile.WallFrameX, tile.WallFrameY + wallFrame, 32, 32);
                        Vector2 pos = startPos + new Vector2(x * 16 - 8, y * 16 - 8);
                        sb.Draw(textureWall, pos * scale, value, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                    else if (tile.HasTile)
                    {
                        Main.instance.LoadTiles(tile.TileType);
                        Texture2D texture = TextureAssets.Tile[tile.TileType].Value;
                        Rectangle? value = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
                        Vector2 pos = startPos + new Vector2(x * 16, y * 16);
                        sb.Draw(texture, pos * scale, value, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
    internal class CopyModeUI : DraggablePanel
    {
        private static UIPanel blueprintContainer;
        private static Dictionary<string, Tile[,]> clipBoard = new Dictionary<string, Tile[,]>();
        private static int colCounter = 0;
        private static int rowCounter = 0;
        private UIPanel searchBoxPanel;
        UIText noBlueprint = new UIText("No blueprints found!");

        public CopyModeUI()
        {
            base.Width.Set(480f, 0f);
            base.Height.Set(300f, 0f);
            base.BackgroundColor = new Color(73, 94, 171) * 0.6f;

            blueprintContainer = new UIPanel();
            blueprintContainer.Width.Set(500f, 0f);
            blueprintContainer.Height.Set(250f, 0f);
            blueprintContainer.Top.Set(30f, 0f);
            blueprintContainer.BackgroundColor = base.BackgroundColor = new Color(73, 94, 171) * 0.7f;
            Append(blueprintContainer);

            UIElement header = new UIElement
            {
                Height = new StyleDimension(24f, 0f),
                Width = new StyleDimension(0f, 1f)
            };
            header.SetPadding(0f);
            this.AddSearchBar(header);
            Append(header);

            if (clipBoard.Count == 0)
            {
                blueprintContainer.Append(noBlueprint);
            }
        }

        public float X
        {
            get { return base.Left.Pixels; }
        }

        public float Y
        {
            get { return base.Top.Pixels; }
        }

        public static bool AddBlueprint(string name, Tile[,] blueprint)
        {
            try
            {
                clipBoard.Add(name, blueprint);
                LogManager.GetLogger("BPConstructs").Info("AddBlueprint was called");
                LogManager.GetLogger("BPConstructs").Info("clipBoard: " + String.Join(Environment.NewLine, clipBoard));

                UIPanel itemPanel = new UIPanel()
                {
                    Width = new StyleDimension(100f, 0f),
                    Height = new StyleDimension(100f, 0f),
                };

                if (colCounter == 4)
                {
                    colCounter = 0;
                    rowCounter++;
                    itemPanel.Top.Set(110 * rowCounter, 0f);
                    blueprintContainer.Append(itemPanel);
                }

                itemPanel.Left.Set(110 * colCounter, 0f);
                itemPanel.Top.Set(110 * rowCounter, 0f);
                blueprintContainer.Append(itemPanel);
                colCounter++;

                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public bool IsMouseInside()
        {
            float upperLeftX = base.Left.Pixels;
            float upperLeftY = base.Top.Pixels;

            float bottomRightX = base.Left.Pixels + base.Width.Pixels;
            float bottomRightY = base.Top.Pixels + base.Height.Pixels;

            return upperLeftX < Main.MouseScreen.X && upperLeftY < Main.MouseScreen.Y &&
                   bottomRightX > Main.MouseScreen.X && bottomRightY > Main.MouseScreen.Y;
        }

        private void AddSearchBar(UIElement searchArea)
        {
            UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search"))
            {
                VAlign = 0.5f,
                HAlign = 0f
            };
            uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
            uIImageButton.SetVisibility(1f, 1f);
            searchArea.Append(uIImageButton);

            UIPanel searchPanel = new UIPanel
            {
                Width = new StyleDimension(0f - uIImageButton.Width.Pixels - 3f, 1f),
                Height = new StyleDimension(0f, 1f),
                VAlign = 0.5f,
                HAlign = 1f
            };
            this.searchBoxPanel = searchPanel;

            UISearchBar searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                HAlign = 0f,
                VAlign = 0.5f,
                Left = new StyleDimension(0f, 0f),
                IgnoresMouseInteraction = true
            };
            searchPanel.Append(searchBar);

            searchArea.Append(searchPanel);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            if (clipBoard.Count == 0 && blueprintContainer.HasChild(noBlueprint))
                blueprintContainer.Append(noBlueprint);
            else if (blueprintContainer.HasChild(noBlueprint))
                blueprintContainer.RemoveChild(noBlueprint);

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }

}
