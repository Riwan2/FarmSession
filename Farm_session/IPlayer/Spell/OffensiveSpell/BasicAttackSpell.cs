using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class BasicAttackSpell : OffensiveSpell
	{
		public Vector2 TargetPosition;
		public Vector2 CasterPosition;

		private BasicAttackParticle basicAttackParticle;
		public int Damage { get; private set; } = 25;
		
		public BasicAttackSpell(OffensiveSpell pCopy, Vector2 pCasterPosition,
							  Vector2 pTargetPosition, int pDamage) : base(pCopy)
		{
			TargetPosition = pTargetPosition;
			CasterPosition = pCasterPosition;
			Damage = pDamage;

			basicAttackParticle = new BasicAttackParticle(Texture, TargetPosition, CasterPosition, 1, duration, Damage);
			basicAttackParticle.Load();

			ProjectileBloomEffect = false;
		}

		public override void Update(GameTime gameTime)
		{
			basicAttackParticle.Update(gameTime);

			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer >= duration) {
				Dead = true;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			basicAttackParticle.Draw(spriteBatch);
		}
	}

	public class BasicAttackParticle : ParticleSystem
	{
		private Vector2 targetPosition;
		private Vector2 casterPosition;
		private int particleNumber;
		private float duration;

		public bool Explode = false;
		private ExplodeParticle explodeParticle;

		private int Damage;

		public BasicAttackParticle(Texture2D pTexture, Vector2 pTargetPosition, Vector2 pCasterPosition,
								int pParticleNumber, float pDuration, int pDamage) : base(pTexture)
		{
			targetPosition = pTargetPosition;
			casterPosition = pCasterPosition;
			particleNumber = pParticleNumber;
			duration = pDuration;

			explodeParticle = new ExplodeParticle(Texture, Vector2.Zero, 0.4f);
			Damage = pDamage;
		}

		private Particle GenerateParticle()
		{
			Color color = new Color(100 + util.getInt(-25, 25), util.getInt(150, 200), 255);
			Particle a = new Particle(casterPosition, new Vector2(0, 0), 0,
									  0, 0.5f, duration, color, Texture, true, true);
			a.AleaNumber = util.getInt(-10, 10);
			a.Scale = (float)(util.getInt(500, 550)) / 1000;
			a.Scale *= 2.7f;
			a.Velocity = new Vector2(5, 5);
			a.Angle = util.getAngle(targetPosition, casterPosition);
			return a;
		}

		public void Load()
		{
			for (int i = 0; i < particleNumber; i++) {
				lstParticles.Add(GenerateParticle());
			}
		}

		private void Die(Particle particle)
		{
			particle.Dead = true;
			if (!Explode) {
				Explode = true;
			}
		}

		private void ParticleUpdate(GameTime gameTime, Particle particle)
		{
			particle.timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 newTargetPosition = new Vector2(targetPosition.X + particle.AleaNumber, particle.BeginPosition.Y
													+ (targetPosition.Y - particle.BeginPosition.Y));
			Vector2 newPos2 = new Vector2(particle.BeginPosition.X + particle.AleaNumber +
										  (targetPosition.X - particle.BeginPosition.X) * 2 / 3,
										  particle.BeginPosition.Y - 400);

			particle.Position = new Vector2(particle.Position.X + (float)Math.Cos(particle.Angle) * particle.Velocity.X,
											particle.Position.Y + (float)Math.Sin(particle.Angle) * particle.Velocity.Y);
			particle.Velocity *= 1.01f;

			for (int a = 0; a < SceneGameplay.enemieManager.lstEnemies.Count; a++) {
				Enemie enemie = SceneGameplay.enemieManager.lstEnemies[a];
				if (enemie.BoundingBox.Contains(particle.Position)) {
					Die(particle);
					enemie.GetDamage(Damage);
					if (enemie.CurrentState == Enemie.eState.idle) {
						enemie.Aggro();
					}
				}
			}

			for (int i = 0; i < TileMap.LstCollision.Count; i++) {
				if (TileMap.LstCollision[i].Contains(particle.Position) || particle.Position.X < 0 ||
				    particle.Position.Y < 0 || particle.Position.X >= TileMap.MapWidth * TileMap.TileWidth ||
				    particle.Position.Y >= TileMap.MapHeight * TileMap.TileHeight) {
					Die(particle);
				}
			}
		}

		public override void Update(GameTime gameTime)
		{

			for (int i = 0; i < lstParticles.Count; i++) {
				if (Explode) {
					explodeParticle.Position = lstParticles[i].Position;
					explodeParticle.Load();
				}

				if (lstParticles[i].Dead) {
					lstParticles.RemoveAt(i);
				} else {
					Particle particle = lstParticles[i];
					ParticleUpdate(gameTime, particle);
				}
			}

			explodeParticle.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			explodeParticle.Draw(spriteBatch);
		}
	}

	public class ExplodeParticle : ParticleSystem
	{
		public Vector2 Position;
		private float time;

		public ExplodeParticle(Texture2D pTexture, Vector2 pPosition, float pTime) : base(pTexture)
		{
			time = pTime;
			Position = pPosition;
		}

		private Particle GenerateParticle()
		{
			Color color = new Color(util.getInt(50, 100), 150 + util.getInt(-25, 25), 255);
			Particle a = new Particle(Position, new Vector2(0, 0), 0,
									  0, 0.5f, time, color, Texture, false, false);
			a.AleaNumber = util.getInt(-10, 10);
			a.Scale = (float)(util.getInt(500, 550)) / 1000;
			a.Scale /= 1.5f;
			a.Velocity = new Vector2(1.2f, 0);
			return a;
		}

		public void Load()
		{
			for (int i = 0; i < 5; i++) {
				Particle a = GenerateParticle();
				int speed = 150;
				a.Velocity = new Vector2((util.getInt(-speed, speed)) / 100f, (util.getInt(-speed, speed)) / 100f);
				a.TimeValue = time;
				lstParticles.Add(a);
			}
		}
	}
}
