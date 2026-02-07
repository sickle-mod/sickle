using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008BA RID: 2234
	[Serializable]
	public class ApiResponseError : WebError
	{
		// Token: 0x04002EF5 RID: 12021
		public string error_description;

		// Token: 0x04002EF6 RID: 12022
		public string error_code;
	}
}
