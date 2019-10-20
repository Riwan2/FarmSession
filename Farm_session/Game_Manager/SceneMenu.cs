using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	class SceneMenu : Scene
	{

		KeyboardState OldKeyboardState;
		KeyboardState NewKeyboardState;

		//Title String
		private string title;
		private Vector2 titlePosition;
		private Vector2 titleStringSize;
		private int titleScale;

		public static bool LoadSaves;

		//Button
		private int buttonScale = 1;
		private int buttonSpace = 10;
		private int buttonWidth = 190;
		private int buttonHeight = 45;
		private Button newGameButton;
		private Button loadGameButton;
		private Button quitGameButton;

		private List<Button> lstButton;

		//Mouse
		private MouseState newMouseState;
		private MouseState oldMouseState;
		private Point mousePos;

		//Test Particle Manager
		ParticleEngine particleEngine;

		public SceneMenu(MainGame pMainGame) : base(pMainGame)
		{
			
		}

		public override void Load()
		{
			//Mouse
			oldMouseState = Mouse.GetState();

			//Button
			Vector2 buttonPosition = new Vector2(MainGame.ScreenWidth / 2 - buttonWidth / 2,
														MainGame.ScreenHeight / 1.6f - buttonHeight / 2);

			lstButton = new List<Button>();

			newGameButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"), 
			                                              570, 48, 190, 48, 0, buttonScale), "New Game", buttonPosition);
			newGameButton.onClick = onClickNewGame;
			lstButton.Add(newGameButton);

			buttonPosition = new Vector2(buttonPosition.X, buttonPosition.Y + buttonHeight + buttonSpace);
			loadGameButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"),
														  570, 48, 190, 48, 0, buttonScale), "Load Save", buttonPosition);
			loadGameButton.onClick = onClickLoadGame;
			lstButton.Add(loadGameButton);

			buttonPosition = new Vector2(buttonPosition.X, buttonPosition.Y + buttonHeight + buttonSpace);
			quitGameButton = new Button(new AnimatedSprite(mainGame.Content.Load<Texture2D>("button"),
														  570, 48, 190, 48, 0, buttonScale), "Quit", buttonPosition);
			quitGameButton.onClick = onClickQuit;
			lstButton.Add(quitGameButton);

			//Title
			title = "FARMING AND FIGHT";
			titleScale = 2;
			SpriteFont font = AssetManager.MainFont;
			titleStringSize = font.MeasureString(title);

			titlePosition = new Vector2(MainGame.ScreenWidth / 2 - titleStringSize.X * titleScale / 2,
										(MainGame.ScreenHeight / 2.9f) - titleStringSize.Y * titleScale / 2);

			//save
			LoadSaves = false;

			//Test particle Engine
			particleEngine = new ParticleEngine(mainGame, new Vector2(MainGame.ScreenWidth / 2,
			                                                          MainGame.ScreenHeight / 2), null);

			base.Load();
		}

		public override void Unload()
		{
			
			base.Unload();
		}

		public void OnClickPlay(Button pSender)
		{
			mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
		}

		public override void Update(GameTime gameTime)
		{
			NewKeyboardState = Keyboard.GetState();
			newMouseState = Mouse.GetState();
			mousePos = new Point(newMouseState.X, newMouseState.Y);

			for (int i = 0; i < lstButton.Count; i++) {
				lstButton[i].Update(gameTime, oldMouseState, newMouseState, mousePos);
			}

			//Particle Engine test
			particleEngine.Update(gameTime);
			particleEngine.Position = new Vector2(mousePos.X, mousePos.Y);

			if (NewKeyboardState.IsKeyDown(Keys.Enter) && OldKeyboardState.IsKeyUp(Keys.Enter)) {
				particleEngine.Load();
			}

			OldKeyboardState = Keyboard.GetState();

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
			                           null, null, null, null);

			mainGame.spriteBatch.DrawString(AssetManager.MainFont, title, titlePosition, 
			                                Color.White, 0, Vector2.Zero, titleScale, 0, 0);

			for (int i = 0; i < lstButton.Count; i++) {
				lstButton[i].Draw(mainGame.spriteBatch);
			}

			//Particle engine test
			particleEngine.Draw(gameTime);
 
			base.Draw(gameTime);
			mainGame.spriteBatch.End();
		}

		public void onClickNewGame(Button pSender)
		{
			LoadSaves = false;
			mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
		}

		public void onClickLoadGame(Button pSender)
		{
			LoadSaves = true;
			mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
		}

		public void onClickQuit(Button pSender)
		{
			mainGame.Exit();
		}
	}
}