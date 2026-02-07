using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008BB RID: 2235
	[Serializable]
	public class ApiResponseRemoveAwardError : ApiResponseError
	{
		// Token: 0x04002EF7 RID: 12023
		public ApiResponseRemoveAwardError.Details error_details;

		// Token: 0x020008BC RID: 2236
		[Serializable]
		public class Details
		{
			// Token: 0x04002EF8 RID: 12024
			public string[] not_owned_awards;

			// Token: 0x04002EF9 RID: 12025
			public string[] invalid_awards;
		}
	}
}
