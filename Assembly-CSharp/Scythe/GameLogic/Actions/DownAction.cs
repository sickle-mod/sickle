using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200060B RID: 1547
	public class DownAction : SectionAction, IXmlSerializable
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06003096 RID: 12438 RVA: 0x000466E4 File Offset: 0x000448E4
		// (set) Token: 0x06003097 RID: 12439 RVA: 0x000466EC File Offset: 0x000448EC
		public DownActionType Type { get; private set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x000466F5 File Offset: 0x000448F5
		// (set) Token: 0x06003099 RID: 12441 RVA: 0x000466FD File Offset: 0x000448FD
		public bool IsRecruitEnlisted { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x00046706 File Offset: 0x00044906
		// (set) Token: 0x0600309B RID: 12443 RVA: 0x0004670E File Offset: 0x0004490E
		public bool OldRecruitValue { get; set; }

		// Token: 0x0600309C RID: 12444 RVA: 0x00127434 File Offset: 0x00125634
		public DownAction(GameManager gameManager, DownActionType type)
		{
			this.gameManager = gameManager;
			this.payActions = new List<PayAction>();
			this.gainActions = new List<GainAction>();
			this.IsRecruitEnlisted = false;
			this.Type = type;
			if (type == DownActionType.Factory)
			{
				this.gainActions.Add(new GainMove(gameManager, 1, 0));
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x0012748C File Offset: 0x0012568C
		public DownAction(GameManager gameManager, DownActionType downActionType, short payAmount, short payUpgradeLevel, short gainCoinsAmount)
		{
			this.gameManager = gameManager;
			this.Type = downActionType;
			this.IsRecruitEnlisted = false;
			this.payActions = new List<PayAction>();
			this.gainActions = new List<GainAction>();
			switch (downActionType)
			{
			case DownActionType.Upgrade:
				this.payActions.Add(new PayResource(gameManager, ResourceType.oil, payAmount, payUpgradeLevel, false, false));
				this.payActions[0].LogInfoType = LogInfoType.Upgrade;
				this.gainActions.Add(new GainUpgrade(gameManager, 1, 0, false));
				break;
			case DownActionType.Deploy:
				this.payActions.Add(new PayResource(gameManager, ResourceType.metal, payAmount, payUpgradeLevel, false, false));
				this.payActions[0].LogInfoType = LogInfoType.Deploy;
				this.gainActions.Add(new GainMech(gameManager, 1, 0, false));
				break;
			case DownActionType.Build:
				this.payActions.Add(new PayResource(gameManager, ResourceType.wood, payAmount, payUpgradeLevel, false, false));
				this.payActions[0].LogInfoType = LogInfoType.Build;
				this.gainActions.Add(new GainBuilding(gameManager, 1, 0, false));
				break;
			case DownActionType.Enlist:
				this.payActions.Add(new PayResource(gameManager, ResourceType.food, payAmount, payUpgradeLevel, false, false));
				this.payActions[0].LogInfoType = LogInfoType.Enlist;
				this.gainActions.Add(new GainRecruit(gameManager, 1, 0, false));
				break;
			}
			if (gainCoinsAmount > 0)
			{
				GainAction gainAction = new GainCoin(gameManager, gainCoinsAmount, 0, false, false, false);
				this.gainActions.Add(gainAction);
				switch (downActionType)
				{
				case DownActionType.Upgrade:
					gainAction.LogInfoType = LogInfoType.Upgrade;
					return;
				case DownActionType.Deploy:
					gainAction.LogInfoType = LogInfoType.Deploy;
					return;
				case DownActionType.Build:
					gainAction.LogInfoType = LogInfoType.Build;
					return;
				case DownActionType.Enlist:
					gainAction.LogInfoType = LogInfoType.Enlist;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x00046717 File Offset: 0x00044917
		public override GainAction GetGainAction(int actionId)
		{
			return base.GetGainAction(actionId);
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x00127640 File Offset: 0x00125840
		public override void UpdateIfFactoryMove(int actionId)
		{
			if (this.Type == DownActionType.Factory && actionId == 0 && this.player != null)
			{
				Character character = this.player.character;
				short num = character.MovesLeft;
				character.MovesLeft = num + 1;
				foreach (Mech mech in this.player.matFaction.mechs)
				{
					num = mech.MovesLeft;
					mech.MovesLeft = num + 1;
				}
				foreach (Worker worker in this.player.matPlayer.workers)
				{
					num = worker.MovesLeft;
					worker.MovesLeft = num + 1;
				}
			}
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x00046720 File Offset: 0x00044920
		public void EnlistRecruit()
		{
			this.IsRecruitEnlisted = true;
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00046729 File Offset: 0x00044929
		public bool RecruitEnlisted()
		{
			return this.IsRecruitEnlisted;
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x00046731 File Offset: 0x00044931
		public override void PrepareForActionCheckAndExecution()
		{
			this.OldRecruitValue = this.IsRecruitEnlisted;
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x00127734 File Offset: 0x00125934
		public override void ExecuteGainActions()
		{
			this.CreateLogInfo();
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.ActionSelected && gainAction.GainAvaliable())
				{
					gainAction.Execute();
				}
			}
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x0004673F File Offset: 0x0004493F
		public bool IsMaxReached()
		{
			return this.gainActions[0].IsMaxReached();
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x0012779C File Offset: 0x0012599C
		private void CreateLogInfo()
		{
			if (this.Type == DownActionType.Factory)
			{
				return;
			}
			LogInfo logInfo = this.CreateLogInfo(this.gainActions[0]);
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction != this.gainActions[0])
				{
					logInfo.AdditionalGain.Add(gainAction.GetLogInfo());
				}
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			base.ReportLog(logInfo);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x0012784C File Offset: 0x00125A4C
		private new LogInfo CreateLogInfo(GainAction action)
		{
			LogInfo logInfo = action.GetLogInfo();
			logInfo.ActionPlacement = ActionPositionType.Down;
			switch (this.Type)
			{
			case DownActionType.Upgrade:
				logInfo.Type = LogInfoType.Upgrade;
				break;
			case DownActionType.Deploy:
				logInfo.Type = LogInfoType.Deploy;
				break;
			case DownActionType.Build:
				logInfo.Type = LogInfoType.Build;
				break;
			case DownActionType.Enlist:
				logInfo.Type = LogInfoType.Enlist;
				break;
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			return logInfo;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x001278D4 File Offset: 0x00125AD4
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

		// Token: 0x060030A8 RID: 12456 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x0012797C File Offset: 0x00125B7C
		public void ReadXml(XmlReader reader)
		{
			foreach (GainAction gainAction in this.gainActions)
			{
				if (reader.Name == "DownGain")
				{
					((IXmlSerializable)gainAction).ReadXml(reader);
				}
				reader.ReadStartElement();
			}
			foreach (PayAction payAction in this.payActions)
			{
				if (reader.Name == "Pay")
				{
					((IXmlSerializable)payAction).ReadXml(reader);
				}
				if (payAction.GetPayType() != PayType.Resource)
				{
					reader.ReadStartElement();
				}
			}
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x00127A4C File Offset: 0x00125C4C
		public void WriteXml(XmlWriter writer)
		{
			foreach (IXmlSerializable xmlSerializable in this.gainActions)
			{
				writer.WriteStartElement("DownGain");
				xmlSerializable.WriteXml(writer);
				writer.WriteEndElement();
			}
			foreach (IXmlSerializable xmlSerializable2 in this.payActions)
			{
				writer.WriteStartElement("Pay");
				xmlSerializable2.WriteXml(writer);
				writer.WriteEndElement();
			}
		}
	}
}
