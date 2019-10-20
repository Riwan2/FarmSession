using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class Hero : AnimatedSprite
	{
		public int Energy;
		public Dictionary<string, Texture2D> Animation = new Dictionary<string, Texture2D>();
		public enum Direction
		{
			Right,
			Up,
			Left,
			Down,
		}

		//Movement
		public Direction currentDirection;
		public bool inMovement;

		public float Speed;

		//Selection de tile
		HeroTileSelect currentTileSelected;
		private Texture2D selectorTexture;
		public int tilePosX; //Position en fonction de la tilemap
		public int tilePosY;
		public int tileIndex;
		public int[] TilePosGid; //There is 2 layers of map

		//Hero centred draw
		private Vector2 origin;

		//Inventory
		public Item CurrentItemSelectioned { get; set; }
		public InventoryManager inventoryManager;
		public OverlayInventory overlayInventory;
		//SceneGameplay
		private SceneGameplay sceneGameplay;
		//interact
		public bool IsInteracting;
		//Baton
		public bool BatonEquiped = false;
		//Layer
		public float HeroLayer = 0.5f;

		public Hero(Texture2D pTileSheet, Texture2D pSelector, int pTotalWidth, int pTotalHeight, 
		            int pWidth, int pHeight, float pFrameTime, int pScale, SceneGameplay pSceneGameplay, Texture2D pBatonTexture) 
			: base(pTileSheet, pTotalWidth, pTotalHeight, pWidth, pHeight, pFrameTime, pScale)
		{
			Energy = 100;
			selectorTexture = pSelector;
			sceneGameplay = pSceneGameplay;
			Load();
		}

		private void Load()
		{
			currentDirection = Direction.Down;
			inMovement = false;

			//TileSelected ?
			currentTileSelected = new HeroTileSelect(selectorTexture, SceneGameplay.GameplayScale, sceneGameplay);
			//TileMap position
			tilePosX = 0;
			tilePosY = 0;
			TilePosGid = new int[3];

			origin = new Vector2(width / 2, height / 2);

			//Interact
			IsInteracting = false;
		}

		private void Interact(Item item, Point mousePos) //What is Tile Selected? What to do?
		{
			int PosX = (int)currentTileSelected.Position.X / TileMap.TileWidth;
			int PosY = (int)currentTileSelected.Position.Y / TileMap.TileHeight;
			int id = currentTileSelected.Gid;

			if (PosX >= 0 && PosX < TileMap.TileWidth && PosY >= 0 && PosY < TileMap.TileHeight) {
				PosY = PosY * TileMap.MapWidth;
				if (CurrentItemSelectioned != null) {
					if (CurrentItemSelectioned.ID == "SCYTHE") {
						GetMaturedCrop(PosX, PosY, id);
					} else if (CurrentItemSelectioned.ID == "WHEATSEED" && CurrentItemSelectioned.Quantity > 0) {
						Plant(PosX, PosY, id);
					} else if (BatonEquiped) {
						Vector2 TargetPosition = new Vector2(mousePos.X - (SceneGameplay.camera.Position.X + MainGame.ScreenWidth),
												 mousePos.Y - (SceneGameplay.camera.Position.Y + MainGame.ScreenHeight));
						switch (HeroSpellSelector.CurrentSpell) {
							case HeroSpellSelector.eSpell.WaterCrop:
								WaterCrop(mousePos, TargetPosition);
								break;
							case HeroSpellSelector.eSpell.BasicAttack:
								BasicAttack(mousePos, TargetPosition);
								break;
							default:
								break;
						}
					}
				}
			}
		}

		private void BasicAttack(Point mousePos, Vector2 TargetPosition)
		{
			BasicAttackSpell basicAttack = new BasicAttackSpell(OffensiveSpellData.Data["BASIC_ATTACK"],
																Position, TargetPosition, 25);
			sceneGameplay.spellManager.AddOffensiveSpell(basicAttack);
		}

		private void WaterCrop(Point mousePos, Vector2 TargetPosition)
		{
			int id = TileMap.GetTileAt((int)TargetPosition.X, (int)TargetPosition.Y, 0);

			if (util.TileType.ContainsKey(id)) {
				if (util.getTileType(id) == "farmLand") {
					Console.WriteLine("water Spell launch");
					WaterCropsSpell waterSpell;
					waterSpell = new WaterCropsSpell(PassivSpellData.Data["WATER_CROP"], Position, TargetPosition);
					sceneGameplay.spellManager.AddPassivSpell(waterSpell);
				}
			}
		}

		private void GetMaturedCrop(int PosX, int PosY, int id) //Get the plant
		{
			if (id == util.getTileId("wheatMatured")) {
				//Delete the matured wheat
				for (int i = 0; i < SceneGameplay.TileObjectList.Count; i++) {
					if (SceneGameplay.TileObjectList[i].Index == PosX + PosY) {
						SceneGameplay.TileObjectList[i].Delete = true;
					}
				}
				TileMap.lstLayer[1][PosX + PosY].Gid = 0;
				inventoryManager.AddObject("WHEAT", 1);
				int nbWheat = Math.Abs(util.getInt(9, 28) / 10);
				Console.WriteLine(nbWheat);
				if (nbWheat > 0) {
					inventoryManager.AddObject("WHEATSEED", nbWheat);
					overlayInventory.Populate();
				}
			}
		}

		private void Plant(int PosX, int PosY, int idLayer1) //Plant seed
		{
			int idLayer0 = TileMap.lstLayer[0][currentTileSelected.Index].Gid;
			if (idLayer0 == util.getTileId("farmLandWet") && idLayer1 == 0) {
				//Plant seed
				Console.WriteLine("Plant");
				sceneGameplay.CropList.Add(new Crop(PosX + PosY, CropData.Data["WHEAT"], 
				                                          sceneGameplay.timeManager));
				inventoryManager.RemoveItemQuantity(CurrentItemSelectioned.InventorySlot, 1);
				overlayInventory.Populate();
			}
		}

		public void Update(GameTime gameTime, KeyboardState newKeyboardState, KeyboardState oldKeyboardState,
								   MouseState newMouseState, MouseState oldMouseState, Point mousePos)
		{
			//BoundingBox
			BoundingBox = new Rectangle((int)(Position.X + origin.X), (int)(Position.Y + origin.Y),
										(int)(width * scale.X), (int)(height * scale.Y));

			//TileMap position + currentTileGid (under player) 
			tilePosX = Math.Abs((int)(Position.X) / TileMap.TileWidth) + 1;
			tilePosY = Math.Abs((int)(Position.Y) / TileMap.TileHeight) + 1;

			tileIndex = TileMap.MapWidth * tilePosY - (TileMap.MapWidth - (tilePosX - 1));

			if (tileIndex <= TileMap.MapWidth * TileMap.MapHeight && tileIndex >= 0) {
				TilePosGid[0] = TileMap.lstLayer[0][tileIndex].Gid;
				TilePosGid[1] = TileMap.lstLayer[1][tileIndex].Gid;
				TilePosGid[2] = TileMap.lstLayer[2][tileIndex].Gid; //CollisionMap;
			}

			//Vitesse toujours la meme meme avec du lag
			Speed = 1.2f * gameTime.ElapsedGameTime.Milliseconds / 10;

			//Input
			if (newKeyboardState.IsKeyDown(Keys.D) && !CollisionRight()) {
				currentDirection = Direction.Right;
				inMovement = true;
				Move(Speed, 0);
			} else if (newKeyboardState.IsKeyDown(Keys.Z) && !CollisionUp()) {
				currentDirection = Direction.Up;
				inMovement = true;
				Move(0, -Speed);
			} else if (newKeyboardState.IsKeyDown(Keys.Q) && !CollisionLeft()) {
				currentDirection = Direction.Left;
				inMovement = true;
				Move(-Speed, 0);
			} else if (newKeyboardState.IsKeyDown(Keys.S) && !CollisionDown()) {
				currentDirection = Direction.Down;
				inMovement = true;
				Move(0, Speed);
			} else {
				inMovement = false;
			}

			//For animation
			if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q)
				|| newKeyboardState.IsKeyDown(Keys.Z) && oldKeyboardState.IsKeyUp(Keys.Z)
				|| newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D)
				|| newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S)) {
				count = 1;
			}

			//Animation update
			if (inMovement) {
				timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (timer >= frameTime) {
					timer = 0;
					count++;
					if (count >= countLimit) {
						count = 1;
					}
				}
			} else {
				count = 0;
			}

			//Selection facing the player
			currentTileSelected.Update(this, newKeyboardState, oldKeyboardState, mousePos);

			//Interact with left Click
			if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				Interact(CurrentItemSelectioned, mousePos);
			}

			//Door
			if (TilePosGid[1] == 75) {
				IsInteracting = true;
				if (newKeyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R)) {
					sceneGameplay.mapManager.ChangeMap("house");
					Console.WriteLine("change level");
				}
			} else if (TilePosGid[0] == 59) {
				sceneGameplay.mapManager.ChangeMap("PlayerLevel");
			} else if (currentTileSelected.Gid == 66 || currentTileSelected.Gid == 78 ||
				currentTileSelected.Gid == 90) {
				IsInteracting = true;
				if (newKeyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R)) {
					if (sceneGameplay.timeManager.CurrentHour >= 20 ||
						sceneGameplay.timeManager.CurrentHour <= 7) {
						sceneGameplay.timeManager.SetDay(sceneGameplay.timeManager.CurrentDay + 1);
					}
				}
			} else {
				IsInteracting = false;
			}

			//Change Map
			if (TilePosGid[1] == 88) {
				sceneGameplay.mapManager.ChangeMap(sceneGameplay.mapManager.CurrentMap.NextMap);
			} else if (TilePosGid[1] == 87) {
				sceneGameplay.mapManager.ChangeMap(sceneGameplay.mapManager.CurrentMap.PreviousMap);
			}

			//Baton
			if (CurrentItemSelectioned != null) {
				if (CurrentItemSelectioned.ID == "BASEBATON") {
					BatonEquiped = true;
				} else {
					BatonEquiped = false;
				}
			}

			if (currentDirection == Direction.Up) {
				if (BatonEquiped) TileSheet = Animation["walk_up_baton"];
				else TileSheet = Animation["walk_up"];
			} else if (currentDirection == Direction.Down) {
				if (BatonEquiped) TileSheet = Animation["walk_down_baton"];
				else TileSheet = Animation["walk_down"];
			} else if (currentDirection == Direction.Right) {
				if (BatonEquiped) TileSheet = Animation["walk_right_baton"];
				else TileSheet = Animation["walk_right"];
			} else if (currentDirection == Direction.Left) {
				if (BatonEquiped) TileSheet = Animation["walk_left_baton"];
				else TileSheet = Animation["walk_left"];
			}

			//if (sceneGameplay.changeMap.CurrentMap.FileName == "house") {
			//	currentTileSelected.WithMouse = false;
			//} else {
			//	currentTileSelected.WithMouse = true;
			//}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (!IsInvisible) {
				//Selection facing the player draw
				currentTileSelected.Draw(spriteBatch);
				//Player drawing
				spriteBatch.Draw(TileSheet, Position, lstRectangles[count], color, 0f, origin, scale, SpriteEffects.None, HeroLayer);
			}
		}

		//Collision
		private bool CollisionRight()
		{
			Point a = new Point((int)(Position.X + (origin.X * scale.X)*0.85), (int)(Position.Y - (origin.Y * scale.Y)*0.9));
			Point b = new Point((int)(Position.X + (origin.X * scale.X)*0.85), (int)(Position.Y + (origin.Y * scale.Y)*0.9));
			for (int i = 0; i < TileMap.LstCollision.Count; i++) {
				if (TileMap.LstCollision[i].Contains(a) || TileMap.LstCollision[i].Contains(b)
				    || a.X >= TileMap.MapWidth * TileMap.TileWidth) {
					return true;
				}
			}
			return false;
		}
		private bool CollisionLeft()
		{
			Point a = new Point((int)(Position.X - (origin.X * scale.X)*0.88), (int)(Position.Y - (origin.Y * scale.Y) * 0.9));
			Point b = new Point((int)(Position.X - (origin.X * scale.X)*0.88), (int)(Position.Y + (origin.Y * scale.Y) * 0.9));
			for (int i = 0; i < TileMap.LstCollision.Count; i++) {
				if (TileMap.LstCollision[i].Contains(a) || TileMap.LstCollision[i].Contains(b)
				   || a.X <= 0) {
					return true;
				}
			}
			return false;
		}
		private bool CollisionUp()
		{
			Point a = new Point((int)(Position.X - (origin.X * scale.X)*0.8), (int)(Position.Y - origin.Y * scale.Y));
			Point b = new Point((int)(Position.X + (origin.X * scale.X)*0.8), (int)(Position.Y - origin.Y * scale.Y));
			for (int i = 0; i < TileMap.LstCollision.Count; i++) {
				if (TileMap.LstCollision[i].Contains(a) || TileMap.LstCollision[i].Contains(b)
				   || a.Y <= 0) {
					return true;
				}
			}
			return false;
		}
		private bool CollisionDown()
		{
			Point a = new Point((int)(Position.X - (origin.X*scale.X)*0.8), (int)(Position.Y + origin.Y*scale.Y));
			Point b = new Point((int)(Position.X + (origin.X*scale.X)*0.8), (int)(Position.Y + origin.Y*scale.Y));
			for (int i = 0; i < TileMap.LstCollision.Count; i++) {
				if (TileMap.LstCollision[i].Contains(a) || TileMap.LstCollision[i].Contains(b)
				    || a.Y >= TileMap.MapHeight * TileMap.TileHeight) {
					return true;
				}
			}
			return false;
		}
	}
}
