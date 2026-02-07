using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008C8 RID: 2248
	[Serializable]
	public class ApiShowcaseBannerResponse
	{
		// Token: 0x04002F1F RID: 12063
		public int status;

		// Token: 0x04002F20 RID: 12064
		public bool error;

		// Token: 0x04002F21 RID: 12065
		public ApiShowcaseBannerResponse.Data data;

		// Token: 0x020008C9 RID: 2249
		[Serializable]
		public class Data
		{
			// Token: 0x04002F22 RID: 12066
			public ApiShowcaseProduct product;
		}
	}
}
