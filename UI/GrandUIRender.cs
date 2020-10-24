using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalTestHelpers.UI
{
	public class GrandUIRender
	{
		public virtual List<SpecialUIElement> UIElements => new List<SpecialUIElement>()
		{
			new SpecialUIElement("Reveal the entire map.", ModContent.GetTexture("CalTestHelpers/UI/EyeTexture"), MapServices.RevealTheEntireMap),
			new SpecialUIElement("Set your spawn point to your position.", Main.itemTexture[ItemID.GoldenBed], () =>
			{
				Main.spawnTileX = (int)(Main.LocalPlayer.position.X - 8 + Main.LocalPlayer.width / 2) / 16;
				Main.spawnTileY = (int)(Main.LocalPlayer.position.Y + Main.LocalPlayer.height) / 16;
				Main.NewText($"Spawn point set. Your new spawn point is: { new Vector2(Main.spawnTileX, Main.spawnTileY) }");
			}),
			new SpecialUIElement("Toggle enemy spawns.", ModContent.GetTexture("CalTestHelpers/UI/EnemyIcon"), () =>
			{
				CalTestHelpersWorld.NoSpawns = !CalTestHelpersWorld.NoSpawns;
				if (CalTestHelpersWorld.NoSpawns)
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						bool isPillar = Main.npc[i].type == NPCID.LunarTowerNebula ||
										Main.npc[i].type == NPCID.LunarTowerSolar ||
										Main.npc[i].type == NPCID.LunarTowerStardust ||
										Main.npc[i].type == NPCID.LunarTowerVortex;
						if (isPillar)
							continue;
						if (Main.npc[i] != null && !Main.npc[i].townNPC)
						{
							Main.npc[i].life = 0;
							if (Main.netMode == NetmodeID.Server)
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i, 0f, 0f, 0f, 0);
						}
					}
				}
				Main.NewText($"Enemies now { (CalTestHelpersWorld.NoSpawns ? "cannot" : "can") } spawn.");
			}),
			new SpecialUIElement("Toggle boss deaths.", ModContent.GetTexture("CalTestHelpers/UI/Blades"), () =>
			{
				CalTestHelpers.SecondaryUIToDisplay = CalTestHelpers.SecondaryUIToDisplay is null ? CalTestHelpers.BossUIRender : null;
			}),
			new SpecialUIElement("Toggle permanent upgrades.", ModContent.GetTexture("CalamityMod/Items/PermanentBoosters/BloodOrange"), () =>
			{
				CalTestHelpers.SecondaryUIToDisplay = CalTestHelpers.SecondaryUIToDisplay is null ? CalTestHelpers.UpgradeUIRenderer : null;
			}),
			new SpecialUIElement("Update proficiency.", ModContent.GetTexture("CalTestHelpers/UI/UpwardBoost"), () =>
			{
				CalTestHelpers.SecondaryUIToDisplay = CalTestHelpers.SecondaryUIToDisplay is null ? CalTestHelpers.ProficiencyUIRenderer : null;
			}),
		};

		public float ResolutionRatio => Main.screenWidth / 2560f;

		public virtual Vector2 TopLeftLocation => new Vector2(Main.screenWidth - 660, 40);

		public virtual float UIScale => ResolutionRatio;

		public virtual Color TextColor => Color.Cyan;

		public virtual Vector2 IconBounds => new Vector2(44f * UIScale);

		public void Draw(SpriteBatch spriteBatch)
		{
			float yArea = TopLeftLocation.Y;
			Texture2D categorySlotTexture = ModContent.GetTexture("CalTestHelpers/UI/CategorySlot");
			if (GetType() == typeof(GrandUIRender))
			{
				Texture2D toggleIcon = ModContent.GetTexture("CalTestHelpers/UI/GrandUIToggle");
				Rectangle currentRectangleArea = new Rectangle((int)TopLeftLocation.X, (int)(yArea - 44 * ResolutionRatio), (int)IconBounds.X, (int)IconBounds.Y);
				Rectangle currentRectangleAreaWorld = new Rectangle((int)(TopLeftLocation.X + Main.screenPosition.X), (int)(currentRectangleArea.Y + Main.screenPosition.Y), (int)IconBounds.X, (int)IconBounds.Y);
				spriteBatch.Draw(toggleIcon, currentRectangleArea.Center(), null, Color.White, 0f, toggleIcon.Size() * 0.5f, ResolutionRatio * 0.6f, SpriteEffects.None, 0f);

				if (CalamityUtils.MouseHitbox.Intersects(currentRectangleAreaWorld))
				{
					// Activate the event if the button is pressed.
					Main.blockMouse = Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						CalTestHelpers.ShouldDisplayUIs = !CalTestHelpers.ShouldDisplayUIs;
					}
				}
			}

			if (!CalTestHelpers.ShouldDisplayUIs)
				return;
			
			foreach (var button in UIElements)
			{
				Rectangle currentRectangleArea = new Rectangle((int)TopLeftLocation.X, (int)yArea, (int)IconBounds.X, (int)IconBounds.Y);
				Rectangle currentRectangleAreaWorld = new Rectangle((int)(TopLeftLocation.X + Main.screenPosition.X), (int)(yArea + Main.screenPosition.Y), (int)IconBounds.X, (int)IconBounds.Y);

				spriteBatch.Draw(categorySlotTexture, currentRectangleArea.TopLeft(), null, Color.White, 0f, Vector2.Zero, UIScale, SpriteEffects.None, 0f);

				float iconScale = 0.75f * UIScale / (button.IconTexture.Size().Length() / IconBounds.Length());
				spriteBatch.Draw(button.IconTexture, currentRectangleArea.Center(), null, Color.White, 0f, button.IconTexture.Size() * 0.5f, iconScale, SpriteEffects.None, 0f);
				button.DrawDescription(spriteBatch, currentRectangleArea.TopRight() + new Vector2(4f, IconBounds.Y * 0.25f), TextColor, UIScale);

				if (button.OnClick != null && CalamityUtils.MouseHitbox.Intersects(currentRectangleAreaWorld))
				{
					Main.blockMouse = Main.LocalPlayer.mouseInterface = true;
					// Activate the event if the button is pressed.
					if ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease))
					{
						button.OnClick();
					}
				}
				yArea += IconBounds.Y;
			}
		}
	}
}