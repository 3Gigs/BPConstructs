using Terraria;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using BPConstructs.Contents.Blocks;
using Microsoft.Xna.Framework;

namespace BPConstructs.Contents
{
    internal class ProjectorUI : UIPanel
    {
        public ProjectorUI()
        {
            // Pink BG Color
            BackgroundColor = new Color(255, 153, 204);
            Height.Set(200f, 0f);
            Width.Set(400f, 0f);
            Left.Set((Main.screenWidth - Width.Pixels) / 2, 0f);
            Top.Set((Main.screenHeight - Height.Pixels) / 2, 0f);
        }
    }

    internal class ProjectorState : UIState
    {
        ProjectorUI ui;

        public override void OnInitialize()
        {
            ui = new ProjectorUI();
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            BPCPlayer bpcplayer = player.GetModPlayer<BPCPlayer>();
            Point16 proj = bpcplayer.CurrProjector();
            if (proj.X != -1 && proj.Y != -1)
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
