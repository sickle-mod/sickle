using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using UnityEngine;

// Token: 0x02000046 RID: 70
public static class LocalStats
{
	// Token: 0x06000234 RID: 564 RVA: 0x0005AD44 File Offset: 0x00058F44
	public static PlayerStats GetLocalPlayerStats()
	{
		string @string = PlayerPrefs.GetString("LocalStats", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			PlayerStats playerStats = new PlayerStats();
			playerStats.ELOList = new List<int> { 0 };
			playerStats.GamesList = new List<int> { 0 };
			playerStats.PlayerFactionStatsString = new List<string>();
			for (int i = 0; i < 49; i++)
			{
				playerStats.PlayerFactionStatsString.Add(string.Empty);
			}
			playerStats.PlayerFactionStats = new PlayerFactionStats[7][];
			for (int j = 0; j < 7; j++)
			{
				playerStats.PlayerFactionStats[j] = new PlayerFactionStats[7];
				for (int k = 0; k < 7; k++)
				{
					playerStats.PlayerFactionStats[j][k] = new PlayerFactionStats();
				}
			}
			if (PlatformManager.IsStandalone)
			{
				if (AsmodeeLogic.Instance != null)
				{
					playerStats.Name = AsmodeeLogic.Instance.GetPlayerName();
				}
				else
				{
					playerStats.Name = ScriptLocalization.Get("Common/Player");
				}
			}
			PlayerPrefs.SetString("LocalStats", PlayerStats.ToJson(playerStats));
			return playerStats;
		}
		return PlayerStats.FromJson(@string);
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00029446 File Offset: 0x00027646
	public static void SetLocalPlayerStats(PlayerStats actualPlayerStats)
	{
		PlayerPrefs.SetString("LocalStats", PlayerStats.ToJson(actualPlayerStats));
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0005AE4C File Offset: 0x0005904C
	public static void UpdateLocalPlayerStats(Player player, int points, int playerPlace)
	{
		PlayerStats localPlayerStats = LocalStats.GetLocalPlayerStats();
		int faction = (int)player.matFaction.faction;
		int matType = (int)player.matPlayer.matType;
		localPlayerStats.RankedGames++;
		if (points > localPlayerStats.RankedTopScore)
		{
			localPlayerStats.RankedTopScore = points;
		}
		if (playerPlace == 1)
		{
			localPlayerStats.RankedFirstPlaceStreak++;
		}
		else
		{
			localPlayerStats.RankedFirstPlaceStreak = 0;
		}
		if (localPlayerStats.ELOList.Count > 20)
		{
			localPlayerStats.ELOList.RemoveAt(0);
			localPlayerStats.GamesList.RemoveAt(0);
		}
		localPlayerStats.ELOList.Add(localPlayerStats.ELO);
		localPlayerStats.GamesList.Add(localPlayerStats.RankedGames);
		PlayerFactionStats playerFactionStats = localPlayerStats.PlayerFactionStats[faction][matType];
		double num = (double)playerFactionStats.RankedGamesAmount * playerFactionStats.RankedAverageScore + (double)points;
		playerFactionStats.RankedGamesAmount++;
		if (points > playerFactionStats.RankedTopScore)
		{
			playerFactionStats.RankedTopScore = points;
		}
		playerFactionStats.RankedAverageScore = num / (double)playerFactionStats.RankedGamesAmount;
		foreach (object obj in Enum.GetValues(typeof(StarType)))
		{
			StarType starType = (StarType)obj;
			playerFactionStats.RankedTotalStars[(int)starType] += player.stars[starType];
		}
		using (List<Building>.Enumerator enumerator2 = player.matPlayer.buildings.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				switch (enumerator2.Current.buildingType)
				{
				case BuildingType.Mine:
					playerFactionStats.RankedTotalMines++;
					break;
				case BuildingType.Monument:
					playerFactionStats.RankedTotalMonuments++;
					break;
				case BuildingType.Armory:
					playerFactionStats.RankedTotalArmories++;
					break;
				case BuildingType.Mill:
					playerFactionStats.RankedTotalMills++;
					break;
				}
			}
		}
		Dictionary<ResourceType, int> dictionary = player.Resources(false);
		playerFactionStats.RankedTotalFood += dictionary[ResourceType.food];
		playerFactionStats.RankedTotalMetal += dictionary[ResourceType.metal];
		playerFactionStats.RankedTotalWood += dictionary[ResourceType.wood];
		playerFactionStats.RankedTotalOil += dictionary[ResourceType.oil];
		playerFactionStats.RankedCombatWonAmount += player.CombatWon;
		playerFactionStats.RankedCombatLostAmount += player.CombatLost;
		playerFactionStats.RankedCombatPowerSpent += player.CombatPowerSpent;
		if (playerFactionStats.RankedCombatMaxPowerUsed < player.CombatMaxPowerUsed)
		{
			playerFactionStats.RankedCombatMaxPowerUsed = player.CombatMaxPowerUsed;
		}
		playerFactionStats.RankedCombatWorkersChased += player.CombatWorkersChased;
		playerFactionStats.RankedTotalPopularity += player.Popularity;
		playerFactionStats.RankedTotalPower += player.Power;
		playerFactionStats.RankedTotalCoins += player.Coins;
		playerFactionStats.RankedTotalMechs += player.matFaction.mechs.Count;
		playerFactionStats.RankedTotalObjectivesDone += player.stars[StarType.Objective];
		playerFactionStats.RankedTotalDistanceTravelled += player.DistanceTravelled;
		switch (GameController.GameManager.GetPlayers().Count)
		{
		case 2:
			playerFactionStats.RankedPlacesTwoPlayersGames[playerPlace - 1]++;
			break;
		case 3:
			playerFactionStats.RankedPlacesThreePlayersGames[playerPlace - 1]++;
			break;
		case 4:
			playerFactionStats.RankedPlacesFourPlayersGames[playerPlace - 1]++;
			break;
		case 5:
			playerFactionStats.RankedPlacesFivePlayersGames[playerPlace - 1]++;
			break;
		case 6:
			playerFactionStats.RankedPlacesSixPlayersGames[playerPlace - 1]++;
			break;
		case 7:
			playerFactionStats.RankedPlacesSevenPlayersGames[playerPlace - 1]++;
			break;
		}
		LocalStats.SetLocalPlayerStats(localPlayerStats);
	}

	// Token: 0x040001A0 RID: 416
	private const string LOCAL_STATS = "LocalStats";
}
