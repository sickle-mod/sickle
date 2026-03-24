using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005EC RID: 1516
	public class MatPlayer : IXmlSerializable
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x000459AC File Offset: 0x00043BAC
		// (set) Token: 0x06002FD6 RID: 12246 RVA: 0x000459B4 File Offset: 0x00043BB4
		public int UpgradesDone { get; private set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x000459BD File Offset: 0x00043BBD
		// (set) Token: 0x06002FD8 RID: 12248 RVA: 0x000459C5 File Offset: 0x00043BC5
		public int RecruitsEnlisted { get; private set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x000459CE File Offset: 0x00043BCE
		// (set) Token: 0x06002FDA RID: 12250 RVA: 0x000459D6 File Offset: 0x00043BD6
		public PlayerMatType matType { get; private set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x000459DF File Offset: 0x00043BDF
		public int StartingObjectiveCards
		{
			get
			{
				return this.startingObjectiveCards;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x000459E7 File Offset: 0x00043BE7
		public int StartingPopularity
		{
			get
			{
				return this.startingPopularity;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x000459EF File Offset: 0x00043BEF
		public int StartingCoins
		{
			get
			{
				return this.startingCoins;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000459F7 File Offset: 0x00043BF7
		public int StartingOrderNumber
		{
			get
			{
				return this.startingOrderNumber;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000459FF File Offset: 0x00043BFF
		// (set) Token: 0x06002FE0 RID: 12256 RVA: 0x00045A07 File Offset: 0x00043C07
		public int SelectedSection { get; private set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x00045A10 File Offset: 0x00043C10
		public int matPlayerSectionsCount
		{
			get
			{
				return this.matPlayerSections.Count;
			}
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00129418 File Offset: 0x00127618
		public MatPlayer(GameManager gameManager, int objectiveCards, int popularity, int coins, int order, PlayerMatType type)
		{
			this.gameManager = gameManager;
			this.SetStartingParameters(objectiveCards, popularity, coins, order);
			this.CreateSections(type);
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x00129468 File Offset: 0x00127668
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

		// Token: 0x06002FE4 RID: 12260 RVA: 0x00045A1D File Offset: 0x00043C1D
		private void SetStartingParameters(int objectiveCards, int popularity, int coins, int order)
		{
			this.startingObjectiveCards = objectiveCards;
			this.startingPopularity = popularity;
			this.startingCoins = coins;
			this.startingOrderNumber = order;
			this.UpgradesDone = 0;
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0012971C File Offset: 0x0012791C
		private void CreateSections(PlayerMatType type)
		{
			this.matPlayerSections.Add(new MatPlayerSection(type, 0, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 1, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 2, this.gameManager));
			this.matPlayerSections.Add(new MatPlayerSection(type, 3, this.gameManager));
			this.matType = type;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x00129790 File Offset: 0x00127990
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

		// Token: 0x06002FE7 RID: 12263 RVA: 0x00045A43 File Offset: 0x00043C43
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

		// Token: 0x06002FE8 RID: 12264 RVA: 0x00045A7A File Offset: 0x00043C7A
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

		// Token: 0x06002FE9 RID: 12265 RVA: 0x00045AA2 File Offset: 0x00043CA2
		public void CheckWorkerStar()
		{
			if (this.workers.Count != 8 || this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Workers) == 1)
			{
				return;
			}
			this.gameManager.PlayerCurrent.GainStar(StarType.Workers);
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x00045AD8 File Offset: 0x00043CD8
		public void CheckWorkerStar(Player player)
		{
			if (player.matPlayer.workers.Count != 8 || player.GetNumberOfStars(StarType.Workers) == 1)
			{
				return;
			}
			player.GainStar(StarType.Workers);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x00045AFF File Offset: 0x00043CFF
		public MatPlayerSection GetPlayerMatSection(int id)
		{
			if (id >= 0 && id < this.matPlayerSections.Count)
			{
				this.SelectedSection = id;
				return this.matPlayerSections[id];
			}
			return null;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x00129864 File Offset: 0x00127A64
		public void IncreaseUpgradeCounter()
		{
			int upgradesDone = this.UpgradesDone;
			this.UpgradesDone = upgradesDone + 1;
			if (this.UpgradesDone == 6 && this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Upgrades) == 0)
			{
				this.gameManager.PlayerCurrent.GainStar(StarType.Upgrades);
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x001298B0 File Offset: 0x00127AB0
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

		// Token: 0x06002FEE RID: 12270 RVA: 0x00129918 File Offset: 0x00127B18
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

		// Token: 0x06002FEF RID: 12271 RVA: 0x00129980 File Offset: 0x00127B80
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

		// Token: 0x06002FF0 RID: 12272 RVA: 0x00045B28 File Offset: 0x00043D28
		public void AddFactoryCard(FactoryCard factoryCard)
		{
			this.matPlayerSections.Add(factoryCard);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x001299DC File Offset: 0x00127BDC
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

		// Token: 0x06002FF2 RID: 12274 RVA: 0x00129A34 File Offset: 0x00127C34
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

		// Token: 0x06002FF3 RID: 12275 RVA: 0x00045B36 File Offset: 0x00043D36
		public int IdleWorkersCount()
		{
			return 8 - this.workers.Count;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x00129A94 File Offset: 0x00127C94
		public void IncrementEnlistedRecruitsCounter()
		{
			int recruitsEnlisted = this.RecruitsEnlisted;
			this.RecruitsEnlisted = recruitsEnlisted + 1;
			if (this.RecruitsEnlisted == 4 && this.gameManager.PlayerCurrent.GetNumberOfStars(StarType.Recruits) == 0)
			{
				this.gameManager.PlayerCurrent.GainStar(StarType.Recruits);
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x00129AB4 File Offset: 0x00127CB4
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

		// Token: 0x06002FF6 RID: 12278 RVA: 0x0002F60E File Offset: 0x0002D80E
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x00129B70 File Offset: 0x00127D70
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

		// Token: 0x06002FF8 RID: 12280 RVA: 0x00129D6C File Offset: 0x00127F6C
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

		// Token: 0x04002099 RID: 8345
		private int startingObjectiveCards;

		// Token: 0x0400209A RID: 8346
		private int startingPopularity;

		// Token: 0x0400209B RID: 8347
		private int startingCoins;

		// Token: 0x0400209C RID: 8348
		private int startingOrderNumber;

		// Token: 0x040020A1 RID: 8353
		private List<MatPlayerSection> matPlayerSections = new List<MatPlayerSection>();

		// Token: 0x040020A2 RID: 8354
		public List<Worker> workers = new List<Worker>();

		// Token: 0x040020A3 RID: 8355
		public List<Building> buildings = new List<Building>();

		// Token: 0x040020A4 RID: 8356
		private GameManager gameManager;
	}
}
