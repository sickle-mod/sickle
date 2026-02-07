using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008E5 RID: 2277
	[Serializable]
	public class PartnerError
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x0004FCE8 File Offset: 0x0004DEE8
		public PartnerAccount Partner
		{
			get
			{
				return this._partner;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0004FCF0 File Offset: 0x0004DEF0
		public string ApiErrorCode
		{
			get
			{
				return this._apiErrorCode;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x0004FCF8 File Offset: 0x0004DEF8
		public string ApiErrorDescription
		{
			get
			{
				return this._apiErrorDescription;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0004FD00 File Offset: 0x0004DF00
		public PartnerError.ExtraDetails ExtraDetailsForAdd
		{
			get
			{
				return this._extraDetails;
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0004FD08 File Offset: 0x0004DF08
		public PartnerError(PartnerAccount partnerAccount, string ApiErrorCode, string ApiErrorDescription, PartnerError.ExtraDetails extraDetails)
		{
			this._partner = partnerAccount;
			this._apiErrorCode = ApiErrorCode;
			this._apiErrorDescription = ApiErrorDescription;
			this._extraDetails = extraDetails;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0004FD2D File Offset: 0x0004DF2D
		public static List<PartnerError> ExtractRemovePartnerError(ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails[] details)
		{
			if (details != null)
			{
				return details.Select((ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails x) => new PartnerError(new PartnerAccount(x.partner, x.partner_user, null), x.error_code, x.error_description, null)).ToList<PartnerError>();
			}
			return null;
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0004FD5E File Offset: 0x0004DF5E
		public static List<PartnerError> ExtractAddPartnerError(ApiResponseLinkUnlinkMultipleError.Details.AddDetails[] details)
		{
			if (details != null)
			{
				return details.Select((ApiResponseLinkUnlinkMultipleError.Details.AddDetails x) => new PartnerError(new PartnerAccount(x.partner, x.partner_user, null), x.error_code, x.error_description, (x.error_details == null) ? null : new PartnerError.ExtraDetails(x.error_details.conflict_id, x.error_details.conflict_login))).ToList<PartnerError>();
			}
			return null;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00158688 File Offset: 0x00156888
		public override bool Equals(object o)
		{
			PartnerError partnerError = o as PartnerError;
			return partnerError != null && (this.Partner.Equals(partnerError.Partner) && this.ApiErrorCode == partnerError.ApiErrorCode && this.ApiErrorDescription == partnerError.ApiErrorDescription) && ((this.ExtraDetailsForAdd == null && partnerError.ExtraDetailsForAdd == null) || this.ExtraDetailsForAdd.Equals(partnerError.ExtraDetailsForAdd));
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00158700 File Offset: 0x00156900
		public override int GetHashCode()
		{
			return ((this.Partner == null) ? 0 : this.Partner.GetHashCode()) ^ ((this.ApiErrorCode == null) ? 0 : this.ApiErrorCode.GetHashCode()) ^ ((this.ApiErrorDescription == null) ? 0 : this.ApiErrorDescription.GetHashCode()) ^ ((this.ExtraDetailsForAdd == null) ? 0 : this.ExtraDetailsForAdd.GetHashCode());
		}

		// Token: 0x04002F8E RID: 12174
		[SerializeField]
		private PartnerAccount _partner;

		// Token: 0x04002F8F RID: 12175
		[SerializeField]
		private string _apiErrorCode;

		// Token: 0x04002F90 RID: 12176
		[SerializeField]
		private string _apiErrorDescription;

		// Token: 0x04002F91 RID: 12177
		[SerializeField]
		private PartnerError.ExtraDetails _extraDetails;

		// Token: 0x020008E6 RID: 2278
		[Serializable]
		public class ExtraDetails
		{
			// Token: 0x17000559 RID: 1369
			// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0004FD8F File Offset: 0x0004DF8F
			public string ConflictLogin
			{
				get
				{
					return this._conflictLogin;
				}
			}

			// Token: 0x1700055A RID: 1370
			// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x0004FD97 File Offset: 0x0004DF97
			public int ConflictId
			{
				get
				{
					return this._conflictId;
				}
			}

			// Token: 0x06003DE6 RID: 15846 RVA: 0x0004FD9F File Offset: 0x0004DF9F
			public ExtraDetails(int conflictId, string conflictLogin)
			{
				this._conflictId = conflictId;
				this._conflictLogin = conflictLogin;
			}

			// Token: 0x06003DE7 RID: 15847 RVA: 0x00158768 File Offset: 0x00156968
			public override bool Equals(object obj)
			{
				PartnerError.ExtraDetails extraDetails = obj as PartnerError.ExtraDetails;
				return extraDetails != null && this.ConflictId == extraDetails.ConflictId && this.ConflictLogin == extraDetails.ConflictLogin;
			}

			// Token: 0x06003DE8 RID: 15848 RVA: 0x0004FDB5 File Offset: 0x0004DFB5
			public override int GetHashCode()
			{
				return this.ConflictId ^ ((this.ConflictLogin == null) ? 0 : this.ConflictLogin.GetHashCode());
			}

			// Token: 0x04002F92 RID: 12178
			[SerializeField]
			private string _conflictLogin;

			// Token: 0x04002F93 RID: 12179
			[SerializeField]
			private int _conflictId;
		}
	}
}
