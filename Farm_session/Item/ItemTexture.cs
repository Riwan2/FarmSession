using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public static class ItemTexture
	{
		public static Dictionary<string, Texture2D> Textures;

		public static void Populate(MainGame mainGame)
		{
			Textures = new Dictionary<string, Texture2D>();

			Textures.Add("WHEAT", mainGame.Content.Load<Texture2D>("GUI/wheatItem"));
			Textures.Add("WHEATSEED", mainGame.Content.Load<Texture2D>("GUI/wheatSeed"));
			Textures.Add("SCYTHE", mainGame.Content.Load<Texture2D>("GUI/scythe"));
			Textures.Add("BASEBATON", mainGame.Content.Load<Texture2D>("GUI/MageBatonGUI"));
		}
	}
}
