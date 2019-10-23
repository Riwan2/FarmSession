using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;
using TiledSharp;

namespace Farming_session
{
	public class TileMap
	{
		private MainGame mainGame;
		private Texture2D tileset;

		public static int TileWidth { get; private set; }
		public static int TileHeight { get; private set; }
		public static int MapWidth { get; private set; }
		public static int MapHeight { get; private set; }

		public int TilesetLines { get; private set; }
		public int TilesetColumns { get; private set; }
		public int tilesetTileWidth { get; private set; }
		public int tilesetTileHeight { get; private set; }

		private TmxMap map;

		public List<int> TileObjectIndex;
		private List<AnimatedTile> LstAnimatedTile; //Animated Tile
		public static List<List<Tile>> lstLayer;

		public static string CurrentMap;

		//Collision
		public static List<Rectangle> LstCollision;
		public static int[] BitMap;

		//PathFinding
		public static Grid CurrentGrid;

		public TileMap(MainGame pMainGame, string level)
		{
			mainGame = pMainGame;
			map = new TmxMap("Content/level/" + level + ".tmx");
			tileset = mainGame.Content.Load<Texture2D>("tileset");
			CurrentMap = level;

			Load();
		}

		private void Load()
		{
			//tileset
			tilesetTileWidth = map.Tilesets[0].TileWidth;
			tilesetTileHeight = map.Tilesets[0].TileHeight;

			TilesetColumns = tileset.Width / tilesetTileWidth;
			TilesetLines = tileset.Height / tilesetTileHeight;

			lstLayer = new List<List<Tile>>();
			LstCollision = new List<Rectangle>();

			for (int i = 0; i < map.Layers.Count; i++) {
				lstLayer.Add(new List<Tile>());
			}

			LstAnimatedTile = new List<AnimatedTile>();
		}

		public void LoadMap()
		{
			//Map
			TileWidth = (map.Tilesets[0].TileWidth) * SceneGameplay.GameplayScale;
			TileHeight = (map.Tilesets[0].TileHeight) * SceneGameplay.GameplayScale;

			MapWidth = map.Width;
			MapHeight = map.Height;

			TileObjectIndex = new List<int>();

			BitMap = new int[map.Layers[0].Tiles.Count];

			int nbLayers = map.Layers.Count;

			for (int nLayer = 0; nLayer < nbLayers; nLayer++) {
				int line = 0;
				int column = 0;

				for (int i = 0; i < map.Layers[nLayer].Tiles.Count; i++) {
					int gid = map.Layers[nLayer].Tiles[i].Gid;

					lstLayer[nLayer].Add(new Tile(gid));

					if (gid == 21 || gid == 22 || gid == 23 || gid == 25) {
						TileObjectIndex.Add(i);
					}

					if (gid == 13) {
						//AnimatedTile animatedTile = new AnimatedTile(i, new List<int>() { 3, 1, 2 }, 1000);
						//LstAnimatedTile.Add(animatedTile);
						//lstLayer[nLayer][i] = animatedTile.gid[0];
					}

					//Collision
					if (gid == 26) {
						Rectangle a = new Rectangle(column * TileWidth, line * TileHeight, TileWidth, TileHeight);
						LstCollision.Add(a);
						Console.WriteLine(nLayer);
					}

					if (nLayer == 2) {
						BitMap[i] = 0;
						if (gid == 26) {
							BitMap[i] = 1;
						}
					}

					Console.WriteLine(BitMap[i]);

					column++;
					if (column == MapWidth) {
						column = 0;
						line++;
					}
				}
			}
		}

		public static Vector2 GetTilePosAt(int posX, int posY, int layer)
		{
			if (posX < 0 || posX > TileMap.MapWidth * TileMap.TileWidth) {
				return Vector2.Zero;
			} else {
				posX = Math.Abs(posX / TileMap.TileWidth); //position tile a partir de vrai position X
			}

			if (posY < 0 || posY > TileMap.MapHeight * TileMap.TileHeight) {
				return Vector2.Zero;
			} else {
				posY = Math.Abs(posY / TileMap.TileHeight) + 1; //position tile a partir de vrai position Y
			}

			return new Vector2(posX, posY);
		}

		public static int GetTileAt(int posX, int posY, int layer)
		{
			Vector2 Pos = GetTilePosAt(posX, posY, layer);
			int newPosX = (int)Pos.X;
			int newPosY = (int)Pos.Y;

			int id = 0;
			if (newPosX <= TileMap.MapWidth && newPosY <= TileMap.MapHeight) {
				id = lstLayer[layer][TileMap.MapWidth * newPosY - (TileMap.MapWidth - newPosX)].Gid;
			}
			return id;
		}

		public void Update(GameTime gameTime)
		{

			for (int i = 0; i < LstAnimatedTile.Count; i++) {
				LstAnimatedTile[i].Update(gameTime);
				if (LstAnimatedTile[i].AnimationChange) {
					int index = LstAnimatedTile[i].index;
					lstLayer[0][index].Gid = LstAnimatedTile[i].CurrentGid;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{

			int nbLayers = map.Layers.Count;

			int line;
			int column;

			for (int nLayer = 0; nLayer < nbLayers; nLayer++) {
				line = 0;
				column = 0;

				for (int i = 0; i < lstLayer[nLayer].Count; i++) {
					int gid = 0;
					gid = lstLayer[nLayer][i].Gid;

					if (gid != 0 && gid != 26) {
						int tileFrame = gid - 1;
						int tilesetColumn = tileFrame % TilesetColumns;
						int tilesetLine = (int)Math.Floor((double)tileFrame / (double)TilesetColumns);

						float x = column * TileWidth;
						float y = line * TileHeight;

						Rectangle tilesetRec = new Rectangle(tilesetTileWidth * tilesetColumn,
															 tilesetTileHeight * tilesetLine,
															 tilesetTileWidth, tilesetTileHeight);
						
						spriteBatch.Draw(tileset, new Vector2(x, y), tilesetRec, lstLayer[nLayer][i].Color, 0f,
										 Vector2.Zero, SceneGameplay.GameplayScale, SpriteEffects.None, 0);
						
					}
					column++;
					if (column == MapWidth) {
						column = 0;
						line++;
					}
				}
			}
		}
	}

	public class Tile
	{
		public int Gid;
		public Color Color;

		public Tile(int pGid)
		{
			Gid = pGid;
			Color = Color.White;
		}
	}

	class AnimatedTile
	{
		public bool AnimationChange;

		public int index;
		float timer = 0;
		float updateTime;

		public List<int> gid;
		public int CurrentGid;
		private int gidIndex;

		public AnimatedTile(int pIndex, List<int> pGid, int pUpdateTime)
		{
			index = pIndex;
			gid = pGid;
			updateTime = pUpdateTime;

			CurrentGid = gid[0];
			gidIndex = 0;
		}

		public void Update(GameTime gameTime)
		{
			AnimationChange = false;
			timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timer >= updateTime) {
				timer = 0;
				if (gidIndex + 1 < gid.Count) {
					gidIndex++;
				} else {
					gidIndex = 0;
				}

				CurrentGid = gid[gidIndex];

				AnimationChange = true;
			}
		}
	}
}

