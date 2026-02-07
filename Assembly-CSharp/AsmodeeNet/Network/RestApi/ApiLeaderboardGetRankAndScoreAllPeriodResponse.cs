using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008A6 RID: 2214
	[Serializable]
	public class ApiLeaderboardGetRankAndScoreAllPeriodResponse
	{
		// Token: 0x04002EAD RID: 11949
		public bool error;

		// Token: 0x04002EAE RID: 11950
		public int status;

		// Token: 0x04002EAF RID: 11951
		public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data data;

		// Token: 0x020008A7 RID: 2215
		[Serializable]
		public class Data
		{
			// Token: 0x04002EB0 RID: 11952
			public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User user;

			// Token: 0x020008A8 RID: 2216
			[Serializable]
			public class User
			{
				// Token: 0x04002EB1 RID: 11953
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period day;

				// Token: 0x04002EB2 RID: 11954
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period week;

				// Token: 0x04002EB3 RID: 11955
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period ever;

				// Token: 0x020008A9 RID: 2217
				[Serializable]
				public class Period
				{
					// Token: 0x04002EB4 RID: 11956
					public bool isNew;

					// Token: 0x04002EB5 RID: 11957
					public int rank;

					// Token: 0x04002EB6 RID: 11958
					public int score;

					// Token: 0x04002EB7 RID: 11959
					public string context;

					// Token: 0x04002EB8 RID: 11960
					public string when;
				}
			}
		}
	}
}
