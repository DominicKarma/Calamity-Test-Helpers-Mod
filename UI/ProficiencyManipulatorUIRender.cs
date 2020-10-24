using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalTestHelpers.UI
{
	public class ProficiencyManipulatorUIRender : GrandUIRender
	{
		public enum ProficiencyClass
		{
			Melee,
			Ranged,
			Magic,
			Summoner,
			Rogue
		}
		public override List<SpecialUIElement> UIElements => new List<SpecialUIElement>()
		{
			new SpecialUIElement("Update your melee profiency.", ModContent.GetTexture("CalamityMod/Items/Weapons/Melee/Animus"), () => ToggleUpgrade(ProficiencyClass.Melee)),
			new SpecialUIElement("Update your ranged profiency.", ModContent.GetTexture("CalamityMod/Items/Weapons/Ranged/Aeries"), () => ToggleUpgrade(ProficiencyClass.Ranged)),
			new SpecialUIElement("Update your magic profiency.", ModContent.GetTexture("CalamityMod/Items/Weapons/Magic/Eternity"), () => ToggleUpgrade(ProficiencyClass.Magic)),
			new SpecialUIElement("Update your summoner profiency.", ModContent.GetTexture("CalamityMod/Items/Weapons/Summon/AbandonedSlimeStaff"), () => ToggleUpgrade(ProficiencyClass.Summoner)),
			new SpecialUIElement("Update your rogue profiency.", ModContent.GetTexture("CalamityMod/Items/Weapons/Rogue/Malachite"),() => ToggleUpgrade(ProficiencyClass.Rogue)),
		};
		public override float UIScale => 0.75f * ResolutionRatio;

		public override Vector2 TopLeftLocation => new Vector2(Main.screenWidth - 660 - 270 * ResolutionRatio, 40);

		public static void ToggleUpgrade(ProficiencyClass classToUpgrade)
		{
			string className = string.Empty;
            ref int experience = ref Main.LocalPlayer.Calamity().meleeLevel;
            ref int level = ref Main.LocalPlayer.Calamity().exactMeleeLevel;
			switch (classToUpgrade)
			{
                case ProficiencyClass.Melee:
                    className = "melee";
                    experience = ref Main.LocalPlayer.Calamity().meleeLevel;
                    level = ref Main.LocalPlayer.Calamity().exactMeleeLevel;
                    break;
                case ProficiencyClass.Ranged:
                    className = "ranged";
                    experience = ref Main.LocalPlayer.Calamity().rangedLevel;
                    level = ref Main.LocalPlayer.Calamity().exactRangedLevel;
                    break;
                case ProficiencyClass.Magic:
                    className = "magic";
                    experience = ref Main.LocalPlayer.Calamity().magicLevel;
                    level = ref Main.LocalPlayer.Calamity().exactMagicLevel;
                    break;
                case ProficiencyClass.Summoner:
                    className = "summoner";
                    experience = ref Main.LocalPlayer.Calamity().summonLevel;
                    level = ref Main.LocalPlayer.Calamity().exactSummonLevel;
                    break;
                case ProficiencyClass.Rogue:
                    className = "rogue";
                    experience = ref Main.LocalPlayer.Calamity().rogueLevel;
                    level = ref Main.LocalPlayer.Calamity().exactRogueLevel;
                    break;
            }

            if (Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift))
            {
                if (level == 0)
                    level = 15;
                else if (level == 15)
                    level = 0;
                else level = 0;
            }
            else
            {
                level = (level + (Main.mouseRight ? -1 : 1)) % 16;
                if (level < 0)
                    level = 15;
            }

            switch (level)
			{
                case 0:
                    experience = 0;
                    break;
                case 1:
                    experience = 100;
                    break;
                case 2:
                    experience = 300;
                    break;
                case 3:
                    experience = 600;
                    break;
                case 4:
                    experience = 1000;
                    break;
                case 5:
                    experience = 1500;
                    break;
                case 6:
                    experience = 2100;
                    break;
                case 7:
                    experience = 2800;
                    break;
                case 8:
                    experience = 3600;
                    break;
                case 9:
                    experience = 4500;
                    break;
                case 10:
                    experience = 5500;
                    break;
                case 11:
                    experience = 6600;
                    break;
                case 12:
                    experience = 7800;
                    break;
                case 13:
                    experience = 9100;
                    break;
                case 14:
                    experience = 10500;
                    break;
                case 15:
                    experience = 12500;
                    break;
            }
			Main.NewText($"Your {className} proficiency is now at level {level}", Color.AliceBlue);
        }
	}
}