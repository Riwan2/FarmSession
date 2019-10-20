using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public interface IActor
	{
		Vector2 Position { get; }
		Rectangle BoundingBox { get; }
		bool IsInvisible { get; }
		bool Delete { get; }

		void Update(GameTime pGameTime);
		void Draw(SpriteBatch spriteBatch);
	}
}
