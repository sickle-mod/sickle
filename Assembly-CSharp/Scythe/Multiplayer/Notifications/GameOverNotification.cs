using System;
using AsmodeeNet.Foundation;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x0200029F RID: 671
	public class GameOverNotification : Notification
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x000365BF File Offset: 0x000347BF
		// (set) Token: 0x06001534 RID: 5428 RVA: 0x000365C7 File Offset: 0x000347C7
		public string GameId { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x000365D0 File Offset: 0x000347D0
		// (set) Token: 0x06001536 RID: 5430 RVA: 0x000365D8 File Offset: 0x000347D8
		public string GameName { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x000365E1 File Offset: 0x000347E1
		// (set) Token: 0x06001538 RID: 5432 RVA: 0x000365E9 File Offset: 0x000347E9
		public int PlayerPlace { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x000365F2 File Offset: 0x000347F2
		// (set) Token: 0x0600153A RID: 5434 RVA: 0x000365FA File Offset: 0x000347FA
		public int EloGained { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x00036603 File Offset: 0x00034803
		// (set) Token: 0x0600153C RID: 5436 RVA: 0x0003660B File Offset: 0x0003480B
		public int KarmaGained { get; private set; }

		// Token: 0x0600153D RID: 5437 RVA: 0x00036614 File Offset: 0x00034814
		public GameOverNotification()
			: base("GAME_OVER", null)
		{
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00036622 File Offset: 0x00034822
		public override string ToString()
		{
			return string.Format("Game {0} has ended.", this.GameName);
		}

		// Token: 0x04000F86 RID: 3974
		public const string Identifier = "GAME_OVER";
	}
}
