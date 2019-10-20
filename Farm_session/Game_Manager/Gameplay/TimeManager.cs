using System;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class TimeManager
	{
		float currentTime;
		public int CurrentHour { get; private set; }
		public float BackgroundValue { get; private set; }
		int HourValue;
		public bool BackgroundFreeze;

		public int CurrentDay { get; private set; }
		private bool DayChanged;
		

		public TimeManager(int pHourValue)
		{
			HourValue = pHourValue;
			CurrentHour = 13;
			BackgroundValue = 1f;
			BackgroundFreeze = false;

			CurrentDay = 0;
			DayChanged = false;
		}

		public void SetHour(int pHour)
		{
			CurrentHour = pHour;
			if (CurrentHour >= 20 && CurrentHour < 23) {
				BackgroundValue = 1f - ((CurrentHour - 23 + 3) * 0.26f);
			} else if (CurrentHour >= 6 && CurrentHour < 9) {
				BackgroundValue = 0.2f + ((CurrentHour - 9 + 3) * 0.26f);
				Console.WriteLine(BackgroundValue);
			}
		}

		public void SetDay(int pDay)
		{
			CurrentDay = pDay;
			SetHour(8);
		}

		public void Update(GameTime gameTime)
		{
			currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (currentTime >= HourValue) {
				currentTime = 0;
				CurrentHour++;
				if (CurrentHour >= 24) {
					CurrentHour = 0;
				}
			}

			if (CurrentHour == 0 && !DayChanged) {
				CurrentDay++;
				DayChanged = true;
			} else {
				DayChanged = false;
			}

			if (!BackgroundFreeze) {
				if (CurrentHour >= 20 && CurrentHour < 23) {
					if (BackgroundValue >= 0.2f) {
						BackgroundValue -= (0.8f / (3 * HourValue)) / 60;
					}
				} else if (CurrentHour >= 6 && CurrentHour < 9) {
					if (BackgroundValue <= 1f) {
						BackgroundValue += (0.8f / (3 * HourValue)) / 60;
					}
				}

				if (CurrentHour >= 23 || CurrentHour < 6) {
					BackgroundValue = 0.2f;
				} else if (CurrentHour >= 9 && CurrentHour < 20) {
					BackgroundValue = 1f;
				}
			}
		}
	}
}
