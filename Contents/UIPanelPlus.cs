using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using log4net;

namespace BPConstructs.Contents
{
    internal class UIPanelPlus : UIPanel
    {
		public Vector2 position;
		public Vector2 offset;

		public UIPanelPlus()
		{
			position = new Vector2(base.Left.Pixels, base.Top.Pixels);
			offset = new Vector2(base.Width.Pixels, base.Height.Pixels);
		}

		public virtual bool IsMouseInside()
		{
			if (Main.mouseX > Main.screenWidth || Main.mouseX < 0 || Main.mouseY > Main.screenHeight || Main.mouseY < 0)
				return false;

			Vector2 vector = position;

			return (float)Main.mouseX >= vector.X && (float)Main.mouseX <= vector.X + base.Width.Pixels && (float)Main.mouseY >= vector.Y && (float)Main.mouseY <= vector.Y + base.Height.Pixels;
		}
	}
}
