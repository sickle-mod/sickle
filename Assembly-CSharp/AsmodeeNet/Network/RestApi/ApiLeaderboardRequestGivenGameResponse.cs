using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008AD RID: 2221
	[Serializable]
	public class ApiLeaderboardRequestGivenGameResponse
	{
		// Token: 0x04002EC1 RID: 11969
		public bool error;

		// Token: 0x04002EC2 RID: 11970
		public int status;

		// Token: 0x04002EC3 RID: 11971
		public ApiLeaderboardRequestGivenGameResponse.Data data;

		// Token: 0x020008AE RID: 2222
		[Serializable]
		public class Data
		{
			// Token: 0x04002EC4 RID: 11972
			public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard leaderboard;

			// Token: 0x020008AF RID: 2223
			[Serializable]
			public class Leaderboard
			{
				// Token: 0x04002EC5 RID: 11973
				public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Links _links;

				// Token: 0x04002EC6 RID: 11974
				public int total;

				// Token: 0x04002EC7 RID: 11975
				public string id;

				// Token: 0x04002EC8 RID: 11976
				public string game;

				// Token: 0x04002EC9 RID: 11977
				public string period;

				// Token: 0x04002ECA RID: 11978
				public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Player[] players;

				// Token: 0x020008B0 RID: 2224
				[Serializable]
				public class Links
				{
					// Token: 0x04002ECB RID: 11979
					public string first;

					// Token: 0x04002ECC RID: 11980
					public string last;

					// Token: 0x04002ECD RID: 11981
					public string next;

					// Token: 0x04002ECE RID: 11982
					public string prev;
				}

				// Token: 0x020008B1 RID: 2225
				[Serializable]
				public class Player
				{
					// Token: 0x04002ECF RID: 11983
					public int rank;

					// Token: 0x04002ED0 RID: 11984
					public int id;

					// Token: 0x04002ED1 RID: 11985
					public int score;

					// Token: 0x04002ED2 RID: 11986
					public string context;

					// Token: 0x04002ED3 RID: 11987
					public string name;

					// Token: 0x04002ED4 RID: 11988
					public string avatar;

					// Token: 0x04002ED5 RID: 11989
					public string when;
				}
			}
		}
	}
}
