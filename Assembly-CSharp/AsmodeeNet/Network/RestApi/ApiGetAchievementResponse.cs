using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000896 RID: 2198
	[Serializable]
	public class ApiGetAchievementResponse
	{
		// Token: 0x04002E57 RID: 11863
		public bool error;

		// Token: 0x04002E58 RID: 11864
		public int status;

		// Token: 0x04002E59 RID: 11865
		public ApiGetAchievementResponse.Data data;

		// Token: 0x02000897 RID: 2199
		[Serializable]
		public class Data
		{
			// Token: 0x04002E5A RID: 11866
			public JsonAchievement achievement;
		}
	}
}
