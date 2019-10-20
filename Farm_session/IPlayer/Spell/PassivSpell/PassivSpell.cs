using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class PassivSpell : Spell
	{

		public PassivSpell(string pDisplayName, float pDuration, Texture2D pTexture)
		{
			DisplayName = pDisplayName;
			duration = pDuration;
			Texture = pTexture;
		}

		public PassivSpell(PassivSpell pCopy) : base(pCopy)
		{
			
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}
	}

	public static class PassivSpellData
	{
		public static Dictionary<string, PassivSpell> Data;

		public static void PopulateData(ContentManager content)
		{
			Data = new Dictionary<string, PassivSpell>();

			Data.Add("WATER_CROP", new PassivSpell("Water Crops", 4, 
			                                        content.Load<Texture2D>("Particle/SquareParticle")));
		}
	}
}
