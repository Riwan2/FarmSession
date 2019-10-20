using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class WaterCropsSpell : PassivSpell
	{
		public Vector2 TargetPosition;
		public Vector2 CasterPosition;

		private WaterCropParticle waterCropParticle;

		public WaterCropsSpell(PassivSpell pCopy, Vector2 pCasterPosition,
		                      Vector2 pTargetPosition) : base(pCopy)
		{
			TargetPosition = pTargetPosition;
			CasterPosition = pCasterPosition;

			waterCropParticle = new WaterCropParticle(Texture, TargetPosition, CasterPosition, 1, duration);
			waterCropParticle.Load();
		}

		public override void Update(GameTime gameTime)
		{
			waterCropParticle.Update(gameTime);

			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer >= duration) {
				Dead = true;
			}

			if (duration - timer <= 1) {
				Vector2 pos = TileMap.GetTilePosAt((int)TargetPosition.X, (int)TargetPosition.Y, 0);
				int posX = (int)pos.X;
				int posY = ((int)pos.Y - 1) * TileMap.MapWidth;
				if (posX + posY < TileMap.lstLayer[0].Count && posX + posY >= 0) {
					TileMap.lstLayer[0][posX + posY].Gid = util.getTileId("farmLandWet");
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			waterCropParticle.Draw(spriteBatch);
		}
	}

	public class WaterCropParticle : ParticleSystem
	{
		private Vector2 targetPosition;
		private Vector2 casterPosition;
		private int particleNumber;
		private float duration;

		private int nbSlave = 0;
		private int maxSlave = 10;
		private float slaveTimer = 0;
		private float slaveTime = 0.05f;

		public WaterCropParticle(Texture2D pTexture, Vector2 pTargetPosition, Vector2 pCasterPosition, 
		                        int pParticleNumber, float pDuration) : base(pTexture)
		{
			targetPosition = pTargetPosition;
			casterPosition = pCasterPosition;
			particleNumber = pParticleNumber;
			duration = pDuration;
		}

		private Particle GenerateParticle()
		{
			Color color = new Color(0 + util.getInt(0, 50), 100 + util.getInt(-25, 25), 255);
			Particle a = new Particle(casterPosition, new Vector2(0, 0), 0,
									  0, 0.5f, duration, color, Texture, false, false);
			a.AleaNumber = util.getInt(-10, 10);
			a.Scale = (float)(util.getInt(500, 550)) / 1000;
			return a;
		}

		public void Load()
		{
			for (int i = 0; i < particleNumber; i++) {
				lstParticles.Add(GenerateParticle());
			}
		}

		private void ParticleUpdate(GameTime gameTime, Particle particle)
		{
			particle.timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 newTargetPosition = new Vector2(targetPosition.X  + particle.AleaNumber, particle.BeginPosition.Y 
			                                        + (targetPosition.Y - particle.BeginPosition.Y));
			Vector2 newPos2 = new Vector2(particle.BeginPosition.X + particle.AleaNumber +
			                              (targetPosition.X - particle.BeginPosition.X) * 2 / 3,
										  particle.BeginPosition.Y - 400);


			particle.Position = GetPoint(particle.timer / 3, particle.BeginPosition,
										 new Vector2(particle.BeginPosition.X +
													 (targetPosition.X - particle.BeginPosition.X) * 1 / 3,
													 particle.BeginPosition.Y - 400), newPos2,
										 newTargetPosition);

			if (targetPosition.X > particle.BeginPosition.X) {
				if (particle.Position.Y >= (particle.BeginPosition.Y + (targetPosition.Y - particle.BeginPosition.Y)) &&
				    particle.Position.X - particle.AleaNumber >= targetPosition.X) {
					particle.Dead = true;
				}
			} else {
				if (particle.Position.Y >= (particle.BeginPosition.Y + (targetPosition.Y - particle.BeginPosition.Y)) &&
				    particle.Position.X - particle.AleaNumber <= targetPosition.X) {
					particle.Dead = true;
				}
			}
		}

		public static Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) //courbe de bezier
		{
			float cx = 3 * (p1.X - p0.X);
			float cy = 3 * (p1.Y - p0.Y);

			float bx = 3 * (p2.X - p1.X) - cx;
			float by = 3 * (p2.Y - p1.Y) - cy;

			float ax = p3.X - p0.X - cx - bx;
			float ay = p3.Y - p0.Y - cy - by;

			float Cube = t * t * t;
			float Square = t * t;

			float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
			float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

			return new Vector2(resX, resY);
		}

		public override void Update(GameTime gameTime)
		{
			
			if (nbSlave <= maxSlave) {
				slaveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (slaveTimer >= slaveTime) {
					Load();
					slaveTimer = 0;
					nbSlave++;
				}
			}

			for (int i = 0; i < lstParticles.Count; i++) {
				if (lstParticles[i].Dead) {
					lstParticles.RemoveAt(i);
				} else {
					Particle particle = lstParticles[i];
					ParticleUpdate(gameTime, particle);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}
	}
}
