using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BPConstructs.Utils;

namespace BPConstructs.Contents
{
    internal class AreaRect : UIElement
    {
        private Vector2 startTile;
        private Vector2 lastMouseTile;
        private bool isMouseDown;

        public override void OnInitialize()
        {
            isMouseDown = false;
            startTile = new Vector2(-1, -1);

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
            Vector2 upperLeft;
            Vector2 bottomRight;
            
            if(isMouseDown)
            {
                upperLeft = new Vector2(
                    Math.Min(startTile.X, lastMouseTile.X), 
                    Math.Min(startTile.Y, lastMouseTile.Y));
                bottomRight = new Vector2(
                    Math.Max(startTile.X, lastMouseTile.X) + 1,
                    Math.Max(startTile.Y, lastMouseTile.Y) + 1);
            }
            else
            {
                upperLeft = Main.MouseWorld.ToTileCoordinates().ToVector2();
                bottomRight = new Vector2(upperLeft.X + 1, upperLeft.Y + 1);
            }

            Vector2 upperLeftScreen = upperLeft * 16f;
            // Vector2 bottomRightScreen = bottomRight * 16f;
            upperLeftScreen -= Main.screenPosition;
            // bottomRightScreen -= Main.screenPosition;
            Vector2 offset = bottomRight - upperLeft;

            if(fill)
                spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value, 
                    upperLeftScreen, 
                    new Rectangle?(rect), 
                    color * 0.6f, 
                    0f, 
                    Vector2.Zero, 
                    16f * offset, 
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

        public override void Update(GameTime gameTime)
        {
            isMouseDown = Main.mouseLeft ? true : false;
            Point mouseTileCoord = Main.MouseWorld.ToTileCoordinates();

            if (isMouseDown)
            {
                if (startTile.X == -1)
                {
                    startTile = mouseTileCoord.ToVector2();
                    lastMouseTile = new Vector2(-1, -1);
                }
                else
                {
                    lastMouseTile = mouseTileCoord.ToVector2();
                }
            }
            else
            {
                startTile = mouseTileCoord.ToVector2();
                lastMouseTile = mouseTileCoord.ToVector2();
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
    internal class CopyMode : DraggablePanel
    {
        public CopyMode()
        {
            base.Width.Set(1000f, 0f);
            base.Height.Set(1000f, 0f);
        }
    }
    internal class ArchitectUI : UIState
    {
        AreaRect areaRect;
        CopyModeIcon btn;
        CopyMode copyMode;

        public ArchitectUI()
        {
            areaRect = new AreaRect();
            btn = new CopyModeIcon();
            copyMode = new CopyMode();
        }

        public override void Update(GameTime gameTime)
        {
            if (areaRect != null && btn != null && copyMode != null)
            {
                Player player = Main.LocalPlayer;
                BPCPlayer modPlayer = player.GetModPlayer<BPCPlayer>();
                if (modPlayer.architectMode == true)
                {
                    Append(btn);
                    Append(areaRect);
                    Append(copyMode);
                    btn.Update(gameTime);
                    areaRect.Update(gameTime);
                    copyMode.Update(gameTime);
                    modPlayer.architectMode = false;
                }
                else
                {
                    RemoveChild(btn);
                    RemoveChild(areaRect);
                    RemoveChild(copyMode);
                }
            }
        }
    }
}
