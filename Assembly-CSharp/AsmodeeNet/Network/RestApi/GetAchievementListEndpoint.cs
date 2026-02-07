using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200087F RID: 2175
	public class GetAchievementListEndpoint : EndpointWithPaginatedResponse<Achievement>
	{
		// Token: 0x06003CF2 RID: 15602 RVA: 0x00156FFC File Offset: 0x001551FC
		public GetAchievementListEndpoint(string game, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?offset={1}&limit={2}", game, offset, limit);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00157060 File Offset: 0x00155260
		public GetAchievementListEndpoint(int category, string game, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?category={1}&offset={2}&limit={3}", new object[] { game, category, offset, limit });
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x001570DC File Offset: 0x001552DC
		public GetAchievementListEndpoint(string game, AchievementType type, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			if (type == AchievementType.Null)
			{
				throw new ArgumentException("'type' parameter value cannot be 'Null'");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?type={1}&offset={2}&limit={3}", new object[]
			{
				game,
				type.ToString().ToLower(),
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00157174 File Offset: 0x00155374
		public GetAchievementListEndpoint(int category, string game, AchievementType type, int offset = 0, int limit = 20, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			if (type == AchievementType.Null)
			{
				throw new ArgumentException("'type' parameter value cannot be 'Null'");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?type={1}&category={2}&offset={3}&limit={4}", new object[]
			{
				game,
				type.ToString().ToLower(),
				category,
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00157218 File Offset: 0x00155418
		protected override void ProcessResponse(Action<PaginatedResult<Achievement>, WebError> onCompletion)
		{
			ApiGetAchievementListResponse apiGetAchievementListResponse = JsonUtility.FromJson<ApiGetAchievementListResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<Achievement> paginatedResult = new PaginatedResult<Achievement>(apiGetAchievementListResponse.data.total, apiGetAchievementListResponse.data.achievements.Select((JsonAchievement x) => new Achievement(x)).ToArray<Achievement>(), base._LinkSetter(apiGetAchievementListResponse.data._links.next), base._LinkSetter(apiGetAchievementListResponse.data._links.prev), base._LinkSetter(apiGetAchievementListResponse.data._links.first), base._LinkSetter(apiGetAchievementListResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}
