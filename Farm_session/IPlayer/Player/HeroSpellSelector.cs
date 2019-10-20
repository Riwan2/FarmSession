using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farming_session
{
	public class HeroSpellSelector
	{
		public enum eSpell
		{
			WaterCrop,
			BasicAttack
		}

		private Hero player;
		public static eSpell CurrentSpell { get; private set; }
		public List<eSpell> SpellList { get; private set; }

		private string spellString;
		private Vector2 spellStringPosition;

		public HeroSpellSelector(Hero pPlayer)
		{
			player = pPlayer;
			spellStringPosition = new Vector2(MainGame.ScreenWidth - 200, 100);

			SpellList = new List<eSpell>();
			SpellList.Add(eSpell.WaterCrop);
			SpellList.Add(eSpell.BasicAttack);

			CurrentSpell = SpellList[0];
			spellString = "WaterCropSpell";
		}

		public void Update(GameTime gameTime, KeyboardState newKeyboardState, KeyboardState oldKeyboardState)
		{
			if (newKeyboardState.IsKeyDown(Keys.F1) && oldKeyboardState.IsKeyUp(Keys.F1)) {
				CurrentSpell = SpellList[0];
				spellString = "WaterCropSpell";
				Console.WriteLine(SpellList[0]);
			} else if (newKeyboardState.IsKeyDown(Keys.F2) && oldKeyboardState.IsKeyUp(Keys.F2)) {
				CurrentSpell = SpellList[1];
				spellString = "BasicAttackSpell";
				Console.WriteLine(SpellList[1]);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(AssetManager.MainFont, spellString, spellStringPosition, Color.White);
		}
	}
}
