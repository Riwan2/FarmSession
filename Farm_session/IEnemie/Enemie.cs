using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Farming_session
{
	public class Enemie
	{
		//Enemie
		public Rectangle BoundingBox { get; protected set; }
		public Vector2 Position { get; set; }
		public Vector2 Direction { get; protected set; }
		public bool IsInvisible { get; set; } = false;
		public bool Delete { get; set; } = false;

		public int Life { get; set; }
		public AnimatedSprite CurrentSprite { get; protected set; }
		protected Dictionary<string, AnimatedSprite> AnimationList;

		public List<Texture2D> lstTextures;
		protected Hero Player;

		public string LastDamageTaken { get; private set; }
		public bool GotHurt { get; set; } = false;

		public enum eState
		{
			idle,
			walkTo,
			attack,
			comeBack,
		}

		public eState CurrentState { get; protected set; }

		public Enemie()
		{
		}

		public Enemie(Enemie pCopy, Hero pPlayer)
		{
			//AnimationList = pCopy.AnimationList;
			Life = pCopy.Life;
			Player = pPlayer;
		}

		public void GetDamage(int pDamage)
		{
			Life -= pDamage;
			LastDamageTaken = pDamage.ToString();
			GotHurt = true;
		}

		public void Aggro()
		{
			CurrentState = eState.walkTo;
		}

		public void SetLayer(float layer)
		{
			CurrentSprite.Layer = layer;
		}

		public virtual void Update(GameTime gameTime)
		{
			CurrentSprite.SetBoundingBox();
			BoundingBox = CurrentSprite.BoundingBox;
			CurrentSprite.Origin = new Vector2(CurrentSprite.width / 2, CurrentSprite.height / 2);

			if (Life <= 0) {
				Console.WriteLine("Mob die");
				Delete = true;
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			CurrentSprite.Position = Position;
			CurrentSprite.Draw(spriteBatch);
		}
	}

	public static class EnemieData
	{
		public static Dictionary<string, Enemie> Data;

		public static void PopulateData(ContentManager content)
		{
			Data = new Dictionary<string, Enemie>();

			Data.Add("Champiglu", new Enemie() {
				lstTextures = new List<Texture2D>() {
					{content.Load<Texture2D>("Monster/MushroomWalk") },
					{content.Load<Texture2D>("Monster/MushroomAttack") },
				}, 
				Life = 100,
			});
		}
	}
}
