using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public delegate void onClick(Button pSender);

	public class Button : GUIElement
	{
		//GUIElement
		public Vector2 Position { get; set; }
		public string ElementString { get; set; }
		public bool IsInvisible { get; set; }

		//Button
		public bool isHover { get; private set; }
		public bool isClicked { get; private set; }
		public AnimatedSprite ButtonSprite { get; private set; }
		//private Vector2 Scale;

		public Rectangle BoundingBox { get; private set; }
		public onClick onClick;

		private float timer;                 //Timer to see the button getting clicked
		private float clickTime = 0.5f;

		//String
		private Color stringColor;
		private Vector2 stringPosition;
		private Vector2 stringScale;

		public Button(AnimatedSprite pButtonSprite, string pElementString, Vector2 pPosition)
		{
			ButtonSprite = pButtonSprite;
			ButtonSprite.Position = pPosition;
			ButtonSprite.SetBoundingBox();

			ElementString = pElementString;
			stringColor = Color.Black;
			stringScale = new Vector2(ButtonSprite.scale.X, ButtonSprite.scale.Y);

			SpriteFont font = AssetManager.MainFont;   //string placement
			Vector2 stringSize = font.MeasureString(pElementString);
			int stringWidth = (int)(stringSize.X * (int)(stringScale.X));
			int stringHeight = (int)(stringSize.Y * (int)(stringScale.Y));

			stringPosition = new Vector2(ButtonSprite.Position.X + (ButtonSprite.width / 2 - stringWidth / 2),
			                             ButtonSprite.Position.Y + (ButtonSprite.height / 2 - stringHeight / 2));
			

			isHover = false;
			isClicked = false;
		}

		public void Update(GameTime gameTime, MouseState oldMouseState, MouseState newMouseState, Point mousePos)
		{
			if (!ButtonSprite.IsInvisible) {
				//isHover
				if (ButtonSprite.BoundingBox.Contains(mousePos)) {
					if (!isHover) {
						isHover = true;
						Console.WriteLine("Mouse on Button");
						ButtonSprite.count = 1;
					}
				} else {
					isHover = false;
					ButtonSprite.count = 0;
				}
				//isClicked
				if (isHover && newMouseState.LeftButton == ButtonState.Pressed &&
					oldMouseState.LeftButton == ButtonState.Released) {
					Console.WriteLine("Button is pressed");
					isClicked = true;
				}

				if (isClicked) {
					timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
					ButtonSprite.count = 2;
					if (onClick != null && timer > clickTime) {
						onClick(this);
						timer = 0;
						isClicked = false;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//Draw button
			ButtonSprite.Draw(spriteBatch);
			//Draw String
			spriteBatch.DrawString(AssetManager.MainFont, ElementString, stringPosition, stringColor, 0,
								  Vector2.Zero, stringScale, SpriteEffects.None, 0);
		}

	}
}
