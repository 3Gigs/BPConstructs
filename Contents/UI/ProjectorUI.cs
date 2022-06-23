using Terraria;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPConstructs.Contents.UI
{
    internal class ProjectorUI : UIPanel
    {
        public ProjectorUI()
        {
            // Pink BG Color
            base.BackgroundColor = new Color(255, 153, 204);
            base.Height.Set(200f, 0f);
            base.Width.Set(400f, 0f);
            base.Left.Set((Main.screenWidth - base.Width.Pixels) / 2, 0f);
            base.Top.Set((Main.screenHeight - base.Height.Pixels) / 2, 0f);
        }
    }
    
    internal class ProjectorState : UIState
    {
        ProjectorUI ui;

        public override void OnInitialize()
        {
            this.ui = new ProjectorUI();
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            BPCPlayer bPCPlayer = player.GetModPlayer<BPCPlayer>();

            if (bPCPlayer.Projector != new Point16(-1, -1))
            {
                Append(ui);
            }
            base.Update(gameTime);
        }
    }
}
