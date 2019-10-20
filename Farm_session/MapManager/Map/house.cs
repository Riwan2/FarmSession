using System;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class house : Map
	{
		public house(MainGame pMainGame, SceneGameplay pCurrentScene, Map pLastMap) 
			: base(pMainGame, pCurrentScene, pLastMap)
		{
			
		}

		public override void Load()
		{
			FileName = "house";
			HeroPosition = new Vector2(MainGame.ScreenWidth / 6.83f,
									   MainGame.ScreenHeight / 2.05f);
			IsSave = false;
			IsShader = false;
			CameraFix = true;
			CameraPosition = new Vector2(-MainGame.ScreenWidth / 1.6f, -MainGame.ScreenHeight / 1.2f);

			base.Load();
		}
	}
}
