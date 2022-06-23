using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BPConstructs.Contents
{
    internal class BPCPlayer : ModPlayer
    {
        private Point16 projector = new Point16(-1,-1);

        public Point16 Projector
        {
            get => this.projector;
        }

        public void OpenProjector(Point16 point)
        {
            projector = point;
        }

        public void CloseProjector()
        {
            projector = new Point16(-1, -1);
        }

        public Point16 CurrProjector()
        {
            return projector;
        }

    }
}
