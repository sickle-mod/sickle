using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008A0 RID: 2208
	[Serializable]
	public class ApiGetUserDetailsResponse
	{
		// Token: 0x04002E80 RID: 11904
		public bool error;

		// Token: 0x04002E81 RID: 11905
		public int status;

		// Token: 0x04002E82 RID: 11906
		public ApiGetUserDetailsResponse.Data data;

		// Token: 0x020008A1 RID: 2209
		[Serializable]
		public class Data
		{
			// Token: 0x04002E83 RID: 11907
			public ApiGetUserDetailsResponse.Data.User user;

			// Token: 0x020008A2 RID: 2210
			[Serializable]
			public class User
			{
				// Token: 0x04002E84 RID: 11908
				public int user_id;

				// Token: 0x04002E85 RID: 11909
				public string login_name;

				// Token: 0x04002E86 RID: 11910
				public string country;

				// Token: 0x04002E87 RID: 11911
				public bool email_valid;

				// Token: 0x04002E88 RID: 11912
				public string language;

				// Token: 0x04002E89 RID: 11913
				public string time_zone;

				// Token: 0x04002E8A RID: 11914
				public int posted_msg_count;

				// Token: 0x04002E8B RID: 11915
				public int last_post_id;

				// Token: 0x04002E8C RID: 11916
				public bool validated;

				// Token: 0x04002E8D RID: 11917
				public string avatar;

				// Token: 0x04002E8E RID: 11918
				public long join_date_ts;

				// Token: 0x04002E8F RID: 11919
				public string join_date;

				// Token: 0x04002E90 RID: 11920
				public string join_date_cet;

				// Token: 0x04002E91 RID: 11921
				public long last_visit_ts;

				// Token: 0x04002E92 RID: 11922
				public string last_visit;

				// Token: 0x04002E93 RID: 11923
				public string last_visit_cet;

				// Token: 0x04002E94 RID: 11924
				public string zipcode;

				// Token: 0x04002E95 RID: 11925
				public string birthday;

				// Token: 0x04002E96 RID: 11926
				public string email;

				// Token: 0x04002E97 RID: 11927
				public string gender;

				// Token: 0x04002E98 RID: 11928
				public bool coppa;

				// Token: 0x04002E99 RID: 11929
				public string name;

				// Token: 0x04002E9A RID: 11930
				public string[] features;

				// Token: 0x04002E9B RID: 11931
				public ApiGetUserDetailsResponse.Data.User.BoardGame[] boardgames;

				// Token: 0x04002E9C RID: 11932
				public ApiGetUserDetailsResponse.Data.User.OnlineGame[] onlinegames;

				// Token: 0x04002E9D RID: 11933
				public ApiGetUserDetailsResponse.Data.User.Partners[] partners;

				// Token: 0x020008A3 RID: 2211
				[Serializable]
				public class BoardGame
				{
					// Token: 0x04002E9E RID: 11934
					public string code;

					// Token: 0x04002E9F RID: 11935
					public string name;

					// Token: 0x04002EA0 RID: 11936
					public string registered_date;

					// Token: 0x04002EA1 RID: 11937
					public long registered_date_ts;
				}

				// Token: 0x020008A4 RID: 2212
				[Serializable]
				public class OnlineGame
				{
					// Token: 0x04002EA2 RID: 11938
					public string game;

					// Token: 0x04002EA3 RID: 11939
					public int nbgames;

					// Token: 0x04002EA4 RID: 11940
					public int karma;

					// Token: 0x04002EA5 RID: 11941
					public float rankscore;

					// Token: 0x04002EA6 RID: 11942
					public int rank;

					// Token: 0x04002EA7 RID: 11943
					public string lastgame;

					// Token: 0x04002EA8 RID: 11944
					public string variant;

					// Token: 0x04002EA9 RID: 11945
					public long lastgame_ts;
				}

				// Token: 0x020008A5 RID: 2213
				[Serializable]
				public class Partners
				{
					// Token: 0x04002EAA RID: 11946
					public int partner_id;

					// Token: 0x04002EAB RID: 11947
					public string partner_user_id;

					// Token: 0x04002EAC RID: 11948
					public string created_at;
				}
			}
		}
	}
}
