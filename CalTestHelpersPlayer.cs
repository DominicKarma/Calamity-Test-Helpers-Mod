using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalTestHelpers
{
	public class CalTestHelpersPlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CalTestHelpers.ToggleUIsHotkey.JustPressed)
				CalTestHelpers.ShouldDisplayUIs = !CalTestHelpers.ShouldDisplayUIs;
		}
	}
}