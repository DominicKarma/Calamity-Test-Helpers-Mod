using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalTestHelpers
{
	public class CalTestHelpersPlayer : ModPlayer
	{
		// Infinite money at all times.
		public override void PostUpdate()
		{
			if (player.inventory[50].type != ItemID.PlatinumCoin ||
				player.inventory[50].stack != 999)
			{
				player.inventory[50].SetDefaults(ItemID.PlatinumCoin);
				player.inventory[50].stack = 999;
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CalTestHelpers.ToggleUIsHotkey.JustPressed)
				CalTestHelpers.ShouldDisplayUIs = !CalTestHelpers.ShouldDisplayUIs;
		}
	}
}