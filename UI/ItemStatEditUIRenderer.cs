using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.UI;

namespace CalTestHelpers.UI
{
	public class ItemStatEditUIRenderer : GrandUIRenderer
	{
		public Item ItemBeingEdited = null;
		public string ItemText = string.Empty;
		public bool EditingItemText = false;
		public override float UIScale => 0.65f * ResolutionRatio;

		public override Vector2 TopLeftLocation => new Vector2(Main.screenWidth - 660 - 270 * ResolutionRatio, 5);

		public override void DrawElements(SpriteBatch spriteBatch, float top)
		{
			Texture2D backgroundTexture = Main.chatBackTexture;
			Texture2D textBackgroundTexture = Main.clothesStyleBackTexture;
			Vector2 baseDrawPosition = TopLeftLocation + Vector2.UnitY * (backgroundTexture.Width + 60f) * ResolutionRatio;
			Vector2 textDrawPosition = baseDrawPosition + new Vector2(175f, 12f) * ResolutionRatio * 0.75f;
			Vector2 backgroundDrawPosition = baseDrawPosition + Vector2.UnitX * backgroundTexture.Width * ResolutionRatio * 0.75f;

			Vector2 itemDrawPosition = baseDrawPosition;
			itemDrawPosition += Vector2.UnitX * backgroundTexture.Width * ResolutionRatio * 0.275f;
			itemDrawPosition += Vector2.UnitY * 72f * ResolutionRatio;

			float backgroundScale = ResolutionRatio;
			Vector2 textBackgroundScale = new Vector2(backgroundScale * backgroundTexture.Width / textBackgroundTexture.Width);
			textBackgroundScale.Y *= 0.5f;

			Vector2 backgroundOrigin = Vector2.UnitX * backgroundTexture.Size() * 0.5f;
			Vector2 textBackgroundOrigin = Vector2.UnitX * textBackgroundTexture.Size() * 0.5f;

			// Draw the backgrounds.
			spriteBatch.Draw(backgroundTexture, backgroundDrawPosition, null, Color.White, 0f, backgroundOrigin, backgroundScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(textBackgroundTexture, backgroundDrawPosition, null, Color.Cyan, 0f, textBackgroundOrigin, textBackgroundScale, SpriteEffects.None, 0f);

			bool pressingMouseLeft = Main.mouseLeft && Main.mouseLeftRelease;

			Vector2 textBackgroundCenter = backgroundDrawPosition + Main.screenPosition;
			textBackgroundCenter.Y += ResolutionRatio * 24f;

			Rectangle textBackgroundArea = Utils.CenteredRectangle(textBackgroundCenter, textBackgroundTexture.Size() * textBackgroundScale);
			bool mouseOverBackground = CalamityUtils.MouseHitbox.Intersects(textBackgroundArea);

			// Edit text if clicking in the bounds of the background.
			if (mouseOverBackground && pressingMouseLeft)
				EditingItemText = true;

			// Otherwise stop editing text if clicking outside of the background.
			else if (!mouseOverBackground && pressingMouseLeft)
				EditingItemText = false;

			if (ItemBeingEdited != null)
				DrawItemStatManipulatorUI(spriteBatch);

			if (EditingItemText)
				EditText(ref ItemText);

			// As well as the text.
			Utils.DrawBorderStringBig(spriteBatch, ItemText, textDrawPosition, Color.White, UIScale);

			IEnumerable<int> itemTypes = ItemOverrideCache.AttemptToLocateItemsWithSimilarName(ItemText);

			int itemCounter = 0;
			int moveToNextLineRate = (int)((textBackgroundTexture.Width * ResolutionRatio - 18) / 36);
			bool hoveringOverIcon = false;
			foreach (int itemType in itemTypes)
			{
				Vector2 instancedItemDrawPosition = itemDrawPosition;

				// Ensure that the item positions never leave the box.
				instancedItemDrawPosition.X += itemCounter % moveToNextLineRate * 36f;
				instancedItemDrawPosition.Y += itemCounter / moveToNextLineRate * 36f;
				Item itemToDraw = ItemOverrideCache.LoadedItems[itemType].Clone();

				Rectangle icon = Utils.CenteredRectangle(instancedItemDrawPosition + Main.screenPosition, Vector2.One * 32f);
				icon.X += 16;
				icon.Y += 16;

				if (CalamityUtils.MouseHitbox.Intersects(icon))
				{
					Main.LocalPlayer.mouseInterface = Main.blockMouse = true;
					Main.HoverItem = itemToDraw;
					hoveringOverIcon = true;

					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						if (ItemBeingEdited is null)
							ItemBeingEdited = Main.HoverItem.Clone();
						else
							ItemBeingEdited = null;
						EditingItemText = true;
					}
				}

				// Draw the item in a slot.
				ItemSlot.Draw(spriteBatch, ref itemToDraw, 0, instancedItemDrawPosition);

				itemCounter++;
			}

			// Draw the item details when hovering over the icon box.
			if (hoveringOverIcon)
				Main.instance.MouseTextHackZoom(string.Empty);
		}

