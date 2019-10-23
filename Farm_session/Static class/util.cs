using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	class util
	{
		static Random RandomGen = new Random();

		//Get string for id
		public static Dictionary<int, string> TileType = new Dictionary<int, string>()
		{
			{ 13, "grass" },
			{ 1, "grass" },
			{ 7, "grass" },
			{ 2, "path" },
			{ 3, "path" },
			{ 4, "path" },
			{ 5, "path" },
			{ 6, "path" },
			{ 8, "path" },
			{ 9, "path" },
			{ 10, "path" },
			{ 11, "path" },
			{ 12, "path" },
			{ 14, "path" },
			{ 15, "path" },
			{ 16, "path" },
			{ 17, "path" },
			{ 18, "path" },
			{ 19, "farmLand" },
			{ 20, "farmLandWet" },
			{ 21, "wheat" },
			{ 22, "wheat" },
			{ 23, "wheat" },
			{ 24, "wheat" },
			{ 25, "lantern" },
		};

		//Get the id 
		public static Dictionary<string, int> TileId = new Dictionary<string, int>()
		{
			{ "farmLand", 19 },
			{ "farmLandWet", 20 },
			{ "wheatSeed", 21},
			{ "wheatMatured", 24 },
			{ "lantern", 25 },
		};

		//Tile
		public static int getTileId(string pType)
		{
			return TileId[pType];
		}

		public static string getTileType(int pId)
		{
			if (TileType.ContainsKey(pId)) {
				return TileType[pId];
			}
			return "NULL";
		}

		//Seed
		public static void setRandomSedd(int pSeed)
		{
			RandomGen = new Random(pSeed);
		}

		public static int getInt(int pMin, int pMax)
		{
			return RandomGen.Next(pMin, pMax + 1);
		}

		public static double getDouble()
		{
			return RandomGen.NextDouble();
		}

		public static float getAngle(Vector2 a, Vector2 b)
		{
			float xDiff = a.X - b.X;
			float yDiff = a.Y - b.Y;
			return (float)Math.Atan2(yDiff, xDiff);
		}

		public static int getDistance(Vector2 a, Vector2 b)
		{
			int d = (int)((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
			return (int)(Math.Sqrt(d));
		}

		public static int getHeuristic(Point a, Point b)
		{
			//Manhattan distance on a square grid
			return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
		}
	}
}
