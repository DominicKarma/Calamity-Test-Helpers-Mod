using Terraria;
using Terraria.ModLoader;

namespace CalTestHelpers
{
	public class CalTestHelpersNPC : GlobalNPC
	{
		public override bool Autoload(ref string name) => true;

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (CalTestHelpersWorld.NoSpawns)
			{
				spawnRate = 0;
				maxSpawns = 0;
			}
		}
	}
}