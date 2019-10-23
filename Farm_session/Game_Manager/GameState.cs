using System;
namespace Farming_session
{
	public class GameState
	{
		public enum SceneType
		{
			Menu,
			Gameplay,
			Test,
		}

		protected MainGame mainGame;
		public Scene currentScene { get; set; }

		public GameState(MainGame pMainGame)
		{
			mainGame = pMainGame;
		}

		public void ChangeScene(SceneType pSceneType)
		{
			if (currentScene != null) {
				currentScene.Unload();
				currentScene = null;
			}

			switch (pSceneType) {
				case SceneType.Menu:
					currentScene = new SceneMenu(mainGame);
					break;
				case SceneType.Gameplay:
					currentScene = new SceneGameplay(mainGame);
					break;
				case SceneType.Test:
					currentScene = new SceneTest(mainGame);
					break;
				default:
					break;
			}
			currentScene.Load();
		}
	}
}
