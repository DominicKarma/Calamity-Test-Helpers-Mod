using Terraria;

namespace CalTestHelpers
{
	public static partial class ILEdits
	{
		private static void OverrideItemSetDefaultData(On.Terraria.Item.orig_SetDefaults orig, Item self, int Type, bool noMatCheck)
		{
			orig(self, Type, noMatCheck);

			// This should only happen at load-time.
			if (ItemOverrideCache.DamageOverrides is null)
				return;

			if (ItemOverrideCache.DamageOverrides[Type] > 0)
				self.damage = ItemOverrideCache.DamageOverrides[Type];

			if (ItemOverrideCache.UseTimeOverrides[Type] > 0)
				self.useTime = ItemOverrideCache.UseTimeOverrides[Type];

			if (ItemOverrideCache.UseAnimationOverrides[Type] > 0)
				self.useAnimation = ItemOverrideCache.UseAnimationOverrides[Type];
		}

		private static void OverrideProjectileSetDefaultData(On.Terraria.Projectile.orig_SetDefaults orig, Projectile self, int Type)
		{
			orig(self, Type);

			// This should only happen at load-time.
			if (ProjectileOverrideCache.LocalIFrameOverrides is null)
				return;

			if (ProjectileOverrideCache.LocalIFrameOverrides[Type] != -2)
				self.localNPCHitCooldown = ProjectileOverrideCache.LocalIFrameOverrides[Type];

			if (ProjectileOverrideCache.StaticIFrameOverrides[Type] != -2)
				self.idStaticNPCHitCooldown = ProjectileOverrideCache.StaticIFrameOverrides[Type];
		}
	}
}