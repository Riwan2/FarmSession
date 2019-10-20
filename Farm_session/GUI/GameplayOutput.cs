using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class GameplayOutput
	{
		//Base
		private MainGame mainGame;
		private SceneGameplay currentScene;
		public Vector2 Position { get; private set; }

		//Option
		private OverlayOptionMenu overlayOptionMenu;

		//Inventory
		public OverlayInventory overlayInventory;
		public InventoryManager InventoryManager;

		public ItemBar itemBar;

		public bool OverlayActive { get; private set; }

		//Paramaters
		private MapFile currentMapFile;
		private Hero player;

		//Time
		private Vector2 timePosition;

		//Fps
		private double Fps_Count;
		private Vector2 Fps_CountPosition;

		//interact
		public bool IsInteracting;
		private string interactString;
		private Vector2 interactPosition;

		//spellSelector
		HeroSpellSelector spellSelector;

		public GameplayOutput(Vector2 pPosition, MainGame pMainGame, MapFile mapFile, Hero pPlayer, SceneGameplay pCurrentScene)
		{
			Position = pPosition;
			mainGame = pMainGame;
			currentScene = pCurrentScene;
			currentMapFile = mapFile;
			player = pPlayer;

			//Item Inventory Texture
			ItemTexture.Populate(mainGame);

			//Item Data
			ItemData.PopulateData();

			//Overlay
			OverlayActive = false;
			overlayOptionMenu = new OverlayOptionMenu(new Vector2(MainGame.ScreenWidth / 2 - OverlayOptionMenu.Width / 2,
			                                        (int)(MainGame.ScreenHeight / 2) - OverlayOptionMenu.Height / 2), 
			                            mainGame, currentScene);

			//Inventory
			InventoryManager = new InventoryManager();
			InventoryManager.AddObject("SCYTHE", 1);
			InventoryManager.AddObject("BASEBATON", 1);

			//ItemBar
			float itemBarX = MainGame.ScreenWidth / 2 - 520 / 2;
			float itemBarY = MainGame.ScreenHeight - MainGame.ScreenHeight / 6 + 50 / 2;
			itemBar = new ItemBar(InventoryManager, new Vector2(itemBarX, itemBarY));
			itemBar.Populate();

			//InvetoryOverlay
			float InventoryposX = MainGame.ScreenWidth/2 - 600 / 2;
			float InventoryposY = MainGame.ScreenHeight/2 - 350 / 2;
			overlayInventory = new OverlayInventory(mainGame, InventoryManager, new Vector2(InventoryposX, InventoryposY), itemBar);

			overlayInventory.Populate();

			//player
			player.inventoryManager = InventoryManager;
			player.overlayInventory = overlayInventory;

			//Time
			timePosition = new Vector2(MainGame.ScreenWidth - 70, 5);

			//Fps
			Fps_CountPosition = new Vector2(0, 0);

			//Interact
			interactString = "Press R";
			IsInteracting = false;
			interactPosition = new Vector2(0, 0);

			//SpellSelector
			spellSelector = new HeroSpellSelector(player);
		}

		public void Update(GameTime gameTime, KeyboardState newKeyState, KeyboardState oldKeyState,
		                  MouseState newMouseState, MouseState oldMouseState, Point mousePos)
		{
			
			if (newKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape) && !overlayInventory.isActive) {
				overlayOptionMenu.isActive = !overlayOptionMenu.isActive;
				OverlayActive = overlayOptionMenu.isActive;
			}

			if (overlayOptionMenu.isActive) {
				overlayOptionMenu.Update(gameTime, newMouseState, oldMouseState, mousePos);
			}

			if (newKeyState.IsKeyDown(Keys.E) && oldKeyState.IsKeyUp(Keys.E) && !overlayOptionMenu.isActive) {
				overlayInventory.isActive = !overlayInventory.isActive;
				OverlayActive = overlayInventory.isActive;
			}

			if (overlayInventory.isActive) {
				overlayInventory.Update(newMouseState, oldMouseState, mousePos);
			}

			//ItemBar
			itemBar.Update(newMouseState, oldMouseState);
			player.CurrentItemSelectioned = itemBar.CurrentItem;

			//FPS
			Fps_Count = currentScene.Fps_Count;

			//Interact
			IsInteracting = player.IsInteracting;

			//SpellSelector
			spellSelector.Update(gameTime, newKeyState, oldKeyState);

			if (SceneGameplay.camera.CameraFix) {
				interactPosition = new Vector2(player.Position.X - 40 + MainGame.ScreenWidth + SceneGameplay.camera.Position.X,
				                               player.Position.Y - 50 + MainGame.ScreenHeight + SceneGameplay.camera.Position.Y);
			} else if (SceneGameplay.camera.CameraBlocked) {
				interactPosition = new Vector2(player.Position.X - 40, player.Position.Y - 50);
			} else {
				interactPosition = new Vector2(MainGame.ScreenWidth / 2 - 40, MainGame.ScreenHeight / 2 - 50);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//Inventory
			if (overlayOptionMenu.isActive) {
				overlayOptionMenu.Draw(spriteBatch);
			} 
			if (overlayInventory.isActive) {
				overlayInventory.Draw(spriteBatch);
			}
			//ItemBar
			itemBar.Draw(spriteBatch);
			//Interact
			if (IsInteracting) {
				spriteBatch.DrawString(AssetManager.MainFont, interactString, interactPosition,
				                       Color.DarkSlateBlue, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
			}
			//SpellSelector
			spellSelector.Draw(spriteBatch);
			//Time
			string time;
			if (currentScene.timeManager.CurrentHour >= 10) {
				time = currentScene.timeManager.CurrentHour.ToString() + ":00";
			} else {
				time = "0" + currentScene.timeManager.CurrentHour.ToString() + ":00";
			}
			spriteBatch.DrawString(AssetManager.MainFont, time, timePosition, Color.White);
			spriteBatch.DrawString(AssetManager.MainFont, Fps_Count.ToString(), Fps_CountPosition, Color.White);
		}
	}
}
