using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using log4net;

namespace BPConstructs.Contents
{
    internal class BPContainerPanel : UIElement
    {
        Tile[,] _tiles;
        ThumbnailImage thumbnail;
        UIImagePlus _borders;
        UIImagePlus _borderSelect;

        public BPContainerPanel(Tile[,] tiles)
        {
            Width.Set(100f, 0f);
            Height.Set(100f, 0f);
            _tiles = tiles;
            _borders = new UIImagePlus((Texture2D)Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Selection"))
            {
                IgnoresMouseInteraction = true
            };
            thumbnail = new ThumbnailImage(tiles, new Vector2(100, 100))
            {
                IgnoresMouseInteraction = true
            };
            this.Append(thumbnail);
        }

        public Tile[,] tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            PlayerInput.SetZoom_UI();

            LogManager.GetLogger("BPConstructs").Info("Left Pixels: " + base.Left.Pixels);

            base.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            if (!this.Elements.Contains(this._borders))
            {
                this.Append(this._borders);
            }
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            this.RemoveChild(this._borders);
        }
    }
}
