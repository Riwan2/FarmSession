using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class OverlayOptionMenu : Overlay
	{
		public SceneGameplay currentScene;
		public Vector2 Position { get; set; }

		//OptionMenu
		public static int Width = 190;
		public static int Height = 135;

		//Button
		private int buttonScale = 1;
		private int buttonSpace = 10;
		private Button optionButton;
		private Button saveButton;
		private Button quitButton;

		//Gui
		private List<Button> lstButton;

		public OverlayOptionMenu(Vector2 pPosition, MainGame pMainGame, SceneGameplay pCurrentScene) : base (pMainGame)
		{
			Position = pPosition;
			currentScene = pCurrentScene;

			Load();
		}

		private void Load()
		{
			lstButton = new List<Button>();

			optionButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"), 570, 48, 190, 48, 0, buttonScale),
									  "Option", new Vector2(Position.X, Position.Y));
			
			lstButton.Add(optionButton);

			saveButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"), 570, 48, 190, 48, 0, buttonScale),
									  "Save", new Vector2(Position.X, Position.Y + 45 + buttonSpace));
			saveButton.onClick = onClickSave;
			lstButton.Add(saveButton);

			quitButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"), 570, 48, 190, 48, 0, buttonScale),
									"Quit", new Vector2(Position.X, Position.Y + 45*2 + buttonSpace*2));
			quitButton.onClick = onClickQuit;
			lstButton.Add(quitButton);

			isActive = false;
		}

		public void Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState, Point mousePos)
		{
			if (!currentScene.mapSaving) {
				for (int i = 0; i < lstButton.Count; i++) {
					lstButton[i].Update(gameTime, oldMouseState, newMouseState, mousePos);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < lstButton.Count; i++) {
				lstButton[i].Draw(spriteBatch);
			}
		}

		public void onClickQuit(Button pSender)
		{
			mainGame.gameState.ChangeScene(GameState.SceneType.Menu);
		}

		public void onClickSave(Button pSender)
		{
			bool saving = true;
			Console.WriteLine(currentScene.mapManager.CurrentMap.FileName);

			while (saving) {
				if (currentScene.mapManager.CurrentMap.IsSave) {
					currentScene.mapFile.SavingMap();
				}
				currentScene.gameFile.SavingGameData();
				Console.WriteLine(currentScene.myCharacter.Position);
				saving = false;
			}
		}
	}
}
