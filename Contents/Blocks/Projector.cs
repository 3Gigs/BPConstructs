using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BPConstructs.Contents.Blocks
{
    internal class Projector : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            DustType = DustID.Adamantite;
            ItemDrop = ModContent.ItemType<ProjectorItem>();

            AddMapEntry(new Color(200, 200, 200));
        }
    }

    internal class ProjectorItem : ModItem
    {
        public override string Texture => "BPConstructs/Assets/ProjectorItem";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Projector");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Projector>();
        }
    }
}
