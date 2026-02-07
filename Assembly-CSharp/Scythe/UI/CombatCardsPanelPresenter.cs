using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000433 RID: 1075
	public class CombatCardsPanelPresenter : MonoBehaviour
	{
		// Token: 0x06002104 RID: 8452 RVA: 0x000C7570 File Offset: 0x000C5770
		public void SetCards(List<CombatCard> combatCards, List<CombatCard> cardsLocked = null)
		{
			if (!GameController.GameManager.IsMultiplayer || !MultiplayerController.Instance.WaitingForCombatCards)
			{
				this.payMode = CombatCardsPanelPresenter.PayMode.None;
				if (cardsLocked == null)
				{
					this.cardsLocked.Clear();
				}
				else
				{
					this.cardsLocked = new HashSet<CombatCard>(cardsLocked);
				}
			}
			this.combatCards = combatCards;
			this.Refresh();
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000C75C8 File Offset: 0x000C57C8
		public void Refresh()
		{
			if (this.combatCards == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.counters[i] = 0;
			}
			for (int j = 0; j < this.combatCards.Count; j++)
			{
				if (!this.cardsLocked.Contains(this.combatCards[j]))
				{
					this.counters[this.combatCards[j].CombatBonus - 2]++;
				}
			}
			for (int k = 0; k < 4; k++)
			{
				this.bombCounters[k].text = this.counters[k].ToString().PadLeft(2, '0');
				if (!GameController.GameManager.IsMultiplayer || !MultiplayerController.Instance.WaitingForCombatCards)
				{
					this.bombs[k].interactable = false;
					this.bombs[k].transform.GetChild(0).GetComponent<Image>().enabled = false;
				}
				if (!PlatformManager.IsStandalone)
				{
					if (this.counters[k] == 0)
					{
						this.cardImages[k].sprite = this.cardInactiveSprite;
						this.cardPowerImages[k].color = this.cardInactivePowerColor;
					}
					else
					{
						this.cardImages[k].sprite = this.cardActiveSprite;
						this.cardPowerImages[k].color = this.cardActivePowerColor;
					}
				}
				else
				{
					this.bombs[k].gameObject.SetActive(this.counters[k] > 0);
				}
			}
			if (this.totalCount != null)
			{
				this.totalCount.text = (this.combatCards.Count - this.cardsLocked.Count).ToString();
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000C7778 File Offset: 0x000C5978
		private CombatCard GetCard(int power)
		{
			for (int i = 0; i < this.combatCards.Count; i++)
			{
				if (this.combatCards[i].CombatBonus == power && !this.cardsLocked.Contains(this.combatCards[i]))
				{
					return this.combatCards[i];
				}
			}
			return null;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000C77D8 File Offset: 0x000C59D8
		public void BombClicked(int power)
		{
			if (this.combatCards.Any((CombatCard card) => card.CombatBonus == power))
			{
				switch (this.payMode)
				{
				case CombatCardsPanelPresenter.PayMode.Card:
					this.combatCardPresenter.OnCardPressed(this.GetCard(power));
					return;
				case CombatCardsPanelPresenter.PayMode.Resource:
					this.payResourcePresenter.OnCombatCardSelected(this.GetCard(power));
					if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1)
					{
						GameController.Instance.HideCombatCards();
					}
					if (!PlatformManager.IsStandalone)
					{
						this.PayResourceWithCombatCardeEnded();
						base.transform.parent.gameObject.SetActive(false);
						return;
					}
					break;
				case CombatCardsPanelPresenter.PayMode.Battle:
					this.combatPreparationPresenter.OnCardAdd(this.GetCard(power));
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0003D0A6 File Offset: 0x0003B2A6
		public void PayResourceWithCombatCardStarted()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.payWithCombatCardInfoText.gameObject.SetActive(true);
				this.showCombatCardsButtonImage.sprite = this.roundButtonBackgroundActiveSprite;
				this.payAsResourceText.gameObject.SetActive(true);
			}
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0003D0E2 File Offset: 0x0003B2E2
		public void UpdateResourceForPayWithResourceText(string text)
		{
			if (!PlatformManager.IsStandalone)
			{
				this.payAsResourceText.text = text;
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0003D0F7 File Offset: 0x0003B2F7
		public void PayResourceWithCombatCardeEnded()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.payWithCombatCardInfoText.gameObject.SetActive(false);
				this.showCombatCardsButtonImage.sprite = this.roundButtonBackgroundSprite;
				this.payAsResourceText.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0003D133 File Offset: 0x0003B333
		public void PayResourceMode()
		{
			this.payMode = CombatCardsPanelPresenter.PayMode.Resource;
			if (!PlatformManager.IsStandalone)
			{
				this.PayResourceWithCombatCardStarted();
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0003D149 File Offset: 0x0003B349
		public void PayCardMode()
		{
			this.payMode = CombatCardsPanelPresenter.PayMode.Card;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0003D152 File Offset: 0x0003B352
		public void BattleMode()
		{
			this.payMode = CombatCardsPanelPresenter.PayMode.Battle;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000C78B8 File Offset: 0x000C5AB8
		public void FocusCards(bool focus)
		{
			for (int i = 0; i < 4; i++)
			{
				this.bombs[i].interactable = focus;
				this.bombs[i].transform.GetChild(0).GetComponent<Image>().enabled = focus;
			}
			if (this.activeLight != null && (!focus || (focus && this.combatCards != null && this.combatCards.Count > 0 && this.payMode != CombatCardsPanelPresenter.PayMode.Resource)))
			{
				this.activeLight.SetActive(focus);
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0003D15B File Offset: 0x0003B35B
		public void LockCard(CombatCard card)
		{
			this.cardsLocked.Add(card);
			this.Refresh();
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0003D170 File Offset: 0x0003B370
		public void UnlockCard(CombatCard card)
		{
			this.cardsLocked.Remove(card);
			this.Refresh();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0003D185 File Offset: 0x0003B385
		public void UnlockAllCards()
		{
			this.cardsLocked.Clear();
			this.Refresh();
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0003D198 File Offset: 0x0003B398
		public void ShowCombatCards()
		{
			this.ammoCover.SetActive(false);
			if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode)
			{
				this.ammoBorder.raycastTarget = false;
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x0003D1D7 File Offset: 0x0003B3D7
		public void HideCombatCards()
		{
			this.ammoCover.SetActive(true);
			if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode)
			{
				this.ammoBorder.raycastTarget = true;
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0003D216 File Offset: 0x0003B416
		public bool AmmoCardsInvisible()
		{
			return this.ammoCover.activeInHierarchy;
		}

		// Token: 0x0400170D RID: 5901
		public Image showCombatCardsButtonImage;

		// Token: 0x0400170E RID: 5902
		public Sprite roundButtonBackgroundSprite;

		// Token: 0x0400170F RID: 5903
		public Sprite roundButtonBackgroundActiveSprite;

		// Token: 0x04001710 RID: 5904
		public Text totalCount;

		// Token: 0x04001711 RID: 5905
		public GameObject activeLight;

		// Token: 0x04001712 RID: 5906
		public PayCombatCardPresenter combatCardPresenter;

		// Token: 0x04001713 RID: 5907
		public PayResourcePresenter payResourcePresenter;

		// Token: 0x04001714 RID: 5908
		public CombatPreperationPresenter combatPreparationPresenter;

		// Token: 0x04001715 RID: 5909
		public Sprite cardActiveSprite;

		// Token: 0x04001716 RID: 5910
		public Color cardActivePowerColor;

		// Token: 0x04001717 RID: 5911
		public Sprite cardInactiveSprite;

		// Token: 0x04001718 RID: 5912
		public Color cardInactivePowerColor;

		// Token: 0x04001719 RID: 5913
		public Text[] bombCounters = new Text[4];

		// Token: 0x0400171A RID: 5914
		public Button[] bombs = new Button[4];

		// Token: 0x0400171B RID: 5915
		public Image[] cardImages = new Image[4];

		// Token: 0x0400171C RID: 5916
		public Image[] cardPowerImages = new Image[4];

		// Token: 0x0400171D RID: 5917
		private int[] counters = new int[4];

		// Token: 0x0400171E RID: 5918
		public TextMeshProUGUI payWithCombatCardInfoText;

		// Token: 0x0400171F RID: 5919
		public TextMeshProUGUI payAsResourceText;

		// Token: 0x04001720 RID: 5920
		private List<CombatCard> combatCards;

		// Token: 0x04001721 RID: 5921
		private CombatCardsPanelPresenter.PayMode payMode;

		// Token: 0x04001722 RID: 5922
		private HashSet<CombatCard> cardsLocked = new HashSet<CombatCard>();

		// Token: 0x04001723 RID: 5923
		[SerializeField]
		private GameObject ammoCover;

		// Token: 0x04001724 RID: 5924
		[SerializeField]
		private Image ammoBorder;

		// Token: 0x02000434 RID: 1076
		private enum PayMode
		{
			// Token: 0x04001726 RID: 5926
			None,
			// Token: 0x04001727 RID: 5927
			Card,
			// Token: 0x04001728 RID: 5928
			Resource,
			// Token: 0x04001729 RID: 5929
			Battle
		}
	}
}
