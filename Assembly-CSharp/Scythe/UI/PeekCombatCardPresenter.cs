using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003FA RID: 1018
	internal class PeekCombatCardPresenter : ActionPresenter
	{
		// Token: 0x06001EF8 RID: 7928 RVA: 0x0003C07D File Offset: 0x0003A27D
		private void Awake()
		{
			if (this.playerSelectionPanel != null)
			{
				this.playerSelectionPanel.PlayerSelected += this.OnPlayerSelected;
			}
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000BE828 File Offset: 0x000BCA28
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainPeekCombatCards;
			this.action.SetStartingPeekFaction();
			if (this.playerSelectionPanel != null)
			{
				this.playerSelectionPanel.Show();
			}
			this.cardsPanel.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			if (this.playerSelectionPanel == null)
			{
				this.SetupStandalonePlayerSelectionPanel();
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000BE898 File Offset: 0x000BCA98
		public void OnFactionSelect(int buttonId)
		{
			for (int i = 0; i < this.factionSelection.Length; i++)
			{
				if (i != buttonId)
				{
					this.factionSelection[i].gameObject.SetActive(false);
				}
				else
				{
					this.factionSelection[i].interactable = false;
				}
			}
			this.cardsPanel.gameObject.SetActive(true);
			this.action.SetFaction(this.factionOrder[buttonId]);
			this.cardsPanel.SetCards(this.action.PeekCombatCards(), null);
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0003C0A4 File Offset: 0x0003A2A4
		public override void OnActionEnded()
		{
			base.gameObject.SetActive(false);
			HumanInputHandler.Instance.OnInputEnded();
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000BE920 File Offset: 0x000BCB20
		private void SetupStandalonePlayerSelectionPanel()
		{
			this.actualFactionEmlbem.sprite = GameController.factionInfo[this.action.GetPlayer().matFaction.faction].logo;
			int num = 0;
			foreach (Faction faction in GameController.GameManager.GetPlayersFactions())
			{
				if (faction != GameController.GameManager.PlayerCurrent.matFaction.faction)
				{
					this.factionOrder.Add(faction);
					this.factionSelection[num].gameObject.SetActive(true);
					this.factionSelection[num].interactable = true;
					this.factionSelection[num].image.sprite = GameController.factionInfo[faction].logo;
					num++;
				}
			}
			for (int i = num; i < this.factionSelection.Length; i++)
			{
				this.factionSelection[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x000BEA34 File Offset: 0x000BCC34
		private void OnPlayerSelected(Player player)
		{
			this.playerSelectionPanel.Hide();
			this.cardsPanel.gameObject.SetActive(true);
			this.action.SetFaction(player.matFaction.faction);
			this.cardsPanel.SetCards(this.action.PeekCombatCards(), null);
		}

		// Token: 0x040015D9 RID: 5593
		public Image actualFactionEmlbem;

		// Token: 0x040015DA RID: 5594
		public Button[] factionSelection;

		// Token: 0x040015DB RID: 5595
		public CombatCardsPanelPresenter cardsPanel;

		// Token: 0x040015DC RID: 5596
		[SerializeField]
		private PlayerSelectionPanel playerSelectionPanel;

		// Token: 0x040015DD RID: 5597
		private GainPeekCombatCards action;

		// Token: 0x040015DE RID: 5598
		private List<Faction> factionOrder = new List<Faction>();
	}
}
