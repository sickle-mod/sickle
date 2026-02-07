using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000894 RID: 2196
	[Serializable]
	public class ApiGetAchievementListResponse
	{
		// Token: 0x04002E51 RID: 11857
		public bool error;

		// Token: 0x04002E52 RID: 11858
		public int status;

		// Token: 0x04002E53 RID: 11859
		public ApiGetAchievementListResponse.Data data;

		// Token: 0x02000895 RID: 2197
		[Serializable]
		public class Data
		{
			// Token: 0x04002E54 RID: 11860
			public JsonAchievement[] achievements;

			// Token: 0x04002E55 RID: 11861
			public int total;

			// Token: 0x04002E56 RID: 11862
			public Links _links;
		}
	}
}
