using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public static class AssetManager
	{
		//SpriteFont
		public static SpriteFont MainFont { get; private set; }

		//Item
		public static Dictionary<string, Texture2D> ItemTexture { get; private set; }

		//Shader
		public static Effect ProjectileBloomEffect;

		//Basic Point
		public static Texture2D PointTexture;

		public static void Load(MainGame mainGame)
		{
			ContentManager pContent = mainGame.Content;
			MainFont = pContent.Load<SpriteFont>("mainFont");

			ItemTexture = new Dictionary<string, Texture2D>()
			{
				{"wheat", pContent.Load<Texture2D>("wheatItem") },
				{"wheatSeed", pContent.Load<Texture2D>("wheatSeed")},
				{"scythe", pContent.Load<Texture2D>("scythe")}
			};

			ProjectileBloomEffect = pContent.Load<Effect>("Shader/ProjectileBloom");

			PointTexture = new Texture2D(mainGame.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			PointTexture.SetData(new[] { Color.White });
		}

		public static Texture2D getItemTexture(string item)
		{
			return ItemTexture[item];
		}
	}
}
