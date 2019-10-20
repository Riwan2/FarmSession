using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public enum ePhase
	{
		began,
		move, 
		ended,
		cancelled
	}

	public class DragEvent
	{
		public float X;
		public float Y;
		public float startX;
		public float startY;
		public ePhase phase;
	}

	public class InventoryIcon
	{
		public Vector2 Position;
		public Texture2D Texture;
		public int Quantity;
		public bool isDraggable;
		public bool isDragging;
		public Vector2 StartPosition;

		public InventoryIcon(Texture2D pTexture, Vector2 pPosition, int pQuantity, bool pDraggable)
		{
			Position = pPosition;
			Texture = pTexture;
			Quantity = pQuantity;
			isDraggable = pDraggable;
		}

		public Rectangle HandleRect
		{
			get 
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
			}
		}

		public Vector2 GetCenter()
		{
			return new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
		}

		public void Touch(DragEvent pDragEvent)
		{
			if (!isDraggable)
				return;

			if (pDragEvent.phase == ePhase.began) {
				isDragging = true;
				StartPosition = Position;
			} 
			else if (pDragEvent.phase == ePhase.move) 
			{
				float x = pDragEvent.X - pDragEvent.startX + StartPosition.X;
				float y = pDragEvent.Y - pDragEvent.startY + StartPosition.Y;
				Position = new Vector2(x, y);
			} 
			else if (pDragEvent.phase == ePhase.ended || pDragEvent.phase == ePhase.cancelled) 
			{
				isDragging = false;

				if (pDragEvent.phase == ePhase.cancelled) {
					Position = StartPosition;
				}
			} 
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Texture != null) {

				Color c;
				if (isDragging)
					c = Color.White * 0.5f;
				else
					c = Color.White;
				spriteBatch.Draw(Texture, Position, c);

				if (Quantity > 1) {
					string quantity = Quantity.ToString();
					Vector2 Size = AssetManager.MainFont.MeasureString(quantity);
					Size = new Vector2(Size.X * 0.6f, Size.Y * 0.6f);
					spriteBatch.DrawString(AssetManager.MainFont, quantity,
										   new Vector2(Position.X + Texture.Width - Size.X,
													   Position.Y + Texture.Height - Size.Y),
										   Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0); 
				}
			}
		}
	}
}
