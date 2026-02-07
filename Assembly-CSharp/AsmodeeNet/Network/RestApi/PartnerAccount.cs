using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008E4 RID: 2276
	[Serializable]
	public class PartnerAccount
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x0004FC24 File Offset: 0x0004DE24
		public int PartnerId
		{
			get
			{
				return this._partnerId;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x0004FC2C File Offset: 0x0004DE2C
		public string PartnerUser
		{
			get
			{
				return this._partnerUser;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0004FC34 File Offset: 0x0004DE34
		public string Token
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x0004FC3C File Offset: 0x0004DE3C
		public string App_Id
		{
			get
			{
				return this._app_id;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x0004FC44 File Offset: 0x0004DE44
		public DateTime? CreatedAt
		{
			get
			{
				return this._createdAt;
			}
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x00027E56 File Offset: 0x00026056
		public PartnerAccount()
		{
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0004FC4C File Offset: 0x0004DE4C
		public PartnerAccount(int partnerId, string partnerUser, DateTime? createdAt = null)
		{
			this._partnerId = partnerId;
			this._partnerUser = partnerUser;
			this._token = string.Empty;
			this._app_id = string.Empty;
			this._createdAt = createdAt;
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0004FC7F File Offset: 0x0004DE7F
		public PartnerAccount(int partnerId, string partnerUser, string token, string app_id, DateTime? createdAt = null)
		{
			this._partnerId = partnerId;
			this._partnerUser = partnerUser;
			this._token = token;
			this._app_id = app_id;
			this._createdAt = createdAt;
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x00158648 File Offset: 0x00156848
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			PartnerAccount partnerAccount = obj as PartnerAccount;
			return partnerAccount != null && this.PartnerId == partnerAccount.PartnerId && this.PartnerUser == partnerAccount.PartnerUser;
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0004FCAC File Offset: 0x0004DEAC
		public override int GetHashCode()
		{
			return this._partnerId ^ ((this._partnerUser == null) ? 0 : this._partnerUser.GetHashCode());
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0004FCCB File Offset: 0x0004DECB
		public override string ToString()
		{
			return string.Format("Partner({0}, {1})", this._partnerId, this._partnerUser);
		}

		// Token: 0x04002F88 RID: 12168
		public const int kSteamPartner = 12;

		// Token: 0x04002F89 RID: 12169
		[SerializeField]
		private int _partnerId;

		// Token: 0x04002F8A RID: 12170
		[SerializeField]
		private string _partnerUser;

		// Token: 0x04002F8B RID: 12171
		[SerializeField]
		private string _token;

		// Token: 0x04002F8C RID: 12172
		[SerializeField]
		private string _app_id;

		// Token: 0x04002F8D RID: 12173
		[SerializeField]
		private DateTime? _createdAt;
	}
}
