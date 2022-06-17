using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BPConstructs.Contents.Tile
{
    internal class ProjectorTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            DustType = DustID.Adamantite;
            ItemDrop = ModContent.ItemType<Items.ProjectorBlock>();

            AddMapEntry(new Color(200, 200, 200));
        }
    }
}
