using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CalTestHelpers
{
	public class ItemOverrideCache
	{
		internal static List<Item> LoadedItems = new List<Item>();
		internal static List<string> ItemNames = new List<string>();

		public static int[] DamageOverrides;
		public static int[] UseTimeOverrides;
		public static int[] UseAnimationOverrides;

		internal static void Load()
		{
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				Item item = new Item();
				item.SetDefaults(i);

				LoadedItems.Add(item);

				string itemName = item.Name.ToLower();
				itemName = string.Concat(itemName.Where(c => !char.IsWhiteSpace(c)));

				ItemNames.Add(itemName);
			}

			DamageOverrides = new int[ItemNames.Count];
			UseTimeOverrides = new int[ItemNames.Count];
			UseAnimationOverrides = new int[ItemNames.Count];
		}

		internal static void Unload()
		{
			LoadedItems = null;
			ItemNames = null;
			DamageOverrides = null;
			UseTimeOverrides = null;
			UseAnimationOverrides = null;
		}

		public static void ResetOverrides()
		{
			DamageOverrides = new int[DamageOverrides.Length];
			UseTimeOverrides = new int[UseTimeOverrides.Length];
			UseAnimationOverrides = new int[UseAnimationOverrides.Length];
		}

		public static IEnumerable<int> AttemptToLocateItemsWithSimilarName(string name)
		{
			if (string.IsNullOrEmpty(name))
				yield break;

			name = name.ToLower();

			// Remove whitespace from the name.
			name = string.Concat(name.Where(c => !char.IsWhiteSpace(c)));

			IEnumerable<string> similarItemNames = ItemNames.Where(n => n.Contains(name)).Take(20);
			foreach (string itemName in similarItemNames)
				yield return ItemNames.IndexOf(itemName);
		}
	}
}