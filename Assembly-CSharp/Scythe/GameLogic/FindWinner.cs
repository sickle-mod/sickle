using System;
using System.Collections.Generic;
using System.Linq;

namespace Scythe.GameLogic
{
	// Token: 0x020005BD RID: 1469
	public class FindWinner
	{
		// Token: 0x06002ED4 RID: 11988 RVA: 0x00045431 File Offset: 0x00043631
		public FindWinner(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x0011C148 File Offset: 0x0011A348
		public List<PlayerEndGameStats> CalculateStats()
		{
			foreach (Player player in this.gameManager.GetPlayers())
			{
				this.playerStats.Add(this.CalculatePoints(player));
			}
			this.playerStats = this.playerStats.OrderByDescending((PlayerEndGameStats stats) => stats.totalPoints).ToList<PlayerEndGameStats>();
			this.SortPlayers();
			for (int i = 0; i < this.playerStats.Count; i++)
			{
				this.playerStats[i].place = i + 1;
			}
			return this.playerStats;
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x0011C218 File Offset: 0x0011A418
		public PlayerEndGameStats CalculatePoints(Player player)
		{
			int num = this.gameManager.players.Count<Player>();
			PlayerEndGameStats playerEndGameStats = new PlayerEndGameStats(this.gameManager.IsRanked);
			playerEndGameStats.player = player;
			playerEndGameStats.name = player.Name;
			playerEndGameStats.faction = (int)player.matFaction.faction;
			playerEndGameStats.units = player.GetAllUnits().Count;
			playerEndGameStats.units += player.matPlayer.buildings.Count;
			Dictionary<ResourceType, int> dictionary = player.Resources(true);
			int num2 = dictionary[ResourceType.food];
			num2 += dictionary[ResourceType.metal];
			num2 += dictionary[ResourceType.wood];
			num2 += dictionary[ResourceType.oil];
			playerEndGameStats.resources = num2;
			playerEndGameStats.resourcePoints = PopularityTrack.ResourceBonus(num2, player.Popularity);
			playerEndGameStats.starPoints = PopularityTrack.StarBonus(player.GetNumberOfStars(), player.Popularity);
			HashSet<GameHex> hashSet = player.OwnedFields(true);
			hashSet.RemoveWhere((GameHex h) => h.hexType == HexType.capital);
			int num3 = hashSet.Count;
			if (hashSet.Contains(this.gameManager.gameBoard.factory))
			{
				num3 += 2;
			}
			if (player.matFaction.faction == Faction.Albion)
			{
				num3 += this.CalculateAlbionBonusPoints(player);
			}
			playerEndGameStats.territories = num3;
			playerEndGameStats.territoryPoints = PopularityTrack.TerritoryBonus(num3, player.Popularity);
			playerEndGameStats.structurePoints = this.gameManager.StructureBonus.CalculateBonus(player);
			playerEndGameStats.coinPoints = player.Coins;
			playerEndGameStats.totalPoints = playerEndGameStats.resourcePoints + playerEndGameStats.starPoints + playerEndGameStats.structurePoints + playerEndGameStats.territoryPoints + playerEndGameStats.coinPoints;
			if (player.matFaction.faction == Faction.Polania)
			{
				playerEndGameStats.totalPoints += this.CalculatePolaniaBonusPoints(player, num);
			}
			return playerEndGameStats;
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x0011C3EC File Offset: 0x0011A5EC
		public int CalculatePolaniaBonusPoints(Player player, int numberOfPlayers)
		{
			int num = 0;
			if (numberOfPlayers < 6)
			{
				return num;
			}
			if (player == null || player.matFaction.faction != Faction.Polania)
			{
				return num;
			}
			using (HashSet<GameHex>.Enumerator enumerator = player.OwnedFields(false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.hasEncounter)
					{
						num += 3;
					}
				}
			}
			return num;
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x0011C460 File Offset: 0x0011A660
		public int CalculateAlbionBonusPoints(Player player)
		{
			int num = 0;
			if (player == null || player.matFaction.faction != Faction.Albion)
			{
				return num;
			}
			foreach (FactionAbilityToken factionAbilityToken in player.matFaction.FactionTokens.GetPlacedTokens())
			{
				FlagToken flagToken = (FlagToken)factionAbilityToken;
				if (flagToken.Position.Owner == player && flagToken.Position != this.gameManager.gameBoard.hexMap[1, 1] && flagToken.Position != this.gameManager.gameBoard.hexMap[2, 1])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x0011C524 File Offset: 0x0011A724
		public int CalculateTogawaBonusPoints(Player player)
		{
			int num = 0;
			if (player == null || player.matFaction.faction != Faction.Togawa)
			{
				return num;
			}
			foreach (FactionAbilityToken factionAbilityToken in player.matFaction.FactionTokens.GetPlacedTokens())
			{
				TrapToken trapToken = (TrapToken)factionAbilityToken;
				if (trapToken.Armed && trapToken.Position.Owner == null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0011C5B0 File Offset: 0x0011A7B0
		private void SortPlayers()
		{
			this.playerStats = (from stats in this.playerStats
				orderby stats.totalPoints descending, stats.units descending, stats.player.Power descending, stats.player.Popularity descending, stats.resources descending, stats.territories descending, stats.starPoints descending
				select stats).ToList<PlayerEndGameStats>();
		}

		// Token: 0x04001F72 RID: 8050
		public List<PlayerEndGameStats> playerStats = new List<PlayerEndGameStats>();

		// Token: 0x04001F73 RID: 8051
		private GameManager gameManager;
	}
}
