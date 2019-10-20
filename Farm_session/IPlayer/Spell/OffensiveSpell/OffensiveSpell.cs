using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class OffensiveSpell : Spell
	{

		public OffensiveSpell()
		{
		}

		public OffensiveSpell(OffensiveSpell pCopy) : base(pCopy)
		{
			
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}
	}

	public static class OffensiveSpellData
	{
		public static Dictionary<string, OffensiveSpell> Data;

		public static void PopulateData(ContentManager content)
		{
			Data = new Dictionary<string, OffensiveSpell>();

			Data.Add("BASIC_ATTACK", new OffensiveSpell() {
				DisplayName = "Basic Attack", duration = 4, 
				Texture = content.Load<Texture2D>("Particle/ProjectileParticle"),
			});
		}
	}

}
