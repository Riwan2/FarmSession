using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class LightShader
	{
		public MainGame mainGame;

		Texture2D lightMask; //LightTexture
		RenderTarget2D lightTarget; //RenderTarget

		Effect lightEffect; //Shader
		public List<LightSource> lstLightSource; //LightSource object

		public Color BackgroundColor; //The background of the light RenderTarget

		public LightShader(MainGame pMainGame)
		{
			mainGame = pMainGame;
			lstLightSource = new List<LightSource>();
			BackgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
		}

		public void AddLightShader(Vector2 pPosition, Color pColor) //Add LightSource to the RenderTarget
		{
			LightSource light = new LightSource(pPosition, pColor);
			lstLightSource.Add(light);
		}

		public void AmbientColor(float pColor) //Change color of the Light RenderTarget Background
		{
			BackgroundColor = new Color(pColor, pColor, pColor, 1);
		}

		public void Load()
		{
			//RenderTarget
			lightMask = mainGame.Content.Load<Texture2D>("WhiteLightCircle");
			lightEffect = mainGame.Content.Load<Effect>("Shader/FirstShader");

			//Size of the screen
			var pp = mainGame.GraphicsDevice.PresentationParameters;
			lightTarget = new RenderTarget2D(mainGame.GraphicsDevice, 
			                                 TileMap.MapWidth * TileMap.TileWidth, 
			                                 TileMap.MapHeight * TileMap.TileHeight);
		}

		public void Draw(RenderTarget2D mainTarget, Camera camera)
		{
			mainGame.GraphicsDevice.SetRenderTargets(lightTarget);
			mainGame.GraphicsDevice.Clear(BackgroundColor); //The Mask Color

			//Draw of the Light
			mainGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
			for (int i = 0; i < lstLightSource.Count; i++) {
				LightSource ls = lstLightSource[i];
				mainGame.spriteBatch.Draw(lightMask, ls.Position, null, ls.Color, 0f,
										  new Vector2(lightMask.Width / 2, lightMask.Height / 2),
										 1, SpriteEffects.None, 0);
				
				if (ls.Delete == true) {
					lstLightSource.RemoveAt(i);
				}
			}
			mainGame.spriteBatch.End();

			//Light Applied to the Draw of the mainTarget
			mainGame.GraphicsDevice.SetRenderTargets(null);

			mainGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, 
			                           null, null, null, camera.Transform);
			lightEffect.Parameters["lightMask"].SetValue(lightTarget);
			lightEffect.CurrentTechnique.Passes[0].Apply();
			mainGame.spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
			mainGame.spriteBatch.End();
		}
	}

	//LightSource
	public class LightSource //LightSource Object
	{
		public Vector2 Position;
		public Color Color;
		public bool Delete;

		public LightSource(Vector2 pPosition, Color pColor)
		{
			Position = pPosition;
			Color = pColor;
			Delete = false;
		}
	}
}
