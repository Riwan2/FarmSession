using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class Primitive
	{
		private SpriteBatch spriteBatch;

		public Primitive(SpriteBatch pSpriteBatch)
		{
			spriteBatch = pSpriteBatch;
		}

		public Primitive()
		{

		}

		public void DrawLine(int pStartX, int pStartY, int pEndX, int pEndY, Color pColor)
		{
			spriteBatch.Begin();

			int xDif = pStartX - pEndX;
			int yDif = pStartY - pEndY;
			int length = (int)Math.Sqrt(xDif * xDif + yDif * yDif);
			float angle = util.getAngle(new Vector2(pEndX, pEndY), new Vector2(pStartX, pStartY));

			spriteBatch.Draw(AssetManager.PointTexture,
			                 new Rectangle(pStartX, pStartY, length, 1),
							 null, pColor, angle, Vector2.Zero, SpriteEffects.None, 0);

			spriteBatch.End();
		}

		public void DrawLine(int pStartX, int pStartY, int pEndX, int pEndY, Color pColor, SpriteBatch pSpriteBatch)
		{
			int xDif = pStartX - pEndX;
			int yDif = pStartY - pEndY;
			int length = (int)Math.Sqrt(xDif * xDif + yDif * yDif);
			float angle = util.getAngle(new Vector2(pEndX, pEndY), new Vector2(pStartX, pStartY));

			pSpriteBatch.Draw(AssetManager.PointTexture,
							 new Rectangle(pStartX, pStartY, length, 1),
							 null, pColor, angle, Vector2.Zero, SpriteEffects.None, 0);
		}

		public void DrawRectangle(int pStartX, int pStartY, int pWidth, int pHeight, Color pColor)
		{
			spriteBatch.Begin();

			spriteBatch.Draw(AssetManager.PointTexture, new Rectangle(pStartX, pStartY, pWidth, pHeight), pColor);

			spriteBatch.End();
		}
	}
}
