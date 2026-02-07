using System;
using System.Collections;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200091A RID: 2330
	public class LeaderboardPublishScoreForOnePeriodEndpoint : Endpoint<LeaderboardScoringInfo>
	{
		// Token: 0x06003F18 RID: 16152 RVA: 0x0015AFE8 File Offset: 0x001591E8
		public LeaderboardPublishScoreForOnePeriodEndpoint(string game, string leaderboard, Period period, LeaderboardScoringInfo userRankAndScore, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			if (userRankAndScore == null || userRankAndScore.Score == -1)
			{
				throw new ArgumentException("A score must be specified to call this endpoint");
			}
			base.DebugModuleName += ".User.Scores";
			base._URL = string.Format("/main/v1/user/me/scores/{0}/{1}/{2}", game, leaderboard, period.ToString().ToLower());
			base._Parameters = new Hashtable();
			base._Parameters.Add("score", userRankAndScore.Score);
			if (!string.IsNullOrEmpty(userRankAndScore.Context))
			{
				base._Parameters.Add("context", userRankAndScore.Context);
			}
			base._HttpMethod = HTTPMethods.Put;
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0015ADCC File Offset: 0x00158FCC
		protected override void ProcessResponse(Action<LeaderboardScoringInfo, WebError> onCompletion)
		{
			Either<LeaderboardScoringInfo, Builder<LeaderboardScoringInfo>.BuilderErrors[]> either = new LeaderboardScoringInfo.Builder(JsonUtility.FromJson<ApiLeaderboardGetRankAndScoreResponse>(base._HTTPResponse.DataAsText)).Build(false);
			if (onCompletion != null)
			{
				if (either.Error == null)
				{
					onCompletion(either.Value, null);
					return;
				}
				onCompletion(null, new LeaderboardScoringInfoError(either.Error));
			}
		}
	}
}
