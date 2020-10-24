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
		public static int BossKillTimeFrames;
		public static bool NoSpawns = false;
		public static List<int> BossKillDPS = new List<int>();
		public override TagCompound Save()
		{
			return new TagCompound()
			{
				["NoSpawns"] = NoSpawns
			};
		}
		public override void Load(TagCompound tag)
		{
			NoSpawns = tag.ContainsKey("NoSpawns");
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(NoSpawns);
		}
		public override void NetReceive(BinaryReader reader)
		{
			NoSpawns = reader.ReadBoolean();
		}
		public override void PostUpdate()
		{
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
									   $"Average DPS: {BossKillDPS.Average() }\n" +
									   $"Maximum DPS: {BossKillDPS.Max()}";

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