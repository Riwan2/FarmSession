using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class EnemieOutput
	{
		Vector2 Position;
		string Damage;
		bool DrawDamage = false;
		Color color;
		Color CurrentColor;

		private float timer = 0;
		private float time = 0.5f;

		public EnemieOutput()
		{
			Damage = "";
			color = Color.Red;
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < SceneGameplay.enemieManager.lstEnemies.Count; i++) {
				Enemie enemie = SceneGameplay.enemieManager.lstEnemies[i];
				if (enemie.GotHurt) {
					Damage = enemie.LastDamageTaken;
					Position = new Vector2(enemie.Position.X,
										   enemie.Position.Y - enemie.BoundingBox.Height / 1.5f);
					enemie.GotHurt = false;
					CurrentColor = color;
					DrawDamage = true;
				}
			}

			if (DrawDamage) {
				timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				CurrentColor = new Color(CurrentColor.R, CurrentColor.G, CurrentColor.B, CurrentColor.A * 0.9f);
				Position = new Vector2(Position.X, Position.Y - 0.8f);
				if (timer >= time) {
					timer = 0;
					DrawDamage = false;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (DrawDamage) {
				spriteBatch.DrawString(AssetManager.MainFont, Damage, Position, CurrentColor, 0, Vector2.Zero,
									  0.75f, SpriteEffects.None, 0);
			}
		}
	}
}
