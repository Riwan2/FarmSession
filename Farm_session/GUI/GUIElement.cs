using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public interface GUIElement
	{
		Vector2 Position { get; set; }
		string ElementString { get; set; }
		bool IsInvisible { get; set; }

		void Draw(SpriteBatch spriteBatch);
	}
}
