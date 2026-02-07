using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200087D RID: 2173
	public class GetAchievementByIdEndpoint : Endpoint<Achievement>
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x00156EFC File Offset: 0x001550FC
		public GetAchievementByIdEndpoint(string game, int id, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievement/{1}", game, id);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00156F58 File Offset: 0x00155158
		protected override void ProcessResponse(Action<Achievement, WebError> onCompletion)
		{
			Achievement achievement = new Achievement(JsonUtility.FromJson<ApiGetAchievementResponse>(base._HTTPResponse.DataAsText).data.achievement);
			if (onCompletion != null)
			{
				onCompletion(achievement, null);
			}
		}
	}
}
