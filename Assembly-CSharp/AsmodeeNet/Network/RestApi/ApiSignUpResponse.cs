using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008D1 RID: 2257
	[Serializable]
	public class ApiSignUpResponse
	{
		// Token: 0x04002F3C RID: 12092
		public bool error;

		// Token: 0x04002F3D RID: 12093
		public int status;

		// Token: 0x04002F3E RID: 12094
		public ApiSignUpResponse.Data data;

		// Token: 0x020008D2 RID: 2258
		[Serializable]
		public class Data
		{
			// Token: 0x04002F3F RID: 12095
			public int user_id;

			// Token: 0x04002F40 RID: 12096
			public string login_name;
		}
	}
}
