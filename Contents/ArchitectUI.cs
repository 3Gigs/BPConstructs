using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using log4net;

// TODO: Use mod instances like in cheat sheet mod

namespace BPConstructs.Contents
{
    internal class ArchitectUI : UIState
    {
        CopyMode copyMode;
        CopyModeUI copyModeUI;
        ModeIcon btn;

        public ArchitectUI()
        {
            copyMode = new CopyMode();
            btn = new ModeIcon();
            copyModeUI = new CopyModeUI();
        }

        public override void Update(GameTime gameTime)
        {
            if (copyMode != null && btn != null && copyModeUI != null)
            {
                Player player = Main.LocalPlayer;
                BPCPlayer modPlayer = player.GetModPlayer<BPCPlayer>();
                if (modPlayer.architectMode == true)
                {
                    Append(btn);
                    Append(copyMode);
                    Append(copyModeUI);
                    btn.Update(gameTime);
                    copyMode.Update(gameTime);
                    copyModeUI.Update(gameTime);

                    if (copyModeUI.IsMouseInside())
                    {
                        CopyMode.forceNoDraw = true;
                    }
                    else
                    {
                        CopyMode.forceNoDraw = false;
                    }

                    modPlayer.architectMode = false;
                }
                else
                {
                    RemoveChild(btn);
                    RemoveChild(copyMode);
                    RemoveChild(copyModeUI);
                }
            }
        }
    }
}
