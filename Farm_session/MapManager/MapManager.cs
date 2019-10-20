using System;
namespace Farming_session
{
	public class MapManager
	{
		private MainGame mainGame;
		private SceneGameplay sceneGameplay;
		public Map CurrentMap { get; set; }

		private Map lastMap;

		public MapManager(MainGame pMainGame, SceneGameplay pSceneGameplay)
		{
			mainGame = pMainGame;
			sceneGameplay = pSceneGameplay;
		}

		public void ChangeMap(string pMapName)
		{
			if (CurrentMap != null) {
				lastMap = CurrentMap;
				CurrentMap.Unload();
				CurrentMap = null;
			}

			CurrentMap = new route1(mainGame, sceneGameplay, lastMap);

			switch (pMapName) {
				case "PlayerLevel":
					CurrentMap = new PlayerLevel(mainGame, sceneGameplay, lastMap);
					break;
				case "house":
					CurrentMap = new house(mainGame, sceneGameplay, lastMap);
					break;
				case "route1":
					CurrentMap = new route1(mainGame, sceneGameplay, lastMap);
					break;
				case "level1":
					CurrentMap = new level1(mainGame, sceneGameplay, lastMap);
					break;
				default:
					Console.WriteLine("Name not included");
					return;
			}
			Console.WriteLine(pMapName);
			CurrentMap.Load();
		}
	}
}
