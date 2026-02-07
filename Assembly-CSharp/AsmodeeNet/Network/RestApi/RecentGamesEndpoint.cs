using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200091E RID: 2334
	public class RecentGamesEndpoint : Endpoint<RecentGame[]>
	{
		// Token: 0x06003F22 RID: 16162 RVA: 0x0005088A File Offset: 0x0004EA8A
		public RecentGamesEndpoint(OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			base._URL = "/main/v1/user/me/lastgames";
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0015B25C File Offset: 0x0015945C
		public RecentGamesEndpoint(string game, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastgames/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x000508BC File Offset: 0x0004EABC
		public RecentGamesEndpoint(GameStatus gameStatus, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			base._URL = string.Format("/main/v1/user/me/lastgames?status={0}", gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0015B2B4 File Offset: 0x001594B4
		public RecentGamesEndpoint(string game, GameStatus gameStatus, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastgames/{0}?status={1}", game, gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0015B310 File Offset: 0x00159510
		protected override void ProcessResponse(Action<RecentGame[], WebError> onCompletion)
		{
			RecentGame[] array = JsonUtility.FromJson<ApiRecentGameResponse>(base._HTTPResponse.DataAsText).data.games.Select((ApiRecentGameResponse.Data.Game x) => new RecentGame(x)).ToArray<RecentGame>();
			if (onCompletion != null)
			{
				onCompletion(array, null);
			}
		}
	}
}
