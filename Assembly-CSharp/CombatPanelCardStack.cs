using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class CombatPanelCardStack : MonoBehaviour
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x060004AB RID: 1195 RVA: 0x00064F4C File Offset: 0x0006314C
	// (remove) Token: 0x060004AC RID: 1196 RVA: 0x00064F84 File Offset: 0x00063184
	public event Action<int> CardSelected;

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060004AD RID: 1197 RVA: 0x0002AF14 File Offset: 0x00029114
	public bool IsEmpty
	{
		get
		{
			return this._playerCards.Count == 0;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060004AE RID: 1198 RVA: 0x0002AF24 File Offset: 0x00029124
	public int CardBonusPower
	{
		get
		{
			return this._cardBonusPower;
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00064FBC File Offset: 0x000631BC
	public void OnCombatCardClicked()
	{
		List<CombatPanelPlayerCard> list = this._playerCards.FindAll((CombatPanelPlayerCard card) => card.IsAvailable);
		if (!this.IsEmpty && this._cardSlotsAvailable > 0 && this.CardSelected != null && list.Count > 0)
		{
			if (list.Count == 1)
			{
				list[0].IsAvailable = false;
				this._allCardsSelected = true;
			}
			else
			{
				CombatPanelPlayerCard combatPanelPlayerCard = list.Last<CombatPanelPlayerCard>();
				combatPanelPlayerCard.IsAvailable = false;
				combatPanelPlayerCard.gameObject.SetActive(false);
			}
			this.CardSelected(this._cardBonusPower);
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00065060 File Offset: 0x00063260
	public void CardReturned()
	{
		List<CombatPanelPlayerCard> list = this._playerCards.FindAll((CombatPanelPlayerCard card) => !card.IsAvailable);
		if (this._allCardsSelected)
		{
			this._playerCards[0].IsAvailable = true;
			this._allCardsSelected = false;
			return;
		}
		if (list.Count > 0)
		{
			list[0].gameObject.SetActive(true);
			list[0].IsAvailable = true;
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0002AF2C File Offset: 0x0002912C
	public void SetUp(int slotsAvailable, CombatCard[] cards)
	{
		this._cardSlotsAvailable = slotsAvailable;
		this.AddCards(cards);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0002AF3C File Offset: 0x0002913C
	public void SetSlotsAvailable(int slotsAvailable)
	{
		this._cardSlotsAvailable = slotsAvailable;
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x000650E4 File Offset: 0x000632E4
	public void ShowNoCardsIndication()
	{
		CombatPanelPlayerCard combatPanelPlayerCard = global::UnityEngine.Object.Instantiate<CombatPanelPlayerCard>(this._playerCardPrefab, base.transform);
		this._playerCards.Add(combatPanelPlayerCard);
		combatPanelPlayerCard.IsAvailable = false;
		combatPanelPlayerCard.SelectPowerValueSprite(this._cardBonusPower);
		combatPanelPlayerCard.gameObject.SetActive(true);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00065130 File Offset: 0x00063330
	public void Reset()
	{
		for (int i = this._playerCards.Count - 1; i >= 0; i--)
		{
			CombatPanelPlayerCard combatPanelPlayerCard = this._playerCards[i];
			combatPanelPlayerCard.CombatCardClicked -= this.OnCombatCardClicked;
			global::UnityEngine.Object.Destroy(combatPanelPlayerCard.gameObject);
		}
		this._playerCards.Clear();
		this._allCardsSelected = false;
		this._cardSlotsAvailable = 0;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00065198 File Offset: 0x00063398
	public void Lock()
	{
		for (int i = 0; i < this._playerCards.Count; i++)
		{
			this._playerCards[i].CombatCardClicked -= this.OnCombatCardClicked;
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000651D8 File Offset: 0x000633D8
	private void AddCards(CombatCard[] cards)
	{
		int num = 0;
		while (num < cards.Length && num < this._maxCardsInStack)
		{
			CombatPanelPlayerCard combatPanelPlayerCard = global::UnityEngine.Object.Instantiate<CombatPanelPlayerCard>(this._playerCardPrefab, base.transform);
			combatPanelPlayerCard.transform.SetAsFirstSibling();
			combatPanelPlayerCard.SelectPowerValueSprite(this._cardBonusPower);
			this._playerCards.Add(combatPanelPlayerCard);
			combatPanelPlayerCard.gameObject.SetActive(true);
			combatPanelPlayerCard.CombatCardClicked += this.OnCombatCardClicked;
			num++;
		}
		if (this._playerCards.Count > 0)
		{
			this._playerCards.Last<CombatPanelPlayerCard>().SelectPowerValueSprite(this._cardBonusPower);
		}
	}

	// Token: 0x040003DF RID: 991
	[SerializeField]
	private int _cardBonusPower;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	private int _maxCardsInStack = 4;

	// Token: 0x040003E1 RID: 993
	[SerializeField]
	private CombatPanelPlayerCard _playerCardPrefab;

	// Token: 0x040003E2 RID: 994
	private List<CombatPanelPlayerCard> _playerCards = new List<CombatPanelPlayerCard>();

	// Token: 0x040003E3 RID: 995
	private bool _allCardsSelected;

	// Token: 0x040003E4 RID: 996
	private int _cardSlotsAvailable;
}
