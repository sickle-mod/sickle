using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005C5 RID: 1477
	public class GameHex : IXmlSerializable
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x0004557B File Offset: 0x0004377B
		// (set) Token: 0x06002F00 RID: 12032 RVA: 0x00045583 File Offset: 0x00043783
		public Building Building { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06002F01 RID: 12033 RVA: 0x0004558C File Offset: 0x0004378C
		// (set) Token: 0x06002F02 RID: 12034 RVA: 0x00045594 File Offset: 0x00043794
		public FactionAbilityToken Token { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06002F03 RID: 12035 RVA: 0x0004559D File Offset: 0x0004379D
		// (set) Token: 0x06002F04 RID: 12036 RVA: 0x000455A5 File Offset: 0x000437A5
		public Player Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x000455AE File Offset: 0x000437AE
		// (set) Token: 0x06002F06 RID: 12038 RVA: 0x000455B6 File Offset: 0x000437B6
		public Player Enemy { get; set; }

		// Token: 0x06002F07 RID: 12039 RVA: 0x0011CF44 File Offset: 0x0011B144
		public GameHex(GameManager gameManager)
		{
			this.Building = null;
			this.Token = null;
			this.resources.Add(ResourceType.food, 0);
			this.resources.Add(ResourceType.metal, 0);
			this.resources.Add(ResourceType.oil, 0);
			this.resources.Add(ResourceType.wood, 0);
			this.gameManager = gameManager;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x0011CFC0 File Offset: 0x0011B1C0
		public GameHex GetNeighbour(EdgeMask direction)
		{
			int num = this.posY % 2;
			if (direction <= EdgeMask.NE)
			{
				switch (direction)
				{
				case EdgeMask.NW:
					return this.GetNeighbour(-num, -1);
				case EdgeMask.W:
					return this.GetNeighbour(-1, 0);
				case (EdgeMask)3:
					break;
				case EdgeMask.SW:
					return this.GetNeighbour(-num, 1);
				default:
					if (direction == EdgeMask.NE)
					{
						return this.GetNeighbour(1 - num, -1);
					}
					break;
				}
			}
			else
			{
				if (direction == EdgeMask.E)
				{
					return this.GetNeighbour(1, 0);
				}
				if (direction == EdgeMask.SE)
				{
					return this.GetNeighbour(1 - num, 1);
				}
			}
			return null;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000455BF File Offset: 0x000437BF
		public bool Conflict(Unit unit)
		{
			return this.Owner != unit.Owner && this.GetOwnerUnitCount() > 0;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x0011D044 File Offset: 0x0011B244
		public HashSet<GameHex> GetFieldsAccessible(Unit unit, bool riverwalkUsed = false)
		{
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			if (this.owner == null)
			{
				this.UpdateOwnership();
			}
			if (this.Conflict(unit))
			{
				return hashSet;
			}
			if (unit is Character && this.hasEncounter && !this.encounterUsed)
			{
				return hashSet;
			}
			if (this.Token != null && this.Token is TrapToken && this.Token.Owner != unit.Owner && (this.Token as TrapToken).Armed)
			{
				return hashSet;
			}
			foreach (GameHex gameHex in this.GetNeighboursAll())
			{
				if (gameHex.hexType != HexType.capital)
				{
					if (this.IsRiverBetween(gameHex))
					{
						if ((unit is Character || unit is Mech) && unit.Owner.matFaction.CanRiverwalk(this, gameHex, unit))
						{
							if (unit.Owner.matFaction.faction != Faction.Togawa || !riverwalkUsed)
							{
								hashSet.Add(gameHex);
							}
						}
						else if (unit.Owner.matFaction.factionPerk == AbilityPerk.Swim && unit is Worker)
						{
							hashSet.Add(gameHex);
						}
					}
					else if (gameHex.hexType == HexType.lake)
					{
						if ((unit is Character || unit is Mech) && (unit.Owner.matFaction.abilities.Contains(AbilityPerk.Seaworthy) || unit.Owner.matFaction.abilities.Contains(AbilityPerk.Submerge) || unit.Owner.matFaction.abilities.Contains(AbilityPerk.Suiton)) && unit.Owner.matFaction.SkillUnlocked[1])
						{
							hashSet.Add(gameHex);
						}
					}
					else
					{
						hashSet.Add(gameHex);
					}
				}
			}
			if (this.hasTunnel || (this.Building != null && this.Building.buildingType == BuildingType.Mine && this.Building.player == unit.Owner))
			{
				foreach (GameHex gameHex2 in this.gameBoard.tunnels)
				{
					if (!hashSet.Contains(gameHex2))
					{
						hashSet.Add(gameHex2);
					}
				}
				if (unit.Owner.matPlayer.GetBuilding(BuildingType.Mine) != null && !hashSet.Contains(unit.Owner.matPlayer.GetBuilding(BuildingType.Mine).position))
				{
					hashSet.Add(unit.Owner.matPlayer.GetBuilding(BuildingType.Mine).position);
				}
			}
			if (unit is Character || unit is Mech)
			{
				if (unit.Owner.matFaction.SkillUnlocked[1])
				{
					if (this.hexType == HexType.lake && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Submerge))
					{
						foreach (GameHex gameHex3 in this.gameBoard.lakes)
						{
							if (!hashSet.Contains(gameHex3))
							{
								hashSet.Add(gameHex3);
							}
						}
					}
					if (this.hexType == HexType.village && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Township))
					{
						if (!hashSet.Contains(this.gameBoard.factory))
						{
							hashSet.Add(this.gameBoard.factory);
						}
						foreach (GameHex gameHex4 in this.gameBoard.villages)
						{
							if (gameHex4.Owner == unit.Owner && !hashSet.Contains(gameHex4))
							{
								hashSet.Add(gameHex4);
							}
						}
					}
					if (this.hexType == HexType.factory && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Township))
					{
						foreach (GameHex gameHex5 in this.gameBoard.villages)
						{
							if (gameHex5.Owner == unit.Owner && !hashSet.Contains(gameHex5))
							{
								hashSet.Add(gameHex5);
							}
						}
					}
					if (this.hexType == HexType.mountain && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Underpass))
					{
						foreach (GameHex gameHex6 in this.gameBoard.tunnels)
						{
							if (!hashSet.Contains(gameHex6))
							{
								hashSet.Add(gameHex6);
							}
						}
						foreach (GameHex gameHex7 in this.gameBoard.mountains)
						{
							if (gameHex7.Owner == unit.Owner && !hashSet.Contains(gameHex7))
							{
								hashSet.Add(gameHex7);
							}
						}
						foreach (GameHex gameHex8 in unit.Owner.FieldsWithPlayerBuildings())
						{
							if (gameHex8.Building.buildingType == BuildingType.Mine && !hashSet.Contains(gameHex8))
							{
								hashSet.Add(gameHex8);
							}
						}
					}
					if ((this.hasTunnel || this.HasTunnelAccess(unit)) && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Underpass))
					{
						foreach (GameHex gameHex9 in this.gameBoard.mountains)
						{
							if (gameHex9.Owner == unit.Owner && !hashSet.Contains(gameHex9))
							{
								hashSet.Add(gameHex9);
							}
						}
					}
					if (unit.Owner.matFaction.abilities.Contains(AbilityPerk.Wayfare))
					{
						if (this.gameManager.players.Count < 6)
						{
							using (Dictionary<Faction, GameHex>.ValueCollection.Enumerator enumerator3 = this.gameBoard.bases.Values.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									GameHex gameHex10 = enumerator3.Current;
									if (gameHex10.hexType == HexType.capital && (this.gameManager.GetPlayerByFaction(gameHex10.factionBase) == null || gameHex10.factionBase == unit.Owner.matFaction.faction))
									{
										hashSet.Add(gameHex10);
									}
								}
								goto IL_06F9;
							}
						}
						if (this.gameManager.players.Count >= 6)
						{
							foreach (GameHex gameHex11 in this.gameBoard.farms)
							{
								if (!hashSet.Contains(gameHex11) && gameHex11.GetOwnerUnitCount() == 0 && gameHex11.GetEnemyUnitCount() == 0)
								{
									hashSet.Add(gameHex11);
								}
							}
						}
					}
				}
				IL_06F9:
				if (unit.Owner.matFaction.SkillUnlocked[3])
				{
					if (unit.Owner.matFaction.abilities.Contains(AbilityPerk.Rally))
					{
						foreach (FactionAbilityToken factionAbilityToken in unit.Owner.matFaction.FactionTokens.GetPlacedTokens())
						{
							if (!hashSet.Contains(factionAbilityToken.Position))
							{
								hashSet.Add(factionAbilityToken.Position);
							}
						}
						foreach (Worker worker in unit.Owner.matPlayer.workers)
						{
							if (!hashSet.Contains(worker.position) && worker.position.hexType != HexType.capital)
							{
								hashSet.Add(worker.position);
							}
						}
					}
					if (unit.Owner.matFaction.abilities.Contains(AbilityPerk.Shinobi))
					{
						foreach (FactionAbilityToken factionAbilityToken2 in unit.Owner.matFaction.FactionTokens.GetPlacedTokens())
						{
							if (!hashSet.Contains(factionAbilityToken2.Position))
							{
								hashSet.Add(factionAbilityToken2.Position);
							}
						}
					}
				}
			}
			if (unit is Worker)
			{
				List<GameHex> list = new List<GameHex>();
				foreach (GameHex gameHex12 in hashSet)
				{
					if (gameHex12.Conflict(unit))
					{
						list.Add(gameHex12);
					}
				}
				foreach (GameHex gameHex13 in list)
				{
					hashSet.Remove(gameHex13);
				}
			}
			if (hashSet.Contains(this))
			{
				hashSet.Remove(this);
			}
			return hashSet;
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x0011DAD4 File Offset: 0x0011BCD4
		public List<GameHex> GetNeighboursAll()
		{
			List<GameHex> list = new List<GameHex>();
			this.AddNeighbour(list, EdgeMask.NW);
			this.AddNeighbour(list, EdgeMask.W);
			this.AddNeighbour(list, EdgeMask.SW);
			this.AddNeighbour(list, EdgeMask.NE);
			this.AddNeighbour(list, EdgeMask.E);
			this.AddNeighbour(list, EdgeMask.SE);
			return list;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x0011DB1C File Offset: 0x0011BD1C
		private void AddNeighbour(List<GameHex> neighbours, EdgeMask direction)
		{
			GameHex neighbour = this.GetNeighbour(direction);
			if (neighbour != null)
			{
				neighbours.Add(neighbour);
			}
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x0011DB3C File Offset: 0x0011BD3C
		private GameHex GetNeighbour(int deltaX, int deltaY)
		{
			int num = this.posX + deltaX;
			int num2 = this.posY + deltaY;
			if (num >= 0 && num < 8 && num2 >= 0 && num2 < 9 && this.gameBoard.hexMap[num, num2].hexType != HexType.forbidden)
			{
				return this.gameBoard.hexMap[num, num2];
			}
			return null;
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x0011DB9C File Offset: 0x0011BD9C
		public bool IsRiverBetween(GameHex hex)
		{
			foreach (EdgeMask edgeMask in Enum.GetValues(typeof(EdgeMask)).Cast<EdgeMask>())
			{
				if (this.GetNeighbour(edgeMask) == hex)
				{
					if ((edgeMask & (EdgeMask)this.riverMask) != (EdgeMask)0)
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x0011DC10 File Offset: 0x0011BE10
		public bool CanMoveThroughTunnels(GameHex hex, Unit unit)
		{
			return (this.hasTunnel || (this.Building != null && this.Building.buildingType == BuildingType.Mine && this.Building.player == unit.Owner)) && (hex.hasTunnel || (hex.Building != null && hex.Building.buildingType == BuildingType.Mine && hex.Building.player == unit.Owner));
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000455DA File Offset: 0x000437DA
		public bool HasTunnelAccess(Unit unit)
		{
			return this.hasTunnel || (this.Building != null && this.Building.player == unit.Owner && this.Building.buildingType == BuildingType.Mine);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00045611 File Offset: 0x00043811
		public bool HasFactionToken(Player player)
		{
			return this.Token != null && this.Token.Owner == player;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x0011DC84 File Offset: 0x0011BE84
		public List<Worker> GetOwnerWorkers()
		{
			List<Worker> list = new List<Worker>();
			if (this.Owner != null)
			{
				foreach (Worker worker in this.Owner.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						list.Add(worker);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x0011DD18 File Offset: 0x0011BF18
		public List<Mech> GetOwnerMechs()
		{
			List<Mech> list = new List<Mech>();
			if (this.Owner != null)
			{
				foreach (Mech mech in this.Owner.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						list.Add(mech);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x0011DDAC File Offset: 0x0011BFAC
		public List<Unit> GetOwnerUnits()
		{
			List<Unit> list = new List<Unit>();
			if (this.HasOwnerCharacter())
			{
				list.Add(this.Owner.character);
			}
			if (this.Owner != null)
			{
				foreach (Worker worker in this.Owner.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						list.Add(worker);
					}
				}
			}
			if (this.Owner != null)
			{
				foreach (Mech mech in this.Owner.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						list.Add(mech);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x0011DED8 File Offset: 0x0011C0D8
		public List<Unit> GetEnemyUnits()
		{
			List<Unit> list = new List<Unit>();
			if (this.HasEnemyCharacter())
			{
				list.Add(this.Enemy.character);
			}
			if (this.Enemy != null)
			{
				foreach (Worker worker in this.Enemy.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						list.Add(worker);
					}
				}
			}
			if (this.Enemy != null)
			{
				foreach (Mech mech in this.Enemy.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						list.Add(mech);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x0011E004 File Offset: 0x0011C204
		public bool HasOwnerCharacter()
		{
			return this.Owner != null && this.Owner.character.position.posX == this.posX && this.Owner.character.position.posY == this.posY;
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x0011E058 File Offset: 0x0011C258
		public int GetOwnerUnitCount()
		{
			int num = 0;
			if (this.Owner != null)
			{
				foreach (Worker worker in this.Owner.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						num++;
					}
				}
				foreach (Mech mech in this.Owner.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						num++;
					}
				}
				if (this.Owner.character.position.posX == this.posX && this.Owner.character.position.posY == this.posY)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x0011E198 File Offset: 0x0011C398
		public List<Worker> GetEnemyWorkers()
		{
			List<Worker> list = new List<Worker>();
			if (this.Enemy != null)
			{
				foreach (Worker worker in this.Enemy.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						list.Add(worker);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x0011E22C File Offset: 0x0011C42C
		public List<Mech> GetEnemyMechs()
		{
			List<Mech> list = new List<Mech>();
			if (this.Enemy != null)
			{
				foreach (Mech mech in this.Enemy.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						list.Add(mech);
					}
				}
			}
			return list;
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x0011E2C0 File Offset: 0x0011C4C0
		public bool HasEnemyCharacter()
		{
			return this.Enemy != null && this.Enemy.character.position.posX == this.posX && this.Enemy.character.position.posY == this.posY;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x0011E314 File Offset: 0x0011C514
		public int GetEnemyUnitCount()
		{
			int num = 0;
			if (this.Enemy != null)
			{
				foreach (Worker worker in this.Enemy.matPlayer.workers)
				{
					if (worker.position.posX == this.posX && worker.position.posY == this.posY)
					{
						num++;
					}
				}
				foreach (Mech mech in this.Enemy.matFaction.mechs)
				{
					if (mech.position.posX == this.posX && mech.position.posY == this.posY)
					{
						num++;
					}
				}
				if (this.Enemy.character.position.posX == this.posX && this.Enemy.character.position.posY == this.posY)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x0011E454 File Offset: 0x0011C654
		public int GetPlayerUnitCount(Player player)
		{
			int num = 0;
			if (this.Owner != null && player == this.Owner)
			{
				num = this.GetOwnerUnitCount();
			}
			else if (this.Enemy != null && player == this.Enemy)
			{
				num = this.GetEnemyUnitCount();
			}
			return num;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x0011E498 File Offset: 0x0011C698
		public bool HadCombatUnitOnHex(Player player)
		{
			if (player.character.lastX == this.posX && player.character.lastY == this.posY)
			{
				return true;
			}
			foreach (Mech mech in player.matFaction.mechs)
			{
				if (mech.lastX == this.posX && mech.lastY == this.posY)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x0011E534 File Offset: 0x0011C734
		public Unit GetCombatUnit(Player player)
		{
			if (player == null)
			{
				return null;
			}
			if (player.character.position == this)
			{
				return player.character;
			}
			foreach (Mech mech in player.matFaction.mechs)
			{
				if (mech.position == this)
				{
					return mech;
				}
			}
			return null;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x0004562B File Offset: 0x0004382B
		public int GetResourceCount()
		{
			return 0 + this.resources[ResourceType.food] + this.resources[ResourceType.metal] + this.resources[ResourceType.oil] + this.resources[ResourceType.wood];
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x0011E5B0 File Offset: 0x0011C7B0
		public bool OneResourceAvaliable()
		{
			int resourceCount = this.GetResourceCount();
			return resourceCount != 0 && (this.resources[ResourceType.food] == resourceCount || this.resources[ResourceType.metal] == resourceCount || this.resources[ResourceType.oil] == resourceCount || this.resources[ResourceType.wood] == resourceCount);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x0011E608 File Offset: 0x0011C808
		public ResourceType GetFirstAvaliableResource()
		{
			if (this.resources[ResourceType.oil] > 0)
			{
				return ResourceType.oil;
			}
			if (this.resources[ResourceType.metal] > 0)
			{
				return ResourceType.metal;
			}
			if (this.resources[ResourceType.food] > 0)
			{
				return ResourceType.food;
			}
			if (this.resources[ResourceType.wood] > 0)
			{
				return ResourceType.wood;
			}
			return ResourceType.oil;
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x0011E65C File Offset: 0x0011C85C
		public void UpdateOwnership()
		{
			this.Owner = null;
			foreach (Player player in this.gameManager.GetPlayers())
			{
				if (player != this.Enemy)
				{
					foreach (Unit unit in player.GetAllUnits())
					{
						if (unit.position.posX == this.posX && unit.position.posY == this.posY)
						{
							this.Owner = player;
							break;
						}
					}
				}
				if (this.Owner != null)
				{
					break;
				}
			}
			if (this.Enemy != null)
			{
				bool flag = false;
				foreach (Player player2 in this.gameManager.GetPlayers())
				{
					if (player2 != this.Owner)
					{
						foreach (Unit unit2 in player2.GetAllUnits())
						{
							if (unit2.position.posX == this.posX && unit2.position.posY == this.posY)
							{
								flag = true;
								if (this.owner == null)
								{
									this.owner = player2;
									this.Enemy = null;
									break;
								}
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					this.Enemy = null;
				}
			}
			if (this.Owner == null && this.Building != null)
			{
				this.Owner = this.Building.player;
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x0011E848 File Offset: 0x0011CA48
		public void Clear()
		{
			this.encounterUsed = false;
			this.encounterTaken = false;
			this.encounterAnimated = false;
			this.resources[ResourceType.food] = 0;
			this.resources[ResourceType.metal] = 0;
			this.resources[ResourceType.oil] = 0;
			this.resources[ResourceType.wood] = 0;
			this.owner = null;
			this.Enemy = null;
			this.Building = null;
			this.Token = null;
			this.skipTopActionPresentationUpdate = false;
			this.skipDownActionPresentationUpdate = false;
			this.snapshotResourcesAfterTopAction.Clear();
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x0011E8D4 File Offset: 0x0011CAD4
		public override string ToString()
		{
			return string.Format("Board position: ({0}, {1})\nType: {2}, Rivermask: {3}\nTunnel: {4}, Encounter field: {5}, Encounter used: {6}, Building: {7}\nOwner: {8}, Enemy: {9}, Encounter taken: {10}", new object[]
			{
				this.posX,
				this.posY,
				this.hexType,
				this.riverMask,
				this.hasTunnel,
				this.hasEncounter,
				this.encounterUsed,
				(this.Building == null) ? "-" : this.Building.buildingType.ToString(),
				(this.Owner == null) ? "-" : this.Owner.matFaction.faction.ToString(),
				(this.Enemy == null) ? "-" : this.Enemy.matFaction.faction.ToString(),
				this.encounterTaken
			});
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x0011E9E8 File Offset: 0x0011CBE8
		public void ReadXml(XmlReader reader)
		{
			if (reader.GetAttribute("EncUsed") != null)
			{
				this.encounterUsed = true;
			}
			if (reader.GetAttribute("EncTaken") != null)
			{
				this.encounterTaken = true;
			}
			if (reader.GetAttribute("R2") != null)
			{
				Dictionary<ResourceType, int> dictionary = this.resources;
				dictionary[ResourceType.food] = dictionary[ResourceType.food] + int.Parse(reader.GetAttribute("R2"));
			}
			if (reader.GetAttribute("R1") != null)
			{
				Dictionary<ResourceType, int> dictionary = this.resources;
				dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + int.Parse(reader.GetAttribute("R1"));
			}
			if (reader.GetAttribute("R0") != null)
			{
				Dictionary<ResourceType, int> dictionary = this.resources;
				dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + int.Parse(reader.GetAttribute("R0"));
			}
			if (reader.GetAttribute("R3") != null)
			{
				Dictionary<ResourceType, int> dictionary = this.resources;
				dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + int.Parse(reader.GetAttribute("R3"));
			}
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x0011EAEC File Offset: 0x0011CCEC
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("X", this.posX.ToString());
			writer.WriteAttributeString("Y", this.posY.ToString());
			if (this.encounterUsed)
			{
				writer.WriteAttributeString("EncUsed", this.encounterUsed.ToString());
			}
			if (this.encounterTaken)
			{
				writer.WriteAttributeString("EncTaken", this.encounterTaken.ToString());
			}
			foreach (ResourceType resourceType in this.resources.Keys)
			{
				if (this.resources[resourceType] > 0)
				{
					string text = "R";
					int num = (int)resourceType;
					writer.WriteAttributeString(text + num.ToString(), this.resources[resourceType].ToString());
				}
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x0011EBE4 File Offset: 0x0011CDE4
		public bool NeedSerialization()
		{
			if (this.encounterUsed)
			{
				return true;
			}
			if (this.encounterTaken)
			{
				return true;
			}
			foreach (ResourceType resourceType in this.resources.Keys)
			{
				if (this.resources[resourceType] > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001FAD RID: 8109
		public GameBoard gameBoard;

		// Token: 0x04001FAE RID: 8110
		public HexType hexType = HexType.forbidden;

		// Token: 0x04001FAF RID: 8111
		public bool hasEncounter;

		// Token: 0x04001FB0 RID: 8112
		public bool encounterUsed;

		// Token: 0x04001FB1 RID: 8113
		public bool encounterTaken;

		// Token: 0x04001FB2 RID: 8114
		public bool encounterAnimated;

		// Token: 0x04001FB3 RID: 8115
		public bool hasTunnel;

		// Token: 0x04001FB6 RID: 8118
		private Player owner;

		// Token: 0x04001FB8 RID: 8120
		public int riverMask;

		// Token: 0x04001FB9 RID: 8121
		public Faction factionBase;

		// Token: 0x04001FBA RID: 8122
		public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

		// Token: 0x04001FBB RID: 8123
		public int posX;

		// Token: 0x04001FBC RID: 8124
		public int posY;

		// Token: 0x04001FBD RID: 8125
		public bool skipTopActionPresentationUpdate;

		// Token: 0x04001FBE RID: 8126
		public bool skipDownActionPresentationUpdate;

		// Token: 0x04001FBF RID: 8127
		public Dictionary<ResourceType, int> snapshotResourcesAfterTopAction = new Dictionary<ResourceType, int>();

		// Token: 0x04001FC0 RID: 8128
		private GameManager gameManager;
	}
}
