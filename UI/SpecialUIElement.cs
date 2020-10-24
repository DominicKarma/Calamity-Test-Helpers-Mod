using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalTestHelpers.UI
{
	public struct SpecialUIElement
	{
		public string Description;
		public Texture2D IconTexture;
		public Action OnClick;
		public SpecialUIElement(string description, Texture2D icon, Action onClickEffect = null)
		{
			Description = description;
			IconTexture = icon;
			OnClick = onClickEffect;
		}
		public void DrawDescription(SpriteBatch spriteBatch, Vector2 drawCoordinates, Color textColor, float maxScale)
		{
			float scale = (new Vector2(300f, 44f) / Main.fontMouseText.MeasureString(Description)).Length();
			if (scale > maxScale)
				scale = maxScale;
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, Description, drawCoordinates.X, drawCoordinates.Y, textColor, Color.Black, Vector2.Zero, scale);
		}
	}
}