using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class SpellManager
	{
		public List<Spell> lstSpell;

		public SpellManager()
		{
			lstSpell = new List<Spell>();
		}

		public void AddPassivSpell(PassivSpell spell)
		{
			lstSpell.Add(spell);
		}

		public void AddOffensiveSpell(OffensiveSpell spell)
		{
			lstSpell.Add(spell);
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < lstSpell.Count; i++) {
				if (lstSpell[i].Dead) {
					lstSpell.RemoveAt(i);
				} else {
					lstSpell[i].Update(gameTime);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			
			
			AssetManager.ProjectileBloomEffect.CurrentTechnique.Passes[0].Apply();
			for (int i = 0; i < lstSpell.Count; i++) {
				if (lstSpell[i].ProjectileBloomEffect) {
					spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null,
					  null, null, null);
					AssetManager.ProjectileBloomEffect.CurrentTechnique.Passes[0].Apply();
					lstSpell[i].Draw(spriteBatch);
					spriteBatch.End();
				} else {
					spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null,
					  null, null, null);
					lstSpell[i].Draw(spriteBatch);
					spriteBatch.End();
				}
			}
		}
	}
}
