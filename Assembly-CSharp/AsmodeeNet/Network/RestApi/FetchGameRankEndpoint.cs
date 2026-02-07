using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200090B RID: 2315
	public class FetchGameRankEndpoint : EndpointWithPaginatedResponse<FetchGameRank>
	{
		// Token: 0x06003EEC RID: 16108 RVA: 0x0015A5E8 File Offset: 0x001587E8
		public FetchGameRankEndpoint(string gameVariant, RankingType rankingType, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".Game.FetchRank";
			if (string.IsNullOrEmpty(gameVariant))
			{
				throw new ArgumentException("'gameVariant' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/rank/{1}?offset={2}&limit={3}", new object[] { gameVariant, rankingType, offset, limit });
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0015A664 File Offset: 0x00158864
		protected override void ProcessResponse(Action<PaginatedResult<FetchGameRank>, WebError> onCompletion)
		{
			ApiFetchGameRankResponse apiFetchGameRankResponse = JsonUtility.FromJson<ApiFetchGameRankResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<FetchGameRank> paginatedResult = new PaginatedResult<FetchGameRank>(apiFetchGameRankResponse.data.total, apiFetchGameRankResponse.data.ranks.Select((ApiFetchGameRankResponse.Data.Rank x) => new FetchGameRank(x)).ToArray<FetchGameRank>(), base._LinkSetter(apiFetchGameRankResponse.data._links.next), base._LinkSetter(apiFetchGameRankResponse.data._links.prev), base._LinkSetter(apiFetchGameRankResponse.data._links.first), base._LinkSetter(apiFetchGameRankResponse.data._links.last));
			onCompletion(paginatedResult, null);
		}
	}
}
