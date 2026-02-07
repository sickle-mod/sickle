using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000448 RID: 1096
	public class ArmTrapPanel : TokenPanelPresenter
	{
		// Token: 0x06002224 RID: 8740 RVA: 0x0003DCA7 File Offset: 0x0003BEA7
		public void ShowPanel(GameHexPresenter attachedGameHex, Unit initiator)
		{
			base.FinishPreviousAnimations(true);
			this.initiator = GameController.GetUnitPresenter(initiator);
			this.AttachArmTrapPanel(attachedGameHex);
			this.UpdateSprite();
			base.ShowPanelTriggerAnimation();
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x0003DCCF File Offset: 0x0003BECF
		public void AttachArmTrapPanel(GameHexPresenter attachedGameHex)
		{
			this.attachedGameHex = attachedGameHex;
			base.UpdatePosition();
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x0003DCDE File Offset: 0x0003BEDE
		public void Hide()
		{
			base.HidePanelTriggerAnimation();
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000CC65C File Offset: 0x000CA85C
		public void UpdateSprite()
		{
			if (this.attachedGameHex == null)
			{
				return;
			}
			if (!(this.attachedGameHex.GetGameHexLogic().Token is TrapToken))
			{
				return;
			}
			switch ((this.attachedGameHex.GetGameHexLogic().Token as TrapToken).Penalty)
			{
			case PayType.Coin:
				this.currentTrapImage.sprite = this.coinsTrap;
				return;
			case PayType.Popularity:
				this.currentTrapImage.sprite = this.popularityTrap;
				return;
			case PayType.Power:
				this.currentTrapImage.sprite = this.powerTrap;
				return;
			case PayType.CombatCard:
				this.currentTrapImage.sprite = this.cardsTrap;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x0003DCE6 File Offset: 0x0003BEE6
		protected override void OnShowTokenPanelComplete()
		{
			base.OnShowTokenPanelComplete();
			this.armTrapButton.interactable = true;
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0003DCFA File Offset: 0x0003BEFA
		protected override void PreHideTokenPanel()
		{
			this.armTrapButton.interactable = false;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x0003DD08 File Offset: 0x0003BF08
		public void TrapArmed()
		{
			this.tokensMenuController.ArmTrap(this.initiator.UnitLogic);
			if (PlatformManager.IsMobile)
			{
				base.HidePanelMobile(true);
				return;
			}
			this.Hide();
		}

		// Token: 0x040017C0 RID: 6080
		[SerializeField]
		private Image currentTrapImage;

		// Token: 0x040017C1 RID: 6081
		[SerializeField]
		private Button armTrapButton;

		// Token: 0x040017C2 RID: 6082
		[SerializeField]
		private Sprite coinsTrap;

		// Token: 0x040017C3 RID: 6083
		[SerializeField]
		private Sprite popularityTrap;

		// Token: 0x040017C4 RID: 6084
		[SerializeField]
		private Sprite powerTrap;

		// Token: 0x040017C5 RID: 6085
		[SerializeField]
		private Sprite cardsTrap;
	}
}
