using System;
namespace Farming_session
{
	public abstract class Overlay
	{
		public bool isActive { get; set; }
		protected MainGame mainGame;

		public Overlay(MainGame pGame)
		{
			mainGame = pGame;
		}
	}
}
