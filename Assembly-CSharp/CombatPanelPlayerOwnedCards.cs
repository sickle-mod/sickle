using System;
using System.Linq;
using Scythe.GameLogic;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class CombatPanelPlayerOwnedCards : MonoBehaviour
{
	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060004CC RID: 1228 RVA: 0x00065554 File Offset: 0x00063754
	// (remove) Token: 0x060004CD RID: 1229 RVA: 0x0006558C File Offset: 0x0006378C
	public event Action<int> CardSelected;

	// Token: 0x060004CE RID: 1230 RVA: 0x000655C4 File Offset: 0x000637C4
	public void SetUp(int cardSlots, CombatCard[] combatCards)
	{
		int i;
		Func<CombatCard, bool> <>9__2;
		int k;
		for (i = 0; i < this._cardStack.Length; i = k + 1)
		{
			Func<CombatCard, bool> func;
			if ((func = <>9__2) == null)
			{
				func = (<>9__2 = (CombatCard card) => card.CombatBonus == this._cardStack[i].CardBonusPower);
			}
			CombatCard[] array = (from card in combatCards.Where(func)
				select (card)).ToArray<CombatCard>();
			this._cardStack[i].SetUp(cardSlots, array);
			this._cardStack[i].CardSelected += this.OnCardSelected;
			k = i;
		}
		CombatPanelCardStack[] array2 = (from stack in this._cardStack
			where stack.IsEmpty
			select (stack)).ToArray<CombatPanelCardStack>();
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].ShowNoCardsIndication();
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00065700 File Offset: 0x00063900
	public void ChangeNumberOfAvailableSlots(int slotsAvailable)
	{
		for (int i = 0; i < this._cardStack.Length; i++)
		{
			this._cardStack[i].SetSlotsAvailable(slotsAvailable);
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00065730 File Offset: 0x00063930
	public void OnCardDeselected(int cardBonusPower)
	{
		(from stack in this._cardStack
			where stack.CardBonusPower == cardBonusPower
			select (stack)).ToArray<CombatPanelCardStack>()[0].CardReturned();
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00065794 File Offset: 0x00063994
	public void Reset()
	{
		for (int i = 0; i < this._cardStack.Length; i++)
		{
			this._cardStack[i].Reset();
			this._cardStack[i].CardSelected -= this.OnCardSelected;
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000657DC File Offset: 0x000639DC
	public void LockState()
	{
		for (int i = 0; i < this._cardStack.Length; i++)
		{
			this._cardStack[i].CardSelected -= this.OnCardSelected;
			this._cardStack[i].Lock();
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0002B023 File Offset: 0x00029223
	private void OnCardSelected(int cardBonusPower)
	{
		this.CardSelected(cardBonusPower);
	}

	// Token: 0x040003FB RID: 1019
	[SerializeField]
	private CombatPanelCardStack[] _cardStack;
}
