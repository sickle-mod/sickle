using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008E1 RID: 2273
	public class LeaderboardScoringInfoError : WebError
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x0004FB57 File Offset: 0x0004DD57
		// (set) Token: 0x06003DC0 RID: 15808 RVA: 0x0004FB5F File Offset: 0x0004DD5F
		public Builder<LeaderboardScoringInfo>.BuilderErrors[] BuildingErrors { get; private set; }

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0004FB68 File Offset: 0x0004DD68
		public LeaderboardScoringInfoError(Builder<LeaderboardScoringInfo>.BuilderErrors[] errors)
		{
			this.status = -1;
			this.error = "Error when building the result.";
			this.BuildingErrors = errors;
		}
	}
}
