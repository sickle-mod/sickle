using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008BD RID: 2237
	[Serializable]
	public class ApiResponseLinkUnlinkMultipleError : ApiResponseError
	{
		// Token: 0x04002EFA RID: 12026
		public ApiResponseLinkUnlinkMultipleError.Details error_details;

		// Token: 0x020008BE RID: 2238
		[Serializable]
		public class Details
		{
			// Token: 0x04002EFB RID: 12027
			public ApiResponseLinkUnlinkMultipleError.Details.AddDetails[] add;

			// Token: 0x04002EFC RID: 12028
			public ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails[] remove;

			// Token: 0x020008BF RID: 2239
			[Serializable]
			public class RemoveDetails
			{
				// Token: 0x04002EFD RID: 12029
				public int partner;

				// Token: 0x04002EFE RID: 12030
				public string partner_user;

				// Token: 0x04002EFF RID: 12031
				public string error_code;

				// Token: 0x04002F00 RID: 12032
				public string error_description;
			}

			// Token: 0x020008C0 RID: 2240
			[Serializable]
			public class AddDetails : ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails
			{
				// Token: 0x04002F01 RID: 12033
				public ApiResponseLinkUnlinkMultipleError.Details.AddDetails.ExtraDetails error_details;

				// Token: 0x020008C1 RID: 2241
				[Serializable]
				public class ExtraDetails
				{
					// Token: 0x04002F02 RID: 12034
					public int conflict_id;

					// Token: 0x04002F03 RID: 12035
					public string conflict_login;
				}
			}
		}
	}
}
