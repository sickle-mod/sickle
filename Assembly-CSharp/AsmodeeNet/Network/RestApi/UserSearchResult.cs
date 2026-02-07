using System;
using System.Linq;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008FE RID: 2302
	[Serializable]
	public class UserSearchResult
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06003EAB RID: 16043 RVA: 0x00050521 File Offset: 0x0004E721
		public int UserId
		{
			get
			{
				return this._userId;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06003EAC RID: 16044 RVA: 0x00050529 File Offset: 0x0004E729
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06003EAD RID: 16045 RVA: 0x00050531 File Offset: 0x0004E731
		public string[] Features
		{
			get
			{
				return this._features;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06003EAE RID: 16046 RVA: 0x00050539 File Offset: 0x0004E739
		public UserSearchResult.BoardGame[] Boardgames
		{
			get
			{
				return this._boardGames;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x00050541 File Offset: 0x0004E741
		public UserSearchResult.OnlineGame[] Onlinegames
		{
			get
			{
				return this._onlineGames;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x00050549 File Offset: 0x0004E749
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00050551 File Offset: 0x0004E751
		public UserSearchResult(int userId, string loginName, string avatar, string[] features, UserSearchResult.BoardGame[] boardGames, UserSearchResult.OnlineGame[] onlineGames)
		{
			this._loginName = loginName;
			this._userId = userId;
			this._avatar = avatar;
			this._features = features;
			this._boardGames = boardGames;
			this._onlineGames = onlineGames;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00159BA0 File Offset: 0x00157DA0
		public UserSearchResult(ApiSearchUserResponse.Data.User apiSearchUserResponseUser)
		{
			this._userId = apiSearchUserResponseUser.user_id;
			this._loginName = apiSearchUserResponseUser.login_name;
			this._avatar = apiSearchUserResponseUser.avatar;
			this._features = ((apiSearchUserResponseUser.features == null) ? null : (apiSearchUserResponseUser.features.Clone() as string[]));
			UserSearchResult.BoardGame[] array;
			if (apiSearchUserResponseUser.boardgames != null)
			{
				array = apiSearchUserResponseUser.boardgames.Select((ApiSearchUserResponse.Data.User.BoardGame y) => new UserSearchResult.BoardGame(y.code, y.name, (y.registered_date == null) ? null : new DateTime?(DateTime.Parse(y.registered_date)))).ToArray<UserSearchResult.BoardGame>();
			}
			else
			{
				array = null;
			}
			this._boardGames = array;
			UserSearchResult.OnlineGame[] array2;
			if (apiSearchUserResponseUser.onlinegames != null)
			{
				array2 = apiSearchUserResponseUser.onlinegames.Select((ApiSearchUserResponse.Data.User.OnlineGame z) => new UserSearchResult.OnlineGame(z.game, z.nbgames, z.karma, z.rankscore, z.rank, (z.lastgame == null) ? null : new DateTime?(DateTime.Parse(z.lastgame)), z.variant)).ToArray<UserSearchResult.OnlineGame>();
			}
			else
			{
				array2 = null;
			}
			this._onlineGames = array2;
		}

		// Token: 0x0400302E RID: 12334
		private string _loginName;

		// Token: 0x0400302F RID: 12335
		private int _userId = -1;

		// Token: 0x04003030 RID: 12336
		private string _avatar;

		// Token: 0x04003031 RID: 12337
		private string[] _features;

		// Token: 0x04003032 RID: 12338
		private UserSearchResult.BoardGame[] _boardGames;

		// Token: 0x04003033 RID: 12339
		private UserSearchResult.OnlineGame[] _onlineGames;

		// Token: 0x020008FF RID: 2303
		public class BoardGame
		{
			// Token: 0x06003EB3 RID: 16051 RVA: 0x0005058D File Offset: 0x0004E78D
			public BoardGame(string code, string name, DateTime? registeredDate)
			{
				this.code = code;
				this.name = name;
				this.registeredDate = registeredDate;
			}

			// Token: 0x04003034 RID: 12340
			public string code;

			// Token: 0x04003035 RID: 12341
			public string name;

			// Token: 0x04003036 RID: 12342
			public DateTime? registeredDate;
		}

		// Token: 0x02000900 RID: 2304
		public class OnlineGame
		{
			// Token: 0x06003EB4 RID: 16052 RVA: 0x000505AA File Offset: 0x0004E7AA
			public OnlineGame(string game, int nbGames, int karma, float rankScore, int rank, DateTime? lastGame, string variant)
			{
				this.game = game;
				this.nbGames = nbGames;
				this.karma = karma;
				this.rankScore = rankScore;
				this.rank = rank;
				this.lastGame = lastGame;
				this.variant = variant;
			}

			// Token: 0x04003037 RID: 12343
			public string game;

			// Token: 0x04003038 RID: 12344
			public int nbGames;

			// Token: 0x04003039 RID: 12345
			public int karma;

			// Token: 0x0400303A RID: 12346
			public float rankScore;

			// Token: 0x0400303B RID: 12347
			public int rank;

			// Token: 0x0400303C RID: 12348
			public DateTime? lastGame;

			// Token: 0x0400303D RID: 12349
			public string variant;
		}
	}
}
