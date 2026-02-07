using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020003EF RID: 1007
	public class HumanInputHandler : MonoBehaviour, IInputHandler
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x0003B8A8 File Offset: 0x00039AA8
		// (set) Token: 0x06001E30 RID: 7728 RVA: 0x0003B8AF File Offset: 0x00039AAF
		public static HumanInputHandler Instance { get; private set; }

		// Token: 0x06001E31 RID: 7729 RVA: 0x0003B8B7 File Offset: 0x00039AB7
		private void Awake()
		{
			HumanInputHandler.Instance = this;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0003B8BF File Offset: 0x00039ABF
		private void OnEnable()
		{
			ActionManager.InputForPayAction += this.EnableInputForPayAction;
			ActionManager.InputForGainAction += this.EnableInputForGainAction;
			ActionManager.BreakActionInput += this.OnBreakSectionAction;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0003B8F7 File Offset: 0x00039AF7
		private void OnDisable()
		{
			ActionManager.InputForPayAction -= this.EnableInputForPayAction;
			ActionManager.InputForGainAction -= this.EnableInputForGainAction;
			ActionManager.BreakActionInput -= this.OnBreakSectionAction;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000B9D2C File Offset: 0x000B7F2C
		public void EnableInputForPayAction(PayAction action)
		{
			if (!action.GetPlayer().IsHuman)
			{
				return;
			}
			PayType payType = action.GetPayType();
			if (payType == PayType.CombatCard)
			{
				this.ShowPresenter(this.payCombatCardPresenter, action);
				return;
			}
			if (payType != PayType.Resource)
			{
				return;
			}
			this.ShowPresenter(this.payDownActionPresenter, action);
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0003B92F File Offset: 0x00039B2F
		private void ShowPresenter(ActionPresenter presenter, BaseAction action)
		{
			this.selectedPresenter = presenter;
			presenter.gameObject.SetActive(true);
			presenter.ChangeLayoutForAction(action);
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000B9D74 File Offset: 0x000B7F74
		public void EnableInputForGainAction(GainAction action)
		{
			if (!action.GetPlayer().IsHuman)
			{
				return;
			}
			switch (action.GetGainType())
			{
			case GainType.Coin:
				this.ShowPresenter(this.simpleAmountPresenter, action);
				return;
			case GainType.Popularity:
				this.ShowPresenter(this.simpleAmountPresenter, action);
				return;
			case GainType.Power:
				this.ShowPresenter(this.simpleAmountPresenter, action);
				return;
			case GainType.CombatCard:
				this.ShowPresenter(this.simpleAmountPresenter, action);
				return;
			case GainType.Produce:
				this.ShowPresenter(this.producePresenter, action);
				return;
			case GainType.AnyResource:
				this.ShowPresenter(this.tradePresenter, action);
				return;
			case GainType.Resource:
				this.ShowPresenter(this.simpleAmountPresenter, action);
				return;
			case GainType.Move:
				this.ShowPresenter(this.movePresenter, action);
				return;
			case GainType.Upgrade:
				this.ShowPresenter(this.upgradePresenter, action);
				return;
			case GainType.Mech:
				this.ShowPresenter(this.deployPresenter, action);
				return;
			case GainType.Worker:
				this.ShowPresenter(this.gainWorkerPresenter, action);
				return;
			case GainType.Building:
				this.ShowPresenter(this.buildPresenter, action);
				return;
			case GainType.Recruit:
				this.ShowPresenter(this.enlistPresenter, action);
				return;
			case GainType.PeekCombatCards:
				this.ShowPresenter(this.peekCombatCardPresenter, action);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0003B94B File Offset: 0x00039B4B
		public ActionPresenter GetSelectedPresenter()
		{
			return this.selectedPresenter;
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0003B953 File Offset: 0x00039B53
		public void SetEndActionController(EndActionController endActionController)
		{
			this.endActionController = endActionController;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0003B95C File Offset: 0x00039B5C
		public void OnBreakSectionAction()
		{
			if (this.selectedPresenter != null)
			{
				this.selectedPresenter.OnActionEnded();
				this.selectedPresenter = null;
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000B9E98 File Offset: 0x000B8098
		public void OnInputEnded()
		{
			if (this.endActionController != null && this.selectedPresenter != this.payCombatCardPresenter && this.selectedPresenter != this.payDownActionPresenter)
			{
				this.endActionController.OnActionFinished();
			}
			if (this.selectedPresenter != null)
			{
				this.selectedPresenter = null;
			}
			GameController.GameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0003B97E File Offset: 0x00039B7E
		public void Clear()
		{
			this.enlistPresenter.Clear();
			this.deployPresenter.Clear();
			this.buildPresenter.Clear();
			this.upgradePresenter.Clear();
		}

		// Token: 0x04001579 RID: 5497
		public ActionPresenter simpleAmountPresenter;

		// Token: 0x0400157A RID: 5498
		public ActionPresenter movePresenter;

		// Token: 0x0400157B RID: 5499
		public ActionPresenter producePresenter;

		// Token: 0x0400157C RID: 5500
		public ActionPresenter tradePresenter;

		// Token: 0x0400157D RID: 5501
		public ActionPresenter payDownActionPresenter;

		// Token: 0x0400157E RID: 5502
		public ActionPresenter enlistPresenter;

		// Token: 0x0400157F RID: 5503
		public ActionPresenter deployPresenter;

		// Token: 0x04001580 RID: 5504
		public ActionPresenter buildPresenter;

		// Token: 0x04001581 RID: 5505
		public ActionPresenter upgradePresenter;

		// Token: 0x04001582 RID: 5506
		public ActionPresenter gainWorkerPresenter;

		// Token: 0x04001583 RID: 5507
		public ActionPresenter payCombatCardPresenter;

		// Token: 0x04001584 RID: 5508
		public ActionPresenter peekCombatCardPresenter;

		// Token: 0x04001585 RID: 5509
		private ActionPresenter selectedPresenter;

		// Token: 0x04001586 RID: 5510
		public EndActionController endActionController;
	}
}
