using System;
using System.Collections.Generic;

namespace ScytheWebRole.ServerLogic
{
	// Token: 0x020001A0 RID: 416
	public static class PlayerRatingCalculator
	{
		// Token: 0x06000C4B RID: 3147 RVA: 0x00080A04 File Offset: 0x0007EC04
		public static void CalculateRatingChange(List<PlayerRating> endGameRanking)
		{
			for (int i = 0; i < endGameRanking.Count; i++)
			{
				for (int j = i + 1; j < endGameRanking.Count; j++)
				{
					PlayerRatingCalculator.CalculateNewRating(endGameRanking[i], endGameRanking[j]);
				}
				endGameRanking[i].ApplyChange();
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00080A54 File Offset: 0x0007EC54
		public static void CalculateNewRating(PlayerRating player1, PlayerRating player2)
		{
			double maximumEloChange = PlayerRatingCalculator.GetMaximumEloChange(player1, player2);
			double maximumEloChange2 = PlayerRatingCalculator.GetMaximumEloChange(player2, player1);
			double num = 1.0 / (1.0 + Math.Pow(10.0, (double)(player2.Rating - player1.Rating) / 400.0));
			double num2 = 1.0 / (1.0 + Math.Pow(10.0, (double)(player1.Rating - player2.Rating) / 400.0));
			double num3 = PlayerRatingCalculator.ScoreRanked(player1, player2);
			double num4 = PlayerRatingCalculator.ScoreRanked(player2, player1);
			player1.IncreaseNewRating((double)player1.Rating + maximumEloChange * (num3 - num));
			player2.IncreaseNewRating((double)player2.Rating + maximumEloChange2 * (num4 - num2));
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00030353 File Offset: 0x0002E553
		private static double GetMaximumEloChange(PlayerRating player, PlayerRating enemy)
		{
			if (player.IsRanked && enemy.IsRanked)
			{
				return 32.0;
			}
			if (player.IsRanked && enemy.IsProvisional)
			{
				return 24.0;
			}
			return 200.0;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00030392 File Offset: 0x0002E592
		private static double ScoreRanked(PlayerRating player, PlayerRating enemy)
		{
			if (player.Place < enemy.Place)
			{
				return 1.0;
			}
			if (player.Place > enemy.Place)
			{
				return 0.0;
			}
			return 0.5;
		}

		// Token: 0x040009B4 RID: 2484
		public const double MAX_RATING_CHANGE_FOR_RANKED_PLAYERS = 32.0;

		// Token: 0x040009B5 RID: 2485
		public const double RATING_DIFFERENCE_IMPACT_RATIO = 400.0;

		// Token: 0x040009B6 RID: 2486
		public const double MAX_RATING_CHANGE_FOR_PROVISIONAL_PLAYERS = 200.0;

		// Token: 0x040009B7 RID: 2487
		public const double MAX_RATING_CHANGE_FOR_DIFFERENT_PLAYERS = 24.0;

		// Token: 0x040009B8 RID: 2488
		public const double AMOUNT_OF_GAMES_TO_BE_RANKED = 6.0;
	}
}
