using CalamityMod;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CalTestHelpers.UI
{
	public class BossDeathUIRenderer : GrandUIRender
	{
		public enum Boss
		{
			KingSlime,
			DesertScourge,
			EyeOfCthulhu,
			Crabulon,
			EaterOfWorlds,
			BrainOfCthulhu,
			HiveMind,
			Perforators,
			QueenBee,
			Skeletron,
			SlimeGod,
			WallOfFlesh,
			Cryogen,
			TheTwins,
			BrimstoneElemental,
			TheDestroyer,
			TheAquaticScourge,
			SkeletronPrime,
			Cloneamitas,
			Plantera,
			Leviathan,
			AstrumAureus,
			Golem,
			PlaguebringerGoliath,
			DukeFishron,
			Ravager,
			LunaticCultist,
			AstrumDeus,
			MoonLord,
			ProfanedGuardians,
			Dragonfolly,
			Providence,
			CeaselessVoid,
			StormWeaver,
			Signus,
			Polterghast,
			OldDuke,
			DevourerOfGods,
			Yharon,
			SupremeCalamitas,
			All
		}
		public override List<SpecialUIElement> UIElements => new List<SpecialUIElement>()
		{
			new SpecialUIElement("Toggle King Slime's Death.", Main.npcHeadBossTexture[7], () => ToggleDeath(Boss.KingSlime)),
			new SpecialUIElement("Toggle The Desert Scourge's Death.", ModContent.GetTexture("CalamityMod/NPCs/DesertScourge/DesertScourgeHead_Head_Boss"), () => ToggleDeath(Boss.DesertScourge)),
			new SpecialUIElement("Toggle The Eye of Cthulhu's Death.", Main.npcHeadBossTexture[0], () => ToggleDeath(Boss.EyeOfCthulhu)),
			new SpecialUIElement("Toggle Crabulon's Death.", ModContent.GetTexture("CalamityMod/NPCs/Crabulon/CrabulonIdle_Head_Boss"), () => ToggleDeath(Boss.Crabulon)),
			new SpecialUIElement("Toggle The Eater of World's Death.", Main.npcHeadBossTexture[2], () => ToggleDeath(Boss.EaterOfWorlds)),
			new SpecialUIElement("Toggle The Brain of Cthulhu's Death.", Main.npcHeadBossTexture[23], () => ToggleDeath(Boss.BrainOfCthulhu)),
			new SpecialUIElement("Toggle The Hive Mind's Death.", ModContent.GetTexture("CalamityMod/NPCs/HiveMind/HiveMindP2_Head_Boss"), () => ToggleDeath(Boss.HiveMind)),
			new SpecialUIElement("Toggle The Perforator's Death.", ModContent.GetTexture("CalamityMod/NPCs/Perforator/PerforatorHive_Head_Boss"), () => ToggleDeath(Boss.Perforators)),
			new SpecialUIElement("Toggle The Queen Bee's Death.", Main.npcHeadBossTexture[14], () => ToggleDeath(Boss.QueenBee)),
			new SpecialUIElement("Toggle Skeletron's Death.", Main.npcHeadBossTexture[19], () => ToggleDeath(Boss.Skeletron)),
			new SpecialUIElement("Toggle The Slime God's Death.", ModContent.GetTexture("CalamityMod/NPCs/SlimeGod/SlimeGodCore_Head_Boss"), () => ToggleDeath(Boss.SlimeGod)),
			new SpecialUIElement("Toggle The Wall of Flesh's Death.", Main.npcHeadBossTexture[22], () => ToggleDeath(Boss.WallOfFlesh)),
			new SpecialUIElement("Toggle Cryogen's Death.", ModContent.GetTexture("CalamityMod/NPCs/Cryogen/Cryogen_Phase1_Head_Boss"), () => ToggleDeath(Boss.Cryogen)),
			new SpecialUIElement("Toggle The Twins' Death.", Main.npcHeadBossTexture[16], () => ToggleDeath(Boss.TheTwins)),
			new SpecialUIElement("Toggle The Brimstone Elemental's Death.", ModContent.GetTexture("CalamityMod/NPCs/BrimstoneElemental/BrimstoneElemental_Head_Boss"), () => ToggleDeath(Boss.BrimstoneElemental)),
			new SpecialUIElement("Toggle The Destroyer's Death.", Main.npcHeadBossTexture[25], () => ToggleDeath(Boss.TheDestroyer)),
			new SpecialUIElement("Toggle The Aquatic Scourge's Death.", ModContent.GetTexture("CalamityMod/NPCs/AquaticScourge/AquaticScourgeHead_Head_Boss"), () => ToggleDeath(Boss.TheAquaticScourge)),
			new SpecialUIElement("Toggle Skeletron Prime's Death.", Main.npcHeadBossTexture[18], () => ToggleDeath(Boss.SkeletronPrime)),
			new SpecialUIElement("Toggle The Calamitas Clone's Death.", ModContent.GetTexture("CalamityMod/NPCs/Calamitas/Calamitas_Head_Boss"), () => ToggleDeath(Boss.Cloneamitas)),
			new SpecialUIElement("Toggle Plantera's Death.", Main.npcHeadBossTexture[11], () => ToggleDeath(Boss.Plantera)),
			new SpecialUIElement("Toggle The Leviathan's Death.", ModContent.GetTexture("CalamityMod/NPCs/Leviathan/Leviathan_Head_Boss"), () => ToggleDeath(Boss.Leviathan)),
			new SpecialUIElement("Toggle Astrum Aureus' Death.", ModContent.GetTexture("CalamityMod/NPCs/AstrumAureus/AstrumAureus_Head_Boss"), () => ToggleDeath(Boss.AstrumAureus)),
			new SpecialUIElement("Toggle Golem's Death.", Main.npcHeadBossTexture[5], () => ToggleDeath(Boss.Golem)),
			new SpecialUIElement("Toggle The Plaguebringer Goliath's Death.", ModContent.GetTexture("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliath_Head_Boss"), () => ToggleDeath(Boss.PlaguebringerGoliath)),
			new SpecialUIElement("Toggle Duke Fishron's Death.", Main.npcHeadBossTexture[4], () => ToggleDeath(Boss.DukeFishron)),
			new SpecialUIElement("Toggle The Ravager's Death.", ModContent.GetTexture("CalamityMod/NPCs/Ravager/RavagerBody_Head_Boss"), () => ToggleDeath(Boss.Ravager)),
			new SpecialUIElement("Toggle The Lunatic Cultist's Death.", Main.npcHeadBossTexture[31], () => ToggleDeath(Boss.LunaticCultist)),
			new SpecialUIElement("Toggle Astrum Deus' Death.", ModContent.GetTexture("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadSpectral_Head_Boss"), () => ToggleDeath(Boss.AstrumDeus)),
			new SpecialUIElement("Toggle The Moon Lord's Death.", Main.npcHeadBossTexture[8], () => ToggleDeath(Boss.MoonLord)),
			new SpecialUIElement("Toggle The Profaned Guardian's Death.", ModContent.GetTexture("CalamityMod/NPCs/ProfanedGuardians/ProfanedGuardianBoss_Head_Boss"), () => ToggleDeath(Boss.ProfanedGuardians)),
			new SpecialUIElement("Toggle The Dragonfolly's Death.", ModContent.GetTexture("CalamityMod/NPCs/Bumblebirb/Birb_Head_Boss"), () => ToggleDeath(Boss.Dragonfolly)),
			new SpecialUIElement("Toggle Providence's Death.", ModContent.GetTexture("CalamityMod/NPCs/Providence/Providence_Head_Boss"), () => ToggleDeath(Boss.Providence)),
			new SpecialUIElement("Toggle The Ceasless Void's Death.", ModContent.GetTexture("CalamityMod/NPCs/CeaselessVoid/CeaselessVoid_Head_Boss"), () => ToggleDeath(Boss.CeaselessVoid)),
			new SpecialUIElement("Toggle The Storm Weaver's Death.", ModContent.GetTexture("CalamityMod/NPCs/StormWeaver/StormWeaverHeadNaked_Head_Boss"), () => ToggleDeath(Boss.StormWeaver)),
			new SpecialUIElement("Toggle Signus' Death.", ModContent.GetTexture("CalamityMod/NPCs/Signus/Signus_Head_Boss"), () => ToggleDeath(Boss.Signus)),
			new SpecialUIElement("Toggle The Polterghast's Death.", ModContent.GetTexture("CalamityMod/NPCs/Polterghast/Polterghast_Head_Boss"), () => ToggleDeath(Boss.Polterghast)),
			new SpecialUIElement("Toggle The Old Duke's Death.", ModContent.GetTexture("CalamityMod/NPCs/OldDuke/OldDuke_Head_Boss"), () => ToggleDeath(Boss.OldDuke)),
			new SpecialUIElement("Toggle The Devourer of Gods' Death.", ModContent.GetTexture("CalamityMod/NPCs/DevourerofGods/DevourerofGodsHeadS_Head_Boss"), () => ToggleDeath(Boss.DevourerOfGods)),
			new SpecialUIElement("Toggle Yharon's Death.", ModContent.GetTexture("CalamityMod/NPCs/Yharon/Yharon_Head_Boss"), () => ToggleDeath(Boss.Yharon)),
			new SpecialUIElement("Toggle Supreme Calamitas' Death.", ModContent.GetTexture("CalamityMod/NPCs/SupremeCalamitas/SupremeCalamitas_Head_Boss"), () => ToggleDeath(Boss.SupremeCalamitas)),
			new SpecialUIElement("Toggle every boss' Death.", ModContent.GetTexture("CalamityMod/Items/DifficultyItems/Death"), () => ToggleDeath(Boss.All)),
		};
		public override float UIScale => 0.65f * ResolutionRatio;

		public override Vector2 TopLeftLocation => new Vector2(Main.screenWidth - 660 - 270 * ResolutionRatio, 5);

		public static void ToggleDeath(Boss bossDeathToToggle)
		{
			if (bossDeathToToggle == Boss.All)
			{
				ToggleAllBossDeaths();
				return;
			}
			string bossName = string.Empty;
			Color textColor = Color.White;
			ref bool bossDeathValue = ref NPC.downedSlimeKing;
			switch (bossDeathToToggle)
			{
				case Boss.KingSlime:
					bossName = "King Slime";
					textColor = new Color(96, 170, 255);
					bossDeathValue = ref NPC.downedSlimeKing;
					break;
				case Boss.DesertScourge:
					bossName = "The Desert Scourge";
					textColor = new Color(216, 151, 82);
					bossDeathValue = ref CalamityWorld.downedDesertScourge;
					break;
				case Boss.EyeOfCthulhu:
					bossName = "The Eye of Cthulhu";
					textColor = new Color(216, 116, 114);
					bossDeathValue = ref NPC.downedBoss1;
					break;
				case Boss.Crabulon:
					bossName = "Crabulon";
					textColor = new Color(0, 184, 216);
					bossDeathValue = ref CalamityWorld.downedCrabulon;
					break;
				case Boss.EaterOfWorlds:
					bossName = "The Eater of Worlds";
					textColor = new Color(160, 131, 201);
					bossDeathValue = ref NPC.downedBoss2;
					break;
				case Boss.BrainOfCthulhu:
					bossName = "The Brain of Cthulhu";
					textColor = new Color(214, 147, 182);
					bossDeathValue = ref NPC.downedBoss2;
					break;
				case Boss.HiveMind:
					bossName = "The Hive Mind";
					textColor = new Color(160, 131, 201);
					bossDeathValue = ref CalamityWorld.downedHiveMind;
					break;
				case Boss.Perforators:
					bossName = "The Perforators";
					textColor = new Color(214, 147, 182);
					bossDeathValue = ref CalamityWorld.downedPerforator;
					break;
				case Boss.QueenBee:
					bossName = "The Queen Bee";
					textColor = new Color(216, 205, 2);
					bossDeathValue = ref NPC.downedQueenBee;
					break;
				case Boss.Skeletron:
					bossName = "Skeletron";
					textColor = new Color(183, 92, 214);
					bossDeathValue = ref NPC.downedBoss3;
					break;
				case Boss.SlimeGod:
					bossName = "The Slime God";
					textColor = new Color(182, 0, 164);
					bossDeathValue = ref CalamityWorld.downedSlimeGod;
					break;
				case Boss.WallOfFlesh:
					bossName = "The Wall of Flesh";
					textColor = new Color(192, 106, 122);
					bossDeathValue = ref Main.hardMode;
					break;
				case Boss.Cryogen:
					bossName = "Cryogen";
					textColor = new Color(174, 229, 239);
					bossDeathValue = ref CalamityWorld.downedCryogen;
					break;
				case Boss.TheTwins:
					bossName = "The Twins";
					textColor = new Color(147, 189, 198);
					bossDeathValue = ref NPC.downedMechBoss2;
					break;
				case Boss.BrimstoneElemental:
					bossName = "The Brimstone Elemental";
					textColor = new Color(196, 7, 102);
					bossDeathValue = ref CalamityWorld.downedBrimstoneElemental;
					break;
				case Boss.TheDestroyer:
					bossName = "The Destroyer";
					textColor = new Color(147, 189, 198);
					bossDeathValue = ref NPC.downedMechBoss1;
					break;
				case Boss.TheAquaticScourge:
					bossName = "The Aquatic Scourge";
					textColor = new Color(54, 156, 196);
					bossDeathValue = ref CalamityWorld.downedAquaticScourge;
					break;
				case Boss.SkeletronPrime:
					bossName = "Skeletron Prime";
					textColor = new Color(147, 189, 198);
					bossDeathValue = ref NPC.downedMechBoss3;
					break;
				case Boss.Cloneamitas:
					bossName = "The Calamitas Clone";
					textColor = new Color(204, 3, 0);
					bossDeathValue = ref CalamityWorld.downedCalamitas;
					break;
				case Boss.Plantera:
					bossName = "Plantera";
					textColor = new Color(204, 89, 209);
					bossDeathValue = ref NPC.downedPlantBoss;
					break;
				case Boss.Leviathan:
					bossName = "Anahita and the Leviathan";
					textColor = new Color(0, 199, 206);
					bossDeathValue = ref CalamityWorld.downedLeviathan;
					break;
				case Boss.AstrumAureus:
					bossName = "Astrum Aureus";
					textColor = new Color(237, 93, 83);
					bossDeathValue = ref CalamityWorld.downedAstrageldon;
					break;
				case Boss.Golem:
					bossName = "Golem";
					textColor = new Color(188, 62, 0);
					bossDeathValue = ref NPC.downedGolemBoss;
					break;
				case Boss.PlaguebringerGoliath:
					bossName = "The Plaguebringer Goliath";
					textColor = new Color(73, 130, 57);
					bossDeathValue = ref CalamityWorld.downedPlaguebringer;
					break;
				case Boss.DukeFishron:
					bossName = "Duke Fishron";
					textColor = new Color(89, 204, 183);
					bossDeathValue = ref NPC.downedFishron;
					break;
				case Boss.Ravager:
					bossName = "The Ravager";
					textColor = new Color(88, 97, 189);
					bossDeathValue = ref CalamityWorld.downedScavenger;
					break;
				case Boss.LunaticCultist:
					bossName = "The Lunatic Cultist";
					textColor = new Color(112, 132, 211);
					bossDeathValue = ref NPC.downedAncientCultist;
					break;
				case Boss.AstrumDeus:
					bossName = "Astrum Deus";
					textColor = new Color(66, 189, 181);
					bossDeathValue = ref CalamityWorld.downedStarGod;
					break;
				case Boss.MoonLord:
					bossName = "The Moon Lord";
					textColor = new Color(0, 215, 155);
					bossDeathValue = ref NPC.downedMoonlord;
					break;
				case Boss.ProfanedGuardians:
					bossName = "The Profaned Guardians";
					textColor = new Color(255, 159, 0);
					bossDeathValue = ref CalamityWorld.downedGuardians;
					break;
				case Boss.Dragonfolly:
					bossName = "The Dragonfolly";
					textColor = new Color(255, 20, 20);
					bossDeathValue = ref CalamityWorld.downedBumble;
					break;
				case Boss.Providence:
					bossName = "Providence";
					textColor = new Color(255, 159, 0);
					bossDeathValue = ref CalamityWorld.downedProvidence;
					break;
				case Boss.CeaselessVoid:
					bossName = "The Ceaseless Void";
					textColor = new Color(125, 100, 153);
					bossDeathValue = ref CalamityWorld.downedSentinel1;
					break;
				case Boss.StormWeaver:
					bossName = "The Storm Weaver";
					textColor = new Color(235, 100, 153);
					bossDeathValue = ref CalamityWorld.downedSentinel2;
					break;
				case Boss.Signus:
					bossName = "Signus";
					textColor = new Color(143, 101, 228);
					bossDeathValue = ref CalamityWorld.downedSentinel3;
					break;
				case Boss.Polterghast:
					bossName = "Polterghast";
					textColor = new Color(35, 200, 254);
					bossDeathValue = ref CalamityWorld.downedPolterghast;
					break;
				case Boss.OldDuke:
					bossName = "The Old Duke";
					textColor = new Color(133, 180, 49);
					bossDeathValue = ref CalamityWorld.downedBoomerDuke;
					break;
				case Boss.DevourerOfGods:
					bossName = "The Devourer of Gods";
					textColor = new Color(0, 255, 255);
					bossDeathValue = ref CalamityWorld.downedDoG;
					break;
				case Boss.Yharon:
					bossName = "Yharon";
					textColor = new Color(255, 182, 55);
					bossDeathValue = ref CalamityWorld.downedYharon;
					break;
				case Boss.SupremeCalamitas:
					bossName = "Supreme Calamitas";
					textColor = new Color(255, 0, 0);
					bossDeathValue = ref CalamityWorld.downedSCal;
					break;
			}
			bossDeathValue = !bossDeathValue;
			NPC.downedMechBossAny = NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3;
			string bossRefernceText = bossName.Last() == 's' ? bossName + "'" : bossName + "'s";
			Main.NewText($"{bossRefernceText} death is now marked as: {bossDeathValue}", textColor);
		}

		public static void ToggleAllBossDeaths()
		{
			bool killAll = false;
			if (NPC.downedSlimeKing)
			{
				killAll = true;
			}
			Main.NewText($"All bosses are now marked as {(killAll ? "alive" : "dead")}", Color.Red);

			NPC.downedSlimeKing = CalamityWorld.downedDesertScourge = NPC.downedBoss1 = !killAll;
			CalamityWorld.downedCrabulon = NPC.downedBoss2 = CalamityWorld.downedHiveMind = CalamityWorld.downedPerforator = !killAll;
			NPC.downedQueenBee = NPC.downedBoss3 = CalamityWorld.downedSlimeGod = Main.hardMode = !killAll;
			NPC.downedMechBoss1 = NPC.downedMechBoss2 = NPC.downedMechBoss3 = !killAll;
			CalamityWorld.downedBrimstoneElemental = CalamityWorld.downedAquaticScourge = CalamityWorld.downedCryogen = !killAll;
			CalamityWorld.downedCalamitas = NPC.downedPlantBoss = CalamityWorld.downedLeviathan = CalamityWorld.downedAstrageldon = !killAll;
			NPC.downedGolemBoss = CalamityWorld.downedPlaguebringer = NPC.downedFishron = CalamityWorld.downedScavenger = !killAll;
			NPC.downedAncientCultist = CalamityWorld.downedStarGod = NPC.downedMoonlord = !killAll;
			CalamityWorld.downedGuardians = CalamityWorld.downedBumble = CalamityWorld.downedProvidence = !killAll;
			CalamityWorld.downedSentinel1 = CalamityWorld.downedSentinel2 = CalamityWorld.downedSentinel3 = !killAll;
			CalamityWorld.downedPolterghast = CalamityWorld.downedBoomerDuke = CalamityWorld.downedDoG = !killAll;
			CalamityWorld.downedYharon = CalamityWorld.downedSCal = !killAll;
			NPC.downedMechBossAny = !killAll;
		}
	}
}