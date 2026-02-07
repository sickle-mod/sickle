using System;
using AsmodeeNet.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x020002A8 RID: 680
	public class YourTurnNotification : Notification
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00036959 File Offset: 0x00034B59
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x00036961 File Offset: 0x00034B61
		public int Turn { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0003696A File Offset: 0x00034B6A
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x00036972 File Offset: 0x00034B72
		public string GameName { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x0003697B File Offset: 0x00034B7B
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x00036983 File Offset: 0x00034B83
		public string GameId { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x0003698C File Offset: 0x00034B8C
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x00036994 File Offset: 0x00034B94
		[JsonConverter(typeof(StringEnumConverter))]
		public Faction Faction { get; private set; }

		// Token: 0x0600158C RID: 5516 RVA: 0x0003699D File Offset: 0x00034B9D
		public YourTurnNotification()
			: base("YOUR_TURN", null)
		{
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000369AB File Offset: 0x00034BAB
		public override string ToString()
		{
			return string.Format("It is your turn ({0}) in game {1}. Game Id {2}. Faction {3}", new object[] { this.Turn, this.GameName, this.GameId, this.Faction });
		}

		// Token: 0x04000FBD RID: 4029
		public const string Identifier = "YOUR_TURN";
	}
}
