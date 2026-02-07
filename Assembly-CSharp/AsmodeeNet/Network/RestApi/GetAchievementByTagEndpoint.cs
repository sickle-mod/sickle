using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200087E RID: 2174
	public class GetAchievementByTagEndpoint : Endpoint<Achievement>
	{
		// Token: 0x06003CF0 RID: 15600 RVA: 0x00156F90 File Offset: 0x00155190
		public GetAchievementByTagEndpoint(string game, string tag, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (string.IsNullOrEmpty(tag))
			{
				throw new ArgumentException("'tag' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievement/{1}", game, tag);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00156F58 File Offset: 0x00155158
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
