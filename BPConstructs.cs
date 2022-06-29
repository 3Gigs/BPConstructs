using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using BPConstructs.Contents;
using log4net;

namespace BPConstructs
{
    internal class BPConstructs : ModSystem
    {
        private UserInterface UI;
        private ArchitectUI bar;
        
        public override void UpdateUI(GameTime gameTime)
        {
            UI?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        UI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void Load()
        {
            if(!Main.dedServ)
            {
                bar = new ArchitectUI();
                bar.Activate();

                UI = new UserInterface();
                UI.SetState(bar);
            }
        }
    }

}