using CalTestHelpers.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalTestHelpers
{
	public class CalTestHelpers : Mod
	{
		public static Mod Calamity = null;
		public static bool ShouldDisplayUIs = false;
		public static GrandUIRenderer UltimateUI = new GrandUIRenderer();
		public static BossDeathUIRenderer BossUIRender = new BossDeathUIRenderer();
		public static PermanentUpgradeUIRenderer UpgradeUIRenderer = new PermanentUpgradeUIRenderer();
		public static ProficiencyManipulatorUIRender ProficiencyUIRenderer = new ProficiencyManipulatorUIRender();
		public static ItemStatEditUIRenderer ItemEditerUIRenderer = new ItemStatEditUIRenderer();
		public static ProjectileStatEditUIRenderer ProjectileEditerUIRenderer = new ProjectileStatEditUIRenderer();
		public static List<SpecialUIElement> SecondaryUIElements = new List<SpecialUIElement>();

		public static GrandUIRenderer SecondaryUIToDisplay;

		public static ModHotKey ToggleUIsHotkey;
		public static int GlobalTickTimer { get; internal set; }

		internal static bool HaveAnyStatManipulationsBeenDone = false;
		internal static bool HasDonePostLoading = false;
		public override void Load()
		{
			ToggleUIsHotkey = RegisterHotKey("Toggle Test UIs", "Q");
			Calamity = ModLoader.GetMod("CalamityMod");
			ILEdits.Load();
		}

		public override void PostUpdateEverything()
		{
			GlobalTickTimer++;
			if (HasDonePostLoading)
				return;

			ItemOverrideCache.Load();
			ProjectileOverrideCache.Load();
			HasDonePostLoading = true;
		}

		public override void Unload()
		{
			ToggleUIsHotkey = null;
			Calamity = null;
			ILEdits.Unload();
			ProjectileOverrideCache.Unload();
			ItemOverrideCache.Unload();
		}

		public override object Call(params object[] args)
		{
			if (args.Length >= 2 && args[0] is string command)
			{
				switch (command.ToLower())
				{
					case "addtograndui":
						if (args[1] is SpecialUIElement renderer)
							SecondaryUIElements.Add(renderer);
						break;
				}
			}
			return null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
            int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
			if (mouseIndex != -1)
			{
				layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Special UIs", () =>
				{
					if (!Main.inFancyUI && Main.playerInventory)
					{
						UltimateUI.Draw(Main.spriteBatch);
						if (SecondaryUIToDisplay != null)
							SecondaryUIToDisplay.Draw(Main.spriteBatch);
					}

					// Draw a gear at the bottom of the screen if any stat manipulations have been done
					// This is done to prevent cheating by nohitters (who use this mod).
					// If you attempt to remove this behavior and do something like this, I am not responsible.
					if (HaveAnyStatManipulationsBeenDone)
					{
						Texture2D gearTexture = ModContent.GetTexture("CalTestHelpers/UI/Gear");
						Vector2 gearDrawPosition = new Vector2(Main.screenWidth - 400f, Main.screenHeight - 60f);
						Main.spriteBatch.Draw(gearTexture, gearDrawPosition, null, Color.Cyan, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public override void PostDrawFullscreenMap(ref string mouseText) => MapServices.TryToTeleportPlayerOnMap();
	}
}