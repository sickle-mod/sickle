using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000934 RID: 2356
	public class UserError : WebError
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x00050AB3 File Offset: 0x0004ECB3
		// (set) Token: 0x06003F69 RID: 16233 RVA: 0x00050ABB File Offset: 0x0004ECBB
		public Builder<User>.BuilderErrors[] BuildingErrors { get; private set; }

		// Token: 0x06003F6A RID: 16234 RVA: 0x00050AC4 File Offset: 0x0004ECC4
		public UserError(Builder<User>.BuilderErrors[] errors)
		{
			this.status = -1;
			this.error = "Error when building the result.";
			this.BuildingErrors = errors;
		}
	}
}
