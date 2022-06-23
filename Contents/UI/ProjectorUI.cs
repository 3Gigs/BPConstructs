using Terraria;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using BPConstructs.Contents.Blocks;
using Microsoft.Xna.Framework;

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
            BPCPlayer bpcplayer = player.GetModPlayer<BPCPlayer>();
            Point16 proj = bpcplayer.CurrProjector();
            if(proj.X != -1 && proj.Y != -1)
            {
                Tile tile = Main.tile[proj.X, proj.Y];

                if (!Main.playerInventory
                    || tile != null
                        && !(TileLoader.GetTile(tile.TileType) is Projector))
                {
                    bpcplayer.CloseProjector();
                }

                if (bpcplayer.Projector != new Point16(-1, -1) && Main.playerInventory)
                {
                    Append(ui);
                }
                else
                {
                    RemoveChild(ui);
                }
            }

            base.Update(gameTime);
        }
    }
}
