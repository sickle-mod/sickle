using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008E9 RID: 2281
	public class RecentGame
	{
		// Token: 0x06003DED RID: 15853 RVA: 0x00158840 File Offset: 0x00156A40
		public RecentGame(ApiRecentGameResponse.Data.Game raw)
		{
			this._tableId = raw.table_id;
			this._date = raw.date;
			this._game = raw.game;
			this._options = raw.options;
			this._status = ((raw.status == null) ? null : new GameStatus?((GameStatus)Enum.Parse(typeof(GameStatus), raw.status)));
			this._variant = raw.variant;
			this._score = raw.score;
			RecentGame.OtherPlayer[] array;
			if (raw.other_players != null)
			{
				array = raw.other_players.Select((ApiRecentGameResponse.Data.Game.OtherPlayer x) => new RecentGame.OtherPlayer(x)).ToArray<RecentGame.OtherPlayer>();
			}
			else
			{
				array = null;
			}
			this._otherPlayers = array;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00158914 File Offset: 0x00156B14
		internal RecentGame(string tableId, string date, string game, string options, GameStatus? status, string variant, int score, RecentGame.OtherPlayer[] otherPlayers)
		{
			this._tableId = tableId;
			this._date = date;
			this._game = game;
			this._options = options;
			this._status = status;
			this._variant = variant;
			this._score = score;
			this._otherPlayers = otherPlayers;
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x0004FDE0 File Offset: 0x0004DFE0
		public string TableId
		{
			get
			{
				return this._tableId;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x0004FDE8 File Offset: 0x0004DFE8
		public string Date
		{
			get
			{
				return this._date;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x0004FDF0 File Offset: 0x0004DFF0
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		public string Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x0004FE00 File Offset: 0x0004E000
		public GameStatus? Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x0004FE08 File Offset: 0x0004E008
		public string Variant
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x0004FE10 File Offset: 0x0004E010
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x0004FE18 File Offset: 0x0004E018
		public RecentGame.OtherPlayer[] OtherPlayers
		{
			get
			{
				return this._otherPlayers;
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00158964 File Offset: 0x00156B64
		public override bool Equals(object obj)
		{
			RecentGame recentGame = obj as RecentGame;
			if (recentGame == null)
			{
				return false;
			}
			if (this.TableId == recentGame.TableId && this.Date == recentGame.Date && this.Game == recentGame.Game && this.Options == recentGame.Options)
			{
				GameStatus? status = this.Status;
				GameStatus? status2 = recentGame.Status;
				if (((status.GetValueOrDefault() == status2.GetValueOrDefault()) & (status != null == (status2 != null))) && this.Variant == recentGame.Variant && this.Score == recentGame.Score)
				{
					return this.OtherPlayers.Diff(recentGame.OtherPlayers).Count<RecentGame.OtherPlayer>() == 0;
				}
			}
			return false;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00158A3C File Offset: 0x00156C3C
		public override int GetHashCode()
		{
			return ((this.TableId == null) ? 0 : this.TableId.GetHashCode()) ^ ((this.Date == null) ? 0 : this.Date.GetHashCode()) ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ ((this.Options == null) ? 0 : this.Options.GetHashCode()) ^ (int)((this.Status == null) ? GameStatus.reserved : this.Status.Value) ^ ((this.Variant == null) ? 0 : this.Variant.GetHashCode()) ^ this.Score ^ this.OtherPlayers.GetHashCode();
		}

		// Token: 0x04002F9B RID: 12187
		private string _tableId;

		// Token: 0x04002F9C RID: 12188
		private string _date;

		// Token: 0x04002F9D RID: 12189
		private string _game;

		// Token: 0x04002F9E RID: 12190
		private string _options;

		// Token: 0x04002F9F RID: 12191
		private GameStatus? _status;

		// Token: 0x04002FA0 RID: 12192
		private string _variant;

		// Token: 0x04002FA1 RID: 12193
		private int _score;

		// Token: 0x04002FA2 RID: 12194
		private RecentGame.OtherPlayer[] _otherPlayers;

		// Token: 0x020008EA RID: 2282
		public class OtherPlayer
		{
			// Token: 0x06003DF9 RID: 15865 RVA: 0x0004FE20 File Offset: 0x0004E020
			public OtherPlayer(ApiRecentGameResponse.Data.Game.OtherPlayer raw)
			{
				this._id = raw.id;
				this._loginName = raw.login_name;
				this._avatar = raw.avatar;
				this._score = raw.score;
			}

			// Token: 0x06003DFA RID: 15866 RVA: 0x0004FE58 File Offset: 0x0004E058
			internal OtherPlayer(int id, string loginName, string avatar, int score)
			{
				this._id = id;
				this._loginName = loginName;
				this._avatar = avatar;
				this._score = score;
			}

			// Token: 0x17000563 RID: 1379
			// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0004FE7D File Offset: 0x0004E07D
			public int Id
			{
				get
				{
					return this._id;
				}
			}

			// Token: 0x17000564 RID: 1380
			// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0004FE85 File Offset: 0x0004E085
			public string LoginName
			{
				get
				{
					return this._loginName;
				}
			}

			// Token: 0x17000565 RID: 1381
			// (get) Token: 0x06003DFD RID: 15869 RVA: 0x0004FE8D File Offset: 0x0004E08D
			public string Avatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x17000566 RID: 1382
			// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0004FE95 File Offset: 0x0004E095
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x06003DFF RID: 15871 RVA: 0x00158AF0 File Offset: 0x00156CF0
			public override bool Equals(object obj)
			{
				RecentGame.OtherPlayer otherPlayer = obj as RecentGame.OtherPlayer;
				return otherPlayer != null && (this.Id == otherPlayer.Id && this.LoginName == otherPlayer.LoginName && this.Avatar == otherPlayer.Avatar) && this.Score == otherPlayer.Score;
			}

			// Token: 0x06003E00 RID: 15872 RVA: 0x0004FE9D File Offset: 0x0004E09D
			public override int GetHashCode()
			{
				return this.Id ^ ((this.LoginName == null) ? 0 : this.LoginName.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ this.Score;
			}

			// Token: 0x04002FA3 RID: 12195
			private int _id;

			// Token: 0x04002FA4 RID: 12196
			private string _loginName;

			// Token: 0x04002FA5 RID: 12197
			private string _avatar;

			// Token: 0x04002FA6 RID: 12198
			private int _score;
		}
	}
}
