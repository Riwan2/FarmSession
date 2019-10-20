using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;

namespace Farming_session
{
	public class BuildMode
	{
		public bool BuildModeActivated { get; set; }
		private SceneGameplay currentScene;

		//Selector
		public Selector selector;
		private Texture2D selectorTexture;
		//TileSelector
		private TileSelector tileSelector;
		private Texture2D tileSelectorTexture;
		//BuildMode string
		private Vector2 stringPosition = new Vector2(0, 0);
		private int stringScale = 1;
		//Mouse
		private MouseState oldMouseState;
		private MouseState newMouseState;
		//Saving the map
		MapFile saveMap = new MapFile();
		//buildModeAffichage string
		private string buildModeString;

		public BuildMode(Texture2D pSelectorTexture, Texture2D pTileSelectorTexture, SceneGameplay pCurrentScene)
		{
			selectorTexture = pSelectorTexture;
			tileSelectorTexture = pTileSelectorTexture;
			currentScene = pCurrentScene;

			Load();	
		}

		private void Load()
		{
			selector = new Selector(selectorTexture);
			tileSelector = new TileSelector(tileSelectorTexture, 96, 80, 16, 16, 2f, SceneGameplay.GameplayScale);
			//Mouse
			oldMouseState = Mouse.GetState();
			//Affichage draw
			buildModeString = "BuildMode Activated \nTileType :" + selector.CurrentTileType + "\n"
							  + "Press enter to save\n" + "A and E to swapp inventory";

			BuildModeActivated = false;
		}

		private void Build()
		{
			int PosX = selector.GridX;
			int PosY = TileMap.MapWidth * selector.GridY;
			int id = tileSelector.currentTileSelected;
			Console.WriteLine("buildfonction");

			//TileObject ?
			if (id == util.getTileId("wheatSeed")) {
				if (TileMap.lstLayer[0][PosX + PosY].Gid == util.getTileId("farmLand")) {
					Crop a = new Crop(PosX + PosY, CropData.Data["WHEAT"], currentScene.timeManager);
					currentScene.CropList.Add(a);
				}

			} else if (id == util.getTileId("lantern")) {
				TileMap.lstLayer[1][PosX + PosY].Gid = id;
				int index = PosX + PosY;
				Vector2 Position = new Vector2(index - (Math.Abs(index / TileMap.MapWidth) * TileMap.MapWidth),
											   Math.Abs(index / TileMap.MapWidth));
				Position = new Vector2(Position.X * TileMap.TileWidth + TileMap.TileWidth / 2,
									   Position.Y * TileMap.TileHeight + TileMap.TileHeight / 2); //centred
				currentScene.lightShaderManager.AddLightShader(Position, Color.Yellow);

			} else {
				TileMap.lstLayer[0][PosX + PosY].Gid = id;
				TileMap.lstLayer[1][PosX + PosY].Gid = 0;
				for (int i = 0; i < SceneGameplay.TileObjectList.Count; i++) {
					if (SceneGameplay.TileObjectList[i].Index == PosX + PosY) {
						SceneGameplay.TileObjectList[i].Delete = true;
					}
				}
			}
		}

		public void BuildUpdate(GameTime gameTime, KeyboardState newKeyboardState, KeyboardState oldKeyboardState)
		{
			newKeyboardState = Keyboard.GetState();

			newMouseState = Mouse.GetState();

			if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				Build();
			}

			oldMouseState = Mouse.GetState();

			//TileSelectorInput
			if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E)) {
				tileSelector.SwitchInventoryRight();
			}

			if (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A)) {
				tileSelector.SwitchInventoryLeft();
			}

			if (BuildModeActivated) {
				selector.Scale = SceneGameplay.GameplayScale;
				selector.Update(gameTime);
				tileSelector.Update(gameTime);
			}

			oldKeyboardState = Keyboard.GetState();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (BuildModeActivated) {
				selector.Draw(spriteBatch);
				tileSelector.Draw(spriteBatch);
				spriteBatch.DrawString(AssetManager.MainFont, buildModeString,
				                       stringPosition, Color.BlanchedAlmond, 0, Vector2.Zero, stringScale, SpriteEffects.None, 0);
			}
		}
	}
}
