using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public abstract class ParticleSystem
	{
		public Texture2D Texture;
		public List<Particle> lstParticles;

		public ParticleSystem(Texture2D pTexture)
		{
			Texture = pTexture;
			lstParticles = new List<Particle>();
		}

		public virtual void Update(GameTime gameTime)
		{
			for (int i = 0; i < lstParticles.Count; i++) {
				if (lstParticles[i].Dead) {
					lstParticles.RemoveAt(i);
				} else {
					lstParticles[i].Update(gameTime);
				}
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < lstParticles.Count; i++) {
				lstParticles[i].Draw(spriteBatch);
			}
		}
	}
}
