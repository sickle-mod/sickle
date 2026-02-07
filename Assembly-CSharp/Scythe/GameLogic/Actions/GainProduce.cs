using System;
using System.Collections.Generic;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000620 RID: 1568
	public class GainProduce : GainAction
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x000472A9 File Offset: 0x000454A9
		// (set) Token: 0x0600318F RID: 12687 RVA: 0x000472B1 File Offset: 0x000454B1
		public Dictionary<GameHex, int> ResourceFields { get; private set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000472BA File Offset: 0x000454BA
		// (set) Token: 0x06003191 RID: 12689 RVA: 0x000472C2 File Offset: 0x000454C2
		public bool MillProduce { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x000472CB File Offset: 0x000454CB
		// (set) Token: 0x06003193 RID: 12691 RVA: 0x000472D3 File Offset: 0x000454D3
		public int FieldsLeftToProduce { get; private set; }

		// Token: 0x06003194 RID: 12692 RVA: 0x000472DC File Offset: 0x000454DC
		public GainProduce()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Produce;
			this.ResourceFields = new Dictionary<GameHex, int>();
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000472FB File Offset: 0x000454FB
		public GainProduce(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Produce;
			this.ResourceFields = new Dictionary<GameHex, int>();
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x0004731B File Offset: 0x0004551B
		public GainProduce(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool millProduce = false, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Produce;
			this.MillProduce = millProduce;
			this.FieldsLeftToProduce = (int)amount;
			this.ResourceFields = new Dictionary<GameHex, int>();
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x0004734B File Offset: 0x0004554B
		public bool AddResourceField(GameHex resourceField, int amount)
		{
			this.ResourceFields.Add(resourceField, amount);
			return true;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x00046D82 File Offset: 0x00044F82
		public void SelectAction()
		{
			base.ActionSelected = true;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0004735B File Offset: 0x0004555B
		public GameHex GetCurrentField()
		{
			return this.fieldToProduce;
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x00047363 File Offset: 0x00045563
		public int GetCurrentAmount()
		{
			return this.amount;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x0012C12C File Offset: 0x0012A32C
		public override bool GainAvaliable()
		{
			bool flag = true;
			int num = 0;
			foreach (Worker worker in this.gameManager.PlayerCurrent.matPlayer.workers)
			{
				if (worker.position == this.gameManager.PlayerCurrent.GetCapital() || worker.position.hexType == HexType.lake)
				{
					num++;
				}
			}
			if (num == this.gameManager.PlayerCurrent.matPlayer.workers.Count)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x0004736B File Offset: 0x0004556B
		public override bool CanExecute()
		{
			return this.CheckLogic(this.ResourceFields);
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x0012C1D8 File Offset: 0x0012A3D8
		private bool CheckLogic(Dictionary<GameHex, int> resourceFields)
		{
			foreach (KeyValuePair<GameHex, int> keyValuePair in resourceFields)
			{
				if (!this.CheckLogic(keyValuePair.Key, keyValuePair.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x0012C23C File Offset: 0x0012A43C
		private bool CheckLogic(GameHex resourceField, int amountToProduce)
		{
			if (resourceField == null)
			{
				return false;
			}
			if (resourceField.hexType == HexType.factory || resourceField.hexType == HexType.lake || resourceField.hexType == HexType.capital)
			{
				return false;
			}
			if (!this.player.OwnedFields(false).Contains(resourceField))
			{
				return false;
			}
			if (resourceField.hexType == HexType.village)
			{
				if (amountToProduce > (int)((short)(8 - this.player.matPlayer.workers.Count)))
				{
					return false;
				}
			}
			else
			{
				short num = 0;
				using (List<Worker>.Enumerator enumerator = this.player.matPlayer.workers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.position == resourceField)
						{
							num += 1;
						}
					}
				}
				if (this.MillProduce && resourceField.Building != null && resourceField.Building.buildingType == BuildingType.Mill && resourceField.Building.player == this.player)
				{
					num += 1;
				}
				if ((int)num < amountToProduce)
				{
					return false;
				}
			}
			return this.FieldsLeftToProduce != 0 || this.MillProduce;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x00047379 File Offset: 0x00045579
		public bool ExecuteOnce()
		{
			return this.ExecuteOnce(this.fieldToProduce, this.amount);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x0012C354 File Offset: 0x0012A554
		public bool ExecuteOnce(GameHex resourceField, int amountToProduce)
		{
			if (!this.CheckLogic(resourceField, amountToProduce))
			{
				return false;
			}
			if (this.gameManager.IsMultiplayer)
			{
				this.fieldToProduce = resourceField;
				this.amount = amountToProduce;
				if ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman))
				{
					this.gameManager.OnActionSent(new GainProduceMessage(resourceField, amountToProduce, (short)this.FieldsLeftToProduce));
				}
			}
			ResourceType resourceType = ResourceType.oil;
			HexType hexType = resourceField.hexType;
			switch (hexType)
			{
			case HexType.mountain:
			{
				Dictionary<ResourceType, int> dictionary = resourceField.resources;
				dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + amountToProduce;
				resourceType = ResourceType.metal;
				break;
			}
			case HexType.forest:
			{
				Dictionary<ResourceType, int> dictionary = resourceField.resources;
				dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + amountToProduce;
				resourceType = ResourceType.wood;
				break;
			}
			case (HexType)3:
				break;
			case HexType.farm:
			{
				Dictionary<ResourceType, int> dictionary = resourceField.resources;
				dictionary[ResourceType.food] = dictionary[ResourceType.food] + amountToProduce;
				resourceType = ResourceType.food;
				break;
			}
			default:
				if (hexType != HexType.tundra)
				{
					if (hexType == HexType.village)
					{
						GainWorker gainWorker = new GainWorker(this.gameManager);
						gainWorker.LogInfoType = LogInfoType.Produce;
						resourceType = ResourceType.worker;
						gainWorker.SetPlayer(this.player);
						gainWorker.SetLocationAndWorkersAmount(resourceField, amountToProduce);
						gainWorker.Execute();
					}
				}
				else
				{
					Dictionary<ResourceType, int> dictionary = resourceField.resources;
					dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + amountToProduce;
					resourceType = ResourceType.oil;
				}
				break;
			}
			if (((this.gameManager.IsMultiplayer && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)) && resourceType != ResourceType.worker)
			{
				ProduceEnemyActionInfo produceEnemyActionInfo = new ProduceEnemyActionInfo();
				produceEnemyActionInfo.fromEncounter = base.IsEncounter;
				produceEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				produceEnemyActionInfo.actionType = LogInfoType.Produce;
				produceEnemyActionInfo.hex = resourceField;
				produceEnemyActionInfo.resourceType = resourceType;
				produceEnemyActionInfo.amount = amountToProduce;
				resourceField.skipTopActionPresentationUpdate = true;
				resourceField.snapshotResourcesAfterTopAction = new Dictionary<ResourceType, int>(resourceField.resources);
				if (amountToProduce != 0)
				{
					this.gameManager.EnemyProduceResources(produceEnemyActionInfo);
				}
			}
			this.AddResourceField(resourceField, amountToProduce);
			if (resourceField.Building == null || resourceField.Building.player != this.GetPlayer() || resourceField.Building.buildingType != BuildingType.Mill || !this.MillProduce || this.GetPlayer().currentMatSection == 4)
			{
				int num = this.FieldsLeftToProduce - 1;
				this.FieldsLeftToProduce = num;
			}
			return true;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x0012C5B0 File Offset: 0x0012A7B0
		public override LogInfo GetLogInfo()
		{
			return new ProductionLogInfo(this.gameManager)
			{
				Type = LogInfoType.Produce,
				PlayerAssigned = this.player.matFaction.faction,
				Hexes = new Dictionary<GameHex, int>(this.ResourceFields),
				MillUsed = this.MillProduce,
				IsEncounter = base.IsEncounter
			};
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x0004738D File Offset: 0x0004558D
		public bool ContainsFieldToProduce()
		{
			return this.fieldToProduce != null;
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x0012C610 File Offset: 0x0012A810
		public override void Execute()
		{
			base.Gained = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainProduceMessage());
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00047398 File Offset: 0x00045598
		public override void Clear()
		{
			base.Gained = false;
			this.ResourceFields.Clear();
			base.ActionSelected = false;
			this.FieldsLeftToProduce = (int)base.Amount;
			this.fieldToProduce = null;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000473C6 File Offset: 0x000455C6
		public override void Upgrade()
		{
			base.Upgrade();
			this.FieldsLeftToProduce = (int)base.Amount;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0012C67C File Offset: 0x0012A87C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Mill") != null)
			{
				this.MillProduce = true;
			}
			this.FieldsLeftToProduce = int.Parse(reader.GetAttribute("Fields"));
			reader.ReadStartElement();
			if (reader.Name == "Hex")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.amount = int.Parse(reader.GetAttribute("Amount"));
				this.fieldToProduce = this.gameManager.gameBoard.hexMap[num, num2];
				reader.ReadStartElement();
			}
			if (reader.Name == "Fields")
			{
				reader.ReadStartElement();
				while (reader.Name == "Hex")
				{
					int num3 = int.Parse(reader.GetAttribute("X"));
					int num4 = int.Parse(reader.GetAttribute("Y"));
					int num5 = int.Parse(reader.GetAttribute("Amount"));
					this.ResourceFields.Add(this.gameManager.gameBoard.hexMap[num3, num4], num5);
					reader.ReadStartElement();
					if (reader.NodeType == XmlNodeType.EndElement)
					{
						reader.ReadEndElement();
					}
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x0012C7DC File Offset: 0x0012A9DC
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.MillProduce)
			{
				writer.WriteAttributeString("Mill", "");
			}
			writer.WriteAttributeString("Fields", this.FieldsLeftToProduce.ToString());
			if (this.fieldToProduce != null && !base.Gained)
			{
				writer.WriteStartElement("Hex");
				writer.WriteAttributeString("X", this.fieldToProduce.posX.ToString());
				writer.WriteAttributeString("Y", this.fieldToProduce.posY.ToString());
				writer.WriteAttributeString("Amount", this.amount.ToString());
				writer.WriteEndElement();
			}
			if (this.ResourceFields.Count > 0)
			{
				writer.WriteStartElement("Fields");
				foreach (KeyValuePair<GameHex, int> keyValuePair in this.ResourceFields)
				{
					writer.WriteStartElement("Hex");
					writer.WriteAttributeString("X", keyValuePair.Key.posX.ToString());
					writer.WriteAttributeString("Y", keyValuePair.Key.posY.ToString());
					writer.WriteAttributeString("Amount", keyValuePair.Value.ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x04002185 RID: 8581
		private GameHex fieldToProduce;

		// Token: 0x04002186 RID: 8582
		private int amount;
	}
}
