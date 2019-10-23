using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class route1 : Map
	{
		private int enemieNumber;

		public route1(MainGame pMainGame, SceneGameplay pCurrentScene, Map pLastMap)
			: base(pMainGame, pCurrentScene, pLastMap)
		{
			SceneGameplay.enemieManager = new EnemieManager();
		}

		public override void Load()
		{
			FileName = "route1";
			HeroPosition = new Vector2(MainGame.ScreenWidth / 21.5f,
									   MainGame.ScreenHeight / 10);
			IsSave = false;
			IsShader = true;
			CameraFix = false;
			PreviousMap = "PlayerLevel";
			base.Load();

			SpawnEnemie();
		}

		public void SpawnEnemie()
		{
			enemieNumber = 10;

			while (enemieNumber > 0) {
				for (int i = 200; i < TileMap.lstLayer[0].Count; i++) {
					if (util.getTileType(TileMap.lstLayer[0][i].Gid) == "path") {
						int aleaNumber = util.getInt(-5, 5);
						if (aleaNumber == 0) {
							int posY = (int)(i / TileMap.MapWidth);
							int posX = i - (TileMap.MapWidth * posY);
							Vector2 Position = new Vector2(posX * TileMap.TileWidth, posY* TileMap.TileHeight);
							Console.WriteLine(Position);
							Champiglu enemie = new Champiglu(EnemieData.Data["Champiglu"], Position, currentScene.myCharacter);
							SceneGameplay.enemieManager.AddEnemie(enemie);
							enemieNumber--;
							if (enemieNumber == 0) {
								break;
							}
						}
					}
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
