using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000634 RID: 1588
	public class TopAction : SectionAction, IXmlSerializable
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x00047E4A File Offset: 0x0004604A
		// (set) Token: 0x06003275 RID: 12917 RVA: 0x00047E52 File Offset: 0x00046052
		public TopActionType Type { get; private set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06003276 RID: 12918 RVA: 0x00047E5B File Offset: 0x0004605B
		// (set) Token: 0x06003277 RID: 12919 RVA: 0x00047E63 File Offset: 0x00046063
		public bool DifferentGain { get; private set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x00047E6C File Offset: 0x0004606C
		// (set) Token: 0x06003279 RID: 12921 RVA: 0x00047E74 File Offset: 0x00046074
		public Building Structure { get; set; }

		// Token: 0x0600327A RID: 12922 RVA: 0x0012F21C File Offset: 0x0012D41C
		public TopAction(GameManager gameManager, PayAction[] payActions, GainAction[] gainActions, bool differentGain = false)
			: base(gameManager, payActions, gainActions)
		{
			this.Type = TopActionType.Factory;
			this.DifferentGain = differentGain;
			for (int i = 0; i < payActions.Length; i++)
			{
				payActions[i].LogInfoType = LogInfoType.FactoryTopAction;
			}
			for (int i = 0; i < gainActions.Length; i++)
			{
				gainActions[i].LogInfoType = LogInfoType.FactoryTopAction;
			}
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00047E7D File Offset: 0x0004607D
		public TopAction(TopActionType actionType, List<PayAction> payActions, List<GainAction> gainActions, bool differentGain)
			: base(payActions, gainActions)
		{
			this.Type = actionType;
			this.DifferentGain = differentGain;
			if (actionType != TopActionType.Factory)
			{
				this.SetStructure(actionType);
			}
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x0012F278 File Offset: 0x0012D478
		public TopAction(GameManager gameManager, TopActionType actionType)
		{
			this.gameManager = gameManager;
			this.payActions = new List<PayAction>();
			this.gainActions = new List<GainAction>();
			this.Type = actionType;
			switch (actionType)
			{
			case TopActionType.Bolster:
				this.payActions.Add(new PayCoin(gameManager, 1, 0, false, false));
				this.gainActions.Add(new GainPower(gameManager, 2, 1, false, false, false));
				this.gainActions.Add(new GainCombatCard(gameManager, 1, 1, false, false, false));
				this.DifferentGain = true;
				this.payActions[0].LogInfoType = LogInfoType.Bolster;
				this.gainActions[0].LogInfoType = LogInfoType.BolsterPower;
				this.gainActions[1].LogInfoType = LogInfoType.BolsterCombatCard;
				break;
			case TopActionType.Trade:
				this.payActions.Add(new PayCoin(gameManager, 1, 0, false, false));
				this.gainActions.Add(new GainAnyResource(gameManager, 2, 0, false));
				this.gainActions.Add(new GainPopularity(gameManager, 1, 1, false, false, false));
				this.payActions[0].LogInfoType = LogInfoType.Trade;
				this.gainActions[0].LogInfoType = LogInfoType.TradeResources;
				this.gainActions[1].LogInfoType = LogInfoType.TradePopularity;
				this.DifferentGain = true;
				break;
			case TopActionType.Produce:
				this.gainActions.Add(new GainProduce(gameManager, 2, 1, false, false));
				this.gainActions[0].LogInfoType = LogInfoType.Produce;
				break;
			case TopActionType.MoveGain:
				this.gainActions.Add(new GainMove(gameManager, 2, 1));
				this.gainActions.Add(new GainCoin(gameManager, 1, 1, false, false, false));
				this.gainActions[0].LogInfoType = LogInfoType.Move;
				this.gainActions[1].LogInfoType = LogInfoType.MoveCoins;
				this.DifferentGain = true;
				break;
			}
			this.SetStructure(actionType);
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x0012F454 File Offset: 0x0012D654
		private void SetStructure(TopActionType type)
		{
			this.Structure = new Building(this.gameManager);
			switch (type)
			{
			case TopActionType.Bolster:
				this.Structure.buildingType = BuildingType.Monument;
				return;
			case TopActionType.Trade:
				this.Structure.buildingType = BuildingType.Armory;
				return;
			case TopActionType.Produce:
				this.Structure.buildingType = BuildingType.Mill;
				return;
			case TopActionType.MoveGain:
				this.Structure.buildingType = BuildingType.Mine;
				return;
			default:
				this.Structure = null;
				return;
			}
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00046717 File Offset: 0x00044917
		public override GainAction GetGainAction(int actionId)
		{
			return base.GetGainAction(actionId);
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0012F4C8 File Offset: 0x0012D6C8
		public override bool CanExecuteGainActions()
		{
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.ActionSelected && !gainAction.CanExecute())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x0012F52C File Offset: 0x0012D72C
		public override void ExecuteGainActions()
		{
			if (this.Type == TopActionType.Factory)
			{
				base.ReportLog(this.CreateFactoryLogInfo());
			}
			LogInfo logInfo = null;
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.ActionSelected && gainAction.GainAvaliable())
				{
					if (this.Type != TopActionType.Factory && gainAction.GetGainType() != GainType.Move)
					{
						if (logInfo == null)
						{
							logInfo = this.CreateLogInfo(gainAction);
							if (this.payLogInfos != null)
							{
								logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
							}
							this.payLogInfos = null;
						}
						else
						{
							logInfo.AdditionalGain.Add(this.CreateLogInfo(gainAction));
						}
					}
					gainAction.Execute();
				}
			}
			if (this.Type != TopActionType.Factory && logInfo != null && logInfo.Type != LogInfoType.Move)
			{
				base.ReportLog(logInfo);
			}
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x0012F614 File Offset: 0x0012D814
		private new LogInfo CreateLogInfo(GainAction action)
		{
			LogInfo logInfo = action.GetLogInfo();
			logInfo.ActionPlacement = ActionPositionType.Top;
			switch (this.Type)
			{
			case TopActionType.Bolster:
				if (action.GetGainType() == GainType.CombatCard)
				{
					logInfo.Type = LogInfoType.BolsterCombatCard;
				}
				else
				{
					logInfo.Type = LogInfoType.BolsterPower;
				}
				break;
			case TopActionType.Trade:
				if (action.GetGainType() == GainType.AnyResource)
				{
					logInfo.Type = LogInfoType.TradeResources;
				}
				else
				{
					logInfo.Type = LogInfoType.TradePopularity;
				}
				break;
			case TopActionType.Produce:
				logInfo.Type = LogInfoType.Produce;
				break;
			case TopActionType.MoveGain:
				if (action.GetGainType() == GainType.Coin)
				{
					logInfo.Type = LogInfoType.MoveCoins;
				}
				else
				{
					logInfo.Type = LogInfoType.Move;
				}
				break;
			case TopActionType.Factory:
				logInfo.Type = LogInfoType.FactoryTopAction;
				break;
			}
			return logInfo;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0012F6B8 File Offset: 0x0012D8B8
		private LogInfo CreateFactoryLogInfo()
		{
			LogInfo logInfo = new FactoryLogInfo(this.gameManager);
			logInfo.Type = LogInfoType.FactoryTopAction;
			logInfo.IsEncounter = false;
			logInfo.PlayerAssigned = this.player.matFaction.faction;
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.ActionSelected && gainAction.GainAvaliable())
				{
					logInfo.AdditionalGain.Add(gainAction.GetLogInfo());
				}
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			return logInfo;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x0012F778 File Offset: 0x0012D978
		private void CreateLogInfo()
		{
			LogInfo logInfo = this.CreateLogInfo(this.gainActions[0]);
			logInfo.PlayerAssigned = this.player.matFaction.faction;
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.ActionSelected)
				{
					logInfo.AdditionalGain.Add(this.CreateLogInfo(gainAction));
				}
			}
			logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			this.payLogInfos = null;
			base.ReportLog(logInfo);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0012F828 File Offset: 0x0012DA28
		public override void ReportUnfinishedAction()
		{
			if (!base.ActionPayed())
			{
				return;
			}
			LogInfo logInfo = new LogInfo(this.gameManager);
			logInfo.PlayerAssigned = this.player.matFaction.faction;
			if (this.gainActions.Count > 0)
			{
				if (this.gainActions[0] is GainMove || this.gainActions[0].Gained)
				{
					return;
				}
				logInfo = this.CreateLogInfo(this.gainActions[0]);
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			base.ReportLog(logInfo);
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x0012F8D0 File Offset: 0x0012DAD0
		public void ReadXml(XmlReader reader)
		{
			for (int i = 0; i < this.payActions.Count; i++)
			{
				if (reader.GetAttribute("P" + i.ToString()) != null)
				{
					this.payActions[i].SetPayed(true);
				}
				else
				{
					this.payActions[i].SetPayed(false);
				}
			}
			reader.ReadStartElement();
			foreach (GainAction gainAction in this.gainActions)
			{
				if (reader.Name == "Gain")
				{
					((IXmlSerializable)gainAction).ReadXml(reader);
				}
				if (gainAction.GetGainType() != GainType.Move && gainAction.GetGainType() != GainType.Produce && gainAction.GetGainType() != GainType.AnyResource)
				{
					reader.ReadStartElement();
				}
			}
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x0012F9B4 File Offset: 0x0012DBB4
		public void WriteXml(XmlWriter writer)
		{
			for (int i = 0; i < this.payActions.Count; i++)
			{
				if (this.payActions[i].Payed)
				{
					writer.WriteAttributeString("P" + i.ToString(), "");
				}
			}
			for (int j = 0; j < this.gainActions.Count; j++)
			{
				writer.WriteStartElement("Gain");
				((IXmlSerializable)this.gainActions[j]).WriteXml(writer);
				writer.WriteEndElement();
			}
		}
	}
}
