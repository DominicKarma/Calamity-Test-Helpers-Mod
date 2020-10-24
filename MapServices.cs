using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;

namespace CalTestHelpers
{
	public class MapServices
	{
		public static void RevealTheEntireMap()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (WorldGen.InWorld(i, j))
						Main.Map.Update(i, j, 255);
				}
			}
			Main.refreshMap = true;
		}

		public static void TryToTeleportPlayerOnMap()
		{
			if (Main.mouseRight && Main.keyState.IsKeyUp(Keys.LeftControl))
			{
				int mapWidth = Main.maxTilesX * 16;
				int mapHeight = Main.maxTilesY * 16;
				Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

				cursorPosition.X -= Main.screenWidth / 2;
				cursorPosition.Y -= Main.screenHeight / 2;

				Vector2 mapPosition = Main.mapFullscreenPos;
				Vector2 cursorWorldPosition = mapPosition;

				cursorPosition /= 16;
				cursorPosition *= 16 / Main.mapFullscreenScale;
				cursorWorldPosition += cursorPosition;
				cursorWorldPosition *= 16;

				cursorWorldPosition.Y -= Main.LocalPlayer.height;
				if (cursorWorldPosition.X < 0)
					cursorWorldPosition.X = 0;
				else if (cursorWorldPosition.X + Main.LocalPlayer.width > mapWidth)
					cursorWorldPosition.X = mapWidth - Main.LocalPlayer.width;

				if (cursorWorldPosition.Y < 0)
					cursorWorldPosition.Y = 0;
				else if (cursorWorldPosition.Y + Main.LocalPlayer.height > mapHeight)
					cursorWorldPosition.Y = mapHeight - Main.LocalPlayer.height;

				if (Main.LocalPlayer.position != cursorWorldPosition)
				{
					Main.LocalPlayer.Teleport(cursorWorldPosition, 1, 0);
					Main.LocalPlayer.position = cursorWorldPosition;
					Main.LocalPlayer.velocity = Vector2.Zero;
					Main.LocalPlayer.fallStart = (int)(Main.LocalPlayer.position.Y / 16f);
					NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.myPlayer, cursorWorldPosition.X, cursorWorldPosition.Y, 1, 0, 0);
				}
			}
		}
	}
}