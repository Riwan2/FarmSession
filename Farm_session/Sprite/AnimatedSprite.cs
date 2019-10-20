using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class AnimatedSprite : IActor
	{
		//IActor
		public Vector2 Position { get; set; }
		public Rectangle BoundingBox { get; protected set; }
		public bool IsInvisible { get; set; } = false;
		public bool Delete { get; set; }

		//Animated Sprite
		//Image
		public Color color;
		public int width;
		public int height;
		public Texture2D TileSheet { get; protected set; }
		protected int totalWidth;
		protected int totalHeight;
		protected int widthTileNumber;
		protected int heightTileNumber;
		public List<Rectangle> lstRectangles { get; protected set;}
		//Time 
		public float timer;
		public float frameTime { get; protected set; }
		public int count;
		public int countLimit { get; protected set; }

		public Vector2 scale;
		public float Layer = 0;

		public Vector2 Origin = Vector2.Zero;

		public AnimatedSprite(Texture2D pTileSheet, int pTotalWidth, int pTotalHeight, 
		                      int pWidth, int pHeight, float pFrameTime, int pScale)
		{
			TileSheet = pTileSheet;
			totalWidth = pTotalWidth;
			totalHeight = pTotalHeight;
			width = pWidth;
			height = pHeight;
			frameTime = pFrameTime;
			scale = new Vector2(pScale, pScale);

			Load();
		}

		public void SetBoundingBox()
		{
			BoundingBox = new Rectangle((int)(Position.X - Origin.X*scale.X), 
			                            (int)(Position.Y - Origin.Y*scale.Y), width*(int)scale.X, height*(int)scale.Y);
		}

		private void Load()
		{
			IsInvisible = false;

			widthTileNumber = totalWidth / width;
			heightTileNumber = totalHeight / height;
			lstRectangles = new List<Rectangle>();

			int x;
			int y;
			int z = 0;

			for (y = 0; y < heightTileNumber; y++) {
				for (x = 0; x < widthTileNumber; x++) {
					lstRectangles.Add(new Rectangle(x * width, y * height, width, height));
					z++;
				}
				x = 0;
			}

			timer = 0f;
			count = 0;
			countLimit = widthTileNumber * heightTileNumber;
			color = Color.White;
		}

		public void Move(float pX, float pY)
		{
			Position = new Vector2(Position.X + pX, Position.Y + pY);
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!IsInvisible) {
				timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (timer >= frameTime) {
					timer = 0;
					count++;
					if (count >= countLimit) {
						count = 0;
					}
				}
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (!IsInvisible) {
				spriteBatch.Draw(TileSheet, Position, lstRectangles[count], color,
								 0, Origin, scale, SpriteEffects.None, Layer);
			}
		}
	}
}
