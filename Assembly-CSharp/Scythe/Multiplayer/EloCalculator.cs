using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.Multiplayer.Data;
using ScytheWebRole.ServerLogic;

namespace Scythe.Multiplayer
{
	// Token: 0x02000214 RID: 532
	public class EloCalculator
	{
		// Token: 0x06000FC4 RID: 4036 RVA: 0x0008E60C File Offset: 0x0008C80C
		public static EloCalculator.PossibleEloChange CalculatePossibleEloChange(List<PlayerInfo> players)
		{
			int minimalELO = EloCalculator.GetMinimalELO(players);
			int maximalELO = EloCalculator.GetMaximalELO(players);
			int elo = PlayerInfo.me.PlayerStats.ELO;
			return new EloCalculator.PossibleEloChange
			{
				min = minimalELO - elo,
				max = maximalELO - elo
			};
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0008E654 File Offset: 0x0008C854
		private static int GetMinimalELO(List<PlayerInfo> players)
		{
			List<PlayerRating> list = new List<PlayerRating>();
			PlayerRating playerRating = new PlayerRating(PlayerInfo.me.PlayerStats.Id);
			playerRating.UpdateData(PlayerInfo.me.PlayerStats.ELO, PlayerInfo.me.PlayerStats.RankedGames);
			EloCalculator.AddAllPlayers(list, players);
			list.OrderBy((PlayerRating rating) => rating.Rating);
			list.Add(playerRating);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetPlace(i + 1);
			}
			PlayerRatingCalculator.CalculateRatingChange(list);
			return playerRating.Rating;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0008E700 File Offset: 0x0008C900
		private static int GetMaximalELO(List<PlayerInfo> players)
		{
			List<PlayerRating> list = new List<PlayerRating>();
			PlayerRating playerRating = new PlayerRating(PlayerInfo.me.PlayerStats.Id);
			playerRating.UpdateData(PlayerInfo.me.PlayerStats.ELO, PlayerInfo.me.PlayerStats.RankedGames);
			list.Add(playerRating);
			EloCalculator.AddAllPlayers(list, players);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetPlace(i + 1);
			}
			PlayerRatingCalculator.CalculateRatingChange(list);
			return playerRating.Rating;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0008E788 File Offset: 0x0008C988
		private static void AddAllPlayers(List<PlayerRating> playersRatings, List<PlayerInfo> players)
		{
			foreach (PlayerInfo playerInfo in players)
			{
				if (playerInfo.PlayerStats.Id != PlayerInfo.me.PlayerStats.Id)
				{
					PlayerRating playerRating = new PlayerRating(playerInfo.PlayerStats.Id);
					playerRating.UpdateData(playerInfo.PlayerStats.ELO, playerInfo.PlayerStats.RankedGames);
					playersRatings.Add(playerRating);
				}
			}
		}

		// Token: 0x02000215 RID: 533
		public struct PossibleEloChange
		{
			// Token: 0x04000C34 RID: 3124
			public int min;

			// Token: 0x04000C35 RID: 3125
			public int max;
		}
	}
}
