using System;
using System.Timers;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer
{
	// Token: 0x0200028F RID: 655
	public static class PlayerClock
	{
		// Token: 0x060014BA RID: 5306 RVA: 0x00036025 File Offset: 0x00034225
		public static void InitTimer()
		{
			PlayerClock.playerClockTimer = new Timer(1000.0);
			PlayerClock.playerClockTimer.Elapsed += PlayerClock.DecreaseTimeLeft;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00036050 File Offset: 0x00034250
		public static void RemoveTimer()
		{
			PlayerClock.playerClockTimer.Elapsed -= PlayerClock.DecreaseTimeLeft;
			PlayerClock.playerClockTimer.Dispose();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00036072 File Offset: 0x00034272
		public static void StartTimer()
		{
			PlayerClock.playerClockTimer.Start();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0003607E File Offset: 0x0003427E
		public static void StopTimer()
		{
			PlayerClock.playerClockTimer.Stop();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0009BAA0 File Offset: 0x00099CA0
		public static void DecreaseTimeLeft(object sender, ElapsedEventArgs e)
		{
			if (MultiplayerController.Instance.GetActivePlayer.PlayerClock > 0)
			{
				PlayerData getActivePlayer = MultiplayerController.Instance.GetActivePlayer;
				int num = getActivePlayer.PlayerClock - 1;
				getActivePlayer.PlayerClock = num;
				if (num == 0)
				{
					MultiplayerController.Instance.UpdatePlayerClockOnServer();
				}
			}
			if (MultiplayerController.Instance.GetSecondActivePlayer != null && MultiplayerController.Instance.GetSecondActivePlayer.PlayerClock > 0)
			{
				PlayerData getSecondActivePlayer = MultiplayerController.Instance.GetSecondActivePlayer;
				int num = getSecondActivePlayer.PlayerClock - 1;
				getSecondActivePlayer.PlayerClock = num;
				if (num == 0)
				{
					MultiplayerController.Instance.UpdatePlayerClockOnServer();
				}
			}
			if (MultiplayerController.Instance.SpectatorMode)
			{
				return;
			}
			if (MultiplayerController.Instance.GetOwnerPlayer.PlayerClock <= 0)
			{
				PlayerClock.StopTimer();
				ConnectionProblem.ShowOutOfTimePanel();
			}
		}

		// Token: 0x04000F1F RID: 3871
		private static Timer playerClockTimer;
	}
}
