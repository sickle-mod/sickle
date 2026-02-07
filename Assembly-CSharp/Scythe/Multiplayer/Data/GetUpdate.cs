using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000313 RID: 787
	public class GetUpdate : Data
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00037A59 File Offset: 0x00035C59
		// (set) Token: 0x060016E0 RID: 5856 RVA: 0x00037A61 File Offset: 0x00035C61
		public int LastExecutedMessage { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00037A6A File Offset: 0x00035C6A
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00037A72 File Offset: 0x00035C72
		public bool Asynchronous { get; set; }

		// Token: 0x060016E3 RID: 5859 RVA: 0x00037A7B File Offset: 0x00035C7B
		public GetUpdate()
		{
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00037A83 File Offset: 0x00035C83
		public GetUpdate(int lastExecutedMessage, bool asynchronous)
		{
			this.LastExecutedMessage = lastExecutedMessage;
			this.Asynchronous = asynchronous;
		}
	}
}
