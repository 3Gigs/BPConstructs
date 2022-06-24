using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BPConstructs.Contents
{
    internal class Blueprint : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Construction Blueprint");
            Tooltip.SetDefault("Good artists copy, great artists steal");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = ItemRarityID.Orange;
        }
    }
}
