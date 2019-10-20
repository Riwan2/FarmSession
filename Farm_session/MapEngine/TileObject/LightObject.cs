using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class LightObject : TileObject
	{
		//TileObject
		public int index { get; }
		public int tileId { get; set; }
		public Vector2 Position { get; set; }
		public bool Delete { get; set; }

		public LightObject(int pIndex)
		{
			index = pIndex;

		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
