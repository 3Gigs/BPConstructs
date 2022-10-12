// Try calling BlueprintContainer.draw() somewhere
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPConstructs.Utils;
using log4net;
using Terraria.GameInput;
using Terraria.Localization;

namespace BPConstructs.Contents
{
    internal class CopyModeUI : DraggablePanel
    {
        private static UIPanelPlus blueprintUIContainer;
        private static Dictionary<string, Tile[,]> tiles = new Dictionary<string, Tile[,]>();
        private static int colCounter = 0;
        private static int rowCounter = 0;
        private UIPanelPlus searchBoxPanel;
        UIText noBlueprint = new UIText("No blueprints found!");

        public CopyModeUI()
        {
            base.Width.Set(480f, 0f);
            base.Height.Set(300f, 0f);
            base.BackgroundColor = new Color(73, 94, 171) * 0.6f;

            blueprintUIContainer = new UIPanelPlus();
            blueprintUIContainer.Width.Set(500f, 0f);
            blueprintUIContainer.Height.Set(250f, 0f);
            blueprintUIContainer.Top.Set(30f, 0f);
            blueprintUIContainer.BackgroundColor = base.BackgroundColor = new Color(73, 94, 171) * 0.7f;
            Append(blueprintUIContainer);

            UIElement header = new UIElement
            {
                Height = new StyleDimension(24f, 0f),
                Width = new StyleDimension(0f, 1f)
            };
            header.SetPadding(0f);
            this.AddSearchBar(header);
            Append(header);

            if (tiles.Count == 0)
            {
                blueprintUIContainer.Append(noBlueprint);
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
                LogManager.GetLogger("BPConstructs").Info("AddBlueprint was called");
                LogManager.GetLogger("BPConstructs").Info("tiles: " + String.Join(Environment.NewLine, tiles));

                tiles.Add(name, blueprint);

                BPContainerPanel itemPanel = new BPContainerPanel(blueprint);

                if (colCounter == 4)
                {
                    colCounter = 0;
                    rowCounter++;
                    itemPanel.Top.Set(110 * rowCounter, 0f);
                }

                itemPanel.Left.Set(110 * colCounter, 0f);
                itemPanel.Top.Set(110 * rowCounter, 0f);

                blueprintUIContainer.Append(itemPanel);

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
            //uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
            //uIImageButton.SetVisibility(1f, 1f);
            //searchArea.Append(uIImageButton);

            UIPanelPlus searchPanel = new UIPanelPlus
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


        public static void DrawPreview(SpriteBatch sb, Tile[,] tiles, Vector2 startPos, float scale = 1f)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            if (tiles.Count == 0 && blueprintUIContainer.HasChild(noBlueprint))
                blueprintUIContainer.Append(noBlueprint);
            else if (blueprintUIContainer.HasChild(noBlueprint))
                blueprintUIContainer.RemoveChild(noBlueprint);
            base.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

}
