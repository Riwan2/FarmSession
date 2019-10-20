using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class Selector : Sprite
	{
		//Selector

		public string CurrentTileType;
		public int TileId;
		private Point mouse;
		//Position
		public int GridX { get; private set; }
		public int GridY { get; private set; }

		public Selector(Texture2D pTexture) : base(pTexture)
		{
			Load();
		}

		private void Load()
		{
			IsInvisible = true;
			TileId = 0;
			Position = new Vector2(0, 0);
		}

		public override void Update(GameTime gameTime)
		{
			mouse = Mouse.GetState().Position;
			//What tile is it ?
			TileId = TileMap.GetTileAt(mouse.X, mouse.Y, 0);
			CurrentTileType = util.getTileType(TileId);
			//Console.WriteLine(TileId + " : " + CurrentTileType);

			//Selector placement
			if (mouse.X > 0 && mouse.X < MainGame.ScreenWidth) {
				GridX = Math.Abs(mouse.X / TileMap.TileWidth);
			}
			if (mouse.Y > 0 && mouse.Y < MainGame.ScreenHeight) {
				GridY = Math.Abs(mouse.Y / TileMap.TileHeight);
			}

			Position = new Vector2(GridX * TileMap.TileWidth, GridY * TileMap.TileHeight);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}
	}
}
