using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200033A RID: 826
	[Serializable]
	public class PlayerStats
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x00038279 File Offset: 0x00036479
		// (set) Token: 0x060017B6 RID: 6070 RVA: 0x00038281 File Offset: 0x00036481
		public string GamesHistory { get; set; }

		// Token: 0x060017B8 RID: 6072 RVA: 0x0003828A File Offset: 0x0003648A
		public List<GameHistoryEntry> GetGamesHistory()
		{
			return JsonConvert.DeserializeObject<List<GameHistoryEntry>>(this.GamesHistory);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0009FEA8 File Offset: 0x0009E0A8
		public static PlayerStats FromJson(string json)
		{
			PlayerStats playerStats;
			try
			{
				playerStats = JsonConvert.DeserializeObject<PlayerStats>(json);
			}
			catch
			{
				playerStats = PlayerStats.CastOldStatsToPlayerStats(JsonConvert.DeserializeObject<PlayerStatsOld>(json));
			}
			playerStats.ELOList = JsonConvert.DeserializeObject<List<int>>(playerStats.ELOListString);
			playerStats.GamesList = JsonConvert.DeserializeObject<List<int>>(playerStats.GamesListString);
			playerStats.PlayerFactionStats = new PlayerFactionStats[7][];
			int num = 0;
			for (int i = 0; i < 7; i++)
			{
				playerStats.PlayerFactionStats[i] = new PlayerFactionStats[7];
				for (int j = 0; j < 5; j++)
				{
					playerStats.PlayerFactionStats[i][j] = JsonConvert.DeserializeObject<PlayerFactionStats>(playerStats.PlayerFactionStatsString[num++], new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.Ignore,
						NullValueHandling = NullValueHandling.Ignore
					});
				}
			}
			for (int k = 0; k < 7; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					if (playerStats.PlayerFactionStatsString.Count > num)
					{
						playerStats.PlayerFactionStats[k][l + 5] = JsonConvert.DeserializeObject<PlayerFactionStats>(playerStats.PlayerFactionStatsString[num++], new JsonSerializerSettings
						{
							DefaultValueHandling = DefaultValueHandling.Ignore,
							NullValueHandling = NullValueHandling.Ignore
						});
					}
					else
					{
						num++;
						playerStats.PlayerFactionStats[k][l + 5] = new PlayerFactionStats();
					}
				}
			}
			return playerStats;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0009FFE4 File Offset: 0x0009E1E4
		public static string ToJson(PlayerStats playerStats)
		{
			playerStats.PlayerFactionStatsString = new List<string>(49);
			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					playerStats.PlayerFactionStatsString.Add(JsonConvert.SerializeObject(playerStats.PlayerFactionStats[i][j], new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
						NullValueHandling = NullValueHandling.Ignore
					}));
				}
			}
			for (int k = 0; k < 7; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					playerStats.PlayerFactionStatsString.Add(JsonConvert.SerializeObject(playerStats.PlayerFactionStats[k][l + 5], new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
						NullValueHandling = NullValueHandling.Ignore
					}));
				}
			}
			playerStats.GamesListString = JsonConvert.SerializeObject(playerStats.GamesList);
			playerStats.ELOListString = JsonConvert.SerializeObject(playerStats.ELOList);
			return JsonConvert.SerializeObject(playerStats);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000A00B4 File Offset: 0x0009E2B4
		private static PlayerStats CastOldStatsToPlayerStats(PlayerStatsOld playerStatsOld)
		{
			return new PlayerStats
			{
				Id = default(Guid),
				Name = playerStatsOld.Name,
				ELO = playerStatsOld.ELO,
				Karma = playerStatsOld.Karma,
				RankingPosition = playerStatsOld.RankingPosition,
				RankedGames = playerStatsOld.RankedGames,
				RankedTopScore = playerStatsOld.RankedTopScore,
				RankedFirstPlaceStreak = playerStatsOld.RankedFirstPlaceStreak,
				ELOList = playerStatsOld.ELOList,
				GamesList = playerStatsOld.GamesList,
				PlayerFactionStats = playerStatsOld.PlayerFactionStats,
				ELOListString = playerStatsOld.ELOListString,
				GamesListString = playerStatsOld.GamesListString,
				PlayerFactionStatsString = playerStatsOld.PlayerFactionStatsString,
				GamesHistory = playerStatsOld.GamesHistory
			};
		}

		// Token: 0x040011A7 RID: 4519
		public Guid Id;

		// Token: 0x040011A8 RID: 4520
		public string Name;

		// Token: 0x040011A9 RID: 4521
		public int ELO;

		// Token: 0x040011AA RID: 4522
		public int Karma;

		// Token: 0x040011AB RID: 4523
		public long RankingPosition;

		// Token: 0x040011AC RID: 4524
		public int RankedGames;

		// Token: 0x040011AD RID: 4525
		public int RankedTopScore;

		// Token: 0x040011AE RID: 4526
		public int RankedFirstPlaceStreak;

		// Token: 0x040011AF RID: 4527
		public List<int> ELOList;

		// Token: 0x040011B0 RID: 4528
		public List<int> GamesList;

		// Token: 0x040011B1 RID: 4529
		public PlayerFactionStats[][] PlayerFactionStats;

		// Token: 0x040011B2 RID: 4530
		public string ELOListString;

		// Token: 0x040011B3 RID: 4531
		public string GamesListString;

		// Token: 0x040011B4 RID: 4532
		public List<string> PlayerFactionStatsString;
	}
}
