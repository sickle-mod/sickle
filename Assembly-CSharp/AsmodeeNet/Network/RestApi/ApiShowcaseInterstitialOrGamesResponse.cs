using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008CA RID: 2250
	[Serializable]
	public class ApiShowcaseInterstitialOrGamesResponse
	{
		// Token: 0x04002F23 RID: 12067
		public int status;

		// Token: 0x04002F24 RID: 12068
		public bool error;

		// Token: 0x04002F25 RID: 12069
		public ApiShowcaseInterstitialOrGamesResponse.Data data;

		// Token: 0x020008CB RID: 2251
		[Serializable]
		public class Data
		{
			// Token: 0x04002F26 RID: 12070
			public ApiShowcaseProduct[] products;
		}
	}
}
