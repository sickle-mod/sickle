using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000338 RID: 824
	public class GameHistoryEntry
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x0003819C File Offset: 0x0003639C
		// (set) Token: 0x06001799 RID: 6041 RVA: 0x000381A4 File Offset: 0x000363A4
		public string GameId { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x000381AD File Offset: 0x000363AD
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x000381B5 File Offset: 0x000363B5
		public int Faction { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x000381BE File Offset: 0x000363BE
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x000381C6 File Offset: 0x000363C6
		public int PlayerMat { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x000381CF File Offset: 0x000363CF
		// (set) Token: 0x0600179F RID: 6047 RVA: 0x000381D7 File Offset: 0x000363D7
		public int Place { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x000381E0 File Offset: 0x000363E0
		// (set) Token: 0x060017A1 RID: 6049 RVA: 0x000381E8 File Offset: 0x000363E8
		public int Points { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x000381F1 File Offset: 0x000363F1
		// (set) Token: 0x060017A3 RID: 6051 RVA: 0x000381F9 File Offset: 0x000363F9
		public bool Asynchronous { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x00038202 File Offset: 0x00036402
		// (set) Token: 0x060017A5 RID: 6053 RVA: 0x0003820A File Offset: 0x0003640A
		public bool Ranked { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x00038213 File Offset: 0x00036413
		// (set) Token: 0x060017A7 RID: 6055 RVA: 0x0003821B File Offset: 0x0003641B
		public bool IFA { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00038224 File Offset: 0x00036424
		// (set) Token: 0x060017A9 RID: 6057 RVA: 0x0003822C File Offset: 0x0003642C
		public int UsedTime { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x00038235 File Offset: 0x00036435
		// (set) Token: 0x060017AB RID: 6059 RVA: 0x0003823D File Offset: 0x0003643D
		public int PlayerClock { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x00038246 File Offset: 0x00036446
		// (set) Token: 0x060017AD RID: 6061 RVA: 0x0003824E File Offset: 0x0003644E
		public bool Leaver { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x00038257 File Offset: 0x00036457
		// (set) Token: 0x060017AF RID: 6063 RVA: 0x0003825F File Offset: 0x0003645F
		public int StructureBonus { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00038268 File Offset: 0x00036468
		// (set) Token: 0x060017B1 RID: 6065 RVA: 0x00038270 File Offset: 0x00036470
		public List<string> PlayersStats { get; set; }

		// Token: 0x060017B2 RID: 6066 RVA: 0x0009FD9C File Offset: 0x0009DF9C
		public List<PlayerEndGameStats> GetEndGameStats()
		{
			List<PlayerEndGameStats> list = new List<PlayerEndGameStats>(this.PlayersStats.Count);
			for (int i = 0; i < this.PlayersStats.Count; i++)
			{
				list.Add(PlayerEndGameStats.Deserialize(this.PlayersStats[i]));
			}
			return list;
		}
	}
}
