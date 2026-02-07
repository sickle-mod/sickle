using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008D9 RID: 2265
	[Serializable]
	public class FetchGameRank
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x0004F90C File Offset: 0x0004DB0C
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06003D83 RID: 15747 RVA: 0x0004F914 File Offset: 0x0004DB14
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x0004F91C File Offset: 0x0004DB1C
		public int NbGames
		{
			get
			{
				return this._nbGames;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06003D85 RID: 15749 RVA: 0x0004F924 File Offset: 0x0004DB24
		public int Karma
		{
			get
			{
				return this._karma;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0004F92C File Offset: 0x0004DB2C
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06003D87 RID: 15751 RVA: 0x0004F934 File Offset: 0x0004DB34
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00157FCC File Offset: 0x001561CC
		public FetchGameRank(ApiFetchGameRankResponse.Data.Rank raw)
		{
			this._id = raw.id;
			this._name = raw.name;
			this._nbGames = raw.nbgames;
			this._karma = raw.karma;
			this._score = raw.score;
			this._rank = raw.rank;
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00158028 File Offset: 0x00156228
		public override bool Equals(object obj)
		{
			FetchGameRank fetchGameRank = obj as FetchGameRank;
			return fetchGameRank != null && (this.Id == fetchGameRank.Id && this.Name == fetchGameRank.Name && this.NbGames == fetchGameRank.NbGames && this.Karma == fetchGameRank.Karma && this.Score == fetchGameRank.Score) && this.Rank == fetchGameRank.Rank;
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0004F93C File Offset: 0x0004DB3C
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ this.NbGames ^ this.Karma ^ this.Score ^ this.Rank;
		}

		// Token: 0x04002F53 RID: 12115
		[SerializeField]
		private int _id;

		// Token: 0x04002F54 RID: 12116
		[SerializeField]
		private string _name;

		// Token: 0x04002F55 RID: 12117
		[SerializeField]
		private int _nbGames;

		// Token: 0x04002F56 RID: 12118
		[SerializeField]
		private int _karma;

		// Token: 0x04002F57 RID: 12119
		[SerializeField]
		private int _score;

		// Token: 0x04002F58 RID: 12120
		[SerializeField]
		private int _rank;
	}
}
