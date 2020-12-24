using CalTestHelpers.UI;
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
		public static GrandUIRender UltimateUI = new GrandUIRender();
		public static BossDeathUIRenderer BossUIRender = new BossDeathUIRenderer();
		public static PermanentUpgradeUIRenderer UpgradeUIRenderer = new PermanentUpgradeUIRenderer();
		public static ProficiencyManipulatorUIRender ProficiencyUIRenderer = new ProficiencyManipulatorUIRender();
		public static List<SpecialUIElement> SecondaryUIElements = new List<SpecialUIElement>();

		public static GrandUIRender SecondaryUIToDisplay;

		public static ModHotKey ToggleUIsHotkey;
		public override void Load()
		{
			ToggleUIsHotkey = RegisterHotKey("Toggle Test UIs", "Q");
			Calamity = ModLoader.GetMod("CalamityMod");
		}

		public override void Unload()
		{
			ToggleUIsHotkey = null;
			Calamity = null;
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
					if (!Main.inFancyUI)
					{
						UltimateUI.Draw(Main.spriteBatch);
						if (SecondaryUIToDisplay != null)
							SecondaryUIToDisplay.Draw(Main.spriteBatch);
					}
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public override void PostDrawFullscreenMap(ref string mouseText) => MapServices.TryToTeleportPlayerOnMap();
	}
}