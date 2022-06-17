using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace BPConstructs.Contents.Items
{
    internal class ProjectorBlock : ModItem
    {
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
            Item.createTile = ModContent.TileType<Tile.ProjectorTile>();
        }
    }
}
