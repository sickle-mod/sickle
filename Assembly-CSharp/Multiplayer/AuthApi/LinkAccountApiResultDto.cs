using System;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A6 RID: 422
	[Serializable]
	public class LinkAccountApiResultDto
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0003043E File Offset: 0x0002E63E
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x00030446 File Offset: 0x0002E646
		public bool IsSuccessful { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0003044F File Offset: 0x0002E64F
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x00030457 File Offset: 0x0002E657
		public string Message { get; set; }
	}
}
