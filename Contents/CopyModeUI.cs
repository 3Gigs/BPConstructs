using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
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
        private static UIPanel blueprintUIContainer;
        private static Dictionary<string, Tile[,]> blueprints = new Dictionary<string, Tile[,]>();
        private static int colCounter = 0;
        private static int rowCounter = 0;
        private UIPanel searchBoxPanel;
        UIText noBlueprint = new UIText("No blueprints found!");

        public CopyModeUI()
        {
            base.Width.Set(480f, 0f);
            base.Height.Set(300f, 0f);
            base.BackgroundColor = new Color(73, 94, 171) * 0.6f;

            blueprintUIContainer = new UIPanel();
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

            if (blueprints.Count == 0)
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
                LogManager.GetLogger("BPConstructs").Info("blueprints: " + String.Join(Environment.NewLine, blueprints));

                blueprints.Add(name, blueprint);

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

            if (blueprints.Count == 0 && blueprintUIContainer.HasChild(noBlueprint))
                blueprintUIContainer.Append(noBlueprint);
            else if (blueprintUIContainer.HasChild(noBlueprint))
                blueprintUIContainer.RemoveChild(noBlueprint);

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }

}
