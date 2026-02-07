using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;

namespace Scythe.UI
{
	// Token: 0x020003F6 RID: 1014
	public class PayCombatCardPresenter : ActionPresenter
	{
		// Token: 0x06001EB5 RID: 7861 RVA: 0x0003BE42 File Offset: 0x0003A042
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as PayCombatCard;
			this.combatCards.Clear();
			if (PlatformManager.IsMobile)
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
			}
			this.EnableInput();
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000BC988 File Offset: 0x000BAB88
		private void EnableInput()
		{
			if (!PlatformManager.IsStandalone)
			{
				GameController.Instance.matFaction.gameObject.SetActive(true);
				GameController.Instance.matFaction.ClearHintStories();
			}
			GameController.Instance.matFaction.combatCardsPresenter.FocusCards(true);
			GameController.Instance.matFaction.combatCardsPresenter.PayCardMode();
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0003BE78 File Offset: 0x0003A078
		public void OnCardPressed(CombatCard cc)
		{
			this.combatCards.Add(cc);
			this.CheckActionPayed();
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000BC9EC File Offset: 0x000BABEC
		private void CheckActionPayed()
		{
			if ((int)this.action.Amount == this.combatCards.Count)
			{
				List<CombatCard> list = new List<CombatCard>();
				foreach (CombatCard combatCard in this.combatCards)
				{
					list.Add(combatCard);
				}
				this.action.SetCombatCards(list);
				this.OnActionEnded();
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0003BE8C File Offset: 0x0003A08C
		private void DisableInput()
		{
			GameController.Instance.matFaction.combatCardsPresenter.FocusCards(false);
			if (!PlatformManager.IsStandalone)
			{
				GameController.Instance.matFaction.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0003BEBF File Offset: 0x0003A0BF
		public override void OnActionEnded()
		{
			this.DisableInput();
			HumanInputHandler.Instance.OnInputEnded();
			base.gameObject.SetActive(false);
		}

		// Token: 0x040015B9 RID: 5561
		private PayCombatCard action;

		// Token: 0x040015BA RID: 5562
		private List<CombatCard> combatCards = new List<CombatCard>();
	}
}
