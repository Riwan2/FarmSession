using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class Crop
	{
		//Crop
		public int Index;
		public int TileId;
		public int currentState;
		public Vector2 Position;
		public bool Delete;
		public List<int> lstTileId;

		public int beginDay;
		private int currentDay;
		public int maxState;

		TimeManager timeManager;

		private bool groundWet;

		public Crop()
		{

		}

		public Crop(int pIndex, Crop pCopy, TimeManager pTimeManager, int pState = 0)
		{
			Index = pIndex;

			TileId = pCopy.lstTileId[pState];
			lstTileId = pCopy.lstTileId;

			currentState = pState;
			maxState = lstTileId.Count;

			timeManager = pTimeManager;
			beginDay = timeManager.CurrentDay;
			currentDay = timeManager.CurrentDay;

			groundWet = false;

			Load();
		}

		private void Load()
		{
			//TileObject
			Position = new Vector2(Index - (Math.Abs(Index / TileMap.MapWidth) * TileMap.MapWidth),
								   Math.Abs(Index / TileMap.MapWidth));
			Position = new Vector2(Position.X * TileMap.TileWidth, Position.Y * TileMap.TileHeight);
			Delete = false;

			TileMap.lstLayer[1][Index].Gid = lstTileId[currentState];
		}

		public void Update(GameTime gameTime)
		{
			if (util.getTileType(TileMap.lstLayer[0][Index].Gid) == "farmLandWet") {
				groundWet = true;
			}

			if (currentDay < timeManager.CurrentDay && !groundWet) {
				beginDay++;
				currentDay++;
			}

			if (beginDay + 1 <= timeManager.CurrentDay && groundWet) {
				currentState++;
				TileMap.lstLayer[0][Index].Gid = util.getTileId("farmLand");

				if (currentState < maxState) {
					TileMap.lstLayer[1][Index].Gid = lstTileId[currentState];
				} else {
					TileMap.lstLayer[0][Index].Gid = util.getTileId("farmLand");
					Delete = true;
				}

				groundWet = false;
			}


		}
	}
}
