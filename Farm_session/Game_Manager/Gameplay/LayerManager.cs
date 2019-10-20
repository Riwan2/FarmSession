using System;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class LayerManager
	{
		private SceneGameplay currentScene;

		public LayerManager(SceneGameplay pScene)
		{
			currentScene = pScene;
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < SceneGameplay.enemieManager.lstEnemies.Count; i++) {
				Enemie enemie = SceneGameplay.enemieManager.lstEnemies[i];
				if (enemie.Position.Y - enemie.BoundingBox.Height/2 > currentScene.myCharacter.Position.Y -
					currentScene.myCharacter.BoundingBox.Height / 2 * 0.7f) {
					enemie.SetLayer(0f);
				} else {
					enemie.SetLayer(1f);
				}
			}
		}
	}
}
