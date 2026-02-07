using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008DB RID: 2267
	public class GameLeaderboard
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x0004F9EC File Offset: 0x0004DBEC
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06003D97 RID: 15767 RVA: 0x0004F9F4 File Offset: 0x0004DBF4
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x0004F9FC File Offset: 0x0004DBFC
		public Period Period
		{
			get
			{
				return this._period;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0004FA04 File Offset: 0x0004DC04
		public GameLeaderboard.Player[] Players
		{
			get
			{
				return this._players;
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0004FA0C File Offset: 0x0004DC0C
		internal GameLeaderboard(string id, string game, Period period, GameLeaderboard.Player[] players)
		{
			this._id = id;
			this._game = game;
			this._period = period;
			this._players = players;
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x001581EC File Offset: 0x001563EC
		public GameLeaderboard(ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard raw)
		{
			this._id = raw.id;
			this._game = raw.game;
			this._period = (Period)Enum.Parse(typeof(Period), raw.period);
			this._players = raw.players.Select((ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Player x) => new GameLeaderboard.Player(x)).ToArray<GameLeaderboard.Player>();
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0015826C File Offset: 0x0015646C
		public override bool Equals(object obj)
		{
			GameLeaderboard gameLeaderboard = obj as GameLeaderboard;
			return gameLeaderboard != null && (this.Id == gameLeaderboard.Id && this.Game == gameLeaderboard.Game && this.Period == gameLeaderboard.Period) && this.Players.Diff(gameLeaderboard.Players).Count<GameLeaderboard.Player>() == 0;
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x001582D4 File Offset: 0x001564D4
		public override int GetHashCode()
		{
			return ((this.Id == null) ? 0 : this.Id.GetHashCode()) ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ (int)this.Period ^ this.Players.GetHashCode();
		}

		// Token: 0x04002F60 RID: 12128
		private string _id;

		// Token: 0x04002F61 RID: 12129
		private string _game;

		// Token: 0x04002F62 RID: 12130
		private Period _period;

		// Token: 0x04002F63 RID: 12131
		private GameLeaderboard.Player[] _players;

		// Token: 0x020008DC RID: 2268
		public class Player
		{
			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x06003D9E RID: 15774 RVA: 0x0004FA31 File Offset: 0x0004DC31
			public int Rank
			{
				get
				{
					return this._rank;
				}
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x06003D9F RID: 15775 RVA: 0x0004FA39 File Offset: 0x0004DC39
			public int Id
			{
				get
				{
					return this._id;
				}
			}

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x0004FA41 File Offset: 0x0004DC41
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0004FA49 File Offset: 0x0004DC49
			public string Context
			{
				get
				{
					return this._context;
				}
			}

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x0004FA51 File Offset: 0x0004DC51
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x0004FA59 File Offset: 0x0004DC59
			public string Avatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x0004FA61 File Offset: 0x0004DC61
			public DateTime? When
			{
				get
				{
					return this._when;
				}
			}

			// Token: 0x06003DA5 RID: 15781 RVA: 0x00158324 File Offset: 0x00156524
			public override bool Equals(object obj)
			{
				GameLeaderboard.Player player = obj as GameLeaderboard.Player;
				return player != null && (this.Rank == player.Rank && this.Id == player.Id && this.Score == player.Score && this.Context == player.Context && this.Name == player.Name && this.Avatar == player.Avatar) && this.When == player.When;
			}

			// Token: 0x06003DA6 RID: 15782 RVA: 0x001583E8 File Offset: 0x001565E8
			public override int GetHashCode()
			{
				return this.Rank ^ this.Id ^ this.Score ^ ((this.Context == null) ? 0 : this.Context.GetHashCode()) ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ ((this.When == null) ? 0 : this.When.GetHashCode());
			}

			// Token: 0x06003DA7 RID: 15783 RVA: 0x00158478 File Offset: 0x00156678
			public Player(ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Player raw)
			{
				this._rank = raw.rank;
				this._id = raw.id;
				this._score = raw.score;
				this._context = raw.context;
				this._name = raw.name;
				this._avatar = raw.avatar;
				this._when = ((raw.when == null) ? null : new DateTime?(DateTime.Parse(raw.when)));
			}

			// Token: 0x06003DA8 RID: 15784 RVA: 0x0004FA69 File Offset: 0x0004DC69
			internal Player(int rank, int id, int score, string context, string name, string avatar, DateTime? when)
			{
				this._rank = rank;
				this._id = id;
				this._score = score;
				this._context = context;
				this._name = name;
				this._avatar = avatar;
				this._when = when;
			}

			// Token: 0x04002F64 RID: 12132
			private int _rank;

			// Token: 0x04002F65 RID: 12133
			private int _id;

			// Token: 0x04002F66 RID: 12134
			private int _score;

			// Token: 0x04002F67 RID: 12135
			private string _context;

			// Token: 0x04002F68 RID: 12136
			private string _name;

			// Token: 0x04002F69 RID: 12137
			private string _avatar;

			// Token: 0x04002F6A RID: 12138
			private DateTime? _when;
		}
	}
}
