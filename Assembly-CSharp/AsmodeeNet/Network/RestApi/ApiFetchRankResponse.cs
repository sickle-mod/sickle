using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000891 RID: 2193
	[Serializable]
	public class ApiFetchRankResponse
	{
		// Token: 0x04002E46 RID: 11846
		public bool error;

		// Token: 0x04002E47 RID: 11847
		public int status;

		// Token: 0x04002E48 RID: 11848
		public ApiFetchRankResponse.Data data;

		// Token: 0x02000892 RID: 2194
		[Serializable]
		public class Data
		{
			// Token: 0x04002E49 RID: 11849
			public ApiFetchRankResponse.Data.User user;

			// Token: 0x02000893 RID: 2195
			[Serializable]
			public class User
			{
				// Token: 0x04002E4A RID: 11850
				public int id;

				// Token: 0x04002E4B RID: 11851
				public string name;

				// Token: 0x04002E4C RID: 11852
				public int nbgames;

				// Token: 0x04002E4D RID: 11853
				public int rank;

				// Token: 0x04002E4E RID: 11854
				public int karma;

				// Token: 0x04002E4F RID: 11855
				public int score;

				// Token: 0x04002E50 RID: 11856
				public string ranking;
			}
		}
	}
}
