using System;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class PlayerLevel : Map
	{
		public PlayerLevel(MainGame pMainGame, SceneGameplay pCurrentScene, Map pLastMap) 
			: base(pMainGame, pCurrentScene, pLastMap)
		{
			
		}

		public override void Load()
		{
			FileName = "PlayerLevel";
			HeroPosition = new Vector2(MainGame.ScreenWidth / 3.19f,
									   MainGame.ScreenHeight / 4);
			HeroPosition2 = new Vector2(MainGame.ScreenWidth * 0.955f,
										MainGame.ScreenHeight * 1.05f);
			IsSave = true;
			IsShader = true;
			CameraFix = false;
			NextMap = "route1";

			base.Load();
		}

		public override void Update(GameTime gameTime)
		{

			//Crop
			for (int i = 0; i < currentScene.CropList.Count; i++) {
				if (currentScene.CropList[i].Delete) {
					currentScene.CropList.RemoveAt(i);
				} else {
					currentScene.CropList[i].Update(gameTime);
				}
			}

			base.Update(gameTime);
		}
	}
}
