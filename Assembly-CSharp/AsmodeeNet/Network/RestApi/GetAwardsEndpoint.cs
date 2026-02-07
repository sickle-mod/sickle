using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000881 RID: 2177
	public class GetAwardsEndpoint : EndpointWithPaginatedResponse<Award>
	{
		// Token: 0x06003CFA RID: 15610 RVA: 0x0004F6BB File Offset: 0x0004D8BB
		public GetAwardsEndpoint(int userId, string game = null, string type = null, int category = -1, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			this.Initialize(string.Format("/main/v1/user/{0}/awards", userId), game, type, category, offset, limit, oauthGate);
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0004F6E6 File Offset: 0x0004D8E6
		public GetAwardsEndpoint(string game = null, string type = null, int category = -1, int offset = -1, int limit = -1, OAuthGate oauthGate = null)
			: base(oauthGate, true)
		{
			this.Initialize("/main/v1/user/me/awards", game, type, category, offset, limit, oauthGate);
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x001572E0 File Offset: 0x001554E0
		private void Initialize(string baseUrl, string game, string type, int category, int offset, int limit, OAuthGate oauthGate)
		{
			base.DebugModuleName += ".User.Award";
			if (game == string.Empty || type == string.Empty)
			{
				throw new ArgumentException("\"game\" and \"type\" parameters can not be empty if specified");
			}
			base._URL = baseUrl;
			string text = "?";
			if (!string.IsNullOrEmpty(game))
			{
				text = text + "game=" + game + "&";
			}
			if (!string.IsNullOrEmpty(type))
			{
				text = text + "type=" + type + "&";
			}
			if (category >= 0)
			{
				text = text + "category=" + category.ToString() + "&";
			}
			if (offset >= 0)
			{
				text = text + "offset=" + offset.ToString() + "&";
			}
			if (limit >= 0)
			{
				text = text + "limit=" + limit.ToString() + "&";
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x001573E4 File Offset: 0x001555E4
		protected override void ProcessResponse(Action<PaginatedResult<Award>, WebError> onCompletion)
		{
			ApiGetAwardListResponse apiGetAwardListResponse = JsonUtility.FromJson<ApiGetAwardListResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<Award> paginatedResult = new PaginatedResult<Award>(apiGetAwardListResponse.data.total, apiGetAwardListResponse.data.awards.Select((ApiGetAwardListResponse.Data.Award x) => new Award(x.id, x.tag, x.table_id, x.info_id, new DateTime?(DateTime.Parse(x.awarded_utc)))).ToArray<Award>(), base._LinkSetter(apiGetAwardListResponse.data._links.next), base._LinkSetter(apiGetAwardListResponse.data._links.prev), base._LinkSetter(apiGetAwardListResponse.data._links.first), base._LinkSetter(apiGetAwardListResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}
