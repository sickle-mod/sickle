using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005DE RID: 1502
	public class MatPlayer : IXmlSerializable
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06002F82 RID: 12162 RVA: 0x00045952 File Offset: 0x00043B52
		// (set) Token: 0x06002F83 RID: 12163 RVA: 0x0004595A File Offset: 0x00043B5A
		public int UpgradesDone { get; private set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06002F84 RID: 12164 RVA: 0x00045963 File Offset: 0x00043B63
		// (set) Token: 0x06002F85 RID: 12165 RVA: 0x0004596B File Offset: 0x00043B6B
		public int RecruitsEnlisted { get; private set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06002F86 RID: 12166 RVA: 0x00045974 File Offset: 0x00043B74
		// (set) Token: 0x06002F87 RID: 12167 RVA: 0x0004597C File Offset: 0x00043B7C
		public PlayerMatType matType { get; private set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06002F88 RID: 12168 RVA: 0x00045985 File Offset: 0x00043B85
		public int StartingObjectiveCards
		{
			get
			{
				return this.startingObjectiveCards;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06002F89 RID: 12169 RVA: 0x0004598D File Offset: 0x00043B8D
		public int StartingPopularity
		{
			get
			{
				return this.startingPopularity;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x00045995 File Offset: 0x00043B95
		public int StartingCoins
		{
			get
			{
				return this.startingCoins;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x0004599D File Offset: 0x00043B9D
		public int StartingOrderNumber
		{
			get
			{
				return this.startingOrderNumber;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x000459A5 File Offset: 0x00043BA5
		// (set) Token: 0x06002F8D RID: 12173 RVA: 0x000459AD File Offset: 0x00043BAD
		public int SelectedSection { get; private set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x000459B6 File Offset: 0x00043BB6
		public int matPlayerSectionsCount
		{
			get
			{
				return this.matPlayerSections.Count;
			}
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x001212F0 File Offset: 0x0011F4F0
		public MatPlayer(GameManager gameManager, int objectiveCards, int popularity, int coins, int order, PlayerMatType type)
		{
			this.gameManager = gameManager;
			this.SetStartingParameters(objectiveCards, popularity, coins, order);
			this.CreateSections(type);
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x00121340 File Offset: 0x0011F540
		public MatPlayer(GameManager gameManager, PlayerMatType type)
		{
			this.gameManager = gameManager;
			switch (type)
			{
			case PlayerMatType.Industrial:
				this.SetStartingParameters(2, 2, 4, 1);
				break;
			case PlayerMatType.Engineering:
				this.SetStartingParameters(2, 2, 5, 2);
				break;
			case PlayerMatType.Patriotic:
				this.SetStartingParameters(2, 2, 6, 4);
				break;
			case PlayerMatType.Mechanical:
				this.SetStartingParameters(2, 3, 6, 6);
				break;
			case PlayerMatType.Agricultural:
				this.SetStartingParameters(2, 4, 7, 7);
				break;
			case PlayerMatType.Militant:
				this.SetStartingParameters(2, 3, 4, 3);
				break;
			case PlayerMatType.Innovative:
				this.SetStartingParameters(2, 3, 5, 5);
				break;
			case PlayerMatType.Campaign00:
				this.SetStartingParameters(2, 0, 0, 8);
				break;
			case PlayerMatType.Campaign01:
				this.SetStartingParameters(2, 1, 0, 0);
				break;
			case PlayerMatType.Tutorial01:
				this.SetStartingParameters(2, 0, 1, 8);
				break;
			case PlayerMatType.Tutorial02Crimea:
				this.SetStartingParameters(0, 0, 0, 0);
				break;
			case PlayerMatType.Tutorial02Saxony:
				this.SetStartingParameters(0, 0, 0, 0);
				break;
			case PlayerMatType.Tutorial02Polania:
				this.SetStartingParameters(0, 0, 0, 0);
				break;
			case PlayerMatType.Tutorial03:
				this.SetStartingParameters(2, 0, 0, 8);
				break;
			case PlayerMatType.Tutorial04:
				this.SetStartingParameters(2, 0, 0, 8);
				break;
			case PlayerMatType.Tutorial01Crimea:
				this.SetStartingParameters(2, 0, 1, 1);
				break;
			case PlayerMatType.Tutorial01Saxony:
				this.SetStartingParameters(2, 0, 1, 2);
				break;
			case PlayerMatType.Tutorial01StarsA:
				this.SetStartingParameters(0, 3, 7, 2);
				break;
			case PlayerMatType.Tutorial01StarsB:
				this.SetStartingParameters(0, 3, 4, 2);
				break;
			case PlayerMatType.Tutorial02Stars:
				this.SetStartingParameters(0, 4, 2, 2);
				break;
			case PlayerMatType.Tutorial03Player:
				this.SetStartingParameters(0, 3, 3, 2);
				break;
			case PlayerMatType.Tutorial03Enemy:
				this.SetStartingParameters(0, 2, 6, 2);
				break;
			case PlayerMatType.Tutorial04Player:
				this.SetStartingParameters(0, 0, 6, 2);
				break;
			case PlayerMatType.Tutorial05Player:
				this.SetStartingParameters(0, 5, 5, 2);
				break;
			case PlayerMatType.Tutorial06Player:
				this.SetStartingParameters(0, 3, 5, 2);
				break;
			case PlayerMatType.Tutorial07Player:
				this.SetStartingParameters(0, 4, 4, 2);
				break;
			case PlayerMatType.Tutorial08Player:
				this.SetStartingParameters(2, 2, 6, 1);
				break;
			case PlayerMatType.Tutorial09Player:
				this.SetStartingParameters(0, 4, 6, 2);
				break;
			case PlayerMatType.Tutorial09AINordic:
				this.SetStartingParameters(0, 3, 8, 2);
				break;
			case PlayerMatType.Tutorial09AISaxony:
				this.SetStartingParameters(0, 2, 6, 2);
				break;
			case PlayerMatType.Tutorial10Player:
				this.SetStartingParameters(2, 8, 2, 2);
				break;
			case PlayerMatType.Tutorial11Player:
				this.SetStartingParameters(2, 2, 9, 1);
				break;
			case PlayerMatType.Tutorial11Enemy:
				this.SetStartingParameters(2, 18, 12, 2);
				break;
			case PlayerMatType.Challenge1:
				this.SetStartingParameters(0, 15, 0, 0);
				break;
			}
			this.CreateSections(type);
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000459C3 File Offset: 0x00043BC3
		private void SetStartingParameters(int objectiveCards, int popularity, int coins, int order)
		{
			this.startingObjectiveCards = objectiveCards;
			this.startingPopularity = popularity;
			this.startingCoins = coins;
			this.startingOrderNumber = order;
			this.UpgradesDone = 0;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x001215F4 File Offset: 0x0011F7F4
		private void CreateSections(PlayerMatType type)
		{
			this.matPlayerSections.Add(new MatPlayerSection(type, 0, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 1, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 2, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 3, this.gameManager));
			this.matType = type;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00121668 File Offset: 0x0011F868
		public void SetPlayer(Player matOwner)
		{
			foreach (MatPlayerSection matPlayerSection in this.matPlayerSections)
			{
				matPlayerSection.SetPlayer(matOwner);
			}
			foreach (Worker worker in this.workers)
			{
				worker.Owner = matOwner;
			}
			foreach (Building building in this.buildings)
			{
				building.player = matOwner;
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000459E9 File Offset: 0x00043BE9
		public void CheckBuildingStar()
		{
			if (this.buildings.Count != 4)
			{
				return;
			}
			if (this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Structures) == 1)
			{
				return;
			}
			this.gameManager.PlayerCurrent.GainStar(StarType.Structures);
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00045A20 File Offset: 0x00043C20
		public void CheckBuildingStar(Player player)
		{
			if (player.matPlayer.buildings.Count != 4)
			{
				return;
			}
			if (player.GetNumberOfStars(StarType.Structures) == 1)
			{
				return;
			}
			player.GainStar(StarType.Structures);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00045A48 File Offset: 0x00043C48
		public void CheckWorkerStar()
		{
			if (this.workers.Count != 8 || this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Workers) == 1)
			{
				return;
			}
			this.gameManager.PlayerCurrent.GainStar(StarType.Workers);
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x00045A7E File Offset: 0x00043C7E
		public void CheckWorkerStar(Player player)
		{
			if (player.matPlayer.workers.Count != 8 || player.GetNumberOfStars(StarType.Workers) == 1)
			{
				return;
			}
			player.GainStar(StarType.Workers);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00045AA5 File Offset: 0x00043CA5
		public MatPlayerSection GetPlayerMatSection(int id)
		{
			if (id >= 0 && id < this.matPlayerSections.Count)
			{
				this.SelectedSection = id;
				return this.matPlayerSections[id];
			}
			return null;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x0012173C File Offset: 0x0011F93C
		public void IncreaseUpgradeCounter()
		{
			int upgradesDone = this.UpgradesDone;
			this.UpgradesDone = upgradesDone + 1;
			if (this.UpgradesDone == 6 && this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Upgrades) == 0)
			{
				this.gameManager.PlayerCurrent.GainStar(StarType.Upgrades);
			}
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x00121788 File Offset: 0x0011F988
		public TopAction GetTopAction(TopActionType type)
		{
			foreach (MatPlayerSection matPlayerSection in this.matPlayerSections)
			{
				if (matPlayerSection.ActionTop.Type == type)
				{
					return matPlayerSection.ActionTop;
				}
			}
			return null;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x001217F0 File Offset: 0x0011F9F0
		public DownAction GetDownAction(DownActionType type)
		{
			foreach (MatPlayerSection matPlayerSection in this.matPlayerSections)
			{
				if (matPlayerSection.ActionDown.Type == type)
				{
					return matPlayerSection.ActionDown;
				}
			}
			return null;
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00121858 File Offset: 0x0011FA58
		public Building GetBuilding(BuildingType buildingType)
		{
			Building building = null;
			foreach (Building building2 in this.buildings)
			{
				if (building2.buildingType == buildingType)
				{
					building = building2;
				}
			}
			return building;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00045ACE File Offset: 0x00043CCE
		public void AddFactoryCard(FactoryCard factoryCard)
		{
			this.matPlayerSections.Add(factoryCard);
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x001218B4 File Offset: 0x0011FAB4
		private LogInfo CreateLogInfo(FactoryCard factoryCard)
		{
			return new FactoryLogInfo(this.gameManager)
			{
				Type = LogInfoType.FactoryCardGain,
				GainedFactoryCard = factoryCard,
				PlayerAssigned = this.gameManager.PlayerCurrent.matFaction.faction,
				Character = this.gameManager.PlayerCurrent.character
			};
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x0012190C File Offset: 0x0011FB0C
		public bool CanEnlist()
		{
			using (List<MatPlayerSection>.Enumerator enumerator = this.matPlayerSections.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.ActionDown.RecruitEnlisted())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x00045ADC File Offset: 0x00043CDC
		public int IdleWorkersCount()
		{
			return 8 - this.workers.Count;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x0012196C File Offset: 0x0011FB6C
		public void IncrementEnlistedRecruitsCounter()
		{
			int recruitsEnlisted = this.RecruitsEnlisted;
			this.RecruitsEnlisted = recruitsEnlisted + 1;
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x0012198C File Offset: 0x0011FB8C
		public void AddCostsToProduce()
		{
			int count = this.workers.Count;
			TopAction topAction = this.GetTopAction(TopActionType.Produce);
			int num = topAction.GetNumberOfPayActions();
			bool flag = false;
			if (!this.gameManager.GameLoading)
			{
				flag = this.matPlayerSections[this.gameManager.PlayerCurrent.currentMatSection].ActionTop.Type == TopActionType.Produce;
			}
			if (count >= 4 && num == 0)
			{
				topAction.AddPayAction(new PayPower(this.gameManager, 1, 0, flag));
				num++;
			}
			if (count >= 6 && num == 1)
			{
				topAction.AddPayAction(new PayPopularity(this.gameManager, 1, 0, flag, false));
				num++;
			}
			if (count == 8 && num == 2)
			{
				topAction.AddPayAction(new PayCoin(this.gameManager, 1, 0, flag, false));
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00121A48 File Offset: 0x0011FC48
		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			this.UpgradesDone = int.Parse(reader.GetAttribute("Upgrades"));
			this.RecruitsEnlisted = int.Parse(reader.GetAttribute("Recruits"));
			int num = 0;
			reader.ReadStartElement();
			while (reader.Name == "Worker")
			{
				Worker worker = new Worker(this.gameManager, null, 1, num++);
				((IXmlSerializable)worker).ReadXml(reader);
				this.workers.Add(worker);
				reader.ReadStartElement();
			}
			while (reader.Name == "Building")
			{
				Building building = new Building(this.gameManager);
				((IXmlSerializable)building).ReadXml(reader);
				this.buildings.Add(building);
				reader.ReadStartElement();
			}
			this.AddCostsToProduce();
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSection matPlayerSection = this.matPlayerSections[i];
				if (reader.Name == "Section")
				{
					if (reader.GetAttribute("Enlisted") != null)
					{
						matPlayerSection.ActionDown.IsRecruitEnlisted = true;
					}
					if (reader.GetAttribute("OldRecruit") != null)
					{
						matPlayerSection.ActionDown.OldRecruitValue = true;
					}
					((IXmlSerializable)matPlayerSection.ActionTop).ReadXml(reader);
					Building building2 = this.GetBuilding(matPlayerSection.ActionTop.Structure.buildingType);
					if (building2 != null)
					{
						matPlayerSection.ActionTop.Structure = building2;
					}
					((IXmlSerializable)matPlayerSection.ActionDown).ReadXml(reader);
				}
				reader.ReadEndElement();
			}
			if (reader.Name == "Factory")
			{
				this.matPlayerSections.Add(new FactoryCard(int.Parse(reader.GetAttribute("Index")), this.gameManager));
				this.matPlayerSections[4].SetPlayer(this.matPlayerSections[0].ActionTop.GetGainAction(0).GetPlayer());
				((IXmlSerializable)this.matPlayerSections[4].ActionTop).ReadXml(reader);
				reader.ReadEndElement();
			}
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x00121C44 File Offset: 0x0011FE44
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Upgrades", this.UpgradesDone.ToString());
			writer.WriteAttributeString("Recruits", this.RecruitsEnlisted.ToString());
			foreach (IXmlSerializable xmlSerializable in this.workers)
			{
				writer.WriteStartElement("Worker");
				xmlSerializable.WriteXml(writer);
				writer.WriteEndElement();
			}
			foreach (IXmlSerializable xmlSerializable2 in this.buildings)
			{
				writer.WriteStartElement("Building");
				xmlSerializable2.WriteXml(writer);
				writer.WriteEndElement();
			}
			for (int i = 0; i < 4; i++)
			{
				writer.WriteStartElement("Section");
				if (this.matPlayerSections[i].ActionDown.IsRecruitEnlisted)
				{
					writer.WriteAttributeString("Enlisted", "");
				}
				if (this.matPlayerSections[i].ActionDown.OldRecruitValue)
				{
					writer.WriteAttributeString("OldRecruit", "");
				}
				((IXmlSerializable)this.matPlayerSections[i].ActionTop).WriteXml(writer);
				((IXmlSerializable)this.matPlayerSections[i].ActionDown).WriteXml(writer);
				writer.WriteEndElement();
			}
			if (this.matPlayerSections.Count == 5)
			{
				writer.WriteStartElement("Factory");
				writer.WriteAttributeString("Index", ((FactoryCard)this.matPlayerSections[4]).Serialize());
				((IXmlSerializable)this.matPlayerSections[4].ActionTop).WriteXml(writer);
				writer.WriteEndElement();
			}
		}

		// Token: 0x04002077 RID: 8311
		private int startingObjectiveCards;

		// Token: 0x04002078 RID: 8312
		private int startingPopularity;

		// Token: 0x04002079 RID: 8313
		private int startingCoins;

		// Token: 0x0400207A RID: 8314
		private int startingOrderNumber;

		// Token: 0x0400207F RID: 8319
		private List<MatPlayerSection> matPlayerSections = new List<MatPlayerSection>();

		// Token: 0x04002080 RID: 8320
		public List<Worker> workers = new List<Worker>();

		// Token: 0x04002081 RID: 8321
		public List<Building> buildings = new List<Building>();

		// Token: 0x04002082 RID: 8322
		private GameManager gameManager;
	}
}
