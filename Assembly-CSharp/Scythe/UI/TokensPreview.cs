using System;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200049E RID: 1182
	public class TokensPreview : MonoBehaviour
	{
		// Token: 0x06002577 RID: 9591 RVA: 0x000DE9A4 File Offset: 0x000DCBA4
		private void Start()
		{
			TokensController.OnTokenPlaced += this.UpdateTokensCounterText;
			GameController.OnEndTurnClick += this.UpdateTokensCounterIcon;
			GameController.OnEndTurnClick += this.UpdateTokensCounterText;
			GameController.OnGameLoaded += this.UpdateTokensCounterIcon;
			GameController.OnGameLoaded += this.UpdateTokensCounterText;
			ChangeFactionPanel.spectatingFactionSwitched += this.UpdateTokensCounterIcon;
			ChangeFactionPanel.spectatingFactionSwitched += this.UpdateTokensCounterText;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000DEA28 File Offset: 0x000DCC28
		private void OnDisable()
		{
			TokensController.OnTokenPlaced -= this.UpdateTokensCounterText;
			GameController.OnEndTurnClick -= this.UpdateTokensCounterIcon;
			GameController.OnEndTurnClick -= this.UpdateTokensCounterText;
			GameController.OnGameLoaded -= this.UpdateTokensCounterIcon;
			GameController.OnGameLoaded -= this.UpdateTokensCounterText;
			ChangeFactionPanel.spectatingFactionSwitched -= this.UpdateTokensCounterIcon;
			ChangeFactionPanel.spectatingFactionSwitched -= this.UpdateTokensCounterText;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000DEAAC File Offset: 0x000DCCAC
		private void UpdateTokensCounterIcon()
		{
			Faction faction = this.GetPlayer().matFaction.faction;
			if (faction != Faction.Albion)
			{
				if (faction != Faction.Togawa)
				{
					if (this.previewButton != null && !this.PreviewButtonShouldBeVisible())
					{
						this.previewButton.gameObject.SetActive(false);
						return;
					}
					Debug.Log("previewButton null");
				}
				else
				{
					if (!(this.previewButton != null) || !(this.previewIcon != null) || !this.PreviewButtonShouldBeVisible())
					{
						Debug.Log("previewButton or previewIcon null");
						return;
					}
					this.previewIcon.sprite = this.trapIcon;
					this.previewButton.gameObject.SetActive(true);
					if (GameController.GameManager.SpectatorMode)
					{
						this.previewButton.interactable = false;
						return;
					}
				}
			}
			else
			{
				if (!(this.previewButton != null) || !(this.previewIcon != null) || !this.PreviewButtonShouldBeVisible())
				{
					Debug.Log("previewButton or previewIcon null");
					return;
				}
				this.previewIcon.sprite = this.flagIcon;
				this.previewButton.gameObject.SetActive(true);
				if (GameController.GameManager.SpectatorMode)
				{
					this.previewButton.interactable = false;
					return;
				}
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000DEBE0 File Offset: 0x000DCDE0
		private bool PreviewButtonShouldBeVisible()
		{
			return (GameController.GameManager.IsMultiplayer && (GameController.GameManager.PlayerMaster.matFaction.faction.Equals(Faction.Togawa) || GameController.GameManager.PlayerMaster.matFaction.faction.Equals(Faction.Albion))) || ((GameController.GameManager.IsAIHotSeat || GameController.GameManager.IsHotSeat) && !GameController.GameManager.IsCampaign && GameController.GameManager.PlayerCurrent.IsHuman);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000DEC84 File Offset: 0x000DCE84
		private void UpdateTokensCounterText()
		{
			TokenSupply factionTokens = this.GetPlayer().matFaction.FactionTokens;
			if (factionTokens != null && this.topPanelFactionTokenText != null)
			{
				this.topPanelFactionTokenText.text = factionTokens.GetUnplacedTokensCount().ToString();
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000DECCC File Offset: 0x000DCECC
		public void TokenPanelActivation()
		{
			if (GameController.GameManager.PlayerMaster.matFaction.faction.Equals(Faction.Togawa))
			{
				this.trapsPanelMobile.WindowPreview(true);
				this.placeTrapPanel.gameObject.SetActive(true);
				this.trapsPanelMobile.TurnOffButtons();
				return;
			}
			if (GameController.GameManager.PlayerMaster.matFaction.faction.Equals(Faction.Albion))
			{
				this.flagsPanelMobile.WindowPreview(true);
				this.placeFlagPanel.gameObject.SetActive(true);
				this.flagsPanelMobile.TurnOffButtons();
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x0003FDD8 File Offset: 0x0003DFD8
		private Player GetPlayer()
		{
			return GameController.GameManager.PlayerMaster;
		}

		// Token: 0x04001A41 RID: 6721
		[SerializeField]
		private Image previewIcon;

		// Token: 0x04001A42 RID: 6722
		[SerializeField]
		private Sprite flagIcon;

		// Token: 0x04001A43 RID: 6723
		[SerializeField]
		private Sprite trapIcon;

		// Token: 0x04001A44 RID: 6724
		[SerializeField]
		private GameObject placeTrapPanel;

		// Token: 0x04001A45 RID: 6725
		[SerializeField]
		private GameObject placeFlagPanel;

		// Token: 0x04001A46 RID: 6726
		[SerializeField]
		private Button previewButton;

		// Token: 0x04001A47 RID: 6727
		[SerializeField]
		private Text topPanelFactionTokenText;

		// Token: 0x04001A48 RID: 6728
		[SerializeField]
		private FlagsPanelMobile flagsPanelMobile;

		// Token: 0x04001A49 RID: 6729
		[SerializeField]
		private TrapsPanelMobile trapsPanelMobile;
	}
}
