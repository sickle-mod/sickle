using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200089A RID: 2202
	[Serializable]
	public class ApiGetAwardListResponse
	{
		// Token: 0x04002E6A RID: 11882
		public bool error;

		// Token: 0x04002E6B RID: 11883
		public int status;

		// Token: 0x04002E6C RID: 11884
		public ApiGetAwardListResponse.Data data;

		// Token: 0x0200089B RID: 2203
		[Serializable]
		public class Data
		{
			// Token: 0x04002E6D RID: 11885
			public int total;

			// Token: 0x04002E6E RID: 11886
			public Links _links;

			// Token: 0x04002E6F RID: 11887
			public ApiGetAwardListResponse.Data.Award[] awards;

			// Token: 0x0200089C RID: 2204
			[Serializable]
			public class Award
			{
				// Token: 0x04002E70 RID: 11888
				public int id;

				// Token: 0x04002E71 RID: 11889
				public string tag;

				// Token: 0x04002E72 RID: 11890
				public int table_id;

				// Token: 0x04002E73 RID: 11891
				public int info_id;

				// Token: 0x04002E74 RID: 11892
				public string awarded_utc;

				// Token: 0x04002E75 RID: 11893
				public string awarded_cet;

				// Token: 0x04002E76 RID: 11894
				public long awarded_ts;
			}
		}
	}
}
