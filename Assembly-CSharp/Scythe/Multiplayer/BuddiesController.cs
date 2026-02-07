using System;
using Multiplayer.AuthApi;
using Scythe.Multiplayer.AuthApi.Models;

namespace Scythe.Multiplayer
{
	// Token: 0x0200022D RID: 557
	public class BuddiesController : PersistentSingleton<BuddiesController>
	{
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06001076 RID: 4214 RVA: 0x0008FE3C File Offset: 0x0008E03C
		// (remove) Token: 0x06001077 RID: 4215 RVA: 0x0008FE74 File Offset: 0x0008E074
		public event Action<GetBuddiesResponse> OnGetBuddiesSuccess;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06001078 RID: 4216 RVA: 0x0008FEAC File Offset: 0x0008E0AC
		// (remove) Token: 0x06001079 RID: 4217 RVA: 0x0008FEE4 File Offset: 0x0008E0E4
		public event Action<FailureResponse> OnGetBuddiesFailure;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x0600107A RID: 4218 RVA: 0x0008FF1C File Offset: 0x0008E11C
		// (remove) Token: 0x0600107B RID: 4219 RVA: 0x0008FF54 File Offset: 0x0008E154
		public event Action<Exception> OnGetBuddiesError;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x0600107C RID: 4220 RVA: 0x0008FF8C File Offset: 0x0008E18C
		// (remove) Token: 0x0600107D RID: 4221 RVA: 0x0008FFC4 File Offset: 0x0008E1C4
		public event Action<AddBuddyResponse> OnAddBuddySuccess;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x0600107E RID: 4222 RVA: 0x0008FFFC File Offset: 0x0008E1FC
		// (remove) Token: 0x0600107F RID: 4223 RVA: 0x00090034 File Offset: 0x0008E234
		public event Action<FailureResponse> OnAddBuddyFailure;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06001080 RID: 4224 RVA: 0x0009006C File Offset: 0x0008E26C
		// (remove) Token: 0x06001081 RID: 4225 RVA: 0x000900A4 File Offset: 0x0008E2A4
		public event Action<Exception> OnAddBuddyError;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06001082 RID: 4226 RVA: 0x000900DC File Offset: 0x0008E2DC
		// (remove) Token: 0x06001083 RID: 4227 RVA: 0x00090114 File Offset: 0x0008E314
		public event Action<AddBuddyByNameResponse> OnAddBuddyByNameSuccess;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06001084 RID: 4228 RVA: 0x0009014C File Offset: 0x0008E34C
		// (remove) Token: 0x06001085 RID: 4229 RVA: 0x00090184 File Offset: 0x0008E384
		public event Action<FailureResponse> OnAddBuddyByNameFailure;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06001086 RID: 4230 RVA: 0x000901BC File Offset: 0x0008E3BC
		// (remove) Token: 0x06001087 RID: 4231 RVA: 0x000901F4 File Offset: 0x0008E3F4
		public event Action<Exception> OnAddBuddyByNameError;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06001088 RID: 4232 RVA: 0x0009022C File Offset: 0x0008E42C
		// (remove) Token: 0x06001089 RID: 4233 RVA: 0x00090264 File Offset: 0x0008E464
		public event Action<RemoveBuddyResponse> OnRemoveBuddySuccess;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x0600108A RID: 4234 RVA: 0x0009029C File Offset: 0x0008E49C
		// (remove) Token: 0x0600108B RID: 4235 RVA: 0x000902D4 File Offset: 0x0008E4D4
		public event Action<FailureResponse> OnRemoveBuddyFailure;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600108C RID: 4236 RVA: 0x0009030C File Offset: 0x0008E50C
		// (remove) Token: 0x0600108D RID: 4237 RVA: 0x00090344 File Offset: 0x0008E544
		public event Action<Exception> OnRemoveBuddyError;

		// Token: 0x0600108E RID: 4238 RVA: 0x00032C7C File Offset: 0x00030E7C
		public void GetBuddies()
		{
			AuthRestAPI.GetBuddies(this.OnGetBuddiesSuccess, this.OnGetBuddiesFailure, this.OnGetBuddiesError);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00032C95 File Offset: 0x00030E95
		public void AddBuddy(string buddyName)
		{
			AuthRestAPI.AddBuddyByName(buddyName, this.OnAddBuddyByNameSuccess, this.OnAddBuddyByNameFailure, this.OnAddBuddyByNameError);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00032CAF File Offset: 0x00030EAF
		public void AddBuddy(Guid guid)
		{
			AuthRestAPI.AddBuddy(guid, this.OnAddBuddySuccess, this.OnAddBuddyFailure, this.OnAddBuddyError);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00032CC9 File Offset: 0x00030EC9
		public void RemoveBuddy(Guid guid)
		{
			AuthRestAPI.RemoveBuddy(guid, this.OnRemoveBuddySuccess, this.OnRemoveBuddyFailure, this.OnRemoveBuddyError);
		}
	}
}
