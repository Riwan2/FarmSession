using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class PriorityQueue<T>
	{
		private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

		public int Count {
			get { return elements.Count; }
		}

		public void Enqueue(T item, int priority)
		{
			elements.Add(Tuple.Create(item, priority));
		}

		public T Dequeue()
		{
			int bestIndex = 0;

			for (int i = 0; i < elements.Count; i++) {
				if (elements[i].Item2 < elements[bestIndex].Item2) {
					bestIndex = i;
				}
			}

			T bestItem = elements[bestIndex].Item1;
			elements.RemoveAt(bestIndex);
			return bestItem;
		}
	}

	public class PathFinding
	{
		Primitive primitive;
		MainGame mainGame;

		int GridWidth = 30;
		int GridHeight = 15;

		int CellWidth = 32;
		int CellHeight = 32;
		int Space = 1;
		int offsetX = 80;
		int offsetY = 100;

		Point Start = new Point(4, 10);
		Point End = new Point(20, 7);

		Point nullPoint = new Point(-1, -1);

		KeyboardState newKeyState;
		KeyboardState oldKeyState;
		MouseState newMouseState;
		MouseState oldMouseState;

		bool WallBuild = false;
		bool[,] mapWall;
		bool[,] mapVisited;

		Dictionary<Point, Point> cameFrom;
		Dictionary<Point, int> costSoFar;

		List<Point> path;
		bool pathFinished = false;

		float timer;
		float UpdateTime = 0.001f;
		bool update = false;
		bool pause = false;

		bool endAlgo = false;

		public PathFinding(MainGame pMainGame)
		{
			mainGame = pMainGame;
			primitive = new Primitive(mainGame.spriteBatch);
			Load();
		}

		private void SetMap()
		{
			if (!WallBuild) mapWall = new bool[GridHeight,GridWidth];
			mapVisited = new bool[GridHeight, GridWidth];

			for (int y = 0; y < GridHeight; y++) {
				for (int x = 0; x < GridWidth; x++) {
					if (!WallBuild) mapWall[y,x] = false;
					mapVisited[y, x] = false;
				}
			}

			WallBuild = true;
			mapVisited[Start.Y, Start.X] = true;
		}

		PriorityQueue<Point> frontier;
		private void Load()
		{
			oldKeyState = Keyboard.GetState();
			oldMouseState = Mouse.GetState();

			costSoFar = new Dictionary<Point, int>();
			costSoFar[Start] = 0;
			cameFrom = new Dictionary<Point, Point>();
			cameFrom[Start] = Point.Zero;

			endAlgo = false;

			pathFinished = false;

			frontier = new PriorityQueue<Point>();
			frontier.Enqueue(Start, 0);

			SetMap();
		}



		public void Update(GameTime gameTime)
		{
			newKeyState = Keyboard.GetState();
			newMouseState = Mouse.GetState();

			MoveStartPoint(newKeyState, oldKeyState);
			Restart(newKeyState, oldKeyState);
			Pause(newKeyState, oldKeyState);
			BuildWall();
			UpdateTimer(gameTime);

			if (update && !endAlgo) {

				while (frontier.Count > 0) {
					var current = frontier.Dequeue();

					if (current == End) {
						endAlgo = true;
						break;
					}

					foreach (Point next in getNeighbors(current)) {
						if (next != nullPoint && !mapVisited[next.Y, next.X]) {
							int newCost = costSoFar[current] + 2;
							if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
								costSoFar[next] = newCost;
								mapVisited[next.Y, next.X] = true;
								int priority = newCost + util.getHeuristic(End, next);
								frontier.Enqueue(next, priority);
								cameFrom[next] = current;
							}
						}
					}
				}
			}

			if (endAlgo && !pathFinished) {
				GetPath();
			}

			oldKeyState = Keyboard.GetState();
			oldMouseState = Mouse.GetState();
		}

		private void GetPath()
		{
			Point current = End;
			path = new List<Point>();
			while (current != Start) {
				path.Add(current);
				current = cameFrom[current];
			}

			path.Add(Start);
			pathFinished = true;
		}

		private List<Point> getNeighbors(Point pos)
		{
			List<Point> lstPoint = new List<Point>();

			int rand = util.getInt(1, 10);

			if (rand != 1) {
				lstPoint.Add(NextDown(pos));
			}
			if (rand != 2) {
				lstPoint.Add(NextLeft(pos));
			}
			if (rand != 3) {
				lstPoint.Add(NextUp(pos));
			}
			if (rand != 4) {
				lstPoint.Add(NextRight(pos));
			}

			return lstPoint;
		}

		private Point NextRight(Point pos)
		{
			Point point = new Point(pos.X + 1, pos.Y);
			if (pos.X >= GridWidth - 1) return nullPoint;
			if (mapWall[point.Y, point.X]) return nullPoint;
			return point;
		}
		private Point NextLeft(Point pos)
		{
			Point point = new Point(pos.X - 1, pos.Y);
			if (pos.X <= 0) return nullPoint;
			if (mapWall[point.Y, point.X]) return nullPoint;
			return point;
		}
		private Point NextUp(Point pos)
		{
			Point point = new Point(pos.X, pos.Y - 1);
			if (pos.Y <= 0) return nullPoint;
			if (mapWall[point.Y, point.X]) return nullPoint;
			return point;
		}
		private Point NextDown(Point pos)
		{
			Point point = new Point(pos.X, pos.Y + 1);
			if (pos.Y >= GridHeight - 1) return nullPoint;
			if (mapWall[point.Y, point.X]) return nullPoint;
			return point;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//Base Grid draw;
			for (int y = 0; y < GridHeight; y++) {
				for (int x = 0; x < GridWidth; x++) {
					Point pos = new Point(offsetX + x * (CellWidth + Space), offsetY + y * (CellHeight + Space));
					if (mapWall[y, x] == true) {
						primitive.DrawRectangle(pos.X, pos.Y, CellWidth, CellHeight, Color.SlateGray);
					} else if (mapVisited[y, x] == true) {
						primitive.DrawRectangle(pos.X, pos.Y, CellWidth, CellHeight, Color.Coral);
					} else {
						primitive.DrawRectangle(pos.X, pos.Y, CellWidth, CellHeight, Color.LightGray);
					}
				}
			}
			//Path
			if (pathFinished) {
				foreach (Point pos in path) {
					Point newPos = new Point(offsetX + pos.X * (CellWidth + Space), offsetY + pos.Y * (CellHeight + Space));
					primitive.DrawRectangle(newPos.X, newPos.Y, CellWidth, CellHeight, Color.PaleGoldenrod);
				}
			}

			//Start and End
			primitive.DrawRectangle(offsetX + Start.X * (CellWidth + Space),
			                        offsetY + Start.Y * (CellHeight + Space),
			                        CellWidth, CellHeight, Color.SteelBlue);
			primitive.DrawRectangle(offsetX + End.X * (CellWidth + Space),
									offsetY + End.Y * (CellHeight + Space),
			                        CellWidth, CellHeight, Color.IndianRed);
		}

		private void MoveStartPoint(KeyboardState pNewKeyState, KeyboardState pOldKeyState)
		{
			if (pNewKeyState.IsKeyDown(Keys.Up) && pOldKeyState.IsKeyUp(Keys.Up)) {
				if (Start.Y > 0) Start = new Point(Start.X, Start.Y - 1);
				Init();
			} else if (pNewKeyState.IsKeyDown(Keys.Right) && pOldKeyState.IsKeyUp(Keys.Right)) {
				if (Start.X < GridWidth - 1) Start = new Point(Start.X + 1, Start.Y);
				Init();
			} else if (pNewKeyState.IsKeyDown(Keys.Down) && pOldKeyState.IsKeyUp(Keys.Down)) {
				if (Start.Y < GridHeight - 1) Start = new Point(Start.X, Start.Y + 1);
				Init();
			} else if (pNewKeyState.IsKeyDown(Keys.Left) && pOldKeyState.IsKeyUp(Keys.Left)) {
				if (Start.X > 0) Start = new Point(Start.X - 1, Start.Y);
				Init();
			}
		}

		private void Init()
		{
			update = false;
			timer = 0;
			pause = false;
			Load();
		}

		private void Restart(KeyboardState pNewKeyState, KeyboardState pOldKeyState)
		{
			if (pNewKeyState.IsKeyDown(Keys.Enter) && pOldKeyState.IsKeyUp(Keys.Enter)) {
				Init();
			}
		}

		private void Pause(KeyboardState pNewKeyState, KeyboardState pOldKeyState)
		{
			if (pNewKeyState.IsKeyDown(Keys.Space) && pOldKeyState.IsKeyUp(Keys.Space)) {
				pause = !pause;
			}

		}

		private void BuildWall()
		{
			if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				Point pos = Mouse.GetState().Position;
				pos = new Point((int)((pos.X - offsetX) / (CellWidth + Space)),
								(int)((pos.Y - offsetY) / (CellWidth + Space)));
				mapWall[pos.Y, pos.X] = !mapWall[pos.Y, pos.X];
			}
		}

		private void UpdateTimer(GameTime gameTime)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer > UpdateTime && !pause) {
				timer = 0;
				update = true;
			} else {
				update = false;
			}
		}
	}
}
