using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class OverlayInventory : Overlay
	{
		public Vector2 Position { get; private set; }
		private InventoryManager inventoryManager;
		private ItemBar itemBar;

		private Texture2D background;
		private Texture2D textureSlot;
		private Texture2D textureSlotSelect;

		private List<InventoryIcon> lstIcons;
		private List<Rectangle> lstSlots;
		private int slotSelected;
		private int slotFrom;

		public const int COLS = 11;
		public const int LINES = 6;

		private Vector2 mouseStart;
		private InventoryIcon dragIcon;

		private const int WIDTHICON = 50;
		private const int HEIGHTICON = 50;
		private Color color;

		public OverlayInventory(MainGame pMainGame, InventoryManager pInventory, Vector2 pPosition, ItemBar pItemBar) : base(pMainGame)
		{
			isActive = false;
			Position = pPosition;

			inventoryManager = pInventory;
			itemBar = pItemBar;

			background = mainGame.Content.Load<Texture2D>("GUI/background");
			textureSlot = mainGame.Content.Load<Texture2D>("GUI/slot");
			textureSlotSelect = mainGame.Content.Load<Texture2D>("GUI/slotSelected");

			lstIcons = new List<InventoryIcon>();
			lstSlots = new List<Rectangle>();
			color = new Color(255, 255, 255, 0.95f);
		}
		

		public void Populate()
		{
			lstIcons.Clear();
			lstSlots.Clear();
			float x = Position.X + 9f;
			float y = Position.Y + 9f;
			slotSelected = -1;
			slotFrom = -1;
			int col = 1;
			int line = 1;
			int slot = 0;

			for (int i = 0; i < InventoryManager.MAXITEM; i++) {

				float xIcon = x + (col - 1) * (textureSlot.Width + 3);
				float yIcon = y + (line - 1) * (textureSlot.Height + 7);
				
				lstSlots.Add(new Rectangle((int)xIcon, (int)yIcon, WIDTHICON, HEIGHTICON));

				Item item = inventoryManager.GetItemAt(slot);

				if (item != null) {
					Texture2D texture = ItemTexture.Textures[item.ID];
					lstIcons.Add(new InventoryIcon(texture, new Vector2(xIcon, yIcon), item.Quantity, true));
				}

				slot++;

				col++;
				if (col > COLS) {
					col = 1;
					line++;
					if (line > LINES) {
						break;
					}
				}
			}

			itemBar.Populate();
		}

		public void Update(MouseState newMouseState, MouseState oldMouseState, Point mousePos)
		{
			if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				for (int i = 0; i < lstIcons.Count; i++) {
					if (lstIcons[i].HandleRect.Contains(mousePos) && lstIcons[i].isDraggable) {
						lstIcons[i].isDragging = true;
						dragIcon = lstIcons[i];
						break;
					}
				}
				if (dragIcon != null) {

					for (int i = 0; i < COLS * LINES; i++) {
						if (lstSlots[i].Contains(mousePos)) {
							slotSelected = i;
							slotFrom = i;
						}
					}
					mouseStart = new Vector2(mousePos.X, mousePos.Y);
					dragIcon.Touch(new DragEvent
					{
						phase = ePhase.began,
						startX = mouseStart.X,
						startY = mouseStart.Y,
						X = mousePos.X,
						Y = mousePos.Y
					});
				}
			} else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed
					   && dragIcon != null) {
				dragIcon.Touch(new DragEvent
				{
					phase = ePhase.move,
					startX = mouseStart.X,
					startY = mouseStart.Y,
					X = mousePos.X,
					Y = mousePos.Y
				});
				for (int i = 0; i < COLS * LINES; i++) {
					if (lstSlots[i].Contains(dragIcon.GetCenter())) {
						slotSelected = i;
					}
				}
			} else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Released
					   && dragIcon != null) {
				slotSelected = -1;
				int slotTo = -1;
				for (int i = 0; i < COLS * LINES; i++) {
					if (lstSlots[i].Contains(dragIcon.GetCenter())) {
						slotTo = i;
						dragIcon.Touch(new DragEvent
						{
							phase = ePhase.ended,
							startX = mouseStart.X,
							startY = mouseStart.Y,
							X = mousePos.X,
							Y = mousePos.Y
						});
					}
				}

				Item itFrom = inventoryManager.GetItemAt(slotFrom);
				Item itTo = inventoryManager.GetItemAt(slotTo);
				if (itFrom != null && slotTo != -1 && (slotTo != slotFrom)) {
					if (itTo != null) {
						if (itFrom.Collectible && itTo.Collectible && itFrom.ID == itTo.ID) {
							itTo.Quantity += itFrom.Quantity;
							if (itTo.Quantity > itTo.MaxPerSlot) {
								int dif = itTo.Quantity - itTo.MaxPerSlot;
								itTo.Quantity = itTo.MaxPerSlot;
								itFrom.Quantity = dif;
							} else {
								itFrom.Quantity = 0;
							}
						} else {
							itTo.InventorySlot = itFrom.InventorySlot;
							itFrom.InventorySlot = slotTo;
						}
					} else {
						itFrom.InventorySlot = slotTo;
						Console.WriteLine("test1");
					}
					if (itFrom.Quantity == 0 && itFrom.Collectible) {
						inventoryManager.RemoveItem(itFrom.InventorySlot);
					}
				}
				dragIcon.Touch(new DragEvent
				{
					phase = ePhase.cancelled,
					startX = mouseStart.X,
					startY = mouseStart.Y,
					X = mousePos.X,
					Y = mousePos.Y
				});
				Populate();
				dragIcon = null;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(background, Position, Color.White);

			float x = Position.X + 9f;
			float y = Position.Y + 9f;

			int line = 1;
			int col = 1;
			int slot = 0;

			for (int i = 0; i < (7 * 11); i++) {

				float xItem = x + (col - 1) * (textureSlot.Width + 3);
				float yItem = y + (line - 1) * (textureSlot.Height + 7);
				if (slot == slotSelected) {
					spriteBatch.Draw(textureSlotSelect, new Vector2(xItem, yItem), Color.White);
				} else {
					spriteBatch.Draw(textureSlot, new Vector2(xItem, yItem), Color.White);
				}

				slot++;

				col++;
				if (col > COLS) {
					line++;
					col = 1;
					if (line > LINES) {
						break;
					}
				}
			}

			for (int i = 0; i < lstIcons.Count; i++) {
				lstIcons[i].Draw(spriteBatch);
			}
		}
	}
}
