using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	abstract public class Map
	{
		protected MainGame mainGame;
		protected SceneGameplay currentScene;

		protected Vector2 HeroPosition;
		protected Vector2 HeroPosition2;

		public string FileName;
		protected bool IsShader;
		//Save
		public bool IsSave;
		//map
		public string PreviousMap;
		public string NextMap;
		//Camera
		public bool CameraFix;
		public Vector2 CameraPosition;

		protected Map lastMap;

		public Map(MainGame pMainGame, SceneGameplay pCurrentScene, Map pLastMap)
		{
			mainGame = pMainGame;
			currentScene = pCurrentScene;
			lastMap = pLastMap;
		}

		public virtual void Load()
		{
			if (lastMap != null) {
				if (NextMap != null && lastMap.FileName == NextMap) {
					currentScene.myCharacter.Position = HeroPosition2;
				} else {
					currentScene.myCharacter.Position = HeroPosition;
				}

				if (lastMap.IsSave) {
					currentScene.mapFile.SavingMap();
				}
			}

			currentScene.map = new TileMap(mainGame, FileName);
			currentScene.map.LoadMap();

			currentScene.gameFile.SavingGameData();
			//Shader
			currentScene.ShaderSet(IsShader);
			//TileObject
			currentScene.TileObjectRecup();
			//Spell
			currentScene.spellManager.lstSpell = new List<Spell>();
			//Camera
			SceneGameplay.camera.CameraFix = CameraFix;

			if (CameraFix) {
				SceneGameplay.camera.Position = CameraPosition;
			} else {
				SceneGameplay.camera.Position = new Vector2(-MainGame.ScreenWidth, -MainGame.ScreenHeight);
				if (currentScene.myCharacter.Position.Y >= (TileMap.MapHeight * TileMap.TileHeight) - MainGame.ScreenHeight / 2) {
					SceneGameplay.camera.Position = new Vector2(SceneGameplay.camera.Position.X, -(TileMap.MapHeight * TileMap.TileHeight -
																	  MainGame.ScreenHeight) - MainGame.ScreenHeight);
				}
				if (currentScene.myCharacter.Position.X >= (TileMap.MapWidth * TileMap.TileWidth) - MainGame.ScreenWidth / 2) {
					SceneGameplay.camera.Position = new Vector2(-(TileMap.MapWidth * TileMap.TileWidth -
													MainGame.ScreenWidth) - MainGame.ScreenWidth, SceneGameplay.camera.Position.Y);
				}
			}
		}

		public virtual void Unload()
		{

		}

		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{

		}
	}

}
