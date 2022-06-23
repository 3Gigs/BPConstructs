using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;


namespace BPConstructs
{
    internal class ExampleButton : UIElement
    {
        Color color = new Color(50, 255, 153);

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay").Value,
                new Vector2(Main.screenWidth + 20, Main.screenHeight - 20) / 2f,
                color
            );
        }
    }

    internal class MenuBar : UIState
    {
        public ExampleButton exampleButton;

        public override void OnInitialize()
        {
            exampleButton = new ExampleButton();

            Append(exampleButton);
        }
    }
}
