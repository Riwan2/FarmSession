using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	abstract public class Scene
	{
		protected MainGame mainGame;

		protected List<IActor> lstActors;

		public Scene(MainGame pMainGame)
		{
			mainGame = pMainGame;
			lstActors = new List<IActor>();
		}

		public virtual void Load()
		{

		}

		public virtual void Unload()
		{
			
		}

		public virtual void Update(GameTime gameTime)
		{
			for (int i = 0; i < lstActors.Count; i++) {
				if (lstActors[i].Delete) {
					lstActors.RemoveAt(i);
				} else {
					lstActors[i].Update(gameTime);
				}
			}
		}

		public virtual void Draw(GameTime gameTime)
		{
			for (int i = 0; i < lstActors.Count; i++) {
				lstActors[i].Draw(mainGame.spriteBatch);
			}
		}

	}
}
