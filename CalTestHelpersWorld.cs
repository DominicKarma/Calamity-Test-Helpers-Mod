using CalamityMod.CalPlayer;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalTestHelpers
{
	public class CalTestHelpersWorld : ModWorld
	{
		public static int OriginalTime;
		public static int BossKillTimeFrames;
		public static bool NoSpawns = false;
		public static bool FrozenTime = false;
		public static List<int> BossKillDPS = new List<int>();
		public override TagCompound Save()
		{
			return new TagCompound()
			{
				["NoSpawns"] = NoSpawns,
				["FrozenTime"] = FrozenTime
			};
		}
		public override void Load(TagCompound tag)
		{
			NoSpawns = tag.GetBool("NoSpawns");
			FrozenTime = tag.GetBool("FrozenTime");
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(NoSpawns);
			writer.Write(FrozenTime);
		}
		public override void NetReceive(BinaryReader reader)
		{
			NoSpawns = reader.ReadBoolean();
			FrozenTime = reader.ReadBoolean();
		}

		public override void PostUpdate()
		{
			if (FrozenTime && Main.netMode == NetmodeID.SinglePlayer)
				Main.time -= Main.dayRate;

			bool anyMiscEventsGoingOn = (bool)CalTestHelpers.Calamity.Call("Difficulty", "bossrush") || CalamityWorld.DoGSecondStageCountdown > 0;
			if (CalamityPlayer.areThereAnyDamnBosses || anyMiscEventsGoingOn)
			{
				BossKillTimeFrames++;

				// Incorporate any new DPS values as they come.
				if (Main.LocalPlayer.dpsDamage != BossKillDPS.LastOrDefault())
				{
					BossKillDPS.Add(Main.LocalPlayer.dpsDamage);
				}
			}
			else if (BossKillTimeFrames > 0)
			{
				string timeString = TimeSpan.FromSeconds(BossKillTimeFrames / 60f).ToString(@"hh\:mm\:ss");
				string textToDisplay = $"Time Elapsed: {timeString}\n" +
									   $"Average DPS: {(BossKillDPS.Count == 0 ? 0f : BossKillDPS.Average())}\n" +
									   $"Maximum DPS: {(BossKillDPS.Count == 0 ? 0f : BossKillDPS.Max())}";

				// Newlines fuck text displays up.
				foreach (var snippet in textToDisplay.Split('\n'))
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
						Main.NewText(snippet, Color.Crimson);
					else if (Main.netMode == NetmodeID.Server)
						NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(snippet), Color.Crimson);
				}
				BossKillDPS.Clear();
				BossKillTimeFrames = 0;
			}
		}
	}
}