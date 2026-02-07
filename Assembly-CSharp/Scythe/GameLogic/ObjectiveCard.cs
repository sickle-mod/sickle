using System;
using System.Collections.Generic;
using System.Xml;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005E2 RID: 1506
	public class ObjectiveCard : Card
	{
		// Token: 0x06002FDD RID: 12253 RVA: 0x00045E37 File Offset: 0x00044037
		public ObjectiveCard(GameManager gameManager, int cardId)
		{
			this.cardId = cardId;
			this.gameManager = gameManager;
			this.status = ObjectiveCard.ObjectiveStatus.Open;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x00045E54 File Offset: 0x00044054
		public ObjectiveCard(GameManager gameManager, int cardId, ObjectiveCard.ObjectiveStatus status)
		{
			this.cardId = cardId;
			this.gameManager = gameManager;
			this.status = status;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x00045E71 File Offset: 0x00044071
		public void SetPlayer(Player player)
		{
			this.player = player;
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x00123EC8 File Offset: 0x001220C8
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.player != null)
			{
				int num = (int)this.status;
				writer.WriteValue(num.ToString() + " ");
			}
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x00123F04 File Offset: 0x00122104
		public bool CanDoActionAfterCompletingObjective()
		{
			switch (this.cardId)
			{
			case 1:
				return false;
			case 2:
				return false;
			case 3:
				return false;
			case 4:
				return false;
			case 5:
				return false;
			case 6:
				return true;
			case 7:
				return true;
			case 8:
				return true;
			case 9:
				return false;
			case 10:
				return false;
			case 11:
				return false;
			case 12:
				return false;
			case 13:
				return true;
			case 14:
				return false;
			case 15:
				return true;
			case 16:
				return false;
			case 17:
				return false;
			case 18:
				return true;
			case 19:
				return false;
			case 20:
				return true;
			case 21:
				return false;
			case 22:
				return true;
			case 23:
				return true;
			case 24:
				return false;
			case 25:
				return true;
			case 26:
				return false;
			case 27:
				return true;
			case 28:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00123FCC File Offset: 0x001221CC
		public bool CheckCondition()
		{
			if (this.status != ObjectiveCard.ObjectiveStatus.Open)
			{
				return false;
			}
			if (this.IsPlayerCharacterInFactoryButNotObtainedFactoryCard())
			{
				return false;
			}
			switch (this.cardId)
			{
			case 1:
				return this.IsControllingTerriories(HexType.mountain, 3);
			case 2:
				return this.IsControllingTunels(3);
			case 3:
				return this.IsControllingTerriories(HexType.farm, 3);
			case 4:
				return this.IsControllingTerriories(HexType.tundra, 3);
			case 5:
				return this.IsControllingTerritory(HexType.factory) && this.IsPlayerStrongest();
			case 6:
				return this.player.wonBattle && this.player.Power >= 7;
			case 7:
				return this.player.matPlayer.GetPlayerMatSection(4) != null && this.player.matFaction.mechs.Count >= 1 && this.player.matPlayer.workers.Count <= 3;
			case 8:
				return this.player.Coins <= 2 && this.player.matPlayer.workers.Count >= 4 && this.player.Popularity >= 7;
			case 9:
				return this.IsControllingTerritoryWithResources(9);
			case 10:
				return this.IsControllingTerriories(HexType.forest, 3);
			case 11:
				return this.IsControllingTerriories(HexType.village, 3);
			case 12:
				return this.player.Coins >= 20;
			case 13:
				return this.IsControllingTerritoryWithTokens(6);
			case 14:
				return this.HasEachOfResourceType() && (this.player.matFaction.mechs.Count >= 1 && this.player.matPlayer.buildings.Count >= 1 && this.AmountOfRecruits() >= 1) && this.player.matPlayer.UpgradesDone >= 1;
			case 15:
				return this.AmountOfRecruits() == this.player.matPlayer.workers.Count;
			case 16:
				if (this.player.character.position.Owner == this.player)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.position == this.player.character.position)
							{
								num++;
							}
						}
					}
					using (List<Worker>.Enumerator enumerator2 = this.player.matPlayer.workers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.position == this.player.character.position)
							{
								num2++;
							}
						}
					}
					foreach (KeyValuePair<ResourceType, int> keyValuePair in this.player.character.position.resources)
					{
						num3 += keyValuePair.Value;
					}
					if (num3 >= 5 && num2 == 1 && num == 1)
					{
						return true;
					}
				}
				return false;
			case 17:
				return this.player.matPlayer.GetPlayerMatSection(4) == null && this.IsControllingSurroundingTerritories(HexType.factory, 2);
			case 18:
				return this.player.Power == 0 && this.player.Popularity >= 13 && this.player.matPlayer.workers.Count >= 5;
			case 19:
				return this.IsControllingSurroundingTerritories(HexType.lake, 5);
			case 20:
				return this.player.matPlayer.buildings.Count >= 3 && this.player.Popularity >= 7 && this.player.matFaction.mechs.Count == 0;
			case 21:
			{
				using (Dictionary<ResourceType, int>.ValueCollection.Enumerator enumerator4 = this.player.Resources(false).Values.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (enumerator4.Current >= 9)
						{
							return true;
						}
					}
				}
				return false;
			}
			case 22:
				return this.player.matPlayer.GetPlayerMatSection(4) != null && this.player.matPlayer.UpgradesDone == 0;
			case 23:
				return this.player.combatCards.Count >= 8 && this.player.GetNumberOfStars(StarType.Combat) >= 1;
			case 24:
				foreach (GameHex gameHex in this.gameManager.gameBoard.factory.GetNeighboursAll())
				{
					if (this.IsControllingTerritoryWithUnits<Worker>(gameHex, this.player.matPlayer.workers, 5))
					{
						return true;
					}
				}
				return false;
			case 25:
				return this.player.Popularity <= 3 && this.player.Power >= 13 && this.player.matFaction.mechs.Count >= 2;
			case 26:
			{
				HashSet<HexType> hashSet = new HashSet<HexType>();
				foreach (Worker worker in this.player.matPlayer.workers)
				{
					hashSet.Add(worker.position.hexType);
				}
				hashSet.Remove(HexType.factory);
				hashSet.Remove(HexType.forbidden);
				hashSet.Remove(HexType.capital);
				hashSet.Remove(HexType.lake);
				return hashSet.Count == 5;
			}
			case 27:
			{
				List<Building> buildings = this.player.matPlayer.buildings;
				if (buildings.Count < 3)
				{
					return false;
				}
				int num4 = 0;
				List<GameHex> neighboursAll = this.gameManager.gameBoard.bases[this.player.matFaction.faction].GetNeighboursAll();
				foreach (Building building in buildings)
				{
					if (neighboursAll.Contains(building.position))
					{
						num4++;
					}
				}
				return buildings.Count >= 3 && num4 < 1;
			}
			case 28:
				return this.IsControllingSurroundingHexTerritories(this.gameManager.gameBoard.hexMap[0, 3], 2);
			default:
				return false;
			}
			bool flag;
			return flag;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x00045E7A File Offset: 0x0004407A
		private bool IsPlayerCharacterInFactoryButNotObtainedFactoryCard()
		{
			return this.player.character.position.hexType == HexType.factory && this.player.matPlayer.matPlayerSectionsCount != 5;
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x001246A4 File Offset: 0x001228A4
		private bool IsControllingTerriories(HexType type, int amount)
		{
			HashSet<GameHex> hashSet = this.player.OwnedFields(false);
			int num = 0;
			using (HashSet<GameHex>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.hexType == type)
					{
						num++;
					}
				}
			}
			return num >= amount;
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0012470C File Offset: 0x0012290C
		private bool IsControllingTerritory(HexType type)
		{
			using (HashSet<GameHex>.Enumerator enumerator = this.player.OwnedFields(false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.hexType == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x0012476C File Offset: 0x0012296C
		private bool IsControllingTerritory<T>(HexType type, List<T> units) where T : Unit
		{
			using (List<T>.Enumerator enumerator = units.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position.hexType == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x001247CC File Offset: 0x001229CC
		private bool IsControllingTunels(int amount)
		{
			HashSet<GameHex> hashSet = this.player.OwnedFields(false);
			int num = 0;
			using (HashSet<GameHex>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.hasTunnel)
					{
						num++;
					}
				}
			}
			return num >= amount;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x00124834 File Offset: 0x00122A34
		private bool IsPlayerStrongest()
		{
			foreach (Player player in this.gameManager.GetPlayers())
			{
				if (this.player.Power < player.Power)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x001248A0 File Offset: 0x00122AA0
		private bool IsControllingTerritoryWithResources(int amount)
		{
			foreach (GameHex gameHex in this.player.OwnedFields(false))
			{
				if (this.HasEachOfResourceTypeOnHex(gameHex))
				{
					int num = 0;
					foreach (KeyValuePair<ResourceType, int> keyValuePair in gameHex.resources)
					{
						num += keyValuePair.Value;
					}
					if (num >= amount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x00124950 File Offset: 0x00122B50
		private bool HasEachOfResourceTypeOnHex(GameHex hex)
		{
			int num = 0;
			foreach (KeyValuePair<ResourceType, int> keyValuePair in hex.resources)
			{
				if (keyValuePair.Value <= 0)
				{
					break;
				}
				if (++num == 4)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x001249BC File Offset: 0x00122BBC
		private bool HasEachOfResourceType()
		{
			foreach (KeyValuePair<ResourceType, int> keyValuePair in this.player.Resources(false))
			{
				if (keyValuePair.Value <= 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x00124A20 File Offset: 0x00122C20
		private int AmountOfRecruits()
		{
			int num = 0;
			foreach (GainType gainType in this.player.matFaction.OneTimeBonuses.Keys)
			{
				if (this.player.matFaction.OneTimeBonusUsed(gainType))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x00124A98 File Offset: 0x00122C98
		private bool IsControllingTerritoryWithTokens(int amount)
		{
			Dictionary<GameHex, int> dictionary = new Dictionary<GameHex, int>();
			List<Worker> workers = this.player.matPlayer.workers;
			List<Mech> mechs = this.player.matFaction.mechs;
			List<Building> buildings = this.player.matPlayer.buildings;
			foreach (Worker worker in workers)
			{
				if (!dictionary.ContainsKey(worker.position))
				{
					dictionary.Add(worker.position, 1);
				}
				else
				{
					Dictionary<GameHex, int> dictionary2 = dictionary;
					GameHex gameHex = worker.position;
					int num = dictionary2[gameHex] + 1;
					dictionary2[gameHex] = num;
				}
			}
			foreach (Mech mech in mechs)
			{
				if (!dictionary.ContainsKey(mech.position))
				{
					dictionary.Add(mech.position, 1);
				}
				else
				{
					Dictionary<GameHex, int> dictionary3 = dictionary;
					GameHex gameHex = mech.position;
					int num = dictionary3[gameHex] + 1;
					dictionary3[gameHex] = num;
				}
			}
			foreach (Building building in buildings)
			{
				if (!dictionary.ContainsKey(building.position))
				{
					dictionary.Add(building.position, 1);
				}
				else
				{
					Dictionary<GameHex, int> dictionary4 = dictionary;
					GameHex gameHex = building.position;
					int num = dictionary4[gameHex] + 1;
					dictionary4[gameHex] = num;
				}
			}
			if (!dictionary.ContainsKey(this.player.character.position))
			{
				dictionary.Add(this.player.character.position, 1);
			}
			else
			{
				Dictionary<GameHex, int> dictionary5 = dictionary;
				GameHex gameHex = this.player.character.position;
				int num = dictionary5[gameHex] + 1;
				dictionary5[gameHex] = num;
			}
			using (Dictionary<GameHex, int>.ValueCollection.Enumerator enumerator4 = dictionary.Values.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					if (enumerator4.Current >= amount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x00124CE4 File Offset: 0x00122EE4
		private bool IsControllingTerritoryWithUnit<T>(GameHex hex, List<T> units) where T : Unit
		{
			using (List<T>.Enumerator enumerator = units.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == hex)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x00124D40 File Offset: 0x00122F40
		private bool IsControllingTerritoryWithUnits<T>(GameHex hex, List<T> units, int amount) where T : Unit
		{
			int num = 0;
			using (List<T>.Enumerator enumerator = units.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == hex)
					{
						num++;
					}
				}
			}
			return num >= amount;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x00124DA0 File Offset: 0x00122FA0
		private bool IsControllingSurroundingTerritories(HexType type, int amount)
		{
			GameHex[,] hexMap = this.gameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex = hexMap[i, j];
					if (gameHex.hexType == type && this.NumberOfSurroundingTerritories(gameHex) >= amount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x00124E1C File Offset: 0x0012301C
		private int NumberOfSurroundingTerritories(GameHex hex)
		{
			List<GameHex> neighboursAll = hex.GetNeighboursAll();
			HashSet<GameHex> hashSet = this.player.OwnedFields(false);
			int num = 0;
			foreach (GameHex gameHex in neighboursAll)
			{
				if (gameHex.hexType != HexType.capital && hashSet.Contains(gameHex))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x00045EAB File Offset: 0x000440AB
		private bool IsControllingSurroundingHexTerritories(GameHex hex, int amount)
		{
			return this.NumberOfSurroundingTerritories(hex) >= amount;
		}

		// Token: 0x0400208C RID: 8332
		private Player player;

		// Token: 0x0400208D RID: 8333
		private GameManager gameManager;

		// Token: 0x0400208E RID: 8334
		public ObjectiveCard.ObjectiveStatus status;

		// Token: 0x020005E3 RID: 1507
		public enum ObjectiveStatus
		{
			// Token: 0x04002090 RID: 8336
			Open,
			// Token: 0x04002091 RID: 8337
			Completed,
			// Token: 0x04002092 RID: 8338
			Disabled
		}
	}
}
