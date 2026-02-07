using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000920 RID: 2336
	public class RecentOpponentsEndpoint : Endpoint<RecentOpponent[]>
	{
		// Token: 0x06003F2A RID: 16170 RVA: 0x0005090D File Offset: 0x0004EB0D
		public RecentOpponentsEndpoint(OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			base._URL = "/main/v1/user/me/lastopponents";
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0015B36C File Offset: 0x0015956C
		public RecentOpponentsEndpoint(string game, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastopponents/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0005093F File Offset: 0x0004EB3F
		public RecentOpponentsEndpoint(GameStatus gameStatus, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			base._URL = string.Format("/main/v1/user/me/lastopponents?status={0}", gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0015B3C4 File Offset: 0x001595C4
		public RecentOpponentsEndpoint(string game, GameStatus gameStatus, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastopponents/{0}?status={1}", game, gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0015B420 File Offset: 0x00159620
		protected override void ProcessResponse(Action<RecentOpponent[], WebError> onCompletion)
		{
			RecentOpponent[] array = JsonUtility.FromJson<ApiRecentOpponentsResponse>(base._HTTPResponse.DataAsText).data.opponents.Select((ApiRecentOpponentsResponse.Data.Opponent x) => new RecentOpponent(x)).ToArray<RecentOpponent>();
			if (onCompletion != null)
			{
				onCompletion(array, null);
			}
		}
	}
}
