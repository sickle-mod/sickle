using System;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C0 RID: 1216
	public class TokensCounterPresenter : MonoBehaviour
	{
		// Token: 0x060026B9 RID: 9913 RVA: 0x000E5DB8 File Offset: 0x000E3FB8
		private void OnEnable()
		{
			TokensController.OnTokenPlaced += this.UpdateTokensCounterText;
			GameController.OnEndTurnClick += this.UpdateTokensCounterIcon;
			GameController.OnEndTurnClick += this.UpdateTokensCounterText;
			GameController.OnGameLoaded += this.UpdateTokensCounterIcon;
			GameController.OnGameLoaded += this.UpdateTokensCounterText;
			ChangeFactionPanel.spectatingFactionSwitched += this.UpdateTokensCounterIcon;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000E5E2C File Offset: 0x000E402C
		private void OnDisable()
		{
			TokensController.OnTokenPlaced -= this.UpdateTokensCounterText;
			GameController.OnEndTurnClick -= this.UpdateTokensCounterIcon;
			GameController.OnEndTurnClick -= this.UpdateTokensCounterText;
			GameController.OnGameLoaded -= this.UpdateTokensCounterIcon;
			GameController.OnGameLoaded -= this.UpdateTokensCounterText;
			ChangeFactionPanel.spectatingFactionSwitched -= this.UpdateTokensCounterIcon;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000E5EA0 File Offset: 0x000E40A0
		private void UpdateTokensCounterIcon()
		{
			Faction faction = this.GetPlayer().matFaction.faction;
			if (faction == Faction.Albion)
			{
				this.topPanelFactionTokenIcon.enabled = true;
				this.topPanelFactionTokenIcon.sprite = this.albionSprite;
				this.topPanelFactionTokenText.enabled = true;
				return;
			}
			if (faction != Faction.Togawa)
			{
				this.topPanelFactionTokenIcon.enabled = false;
				this.topPanelFactionTokenText.enabled = false;
				return;
			}
			this.topPanelFactionTokenIcon.enabled = true;
			this.topPanelFactionTokenIcon.sprite = this.togawaSprite;
			this.topPanelFactionTokenText.enabled = true;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000E5F34 File Offset: 0x000E4134
		private void UpdateTokensCounterText()
		{
			TokenSupply factionTokens = this.GetPlayer().matFaction.FactionTokens;
			if (factionTokens != null)
			{
				this.topPanelFactionTokenText.text = factionTokens.GetUnplacedTokensCount().ToString();
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0008A35C File Offset: 0x0008855C
		private Player GetPlayer()
		{
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			return player;
		}

		// Token: 0x04001BB7 RID: 7095
		[SerializeField]
		private Image topPanelFactionTokenIcon;

		// Token: 0x04001BB8 RID: 7096
		[SerializeField]
		private Text topPanelFactionTokenText;

		// Token: 0x04001BB9 RID: 7097
		public Sprite albionSprite;

		// Token: 0x04001BBA RID: 7098
		public Sprite togawaSprite;
	}
}
