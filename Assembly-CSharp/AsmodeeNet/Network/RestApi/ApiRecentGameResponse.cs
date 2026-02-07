using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008B2 RID: 2226
	[Serializable]
	public class ApiRecentGameResponse
	{
		// Token: 0x04002ED6 RID: 11990
		public ApiRecentGameResponse.Data data;

		// Token: 0x04002ED7 RID: 11991
		public bool error;

		// Token: 0x04002ED8 RID: 11992
		public int status;

		// Token: 0x020008B3 RID: 2227
		[Serializable]
		public class Data
		{
			// Token: 0x04002ED9 RID: 11993
			public ApiRecentGameResponse.Data.Game[] games;

			// Token: 0x020008B4 RID: 2228
			[Serializable]
			public class Game
			{
				// Token: 0x04002EDA RID: 11994
				public string table_id;

				// Token: 0x04002EDB RID: 11995
				public string date;

				// Token: 0x04002EDC RID: 11996
				public string game;

				// Token: 0x04002EDD RID: 11997
				public string options;

				// Token: 0x04002EDE RID: 11998
				public string status;

				// Token: 0x04002EDF RID: 11999
				public string variant;

				// Token: 0x04002EE0 RID: 12000
				public int score;

				// Token: 0x04002EE1 RID: 12001
				public ApiRecentGameResponse.Data.Game.OtherPlayer[] other_players;

				// Token: 0x020008B5 RID: 2229
				[Serializable]
				public class OtherPlayer
				{
					// Token: 0x04002EE2 RID: 12002
					public int id;

					// Token: 0x04002EE3 RID: 12003
					public string login_name;

					// Token: 0x04002EE4 RID: 12004
					public string avatar;

					// Token: 0x04002EE5 RID: 12005
					public int score;
				}
			}
		}
	}
}
