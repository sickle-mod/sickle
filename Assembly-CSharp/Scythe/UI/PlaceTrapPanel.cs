using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200044A RID: 1098
	public class PlaceTrapPanel : TokenPanelPresenter
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x000CC79C File Offset: 0x000CA99C
		protected override void OnEnable()
		{
			base.OnEnable();
			if (GameController.GameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Togawa))
			{
				foreach (FactionAbilityToken factionAbilityToken in GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetPlacedTokens())
				{
					TrapToken trapToken = (TrapToken)factionAbilityToken;
					this.DisableTrapButton(trapToken.Penalty);
				}
			}
			this.UpdateNumberOfTokens();
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000CC840 File Offset: 0x000CAA40
		private void UpdateNumberOfTokens()
		{
			this.tokenLeftAmount.text = GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetUnplacedTokensCount().ToString();
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0003DDFC File Offset: 0x0003BFFC
		public void ShowTrapPanelIcon(GameHexPresenter attachedGameHex, Unit initiator)
		{
			if (this.attachedGameHex == attachedGameHex)
			{
				return;
			}
			base.FinishPreviousAnimations(true);
			this.initiator = GameController.GetUnitPresenter(initiator);
			this.AttachTrapsPanel(attachedGameHex);
			base.ShowPanelTriggerAnimation();
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x0003DCCF File Offset: 0x0003BECF
		public void AttachTrapsPanel(GameHexPresenter attachedGameHex)
		{
			this.attachedGameHex = attachedGameHex;
			base.UpdatePosition();
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000CC87C File Offset: 0x000CAA7C
		private void EnableButtons()
		{
			TokenSupply factionTokens = base.tokenManager.GetLastMovedUnit().Owner.matFaction.FactionTokens;
			for (int i = 0; i < factionTokens.GetTokensCount(); i++)
			{
				this.buttons.GetChild(i).GetComponent<Button>().interactable = !factionTokens.GetToken(i).IsTokenPlaced();
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000CC8DC File Offset: 0x000CAADC
		private void DisableButtons()
		{
			for (int i = 0; i < this.buttons.childCount; i++)
			{
				this.buttons.GetChild(i).GetComponent<Button>().interactable = false;
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000CC918 File Offset: 0x000CAB18
		private void DisableTrapButton(PayType penalty)
		{
			switch (penalty)
			{
			case PayType.Coin:
				this.coinTrap.interactable = false;
				this.coinTrapImage.color = new Color(this.coinTrapImage.color.r, this.coinTrapImage.color.g, this.coinTrapImage.color.b, 0.3f);
				return;
			case PayType.Popularity:
				this.popularityTrap.interactable = false;
				this.popularityTrapImage.color = new Color(this.popularityTrapImage.color.r, this.popularityTrapImage.color.g, this.popularityTrapImage.color.b, 0.3f);
				return;
			case PayType.Power:
				this.powerTrap.interactable = false;
				this.powerTrapImage.color = new Color(this.powerTrapImage.color.r, this.powerTrapImage.color.g, this.powerTrapImage.color.b, 0.3f);
				return;
			case PayType.CombatCard:
				this.combatCardTrap.interactable = false;
				this.combatCardTrapImage.color = new Color(this.combatCardTrapImage.color.r, this.combatCardTrapImage.color.g, this.combatCardTrapImage.color.b, 0.3f);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0003DE28 File Offset: 0x0003C028
		public void Hide()
		{
			if (!PlatformManager.IsMobile)
			{
				base.HidePanelTriggerAnimation();
				return;
			}
			if (SingletonMono<TrapsPanelMobile>.Instance.isPreview)
			{
				base.HidePanelMobile(false);
				return;
			}
			base.HidePanelMobile(true);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000CCA84 File Offset: 0x000CAC84
		public void TokenSelected(int id)
		{
			GameHex gameHexLogic = this.attachedGameHex.GetGameHexLogic();
			base.tokenManager.GetLastMovedUnit();
			base.tokenManager.PlaceToken(gameHexLogic, id);
			this.UpdateNumberOfTokens();
			this.tokensMenuController.TokenPlaced();
			this.Hide();
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0003DE53 File Offset: 0x0003C053
		protected override void OnShowTokenPanelComplete()
		{
			base.OnShowTokenPanelComplete();
			this.EnableButtons();
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0003DE61 File Offset: 0x0003C061
		protected override void PreHideTokenPanel()
		{
			this.DisableButtons();
		}

		// Token: 0x040017CA RID: 6090
		[SerializeField]
		private GameObject trapIcon;

		// Token: 0x040017CB RID: 6091
		[SerializeField]
		private Transform trapPanel;

		// Token: 0x040017CC RID: 6092
		[SerializeField]
		private Transform buttons;

		// Token: 0x040017CD RID: 6093
		[SerializeField]
		private Button popularityTrap;

		// Token: 0x040017CE RID: 6094
		[SerializeField]
		private Button combatCardTrap;

		// Token: 0x040017CF RID: 6095
		[SerializeField]
		private Button powerTrap;

		// Token: 0x040017D0 RID: 6096
		[SerializeField]
		private Button coinTrap;

		// Token: 0x040017D1 RID: 6097
		[SerializeField]
		private Image popularityTrapImage;

		// Token: 0x040017D2 RID: 6098
		[SerializeField]
		private Image combatCardTrapImage;

		// Token: 0x040017D3 RID: 6099
		[SerializeField]
		private Image powerTrapImage;

		// Token: 0x040017D4 RID: 6100
		[SerializeField]
		private Image coinTrapImage;

		// Token: 0x040017D5 RID: 6101
		[SerializeField]
		private TextMeshProUGUI tokenLeftAmount;
	}
}
