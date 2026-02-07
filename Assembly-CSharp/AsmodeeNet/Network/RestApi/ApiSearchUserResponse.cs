using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008C3 RID: 2243
	[Serializable]
	public class ApiSearchUserResponse
	{
		// Token: 0x04002F07 RID: 12039
		public bool error;

		// Token: 0x04002F08 RID: 12040
		public int status;

		// Token: 0x04002F09 RID: 12041
		public ApiSearchUserResponse.Data data;

		// Token: 0x020008C4 RID: 2244
		[Serializable]
		public class Data
		{
			// Token: 0x04002F0A RID: 12042
			public int total;

			// Token: 0x04002F0B RID: 12043
			public Links _links;

			// Token: 0x04002F0C RID: 12044
			public ApiSearchUserResponse.Data.User[] users;

			// Token: 0x020008C5 RID: 2245
			[Serializable]
			public class User
			{
				// Token: 0x04002F0D RID: 12045
				public int user_id;

				// Token: 0x04002F0E RID: 12046
				public string login_name;

				// Token: 0x04002F0F RID: 12047
				public string avatar;

				// Token: 0x04002F10 RID: 12048
				public string[] features;

				// Token: 0x04002F11 RID: 12049
				public ApiSearchUserResponse.Data.User.BoardGame[] boardgames;

				// Token: 0x04002F12 RID: 12050
				public ApiSearchUserResponse.Data.User.OnlineGame[] onlinegames;

				// Token: 0x020008C6 RID: 2246
				[Serializable]
				public class BoardGame
				{
					// Token: 0x04002F13 RID: 12051
					public string code;

					// Token: 0x04002F14 RID: 12052
					public string name;

					// Token: 0x04002F15 RID: 12053
					public string registered_date;

					// Token: 0x04002F16 RID: 12054
					public long registered_date_ts;
				}

				// Token: 0x020008C7 RID: 2247
				[Serializable]
				public class OnlineGame
				{
					// Token: 0x04002F17 RID: 12055
					public string game;

					// Token: 0x04002F18 RID: 12056
					public int nbgames;

					// Token: 0x04002F19 RID: 12057
					public int karma;

					// Token: 0x04002F1A RID: 12058
					public float rankscore;

					// Token: 0x04002F1B RID: 12059
					public int rank;

					// Token: 0x04002F1C RID: 12060
					public string lastgame;

					// Token: 0x04002F1D RID: 12061
					public string variant;

					// Token: 0x04002F1E RID: 12062
					public long lastgame_ts;
				}
			}
		}
	}
}
