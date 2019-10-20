using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class HeroTileSelect
	{
		public int Index;
		public int Gid;
		public Color color;

		private int scale;
		private bool drawing;
		public Vector2 Position { get; private set; }
		Texture2D texture;

		//Direction
		float angle;
		public bool WithMouse;
		private SceneGameplay scene;

		public HeroTileSelect(Texture2D pTexture, int pScale, SceneGameplay pSceneGameplay)
		{
			Index = 0;
			Gid = 0;
			drawing = false;
			texture = pTexture;
			Position = Vector2.Zero;
			scale = pScale;
			WithMouse = true;
			scene = pSceneGameplay;

			color = new Color(255, 255, 255, 0.9f);
		}

		private Vector2 right(Hero hero)
		{
			return new Vector2(hero.tilePosX * TileMap.TileWidth, (hero.tilePosY - 1) * TileMap.TileHeight);
		}

		private Vector2 left(Hero hero)
		{
			return new Vector2((hero.tilePosX - 2) * TileMap.TileWidth, (hero.tilePosY - 1) * TileMap.TileHeight);
		}

		private Vector2 up(Hero hero)
		{
			return new Vector2((hero.tilePosX - 1) * TileMap.TileWidth, (hero.tilePosY - 2) * TileMap.TileHeight);
		}

		private Vector2 down(Hero hero)
		{
			return new Vector2((hero.tilePosX - 1) * TileMap.TileWidth, hero.tilePosY * TileMap.TileHeight);
		}

		public void Update(Hero hero, KeyboardState newKeyboardState, KeyboardState oldKeyboardState, Point mousePos)
		{
			if (hero.currentDirection == Hero.Direction.Right) {      //Selection facing the player
				Index = hero.tileIndex + 1;
				Position = right(hero);

			} else if (hero.currentDirection == Hero.Direction.Down) {
				Index = hero.tileIndex + TileMap.MapWidth;
				Position = down(hero);

			} else if (hero.currentDirection == Hero.Direction.Left) {
				Index = hero.tileIndex - 1;
				Position = left(hero);

			} else if (hero.currentDirection == Hero.Direction.Up) {
				Index = hero.tileIndex - TileMap.MapWidth;
				Position = up(hero);
			}

			if (WithMouse) {
				//Direction Set
				Vector2 PosCharacter = new Vector2(MainGame.ScreenWidth + SceneGameplay.camera.Position.X + hero.Position.X,
				                                   MainGame.ScreenHeight + SceneGameplay.camera.Position.Y + hero.Position.Y); 
				angle = -util.getAngle(new Vector2(mousePos.X, mousePos.Y), PosCharacter);

				if (angle > Math.PI / 3 && angle < (2 * Math.PI) / 3) {
					Index = hero.tileIndex - TileMap.MapWidth;
					Position = up(hero);
				} else if (angle < -Math.PI / 3 && angle > (-2 * Math.PI) / 3) {
					Index = hero.tileIndex + TileMap.MapWidth;
					Position = down(hero);
				} else if ((angle > (2 * Math.PI) / 3 && angle <= Math.PI) || (angle < (-2 * Math.PI) / 3 && angle >= -Math.PI)) {
					Index = hero.tileIndex - 1;
					Position = left(hero);
				} else if ((angle < Math.PI / 3 && angle >= 0) || (angle > -Math.PI / 3 && angle <= 0)) {
					Index = hero.tileIndex + 1;
					Position = right(hero);
				}
			}

			if (Index >= 0 && Index < TileMap.lstLayer[1].Count) {
				Gid = TileMap.lstLayer[1][Index].Gid;
			}

			if (newKeyboardState.IsKeyDown(Keys.LeftAlt) && oldKeyboardState.IsKeyUp(Keys.LeftAlt)) {
				drawing = !drawing;
			} 
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (drawing) {
				spriteBatch.Draw(texture, Position, null, color, 0, Vector2.Zero,
								 scale, SpriteEffects.None, 0);
			}
		}
	}
}
