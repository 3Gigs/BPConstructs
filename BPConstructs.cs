using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using BPConstructs.Contents;

namespace BPConstructs
{
    public class BPConstructs : ModSystem
    {
        internal ProjectorState projectorUI;
        private UserInterface _projectorUI;
        public override void UpdateUI(GameTime gameTime)
        {
            _projectorUI?.Update(gameTime);
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
                        _projectorUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void Load()
        {
            projectorUI = new ProjectorState();
            projectorUI.Activate();
            _projectorUI = new UserInterface();
            _projectorUI.SetState(projectorUI);
        }
    }

}