using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public abstract class Spell
	{
		public string DisplayName;
		public float duration;
		public Texture2D Texture;
		public bool Dead { get; protected set; } = false;

		public bool ProjectileBloomEffect = false;

		protected float timer;

		public Spell()
		{
		}

		public Spell(Spell pCopy)
		{
			DisplayName = pCopy.DisplayName;
			duration = pCopy.duration;
			Texture = pCopy.Texture;
		}

		public abstract void Update(GameTime gameTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
