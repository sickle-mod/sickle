using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B0 RID: 1200
	public class TrapsPanelMobile : SingletonMono<TrapsPanelMobile>
	{
		// Token: 0x0600260F RID: 9743 RVA: 0x00040514 File Offset: 0x0003E714
		public void WindowPreview(bool enablePreview)
		{
			this.isPreview = enablePreview;
			if (enablePreview)
			{
				this.trapsText.text = ScriptLocalization.Get("GameScene/Traps");
				return;
			}
			this.trapsText.text = ScriptLocalization.Get("GameScene/PlaceTrap");
			this.TurnOnButtons();
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000E206C File Offset: 0x000E026C
		public void TurnOffButtons()
		{
			this.isPreview = true;
			foreach (FactionAbilityToken factionAbilityToken in GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetPlacedTokens())
			{
				TrapToken trapToken = (TrapToken)factionAbilityToken;
				this.ChangeAlphaButtons(trapToken.Penalty);
			}
			this.ChangeInteractionTrapButton(PayType.Coin, false);
			this.ChangeInteractionTrapButton(PayType.Popularity, false);
			this.ChangeInteractionTrapButton(PayType.Power, false);
			this.ChangeInteractionTrapButton(PayType.CombatCard, false);
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00040551 File Offset: 0x0003E751
		public void TurnOnButtons()
		{
			this.ChangeInteractionTrapButton(PayType.Coin, true);
			this.ChangeInteractionTrapButton(PayType.Popularity, true);
			this.ChangeInteractionTrapButton(PayType.Power, true);
			this.ChangeInteractionTrapButton(PayType.CombatCard, true);
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000E2104 File Offset: 0x000E0304
		private void ChangeAlphaButtons(PayType penalty)
		{
			switch (penalty)
			{
			case PayType.Coin:
				this.coinTrapImage.color = new Color(this.coinTrapImage.color.r, this.coinTrapImage.color.g, this.coinTrapImage.color.b, 0.3f);
				return;
			case PayType.Popularity:
				this.popularityTrapImage.color = new Color(this.popularityTrapImage.color.r, this.popularityTrapImage.color.g, this.popularityTrapImage.color.b, 0.3f);
				return;
			case PayType.Power:
				this.powerTrapImage.color = new Color(this.powerTrapImage.color.r, this.powerTrapImage.color.g, this.powerTrapImage.color.b, 0.3f);
				return;
			case PayType.CombatCard:
				this.combatCardTrapImage.color = new Color(this.combatCardTrapImage.color.r, this.combatCardTrapImage.color.g, this.combatCardTrapImage.color.b, 0.3f);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000E2240 File Offset: 0x000E0440
		private void ChangeInteractionTrapButton(PayType penalty, bool enabled)
		{
			switch (penalty)
			{
			case PayType.Coin:
				this.coinTrap.interactable = enabled;
				return;
			case PayType.Popularity:
				this.popularityTrap.interactable = enabled;
				return;
			case PayType.Power:
				this.powerTrap.interactable = enabled;
				return;
			case PayType.CombatCard:
				this.combatCardTrap.interactable = enabled;
				return;
			default:
				return;
			}
		}

		// Token: 0x04001AFC RID: 6908
		[SerializeField]
		private Image popularityTrapImage;

		// Token: 0x04001AFD RID: 6909
		[SerializeField]
		private Image combatCardTrapImage;

		// Token: 0x04001AFE RID: 6910
		[SerializeField]
		private Image powerTrapImage;

		// Token: 0x04001AFF RID: 6911
		[SerializeField]
		private Image coinTrapImage;

		// Token: 0x04001B00 RID: 6912
		[SerializeField]
		private Button popularityTrap;

		// Token: 0x04001B01 RID: 6913
		[SerializeField]
		private Button combatCardTrap;

		// Token: 0x04001B02 RID: 6914
		[SerializeField]
		private Button powerTrap;

		// Token: 0x04001B03 RID: 6915
		[SerializeField]
		private Button coinTrap;

		// Token: 0x04001B04 RID: 6916
		[SerializeField]
		private TextMeshProUGUI trapsText;

		// Token: 0x04001B05 RID: 6917
		public bool isPreview;
	}
}
