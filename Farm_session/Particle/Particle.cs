using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class Particle
	{
		public Vector2 Position;
		public Vector2 Velocity;
		public float Angle;
		public float AngleVelocity;

		public float Scale;
		public float ScaleVelocity;

		public float TimeValue;
		public float timer;
		public bool Dead;

		public Color color;
		public Texture2D Texture;
		private Vector2 origin;

		public bool ColorGradient;
		public double colorVelocity;
		private float fadeDuration;
		public bool AngleVelocityActivated;

		public Vector2 BeginPosition;
		public int AleaNumber;

		public Particle(Vector2 pPosition, Vector2 pVelocity, float pAngle, float pAngleVelocity,
						float pScale, float pTimeValue, Color pColor, Texture2D pTexture, bool pColorGradient,
					   bool pAngleVelocityActivated, float pFadeDuration = 1)
		{
			Position = pPosition;
			BeginPosition = Position;
			Velocity = pVelocity;
			Angle = pAngle;
			AngleVelocity = pAngleVelocity;
			Scale = pScale;
			TimeValue = pTimeValue;
			color = pColor;
			Texture = pTexture;
			colorVelocity = 1;
			AngleVelocityActivated = pAngleVelocityActivated;
			fadeDuration = pFadeDuration;

			ColorGradient = pColorGradient;
			origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

			Dead = false;
		}

		public void Update(GameTime gameTime)
		{
			if (AngleVelocityActivated) {
				Position += new Vector2(Velocity.X * (float)Math.Cos(Angle),
									   Velocity.Y * (float)Math.Sin(Angle));
			} else {
				Position += Velocity;
			}
			Angle += AngleVelocity;

			if (ColorGradient) {
				color = new Color((int)(color.R*colorVelocity), (int)(color.G*colorVelocity), (int)(color.B*colorVelocity));
				if (TimeValue - timer <= fadeDuration) {
					colorVelocity -= 0.001;
				}
			}

			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer >= TimeValue) {
				Dead = true;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, color, Angle, origin,
							 Scale, SpriteEffects.None, 0);
		}
	}
}
