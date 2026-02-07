using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200090D RID: 2317
	public class FetchRankEndpoint : Endpoint<FetchRank>
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x0005072B File Offset: 0x0004E92B
		// (set) Token: 0x06003EF2 RID: 16114 RVA: 0x00050733 File Offset: 0x0004E933
		public FetchRank FetchRankResult { get; private set; }

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0015A728 File Offset: 0x00158928
		public FetchRankEndpoint(int user, string gameNameOrVariant, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.FetchRank";
			if (string.IsNullOrEmpty(gameNameOrVariant))
			{
				throw new ArgumentException("'gameNameOrVariant' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/{0}/rank/{1}", user, gameNameOrVariant);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0015A784 File Offset: 0x00158984
		protected override void ProcessResponse(Action<FetchRank, WebError> onCompletion)
		{
			FetchRank fetchRank = new FetchRank(JsonUtility.FromJson<ApiFetchRankResponse>(base._HTTPResponse.DataAsText).data.user);
			if (onCompletion != null)
			{
				onCompletion(fetchRank, null);
			}
		}
	}
}
