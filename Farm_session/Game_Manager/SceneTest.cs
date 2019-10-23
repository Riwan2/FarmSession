using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	class SceneTest : Scene
	{
		PathFinding pathFinding;

		public SceneTest(MainGame pMainGame) : base(pMainGame)
		{
		}

		public override void Load()
		{
			pathFinding = new PathFinding(mainGame);
			base.Load();
		}

		public override void Unload()
		{
			base.Unload();
		}

		public override void Update(GameTime gameTime)
		{
			pathFinding.Update(gameTime);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			pathFinding.Draw(mainGame.spriteBatch);
			base.Draw(gameTime);
		}
	}
}