using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class MainGame : Game
	{
		GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public GameState gameState;

		public static int ScreenWidth = 1152;
		public static int ScreenHeight = 672;

		public MainGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			gameState = new GameState(this);
			IsMouseVisible = true;
			graphics.PreferredBackBufferWidth = ScreenWidth;
			graphics.PreferredBackBufferHeight = ScreenHeight;
			graphics.IsFullScreen = false;
			graphics.SynchronizeWithVerticalRetrace = true;
		}


		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>	
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//TODO: use this.Content to load your game content here 

			AssetManager.Load(this);
			gameState.ChangeScene(GameState.SceneType.Test);
		}

		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			//if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			//	Exit();
#endif

			// TODO: Add your update logic here

			if (gameState.currentScene != null) {
				gameState.currentScene.Update(gameTime);
			}

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			//TODO: Add your drawing code here
			graphics.GraphicsDevice.Clear(Color.Black);

			if (gameState.currentScene != null) {
				gameState.currentScene.Draw(gameTime);
			}

			base.Draw(gameTime);
		}
	}
}
