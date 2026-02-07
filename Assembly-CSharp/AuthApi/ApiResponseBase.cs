using System;

namespace AuthApi
{
	// Token: 0x020001C2 RID: 450
	public abstract class ApiResponseBase
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00030C29 File Offset: 0x0002EE29
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00030C31 File Offset: 0x0002EE31
		public bool IsSuccesful { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00030C3A File Offset: 0x0002EE3A
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x00030C42 File Offset: 0x0002EE42
		public string Message { get; set; }
	}
}
