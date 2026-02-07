using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000918 RID: 2328
	public class LeaderboardPublishScoreForAllPeriodEndpoint : Endpoint<LeaderboardPublishScoreForAllPeriodEndpoint.Result>
	{
		// Token: 0x06003F0E RID: 16142 RVA: 0x0015AE20 File Offset: 0x00159020
		public LeaderboardPublishScoreForAllPeriodEndpoint(string game, string leaderboard, LeaderboardScoringInfo userRankAndScore, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			if (userRankAndScore == null || userRankAndScore.Score == -1)
			{
				throw new ArgumentException("A score must be specified to call this endpoint");
			}
			base.DebugModuleName += ".User.Scores";
			base._URL = string.Format("/main/v1/user/me/scores/{0}/{1}", game, leaderboard);
			base._Parameters = new Hashtable();
			base._Parameters.Add("score", userRankAndScore.Score);
			if (!string.IsNullOrEmpty(userRankAndScore.Context))
			{
				base._Parameters.Add("context", userRankAndScore.Context);
			}
			base._HttpMethod = HTTPMethods.Put;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0015AEC8 File Offset: 0x001590C8
		private LeaderboardScoringInfo _BuildLeaderboard(ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period raw, ref List<Builder<LeaderboardScoringInfo>.BuilderErrors> errors)
		{
			Either<LeaderboardScoringInfo, Builder<LeaderboardScoringInfo>.BuilderErrors[]> either = new LeaderboardScoringInfo.Builder().Score(raw.score).Context(raw.context).Rank(raw.rank)
				.When((raw.when == null) ? null : new DateTime?(DateTime.Parse(raw.when)))
				.IsNew(raw.isNew)
				.Build(false);
			if (either.Error != null)
			{
				errors.AddRange(either.Error);
			}
			return either.Value;
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x0015AF50 File Offset: 0x00159150
		protected override void ProcessResponse(Action<LeaderboardPublishScoreForAllPeriodEndpoint.Result, WebError> onCompletion)
		{
			ApiLeaderboardGetRankAndScoreAllPeriodResponse apiLeaderboardGetRankAndScoreAllPeriodResponse = JsonUtility.FromJson<ApiLeaderboardGetRankAndScoreAllPeriodResponse>(base._HTTPResponse.DataAsText);
			List<Builder<LeaderboardScoringInfo>.BuilderErrors> list = new List<Builder<LeaderboardScoringInfo>.BuilderErrors>();
			LeaderboardPublishScoreForAllPeriodEndpoint.Result result = new LeaderboardPublishScoreForAllPeriodEndpoint.Result(this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.day, ref list), this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.week, ref list), this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.ever, ref list));
			if (onCompletion != null)
			{
				if (list.Count == 0)
				{
					onCompletion(result, null);
					return;
				}
				onCompletion(null, new LeaderboardScoringInfoError(list.ToArray()));
			}
		}

		// Token: 0x02000919 RID: 2329
		public class Result
		{
			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x06003F11 RID: 16145 RVA: 0x0005079E File Offset: 0x0004E99E
			// (set) Token: 0x06003F12 RID: 16146 RVA: 0x000507A6 File Offset: 0x0004E9A6
			public LeaderboardScoringInfo DailyLeaderboard { get; private set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x06003F13 RID: 16147 RVA: 0x000507AF File Offset: 0x0004E9AF
			// (set) Token: 0x06003F14 RID: 16148 RVA: 0x000507B7 File Offset: 0x0004E9B7
			public LeaderboardScoringInfo WeeklyLeaderboard { get; private set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x06003F15 RID: 16149 RVA: 0x000507C0 File Offset: 0x0004E9C0
			// (set) Token: 0x06003F16 RID: 16150 RVA: 0x000507C8 File Offset: 0x0004E9C8
			public LeaderboardScoringInfo ForeverLeaderboard { get; private set; }

			// Token: 0x06003F17 RID: 16151 RVA: 0x000507D1 File Offset: 0x0004E9D1
			public Result(LeaderboardScoringInfo day, LeaderboardScoringInfo week, LeaderboardScoringInfo ever)
			{
				this.DailyLeaderboard = day;
				this.WeeklyLeaderboard = week;
				this.ForeverLeaderboard = ever;
			}
		}
	}
}
