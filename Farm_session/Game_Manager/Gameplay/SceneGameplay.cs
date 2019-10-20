using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class SceneGameplay : Scene
	{
		//Player
		public Hero myCharacter;
		public static int GameplayScale { get; set; } = 3;
		//GameFile
		public GameFile gameFile;
		//Map
		public TileMap map;
		public MapFile mapFile;
		public bool mapSaving;
		//TileObject
		public static List<TileObject> TileObjectList;
		public List<Crop> CropList;
		//BuildMode
		BuildMode buildMode;
		//KeysBoard
		private KeyboardState newKeyboardState;
		private KeyboardState oldKeyboardState;
		//Mouse
		private MouseState newMouseState;
		private MouseState oldMouseState;
		private Point mousePos;
		//GUI
		private GameplayOutput gameplayOutput;
		public double Fps_Count;
		//Shader
		RenderTarget2D mainTarget;
		public LightShader lightShaderManager;
		bool shaderActivated;
		//TimeManager
		public TimeManager timeManager;
		//MapManager
		public MapManager mapManager;
		//Camera
		public static Camera camera;
		//ParticleEngine
		ParticleEngine particleEngine;
		//SpellManager
		public SpellManager spellManager;
		//Enemie
		public static EnemieManager enemieManager;
		EnemieOutput enemieOutput;
		//Layer
		LayerManager layerManager;

		public SceneGameplay(MainGame pMainGame) : base(pMainGame)
		{
			
		}

		public override void Load()
		{
			Rectangle Screen = mainGame.Window.ClientBounds;

			//GameFile
			gameFile = new GameFile(this);
			gameFile.ImportingGameData();

			//Hero set
			myCharacter = new Hero(mainGame.Content.Load<Texture2D>("Player/HeroWalk_down"),
								   mainGame.Content.Load<Texture2D>("PlayerSelector"),
								   16, 48, 16, 16, 0.4f, GameplayScale, this,
								   mainGame.Content.Load<Texture2D>("Player/MageBaton"));
			myCharacter.Position = gameFile.HeroPosition;

			myCharacter.Animation["walk_left"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_left");
			myCharacter.Animation["walk_up"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_up");
			myCharacter.Animation["walk_right"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_right");
			myCharacter.Animation["walk_down"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_down");
			//Baton
			myCharacter.Animation["walk_left_baton"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_left_baton");
			myCharacter.Animation["walk_up_baton"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_up_baton");
			myCharacter.Animation["walk_right_baton"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_right_baton");
			myCharacter.Animation["walk_down_baton"] = mainGame.Content.Load<Texture2D>("Player/HeroWalk_down_baton");

			//TimeManager
			timeManager = new TimeManager(13);
			timeManager.SetHour(gameFile.CurrentTime);

			//Camera
			camera = new Camera(mainGame.GraphicsDevice.Viewport, myCharacter.Position);

			//Crop
			map = new TileMap(mainGame, "PlayerLevel");
			map.LoadMap();
			CropData.PopulateData();
			CropList = new List<Crop>();
			CropRecup();
			map = null;

			//Spell
			spellManager = new SpellManager();
			PassivSpellData.PopulateData(mainGame.Content);
			OffensiveSpellData.PopulateData(mainGame.Content);

			//Enemie
			EnemieData.PopulateData(mainGame.Content);
			enemieOutput = new EnemieOutput();

			//mapFile loading
			mapFile = new MapFile();

			//GUI
			gameplayOutput = new GameplayOutput(new Vector2(Screen.Width / 2, Screen.Height / 2), mainGame, mapFile, myCharacter, this);

			//ChangeMap
			mapManager = new MapManager(mainGame, this);
			mapManager.ChangeMap(gameFile.CurrentMap);
			Console.WriteLine(timeManager);
			if (SceneMenu.LoadSaves) {
				mapFile.LoadingMap();
				mapFile.SetMapFile();

				map = new TileMap(mainGame, mapManager.CurrentMap.FileName);
				//map loading
				map.LoadMap();
				mapSaving = false;

				myCharacter.Position = gameFile.HeroPosition;
				//Camera
				camera.CameraFix = mapManager.CurrentMap.CameraFix;
				camera.Position = new Vector2(-MainGame.ScreenWidth, -MainGame.ScreenHeight);

				if (camera.CameraFix) {
					camera.Position = mapManager.CurrentMap.CameraPosition;
				} else {
					if (myCharacter.Position.Y >= (TileMap.MapHeight * TileMap.TileHeight) - MainGame.ScreenHeight / 2) {
						camera.Position = new Vector2(camera.Position.X, -(TileMap.MapHeight * TileMap.TileHeight -
						                                                  MainGame.ScreenHeight) - MainGame.ScreenHeight);
					}
					if (myCharacter.Position.X >= (TileMap.MapWidth * TileMap.TileWidth) - MainGame.ScreenWidth / 2) {
						camera.Position = new Vector2(-(TileMap.MapWidth * TileMap.TileWidth -
						                                MainGame.ScreenWidth) - MainGame.ScreenWidth, camera.Position.Y);
					}
				}
				//Inventory + time
				timeManager.SetHour(gameFile.CurrentTime);
				gameFile.GetInventory(gameplayOutput.InventoryManager);
				gameplayOutput.overlayInventory.Populate();
			} else {
				//Create + save the map
				map = new TileMap(mainGame, "level1");
				map.LoadMap();
				mapFile.SavingMap();
				mapFile.SetMapFile();
				mapFile.LoadingMap();
				gameFile.SavingGameData();
				gameFile.ImportingGameData();
				mapManager.ChangeMap("level1");
				mapSaving = false;
			}

			//Keyboard
			oldKeyboardState = Keyboard.GetState();
			//Mouse
			oldMouseState = Mouse.GetState();

			//buildMode set
			buildMode = new BuildMode(mainGame.Content.Load<Texture2D>("Selector"),
									  mainGame.Content.Load<Texture2D>("tileset"), this);

			//ParticleEngine
			particleEngine = new ParticleEngine(mainGame, myCharacter.Position, myCharacter);
			//particleEngine.PlayerFootPrint = true;

			//Layer
			layerManager = new LayerManager(this);

			base.Load();
		}

		public override void Unload()
		{

			base.Unload();
		}

		public override void Update(GameTime gameTime)
		{
			newKeyboardState = Keyboard.GetState();
			newMouseState = Mouse.GetState();
			mousePos = newMouseState.Position;

			//GUI
			gameplayOutput.Update(gameTime, newKeyboardState, oldKeyboardState, newMouseState, oldMouseState, mousePos);

			if (!gameplayOutput.OverlayActive) {

				if (newKeyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter)) {
					mapSaving = true;
					mapFile.SavingMap();
					Console.WriteLine("saving the game");
					mapSaving = false;
				}

				if (!mapSaving && buildMode.BuildModeActivated) {
					buildMode.BuildUpdate(gameTime, newKeyboardState, oldKeyboardState);
					myCharacter.IsInvisible = true;
				} else {
					myCharacter.IsInvisible = false;
				}

				base.Update(gameTime);

				//BuildMode
				if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B)) {

					buildMode.BuildModeActivated = !buildMode.BuildModeActivated;
					buildMode.selector.IsInvisible = !buildMode.selector.IsInvisible;
				}

				//TileObject
				for (int i = 0; i < TileObjectList.Count; i++) {
					if (TileObjectList[i].Delete) {
						TileObjectList.RemoveAt(i);
					} else {
						//TODO update for Tile Object
					}
				}

				if (mapManager.CurrentMap != null) {
					mapManager.CurrentMap.Update(gameTime);
				}

				//Character
				myCharacter.Update(gameTime, newKeyboardState, oldKeyboardState, newMouseState, oldMouseState, mousePos);
				//Enemie
				enemieManager.Update(gameTime);
				enemieOutput.Update(gameTime);
				//LightShader
				lightShaderManager.AmbientColor(timeManager.BackgroundValue);
				//Camera
				camera.UpdateCamera(mainGame.GraphicsDevice.Viewport, myCharacter);
				//TimeManager
				timeManager.Update(gameTime);
				//ParticleEngine
				particleEngine.Update(gameTime);
				//SpellManager
				spellManager.Update(gameTime);
				//LayerManager
				layerManager.Update(gameTime);

				map.Update(gameTime);

			}

			oldKeyboardState = Keyboard.GetState();
			oldMouseState = Mouse.GetState();
		}

		public override void Draw(GameTime gameTime)
		{
			if (!mapSaving) {
				//Draw here all the game stuff whe will be altered by the lightShader
				mainGame.GraphicsDevice.SetRenderTargets(mainTarget);
				mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
										   null, null, null, null);
				//map draw
				map.Draw(mainGame.spriteBatch);
				//ParticleEngine
				particleEngine.Draw(gameTime);
				//buildMode draw
				buildMode.Draw(mainGame.spriteBatch);
				//Draw scene
				base.Draw(gameTime);
				//CurrentMap
				if (mapManager.CurrentMap != null) {
					mapManager.CurrentMap.Draw(mainGame.spriteBatch);
				}
				mainGame.spriteBatch.End();

				//SpellManager Got his own spriteBatch and his own Shader
				spellManager.Draw(mainGame.spriteBatch);

				//Draw Here Object who need to be layered
				mainGame.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
				                           null, null, null, null);
				//Enemie
				enemieManager.Draw(mainGame.spriteBatch);
				//Character
				myCharacter.Draw(mainGame.spriteBatch);
				mainGame.spriteBatch.End();

				if (shaderActivated) {
					lightShaderManager.Draw(mainTarget, camera); //Shader
				} else {
					mainGame.GraphicsDevice.SetRenderTarget(null);
					mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, 
					                           null, null, camera.Transform);
					mainGame.spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
					mainGame.spriteBatch.End();
				}

				//Draw GameObject who can't be modified by lightShader
				mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, null,
				                           null, null, null, camera.Transform);
				enemieOutput.Draw(mainGame.spriteBatch);
				mainGame.spriteBatch.End();

				//Draw all the gui
				mainGame.spriteBatch.Begin();
				gameplayOutput.Draw(mainGame.spriteBatch); //GUI
				mainGame.spriteBatch.End();

				Fps_Count = Math.Floor(1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds);
			}
		}

		//SHADER
		public void ShaderSet(bool activated)
		{
			//Shader
			var pp = mainGame.GraphicsDevice.PresentationParameters;
			mainTarget = new RenderTarget2D(mainGame.GraphicsDevice, TileMap.MapWidth * TileMap.TileWidth,
			                                TileMap.MapHeight * TileMap.TileHeight);

			lightShaderManager = new LightShader(mainGame);

			if (activated) {
				lightShaderManager.AmbientColor(timeManager.BackgroundValue);
				lightShaderManager.Load();
				shaderActivated = true;
			} else {
				shaderActivated = false;
			}
		}

		//GET INFO BY THE MAP
		public void TileObjectRecup()
		{
			//Recup map tileObject
			TileObjectList = new List<TileObject>();

			for (int i = 0; i < map.TileObjectIndex.Count; i++) {
				int gid = TileMap.lstLayer[1][map.TileObjectIndex[i]].Gid;
				if (gid == 25) {
					int index = map.TileObjectIndex[i];
					Vector2 Position = new Vector2(index - (Math.Abs(index / TileMap.MapWidth) * TileMap.MapWidth),
												   Math.Abs(index / TileMap.MapWidth));
					Position = new Vector2(Position.X * TileMap.TileWidth + TileMap.TileWidth / 2,
										   Position.Y * TileMap.TileHeight + TileMap.TileHeight / 2); //centred
					lightShaderManager.AddLightShader(Position, Color.Yellow);
				}
			}
		}

		public void CropRecup() //Recup the crop on the map
		{
			for (int i = 0; i < map.TileObjectIndex.Count; i++) {
				int gid = TileMap.lstLayer[1][map.TileObjectIndex[i]].Gid;
				if (gid == 21) {
					Crop crop = new Crop(map.TileObjectIndex[i], CropData.Data["WHEAT"], timeManager, 0);
					CropList.Add(crop);
				} else if (gid == 22) {
					Crop crop = new Crop(map.TileObjectIndex[i], CropData.Data["WHEAT"], timeManager, 1);
					CropList.Add(crop);
				} else if (gid == 23) {
					Crop crop = new Crop(map.TileObjectIndex[i], CropData.Data["WHEAT"], timeManager, 2);
					CropList.Add(crop);
				}
			}
		}
	}
}