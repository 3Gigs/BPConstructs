using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents
{
    /**
     * Represents the blueprint icon under the player
     */
    internal class ModeIcon : UIElement
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

}
