using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalTestHelpers.Items
{
    public class ChargerTestItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charger Test Item");
            Tooltip.SetDefault("Testing/Cheat Item\n" +
                               "Fully charges all Draedon's Arsenal items in your inventory");
        }

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.rare = ItemRarityID.Red;
            item.Calamity().customRarity = CalamityRarity.DraedonRust;

            item.consumable = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 29;
            item.useAnimation = 29;
            item.autoReuse = false;
            item.useTurn = true;
        }

        public override bool UseItem(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item.type < ItemID.Count)
                    continue;
                CalamityGlobalItem modItem = item.Calamity();
                if (modItem != null && modItem.UsesCharge)
                    modItem.Charge = modItem.MaxCharge;
            }
            Main.NewText("All chargeable Arsenal weapons in your inventory have been fully charged.", Color.Cyan);
            return true;
        }
    }
}