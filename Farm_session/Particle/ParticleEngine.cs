using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class ParticleEngine
	{
		private MainGame mainGame;
		public Vector2 Position;

		public List<Particle> lstParticles;
		private Texture2D Texture;

		public bool PlayerFootPrint;
		private float footPrintTime = 0.5f;
		private float footPrintTimer;
		private Hero player;

		public ParticleEngine(MainGame pMainGame, Vector2 pPosition, Hero pPlayer)
		{
			mainGame = pMainGame;
			Position = pPosition;
			player = pPlayer;

			lstParticles = new List<Particle>();
			Texture = mainGame.Content.Load<Texture2D>("Particle/RoundParticle");

			PlayerFootPrint = false;
		}

		private Particle GenerateParticle()
		{
			float speedY = 1;
			float speedX = 3;
			Vector2 Velocity = new Vector2(speedX + (float)(util.getDouble() * 0.9f), 
			                               speedY + (float)(util.getDouble() * 0.9f));
			float AngleVelocity = 0.005f;
			float Scale = (float)util.getDouble();
			Color color = new Color((float)util.getDouble(),
									(float)util.getDouble(),
									(float)util.getDouble());
			return new Particle(Position, Velocity, 0, AngleVelocity, Scale, 11, color, Texture, true, true);
		}

		public void Load()
		{
			for (int i = 0; i < 100; i++) {
				lstParticles.Add(GenerateParticle());
			}
		}

		public void FootPrintLoad()
		{
			for (int i = 0; i < 4; i++) {
				Vector2 velocity = Vector2.Zero;
				Vector2 pos = player.Position;
				if (player.currentDirection == Hero.Direction.Down) {
					velocity = new Vector2(0, -(float)util.getDouble());
					pos = new Vector2(pos.X-10 + util.getInt(-50, 50) / 20, pos.Y + 24);
				} else if (player.currentDirection == Hero.Direction.Left) {
					velocity = new Vector2((float)util.getDouble(), 0);
					pos = new Vector2(pos.X - 15, pos.Y + 20 + util.getInt(-50, 50) / 20);
				} else if (player.currentDirection == Hero.Direction.Right) {
					velocity = new Vector2(-(float)util.getDouble(), 0);
					pos = new Vector2(pos.X, pos.Y + 20 + util.getInt(-50, 50) / 20);
				} else if (player.currentDirection == Hero.Direction.Up) {
					velocity = new Vector2(0, (float)util.getDouble());
					pos = new Vector2(pos.X - 10 + util.getInt(-50, 50) / 20, pos.Y - 15);
				}
				velocity = new Vector2(velocity.X * 0.5f, velocity.Y * 0.5f);
				Color color = new Color(150, 150, 150);
				Particle a = new Particle(pos, velocity, 0, 0, 0.5f, 0.75f, color, Texture, true, false);
				lstParticles.Add(a);
			}
		}

		public void Update(GameTime gameTime)
		{
			if (PlayerFootPrint && player.inMovement) { //PLayerFootPrint
				footPrintTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (footPrintTimer >= footPrintTime) {
					footPrintTimer = 0;
					FootPrintLoad();
				}
			}

			for (int i = 0; i < lstParticles.Count; i++) {
				if (lstParticles[i].Dead) {
					lstParticles.RemoveAt(i);
				} else {
					lstParticles[i].Update(gameTime);
				}
			}
		}

		public void Draw(GameTime gameTime)
		{
			for (int i = 0; i < lstParticles.Count; i++) {
				lstParticles[i].Draw(mainGame.spriteBatch);
			}
		}
	}
}
