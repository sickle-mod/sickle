using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D1 RID: 2513
	public class ConnectAsmodeeNetAnalyticsContext : AnalyticsContext
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x00052316 File Offset: 0x00050516
		// (set) Token: 0x060041D7 RID: 16855 RVA: 0x0005231E File Offset: 0x0005051E
		public string SigninSessionId { get; private set; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x00052327 File Offset: 0x00050527
		// (set) Token: 0x060041D9 RID: 16857 RVA: 0x0005232F File Offset: 0x0005052F
		public string ConnectPath { get; private set; }

		// Token: 0x060041DA RID: 16858 RVA: 0x00162868 File Offset: 0x00160A68
		public ConnectAsmodeeNetAnalyticsContext(string connectPath)
		{
			this.SigninSessionId = Guid.NewGuid().ToString();
			this.ConnectPath = connectPath;
		}
	}
}
