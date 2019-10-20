using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class TileObject
	{
		public int Index;
		public int TileId;
		public Vector2 Position;
		public bool Delete;
		public eType Type;

		public enum eType
		{
			None,
			Crop
		};

		public TileObject(int pIndex, int pTileId, Vector2 pPosition, eType pType)
		{
			Index = pIndex;
			TileId = pTileId;
			Position = pPosition;
			Type = pType;
		}

		public TileObject(int pIndex)
		{
			Index = pIndex;
		}

		public TileObject()
		{

		}
	}
}
