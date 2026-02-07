using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Network.RestApi;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class AchievementManager
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000075 RID: 117 RVA: 0x000283F8 File Offset: 0x000265F8
	private static bool LogAchievements
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000076 RID: 118 RVA: 0x000283FB File Offset: 0x000265FB
	private static GameManager GameManager
	{
		get
		{
			return GameController.GameManager;
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00053A88 File Offset: 0x00051C88
	public static void SetAchievementList(Achievement[] achievements, bool remoteValues = true)
	{
		List<Achievement> list = achievements.ToList<Achievement>();
		AchievementManager.mappedAchievements = new Dictionary<Achievements, Achievement>();
		List<Achievements> list2 = Enum.GetValues(typeof(Achievements)).Cast<Achievements>().ToList<Achievements>();
		string tagString = "";
		Predicate<Achievement> <>9__0;
		foreach (Achievements achievements2 in list2)
		{
			tagString = achievements2.ToString();
			List<Achievement> list3 = list;
			Predicate<Achievement> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Achievement a) => a.Tag == tagString);
			}
			Achievement achievement = list3.Find(predicate);
			if (achievement != null)
			{
				if (!AchievementManager.mappedAchievements.ContainsKey(achievements2))
				{
					AchievementManager.mappedAchievements.Add(achievements2, achievement);
				}
				else
				{
					AchievementManager.mappedAchievements[achievements2] = achievement;
				}
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00053B74 File Offset: 0x00051D74
	private static void SetGainedAwardsList(Award[] awards, bool remoteValues = true)
	{
		List<Award> list = awards.ToList<Award>();
		AchievementManager.mappedAwards = new Dictionary<Achievements, Award>();
		List<Achievements> list2 = Enum.GetValues(typeof(Achievements)).Cast<Achievements>().ToList<Achievements>();
		string tagString = "";
		if (remoteValues)
		{
			AchievementManager.MergeLocalAndRemoteAwardsList(awards);
		}
		Predicate<Award> <>9__0;
		foreach (Achievements achievements in list2)
		{
			tagString = achievements.ToString();
			List<Award> list3 = list;
			Predicate<Award> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Award a) => a.Tag == tagString);
			}
			Award award = list3.Find(predicate);
			if (award != null)
			{
				if (!AchievementManager.mappedAwards.ContainsKey(achievements))
				{
					AchievementManager.mappedAwards.Add(achievements, award);
				}
				else
				{
					AchievementManager.mappedAwards[achievements] = award;
				}
			}
		}
		AchievementManager.PrintUnlockedAchievements();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00053C6C File Offset: 0x00051E6C
	public static void GenerateAchievementList()
	{
		List<Achievements> list = Enum.GetValues(typeof(Achievements)).Cast<Achievements>().ToList<Achievements>();
		List<Achievement> list2 = new List<Achievement>();
		foreach (Achievements achievements in list)
		{
			string text = achievements.ToString();
			JsonAchievement jsonAchievement = new JsonAchievement();
			jsonAchievement.game = "scte";
			jsonAchievement.tag = text;
			jsonAchievement.texts = new JsonAchievement.Text[]
			{
				new JsonAchievement.Text()
			};
			jsonAchievement.texts[0].lang = LocalizationManager.CurrentLanguageCode;
			jsonAchievement.texts[0].name = ScriptLocalization.Get("Achievements/" + text + "Name");
			jsonAchievement.texts[0].description = ScriptLocalization.Get("Achievements/" + text + "Description");
			Achievement achievement = new Achievement(jsonAchievement);
			list2.Add(achievement);
		}
		AchievementManager.SetAchievementList(list2.ToArray(), true);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00053D94 File Offset: 0x00051F94
	public static void GenerateGainedAwardsList()
	{
		if (AchievementManager.debugMessages)
		{
			Debug.Log("Generating awards");
		}
		List<Achievements> list = Enum.GetValues(typeof(Achievements)).Cast<Achievements>().ToList<Achievements>();
		List<Award> list2 = new List<Award>();
		foreach (Achievements achievements in list)
		{
			if (AchievementManager.IsAchievementUnlockedThirdParty(achievements))
			{
				if (AchievementManager.debugMessages)
				{
					Debug.Log(achievements.ToString() + " is unlocked");
				}
				Award award = new Award(achievements.ToString(), -1, -1, null);
				list2.Add(award);
			}
		}
		AchievementManager.SetGainedAwardsList(list2.ToArray(), false);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00053E68 File Offset: 0x00052068
	private static void MergeLocalAndRemoteAwardsList(Award[] remoteAwards)
	{
		IEnumerable<Achievements> enumerable = Enum.GetValues(typeof(Achievements)).Cast<Achievements>();
		List<Achievements> list = new List<Achievements>();
		List<Achievements> list2 = new List<Achievements>();
		foreach (Achievements achievements in enumerable)
		{
			bool flag = AchievementManager.IsAchievementUnlockedThirdParty(achievements);
			bool flag2 = AchievementManager.IsAchievementUnlockedRemote(achievements, remoteAwards);
			if (AchievementManager.debugMessages)
			{
				Debug.Log(string.Concat(new string[]
				{
					achievements.ToString(),
					" steam state ",
					flag.ToString(),
					" Asmodee state ",
					flag2.ToString()
				}));
			}
			if (flag != flag2)
			{
				if (flag)
				{
					list.Add(achievements);
				}
				else
				{
					list2.Add(achievements);
				}
			}
		}
		if (list.Count != 0)
		{
			AchievementManager.UnlockAchievementRemote(list.ToArray());
		}
		if (list2.Count != 0)
		{
			AchievementManager.UnlockAchievementsThirdParty(list2);
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00053F64 File Offset: 0x00052164
	private static void PrintUnlockedAchievements()
	{
		Dictionary<Achievements, Award>.KeyCollection keys = AchievementManager.mappedAwards.Keys;
		string text = "";
		foreach (Achievements achievements in keys)
		{
			text = text + achievements.ToString() + "\n";
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00028402 File Offset: 0x00026602
	public static bool AchievementsListGenerated()
	{
		return AchievementManager.mappedAchievements.Count == 0;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00053FD4 File Offset: 0x000521D4
	private static bool CorrectGameMode(Player player)
	{
		if (GameController.GameManager.IsCampaign || (GameController.GameManager.IsMultiplayer && GameController.GameManager.SpectatorMode))
		{
			return false;
		}
		if (!AchievementManager.GameManager.IsMultiplayer || !AchievementManager.GameManager.GameLoading)
		{
			return (AchievementManager.GameManager.IsMultiplayer && player == AchievementManager.GameManager.PlayerOwner) || (AchievementManager.GameManager.IsHotSeat && player.IsHuman);
		}
		if (AchievementManager.GameManager.IsPlayerOwnerAlreadyLoaded())
		{
			return AchievementManager.GameManager.IsMultiplayer && player == AchievementManager.GameManager.PlayerOwner;
		}
		return AchievementManager.GameManager.IsPlayerOwnerBeingLoaded();
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00054084 File Offset: 0x00052284
	private static void UpdateAchievementForAllAchievementsUnlocked()
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		int num = 55;
		if (AchievementManager.mappedAwards.Count >= num)
		{
			AchievementManager.UpdateAchievement(Achievements.Homebase);
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000540B0 File Offset: 0x000522B0
	public static void UnlockAllAchievements()
	{
		int num = 0;
		int num2 = 28;
		for (int i = num; i < num2; i++)
		{
			AchievementManager.UpdateAchievement((Achievements)i);
		}
		AchievementManager.UpdateAchievement(Achievements.FirstTutorial);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000540DC File Offset: 0x000522DC
	public static void UpdateAchievementStars(StarType starType, Player player)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			if (AchievementManager.CorrectGameMode(player))
			{
				switch (starType)
				{
				case StarType.Upgrades:
					AchievementManager.UpdateAchievement(Achievements.UpgradeStar);
					break;
				case StarType.Mechs:
					AchievementManager.UpdateAchievement(Achievements.MechStar);
					break;
				case StarType.Structures:
					AchievementManager.UpdateAchievement(Achievements.StructureStar);
					break;
				case StarType.Recruits:
					AchievementManager.UpdateAchievement(Achievements.EnlistStar);
					break;
				case StarType.Workers:
					AchievementManager.UpdateAchievement(Achievements.WorkersStar);
					break;
				case StarType.Objective:
					AchievementManager.UpdateAchievement(Achievements.ObjectiveStar);
					break;
				case StarType.Combat:
					if (player.stars[starType] == 2)
					{
						AchievementManager.UpdateAchievement(Achievements.BattleStars);
					}
					break;
				case StarType.Popularity:
					AchievementManager.UpdateAchievement(Achievements.PopularityStar);
					break;
				case StarType.Power:
					AchievementManager.UpdateAchievement(Achievements.PowerStar);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00054198 File Offset: 0x00052398
	public static void UpdateAchievementsEndgame(List<PlayerEndGameStats> stats, Player player)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			Player player2 = stats[0].player;
			List<Achievements> list = new List<Achievements>();
			PlayerEndGameStats playerEndGameStats = stats.Find((PlayerEndGameStats outcome) => outcome.player.GetNumberOfStars() == 6);
			if (AchievementManager.CorrectGameMode(player2) && ((AchievementManager.GameManager.IsMultiplayer && player2.matFaction.faction == player.matFaction.faction) || (AchievementManager.GameManager.IsHotSeat && player2.IsHuman)))
			{
				list.Add(Achievements.WinGame);
				if (AchievementManager.GameManager.IsMultiplayer && GameServiceController.Instance.InvadersFromAfarUnlocked())
				{
					list.Add(Achievements.WinOnlineGame);
				}
				if (player2.GetNumberOfStars() < 6 && playerEndGameStats != null)
				{
					list.Add(Achievements.WinEnemy6Stars);
				}
				if (stats[0].totalPoints >= 100)
				{
					list.Add(Achievements.Win100Coins);
				}
				if (player2.Popularity <= 6)
				{
					list.Add(Achievements.WinLowPopularity);
				}
				if (player2.CombatWon > 2)
				{
					list.Add(Achievements.WinMultipleCombats);
				}
				if (player2.matPlayer.UpgradesDone == 0)
				{
					list.Add(Achievements.WinNoUpgrades);
				}
				if (player2.matFaction.mechs.Count == 0)
				{
					list.Add(Achievements.WinNoMechs);
				}
				if (player2.matPlayer.buildings.Count == 0)
				{
					list.Add(Achievements.WinNoStructures);
				}
				if (player2.matPlayer.RecruitsEnlisted == 0)
				{
					list.Add(Achievements.WinNoRecruits);
				}
			}
			if (AchievementManager.GameManager.IsMultiplayer)
			{
				list.AddRange(AchievementManager.GetOtherEndgameAchievementsToUnlock(stats, player));
			}
			else
			{
				foreach (Player player3 in AchievementManager.GameManager.players)
				{
					list.AddRange(AchievementManager.GetOtherEndgameAchievementsToUnlock(stats, player3));
				}
			}
			if (GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				list.AddRange(AchievementManager.GetEndgameDLCAchievementsToUnlock(stats, player));
			}
			AchievementManager.UpdateAchievements(list);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00028411 File Offset: 0x00026611
	public static void UpdateAchievementTutorial()
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		AchievementManager.UpdateAchievement(Achievements.FirstTutorial);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000543BC File Offset: 0x000525BC
	private static IEnumerable<Achievements> GetOtherEndgameAchievementsToUnlock(List<PlayerEndGameStats> stats, Player player)
	{
		List<Achievements> list = new List<Achievements>();
		if (AchievementManager.CorrectGameMode(player))
		{
			Player player2 = AchievementManager.PlayerWithTheBiggestTerritoryUnderControl(stats);
			if (player2 != null && player2 == player && player2.IsHuman)
			{
				list.Add(Achievements.WinMostTiles);
			}
			player2 = AchievementManager.PlayerWithTheMostResourcesUnderControl(stats);
			if (player2 != null && player2 == player && player2.IsHuman)
			{
				list.Add(Achievements.WinMostResources);
			}
			if ((AchievementManager.GameManager.IsMultiplayer && AchievementManager.GameManager.StructureBonus.structureBonus.HighestBonus() == AchievementManager.StructurePointsOfAPlayer(stats, player)) || (AchievementManager.GameManager.IsHotSeat && stats[0].player.IsHuman && AchievementManager.GameManager.StructureBonus.structureBonus.HighestBonus() == AchievementManager.StructurePointsOfAPlayer(stats, stats[0].player)))
			{
				list.Add(Achievements.WinHighStructureBonus);
			}
			if ((AchievementManager.GameManager.IsMultiplayer && AchievementManager.GameManager.gameBoard.factory.Owner == player) || (AchievementManager.GameManager.IsHotSeat && AchievementManager.GameManager.gameBoard.factory.Owner != null && AchievementManager.GameManager.gameBoard.factory.Owner.IsHuman))
			{
				list.Add(Achievements.FactoryInControl);
			}
		}
		return list;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x000544F8 File Offset: 0x000526F8
	public static List<Achievements> GetEndgameDLCAchievementsToUnlock(List<PlayerEndGameStats> stats, Player player)
	{
		List<Achievements> list = new List<Achievements>();
		if (!AchievementManager.LogAchievements)
		{
			return list;
		}
		try
		{
			Player player2 = stats[0].player;
			if (AchievementManager.CorrectGameMode(player2) && ((AchievementManager.GameManager.IsMultiplayer && player2.matFaction.faction == player.matFaction.faction) || (AchievementManager.GameManager.IsHotSeat && player2.IsHuman)))
			{
				list.AddRange(AchievementManager.IFAFactionAchievementsToUnlock(player2));
				list.AddRange(AchievementManager.IFAPlayerMatAchievementsToUnlock(player2));
				list.AddRange(AchievementManager.CheckNumberOfCombatVictories(player2));
				if (AchievementManager.GameManager.TurnCount <= 15)
				{
					list.Add(Achievements.WinIn15Turns);
				}
				if (player2.StartedCombatsAsAnAttacker == 0)
				{
					list.Add(Achievements.WinWithoutAttacking);
				}
				if (stats[0].territories >= 13)
				{
					list.Add(Achievements.WinWith13Territories);
				}
				if (AchievementManager.GameManager.gameBoard.hexMap[3, 3].Owner == player2 && AchievementManager.GameManager.gameBoard.hexMap[4, 4].Owner == player2 && AchievementManager.GameManager.gameBoard.hexMap[3, 5].Owner == player2)
				{
					list.Add(Achievements.WinWithFactoryNeighborhood);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
		return list;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00054658 File Offset: 0x00052858
	private static List<Achievements> IFAFactionAchievementsToUnlock(Player winner)
	{
		List<Achievements> list = new List<Achievements>
		{
			Achievements.WinAsPolania,
			Achievements.WinAsAlbion,
			Achievements.WinAsNordic,
			Achievements.WinAsRusviet,
			Achievements.WinAsTogawa,
			Achievements.WinAsCrimea,
			Achievements.WinAsSaxony
		};
		List<Achievements> list2 = new List<Achievements>();
		list2.Add(list[(int)winner.matFaction.faction]);
		int num = 1;
		for (int i = 0; i < 7; i++)
		{
			if (i != (int)winner.matFaction.faction)
			{
				num += (AchievementManager.IsAchievementUnlocked(list[i], false) ? 1 : 0);
			}
		}
		if (num == 7)
		{
			list2.Add(Achievements.WinAsEveryFaction);
		}
		return list2;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00054700 File Offset: 0x00052900
	private static List<Achievements> IFAPlayerMatAchievementsToUnlock(Player winner)
	{
		List<Achievements> list = new List<Achievements>
		{
			Achievements.WinWithIndustrial,
			Achievements.WinWithEngineering,
			Achievements.WinWithPatriotic,
			Achievements.WinWithMechanical,
			Achievements.WinWithAgricultural,
			Achievements.WinWithMilitant,
			Achievements.WinWithInnovative
		};
		List<Achievements> list2 = new List<Achievements>();
		list2.Add(list[(int)winner.matPlayer.matType]);
		int num = 1;
		for (int i = 0; i < 7; i++)
		{
			if (i != (int)winner.matPlayer.matType)
			{
				num += (AchievementManager.IsAchievementUnlocked(list[i], false) ? 1 : 0);
			}
		}
		if (num == 7)
		{
			list2.Add(Achievements.WinWithAllMats);
		}
		return list2;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00028422 File Offset: 0x00026622
	public static List<Achievements> CheckNumberOfCombatVictories(Player winner)
	{
		if (winner.CombatWon >= 5)
		{
			return new List<Achievements> { Achievements.WinWith5Battles };
		}
		return new List<Achievements>();
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000547A8 File Offset: 0x000529A8
	public static void CheckDLCCombatAchievements(GameHex battlefield, Player winner, int winnerUsedPower, int defeatedUsedPower)
	{
		if (winner.matFaction.faction == Faction.Albion && AchievementManager.GameManager.combatManager.GetDefeated().matFaction.faction == Faction.Togawa)
		{
			AchievementManager.UpdateAchievement(Achievements.BeatTogawaWithAlbion);
		}
		if (winner.matFaction.faction == Faction.Togawa && AchievementManager.GameManager.combatManager.GetDefeated().matFaction.faction == Faction.Albion)
		{
			AchievementManager.UpdateAchievement(Achievements.BeatAlbionWithTogawa);
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00054818 File Offset: 0x00052A18
	public static void CheckMultiplayerAchievements()
	{
		int num = 1250;
		if (PlayerInfo.me != null && PlayerInfo.me.PlayerStats.ELO >= num)
		{
			AchievementManager.UpdateAchievement(Achievements.ReachElo1250Rating);
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0005484C File Offset: 0x00052A4C
	public static void UpdateAchievementFirstInFactory()
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			if (AchievementManager.CorrectGameMode(AchievementManager.GameManager.PlayerCurrent))
			{
				List<Player> players = AchievementManager.GameManager.GetPlayers();
				bool flag = true;
				using (List<Player>.Enumerator enumerator = players.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.matPlayer.matPlayerSectionsCount == 5)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					AchievementManager.UpdateAchievement(Achievements.FirstInFactory);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000548E8 File Offset: 0x00052AE8
	public static void UpdateAchievementCombat(GameHex battlefield, Player winner, int winnerUsedPower, int defeatedUsedPower)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			if (AchievementManager.CorrectGameMode(winner))
			{
				if (winnerUsedPower == 0)
				{
					AchievementManager.UpdateAchievement(Achievements.CombatUse0Power);
				}
				if (battlefield.hexType == HexType.lake)
				{
					AchievementManager.UpdateAchievement(Achievements.DefeatEnemyOnLake);
				}
				if (battlefield.hexType == HexType.factory)
				{
					AchievementManager.UpdateAchievement(Achievements.DefeatEnemyOnFactory);
				}
				AchievementManager.CheckDLCCombatAchievements(battlefield, winner, winnerUsedPower, defeatedUsedPower);
				bool flag;
				if (battlefield.Owner == winner)
				{
					flag = battlefield.HasEnemyCharacter();
				}
				else
				{
					flag = battlefield.HasOwnerCharacter();
				}
				if (flag)
				{
					AchievementManager.UpdateAchievement(Achievements.DefeatEnemyHero);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0005497C File Offset: 0x00052B7C
	public static void UpdateAchievementEncounter()
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			if (AchievementManager.CorrectGameMode(AchievementManager.GameManager.PlayerCurrent) && AchievementManager.GameManager.PlayerCurrent.EncountersVisited == 5)
			{
				AchievementManager.UpdateAchievement(Achievements.Encounters);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000549D8 File Offset: 0x00052BD8
	public static void UpdateAchievementMine(Building building, GameHex hex = null)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		try
		{
			if (building.buildingType == BuildingType.Mine)
			{
				if (hex == null)
				{
					hex = building.position;
				}
				if (AchievementManager.CorrectGameMode(AchievementManager.GameManager.PlayerCurrent))
				{
					List<Faction> playersFactions = AchievementManager.GameManager.GetPlayersFactions();
					playersFactions.Remove(AchievementManager.GameManager.PlayerCurrent.matFaction.faction);
					bool flag = false;
					foreach (Faction faction in playersFactions)
					{
						for (int i = 0; i < 3; i++)
						{
							if (AchievementManager.bases[faction][i, 0] == hex.posX && AchievementManager.bases[faction][i, 1] == hex.posY)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						AchievementManager.UpdateAchievement(Achievements.MineEnemyBase);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00054AE0 File Offset: 0x00052CE0
	private static Player PlayerWithTheBiggestTerritoryUnderControl(List<PlayerEndGameStats> playerStats)
	{
		Player player = null;
		int num = 0;
		foreach (PlayerEndGameStats playerEndGameStats in playerStats)
		{
			int count = playerEndGameStats.player.OwnedFields(true).Count;
			if (count > num)
			{
				num = count;
				player = playerEndGameStats.player;
			}
		}
		return player;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00054B50 File Offset: 0x00052D50
	private static Player PlayerWithTheMostResourcesUnderControl(List<PlayerEndGameStats> playerStats)
	{
		Player player = null;
		int num = 0;
		foreach (PlayerEndGameStats playerEndGameStats in playerStats)
		{
			Dictionary<ResourceType, int> dictionary = playerEndGameStats.player.Resources(true);
			int num2 = dictionary[ResourceType.food] + dictionary[ResourceType.oil] + dictionary[ResourceType.wood] + dictionary[ResourceType.metal];
			if (num2 > num)
			{
				num = num2;
				player = playerEndGameStats.player;
			}
		}
		return player;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00054BE0 File Offset: 0x00052DE0
	private static int StructurePointsOfAPlayer(List<PlayerEndGameStats> playerStats, Player player)
	{
		for (int i = 0; i < playerStats.Count; i++)
		{
			if (playerStats[i].player.matFaction.faction == player.matFaction.faction)
			{
				return playerStats[i].structurePoints;
			}
		}
		return 0;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00054C30 File Offset: 0x00052E30
	public static void UpdateAchievement(Achievements achievement)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		if (AchievementManager.debugMessages)
		{
			Debug.Log("Updating achievement: " + achievement.ToString());
		}
		try
		{
			if (!AchievementManager.IsAchievementUnlocked(achievement, false))
			{
				AchievementManager.UnlockAchievement(achievement);
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Unable to unlock achievement.\n" + ex.ToString());
		}
		if (achievement != Achievements.Homebase)
		{
			AchievementManager.UpdateAchievementForAllAchievementsUnlocked();
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00054CAC File Offset: 0x00052EAC
	public static void UpdateAchievements(IEnumerable<Achievements> achievements)
	{
		if (!AchievementManager.LogAchievements)
		{
			return;
		}
		List<Achievements> list = new List<Achievements>();
		foreach (Achievements achievements2 in achievements)
		{
			if (AchievementManager.debugMessages)
			{
				Debug.Log("Updating achievement: " + achievements2.ToString());
			}
			if (!AchievementManager.IsAchievementUnlocked(achievements2, false) && !list.Contains(achievements2))
			{
				list.Add(achievements2);
			}
		}
		if (list.Count != 0)
		{
			AchievementManager.UnlockAchievements(list);
			AchievementManager.UpdateAchievementForAllAchievementsUnlocked();
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00028440 File Offset: 0x00026640
	public static bool IsAchievementUnlocked(Achievements achievementTag, bool checkAlsoInPlayerPerfs = false)
	{
		return (checkAlsoInPlayerPerfs && PlayerPrefs.GetInt("UnlockedAchievements_" + achievementTag.ToString(), 0) == 1) || (AchievementManager.mappedAwards != null && AchievementManager.mappedAwards.ContainsKey(achievementTag));
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00054D4C File Offset: 0x00052F4C
	private static void UnlockAchievement(Achievements achievement)
	{
		if (AchievementManager.debugMessages)
		{
			Debug.Log(string.Concat(new string[]
			{
				"[Achievement Manager] Achievement ",
				achievement.ToString(),
				" - ",
				AchievementManager.mappedAchievements[achievement].Texts[0].Name,
				" unlocked."
			}));
		}
		Award award = new Award(achievement.ToString(), -1, -1, null);
		AchievementManager.mappedAwards.Add(achievement, award);
		AchievementManager.UnlockAchievementThirdParty(achievement);
		AchievementManager.UnlockAchievementRemote(achievement);
		PlayerPrefs.SetInt("UnlockedAchievements_" + achievement.ToString(), 1);
		AnalyticsEventLogger.Instance.LogAchievementUnlocked(achievement);
		AchievementManager.ReportContentUnlocked(achievement);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00054E18 File Offset: 0x00053018
	private static void UnlockAchievements(IEnumerable<Achievements> achievements)
	{
		foreach (Achievements achievements2 in achievements)
		{
			if (AchievementManager.debugMessages)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[Achievement Manager] Achievement ",
					achievements2.ToString(),
					" - ",
					AchievementManager.mappedAchievements[achievements2].Texts[0].Name,
					" unlocked."
				}));
			}
			Award award = new Award(achievements2.ToString(), -1, -1, null);
			AchievementManager.mappedAwards.Add(achievements2, award);
			PlayerPrefs.SetInt("UnlockedAchievements_" + achievements2.ToString(), 1);
			AnalyticsEventLogger.Instance.LogAchievementUnlocked(achievements2);
			AchievementManager.ReportContentUnlocked(achievements2);
		}
		AchievementManager.UnlockAchievementsThirdParty(achievements);
		AchievementManager.UnlockAchievementRemote(achievements.ToArray<Achievements>());
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00054F24 File Offset: 0x00053124
	public static void ReportContentUnlocked(Achievements achievement)
	{
		foreach (int num in AchievementManager.achievementsUnlockingObjectiveCards.Keys)
		{
			if (AchievementManager.achievementsUnlockingObjectiveCards[num] == achievement)
			{
				if (AchievementManager.debugMessages)
				{
					Debug.Log("[Achievement Manager] Content for " + AchievementManager.mappedAchievements[achievement].Texts[0].Name + " unlocked.");
				}
				string text = "objective_card_" + num.ToString();
				AnalyticsEventLogger.Instance.LogContentUnlock(achievement, text, UnlockReasons.achievement_unlocked);
			}
		}
		foreach (int num2 in AchievementManager.achievementsUnlockingEncounterCards.Keys)
		{
			if (AchievementManager.achievementsUnlockingEncounterCards[num2] == achievement)
			{
				if (AchievementManager.debugMessages)
				{
					Debug.Log("[Achievement Manager] Content for " + AchievementManager.mappedAchievements[achievement].Texts[0].Name + " unlocked.");
				}
				string text2 = "encounter_card_" + num2.ToString();
				AnalyticsEventLogger.Instance.LogContentUnlock(achievement, text2, UnlockReasons.achievement_unlocked);
			}
		}
		foreach (int num3 in AchievementManager.achievementsUnlockingFactoryCards.Keys)
		{
			if (AchievementManager.achievementsUnlockingFactoryCards[num3] == achievement)
			{
				if (AchievementManager.debugMessages)
				{
					Debug.Log("[Achievement Manager] Content for " + AchievementManager.mappedAchievements[achievement].Texts[0].Name + " unlocked.");
				}
				string text3 = "factory_card_" + num3.ToString();
				AnalyticsEventLogger.Instance.LogContentUnlock(achievement, text3, UnlockReasons.achievement_unlocked);
			}
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0002847B File Offset: 0x0002667B
	public static void RemoveGainedAchievements()
	{
		AchievementManager.RemoveGainedAchievementsRemote();
		AchievementManager.RemoveGainedAchievementsThirdParty();
		AchievementManager.mappedAwards.Clear();
		if (AchievementManager.debugMessages)
		{
			Debug.LogWarning("[Achievement Manager] Removed all gained awards!");
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00055118 File Offset: 0x00053318
	private static bool IsAchievementUnlockedRemote(Achievements achievementTag, Award[] remoteAwards)
	{
		return remoteAwards != null && remoteAwards.ToList<Award>().Find((Award a) => a.Tag == achievementTag.ToString()) != null;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000283F8 File Offset: 0x000265F8
	private static bool CanWorkWithRemote()
	{
		return true;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00055154 File Offset: 0x00053354
	private static void UnlockAchievementRemote(Achievements achievement)
	{
		if (AchievementManager.CanWorkWithRemote())
		{
			Award[] array = new Award[]
			{
				new Award(achievement.ToString(), -1, -1, null)
			};
			AsmodeeLogic.Instance.AddAward(array);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0005519C File Offset: 0x0005339C
	private static void UnlockAchievementRemote(Achievements[] achievements)
	{
		if (achievements == null || achievements.Length == 0)
		{
			return;
		}
		Award[] array = new Award[achievements.Length];
		for (int i = 0; i < achievements.Length; i++)
		{
			if (AchievementManager.CanWorkWithRemote())
			{
				array[i] = new Award(achievements[i].ToString(), -1, -1, null);
			}
		}
		if (AchievementManager.CanWorkWithRemote())
		{
			AsmodeeLogic.Instance.AddAward(array);
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000284A2 File Offset: 0x000266A2
	private static void UnlockAchievementThirdParty(Achievements achievement)
	{
		GameServiceController.Instance.SetAchievement(achievement);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000284AF File Offset: 0x000266AF
	private static void UnlockAchievementsThirdParty(IEnumerable<Achievements> achievements)
	{
		GameServiceController.Instance.SetAchievements(achievements);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000284BC File Offset: 0x000266BC
	private static bool IsAchievementUnlockedThirdParty(Achievements achievement)
	{
		return GameServiceController.Instance.IsAchievementUnlocked(achievement);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000284C9 File Offset: 0x000266C9
	private static void RemoveGainedAchievementsRemote()
	{
		if (!AchievementManager.CanWorkWithRemote())
		{
			return;
		}
		if (AchievementManager.mappedAwards.Values.Count != 0)
		{
			AsmodeeLogic.Instance.RemoveAwards(AchievementManager.mappedAwards.Values.ToArray<Award>());
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x000284FD File Offset: 0x000266FD
	private static void RemoveGainedAchievementsThirdParty()
	{
		GameServiceController.Instance.ResetAllAchievements();
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00028509 File Offset: 0x00026709
	public static Achievement GetAchievement(Achievements achievement)
	{
		if (!AchievementManager.mappedAchievements.ContainsKey(achievement))
		{
			return null;
		}
		return AchievementManager.mappedAchievements[achievement];
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00028525 File Offset: 0x00026725
	public static Achievement GetAchievementUnlockingObjectiveCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingObjectiveCards.ContainsKey(cardID))
		{
			return AchievementManager.GetAchievement(AchievementManager.achievementsUnlockingObjectiveCards[cardID]);
		}
		return null;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00028546 File Offset: 0x00026746
	public static Achievement GetAchievementUnlockingEncounterCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingEncounterCards.ContainsKey(cardID))
		{
			return AchievementManager.GetAchievement(AchievementManager.achievementsUnlockingEncounterCards[cardID]);
		}
		return null;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00028567 File Offset: 0x00026767
	public static Achievement GetAchievementUnlockingFactoryCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingFactoryCards.ContainsKey(cardID))
		{
			return AchievementManager.GetAchievement(AchievementManager.achievementsUnlockingFactoryCards[cardID]);
		}
		return null;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00055208 File Offset: 0x00053408
	public static string GetAchievementDescriptionForObjectiveCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingObjectiveCards.ContainsKey(cardID))
		{
			return ScriptLocalization.Get("Achievements/" + AchievementManager.achievementsUnlockingObjectiveCards[cardID].ToString() + "Description");
		}
		return "";
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00055258 File Offset: 0x00053458
	public static string GetAchievementDescriptionForEncounterCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingEncounterCards.ContainsKey(cardID))
		{
			return ScriptLocalization.Get("Achievements/" + AchievementManager.achievementsUnlockingEncounterCards[cardID].ToString() + "Description");
		}
		return "";
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000552A8 File Offset: 0x000534A8
	public static string GetAchievementDescriptionForFactoryCard(int cardID)
	{
		if (AchievementManager.achievementsUnlockingFactoryCards.ContainsKey(cardID))
		{
			return ScriptLocalization.Get("Achievements/" + AchievementManager.achievementsUnlockingFactoryCards[cardID].ToString() + "Description");
		}
		return "";
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00028588 File Offset: 0x00026788
	public static bool ObjectiveCardUnlocked(int cardID)
	{
		return !AchievementManager.achievementsUnlockingObjectiveCards.ContainsKey(cardID) || AchievementManager.IsAchievementUnlocked(AchievementManager.achievementsUnlockingObjectiveCards[cardID], true);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000285AA File Offset: 0x000267AA
	public static bool EncounterCardUnlocked(int cardID)
	{
		return !AchievementManager.achievementsUnlockingEncounterCards.ContainsKey(cardID) || AchievementManager.IsAchievementUnlocked(AchievementManager.achievementsUnlockingEncounterCards[cardID], true);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000285CC File Offset: 0x000267CC
	public static bool FactoryCardUnlocked(int cardID)
	{
		return !AchievementManager.achievementsUnlockingFactoryCards.ContainsKey(cardID) || AchievementManager.IsAchievementUnlocked(AchievementManager.achievementsUnlockingFactoryCards[cardID], true);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000552F8 File Offset: 0x000534F8
	public static int NumberOfUnlockedObjectiveCards()
	{
		int num = 0;
		using (Dictionary<int, Achievements>.KeyCollection.Enumerator enumerator = AchievementManager.achievementsUnlockingObjectiveCards.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (AchievementManager.ObjectiveCardUnlocked(enumerator.Current))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00055358 File Offset: 0x00053558
	public static int NumberOfUnlockedEncounterCards()
	{
		int num = 0;
		using (Dictionary<int, Achievements>.KeyCollection.Enumerator enumerator = AchievementManager.achievementsUnlockingEncounterCards.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (AchievementManager.EncounterCardUnlocked(enumerator.Current))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000553B8 File Offset: 0x000535B8
	public static int NumberOfUnlockedFactoryCards()
	{
		int num = 0;
		using (Dictionary<int, Achievements>.KeyCollection.Enumerator enumerator = AchievementManager.achievementsUnlockingFactoryCards.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (AchievementManager.FactoryCardUnlocked(enumerator.Current))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000285EE File Offset: 0x000267EE
	private static T ParseEnum<T>(string value)
	{
		return (T)((object)Enum.Parse(typeof(T), value, true));
	}

	// Token: 0x040000AA RID: 170
	private static Dictionary<Achievements, Achievement> mappedAchievements = new Dictionary<Achievements, Achievement>();

	// Token: 0x040000AB RID: 171
	private static Dictionary<Achievements, Award> mappedAwards = new Dictionary<Achievements, Award>();

	// Token: 0x040000AC RID: 172
	private const string UNLOCKED_ACHIEVEMENTS = "UnlockedAchievements";

	// Token: 0x040000AD RID: 173
	private const string ACHIEVEMENT_MANAGER = "[Achievement Manager]";

	// Token: 0x040000AE RID: 174
	private const string GAME_CODE = "scte";

	// Token: 0x040000AF RID: 175
	private const string NAME = "Name";

	// Token: 0x040000B0 RID: 176
	private const string DESCRIPTION = "Description";

	// Token: 0x040000B1 RID: 177
	private static bool debugMessages = false;

	// Token: 0x040000B2 RID: 178
	private static Dictionary<int, Achievements> achievementsUnlockingObjectiveCards = new Dictionary<int, Achievements>
	{
		{
			24,
			Achievements.FactoryInControl
		},
		{
			25,
			Achievements.PowerStar
		},
		{
			26,
			Achievements.WinMostTiles
		},
		{
			27,
			Achievements.WinHighStructureBonus
		}
	};

	// Token: 0x040000B3 RID: 179
	private static Dictionary<int, Achievements> achievementsUnlockingEncounterCards = new Dictionary<int, Achievements>
	{
		{
			29,
			Achievements.WinEnemy6Stars
		},
		{
			30,
			Achievements.PopularityStar
		},
		{
			31,
			Achievements.WinMultipleCombats
		},
		{
			32,
			Achievements.StructureStar
		},
		{
			33,
			Achievements.WinNoUpgrades
		},
		{
			34,
			Achievements.FirstInFactory
		},
		{
			35,
			Achievements.WinLowPopularity
		},
		{
			36,
			Achievements.ObjectiveStar
		}
	};

	// Token: 0x040000B4 RID: 180
	private static Dictionary<int, Achievements> achievementsUnlockingFactoryCards = new Dictionary<int, Achievements>
	{
		{
			13,
			Achievements.WinMostResources
		},
		{
			14,
			Achievements.WorkersStar
		},
		{
			15,
			Achievements.BattleStars
		},
		{
			16,
			Achievements.UpgradeStar
		},
		{
			17,
			Achievements.MechStar
		},
		{
			18,
			Achievements.EnlistStar
		}
	};

	// Token: 0x040000B5 RID: 181
	private static Dictionary<Faction, int[,]> bases = new Dictionary<Faction, int[,]>
	{
		{
			Faction.Polania,
			new int[,]
			{
				{ 1, 3 },
				{ 0, 4 },
				{ 1, 4 }
			}
		},
		{
			Faction.Albion,
			new int[,]
			{
				{ 1, 1 },
				{ 2, 1 },
				{ 1, 2 }
			}
		},
		{
			Faction.Nordic,
			new int[,]
			{
				{ 4, 1 },
				{ 5, 1 },
				{ 4, 2 }
			}
		},
		{
			Faction.Rusviet,
			new int[,]
			{
				{ 6, 3 },
				{ 5, 4 },
				{ 6, 4 }
			}
		},
		{
			Faction.Togawa,
			new int[,]
			{
				{ 5, 6 },
				{ 6, 6 },
				{ 6, 7 }
			}
		},
		{
			Faction.Crimea,
			new int[,]
			{
				{ 3, 7 },
				{ 4, 7 },
				{ 3, 8 }
			}
		},
		{
			Faction.Saxony,
			new int[,]
			{
				{ 0, 6 },
				{ 1, 6 },
				{ 1, 7 }
			}
		}
	};
}
