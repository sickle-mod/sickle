using System;
using AsmodeeNet.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x0200029E RID: 670
	public class CombatNotification : Notification
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x0003654B File Offset: 0x0003474B
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x00036553 File Offset: 0x00034753
		public string GameId { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x0003655C File Offset: 0x0003475C
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x00036564 File Offset: 0x00034764
		public string GameName { get; private set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0003656D File Offset: 0x0003476D
		// (set) Token: 0x0600152B RID: 5419 RVA: 0x00036575 File Offset: 0x00034775
		[JsonConverter(typeof(StringEnumConverter))]
		public Faction Faction { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0003657E File Offset: 0x0003477E
		// (set) Token: 0x0600152D RID: 5421 RVA: 0x00036586 File Offset: 0x00034786
		[JsonConverter(typeof(StringEnumConverter))]
		public CombatStage CombatStage { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0003658F File Offset: 0x0003478F
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x00036597 File Offset: 0x00034797
		[JsonConverter(typeof(StringEnumConverter))]
		public Faction? AttackerFaction { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x000365A0 File Offset: 0x000347A0
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x000365A8 File Offset: 0x000347A8
		[JsonConverter(typeof(StringEnumConverter))]
		public Faction? DefenderFaction { get; private set; }

		// Token: 0x06001532 RID: 5426 RVA: 0x000365B1 File Offset: 0x000347B1
		public CombatNotification()
			: base("COMBAT", null)
		{
		}

		// Token: 0x04000F7F RID: 3967
		public const string Identifier = "COMBAT";
	}
}
