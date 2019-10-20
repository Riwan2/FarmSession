using System;
using System.Collections.Generic;

namespace Farming_session
{
	public class InventoryManager
	{
		private List<Item> lstInventory;
		public const int MAXITEM = 11 * 6;

		public InventoryManager()
		{
			lstInventory = new List<Item>();
		}

		public void Empty()
		{
			lstInventory = new List<Item>();
		}

		public Item GetItemAt(int pSlot)
		{
			for (int i = 0; i < lstInventory.Count; i++) {
				if (lstInventory[i].InventorySlot == pSlot) {
					return lstInventory[i];
				}
			}
			return null;
		}

		public void AddObjectAt(int pSlot, string pItemID, int pQuantity)
		{
			Item item = new Item(ItemData.Data[pItemID]);
			item.Quantity = pQuantity;
			item.InventorySlot = pSlot;
			lstInventory.Add(item);
		}

		public bool AddObject(string pItemID, int pQuantity)
		{
			if (lstInventory.Count >= MAXITEM) 
				return false;
			
			if (ItemData.Data.ContainsKey(pItemID)) {
				Item item = new Item(ItemData.Data[pItemID]);

				if (pQuantity > item.MaxPerSlot) item.Quantity = item.MaxPerSlot;
				else item.Quantity = pQuantity;

				int slot = -1;

				for (int i = 0; i < MAXITEM; i++) {
					if (GetItemAt(i) == null) {
						slot = i;
						break;
					} else if (GetItemAt(i).Collectible && GetItemAt(i).ID == item.ID && GetItemAt(i) != null) {
						if (GetItemAt(i).Quantity + item.Quantity <= item.MaxPerSlot) {
							GetItemAt(i).Quantity += item.Quantity;
							break;
						}
					}
				}
				if (slot != -1) {
					item.InventorySlot = slot;
					lstInventory.Add(item);
					return true;
				}
			}

			return false;
		}

		public void RemoveItemQuantity(int pSlot, int Quantity)
		{
			for (int i = 0; i < lstInventory.Count; i++) {
				if (lstInventory[i].InventorySlot == pSlot) {
					lstInventory[i].Quantity -= Quantity;
					if (lstInventory[i].Quantity > 0) {
						break;
					} else {
						RemoveItem(lstInventory[i].InventorySlot);
						break;
					}
				}
			}
		}

		public void RemoveItem(int pSlot)
		{
			for (int i = 0; i < lstInventory.Count; i++) {
				if (lstInventory[i].InventorySlot == pSlot) {
					lstInventory.RemoveAt(i);
					break;
				}
			}
		}

		public List<Item> GetObjectList()
		{
			return lstInventory;
		}
	}
}
