using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;

namespace Farming_session
{
	public class Champiglu : Enemie
	{
		//Movement
		public float Speed { get; private set; } = 1f;
		public float SprintSpeed { get; private set; } = 1.2f;
		public float CurrentSpeed { get; private set; }
		private bool inMovement;
		public int count;
		//Base
		public Vector2 spawnPosition;
		public int RadiusVisionLength { get; private set; } = 150;
		//Attack
		private float attackTimer = 0f;
		private float attackTime = 0.8f;
		//CooldownAttack
		private bool attackReady = true;
		private float attackCooldownTimer = 0f;
		private float attackCooldown = 1f;

		//Position[] Path;
		//int PathIndex = 0;

		public Champiglu(Enemie pCopy, Vector2 pPosition, Hero pPlayer) : base(pCopy, pPlayer)
		{
			Position = pPosition;

			AnimationList = new Dictionary<string, AnimatedSprite>() {
				{"walk", new AnimatedSprite(pCopy.lstTextures[0], 25, 100, 25, 25, 0.3f, 2) },
				{"attack", new AnimatedSprite(pCopy.lstTextures[1], 25, 100, 25, 25, 0.2f, 2) },
			};

			CurrentSprite = AnimationList["walk"];
			inMovement = false;

			CurrentState = eState.idle;
			CurrentSpeed = Speed;

			spawnPosition = Position;
		}

		public override void Update(GameTime gameTime)
		{
			switch (CurrentState) {
				case eState.idle:
					inMovement = false;
					if (util.getDistance(Position, Player.Position) < RadiusVisionLength) {
						CurrentState = eState.walkTo;
					}
					break;
					
				case eState.walkTo:
					inMovement = true;
					if (util.getDistance(Position, spawnPosition) > 400) {
						CurrentState = eState.comeBack;
					} else if (util.getDistance(Position, Player.Position) > 20) {
						
						Vector2 pos = TileMap.GetTilePosAt((int)Position.X, (int)Position.Y, 0);
						Vector2 playerPos = TileMap.GetTilePosAt((int)Player.Position.X, (int)Player.Position.Y, 0);

						float angle = util.getAngle(Player.Position, Position);
						Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

						if (util.getDistance(Position, Player.Position) > 200) CurrentSpeed = SprintSpeed;
						else CurrentSpeed = Speed;

						Position += Direction * CurrentSpeed;
					} else if (attackReady && CurrentSprite.count == 0) {
						CurrentSprite = AnimationList["attack"];
						CurrentSprite.count = 0;
						CurrentState = eState.attack;
					}
					break;

				case eState.attack:
					inMovement = true;
					attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
					Console.WriteLine("test");
					if (attackTimer > attackTime) {
						attackTimer = 0;
						Console.WriteLine("Attack");
						CurrentSprite = AnimationList["walk"];
						attackReady = false;
						CurrentState = eState.walkTo;
					}
					break;
					
				case eState.comeBack:
					inMovement = true;
					if (!(Position.X > spawnPosition.X - 10 && Position.X < spawnPosition.X + 10)) {
						float angle = util.getAngle(spawnPosition, Position);
						Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
						Position += Direction * Speed;
						inMovement = true;
					} else {
						CurrentSprite.count = 0;
						CurrentState = eState.idle;
					}
					break;
					
				default:
					break;
			}

			if (!attackReady) {
				attackCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds; ;
				if (attackCooldownTimer > attackCooldown) {
					attackReady = true;
					attackCooldownTimer = 0;
				}
			}

			if (inMovement) {
				CurrentSprite.Update(gameTime);
			}

			for (int i = 0; i < SceneGameplay.enemieManager.lstEnemies.Count;i++) {
				Enemie friend = SceneGameplay.enemieManager.lstEnemies[i];
				if (util.getDistance(Position, friend.Position) < RadiusVisionLength) {
					if ((friend.CurrentState == Enemie.eState.attack ||
						friend.CurrentState == Enemie.eState.walkTo) &&
					    (CurrentState == eState.idle) && friend != this) {
						Aggro();
					}
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}
	}
}
