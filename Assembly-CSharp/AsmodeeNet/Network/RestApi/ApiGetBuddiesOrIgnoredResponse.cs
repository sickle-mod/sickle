using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200089D RID: 2205
	[Serializable]
	public class ApiGetBuddiesOrIgnoredResponse
	{
		// Token: 0x04002E77 RID: 11895
		public bool error;

		// Token: 0x04002E78 RID: 11896
		public int status;

		// Token: 0x04002E79 RID: 11897
		public ApiGetBuddiesOrIgnoredResponse.Data data;

		// Token: 0x0200089E RID: 2206
		[Serializable]
		public class Data
		{
			// Token: 0x04002E7A RID: 11898
			public int total;

			// Token: 0x04002E7B RID: 11899
			public Links _links;

			// Token: 0x04002E7C RID: 11900
			public ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored[] buddies;

			// Token: 0x04002E7D RID: 11901
			public ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored[] ignored;

			// Token: 0x0200089F RID: 2207
			[Serializable]
			public class BuddyOrIgnored
			{
				// Token: 0x04002E7E RID: 11902
				public int id;

				// Token: 0x04002E7F RID: 11903
				public string login_name;
			}
		}
	}
}
