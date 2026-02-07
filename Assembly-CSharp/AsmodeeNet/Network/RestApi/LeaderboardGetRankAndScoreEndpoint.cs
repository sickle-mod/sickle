using System;
using System.Collections;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000917 RID: 2327
	public class LeaderboardGetRankAndScoreEndpoint : Endpoint<LeaderboardScoringInfo>
	{
		// Token: 0x06003F0C RID: 16140 RVA: 0x0015AD1C File Offset: 0x00158F1C
		public LeaderboardGetRankAndScoreEndpoint(int userId, string game, string leaderboard, Period period, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Scores";
			base._URL = string.Format("/main/v1/user/{0}/scores/{1}/{2}/{3}", new object[]
			{
				userId,
				game,
				leaderboard,
				period.ToString().ToLower()
			});
			base._Parameters = new Hashtable
			{
				{ "userId", userId },
				{ "game", game },
				{ "leaderboard", leaderboard },
				{ "period", period }
			};
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0015ADCC File Offset: 0x00158FCC
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
