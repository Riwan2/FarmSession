using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class EnemieManager
	{
		public List<Enemie> lstEnemies;

		public EnemieManager()
		{
			lstEnemies = new List<Enemie>();
		}

		public void AddEnemie(Enemie pEnemie)
		{
			lstEnemies.Add(pEnemie);
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < lstEnemies.Count; i++) {
				Champiglu a = (Champiglu)lstEnemies[i];

				if (lstEnemies[i].Delete) {
					lstEnemies.RemoveAt(i);
				} else {
					lstEnemies[i].Update(gameTime);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < lstEnemies.Count; i++) {
				lstEnemies[i].Draw(spriteBatch);
			}
		}
	}
}
