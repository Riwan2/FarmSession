using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class TileSelector : AnimatedSprite
	{
		//TileSelector
		//draw
		private int space = 5;
		private int tileNumber = 6;
		private int offsetX = 150;
		private int tileWidth = 16;
		//update
		public int currentTileSelected;
		private int currentCaseSelected;
		private int inventoryCase;
		private int currentScrollValue;
		private int previousScrollValue;

		private int tileBegining = 0;
		private int tileEnd = 6;

		public TileSelector(Texture2D pTileSheet, int pTotalWidth, int pTotalHeight, int pWidth, int pHeight, float pFrameTime, int pScale)
			: base(pTileSheet, pTotalWidth, pTotalHeight, pWidth, pHeight, pFrameTime, pScale)
		{
			tileWidth = tileWidth * pScale;
			space = space * pScale;
			Load();
		}

		private void Load()
		{
			Position = new Vector2(offsetX, 0);
			currentCaseSelected = 1;

			previousScrollValue = 0;

		}

		public void SwitchRight()
		{
			if (currentCaseSelected < tileNumber) {
				currentCaseSelected++;
			}
		}

		public void SwitchLeft()
		{
			if (currentCaseSelected > 1) {
				currentCaseSelected--;
			}
		}

		public void SwitchInventoryRight()
		{
			if ((tileEnd + tileNumber) <= 32) {
				Console.WriteLine("SwitchRight");
				tileBegining = tileEnd;
				tileEnd = tileEnd + tileNumber;
			}
		}

		public void SwitchInventoryLeft()
		{
			if ((tileBegining - tileNumber) >= 0) {
				tileEnd = tileBegining;
				tileBegining = tileBegining - tileNumber;
			}
		}

		public override void Update(GameTime gameTime)
		{
			currentScrollValue = Mouse.GetState().ScrollWheelValue;

			if (currentScrollValue < previousScrollValue) {
				SwitchRight();
			}
			if (currentScrollValue > previousScrollValue) {
				SwitchLeft();
			}

			currentTileSelected = currentCaseSelected;
			currentTileSelected += tileBegining;

			previousScrollValue = Mouse.GetState().ScrollWheelValue;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			for (int i = tileBegining; i < tileEnd; i++) {
				spriteBatch.Draw(TileSheet, Position,lstRectangles[i], Color.White,
				                 0, Vector2.Zero, scale, SpriteEffects.None, 0);

				inventoryCase = (i + 1) - tileBegining;
				if (inventoryCase == currentCaseSelected) {
					spriteBatch.DrawString(AssetManager.MainFont, "" + inventoryCase, new Vector2(Position.X, Position.Y + tileWidth),
									   Color.Black);
				} else {
					spriteBatch.DrawString(AssetManager.MainFont, "" + inventoryCase, new Vector2(Position.X, Position.Y + tileWidth),
										   Color.White);
				}

				Position = new Vector2(Position.X + tileWidth + space, Position.Y);
			}
			Position = new Vector2(offsetX, 0);
		}

	}
}
