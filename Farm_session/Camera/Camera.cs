using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class Camera
	{
		public Vector2 Position { get; set; }
		public Rectangle Bounds { get; protected set; }
		public Matrix Transform { get; protected set; }

		public bool CameraFix = false;
		public bool CameraBlocked = false;

		public Camera(Viewport viewport, Vector2 initPos)
		{
			Bounds = viewport.Bounds;
			Position = initPos;
		}

		public void MoveCamera(Vector2 movePosition)
		{
			Vector2 newPosition = Position + movePosition;
			Position = new Vector2((int)newPosition.X, (int)newPosition.Y);
		}

		private void FollowPlayerX(Hero player)
		{
			Position = new Vector2(-player.Position.X - Bounds.Width / 2, Position.Y);
		}

		private void FollowPlayerY(Hero player)
		{
			Position = new Vector2((int)Position.X, (int)(-player.Position.Y - Bounds.Height / 2));
		}

		private void UpdateMatrix()
		{
			Transform = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) *
							  Matrix.CreateScale(1) *
							  Matrix.CreateTranslation(new Vector3(Bounds.Width, Bounds.Height, 0));
		}

		private void Fix()
		{
			Transform = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) *
							  Matrix.CreateScale(1) *
							  Matrix.CreateTranslation(new Vector3(Bounds.Width, Bounds.Height, 0));
		}

		public void UpdateCamera(Viewport bounds, Hero player)
		{
			Bounds = bounds.Bounds;
			if (!CameraFix && player.Position.Y >= bounds.Height/2 && 
			    player.Position.Y <= (TileMap.MapHeight*TileMap.TileHeight) - bounds.Height/2) 
			{
				FollowPlayerY(player);
			}
			if (!CameraFix && player.Position.X >= bounds.Width / 2 &&
					   player.Position.X <= (TileMap.MapWidth * TileMap.TileWidth) - bounds.Width / 2) {
				FollowPlayerX(player);
			}

			if (!CameraFix && 
			    (player.Position.Y >= bounds.Height / 2 &&
			     player.Position.Y <= (TileMap.MapHeight * TileMap.TileHeight)- bounds.Height / 2) || 
			    (player.Position.X >= bounds.Width / 2 &&
			     player.Position.X <= (TileMap.MapWidth * TileMap.TileWidth)- bounds.Width / 2)) {

				CameraBlocked = false;
			} else {
				CameraBlocked = true;
				Fix();
			}

			UpdateMatrix();
		}
	}
}
