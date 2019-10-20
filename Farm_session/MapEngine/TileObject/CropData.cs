using System;
using System.Collections.Generic;

namespace Farming_session
{
	public static class CropData
	{
		public enum eCropType
		{
			None,
			Wheat
		};

		public static Dictionary<string, Crop> Data;

		public static void PopulateData()
		{
			Data = new Dictionary<string, Crop>();

			Data.Add("WHEAT", new Crop {
				lstTileId = new List<int>() { 21, 22, 23, 24 }
			});
		}
	}
}
