using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200030A RID: 778
	public class AdminMessage
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00037706 File Offset: 0x00035906
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x0003770E File Offset: 0x0003590E
		public string Message { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00037717 File Offset: 0x00035917
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x0003771F File Offset: 0x0003591F
		public long Counter { get; set; }
	}
}
