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
	public class ProjectileStatEditUIRenderer : GrandUIRenderer
	{
		public Projectile ProjectileBeingEdited = null;
		public string ProjectileText = string.Empty;
		public bool EditingProjectileText = false;
		public override float UIScale => 0.65f * ResolutionRatio;

		public override Vector2 TopLeftLocation => new Vector2(Main.screenWidth - 660 - 270 * ResolutionRatio, 5);

		public override void DrawElements(SpriteBatch spriteBatch, float top)
		{
			Texture2D backgroundTexture = Main.chatBackTexture;
			Texture2D textBackgroundTexture = Main.clothesStyleBackTexture;
			Vector2 baseDrawPosition = TopLeftLocation + Vector2.UnitY * (backgroundTexture.Width + 60f) * ResolutionRatio;
			Vector2 textDrawPosition = baseDrawPosition + new Vector2(175f, 12f) * ResolutionRatio * 0.75f;
			Vector2 backgroundDrawPosition = baseDrawPosition + Vector2.UnitX * backgroundTexture.Width * ResolutionRatio * 0.75f;

			Vector2 projectileDrawPosition = baseDrawPosition;
			projectileDrawPosition += Vector2.UnitX * backgroundTexture.Width * ResolutionRatio * 0.275f;
			projectileDrawPosition += Vector2.UnitY * 72f * ResolutionRatio;

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
				EditingProjectileText = true;

			// Otherwise stop editing text if clicking outside of the background.
			else if (!mouseOverBackground && pressingMouseLeft)
				EditingProjectileText = false;

			if (ProjectileBeingEdited != null)
				DrawProjectileStatManipulatorUI(spriteBatch);

			if (EditingProjectileText)
				EditText(ref ProjectileText);

			// As well as the text.
			Utils.DrawBorderStringBig(spriteBatch, ProjectileText, textDrawPosition, Color.White, UIScale);

			IEnumerable<int> projectileTypes = ProjectileOverrideCache.AttemptToLocateProjectilesWithSimilarName(ProjectileText);

			int projectileCounter = 0;
			int moveToNextLineRate = (int)((textBackgroundTexture.Width * ResolutionRatio - 18) / 36);
			bool hoveringOverIcon = false;
			foreach (int projectileType in projectileTypes)
			{
				Vector2 instancedProjectileDrawPosition = projectileDrawPosition;

				// Ensure that the projectile positions never leave the box.
				instancedProjectileDrawPosition += Vector2.One * ResolutionRatio * 25f;
				instancedProjectileDrawPosition.X += projectileCounter % moveToNextLineRate * 36f;
				instancedProjectileDrawPosition.Y += projectileCounter / moveToNextLineRate * 36f;

				Projectile projectileToDraw = new Projectile();
				projectileToDraw.CloneDefaults(projectileType);

				Rectangle icon = Utils.CenteredRectangle(instancedProjectileDrawPosition + Main.screenPosition, Vector2.One * 32f);

				if (CalamityUtils.MouseHitbox.Intersects(icon))
				{
					Main.LocalPlayer.mouseInterface = Main.blockMouse = true;
					Main.instance.MouseText(Lang.GetProjectileName(projectileType).Value);
					hoveringOverIcon = true;

					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						if (ProjectileBeingEdited is null)
						{
							ProjectileBeingEdited = new Projectile();
							ProjectileBeingEdited.SetDefaults(projectileType);
						}
						else
							ProjectileBeingEdited = null;
						EditingProjectileText = true;
					}
				}

				// Draw the projectile.
				Main.instance.LoadProjectile(projectileType);
				Texture2D projectileTexture = Main.projectileTexture[projectileType];
				int totalFrames = Main.projFrames[projectileType];

				// This assumption of only 1 horizontal frame could be wrong, but there's no easily accessable
				// way of determining if there is a horizontal frame or not.
				Rectangle projectileFrame = projectileTexture.Frame(1, totalFrames, 0, (int)(Main.GlobalTime * 6.6f % totalFrames));
				Vector2 scale = Vector2.Min(new Vector2(36f) / MathHelper.Max(projectileFrame.Height, projectileFrame.Width), new Vector2(1f));
				spriteBatch.Draw(projectileTexture, instancedProjectileDrawPosition, projectileFrame, Color.White, 0f, projectileFrame.Size() * 0.5f, scale, SpriteEffects.None, 0f);

				projectileCounter++;
			}

			// Draw the projectile details when hovering over the icon box.
			if (hoveringOverIcon)
				Main.instance.MouseTextHackZoom(string.Empty);
		}

		public void DrawProjectileStatManipulatorUI(SpriteBatch spriteBatch)
		{
			bool readyToDoInputUpdate = CalTestHelpers.GlobalTickTimer % 4 == 0;
			Texture2D elementBackgroundTexture = Main.clothesStyleBackTexture;
			Vector2 elementBackgroundScale = new Vector2(1f, 0.45f) * ResolutionRatio;
			Vector2 statManipulatorOrigin = Vector2.UnitX * elementBackgroundTexture.Size() * 0.5f;
			Vector2 statManipulatorPosition = TopLeftLocation - Vector2.UnitX * (elementBackgroundTexture.Width * 0.5f - 120f) * ResolutionRatio;

			// List certain things to change based on what the projectile is, along with a manipulation UI.

			int totalIcons = 0;

			// Local I-frames.
			string iframeText = $" Local I-Frames: {ProjectileBeingEdited.localNPCHitCooldown}";
			if (!ProjectileBeingEdited.usesLocalNPCImmunity)
				iframeText = " This projectile does not use local i-frames.";
			drawCustomManipulatorIcon(iframeText, () =>
			{
				int iframes = ProjectileBeingEdited.localNPCHitCooldown;
				if (Main.mouseLeft && ProjectileBeingEdited.usesLocalNPCImmunity)
				{
					if (Main.keyState.PressingShift())
						iframes--;
					else
						iframes++;
					if (iframes >= 125)
						iframes = 125;
					if (iframes <= -1)
						iframes = -1;

					if (ProjectileBeingEdited.localNPCHitCooldown != iframes)
					{
						ResetProjectileStats();
						ProjectileBeingEdited.localNPCHitCooldown = iframes;
					}
				}
			});
			iframeText = $" Static I-Frames: {ProjectileBeingEdited.idStaticNPCHitCooldown}";
			if (!ProjectileBeingEdited.usesIDStaticNPCImmunity)
				iframeText = " This projectile does not use static i-frames.";
			drawCustomManipulatorIcon(iframeText, () =>
			{
				int iframes = ProjectileBeingEdited.idStaticNPCHitCooldown;
				if (Main.mouseLeft && ProjectileBeingEdited.usesIDStaticNPCImmunity)
				{
					if (Main.keyState.PressingShift())
						iframes--;
					else
						iframes++;
					if (iframes >= 125)
						iframes = 125;
					if (iframes <= -1)
						iframes = -1;

					if (ProjectileBeingEdited.idStaticNPCHitCooldown != iframes)
					{
						ResetProjectileStats();
						ProjectileBeingEdited.idStaticNPCHitCooldown = iframes;
					}
				}
			});

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

		public void ResetProjectileStats()
		{
			int projectileType = ProjectileBeingEdited.type;
			ProjectileOverrideCache.LocalIFrameOverrides[projectileType] = ProjectileBeingEdited.localNPCHitCooldown;
			ProjectileOverrideCache.StaticIFrameOverrides[projectileType] = ProjectileBeingEdited.idStaticNPCHitCooldown;
			CalTestHelpers.HaveAnyStatManipulationsBeenDone = true;
		}

		public void EditText(ref string text)
		{
			if (!EditingProjectileText)
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
					EditingProjectileText = false;
				}
				if (Main.inputTextEscape)
					EditingProjectileText = false;
			}
		}
	}
}