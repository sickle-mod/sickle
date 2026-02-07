using System;
using System.Text;

namespace Scythe.GameLogic
{
	// Token: 0x020005E7 RID: 1511
	public class PlayerEndGameStats
	{
		// Token: 0x0600301E RID: 12318 RVA: 0x0004606E File Offset: 0x0004426E
		public PlayerEndGameStats(bool ranked)
		{
			this.ranked = ranked;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x00126428 File Offset: 0x00124628
		public string Serialize()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", new object[]
			{
				(int)this.player.matFaction.faction,
				this.resourcePoints,
				this.starPoints,
				this.territoryPoints,
				this.coinPoints,
				this.structurePoints,
				this.totalPoints,
				this.oldRating,
				this.rating,
				this.player.Name,
				this.player.Popularity
			});
			return stringBuilder.ToString();
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x00126500 File Offset: 0x00124700
		public static PlayerEndGameStats Deserialize(string data, GameManager gameManager)
		{
			string[] array = data.Split(' ', StringSplitOptions.None);
			return new PlayerEndGameStats(gameManager.IsRanked)
			{
				player = gameManager.GetPlayerByFaction((Faction)int.Parse(array[0])),
				resourcePoints = int.Parse(array[1]),
				starPoints = int.Parse(array[2]),
				territoryPoints = int.Parse(array[3]),
				coinPoints = int.Parse(array[4]),
				structurePoints = int.Parse(array[5]),
				totalPoints = int.Parse(array[6]),
				oldRating = int.Parse(array[7]),
				rating = int.Parse(array[8])
			};
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x001265A8 File Offset: 0x001247A8
		public static PlayerEndGameStats Deserialize(string data)
		{
			string[] array = data.Split(' ', StringSplitOptions.None);
			PlayerEndGameStats playerEndGameStats = new PlayerEndGameStats(array.Length > 7);
			playerEndGameStats.faction = int.Parse(array[0]);
			playerEndGameStats.resourcePoints = int.Parse(array[1]);
			playerEndGameStats.starPoints = int.Parse(array[2]);
			playerEndGameStats.territoryPoints = int.Parse(array[3]);
			playerEndGameStats.coinPoints = int.Parse(array[4]);
			playerEndGameStats.structurePoints = int.Parse(array[5]);
			playerEndGameStats.totalPoints = int.Parse(array[6]);
			playerEndGameStats.oldRating = int.Parse(array[7]);
			playerEndGameStats.rating = int.Parse(array[8]);
			playerEndGameStats.name = array[9];
			for (int i = 10; i < array.Length - 1; i++)
			{
				PlayerEndGameStats playerEndGameStats2 = playerEndGameStats;
				playerEndGameStats2.name = playerEndGameStats2.name + " " + array[i];
			}
			playerEndGameStats.popularity = int.Parse(array[array.Length - 1]);
			return playerEndGameStats;
		}

		// Token: 0x040020C5 RID: 8389
		public Player player;

		// Token: 0x040020C6 RID: 8390
		public int resourcePoints;

		// Token: 0x040020C7 RID: 8391
		public int starPoints;

		// Token: 0x040020C8 RID: 8392
		public int territoryPoints;

		// Token: 0x040020C9 RID: 8393
		public int coinPoints;

		// Token: 0x040020CA RID: 8394
		public int structurePoints;

		// Token: 0x040020CB RID: 8395
		public int totalPoints;

		// Token: 0x040020CC RID: 8396
		public int oldRating;

		// Token: 0x040020CD RID: 8397
		public int rating;

		// Token: 0x040020CE RID: 8398
		public int units;

		// Token: 0x040020CF RID: 8399
		public int resources;

		// Token: 0x040020D0 RID: 8400
		public int territories;

		// Token: 0x040020D1 RID: 8401
		public int faction;

		// Token: 0x040020D2 RID: 8402
		public string name = string.Empty;

		// Token: 0x040020D3 RID: 8403
		public int popularity;

		// Token: 0x040020D4 RID: 8404
		public int place;

		// Token: 0x040020D5 RID: 8405
		private bool ranked;
	}
}
