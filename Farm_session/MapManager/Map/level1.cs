using System;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class level1 : Map
	{
		public level1(MainGame pMainGame, SceneGameplay pCurrentScene, Map pLastMap)
			: base(pMainGame, pCurrentScene, pLastMap)
		{

		}

		public override void Load()
		{
			FileName = "PlayerLevel";
			IsSave = true;
			IsShader = true;
			CameraFix = false;
			NextMap = "route1";

			base.Load();

			currentScene.myCharacter.Position = new Vector2(MainGame.ScreenWidth / 3.15f, MainGame.ScreenHeight / 2);
			currentScene.timeManager.SetHour(13);
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