		public void DrawItemStatManipulatorUI(SpriteBatch spriteBatch)
		{
			bool readyToDoInputUpdate = CalTestHelpers.GlobalTickTimer % 4 == 0;
			Texture2D elementBackgroundTexture = Main.clothesStyleBackTexture;
			Vector2 elementBackgroundScale = new Vector2(1f, 0.45f) * ResolutionRatio;
			Vector2 statManipulatorOrigin = Vector2.UnitX * elementBackgroundTexture.Size() * 0.5f;
			Vector2 statManipulatorPosition = TopLeftLocation - Vector2.UnitX * (elementBackgroundTexture.Width * 0.5f - 120f) * ResolutionRatio;

			// List certain things to change based on what the item is, along with a manipulation UI.

			int totalIcons = 0;

			// Damage, useTime, useAnimation, ect.
			if (ItemBeingEdited.damage > 0 && !ItemBeingEdited.accessory)
			{
				drawCustomManipulatorIcon($" {ItemBeingEdited.Name} Damage: {ItemBeingEdited.damage}", () =>
				{
					int damage = ItemBeingEdited.damage;
					if (Main.mouseLeft)
					{
						if (Main.keyState.PressingShift())
							damage--;
						else
							damage++;
						if (damage <= 0)
							damage = 1;

						if (ItemBeingEdited.damage != damage)
						{
							ResetItemStats();
							ItemBeingEdited.damage = damage;
						}
					}
				});
			}

			if (ItemBeingEdited.useTime > 0)
			{
				drawCustomManipulatorIcon($" {ItemBeingEdited.Name} Use Time: {ItemBeingEdited.useTime}", () =>
				{
					int useTime = ItemBeingEdited.useTime;
					if (Main.mouseLeft)
					{
						if (Main.keyState.PressingShift())
							useTime--;
						else
							useTime++;
						if (useTime >= 125)
							useTime = 125;
						if (useTime <= 0)
							useTime = 1;

						if (ItemBeingEdited.useTime != useTime)
						{
							ResetItemStats();
							ItemBeingEdited.useTime = useTime;
						}
					}
				});
			}

			if (ItemBeingEdited.useAnimation > 0)
			{
				drawCustomManipulatorIcon($" {ItemBeingEdited.Name} Use Animation: {ItemBeingEdited.useAnimation}", () =>
				{
					int useAnimation = ItemBeingEdited.useAnimation;
					if (Main.mouseLeft)
					{
						if (Main.keyState.PressingShift())
							useAnimation--;
						else
							useAnimation++;
						if (useAnimation >= 125)
							useAnimation = 125;
						if (useAnimation <= 0)
							useAnimation = 1;

						if (ItemBeingEdited.useAnimation != useAnimation)
						{
							ResetItemStats();
							ItemBeingEdited.useAnimation = useAnimation;
						}
					}
				});
			}

			// Bleh.
			void drawCustomManipulatorIcon(string text, Action hoverOverEffects)
			{
				Vector2 baseIconDrawPosition = statManipulatorPosition;
				baseIconDrawPosition += Vector2.UnitY * elementBackgroundTexture.Height * elementBackgroundScale.Y * totalIcons;

				Vector2 topLeft = baseIconDrawPosition;
				topLeft += elementBackgroundTexture.Size() * elementBackgroundScale * new Vector2(0f, 0.5f);
				topLeft += Main.screenPosition;

				Vector2 textDrawPosition = baseIconDrawPosition;
				textDrawPosition -= elementBackgroundTexture.Size() * elementBackgroundScale * new Vector2(0.5f, -0.15f);

				Color drawColor = Color.White;
				if (CalamityUtils.MouseHitbox.Intersects(Utils.CenteredRectangle(topLeft, elementBackgroundTexture.Size() * elementBackgroundScale)))
				{
					drawColor = Color.BlueViolet;
					if (readyToDoInputUpdate)
						hoverOverEffects();
				}

				spriteBatch.Draw(elementBackgroundTexture, baseIconDrawPosition, null, drawColor, 0f, statManipulatorOrigin, elementBackgroundScale, SpriteEffects.None, 0f);
				Utils.DrawBorderStringBig(spriteBatch, text, textDrawPosition, Color.White, UIScale);
				totalIcons++;
			}
		}

		public void ResetItemStats()
		{
			int itemType = ItemBeingEdited.type;
			ItemOverrideCache.DamageOverrides[itemType] = ItemBeingEdited.damage;
			ItemOverrideCache.UseTimeOverrides[itemType] = ItemBeingEdited.useTime;
			ItemOverrideCache.UseAnimationOverrides[itemType] = ItemBeingEdited.useAnimation;
			CalTestHelpers.HaveAnyStatManipulationsBeenDone = true;
		}

		public void EditText(ref string text)
		{
			if (!EditingItemText)
				return;

			if (!IngameFancyUI.CanShowVirtualKeyboard(1) || UIVirtualKeyboard.KeyboardContext != 1)
			{
				Main.editSign = true;
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();
				text = Main.GetInputText(text);
				if (Main.inputTextEnter)
				{
					text += Encoding.ASCII.GetString(new byte[] { 10 });
					EditingItemText = false;
				}
				if (Main.inputTextEscape)
					EditingItemText = false;
			}
		}
	}
}