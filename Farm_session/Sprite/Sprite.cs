using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class Sprite : IActor
	{
		//Iactor
		public Vector2 Position { get; set; }
		public Rectangle BoundingBox { get; private set;}
		public bool IsInvisible { get; set; }
		public bool Delete { get; set; }

		//Sprite
		public Texture2D Texture { get; }
		public int Scale { get; set; } = 1;

		public Sprite(Texture2D pTexture)
		{
			Texture = pTexture;
			Load();
		}

		public Sprite(Texture2D pTexture, int pScale)
		{
			Texture = pTexture;
			Scale = pScale;
			Load();
		}

		private void Load()
		{
			IsInvisible = false;
		}

		public void Move(float pX, float pY)
		{
			Position = new Vector2(Position.X + pX, Position.Y + pY);
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!IsInvisible) {
				BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width * Scale, Texture.Height * Scale);
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (!IsInvisible) {
				spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero,
				                 Scale, SpriteEffects.None, 0);
			}
		}
	}
}
