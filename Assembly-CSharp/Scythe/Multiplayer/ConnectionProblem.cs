using System;
using System.Timers;
using Scythe.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000217 RID: 535
	public class ConnectionProblem
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06000FCC RID: 4044 RVA: 0x0008E824 File Offset: 0x0008CA24
		// (remove) Token: 0x06000FCD RID: 4045 RVA: 0x0008E858 File Offset: 0x0008CA58
		public static event ConnectionProblem.OnConnectionProblem ConnectionProblemOccured;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06000FCE RID: 4046 RVA: 0x0008E88C File Offset: 0x0008CA8C
		// (remove) Token: 0x06000FCF RID: 4047 RVA: 0x0008E8C0 File Offset: 0x0008CAC0
		public static event ConnectionProblem.OnDisconnected Disconnected;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06000FD0 RID: 4048 RVA: 0x0008E8F4 File Offset: 0x0008CAF4
		// (remove) Token: 0x06000FD1 RID: 4049 RVA: 0x0008E928 File Offset: 0x0008CB28
		public static event ConnectionProblem.OnRunOutOfTime OutOfTime;

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x00032374 File Offset: 0x00030574
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0003237B File Offset: 0x0003057B
		public static bool ErrorShown { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00032383 File Offset: 0x00030583
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0003238A File Offset: 0x0003058A
		public static int TimeLeft { get; private set; }

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0008E95C File Offset: 0x0008CB5C
		public static void Init()
		{
			ConnectionProblem.ErrorShown = false;
			if (ConnectionProblem.connectionTimer != null)
			{
				ConnectionProblem.connectionTimer.Stop();
			}
			else
			{
				ConnectionProblem.connectionTimer = new Timer(1000.0);
				ConnectionProblem.connectionTimer.Elapsed += ConnectionProblem.DecreaseTimeToConnect;
				ConnectionProblem.connectionTimer.AutoReset = false;
			}
			ConnectionProblem.TimeLeft = 45;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00032392 File Offset: 0x00030592
		public static void ShowConnectionProblemPanel()
		{
			ConnectionProblem.currentError = "GameScene/ConnectionProblemInfo";
			ConnectionProblem.currentErrorTitle = "GameScene/ConnectionProblem";
			ConnectionProblem.ShowErrorPanel();
			if (ConnectionProblem.connectionTimer != null)
			{
				ConnectionProblem.TimeLeft = 45;
				ConnectionProblem.connectionTimer.Start();
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000323C5 File Offset: 0x000305C5
		public static void ShowDisconnectedPanel()
		{
			ConnectionProblem.currentError = "GameScene/ConnectionProblem";
			ConnectionProblem.currentErrorTitle = "GameScene/ConnectionProblem";
			ConnectionProblem.ShowErrorPanel();
			ConnectionProblem.Disconnect();
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0008E9C0 File Offset: 0x0008CBC0
		public static void ShowOutOfTimePanel()
		{
			ConnectionProblem.currentError = "GameScene/ConnectionProblemOutOfTime";
			ConnectionProblem.currentErrorTitle = "GameScene/ConnectionProblemOutOfTime";
			ConnectionProblem.ShowErrorPanel();
			MultiplayerController.Instance.RunOutOfTime = true;
			if (ConnectionProblem.OutOfTime != null)
			{
				ConnectionProblem.OutOfTime();
			}
			if (GameController.GameManager.GetPlayersWithoutAICount() > 1)
			{
				MultiplayerController.Instance.LeaveGame();
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x000323E5 File Offset: 0x000305E5
		private static void ShowErrorPanel()
		{
			if (ConnectionProblem.ErrorShown)
			{
				return;
			}
			ConnectionProblem.ErrorShown = true;
			if (ConnectionProblem.ConnectionProblemOccured != null)
			{
				ConnectionProblem.ConnectionProblemOccured();
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0008EA1C File Offset: 0x0008CC1C
		public static void HideErrorPanel()
		{
			if (MultiplayerController.Instance.Disconnected || MultiplayerController.Instance.RunOutOfTime)
			{
				return;
			}
			if (!ConnectionProblem.ErrorShown)
			{
				return;
			}
			ConnectionProblem.ErrorShown = false;
			if (ConnectionProblem.ConnectionProblemOccured != null)
			{
				ConnectionProblem.ConnectionProblemOccured();
			}
			if (ConnectionProblem.connectionTimer != null)
			{
				ConnectionProblem.connectionTimer.Stop();
			}
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00032406 File Offset: 0x00030606
		private static void DecreaseTimeToConnect(object sender, ElapsedEventArgs e)
		{
			ConnectionProblem.TimeLeft--;
			if (ConnectionProblem.TimeLeft <= 0)
			{
				ConnectionProblem.Disconnect();
				return;
			}
			ConnectionProblem.connectionTimer.Enabled = true;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0003242D File Offset: 0x0003062D
		private static void Disconnect()
		{
			if (ConnectionProblem.connectionTimer != null)
			{
				ConnectionProblem.connectionTimer.Stop();
			}
			MultiplayerController.Instance.Disconnected = true;
			MultiplayerController.Instance.LeaveGame();
			if (ConnectionProblem.Disconnected != null)
			{
				ConnectionProblem.Disconnected();
			}
		}

		// Token: 0x04000C38 RID: 3128
		public const string NO_INTERNET_CONNECTION = "GameScene/ConnectionProblemInfo";

		// Token: 0x04000C39 RID: 3129
		public const string DISCONNECTED = "GameScene/ConnectionProblem";

		// Token: 0x04000C3A RID: 3130
		public const string OUT_OF_TIME = "GameScene/ConnectionProblemOutOfTime";

		// Token: 0x04000C3B RID: 3131
		private const int TIME_TO_RECONNECT = 45;

		// Token: 0x04000C40 RID: 3136
		public static string currentError = "GameScene/ConnectionProblemInfo";

		// Token: 0x04000C41 RID: 3137
		public static string currentErrorTitle = "GameScene/ConnectionProblem";

		// Token: 0x04000C42 RID: 3138
		private static Timer connectionTimer;

		// Token: 0x02000218 RID: 536
		// (Invoke) Token: 0x06000FE1 RID: 4065
		public delegate void OnConnectionProblem();

		// Token: 0x02000219 RID: 537
		// (Invoke) Token: 0x06000FE5 RID: 4069
		public delegate void OnDisconnected();

		// Token: 0x0200021A RID: 538
		// (Invoke) Token: 0x06000FE9 RID: 4073
		public delegate void OnRunOutOfTime();
	}
}
