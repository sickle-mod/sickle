using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008AA RID: 2218
	[Serializable]
	public class ApiLeaderboardGetRankAndScoreResponse
	{
		// Token: 0x04002EB9 RID: 11961
		public bool error;

		// Token: 0x04002EBA RID: 11962
		public int status;

		// Token: 0x04002EBB RID: 11963
		public ApiLeaderboardGetRankAndScoreResponse.Data data;

		// Token: 0x020008AB RID: 2219
		[Serializable]
		public class Data
		{
			// Token: 0x04002EBC RID: 11964
			public ApiLeaderboardGetRankAndScoreResponse.Data.User user;

			// Token: 0x020008AC RID: 2220
			[Serializable]
			public class User
			{
				// Token: 0x04002EBD RID: 11965
				public int rank;

				// Token: 0x04002EBE RID: 11966
				public int score;

				// Token: 0x04002EBF RID: 11967
				public string context;

				// Token: 0x04002EC0 RID: 11968
				public string when;
			}
		}
	}
}
