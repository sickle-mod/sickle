using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x02000560 RID: 1376
	public class ActionManager : IXmlSerializable
	{
		// Token: 0x14000130 RID: 304
		// (add) Token: 0x06002C25 RID: 11301 RVA: 0x000F53A4 File Offset: 0x000F35A4
		// (remove) Token: 0x06002C26 RID: 11302 RVA: 0x000F53D8 File Offset: 0x000F35D8
		public static event ActionManager.EnableInputForPayAction InputForPayAction;

		// Token: 0x14000131 RID: 305
		// (add) Token: 0x06002C27 RID: 11303 RVA: 0x000F540C File Offset: 0x000F360C
		// (remove) Token: 0x06002C28 RID: 11304 RVA: 0x000F5440 File Offset: 0x000F3640
		public static event ActionManager.EnableInputForGainAction InputForGainAction;

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x06002C29 RID: 11305 RVA: 0x000F5474 File Offset: 0x000F3674
		// (remove) Token: 0x06002C2A RID: 11306 RVA: 0x000F54A8 File Offset: 0x000F36A8
		public static event ActionManager.OnBreakSectionAction BreakActionInput;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x06002C2B RID: 11307 RVA: 0x000F54DC File Offset: 0x000F36DC
		// (remove) Token: 0x06002C2C RID: 11308 RVA: 0x000F5514 File Offset: 0x000F3714
		public event ActionManager.ReportEndOfAction SectionActionFinished;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x06002C2D RID: 11309 RVA: 0x000F554C File Offset: 0x000F374C
		// (remove) Token: 0x06002C2E RID: 11310 RVA: 0x000F5580 File Offset: 0x000F3780
		public static event ActionManager.DisableActionInputAction DisableActionInput;

		// Token: 0x06002C2F RID: 11311 RVA: 0x00044204 File Offset: 0x00042404
		public ActionManager(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000F55B4 File Offset: 0x000F37B4
		public void SetSectionAction(SectionAction section, IActionProxy actionProxy, int gainActionId = -1)
		{
			if (this.gameManager.IsMultiplayer && ((this.gameManager.PlayerOwner == null && !this.gameManager.PlayerCurrent.IsHuman) || this.gameManager.IsMyTurn()))
			{
				int num = 2;
				if (section is TopAction)
				{
					num = 1;
				}
				else if (section is DownAction)
				{
					num = 0;
				}
				this.gameManager.OnActionSent(new ChooseActionMessage(this.gameManager.PlayerCurrent.matFaction.faction, num, gainActionId));
			}
			if (section is TopAction)
			{
				this.gameManager.PlayerCurrent.topActionInProgress = true;
				TopAction topAction = section as TopAction;
				if (topAction.Type == TopActionType.MoveGain)
				{
					if (gainActionId == 1)
					{
						this.CreateActionInfo(new string[] { "Move - get coins" });
					}
					else
					{
						this.CreateActionInfo(new string[] { "Move units" });
					}
				}
				else if (topAction.Type == TopActionType.Factory)
				{
					this.CreateActionInfo(new string[] { "Factory action" });
				}
				else if (section.CanPlayerPayActions())
				{
					this.CreateActionInfo(new string[] { topAction.Type.ToString() });
				}
			}
			else if (section is DownAction)
			{
				this.gameManager.PlayerCurrent.topActionInProgress = false;
				this.gameManager.PlayerCurrent.topActionFinished = true;
				this.gameManager.PlayerCurrent.bottomActionInProgress = true;
				this.gameManager.tokenManager.Clear();
				if (section.CanPlayerPayActions())
				{
					this.CreateActionInfo(new string[] { (section as DownAction).Type.ToString() });
				}
			}
			else
			{
				this.lastActionEncounter = true;
				this.gameManager.LastEncounterCard.ChooseOption();
				gainActionId = -1;
			}
			this.section = section;
			this.actionProxy = actionProxy;
			this.gainActionId = gainActionId;
			this.actionCanceled = false;
			this.actionIterator = 0;
			this.PrepareNextAction();
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x0004422C File Offset: 0x0004242C
		public void SetActionProxy(IActionProxy proxy)
		{
			if (this.actionProxy == null)
			{
				this.actionProxy = proxy;
			}
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x0004423D File Offset: 0x0004243D
		public bool SectionActionSelected()
		{
			return this.section != null;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000F57B0 File Offset: 0x000F39B0
		protected void CreateActionInfo(params string[] infoList)
		{
			if (infoList.Length == 0)
			{
				return;
			}
			string text = string.Concat(new string[]
			{
				"^",
				this.gameManager.PlayerCurrent.matFaction.faction.ToString()[0].ToString(),
				"[",
				infoList[0],
				"]"
			});
			this.gameManager.BroadcastActionInfo(text);
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000F582C File Offset: 0x000F3A2C
		public void PrepareNextAction()
		{
			if (this.actionCanceled)
			{
				if (this.lastSelectedGainAction is GainMove)
				{
					this.section.ExecuteGainActions();
					if (this.CheckIfCombatIsNeeded())
					{
						this.PrepareCombat();
						return;
					}
					this.lastSelectedGainAction = null;
				}
				return;
			}
			this.previousActionExecuted = true;
			if (this.gameManager.IsMultiplayer && this.section == null && this.lastSelectedGainAction != null)
			{
				this.gameManager.ChangeActivePlayer((int)this.gameManager.PlayerCurrent.matFaction.faction);
				GainNonboardResourceLogInfo gainNonboardResourceLogInfo = this.lastSelectedGainAction.GetLogInfo() as GainNonboardResourceLogInfo;
				if (gainNonboardResourceLogInfo.Amount != 0)
				{
					gainNonboardResourceLogInfo.ActionPlacement = ActionPositionType.OngoingRecruitBonus;
					this.gameManager.actionLog.LogInfoReported(gainNonboardResourceLogInfo);
				}
				this.lastSelectedGainAction.Execute();
				this.gameManager.CheckStars();
				this.Clear();
				this.gameManager.OnActionFinished();
				return;
			}
			this.gameManager.CheckStars();
			if (this.section != null)
			{
				if (!this.payActionsExecuted)
				{
					if (!this.section.ActionPayed())
					{
						this.HandlePayActions();
					}
					else
					{
						this.RunPayActions();
					}
				}
				if ((this.section.ActionPayed() || this.payActionsExecuted) && !this.gainActionsExecuted)
				{
					this.HandleGainActions();
					if (this.combatStarted)
					{
						return;
					}
				}
				if (this.section.ActionPayed() && this.gainActionsExecuted && this.previousActionExecuted)
				{
					this.HandleBonusActions();
				}
			}
			if (this.previousActionExecuted && this.bonusActionsExecuted)
			{
				this.EndSectionAction(false);
				this.gameManager.CheckStars();
				this.gameManager.CheckObjectiveCards();
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000F59C8 File Offset: 0x000F3BC8
		private void HandlePayActions()
		{
			if (!this.payActionsPrepared)
			{
				this.PrepareNextPayAction();
			}
			if (this.payActionsPrepared && this.previousActionExecuted)
			{
				if (!this.gameManager.IsMyTurn())
				{
					for (int i = 0; i < this.section.GetNumberOfPayActions(); i++)
					{
						if (!this.section.GetPayAction(i).Payed)
						{
							this.previousActionExecuted = false;
							return;
						}
					}
				}
				if (!this.RunPayActions())
				{
					this.gainActionsExecuted = true;
				}
				this.gameManager.OnUpdatePlayerStats();
			}
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000F5A4C File Offset: 0x000F3C4C
		public void PrepareNextPayAction()
		{
			while (this.actionIterator < this.section.GetNumberOfPayActions())
			{
				if (this.gameManager.IsMyTurn())
				{
					this.PrepareInputForPayAction(this.section.GetPayAction(this.actionIterator));
				}
				this.actionIterator++;
				if (this.actionIterator >= this.section.GetNumberOfPayActions())
				{
					this.payActionsPrepared = true;
				}
			}
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00044248 File Offset: 0x00042448
		private void PreparePreviousPayAction()
		{
			this.PrepareInputForPayAction(this.section.GetPayAction(this.actionIterator - 1));
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000F5ABC File Offset: 0x000F3CBC
		private void PrepareInputForPayAction(PayAction action)
		{
			PayType payType = action.GetPayType();
			if (payType - PayType.CombatCard <= 1)
			{
				this.previousActionExecuted = false;
				if (ActionManager.InputForPayAction != null && !this.gameManager.TestingMode)
				{
					ActionManager.InputForPayAction(action);
				}
			}
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x00044263 File Offset: 0x00042463
		private bool RunPayActions()
		{
			if (this.section.CanExecutePayActions())
			{
				this.section.ExecutePayActions();
				this.actionIterator = 0;
				this.payActionsExecuted = true;
				return true;
			}
			return false;
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000F5AFC File Offset: 0x000F3CFC
		private void HandleGainActions()
		{
			if (this.lastSelectedGainAction is GainMove && this.CheckIfCombatIsNeeded())
			{
				this.section.ExecuteGainActions();
				this.gainActionsExecuted = true;
				this.PrepareCombat();
				this.combatStarted = true;
				return;
			}
			if (this.moreActions)
			{
				this.PrepareNextGainAction();
			}
			if (!this.moreActions && this.previousActionExecuted && !this.gainActionsExecuted)
			{
				this.section.PrepareForActionCheckAndExecution();
				if (this.section.AllActionsSkipped())
				{
					this.SendSkipActionMessageForPlayerTurn();
				}
				else if (this.section.ActionsPrepared())
				{
					this.RunGainActions();
				}
				this.gainActionsExecuted = true;
			}
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000F5BA0 File Offset: 0x000F3DA0
		public void PrepareNextGainAction()
		{
			if (this.gainActionId > -1)
			{
				GainAction gainAction = this.section.GetGainAction(this.gainActionId);
				if (gainAction.GainAvaliable())
				{
					this.PrepareInputForGainAction(gainAction);
				}
				this.actionIterator++;
				this.moreActions = false;
				return;
			}
			if (this.AnyPreviousActionWasCanceled())
			{
				this.moreActions = false;
				return;
			}
			for (;;)
			{
				GainAction gainAction = this.section.GetGainAction(this.actionIterator);
				this.section.UpdateIfFactoryMove(this.actionIterator);
				this.actionIterator++;
				if (this.actionIterator >= this.section.GetNumberOfGainActions() || gainAction.GainAvaliable())
				{
					if (gainAction.GainAvaliable())
					{
						this.PrepareInputForGainAction(gainAction);
					}
					if (!gainAction.ActionSelected || this.actionIterator >= this.section.GetNumberOfGainActions())
					{
						break;
					}
				}
			}
			if (this.actionIterator >= this.section.GetNumberOfGainActions())
			{
				this.moreActions = false;
				return;
			}
			this.moreActions = true;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000F5C98 File Offset: 0x000F3E98
		private bool AnyPreviousActionWasCanceled()
		{
			for (int i = 0; i < this.actionIterator; i++)
			{
				GainAction gainAction = this.section.GetGainAction(i);
				if (!gainAction.ActionSelected && gainAction.GainAvaliable())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000F5CD8 File Offset: 0x000F3ED8
		public void PrepareInputForGainAction(GainAction action)
		{
			this.lastSelectedGainAction = action;
			if ((action.IsBonusAction && this.CanHandleOngoingBonusAutomatically(action)) || this.CanHandleActionAutomatically(action))
			{
				this.HandleAutomaticGain(action);
				this.previousActionExecuted = true;
				return;
			}
			this.previousActionExecuted = false;
			if (action is GainMove)
			{
				this.gameManager.moveManager.SetMoveAction((GainMove)action);
			}
			if (ActionManager.InputForGainAction != null && !this.gameManager.TestingMode && !this.gameManager.SpectatorMode && (!this.gameManager.IsMultiplayer || action.GetPlayer() == this.gameManager.PlayerOwner || (!action.GetPlayer().IsHuman && this.gameManager.PlayerOwner == null)))
			{
				ActionManager.InputForGainAction(action);
			}
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x0004428E File Offset: 0x0004248E
		private void SendSkipActionMessageForPlayerTurn()
		{
			if (this.gameManager.IsMultiplayer && this.gameManager.IsMyTurn())
			{
				this.gameManager.OnActionSent(new SkipActionMessage());
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000F5DA0 File Offset: 0x000F3FA0
		private bool CanHandleActionAutomatically(GainAction bonus)
		{
			return !this.gameManager.SpectatorMode && (!this.gameManager.IsMultiplayer || bonus.GetPlayer() == this.gameManager.PlayerOwner || (!bonus.GetPlayer().IsHuman && this.gameManager.PlayerOwner == null)) && this.BonusExistsOnTheAutomaticGainList(bonus) && (this.PlayerAutomaticlyGainsBonus(bonus) || !bonus.GetPlayer().IsHuman || this.HackForHotseat(bonus));
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000442BA File Offset: 0x000424BA
		private bool CanHandleOngoingBonusAutomatically(GainAction bonus)
		{
			return this.BonusExistsOnTheAutomaticGainList(bonus) && (this.PlayerAutomaticlyGainsBonus(bonus) || this.gameManager.IsMultiplayer || this.HackForHotseat(bonus));
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000442E7 File Offset: 0x000424E7
		private bool BonusExistsOnTheAutomaticGainList(GainAction bonus)
		{
			return bonus.GetPlayer().automaticGain.ContainsKey(bonus.GetGainType());
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000442FF File Offset: 0x000424FF
		private bool PlayerAutomaticlyGainsBonus(GainAction bonus)
		{
			return bonus.GetPlayer().automaticGain[bonus.GetGainType()] || this.gameManager.GameFinished;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x00044326 File Offset: 0x00042526
		private bool HackForHotseat(GainAction bonus)
		{
			return bonus.GetPlayer() != this.gameManager.PlayerCurrent && this.GameIsLocal();
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00044343 File Offset: 0x00042543
		private bool GameIsLocal()
		{
			return this.gameManager.IsHotSeat || this.gameManager.IsAIHotSeat || this.gameManager.IsCampaign;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0004436C File Offset: 0x0004256C
		private void PreparePreviousGainAction()
		{
			if (this.lastSelectedGainAction == null)
			{
				return;
			}
			this.section.OverrideGainAction(this.actionIterator - 1, this.lastSelectedGainAction);
			this.PrepareInputForGainAction(this.lastSelectedGainAction);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x0004439C File Offset: 0x0004259C
		public void RunGainActions()
		{
			this.section.ExecuteGainActions();
			this.actionIterator = 0;
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000443B0 File Offset: 0x000425B0
		private void HandleBonusActions()
		{
			if (this.section is TopAction)
			{
				this.PrepareBuildingBonus();
				return;
			}
			if (this.section is DownAction)
			{
				this.PrepareRecruitBonus();
				return;
			}
			this.bonusActionsExecuted = true;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000F5E20 File Offset: 0x000F4020
		private void PrepareRecruitBonus()
		{
			DownAction downAction = this.section as DownAction;
			if (this.neighbours.Count == 0)
			{
				if (downAction.Type == DownActionType.Factory)
				{
					this.bonusActionsExecuted = true;
					return;
				}
				if (downAction.OldRecruitValue && downAction.IsRecruitEnlisted)
				{
					this.neighbours.Add(this.gameManager.PlayerCurrent);
				}
				List<Player> playerNeighbours = this.gameManager.GetPlayerNeighbours(this.gameManager.PlayerCurrent);
				if (!this.gameManager.GameFinished)
				{
					foreach (Player player in playerNeighbours)
					{
						if (player.matPlayer.GetDownAction(downAction.Type).IsRecruitEnlisted)
						{
							this.neighbours.Add(player);
						}
					}
				}
				if (this.neighbours.Count == 0)
				{
					this.bonusActionsExecuted = true;
					return;
				}
				this.nextPlayer = 0;
				this.bonusActionsExecuted = false;
			}
			do
			{
				if (this.lastBonusAction != null && this.lastBonusAction.ActionSelected)
				{
					LogInfo logInfo = this.lastBonusAction.GetLogInfo();
					logInfo.ActionPlacement = ActionPositionType.OngoingRecruitBonus;
					this.gameManager.actionLog.LogInfoReported(logInfo);
					this.lastBonusAction.Execute();
				}
				if (this.nextPlayer < this.neighbours.Count)
				{
					if (!this.gameManager.GameFinished)
					{
						this.GainRecruitBonus(this.neighbours[this.nextPlayer]);
					}
					else if (this.neighbours[this.nextPlayer] == this.gameManager.PlayerCurrent)
					{
						this.GainRecruitBonus(this.neighbours[this.nextPlayer]);
					}
					this.nextPlayer++;
				}
				else
				{
					this.gameManager.EnemysEnlistBonusEnd();
					this.bonusActionsExecuted = true;
				}
			}
			while (!this.bonusActionsExecuted && ((this.lastBonusAction != null && this.lastBonusAction.ActionSelected) || this.lastBonusAction == null));
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000F6028 File Offset: 0x000F4228
		public void GainRecruitBonus(Player player)
		{
			DownAction downAction = player.matPlayer.GetDownAction((this.section as DownAction).Type);
			this.lastBonusAction = null;
			this.PrepareEnlistBonusEnemyActionInfo(downAction, player);
			switch (downAction.Type)
			{
			case DownActionType.Upgrade:
				this.lastBonusAction = new GainPower(this.gameManager, 1, 0, false, false, true);
				this.lastBonusAction.SetPlayer(player);
				break;
			case DownActionType.Deploy:
				this.lastBonusAction = new GainCoin(this.gameManager, 1, 0, false, false, true);
				this.lastBonusAction.SetPlayer(player);
				break;
			case DownActionType.Build:
				this.lastBonusAction = new GainPopularity(this.gameManager, 1, 0, false, false, true);
				this.lastBonusAction.SetPlayer(player);
				break;
			case DownActionType.Enlist:
				this.lastBonusAction = new GainCombatCard(this.gameManager, 1, 0, false, false, true);
				this.lastBonusAction.SetPlayer(player);
				break;
			}
			if (this.lastBonusAction != null)
			{
				this.PrepareInputForGainAction(this.lastBonusAction);
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000F6124 File Offset: 0x000F4324
		private void PrepareEnlistBonusEnemyActionInfo(DownAction action, Player player)
		{
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				if (action.Type == DownActionType.Enlist && this.gameManager.CombatCardsLeft() == 0)
				{
					return;
				}
				EnlistBonusEnemyActionInfo enlistBonusEnemyActionInfo = new EnlistBonusEnemyActionInfo();
				enlistBonusEnemyActionInfo.actionType = LogInfoType.RecruitBonus;
				enlistBonusEnemyActionInfo.actionOwner = player.matFaction.faction;
				switch (action.Type)
				{
				case DownActionType.Upgrade:
					enlistBonusEnemyActionInfo.oneTimeBonus = GainType.Power;
					break;
				case DownActionType.Deploy:
					enlistBonusEnemyActionInfo.oneTimeBonus = GainType.Coin;
					break;
				case DownActionType.Build:
					enlistBonusEnemyActionInfo.oneTimeBonus = GainType.Popularity;
					break;
				case DownActionType.Enlist:
					enlistBonusEnemyActionInfo.oneTimeBonus = GainType.CombatCard;
					break;
				}
				this.gameManager.EnemyEnlistBonus(enlistBonusEnemyActionInfo);
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000F6200 File Offset: 0x000F4400
		private void PrepareBuildingBonus()
		{
			TopAction topAction = this.section as TopAction;
			if (this.lastBonusAction == null && topAction.Structure != null && topAction.Structure.IsOnMap())
			{
				this.lastBonusAction = topAction.Structure.GetBonus(this.gameManager.PlayerCurrent);
			}
			if (this.lastBonusAction != null)
			{
				if (!this.lastBonusAction.ActionSelected)
				{
					this.bonusActionsExecuted = false;
					this.PrepareInputForGainAction(this.lastBonusAction);
				}
				if (this.lastBonusAction.ActionSelected)
				{
					LogInfo logInfo = this.lastBonusAction.GetLogInfo();
					logInfo.ActionPlacement = ActionPositionType.BuildingBonus;
					this.gameManager.actionLog.LogInfoReported(logInfo);
					this.bonusActionsExecuted = true;
					this.lastBonusAction.Execute();
					return;
				}
			}
			else
			{
				this.bonusActionsExecuted = true;
			}
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000F62C8 File Offset: 0x000F44C8
		public void HandleAutomaticGain(GainAction action)
		{
			switch (action.GetGainType())
			{
			case GainType.Coin:
				((GainCoin)action).SetCoins(action.Amount);
				return;
			case GainType.Popularity:
				((GainPopularity)action).SetPopularity(action.Amount);
				return;
			case GainType.Power:
				((GainPower)action).SetPower(action.Amount);
				return;
			case GainType.CombatCard:
				((GainCombatCard)action).SetCards(action.Amount);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000443E1 File Offset: 0x000425E1
		private bool CheckIfCombatIsNeeded()
		{
			return this.gameManager.combatManager.IsPlayerInCombat();
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000443F3 File Offset: 0x000425F3
		private void PrepareCombat()
		{
			this.gameManager.combatManager.OnCombatStageChanged += this.OnCombatResolved;
			this.gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000F6340 File Offset: 0x000F4540
		private void OnCombatResolved(CombatStage stage)
		{
			if (stage == CombatStage.CombatResovled)
			{
				this.gameManager.combatManager.OnCombatStageChanged -= this.OnCombatResolved;
				if (this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)
				{
					return;
				}
				this.gainActionsExecuted = true;
				this.PrepareNextAction();
			}
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00044421 File Offset: 0x00042621
		public void BreakSectionAction(bool onLoad = false)
		{
			this.actionCanceled = true;
			if (ActionManager.BreakActionInput != null)
			{
				ActionManager.BreakActionInput();
			}
			this.EndSectionAction(onLoad);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x000F639C File Offset: 0x000F459C
		private void EndSectionAction(bool onLoad = false)
		{
			if (this.section != null && this.gameManager.PlayerCurrent != null)
			{
				if (this.section.ActionInterrupted() && !onLoad)
				{
					this.section.ReportUnfinishedAction();
				}
				bool flag = false;
				bool flag2 = false;
				if (this.gameManager.PlayerCurrent.topActionInProgress)
				{
					this.gameManager.PlayerCurrent.topActionFinished = true;
					this.gameManager.PlayerCurrent.topActionInProgress = false;
				}
				else if (this.lastActionEncounter)
				{
					flag = true;
					EncounterCard lastEncounterCard = this.gameManager.LastEncounterCard;
					if (lastEncounterCard != null)
					{
						lastEncounterCard.FinishAction();
						if (lastEncounterCard.AmountChoosen >= 2 || this.gameManager.PlayerCurrent.matFaction.factionPerk != AbilityPerk.Meander)
						{
							this.gameManager.PlayerCurrent.character.position.encounterTaken = true;
							this.gameManager.ClearLastEncounterCard();
							flag2 = true;
						}
					}
				}
				else if (this.gameManager.PlayerCurrent.bottomActionInProgress)
				{
					this.gameManager.PlayerCurrent.bottomActionInProgress = false;
					this.gameManager.PlayerCurrent.downActionFinished = true;
				}
				if (!this.gameManager.IsMultiplayer && !onLoad)
				{
					bool flag3 = this.lastActionEncounter;
					this.Clear();
					if (this.SectionActionFinished != null && (!flag || (flag && flag2)))
					{
						this.SectionActionFinished();
					}
					if (this.section != null && !this.gameManager.GameLoading)
					{
						this.section.ClearActions();
					}
				}
				if ((!flag || (flag && flag2)) && ActionManager.DisableActionInput != null)
				{
					ActionManager.DisableActionInput();
				}
			}
			else if (this.section != null)
			{
				Player playerCurrent = this.gameManager.PlayerCurrent;
			}
			this.Clear();
			if (this.actionProxy != null)
			{
				this.actionProxy.SectionActionFinished();
			}
			this.actionProxy = null;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x000F6564 File Offset: 0x000F4764
		public void Clear()
		{
			this.gameManager.combatManager.OnCombatStageChanged -= this.OnCombatResolved;
			if (this.section != null && !this.gameManager.GameLoading)
			{
				this.section.ClearActions();
			}
			if (this.lastSelectedGainAction is GainMove)
			{
				this.gameManager.moveManager.Clear();
			}
			this.section = null;
			this.lastSelectedGainAction = null;
			this.lastBonusAction = null;
			this.actionIterator = 0;
			this.gainActionId = -1;
			this.lastActionEncounter = false;
			this.payActionsPrepared = false;
			this.gainActionsExecuted = false;
			this.bonusActionsExecuted = false;
			this.nextPlayer = 0;
			this.moreActions = true;
			this.previousActionExecuted = false;
			this.actionCanceled = false;
			this.payActionsExecuted = false;
			this.combatStarted = false;
			this.neighbours.Clear();
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000F6640 File Offset: 0x000F4840
		public bool SetPayAction(PayAction action)
		{
			if (this.section == null)
			{
				return false;
			}
			int numberOfPayActions = this.section.GetNumberOfPayActions();
			bool flag = false;
			int i;
			for (i = 0; i < numberOfPayActions; i++)
			{
				if (this.section.GetPayAction(i).GetPayType() == action.GetPayType())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (action.GetPlayer() == null)
			{
				return false;
			}
			if (this.section.GetPayAction(i).GetPlayer() != action.GetPlayer())
			{
				return false;
			}
			if (!action.CanPlayerPay() || !action.CanExecute())
			{
				return false;
			}
			if (this.section.GetPayAction(i).Payed)
			{
				return false;
			}
			this.section.OverridePayAction(i, action);
			return true;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000F66F0 File Offset: 0x000F48F0
		public bool SetGainAction(GainAction action)
		{
			if (this.section == null)
			{
				return false;
			}
			if (this.lastBonusAction != null && this.lastBonusAction.GetGainType() == action.GetGainType())
			{
				return this.SetBonusAction(action);
			}
			if (this.section.GetGainAction(0).GetGainType() == GainType.Recruit && action.GetGainType() == GainType.CombatCard)
			{
				return true;
			}
			int numberOfGainActions = this.section.GetNumberOfGainActions();
			bool flag = false;
			int i = 0;
			while (i < numberOfGainActions)
			{
				if (this.section.GetGainAction(i).GetGainType() == action.GetGainType() && !this.section.GetGainAction(i).ActionSelected)
				{
					if (this.section.GetGainAction(i).Gained)
					{
						return false;
					}
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (action.GetPlayer() == null)
			{
				return false;
			}
			if (this.section.GetGainAction(i).GetPlayer().matFaction.faction != action.GetPlayer().matFaction.faction)
			{
				return false;
			}
			if (!action.CanExecute())
			{
				return false;
			}
			if (this.gainActionsExecuted)
			{
				return false;
			}
			this.section.OverrideGainAction(i, action);
			return true;
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x000F680C File Offset: 0x000F4A0C
		public bool SetBonusAction(GainAction action)
		{
			if (this.lastBonusAction == null)
			{
				return false;
			}
			if (this.lastBonusAction.GetGainType() != action.GetGainType())
			{
				return false;
			}
			if (action.Amount > this.lastBonusAction.Amount)
			{
				return false;
			}
			if (action.GetPlayer().matFaction.faction != this.lastBonusAction.GetPlayer().matFaction.faction)
			{
				return false;
			}
			if (!action.CanExecute())
			{
				return false;
			}
			this.lastBonusAction = action;
			return true;
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000F6888 File Offset: 0x000F4A88
		public void PreparePreviousAction()
		{
			if (this.PayActionInProgress())
			{
				this.PreparePreviousPayAction();
			}
			else if (this.GainActionInProgress())
			{
				this.PreparePreviousGainAction();
			}
			if (this.gameManager.combatManager.GetBattlefields().Count > 0)
			{
				this.gameManager.combatManager.OnCombatStageChanged += this.OnCombatResolved;
			}
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00044442 File Offset: 0x00042642
		private bool PayActionInProgress()
		{
			return this.section != null && !this.payActionsExecuted && !this.section.ActionPayed();
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x00044466 File Offset: 0x00042666
		private bool GainActionInProgress()
		{
			return !this.gainActionsExecuted && this.section != null;
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000F68E8 File Offset: 0x000F4AE8
		public PayAction GetLastPayAction()
		{
			if (this.section == null)
			{
				return null;
			}
			for (int i = 0; i < this.section.GetNumberOfPayActions(); i++)
			{
				if (!this.section.GetPayAction(i).Payed)
				{
					return this.section.GetPayAction(i);
				}
			}
			return null;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0004447B File Offset: 0x0004267B
		public GainAction GetLastBonusAction()
		{
			return this.lastBonusAction;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x00044483 File Offset: 0x00042683
		public GainAction GetLastSelectedGainAction()
		{
			return this.lastSelectedGainAction;
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x0004448B File Offset: 0x0004268B
		public int GetGainActionId()
		{
			return this.gainActionId;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000F6938 File Offset: 0x000F4B38
		public bool MoreHumanPlayersToGetBonus()
		{
			for (int i = this.nextPlayer; i < this.neighbours.Count; i++)
			{
				if (this.neighbours[i].IsHuman)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000F6978 File Offset: 0x000F4B78
		public void ReadXml(XmlReader reader)
		{
			this.Clear();
			reader.MoveToContent();
			if (reader.GetAttribute("NotUsed") != null)
			{
				return;
			}
			this.gameManager.combatManager.OnCombatStageChanged -= this.OnCombatResolved;
			if (reader.GetAttribute("ActionId") != null)
			{
				this.gainActionId = int.Parse(reader.GetAttribute("ActionId"));
			}
			if (reader.GetAttribute("ActionIter") != null)
			{
				this.actionIterator = int.Parse(reader.GetAttribute("ActionIter"));
			}
			if (reader.GetAttribute("NextPlayer") != null)
			{
				this.nextPlayer = int.Parse(reader.GetAttribute("NextPlayer"));
			}
			if (reader.GetAttribute("ActionPrepared") != null)
			{
				this.payActionsPrepared = true;
			}
			if (reader.GetAttribute("GainExecuted") != null)
			{
				this.gainActionsExecuted = true;
			}
			if (reader.GetAttribute("BonusExecuted") != null)
			{
				this.bonusActionsExecuted = true;
			}
			if (reader.GetAttribute("NoMoreAction") != null)
			{
				this.moreActions = false;
			}
			if (reader.GetAttribute("ActionExecuted") != null)
			{
				this.previousActionExecuted = true;
			}
			if (reader.GetAttribute("ActionCanceled") != null)
			{
				this.actionCanceled = true;
			}
			if (reader.GetAttribute("PayActionsExecuted") != null)
			{
				this.payActionsExecuted = true;
			}
			if (reader.GetAttribute("LastEncounter") != null)
			{
				this.lastActionEncounter = true;
			}
			if (this.gameManager.combatManager.GetBattlefields().Count > 0 && (this.actionCanceled || this.gainActionsExecuted))
			{
				this.gameManager.combatManager.OnCombatStageChanged += this.OnCombatResolved;
			}
			if (this.gameManager.PlayerCurrent.topActionInProgress)
			{
				this.section = this.gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.gameManager.PlayerCurrent.currentMatSection).ActionTop;
			}
			else if (this.gameManager.LastEncounterCard != null && this.lastActionEncounter)
			{
				this.section = this.gameManager.LastEncounterCard.GetCurrentAction();
			}
			else if (this.gameManager.PlayerCurrent.bottomActionInProgress)
			{
				this.section = this.gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.gameManager.PlayerCurrent.currentMatSection).ActionDown;
			}
			if (reader.GetAttribute("Neighbours") != null)
			{
				foreach (string text in reader.GetAttribute("Neighbours").Split(' ', StringSplitOptions.None))
				{
					if (text == "")
					{
						break;
					}
					this.neighbours.Add(this.gameManager.GetPlayerByFaction((Faction)int.Parse(text)));
				}
			}
			reader.ReadStartElement();
			if (reader.Name == "GainBonus")
			{
				this.ReadAction(ref this.lastBonusAction, reader);
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
				else
				{
					reader.ReadStartElement();
				}
			}
			if (reader.Name == "GainAction")
			{
				this.ReadAction(ref this.lastSelectedGainAction, reader);
				if (this.lastBonusAction == null && this.lastSelectedGainAction != null)
				{
					if (this.gainActionId != -1)
					{
						this.section.OverrideGainAction(this.gainActionId, this.lastSelectedGainAction);
					}
					else if (this.actionIterator == 0)
					{
						this.section.OverrideGainAction(0, this.lastSelectedGainAction);
					}
					else
					{
						this.section.OverrideGainAction(this.actionIterator - 1, this.lastSelectedGainAction);
					}
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
					return;
				}
				reader.ReadStartElement();
			}
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000F6CF8 File Offset: 0x000F4EF8
		public void WriteXml(XmlWriter writer)
		{
			if (this.section == null)
			{
				writer.WriteAttributeString("NotUsed", "");
				return;
			}
			if (this.gainActionId != -1)
			{
				writer.WriteAttributeString("ActionId", this.gainActionId.ToString());
			}
			if (this.actionIterator != 0)
			{
				writer.WriteAttributeString("ActionIter", this.actionIterator.ToString());
			}
			if (this.nextPlayer != 0)
			{
				writer.WriteAttributeString("NextPlayer", this.nextPlayer.ToString());
			}
			if (this.payActionsPrepared)
			{
				writer.WriteAttributeString("ActionPrepared", "");
			}
			if (this.gainActionsExecuted)
			{
				writer.WriteAttributeString("GainExecuted", "");
			}
			if (this.bonusActionsExecuted)
			{
				writer.WriteAttributeString("BonusExecuted", "");
			}
			if (!this.moreActions)
			{
				writer.WriteAttributeString("NoMoreAction", "");
			}
			if (this.previousActionExecuted)
			{
				writer.WriteAttributeString("ActionExecuted", "");
			}
			if (this.actionCanceled)
			{
				writer.WriteAttributeString("ActionCanceled", "");
			}
			if (this.payActionsExecuted)
			{
				writer.WriteAttributeString("PayActionsExecuted", "");
			}
			if (this.lastActionEncounter)
			{
				writer.WriteAttributeString("LastEncounter", "");
			}
			if (this.neighbours.Count != 0)
			{
				string text = "";
				foreach (Player player in this.neighbours)
				{
					string text2 = text;
					int faction = (int)player.matFaction.faction;
					text = text2 + faction.ToString() + " ";
				}
				writer.WriteAttributeString("Neighbours", text);
			}
			if (this.lastBonusAction != null)
			{
				writer.WriteStartElement("GainBonus");
				this.WriteAction(this.lastBonusAction, writer);
				writer.WriteEndElement();
			}
			if (this.lastSelectedGainAction != null)
			{
				writer.WriteStartElement("GainAction");
				this.WriteAction(this.lastSelectedGainAction, writer);
				writer.WriteEndElement();
			}
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000F6F04 File Offset: 0x000F5104
		private void ReadAction(ref GainAction action, XmlReader reader)
		{
			string attribute = reader.GetAttribute("GainType");
			if (string.IsNullOrEmpty(attribute))
			{
				return;
			}
			switch (int.Parse(attribute))
			{
			case 0:
				action = new GainCoin(this.gameManager);
				break;
			case 1:
				action = new GainPopularity(this.gameManager);
				break;
			case 2:
				action = new GainPower(this.gameManager);
				break;
			case 3:
				action = new GainCombatCard(this.gameManager);
				break;
			case 4:
				action = new GainProduce(this.gameManager);
				break;
			case 5:
				action = new GainAnyResource(this.gameManager);
				break;
			case 6:
				action = new GainResource(this.gameManager);
				break;
			case 7:
				action = new GainMove(this.gameManager);
				break;
			case 8:
				action = new GainUpgrade(this.gameManager);
				break;
			case 9:
				action = new GainMech(this.gameManager);
				break;
			case 10:
				action = new GainWorker(this.gameManager);
				break;
			case 11:
				action = new GainBuilding(this.gameManager);
				break;
			case 12:
				action = new GainRecruit(this.gameManager);
				break;
			case 13:
				action = new GainPeekCombatCards(this.gameManager, 0, 0, false);
				break;
			}
			((IXmlSerializable)action).ReadXml(reader);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000F705C File Offset: 0x000F525C
		private void WriteAction(GainAction action, XmlWriter writer)
		{
			writer.WriteAttributeString("GainType", ((int)action.GetGainType()).ToString());
			((IXmlSerializable)action).WriteXml(writer);
		}

		// Token: 0x04001E60 RID: 7776
		private IActionProxy actionProxy;

		// Token: 0x04001E61 RID: 7777
		private SectionAction section;

		// Token: 0x04001E62 RID: 7778
		private int gainActionId = -1;

		// Token: 0x04001E63 RID: 7779
		private int actionIterator;

		// Token: 0x04001E64 RID: 7780
		private bool payActionsPrepared;

		// Token: 0x04001E65 RID: 7781
		private bool gainActionsExecuted;

		// Token: 0x04001E66 RID: 7782
		private bool bonusActionsExecuted;

		// Token: 0x04001E67 RID: 7783
		private bool moreActions = true;

		// Token: 0x04001E68 RID: 7784
		private bool previousActionExecuted;

		// Token: 0x04001E69 RID: 7785
		private bool actionCanceled;

		// Token: 0x04001E6A RID: 7786
		private List<Player> neighbours = new List<Player>();

		// Token: 0x04001E6B RID: 7787
		private int nextPlayer;

		// Token: 0x04001E6C RID: 7788
		private GainAction lastBonusAction;

		// Token: 0x04001E6D RID: 7789
		private GainAction lastSelectedGainAction;

		// Token: 0x04001E6E RID: 7790
		private bool payActionsExecuted;

		// Token: 0x04001E6F RID: 7791
		private bool lastActionEncounter;

		// Token: 0x04001E70 RID: 7792
		private bool combatStarted;

		// Token: 0x04001E71 RID: 7793
		private GameManager gameManager;

		// Token: 0x02000561 RID: 1377
		// (Invoke) Token: 0x06002C64 RID: 11364
		public delegate void EnableInputForPayAction(PayAction action);

		// Token: 0x02000562 RID: 1378
		// (Invoke) Token: 0x06002C68 RID: 11368
		public delegate void EnableInputForGainAction(GainAction action);

		// Token: 0x02000563 RID: 1379
		// (Invoke) Token: 0x06002C6C RID: 11372
		public delegate void OnBreakSectionAction();

		// Token: 0x02000564 RID: 1380
		// (Invoke) Token: 0x06002C70 RID: 11376
		public delegate void ReportEndOfAction();

		// Token: 0x02000565 RID: 1381
		// (Invoke) Token: 0x06002C74 RID: 11380
		public delegate void DisableActionInputAction();
	}
}
