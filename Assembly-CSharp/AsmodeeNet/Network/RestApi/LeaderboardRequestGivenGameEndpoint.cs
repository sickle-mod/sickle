using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200091B RID: 2331
	public class LeaderboardRequestGivenGameEndpoint : EndpointWithPaginatedResponse<GameLeaderboard.Player>
	{
		// Token: 0x06003F1A RID: 16154 RVA: 0x0015B0A4 File Offset: 0x001592A4
		public LeaderboardRequestGivenGameEndpoint(string game, string leaderboard, Period period, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (string.IsNullOrEmpty(leaderboard))
			{
				throw new ArgumentException("'leaderboard' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/leaderboards/{1}/{2}?offset={3}&limit={4}", new object[] { game, leaderboard, period, offset, limit });
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0015B124 File Offset: 0x00159324
		protected override void ProcessResponse(Action<PaginatedResult<GameLeaderboard.Player>, WebError> onCompletion)
		{
			ApiLeaderboardRequestGivenGameResponse apiLeaderboardRequestGivenGameResponse = JsonUtility.FromJson<ApiLeaderboardRequestGivenGameResponse>(base._HTTPResponse.DataAsText);
			GameLeaderboard gameLeaderboard = new GameLeaderboard(apiLeaderboardRequestGivenGameResponse.data.leaderboard);
			LeaderboardRequestGivenGameEndpoint.GameLeaderboardPaginatedResult<GameLeaderboard.Player> gameLeaderboardPaginatedResult = new LeaderboardRequestGivenGameEndpoint.GameLeaderboardPaginatedResult<GameLeaderboard.Player>(apiLeaderboardRequestGivenGameResponse.data.leaderboard.total, gameLeaderboard.Players.Clone() as GameLeaderboard.Player[], base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.next), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.prev), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.first), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.last), gameLeaderboard);
			if (onCompletion != null)
			{
				onCompletion(gameLeaderboardPaginatedResult, null);
			}
		}

		// Token: 0x0200091C RID: 2332
		public class GameLeaderboardPaginatedResult<T> : PaginatedResult<T> where T : class
		{
			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x06003F1C RID: 16156 RVA: 0x000507EE File Offset: 0x0004E9EE
			// (set) Token: 0x06003F1D RID: 16157 RVA: 0x000507F6 File Offset: 0x0004E9F6
			public GameLeaderboard GameLeaderboard { get; private set; }

			// Token: 0x06003F1E RID: 16158 RVA: 0x000507FF File Offset: 0x0004E9FF
			public GameLeaderboardPaginatedResult(int totalElement, T[] elements, Action<Action<PaginatedResult<T>, WebError>> next, Action<Action<PaginatedResult<T>, WebError>> prev, Action<Action<PaginatedResult<T>, WebError>> first, Action<Action<PaginatedResult<T>, WebError>> last, GameLeaderboard leaderboard)
				: base(totalElement, elements, next, prev, first, last)
			{
				this.GameLeaderboard = leaderboard;
			}
		}
	}
}
