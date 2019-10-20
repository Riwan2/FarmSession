using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class ItemBar
	{
		private InventoryManager inventoryManager;
		private List<InventoryIcon> lstIcons;
		public List<Item> LstItems;

		//private Texture2D textureBar;
		public Vector2 Position;

		private const int MAXSLOT = 10;
		private const int SLOTWIDTH = 50;

		public Item CurrentItem { get; private set; }
		private int slotSelected;

		public ItemBar(InventoryManager pInventory, Vector2 pPosition)
		{
			inventoryManager = pInventory;
			lstIcons = new List<InventoryIcon>();
			LstItems = new List<Item>();

			Position = pPosition;

			CurrentItem = null;
			slotSelected = 0;
		}

		public void Populate()
		{
			lstIcons.Clear();
			LstItems.Clear();

			float x = Position.X + 2;
			float y = Position.Y + 2;

			int col = 1;

			for (int i = 0; i < MAXSLOT; i++) {
				float xItem = x + (col - 1) * (SLOTWIDTH + 2);

				Item item = inventoryManager.GetItemAt(i);
				if (item != null) {
					lstIcons.Add(new InventoryIcon(ItemTexture.Textures[item.ID], new Vector2(xItem, y),
												   item.Quantity, false));
					LstItems.Add(item);
				} else {
					lstIcons.Add(null);
					LstItems.Add(null);
				}

				col++;
			}

			CurrentItem = LstItems[slotSelected];
		}

		public void Update(MouseState newMouseState, MouseState oldMouseState)
		{
			if (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue) {
				if (slotSelected + 1 < MAXSLOT) slotSelected++;
				CurrentItem = LstItems[slotSelected];
			}
			if (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue) {
				if (slotSelected - 1 >= 0) slotSelected--;
				CurrentItem = LstItems[slotSelected];
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < MAXSLOT; i++) {
				if (lstIcons[i] != null) {
					lstIcons[i].Draw(spriteBatch);
				}

				float xItem = Position.X + 2 + (slotSelected) * (SLOTWIDTH + 2);
				spriteBatch.DrawString(AssetManager.MainFont, "ABS", new Vector2(xItem, Position.Y + 50), Color.White);
			}
		}
	}
}
