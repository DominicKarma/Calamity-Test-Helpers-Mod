namespace CalTestHelpers
{
	public static partial class ILEdits
	{
		public static void Load()
		{
			On.Terraria.Item.SetDefaults += OverrideItemSetDefaultData;
			On.Terraria.Projectile.SetDefaults += OverrideProjectileSetDefaultData;
		}

		public static void Unload()
		{
			On.Terraria.Item.SetDefaults -= OverrideItemSetDefaultData;
			On.Terraria.Projectile.SetDefaults -= OverrideProjectileSetDefaultData;
		}
	}
}