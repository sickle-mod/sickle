using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008EC RID: 2284
	public class RecentOpponent
	{
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x0004FEEE File Offset: 0x0004E0EE
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x0004FEF6 File Offset: 0x0004E0F6
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x0004FEFE File Offset: 0x0004E0FE
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x0004FF06 File Offset: 0x0004E106
		public string LastGameDate
		{
			get
			{
				return this._lastGameDate;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x0004FF0E File Offset: 0x0004E10E
		public RecentOpponent.Game[] Games
		{
			get
			{
				return this._games;
			}
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00158B50 File Offset: 0x00156D50
		public RecentOpponent(ApiRecentOpponentsResponse.Data.Opponent raw)
		{
			this._id = raw.id;
			this._loginName = raw.login_name;
			this._avatar = raw.avatar;
			this._lastGameDate = raw.last_game_date;
			this._games = raw.games.Select((ApiRecentOpponentsResponse.Data.Opponent.Game x) => new RecentOpponent.Game(x)).ToArray<RecentOpponent.Game>();
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0004FF16 File Offset: 0x0004E116
		internal RecentOpponent(int id, string loginName, string avatar, string lastGameDate, RecentOpponent.Game[] games)
		{
			this._id = id;
			this._loginName = loginName;
			this._avatar = avatar;
			this._lastGameDate = lastGameDate;
			this._games = games;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00158BC8 File Offset: 0x00156DC8
		public override bool Equals(object obj)
		{
			RecentOpponent recentOpponent = obj as RecentOpponent;
			return recentOpponent != null && (this.Id == recentOpponent.Id && this.LoginName == recentOpponent.LoginName && this.Avatar == recentOpponent.Avatar && this.LastGameDate == recentOpponent.LastGameDate) && this.Games.Diff(recentOpponent.Games).Count<RecentOpponent.Game>() == 0;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00158C44 File Offset: 0x00156E44
		public override int GetHashCode()
		{
			return this.Id ^ ((this.LoginName == null) ? 0 : this.LoginName.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ ((this.LastGameDate == null) ? 0 : this.LastGameDate.GetHashCode()) ^ this.Games.GetHashCode();
		}

		// Token: 0x04002FA9 RID: 12201
		private int _id;

		// Token: 0x04002FAA RID: 12202
		private string _loginName;

		// Token: 0x04002FAB RID: 12203
		private string _avatar;

		// Token: 0x04002FAC RID: 12204
		private string _lastGameDate;

		// Token: 0x04002FAD RID: 12205
		private RecentOpponent.Game[] _games;

		// Token: 0x020008ED RID: 2285
		public class Game
		{
			// Token: 0x1700056C RID: 1388
			// (get) Token: 0x06003E0D RID: 15885 RVA: 0x0004FF43 File Offset: 0x0004E143
			public string TableId
			{
				get
				{
					return this._tableId;
				}
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x06003E0E RID: 15886 RVA: 0x0004FF4B File Offset: 0x0004E14B
			public string GameName
			{
				get
				{
					return this._gameName;
				}
			}

			// Token: 0x1700056E RID: 1390
			// (get) Token: 0x06003E0F RID: 15887 RVA: 0x0004FF53 File Offset: 0x0004E153
			public string Date
			{
				get
				{
					return this._date;
				}
			}

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x06003E10 RID: 15888 RVA: 0x0004FF5B File Offset: 0x0004E15B
			public GameStatus? Status
			{
				get
				{
					return this._status;
				}
			}

			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x06003E11 RID: 15889 RVA: 0x0004FF63 File Offset: 0x0004E163
			public int OtherScore
			{
				get
				{
					return this._otherScore;
				}
			}

			// Token: 0x17000571 RID: 1393
			// (get) Token: 0x06003E12 RID: 15890 RVA: 0x0004FF6B File Offset: 0x0004E16B
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x06003E13 RID: 15891 RVA: 0x00158CA8 File Offset: 0x00156EA8
			public Game(ApiRecentOpponentsResponse.Data.Opponent.Game raw)
			{
				this._tableId = raw.table_id;
				this._gameName = raw.game;
				this._date = raw.date;
				this._status = ((raw.status == null) ? null : new GameStatus?((GameStatus)Enum.Parse(typeof(GameStatus), raw.status)));
				this._otherScore = raw.other_score;
				this._score = raw.score;
			}

			// Token: 0x06003E14 RID: 15892 RVA: 0x0004FF73 File Offset: 0x0004E173
			internal Game(string tableId, string gameName, string date, GameStatus? status, int otherScore, int score)
			{
				this._tableId = tableId;
				this._gameName = gameName;
				this._date = date;
				this._status = status;
				this._otherScore = otherScore;
				this._score = score;
			}

			// Token: 0x06003E15 RID: 15893 RVA: 0x00158D30 File Offset: 0x00156F30
			public override bool Equals(object obj)
			{
				RecentOpponent.Game game = obj as RecentOpponent.Game;
				if (game == null)
				{
					return false;
				}
				if (this.TableId == game.TableId && this.GameName == game.GameName && this.Date == game.Date)
				{
					GameStatus? status = this.Status;
					GameStatus? status2 = game.Status;
					if (((status.GetValueOrDefault() == status2.GetValueOrDefault()) & (status != null == (status2 != null))) && this.OtherScore == game.OtherScore)
					{
						return this.Score == game.Score;
					}
				}
				return false;
			}

			// Token: 0x06003E16 RID: 15894 RVA: 0x00158DD4 File Offset: 0x00156FD4
			public override int GetHashCode()
			{
				return ((this.TableId == null) ? 0 : this.TableId.GetHashCode()) ^ ((this.GameName == null) ? 0 : this.GameName.GetHashCode()) ^ ((this.Date == null) ? 0 : this.Date.GetHashCode()) ^ (int)this.Status.Value ^ this.OtherScore ^ this.Score;
			}

			// Token: 0x04002FAE RID: 12206
			private string _tableId;

			// Token: 0x04002FAF RID: 12207
			private string _gameName;

			// Token: 0x04002FB0 RID: 12208
			private string _date;

			// Token: 0x04002FB1 RID: 12209
			private GameStatus? _status;

			// Token: 0x04002FB2 RID: 12210
			private int _otherScore;

			// Token: 0x04002FB3 RID: 12211
			private int _score;
		}
	}
}
