using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034E RID: 846
	public class PlayerInit
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x000386A2 File Offset: 0x000368A2
		// (set) Token: 0x0600183D RID: 6205 RVA: 0x000386AA File Offset: 0x000368AA
		public int Faction { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x000386B3 File Offset: 0x000368B3
		// (set) Token: 0x0600183F RID: 6207 RVA: 0x000386BB File Offset: 0x000368BB
		public int Mat { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x000386C4 File Offset: 0x000368C4
		// (set) Token: 0x06001841 RID: 6209 RVA: 0x000386CC File Offset: 0x000368CC
		public List<int> CombatCards { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x000386D5 File Offset: 0x000368D5
		// (set) Token: 0x06001843 RID: 6211 RVA: 0x000386DD File Offset: 0x000368DD
		public List<int> ObjectiveCards { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x000386E6 File Offset: 0x000368E6
		// (set) Token: 0x06001845 RID: 6213 RVA: 0x000386EE File Offset: 0x000368EE
		public string Name { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001846 RID: 6214 RVA: 0x000386F7 File Offset: 0x000368F7
		// (set) Token: 0x06001847 RID: 6215 RVA: 0x000386FF File Offset: 0x000368FF
		public Guid Id { get; set; }
	}
}
