using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class Item
	{
		public string ID;
		public string Name;
		public bool Collectible;
		public int MaxPerSlot;
		public int Quantity;
		public int InventorySlot;
		public ItemData.eItemType Type;

		public Item()
		{

		}

		public Item(Item pCopy) //Create item by copy
		{
			ID = pCopy.ID;
			Name = pCopy.Name;
			Collectible = pCopy.Collectible;
			MaxPerSlot = pCopy.MaxPerSlot;
			Quantity = pCopy.Quantity;
			InventorySlot = pCopy.InventorySlot;
			Type = pCopy.Type;
		}
	}

	public static class ItemData
	{
		public enum eItemType //Type of the item
		{
			None,
			Ressource,
			Seed,
			Tool,
			Staff
		};

		public static Dictionary<string, Item> Data; //Store the data of all the item

		public static void PopulateData()
		{
			Data = new Dictionary<string, Item>();

			Data.Add("WHEAT", new Item {
				ID = "WHEAT", Name = "Wheat", Collectible = true, MaxPerSlot = 48,
				Type = eItemType.Ressource
			});
			Data.Add("WHEATSEED", new Item {
				ID = "WHEATSEED", Name = "WheatSeed", Collectible = true,
				MaxPerSlot = 24, Type = eItemType.Seed
			});
			Data.Add("SCYTHE", new Item { ID = "SCYTHE", Name = "Scythe", Collectible = false, Type = eItemType.Tool });
			Data.Add("BASEBATON", new Item { ID = "BASEBATON", Name = "Magic Wand", Collectible = false, Type = eItemType.Staff });
		}
	}
}
