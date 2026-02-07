using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x0200055F RID: 1375
	public class ActionLog : IXmlSerializable
	{
		// Token: 0x1400012F RID: 303
		// (add) Token: 0x06002C03 RID: 11267 RVA: 0x000F44E0 File Offset: 0x000F26E0
		// (remove) Token: 0x06002C04 RID: 11268 RVA: 0x000F4514 File Offset: 0x000F2714
		public static event Action<LogInfo, int> LogInfoCreated;

		// Token: 0x06002C05 RID: 11269 RVA: 0x000440AA File Offset: 0x000422AA
		public ActionLog(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000440DA File Offset: 0x000422DA
		public void Clear()
		{
			this.lastTurn = 0;
			this.workersStarLog = null;
			this.newestLogInfos.Clear();
			this.logInfoHistory.Clear();
			this.ClearAwaitingPayActions();
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00044106 File Offset: 0x00042306
		public void ClearAwaitingPayActions()
		{
			this.awaitingPayActions.Clear();
			this.newestLogInfos.Clear();
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0004411E File Offset: 0x0004231E
		public LogInfo GetLogInfo(int id)
		{
			return this.logInfoHistory[id];
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x0004412C File Offset: 0x0004232C
		public int GetLogAmount()
		{
			return this.logInfoHistory.Count;
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x00044139 File Offset: 0x00042339
		public List<LogInfo> GetLogHistory()
		{
			return this.logInfoHistory;
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00044141 File Offset: 0x00042341
		public void RemoveLastLog()
		{
			this.logInfoHistory.RemoveAt(this.logInfoHistory.Count - 1);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000F4548 File Offset: 0x000F2748
		public void LogInfoReported(LogInfo logInfo)
		{
			this.PrintDebug(logInfo, "Log recieved");
			if (logInfo is PayResourceLogInfo || logInfo is PayNonboardResourceLogInfo)
			{
				if (this.gameManager.actionManager.GetLastSelectedGainAction() == null)
				{
					this.awaitingPayActions.Add(logInfo);
				}
				return;
			}
			if (logInfo.Type == LogInfoType.Produce)
			{
				using (Dictionary<GameHex, int>.KeyCollection.Enumerator enumerator = (logInfo as ProductionLogInfo).Hexes.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == null)
						{
							break;
						}
					}
				}
				if (this.logInfoHistory.Count > 0)
				{
					LogInfo logInfo2 = this.logInfoHistory[this.logInfoHistory.Count - 1];
				}
			}
			if (logInfo.Type == LogInfoType.GainStar && (logInfo as StarLogInfo).GainedStar == StarType.Workers)
			{
				this.workersStarLog = logInfo;
				return;
			}
			this.newestLogInfos.Enqueue(logInfo);
			if (logInfo.Type == LogInfoType.Produce && this.workersStarLog != null)
			{
				this.newestLogInfos.Enqueue(this.workersStarLog);
				this.workersStarLog = null;
			}
			while (this.newestLogInfos.Count != 0)
			{
				LogInfo logInfo3 = this.newestLogInfos.Dequeue();
				if (this.LogCanBeMerged(logInfo3))
				{
					this.MergeLog(logInfo3);
				}
				else
				{
					this.CreateNewLog(logInfo3);
				}
			}
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void PrintDebug(LogInfo logInfo, string message = "Log recieved")
		{
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000F469C File Offset: 0x000F289C
		private bool LogCanBeMerged(LogInfo logInfo)
		{
			return this.logInfoHistory.Count > 0 && this.lastTurn == this.gameManager.TurnCount && this.PreviousActionHasTheSamePlayer(logInfo) && this.NotAStructureBonus(logInfo) && this.NotANonmergableAction(logInfo) && (this.CanBeMergedToEncounter(logInfo) || this.CanBeMergedToProduce(logInfo) || this.CanBeMergedToOneMove(logInfo) || this.CanBeMergedToActionWithAdditionalGain(logInfo));
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000F470C File Offset: 0x000F290C
		private bool NotAStructureBonus(LogInfo logInfo)
		{
			return (logInfo.Type != LogInfoType.GainPopularity && logInfo.Type != LogInfoType.GainPower) || logInfo.ActionPlacement != ActionPositionType.Top || logInfo.ActionPlacement != this.logInfoHistory[this.logInfoHistory.Count - 1].ActionPlacement || logInfo.PlayerAssigned != this.logInfoHistory[this.logInfoHistory.Count - 1].PlayerAssigned;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x0004415B File Offset: 0x0004235B
		private bool NotANonmergableAction(LogInfo logInfo)
		{
			return logInfo.Type != LogInfoType.GainStar && logInfo.Type != LogInfoType.Combat && logInfo.Type != LogInfoType.FactoryCardGain && logInfo.Type != LogInfoType.PassCoins;
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x0004418A File Offset: 0x0004238A
		private bool PreviousActionHasTheSamePlayer(LogInfo logInfo)
		{
			return logInfo.PlayerAssigned == this.logInfoHistory[this.logInfoHistory.Count - 1].PlayerAssigned;
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000F4784 File Offset: 0x000F2984
		private bool CanBeMergedToEncounter(LogInfo logInfo)
		{
			return (this.gameManager.IsMultiplayer && this.logInfoHistory.Count >= 3 && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == LogInfoType.GainStar && logInfo.IsEncounter && this.logInfoHistory[this.logInfoHistory.Count - 2].IsEncounter) || (logInfo.IsEncounter && this.logInfoHistory[this.logInfoHistory.Count - 1].IsEncounter);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000F4820 File Offset: 0x000F2A20
		private bool CanBeMergedToProduce(LogInfo logInfo)
		{
			return logInfo.Type == LogInfoType.Produce && logInfo is ProductionLogInfo && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == logInfo.Type && this.logInfoHistory[this.logInfoHistory.Count - 1] is ProductionLogInfo;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000F4888 File Offset: 0x000F2A88
		private bool CanBeMergedToActionWithAdditionalGain(LogInfo logInfo)
		{
			return logInfo.Type != LogInfoType.Move && !logInfo.IsEncounter && logInfo.Type != LogInfoType.TokenAction && ((this.gameManager.IsMultiplayer && this.logInfoHistory.Count >= 3 && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == LogInfoType.GainStar && logInfo.ActionPlacement == this.logInfoHistory[this.logInfoHistory.Count - 2].ActionPlacement) || logInfo.ActionPlacement == this.logInfoHistory[this.logInfoHistory.Count - 1].ActionPlacement);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000F493C File Offset: 0x000F2B3C
		private bool CanBeMergedToOneMove(LogInfo logInfo)
		{
			if (logInfo.Type == LogInfoType.Move && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == logInfo.Type && this.gameManager.IsMultiplayer && !this.gameManager.IsMyTurn())
			{
				Unit unit = (logInfo as HexUnitResourceLogInfo).Units[0];
				Unit unit2 = (this.logInfoHistory[this.logInfoHistory.Count - 1] as HexUnitResourceLogInfo).Units[0];
				return unit.Id == unit2.Id && unit.UnitType == unit2.UnitType;
			}
			return false;
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000F49F0 File Offset: 0x000F2BF0
		private void MergeLog(LogInfo logInfo)
		{
			if (this.CanBeMergedToEncounter(logInfo))
			{
				this.MergeEncounterLogs(logInfo);
			}
			else if (this.CanBeMergedToProduce(logInfo))
			{
				this.MergeProduceLogs(logInfo);
			}
			else if (this.CanBeMergedToOneMove(logInfo))
			{
				this.MergeMoveLogs(logInfo);
			}
			else if (this.CanBeMergedToActionWithAdditionalGain(logInfo))
			{
				this.MergeAdditionalGain(logInfo);
			}
			this.AddPayLogs(this.logInfoHistory[this.logInfoHistory.Count - 1]);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000F4A64 File Offset: 0x000F2C64
		private void MergeEncounterLogs(LogInfo logInfo)
		{
			int num = ((this.gameManager.IsMultiplayer && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == LogInfoType.GainStar && this.logInfoHistory.Count >= 3) ? (this.logInfoHistory.Count - 2) : (this.logInfoHistory.Count - 1));
			if (this.logInfoHistory[num].Type == logInfo.Type && logInfo.Type == LogInfoType.GainCombatCard)
			{
				(this.logInfoHistory[num] as GainNonboardResourceLogInfo).Amount += (logInfo as GainNonboardResourceLogInfo).Amount;
				return;
			}
			this.logInfoHistory[num].PayLogInfos.AddRange(new List<LogInfo>(logInfo.PayLogInfos));
			this.logInfoHistory[num].AdditionalGain.Add(logInfo);
			this.logInfoHistory[num].AdditionalGain.AddRange(new List<LogInfo>(logInfo.AdditionalGain));
			logInfo.PayLogInfos.Clear();
			logInfo.AdditionalGain.Clear();
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000F4B84 File Offset: 0x000F2D84
		private void MergeProduceLogs(LogInfo logInfo)
		{
			ProductionLogInfo productionLogInfo = this.logInfoHistory[this.logInfoHistory.Count - 1] as ProductionLogInfo;
			ProductionLogInfo productionLogInfo2 = logInfo as ProductionLogInfo;
			if (productionLogInfo != null)
			{
				LogInfoType type = productionLogInfo.Type;
				if (productionLogInfo != null)
				{
					Dictionary<GameHex, int> hexes = productionLogInfo.Hexes;
				}
			}
			if (productionLogInfo2.Hexes != null)
			{
				if (productionLogInfo.Hexes == null)
				{
					productionLogInfo.Hexes = new Dictionary<GameHex, int>(productionLogInfo2.Hexes);
				}
				else
				{
					foreach (GameHex gameHex in productionLogInfo2.Hexes.Keys)
					{
						if (!productionLogInfo.Hexes.ContainsKey(gameHex))
						{
							productionLogInfo.Hexes.Add(gameHex, productionLogInfo2.Hexes[gameHex]);
						}
						else
						{
							Dictionary<GameHex, int> hexes2 = productionLogInfo.Hexes;
							GameHex gameHex2 = gameHex;
							hexes2[gameHex2] += productionLogInfo2.Hexes[gameHex];
						}
					}
				}
			}
			if (productionLogInfo2.MillUsed)
			{
				productionLogInfo.MillUsed = true;
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000F4CA0 File Offset: 0x000F2EA0
		private void MergeMoveLogs(LogInfo logInfo)
		{
			HexUnitResourceLogInfo hexUnitResourceLogInfo = logInfo as HexUnitResourceLogInfo;
			HexUnitResourceLogInfo hexUnitResourceLogInfo2 = this.logInfoHistory[this.logInfoHistory.Count - 1] as HexUnitResourceLogInfo;
			hexUnitResourceLogInfo2.Hexes[1] = hexUnitResourceLogInfo.Hexes[1];
			for (int i = 1; i < hexUnitResourceLogInfo.Units.Count - 1; i++)
			{
				if (!hexUnitResourceLogInfo2.Units.Contains(hexUnitResourceLogInfo.Units[i]))
				{
					hexUnitResourceLogInfo2.Units.Add(hexUnitResourceLogInfo.Units[i]);
				}
			}
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000F4D34 File Offset: 0x000F2F34
		private void MergeAdditionalGain(LogInfo logInfo)
		{
			int num = ((this.gameManager.IsMultiplayer && this.logInfoHistory[this.logInfoHistory.Count - 1].Type == LogInfoType.GainStar && this.logInfoHistory.Count >= 3) ? (this.logInfoHistory.Count - 2) : (this.logInfoHistory.Count - 1));
			if (this.logInfoHistory[num].Type == logInfo.Type && logInfo is GainNonboardResourceLogInfo)
			{
				(this.logInfoHistory[num] as GainNonboardResourceLogInfo).Amount += (logInfo as GainNonboardResourceLogInfo).Amount;
				return;
			}
			this.logInfoHistory[num].PayLogInfos.AddRange(new List<LogInfo>(logInfo.PayLogInfos));
			if (logInfo.Type != LogInfoType.FactoryTopAction)
			{
				this.logInfoHistory[num].AdditionalGain.Add(logInfo);
			}
			this.logInfoHistory[num].AdditionalGain.AddRange(new List<LogInfo>(logInfo.AdditionalGain));
			logInfo.AdditionalGain.Clear();
			logInfo.PayLogInfos.Clear();
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000F4E60 File Offset: 0x000F3060
		public void CreateNewLog(LogInfo logInfo)
		{
			this.AddPayLogs(logInfo);
			this.logInfoHistory.Add(logInfo);
			this.lastTurn = this.gameManager.TurnCount;
			this.TrimLogsIfOnServerSide();
			if (ActionLog.LogInfoCreated != null)
			{
				ActionLog.LogInfoCreated(logInfo, this.logInfoHistory.Count - 1);
			}
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000441B1 File Offset: 0x000423B1
		private void TrimLogsIfOnServerSide()
		{
			if (this.gameManager.IsServer && this.logInfoHistory.Count > 20)
			{
				this.logInfoHistory.RemoveAt(0);
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000F4EB8 File Offset: 0x000F30B8
		private void AddPayLogs(LogInfo logInfo)
		{
			if (this.awaitingPayActions.Count > 0 && logInfo.ActionPlacement == this.awaitingPayActions[0].ActionPlacement)
			{
				if (logInfo.PayLogInfos == null)
				{
					logInfo.PayLogInfos = new List<LogInfo>(this.awaitingPayActions);
				}
				else
				{
					logInfo.PayLogInfos.AddRange(this.awaitingPayActions);
				}
				this.awaitingPayActions.Clear();
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000F4F24 File Offset: 0x000F3124
		public void FlushAwaitingPayActions()
		{
			if (this.awaitingPayActions.Count > 0 && !this.gameManager.IsMyTurn())
			{
				LogInfo logInfo = null;
				LogInfo logInfo2 = this.awaitingPayActions[0];
				Player playerByFaction = this.gameManager.GetPlayerByFaction(logInfo2.PlayerAssigned);
				if (logInfo2.ActionPlacement == ActionPositionType.Top)
				{
					TopActionType topActionType;
					if (playerByFaction.currentMatSection == -1)
					{
						topActionType = playerByFaction.matPlayer.GetPlayerMatSection(playerByFaction.lastMatSection).ActionTop.Type;
					}
					else
					{
						topActionType = playerByFaction.matPlayer.GetPlayerMatSection(playerByFaction.currentMatSection).ActionTop.Type;
					}
					logInfo = this.CreateEmptyTopActionLog(topActionType, logInfo2.PlayerAssigned);
				}
				else if (logInfo2.ActionPlacement == ActionPositionType.Down)
				{
					DownActionType downActionType;
					if (playerByFaction.currentMatSection == -1)
					{
						downActionType = playerByFaction.matPlayer.GetPlayerMatSection(playerByFaction.lastMatSection).ActionDown.Type;
					}
					else
					{
						downActionType = playerByFaction.matPlayer.GetPlayerMatSection(playerByFaction.currentMatSection).ActionDown.Type;
					}
					logInfo = this.CreateEmptyDownActionLog(downActionType, logInfo2.PlayerAssigned);
				}
				else if (logInfo2.ActionPlacement == ActionPositionType.Other)
				{
					logInfo = this.CreateEmptyEncounterActionLog(logInfo2.PlayerAssigned);
				}
				if (logInfo != null)
				{
					this.CreateNewLog(logInfo);
				}
				this.awaitingPayActions.Clear();
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000F5058 File Offset: 0x000F3258
		private LogInfo CreateEmptyTopActionLog(TopActionType type, Faction playerAssigned)
		{
			LogInfo logInfo = new LogInfo(this.gameManager);
			switch (type)
			{
			case TopActionType.Bolster:
				logInfo = new GainNonboardResourceLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.BolsterPower;
				break;
			case TopActionType.Trade:
				logInfo = new HexUnitResourceLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.TradeResources;
				break;
			case TopActionType.Produce:
				logInfo = new HexUnitResourceLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Produce;
				break;
			case TopActionType.MoveGain:
				logInfo = new HexUnitResourceLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Move;
				break;
			case TopActionType.Factory:
				logInfo = new FactoryLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.FactoryTopAction;
				break;
			}
			logInfo.ActionPlacement = ActionPositionType.Top;
			logInfo.PlayerAssigned = playerAssigned;
			return logInfo;
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000F5108 File Offset: 0x000F3308
		private LogInfo CreateEmptyDownActionLog(DownActionType type, Faction playerAssigned)
		{
			LogInfo logInfo = new LogInfo(this.gameManager);
			switch (type)
			{
			case DownActionType.Upgrade:
				logInfo = new UpgradeLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Upgrade;
				break;
			case DownActionType.Deploy:
				logInfo = new DeployLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Deploy;
				break;
			case DownActionType.Build:
				logInfo = new BuildLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Build;
				break;
			case DownActionType.Enlist:
				logInfo = new EnlistLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Enlist;
				break;
			case DownActionType.Factory:
				logInfo = new HexUnitResourceLogInfo(this.gameManager);
				logInfo.Type = LogInfoType.Move;
				break;
			}
			logInfo.ActionPlacement = ActionPositionType.Down;
			logInfo.PlayerAssigned = playerAssigned;
			return logInfo;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000441DB File Offset: 0x000423DB
		private LogInfo CreateEmptyEncounterActionLog(Faction playerAssigned)
		{
			return new GainNonboardResourceLogInfo(this.gameManager)
			{
				Gained = GainType.Coin,
				ActionPlacement = ActionPositionType.Other,
				IsEncounter = true,
				PlayerAssigned = playerAssigned
			};
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000F51B8 File Offset: 0x000F33B8
		public void ReadXml(XmlReader reader)
		{
			this.Clear();
			reader.ReadStartElement();
			int num;
			while (int.TryParse(reader.Name.Substring(1), out num))
			{
				LogInfo logInfo = null;
				switch (num)
				{
				case 0:
					logInfo = new PayResourceLogInfo(this.gameManager);
					break;
				case 1:
					logInfo = new PayNonboardResourceLogInfo(this.gameManager);
					break;
				case 2:
					logInfo = new HexUnitResourceLogInfo(this.gameManager);
					break;
				case 3:
					logInfo = new CombatLogInfo(this.gameManager);
					break;
				case 4:
					logInfo = new UpgradeLogInfo(this.gameManager);
					break;
				case 5:
					logInfo = new EnlistLogInfo(this.gameManager);
					break;
				case 6:
					logInfo = new BuildLogInfo(this.gameManager);
					break;
				case 7:
					logInfo = new DeployLogInfo(this.gameManager);
					break;
				case 8:
					logInfo = new ProductionLogInfo(this.gameManager);
					break;
				case 9:
					logInfo = new GainNonboardResourceLogInfo(this.gameManager);
					break;
				case 10:
					logInfo = new FactoryLogInfo(this.gameManager);
					break;
				case 11:
					logInfo = new WorkerLogInfo(this.gameManager);
					break;
				case 12:
					logInfo = new SneakPeakLogInfo(this.gameManager);
					break;
				case 13:
					logInfo = new StarLogInfo(this.gameManager);
					break;
				case 14:
					logInfo = new TokenActionLogInfo(this.gameManager);
					break;
				case 15:
					logInfo = new PassCoinLogInfo(this.gameManager);
					break;
				}
				((IXmlSerializable)logInfo).ReadXml(reader);
				this.logInfoHistory.Add(logInfo);
			}
			this.lastTurn = this.gameManager.TurnCount;
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000F5350 File Offset: 0x000F3550
		public void WriteXml(XmlWriter writer)
		{
			foreach (LogInfo logInfo in this.logInfoHistory)
			{
				((IXmlSerializable)logInfo).WriteXml(writer);
			}
		}

		// Token: 0x04001E53 RID: 7763
		private int lastTurn;

		// Token: 0x04001E54 RID: 7764
		private Queue<LogInfo> newestLogInfos = new Queue<LogInfo>();

		// Token: 0x04001E55 RID: 7765
		private List<LogInfo> logInfoHistory = new List<LogInfo>();

		// Token: 0x04001E56 RID: 7766
		private List<LogInfo> awaitingPayActions = new List<LogInfo>();

		// Token: 0x04001E57 RID: 7767
		private LogInfo workersStarLog;

		// Token: 0x04001E58 RID: 7768
		private bool multiplayerPayActionFinished;

		// Token: 0x04001E5A RID: 7770
		private GameManager gameManager;
	}
}
