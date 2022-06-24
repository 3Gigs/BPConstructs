using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using Terraria.Localization;
using BPConstructs.Utils;

namespace BPConstructs.Contents
{
    internal class Projector : ModTile
    {
        public readonly static int WIDTH = 2;
        public readonly static int HEIGHT = 2;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = WIDTH;
            TileObjectData.newTile.Width = HEIGHT;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(
                ModContent
                    .GetInstance<ProjectorTE>()
                    .Hook_AfterPlacement,
                -1,
                0,
                false
            );
            TileObjectData.addTile(Type);

            DustType = DustID.Adamantite;

            AddMapEntry(new Color(200, 200, 200));
        }

        public override bool RightClick(int x, int y)
        {
            Player player = Main.LocalPlayer;
            BPCPlayer bpplayer = player.GetModPlayer<BPCPlayer>();

            //Should your tile entity bring up a UI, this line is useful to prevent item slots from 
            Main.mouseRightRelease = false;

            //The following four (4) if-blocks are recommended to be used if your multitile opens a UI when right clicked:
            if (player.sign > -1)
            {
                SoundEngine.PlaySound(SoundID.MenuClose with
                {
                    Volume = -1,
                });
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick with
                {
                    Volume = -1,
                });
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(
                    MessageID.SyncPlayerChest,
                    -1,
                    -1,
                    NetworkText
                        .FromLiteral(Main.chest[player.chest].name),
                    player.chest, 1f, 0f, 0f, 0, 0, 0
                );
                player.editedChestName = false;
            }
            if (player.talkNPC > -1)
            {
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = string.Empty;
            }

            if (TileUtils.TryGetTileEntityAs(x, y, out ProjectorTE entity))
            {
                // Do things to your entity here
                Main.playerInventory = true;
                bpplayer.OpenProjector(new Point16(x, y));


                return true;
            }

            return false;
        }

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            int item = ModContent.ItemType<ProjectorItem>();
            Item.NewItem(
                new EntitySource_TileBreak(x, y),
                new Vector2(x * 16, y * 16),
                new Vector2(WIDTH * 16, HEIGHT * 16),
                item);

            Point16 origin = TileUtils.GetTileOrigin(x, y);
            ModContent.GetInstance<ProjectorTE>().Kill(origin.X, origin.Y);
        }

    }

    internal class ProjectorTE : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            return tile.TileType == ModContent.TileType<Projector>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                NetMessage.SendTileSquare(Main.myPlayer, i, j, Projector.WIDTH, Projector.HEIGHT);

                //Sync the placement of the tile entity with other clients
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
            }

            Point16 tileOrigin = new Point16(0, 1);
            return Place(i - tileOrigin.X, j - tileOrigin.Y);
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
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
            Item.width = Projector.WIDTH;
            Item.height = Projector.HEIGHT;
            Item.maxStack = 999;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Projector>();
        }
    }
}
