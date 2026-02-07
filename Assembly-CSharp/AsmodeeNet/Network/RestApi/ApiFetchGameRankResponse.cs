using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200088E RID: 2190
	[Serializable]
	public class ApiFetchGameRankResponse
	{
		// Token: 0x04002E3A RID: 11834
		public bool error;

		// Token: 0x04002E3B RID: 11835
		public int status;

		// Token: 0x04002E3C RID: 11836
		public ApiFetchGameRankResponse.Data data;

		// Token: 0x0200088F RID: 2191
		[Serializable]
		public class Data
		{
			// Token: 0x04002E3D RID: 11837
			public ApiFetchGameRankResponse.Data.Rank[] ranks;

			// Token: 0x04002E3E RID: 11838
			public int total;

			// Token: 0x04002E3F RID: 11839
			public Links _links;

			// Token: 0x02000890 RID: 2192
			[Serializable]
			public class Rank
			{
				// Token: 0x04002E40 RID: 11840
				public int id;

				// Token: 0x04002E41 RID: 11841
				public string name;

				// Token: 0x04002E42 RID: 11842
				public int nbgames;

				// Token: 0x04002E43 RID: 11843
				public int karma;

				// Token: 0x04002E44 RID: 11844
				public int score;

				// Token: 0x04002E45 RID: 11845
				public int rank;
			}
		}
	}
}
