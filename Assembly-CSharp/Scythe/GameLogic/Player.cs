using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005E6 RID: 1510
	public class Player : IXmlSerializable
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x00045EBA File Offset: 0x000440BA
		// (set) Token: 0x06002FF4 RID: 12276 RVA: 0x00124E90 File Offset: 0x00123090
		public int Power
		{
			get
			{
				return this.power;
			}
			set
			{
				this.power = value;
				if (this.power > 16)
				{
					this.power = 16;
				}
				if (this.power < 0)
				{
					this.power = 0;
				}
				if (this.power == 16 && this.stars[StarType.Power] == 0 && !this.gameManager.GameFinished && !this.gameManager.GameLoading)
				{
					this.GainStar(StarType.Power);
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x00045EC2 File Offset: 0x000440C2
		// (set) Token: 0x06002FF6 RID: 12278 RVA: 0x00124F00 File Offset: 0x00123100
		public int Popularity
		{
			get
			{
				return this.popularity;
			}
			set
			{
				this.popularity = value;
				if (this.popularity > 18)
				{
					this.popularity = 18;
				}
				if (this.popularity < 0)
				{
					this.popularity = 0;
				}
				if (this.popularity == 18 && this.stars[StarType.Popularity] == 0 && !this.gameManager.GameFinished && !this.gameManager.GameLoading)
				{
					this.GainStar(StarType.Popularity);
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x00045ECA File Offset: 0x000440CA
		// (set) Token: 0x06002FF8 RID: 12280 RVA: 0x00045ED2 File Offset: 0x000440D2
		public int Coins { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x00045EDB File Offset: 0x000440DB
		// (set) Token: 0x06002FFA RID: 12282 RVA: 0x00045EE3 File Offset: 0x000440E3
		public int ObjectivesDone { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x00045EEC File Offset: 0x000440EC
		// (set) Token: 0x06002FFC RID: 12284 RVA: 0x00045EF4 File Offset: 0x000440F4
		public int Victories { get; private set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x00045EFD File Offset: 0x000440FD
		public bool ActionInProgress
		{
			get
			{
				return this.bottomActionInProgress || this.topActionInProgress;
			}
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x00124F70 File Offset: 0x00123170
		public Player(GameManager gameManager)
		{
			this.gameManager = gameManager;
			foreach (StarType starType in Enum.GetValues(typeof(StarType)).Cast<StarType>())
			{
				this.stars.Add(starType, 0);
			}
			this.InitAutomaticGain();
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x00125014 File Offset: 0x00123214
		public Player(GameManager gameManager, MatFaction matFaction, MatPlayer matPlayer, int type = 0)
			: this(gameManager)
		{
			this.IsHuman = type == 0;
			this.character = new Character(gameManager, this, 1);
			this.matFaction = matFaction;
			matFaction.MatOwner = this;
			this.matPlayer = matPlayer;
			matPlayer.SetPlayer(this);
			this.Power = matFaction.StartingPower;
			this.Popularity = matPlayer.StartingPopularity;
			this.Coins = matPlayer.StartingCoins;
			this.ObjectivesDone = 0;
			this.Victories = 0;
			switch (type)
			{
			case 1:
				this.aiDifficulty = AIDifficulty.Easy;
				break;
			case 2:
				this.aiDifficulty = AIDifficulty.Medium;
				break;
			case 3:
				this.aiDifficulty = AIDifficulty.Hard;
				break;
			}
			if (!gameManager.IsMultiplayer)
			{
				this.combatCards = gameManager.GetCombatCards(matFaction.StartingCombatCards);
				this.combatCards.Sort();
				this.objectiveCards = gameManager.GetObjectiveCards(matPlayer.StartingObjectiveCards);
				foreach (ObjectiveCard objectiveCard in this.objectiveCards)
				{
					objectiveCard.SetPlayer(this);
				}
			}
			this.InitCharacterPosition();
			this.InitWorkers();
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x00125148 File Offset: 0x00123348
		private void InitAutomaticGain()
		{
			this.automaticGain = new Dictionary<GainType, bool>();
			this.automaticGain[GainType.Power] = true;
			this.automaticGain[GainType.Coin] = true;
			this.automaticGain[GainType.Popularity] = true;
			this.automaticGain[GainType.CombatCard] = true;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x00045F0F File Offset: 0x0004410F
		public void AddCombatCard(CombatCard combatCard)
		{
			this.combatCards.Add(combatCard);
			this.combatCards.Sort();
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x00045F28 File Offset: 0x00044128
		public void RemoveCombatCard(CombatCard combatCard)
		{
			this.combatCards.Remove(combatCard);
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x00045F37 File Offset: 0x00044137
		public void AddObjectiveCard(ObjectiveCard objectiveCard)
		{
			objectiveCard.SetPlayer(this);
			this.objectiveCards.Add(objectiveCard);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x00125194 File Offset: 0x00123394
		public void CompleteObjective(int index)
		{
			int objectivesDone = this.ObjectivesDone;
			this.ObjectivesDone = objectivesDone + 1;
			this.objectiveCards[index].status = ObjectiveCard.ObjectiveStatus.Completed;
			if (this.matFaction.factionPerk != AbilityPerk.Dominate)
			{
				for (int i = 0; i < this.objectiveCards.Count; i++)
				{
					if (this.objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
					{
						this.objectiveCards[i].status = ObjectiveCard.ObjectiveStatus.Disabled;
					}
				}
			}
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x00125210 File Offset: 0x00123410
		public bool CanStillDoneTheObjective()
		{
			using (List<ObjectiveCard>.Enumerator enumerator = this.objectiveCards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.status == ObjectiveCard.ObjectiveStatus.Open)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x00045F4C File Offset: 0x0004414C
		public bool CanCompleteObjective(int objectiveId)
		{
			return this.objectiveCards[objectiveId].status == ObjectiveCard.ObjectiveStatus.Open && this.objectiveCards[objectiveId].CheckCondition();
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x0012526C File Offset: 0x0012346C
		public bool CanCompleteAnyObjective()
		{
			using (List<ObjectiveCard>.Enumerator enumerator = this.objectiveCards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckCondition())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x001252C8 File Offset: 0x001234C8
		public void BattleVictory()
		{
			int victories = this.Victories;
			this.Victories = victories + 1;
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x001252E8 File Offset: 0x001234E8
		public void GainStar(StarType starType)
		{
			if (!this.gameManager.GameFinished)
			{
				Dictionary<StarType, int> dictionary = this.stars;
				int num = dictionary[starType];
				dictionary[starType] = num + 1;
			}
			this.gameManager.CheckStars();
			this.gameManager.actionLog.LogInfoReported(this.CreateLogInfo(starType));
			AchievementManager.UpdateAchievementStars(starType, this);
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x00045F74 File Offset: 0x00044174
		private LogInfo CreateLogInfo(StarType starType)
		{
			return new StarLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainStar,
				PlayerAssigned = this.matFaction.faction,
				GainedStar = starType,
				starsUnlocked = this.GetNumberOfStars()
			};
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x00045FAD File Offset: 0x000441AD
		public int GetNumberOfStars(StarType starType)
		{
			return this.stars[starType];
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x00125344 File Offset: 0x00123544
		public int GetNumberOfStars()
		{
			int num = 0;
			for (int i = 0; i < this.stars.Count; i++)
			{
				num += this.stars[(StarType)i];
			}
			return num;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x0012537C File Offset: 0x0012357C
		public HashSet<GameHex> OwnedFields(bool endGameCounting = false)
		{
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			if (this.character.position.Owner == this)
			{
				hashSet.Add(this.character.position);
			}
			foreach (Worker worker in this.matPlayer.workers)
			{
				if (!hashSet.Contains(worker.position) && worker.position.Owner == this)
				{
					hashSet.Add(worker.position);
				}
			}
			foreach (Mech mech in this.matFaction.mechs)
			{
				if (!hashSet.Contains(mech.position) && mech.position.Owner == this)
				{
					hashSet.Add(mech.position);
				}
			}
			foreach (Building building in this.matPlayer.buildings)
			{
				if (!hashSet.Contains(building.position) && building.position.Owner == this)
				{
					hashSet.Add(building.position);
				}
			}
			if (endGameCounting && this.matFaction.faction.Equals(Faction.Togawa))
			{
				foreach (FactionAbilityToken factionAbilityToken in this.matFaction.FactionTokens.GetPlacedTokens())
				{
					TrapToken trapToken = (TrapToken)factionAbilityToken;
					if (trapToken.Armed && trapToken.Position.Owner == null && !hashSet.Contains(trapToken.Position))
					{
						hashSet.Add(trapToken.Position);
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x001255A4 File Offset: 0x001237A4
		public HashSet<GameHex> FieldsWithPlayerBuildings()
		{
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			foreach (Building building in this.matPlayer.buildings)
			{
				if (!hashSet.Contains(building.position))
				{
					hashSet.Add(building.position);
				}
			}
			return hashSet;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00125618 File Offset: 0x00123818
		public Dictionary<ResourceType, int> Resources(bool endGameCounting = false)
		{
			if (this.gameManager.TemporaryResourcesEnabled() && this.gameManager.PlayerCurrent == this)
			{
				return this.gameManager.GetTemporaryResources();
			}
			Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
			dictionary.Add(ResourceType.food, 0);
			dictionary.Add(ResourceType.metal, 0);
			dictionary.Add(ResourceType.oil, 0);
			dictionary.Add(ResourceType.wood, 0);
			foreach (GameHex gameHex in this.OwnedFields(endGameCounting))
			{
				if (gameHex.hexType != HexType.capital)
				{
					Dictionary<ResourceType, int> dictionary2 = dictionary;
					dictionary2[ResourceType.food] = dictionary2[ResourceType.food] + gameHex.resources[ResourceType.food];
					dictionary2 = dictionary;
					dictionary2[ResourceType.metal] = dictionary2[ResourceType.metal] + gameHex.resources[ResourceType.metal];
					dictionary2 = dictionary;
					dictionary2[ResourceType.oil] = dictionary2[ResourceType.oil] + gameHex.resources[ResourceType.oil];
					dictionary2 = dictionary;
					dictionary2[ResourceType.wood] = dictionary2[ResourceType.wood] + gameHex.resources[ResourceType.wood];
				}
			}
			return dictionary;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x00045FBB File Offset: 0x000441BB
		public int GetCombatCardsCount()
		{
			if (this.gameManager.TemporaryResourcesEnabled() && this.gameManager.PlayerCurrent == this)
			{
				return this.gameManager.GetTemporaryCombatCardsCount();
			}
			return this.combatCards.Count;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x00125738 File Offset: 0x00123938
		public void UpgradeMaxMoveCount()
		{
			this.character.UpgradeMaxMoveCount();
			foreach (Mech mech in this.matFaction.mechs)
			{
				mech.UpgradeMaxMoveCount();
			}
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x00125798 File Offset: 0x00123998
		private void InitCharacterPosition()
		{
			GameHex[,] hexMap = this.gameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex = hexMap[i, j];
					if (gameHex.hexType == HexType.capital && gameHex.factionBase == this.matFaction.faction)
					{
						this.character.position = gameHex;
						return;
					}
				}
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x00125828 File Offset: 0x00123A28
		private void InitWorkers()
		{
			GameHex gameHex = null;
			GameHex[,] hexMap = this.gameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex2 = hexMap[i, j];
					if (gameHex2.hexType == HexType.capital && gameHex2.factionBase == this.matFaction.faction)
					{
						gameHex = gameHex2;
						goto IL_007F;
					}
				}
			}
			IL_007F:
			if (gameHex == null)
			{
				return;
			}
			foreach (GameHex gameHex3 in gameHex.GetNeighboursAll())
			{
				if (gameHex3.hexType != HexType.lake && gameHex3.hexType != HexType.forbidden && !gameHex3.IsRiverBetween(gameHex))
				{
					Worker worker = new Worker(this.gameManager, this, 1, -1);
					worker.position = gameHex3;
					this.matPlayer.workers.Add(worker);
				}
			}
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00045FEF File Offset: 0x000441EF
		private void AddResources(GameHex hex)
		{
			hex.resources[ResourceType.food] = 16;
			hex.resources[ResourceType.metal] = 16;
			hex.resources[ResourceType.oil] = 16;
			hex.resources[ResourceType.wood] = 16;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x00125944 File Offset: 0x00123B44
		public List<Unit> GetAllUnits()
		{
			List<Unit> list = new List<Unit>();
			list.Add(this.character);
			foreach (Mech mech in this.matFaction.mechs)
			{
				list.Add(mech);
			}
			foreach (Worker worker in this.matPlayer.workers)
			{
				list.Add(worker);
			}
			return list;
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x001259F8 File Offset: 0x00123BF8
		public GameHex GetCapital()
		{
			GameHex gameHex = null;
			GameHex[,] hexMap = this.gameManager.gameBoard.hexMap;
			int upperBound = hexMap.GetUpperBound(0);
			int upperBound2 = hexMap.GetUpperBound(1);
			for (int i = hexMap.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = hexMap.GetLowerBound(1); j <= upperBound2; j++)
				{
					GameHex gameHex2 = hexMap[i, j];
					if (gameHex2.hexType == HexType.capital && gameHex2.factionBase == this.matFaction.faction)
					{
						return gameHex2;
					}
				}
			}
			return gameHex;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x00046029 File Offset: 0x00044229
		public bool CanGetCombatStar()
		{
			return this.matFaction.factionPerk == AbilityPerk.Dominate || this.stars[StarType.Combat] != 2;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x0004604E File Offset: 0x0004424E
		public void CheckCombatStar()
		{
			if (!this.CanGetCombatStar())
			{
				return;
			}
			this.GainStar(StarType.Combat);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00046060 File Offset: 0x00044260
		public bool PlayerMatSectionSelected()
		{
			return this.currentMatSection != -1;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x00125A88 File Offset: 0x00123C88
		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			int.TryParse(reader.GetAttribute("CWon"), out this.CombatWon);
			int.TryParse(reader.GetAttribute("Enc"), out this.EncountersVisited);
			string attribute = reader.GetAttribute("CAttacker");
			if (attribute != null)
			{
				int.TryParse(attribute, out this.StartedCombatsAsAnAttacker);
			}
			else
			{
				this.StartedCombatsAsAnAttacker = 0;
			}
			if (this.gameManager.IsMultiplayer)
			{
				int.TryParse(reader.GetAttribute("Distance"), out this.DistanceTravelled);
				int.TryParse(reader.GetAttribute("CLost"), out this.CombatLost);
				int.TryParse(reader.GetAttribute("CPower"), out this.CombatPowerSpent);
				int.TryParse(reader.GetAttribute("CMPower"), out this.CombatMaxPowerUsed);
				int.TryParse(reader.GetAttribute("CWChased"), out this.CombatWorkersChased);
			}
			this.IsHuman = bool.Parse(reader.GetAttribute("IsHuman"));
			if (reader.GetAttribute("AiDifficulty") != null)
			{
				this.aiDifficulty = (AIDifficulty)int.Parse(reader.GetAttribute("AiDifficulty"));
			}
			else
			{
				this.aiDifficulty = AIDifficulty.Medium;
			}
			if (reader.GetAttribute("WonBattle") != null)
			{
				this.wonBattle = true;
			}
			this.Name = reader.GetAttribute("Name");
			this.Power = int.Parse(reader.GetAttribute("Power"));
			this.Popularity = int.Parse(reader.GetAttribute("Popularity"));
			this.Coins = int.Parse(reader.GetAttribute("Coins"));
			this.ObjectivesDone = int.Parse(reader.GetAttribute("ObjectivesDone"));
			this.Victories = int.Parse(reader.GetAttribute("Victories"));
			this.lastMatSection = int.Parse(reader.GetAttribute("LastMat"));
			this.currentMatSection = int.Parse(reader.GetAttribute("CurrMat"));
			if (reader.GetAttribute("TopFinished") != null)
			{
				this.topActionFinished = true;
			}
			if (reader.GetAttribute("DownFinished") != null)
			{
				this.downActionFinished = true;
			}
			if (reader.GetAttribute("TopInProgress") != null)
			{
				this.topActionInProgress = true;
			}
			if (reader.GetAttribute("DownInProgress") != null)
			{
				this.bottomActionInProgress = true;
			}
			Faction faction = (Faction)int.Parse(reader.GetAttribute("Faction"));
			this.matFaction = new MatFaction(this.gameManager, faction);
			this.matFaction.MatOwner = this;
			PlayerMatType playerMatType = (PlayerMatType)int.Parse(reader.GetAttribute("MatType"));
			this.matPlayer = new MatPlayer(this.gameManager, playerMatType);
			reader.ReadStartElement();
			this.character = new Character(this.gameManager, this, 1);
			if (reader.Name == "Character")
			{
				((IXmlSerializable)this.character).ReadXml(reader);
			}
			reader.ReadStartElement();
			this.matPlayer.SetPlayer(this);
			((IXmlSerializable)this.matPlayer).ReadXml(reader);
			reader.ReadEndElement();
			this.matPlayer.SetPlayer(this);
			((IXmlSerializable)this.matFaction).ReadXml(reader);
			reader.ReadEndElement();
			if (reader.Name == "CombatCards")
			{
				foreach (string text in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text == "")
					{
						break;
					}
					this.combatCards.Add(new CombatCard(int.Parse(text)));
				}
			}
			if (reader.Name == "ObjectiveCards")
			{
				string[] array2 = reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None);
				for (int j = 0; j < array2.Length - 1; j += 2)
				{
					int num = int.Parse(array2[j]);
					int num2 = int.Parse(array2[j + 1]);
					this.AddObjectiveCard(new ObjectiveCard(this.gameManager, num, (ObjectiveCard.ObjectiveStatus)num2));
				}
			}
			if (reader.Name == "Stars")
			{
				for (int k = 0; k <= 8; k++)
				{
					if (reader.GetAttribute("S" + k.ToString()) != null)
					{
						this.stars[(StarType)k] = int.Parse(reader.GetAttribute("S" + k.ToString()));
					}
				}
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00125EB4 File Offset: 0x001240B4
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("CWon", this.CombatWon.ToString());
			writer.WriteAttributeString("Enc", this.EncountersVisited.ToString());
			writer.WriteAttributeString("CAttacker", this.StartedCombatsAsAnAttacker.ToString());
			if (this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner == null && !this.gameManager.SendingToPlayer)
			{
				writer.WriteAttributeString("Distance", this.DistanceTravelled.ToString());
				writer.WriteAttributeString("CLost", this.CombatLost.ToString());
				writer.WriteAttributeString("CPower", this.CombatPowerSpent.ToString());
				writer.WriteAttributeString("CMPower", this.CombatMaxPowerUsed.ToString());
				writer.WriteAttributeString("CWChased", this.CombatWorkersChased.ToString());
			}
			writer.WriteAttributeString("IsHuman", this.IsHuman.ToString());
			string text = "AiDifficulty";
			int num = (int)this.aiDifficulty;
			writer.WriteAttributeString(text, num.ToString());
			if (this.wonBattle)
			{
				writer.WriteAttributeString("WonBattle", "");
			}
			writer.WriteAttributeString("Name", this.Name);
			writer.WriteAttributeString("Power", this.Power.ToString());
			writer.WriteAttributeString("Popularity", this.Popularity.ToString());
			writer.WriteAttributeString("Coins", this.Coins.ToString());
			writer.WriteAttributeString("ObjectivesDone", this.ObjectivesDone.ToString());
			writer.WriteAttributeString("Victories", this.Victories.ToString());
			writer.WriteAttributeString("LastMat", this.lastMatSection.ToString());
			writer.WriteAttributeString("CurrMat", this.currentMatSection.ToString());
			if (this.topActionFinished)
			{
				writer.WriteAttributeString("TopFinished", "");
			}
			if (this.downActionFinished)
			{
				writer.WriteAttributeString("DownFinished", "");
			}
			if (this.topActionInProgress)
			{
				writer.WriteAttributeString("TopInProgress", "");
			}
			if (this.bottomActionInProgress)
			{
				writer.WriteAttributeString("DownInProgress", "");
			}
			string text2 = "Faction";
			num = (int)this.matFaction.faction;
			writer.WriteAttributeString(text2, num.ToString());
			writer.WriteAttributeString("MatType", ((int)this.matPlayer.matType).ToString());
			writer.WriteStartElement("Character");
			((IXmlSerializable)this.character).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("MatPlayer");
			((IXmlSerializable)this.matPlayer).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("MatFaction");
			((IXmlSerializable)this.matFaction).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("CombatCards");
			for (int i = 0; i < this.combatCards.Count; i++)
			{
				((IXmlSerializable)this.combatCards[i]).WriteXml(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("ObjectiveCards");
			for (int j = 0; j < this.objectiveCards.Count; j++)
			{
				((IXmlSerializable)this.objectiveCards[j]).WriteXml(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Stars");
			foreach (StarType starType in this.stars.Keys)
			{
				if (this.stars[starType] > 0)
				{
					string text3 = "S";
					num = (int)starType;
					writer.WriteAttributeString(text3 + num.ToString(), this.stars[starType].ToString());
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x00126298 File Offset: 0x00124498
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				" Faction: ",
				this.matFaction.faction.ToString(),
				" Power: ",
				this.power.ToString(),
				" Popularity: ",
				this.popularity.ToString(),
				" Coins: ",
				this.Coins.ToString(),
				" "
			}));
			stringBuilder.Append(string.Concat(new string[]
			{
				" Objectives done ",
				this.ObjectivesDone.ToString(),
				" Victories: ",
				this.Victories.ToString(),
				" Workers: ",
				this.matPlayer.workers.Count.ToString(),
				"\n"
			}));
			stringBuilder.Append("Resources: ");
			Dictionary<ResourceType, int> dictionary = this.Resources(false);
			foreach (ResourceType resourceType in dictionary.Keys)
			{
				stringBuilder.Append(resourceType.ToString() + " " + dictionary[resourceType].ToString() + "; ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040020A1 RID: 8353
		public bool IsHuman;

		// Token: 0x040020A2 RID: 8354
		public string Name;

		// Token: 0x040020A3 RID: 8355
		public MatFaction matFaction;

		// Token: 0x040020A4 RID: 8356
		public MatPlayer matPlayer;

		// Token: 0x040020A5 RID: 8357
		[NonSerialized]
		public Dictionary<GainType, bool> automaticGain;

		// Token: 0x040020A6 RID: 8358
		public Character character;

		// Token: 0x040020A7 RID: 8359
		public List<CombatCard> combatCards = new List<CombatCard>();

		// Token: 0x040020A8 RID: 8360
		public List<ObjectiveCard> objectiveCards = new List<ObjectiveCard>();

		// Token: 0x040020A9 RID: 8361
		private int power;

		// Token: 0x040020AA RID: 8362
		private int popularity;

		// Token: 0x040020AB RID: 8363
		public Dictionary<StarType, int> stars = new Dictionary<StarType, int>();

		// Token: 0x040020AC RID: 8364
		public bool wonBattle;

		// Token: 0x040020AD RID: 8365
		public int lastMatSection = -1;

		// Token: 0x040020AE RID: 8366
		public int currentMatSection = -1;

		// Token: 0x040020AF RID: 8367
		public bool topActionFinished;

		// Token: 0x040020B0 RID: 8368
		public bool downActionFinished;

		// Token: 0x040020B1 RID: 8369
		public bool bottomActionInProgress;

		// Token: 0x040020B2 RID: 8370
		public bool topActionInProgress;

		// Token: 0x040020B3 RID: 8371
		[NonSerialized]
		public const string AUTOGAIN_POWER_KEY = "AutoGainPower";

		// Token: 0x040020B4 RID: 8372
		[NonSerialized]
		public const string AUTOGAIN_COIN_KEY = "AutoGainCoin";

		// Token: 0x040020B5 RID: 8373
		[NonSerialized]
		public const string AUTOGAIN_POPULARITY_KEY = "AutoGainPopularity";

		// Token: 0x040020B6 RID: 8374
		[NonSerialized]
		public const string AUTOGAIN_AMMO_KEY = "AutoGainAmmo";

		// Token: 0x040020B7 RID: 8375
		[NonSerialized]
		public AiPlayer aiPlayer;

		// Token: 0x040020B8 RID: 8376
		public AIDifficulty aiDifficulty;

		// Token: 0x040020B9 RID: 8377
		private GameManager gameManager;

		// Token: 0x040020BA RID: 8378
		public int StartedCombatsAsAnAttacker;

		// Token: 0x040020BB RID: 8379
		public int DistanceTravelled;

		// Token: 0x040020BC RID: 8380
		public int CombatWon;

		// Token: 0x040020BD RID: 8381
		public int CombatLost;

		// Token: 0x040020BE RID: 8382
		public int CombatPowerSpent;

		// Token: 0x040020BF RID: 8383
		public int CombatMaxPowerUsed;

		// Token: 0x040020C0 RID: 8384
		public int CombatWorkersChased;

		// Token: 0x040020C1 RID: 8385
		public int EncountersVisited;
	}
}
