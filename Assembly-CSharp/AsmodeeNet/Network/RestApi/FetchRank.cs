using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008DA RID: 2266
	[Serializable]
	public class FetchRank
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06003D8B RID: 15755 RVA: 0x0004F977 File Offset: 0x0004DB77
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0004F97F File Offset: 0x0004DB7F
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x0004F987 File Offset: 0x0004DB87
		public int NbGames
		{
			get
			{
				return this._nbgames;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x0004F98F File Offset: 0x0004DB8F
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x0004F997 File Offset: 0x0004DB97
		public int Karma
		{
			get
			{
				return this._karma;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x0004F99F File Offset: 0x0004DB9F
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x0004F9A7 File Offset: 0x0004DBA7
		public string Ranking
		{
			get
			{
				return this._ranking;
			}
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x0004F9AF File Offset: 0x0004DBAF
		public FetchRank(int id, string name, int nbGames, int rank, int karma, int score, string ranking)
		{
			this._id = id;
			this._name = name;
			this._nbgames = nbGames;
			this._rank = rank;
			this._karma = karma;
			this._score = score;
			this._ranking = ranking;
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0015809C File Offset: 0x0015629C
		public FetchRank(ApiFetchRankResponse.Data.User raw)
		{
			this._id = raw.id;
			this._name = raw.name;
			this._nbgames = raw.nbgames;
			this._rank = raw.rank;
			this._karma = raw.karma;
			this._score = raw.score;
			this._ranking = raw.ranking;
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x00158104 File Offset: 0x00156304
		public override bool Equals(object o)
		{
			FetchRank fetchRank = o as FetchRank;
			return fetchRank != null && (this.Id == fetchRank.Id && this.Name == fetchRank.Name && this.NbGames == fetchRank.NbGames && this.Rank == fetchRank.Rank && this.Karma == fetchRank.Karma && this.Score == fetchRank.Score) && this.Ranking == fetchRank.Ranking;
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0015818C File Offset: 0x0015638C
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ this.NbGames ^ this.Rank ^ this.Karma ^ this.Score ^ ((this.Ranking == null) ? 0 : this.Ranking.GetHashCode());
		}

		// Token: 0x04002F59 RID: 12121
		[SerializeField]
		private int _id;

		// Token: 0x04002F5A RID: 12122
		[SerializeField]
		private string _name;

		// Token: 0x04002F5B RID: 12123
		[SerializeField]
		private int _nbgames;

		// Token: 0x04002F5C RID: 12124
		[SerializeField]
		private int _rank;

		// Token: 0x04002F5D RID: 12125
		[SerializeField]
		private int _karma;

		// Token: 0x04002F5E RID: 12126
		[SerializeField]
		private int _score;

		// Token: 0x04002F5F RID: 12127
		[SerializeField]
		private string _ranking;
	}
}
