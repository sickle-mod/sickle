using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008B6 RID: 2230
	[Serializable]
	public class ApiRecentOpponentsResponse
	{
		// Token: 0x04002EE6 RID: 12006
		public ApiRecentOpponentsResponse.Data data;

		// Token: 0x04002EE7 RID: 12007
		public bool error;

		// Token: 0x04002EE8 RID: 12008
		public int status;

		// Token: 0x020008B7 RID: 2231
		[Serializable]
		public class Data
		{
			// Token: 0x04002EE9 RID: 12009
			public ApiRecentOpponentsResponse.Data.Opponent[] opponents;

			// Token: 0x020008B8 RID: 2232
			[Serializable]
			public class Opponent
			{
				// Token: 0x04002EEA RID: 12010
				public int id;

				// Token: 0x04002EEB RID: 12011
				public string login_name;

				// Token: 0x04002EEC RID: 12012
				public string avatar;

				// Token: 0x04002EED RID: 12013
				public string last_game_date;

				// Token: 0x04002EEE RID: 12014
				public ApiRecentOpponentsResponse.Data.Opponent.Game[] games;

				// Token: 0x020008B9 RID: 2233
				[Serializable]
				public class Game
				{
					// Token: 0x04002EEF RID: 12015
					public string table_id;

					// Token: 0x04002EF0 RID: 12016
					public string game;

					// Token: 0x04002EF1 RID: 12017
					public string date;

					// Token: 0x04002EF2 RID: 12018
					public string status;

					// Token: 0x04002EF3 RID: 12019
					public int other_score;

					// Token: 0x04002EF4 RID: 12020
					public int score;
				}
			}
		}
	}
}
