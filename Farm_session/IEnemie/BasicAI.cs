using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class BasicAI
	{
		Enemie enemie;
		Hero player;
		Point enemiePoint;
		Point d;

		public bool CanSeePlayer = true;
		Primitive primitive;

		public BasicAI(Enemie pEnemie, Hero pPlayer)
		{
			enemie = pEnemie;
			player = pPlayer;
			primitive = new Primitive();
		}

		public void Update(GameTime gameTime)
		{
			enemiePoint = new Point((int)enemie.Position.X, (int)enemie.Position.Y);
			d = new Point((int)player.Position.X, (int)player.Position.Y);
			CanSeePlayer = true;

			List<Point> c = BresenhamLine(enemiePoint.X, enemiePoint.Y, 
			                              (int)player.Position.X, (int)player.Position.Y);

			if (c.Count > 0) {
				int rayPointIndex = 0;

				if (c[0] != enemiePoint) rayPointIndex = c.Count - 1;

				// Loop through all the points starting from "position"
				while (true) {
					Point rayPoint = c[rayPointIndex];
					if (TileMap.BitMap[TileMap.GetTileAt((int)rayPoint.X, (int)rayPoint.Y, 2)] == 1) {
						CanSeePlayer = false;
						d = rayPoint;
						break;
					}
					if (c[0] != enemiePoint) {
						rayPointIndex--;
						if (rayPointIndex < 0) break;
					} else {
						rayPointIndex++;
						if (rayPointIndex >= c.Count) break;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			primitive.DrawLine(enemiePoint.X, enemiePoint.Y, d.X, d.Y, Color.White, spriteBatch);
		}

		// Swap the values of A and B
		private void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}

		// Returns the list of points from (x0, y0) to (x1, y1)
		private List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
		{
			// Optimization: it would be preferable to calculate in
			// advance the size of "result" and to use a fixed-size array
			// instead of a list.
			List<Point> result = new List<Point>();

			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep) {
				Swap(ref x0, ref y0);
				Swap(ref x1, ref y1);
			}
			if (x0 > x1) {
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);
			}

			int deltax = x1 - x0;
			int deltay = Math.Abs(y1 - y0);
			int error = 0;
			int ystep;
			int y = y0;
			if (y0 < y1) ystep = 1; else ystep = -1;
			for (int x = x0; x <= x1; x++) {
				if (steep) result.Add(new Point(y, x));
				else result.Add(new Point(x, y));
				error += deltay;
				if (2 * error >= deltax) {
					y += ystep;
					error -= deltax;
				}
			}

			return result;
		}
	}
}
