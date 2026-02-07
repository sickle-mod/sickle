using System;
using System.Collections.Generic;
using System.Linq;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000631 RID: 1585
	public class SectionAction
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x00047C2F File Offset: 0x00045E2F
		public int gainActionsCount
		{
			get
			{
				return this.gainActions.Count;
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0012EA58 File Offset: 0x0012CC58
		public SectionAction(GameManager gameManager, PayAction[] payActions, GainAction[] gainActions)
		{
			this.gameManager = gameManager;
			this.payActions = new List<PayAction>();
			this.gainActions = new List<GainAction>();
			foreach (PayAction payAction in payActions)
			{
				this.payActions.Add(payAction);
			}
			foreach (GainAction gainAction in gainActions)
			{
				this.gainActions.Add(gainAction);
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0012EACC File Offset: 0x0012CCCC
		public SectionAction(GameManager gameManager, PayAction payAction, params GainAction[] gainActions)
		{
			this.gameManager = gameManager;
			this.payActions = new List<PayAction>();
			this.gainActions = new List<GainAction>();
			if (payAction != null)
			{
				this.payActions.Add(payAction);
			}
			foreach (GainAction gainAction in gainActions)
			{
				this.gainActions.Add(gainAction);
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x00047C3C File Offset: 0x00045E3C
		public SectionAction(List<PayAction> payActions, List<GainAction> gainActions)
		{
			this.payActions = payActions;
			this.gainActions = gainActions;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x00027E56 File Offset: 0x00026056
		protected SectionAction()
		{
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x0012EB2C File Offset: 0x0012CD2C
		public void SetPlayer(Player player)
		{
			this.player = player;
			foreach (PayAction payAction in this.payActions)
			{
				payAction.SetPlayer(player);
			}
			foreach (GainAction gainAction in this.gainActions)
			{
				gainAction.SetPlayer(player);
			}
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x00047C52 File Offset: 0x00045E52
		public Player GetPlayer()
		{
			return this.player;
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x00047C5A File Offset: 0x00045E5A
		public void ReportLog(LogInfo logInfo)
		{
			this.gameManager.actionLog.LogInfoReported(logInfo);
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x00047C6D File Offset: 0x00045E6D
		public void AddPayAction(PayAction payAction)
		{
			if (payAction != null)
			{
				this.payActions.Add(payAction);
			}
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x00047C7E File Offset: 0x00045E7E
		public int GetNumberOfPayActions()
		{
			return this.payActions.Count;
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x00047C8B File Offset: 0x00045E8B
		public PayAction GetPayAction(int actionId)
		{
			return this.payActions[actionId];
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x00047C99 File Offset: 0x00045E99
		public void OverridePayAction(int actionId, PayAction newAction)
		{
			this.payActions[actionId] = newAction;
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00047CA8 File Offset: 0x00045EA8
		public bool CanPlayerPayActions()
		{
			return this.payActions.All((PayAction a) => a.CanPlayerPay());
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x00047CD4 File Offset: 0x00045ED4
		public int GetMissingResourceCount()
		{
			return this.payActions.Sum((PayAction a) => a.GetMissingResourceCount());
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x00047D00 File Offset: 0x00045F00
		public bool CanPlayerGainFromActions(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			return this.gainActions[index].GainAvaliable();
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x00047D1A File Offset: 0x00045F1A
		public bool CanPlayerGainBuildFromActions()
		{
			return this.gainActions.Any((GainAction x) => x.GetGainType() == GainType.Building);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00047D46 File Offset: 0x00045F46
		public bool CanPlayerGainBuildFromAction(int actionId)
		{
			return this.gainActions[actionId].GetGainType() == GainType.Building;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x00047D5D File Offset: 0x00045F5D
		public bool CanPlayerGainWorkerFromActions()
		{
			return this.gainActions.Any((GainAction x) => x.GetGainType() == GainType.Worker);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x00047D89 File Offset: 0x00045F89
		public bool CanPlayerGainWorkerFromAction(int actionId)
		{
			return this.gainActions[actionId].GetGainType() == GainType.Worker;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x0012EBC4 File Offset: 0x0012CDC4
		public bool CanPlayerGainFromActions()
		{
			using (List<GainAction>.Enumerator enumerator = this.gainActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.GainAvaliable())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0012EC20 File Offset: 0x0012CE20
		public bool CanExecutePayActions()
		{
			using (List<PayAction>.Enumerator enumerator = this.payActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanExecute())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x0012EC7C File Offset: 0x0012CE7C
		public void ExecutePayActions()
		{
			foreach (PayAction payAction in this.payActions)
			{
				payAction.Execute();
			}
			this.CreatePayLogInfo();
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0012ECD4 File Offset: 0x0012CED4
		public bool ActionPayed()
		{
			using (List<PayAction>.Enumerator enumerator = this.payActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Payed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x00047DA0 File Offset: 0x00045FA0
		public void AddGainAction(GainAction gainAction)
		{
			if (gainAction != null)
			{
				this.gainActions.Add(gainAction);
			}
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x00047C2F File Offset: 0x00045E2F
		public int GetNumberOfGainActions()
		{
			return this.gainActions.Count;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x00047DB1 File Offset: 0x00045FB1
		public virtual GainAction GetGainAction(int actionId)
		{
			return this.gainActions[actionId];
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void UpdateIfFactoryMove(int actionId)
		{
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x00047DBF File Offset: 0x00045FBF
		public void OverrideGainAction(int actionId, GainAction newAction)
		{
			this.gainActions[actionId] = newAction;
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x0012ED30 File Offset: 0x0012CF30
		public virtual bool CanExecuteGainActions()
		{
			using (List<GainAction>.Enumerator enumerator = this.gainActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanExecute())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void PrepareForActionCheckAndExecution()
		{
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x0012ED8C File Offset: 0x0012CF8C
		public virtual bool ActionsPrepared()
		{
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.GainAvaliable() && gainAction.ActionSelected)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x00047DCE File Offset: 0x00045FCE
		public virtual bool AllActionsSkipped()
		{
			return !this.NoGainActionsToSkip() && this.IsAnyGainActionSkipped();
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x00047DE0 File Offset: 0x00045FE0
		public virtual bool ActionPrepared(int actionId)
		{
			return this.gainActions[actionId].ActionSelected;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x0012EDF0 File Offset: 0x0012CFF0
		public virtual void ExecuteGainActions()
		{
			LogInfo logInfo = new LogInfo(this.gameManager);
			logInfo.ActionPlacement = ActionPositionType.Other;
			if (this.gainActions.Count > 0)
			{
				logInfo = this.gainActions[0].GetLogInfo();
			}
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.GainAvaliable())
				{
					if (this.gainActions[0] != gainAction)
					{
						logInfo.AdditionalGain.Add(gainAction.GetLogInfo());
						if (logInfo.IsEncounter)
						{
							logInfo.AdditionalGain[logInfo.AdditionalGain.Count - 1].IsEncounter = true;
						}
					}
					gainAction.LogInfoType = LogInfoType.Encounter;
					gainAction.Execute();
				}
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			this.ReportLog(logInfo);
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0012EEF4 File Offset: 0x0012D0F4
		public void CreateLogInfo(GainAction action)
		{
			LogInfo logInfo = action.GetLogInfo();
			this.ReportLog(logInfo);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x00047DF3 File Offset: 0x00045FF3
		public void CreateLogInfo(LogInfo logInfo)
		{
			this.ReportLog(logInfo);
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x0012EF10 File Offset: 0x0012D110
		public virtual void ReportUnfinishedAction()
		{
			if (!this.ActionPayed())
			{
				return;
			}
			LogInfo logInfo = new LogInfo(this.gameManager);
			logInfo.ActionPlacement = ActionPositionType.Other;
			if (this.gainActions.Count > 0)
			{
				if (this.gainActions[0] is GainMove || this.gainActions[0].Gained)
				{
					return;
				}
				logInfo = this.gainActions[0].GetLogInfo();
			}
			if (this.payLogInfos != null)
			{
				logInfo.PayLogInfos = new List<LogInfo>(this.payLogInfos);
			}
			this.payLogInfos = null;
			this.ReportLog(logInfo);
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x0012EFA8 File Offset: 0x0012D1A8
		public List<LogInfo> CreatePayLogInfo()
		{
			this.payLogInfos = new List<LogInfo>();
			foreach (PayAction payAction in this.payActions)
			{
				if (payAction.Payed)
				{
					this.payLogInfos.Add(payAction.GetLogInfo());
				}
			}
			return this.payLogInfos;
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x0012F020 File Offset: 0x0012D220
		public void ClearActions()
		{
			if (this.payLogInfos != null && (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner != null))
			{
				for (int i = 0; i < this.payLogInfos.Count; i++)
				{
					this.ReportLog(this.payLogInfos[i]);
				}
			}
			foreach (PayAction payAction in this.payActions)
			{
				payAction.Clear();
			}
			foreach (GainAction gainAction in this.gainActions)
			{
				gainAction.Clear();
			}
			this.payLogInfos = null;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x0012F100 File Offset: 0x0012D300
		public bool ActionInterrupted()
		{
			using (List<GainAction>.Enumerator enumerator = this.gainActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Gained)
					{
						return false;
					}
				}
			}
			foreach (GainAction gainAction in this.gainActions)
			{
				if (gainAction.GainAvaliable() && ((this.ActionPayed() && !gainAction.ActionSelected) || (gainAction.ActionSelected && !gainAction.Gained)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x00047DFC File Offset: 0x00045FFC
		private bool NoGainActionsToSkip()
		{
			return this.gainActions.Count == 0 || !this.CanPlayerGainFromActions();
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x0012F1C0 File Offset: 0x0012D3C0
		private bool IsAnyGainActionSkipped()
		{
			using (List<GainAction>.Enumerator enumerator = this.gainActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ActionSelected)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040021A8 RID: 8616
		protected List<PayAction> payActions;

		// Token: 0x040021A9 RID: 8617
		protected List<GainAction> gainActions;

		// Token: 0x040021AA RID: 8618
		protected Player player;

		// Token: 0x040021AB RID: 8619
		protected List<LogInfo> payLogInfos;

		// Token: 0x040021AC RID: 8620
		protected GameManager gameManager;
	}
}
