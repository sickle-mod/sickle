using System;
using System.Collections.Generic;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008DF RID: 2271
	[Serializable]
	public class LeaderboardScoringInfo
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x0004FABA File Offset: 0x0004DCBA
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x0004FAC2 File Offset: 0x0004DCC2
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x0004FACA File Offset: 0x0004DCCA
		public string Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06003DAF RID: 15791 RVA: 0x0004FAD2 File Offset: 0x0004DCD2
		public DateTime? When
		{
			get
			{
				return this._when;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06003DB0 RID: 15792 RVA: 0x0004FADA File Offset: 0x0004DCDA
		private bool? IsNew
		{
			get
			{
				return this._isNew;
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x001584FC File Offset: 0x001566FC
		private LeaderboardScoringInfo(Builder<LeaderboardScoringInfo> builder)
		{
			LeaderboardScoringInfo.Builder builder2 = builder as LeaderboardScoringInfo.Builder;
			this._rank = builder2.GetRank;
			this._score = builder2.GetScore;
			this._context = builder2.GetContext;
			this._when = builder2.GetWhen;
		}

		// Token: 0x04002F73 RID: 12147
		private int _score = -1;

		// Token: 0x04002F74 RID: 12148
		private string _context;

		// Token: 0x04002F75 RID: 12149
		private int _rank = -1;

		// Token: 0x04002F76 RID: 12150
		private DateTime? _when;

		// Token: 0x04002F77 RID: 12151
		private bool? _isNew;

		// Token: 0x020008E0 RID: 2272
		public class Builder : Builder<LeaderboardScoringInfo>
		{
			// Token: 0x06003DB2 RID: 15794 RVA: 0x0004FAE2 File Offset: 0x0004DCE2
			public Builder()
			{
			}

			// Token: 0x06003DB3 RID: 15795 RVA: 0x00158554 File Offset: 0x00156754
			public Builder(ApiLeaderboardGetRankAndScoreResponse raw)
			{
				this._score = raw.data.user.score;
				this._context = raw.data.user.context;
				this._rank = raw.data.user.rank;
				this._when = ((raw.data.user.when == null) ? null : new DateTime?(DateTime.Parse(raw.data.user.when)));
			}

			// Token: 0x06003DB4 RID: 15796 RVA: 0x001585F4 File Offset: 0x001567F4
			public override Builder<LeaderboardScoringInfo>.BuilderErrors[] Validate()
			{
				List<Builder<LeaderboardScoringInfo>.BuilderErrors> list = null;
				if (this._context != null && this._context.Length > 200)
				{
					if (list == null)
					{
						list = new List<Builder<LeaderboardScoringInfo>.BuilderErrors>();
					}
					list.Add(new Builder<LeaderboardScoringInfo>.BuilderErrors("Context", "\"context\" length must not exceed 200 characters"));
				}
				if (list != null)
				{
					return list.ToArray();
				}
				return null;
			}

			// Token: 0x06003DB5 RID: 15797 RVA: 0x0004FAF8 File Offset: 0x0004DCF8
			public LeaderboardScoringInfo.Builder Score(int score)
			{
				this._score = score;
				return this;
			}

			// Token: 0x06003DB6 RID: 15798 RVA: 0x0004FB02 File Offset: 0x0004DD02
			public LeaderboardScoringInfo.Builder Context(string context)
			{
				this._context = context;
				return this;
			}

			// Token: 0x06003DB7 RID: 15799 RVA: 0x0004FB0C File Offset: 0x0004DD0C
			public LeaderboardScoringInfo.Builder Rank(int rank)
			{
				this._rank = rank;
				return this;
			}

			// Token: 0x06003DB8 RID: 15800 RVA: 0x0004FB16 File Offset: 0x0004DD16
			public LeaderboardScoringInfo.Builder When(DateTime? when)
			{
				this._when = when;
				return this;
			}

			// Token: 0x06003DB9 RID: 15801 RVA: 0x0004FB20 File Offset: 0x0004DD20
			public LeaderboardScoringInfo.Builder IsNew(bool isNew)
			{
				this._isNew = new bool?(isNew);
				return this;
			}

			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x06003DBA RID: 15802 RVA: 0x0004FB2F File Offset: 0x0004DD2F
			public int GetScore
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x06003DBB RID: 15803 RVA: 0x0004FB37 File Offset: 0x0004DD37
			public string GetContext
			{
				get
				{
					return this._context;
				}
			}

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x06003DBC RID: 15804 RVA: 0x0004FB3F File Offset: 0x0004DD3F
			public int GetRank
			{
				get
				{
					return this._rank;
				}
			}

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x06003DBD RID: 15805 RVA: 0x0004FB47 File Offset: 0x0004DD47
			public DateTime? GetWhen
			{
				get
				{
					return this._when;
				}
			}

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x06003DBE RID: 15806 RVA: 0x0004FB4F File Offset: 0x0004DD4F
			public bool? GetIsNew
			{
				get
				{
					return this._isNew;
				}
			}

			// Token: 0x04002F78 RID: 12152
			private int _score = -1;

			// Token: 0x04002F79 RID: 12153
			private string _context;

			// Token: 0x04002F7A RID: 12154
			private int _rank = -1;

			// Token: 0x04002F7B RID: 12155
			private DateTime? _when;

			// Token: 0x04002F7C RID: 12156
			private bool? _isNew;
		}
	}
}
