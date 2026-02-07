using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class CombatPanelPlayerPart : CombatPanelPart
{
	// Token: 0x1400002E RID: 46
	// (add) Token: 0x060004DF RID: 1247 RVA: 0x00065824 File Offset: 0x00063A24
	// (remove) Token: 0x060004E0 RID: 1248 RVA: 0x0006585C File Offset: 0x00063A5C
	public event Action<int> TotalPowerChanged;

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0002B079 File Offset: 0x00029279
	public List<CombatCard> SelectedCards
	{
		get
		{
			return this._selectedCards;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0002B081 File Offset: 0x00029281
	public int DistributedPower
	{
		get
		{
			return this._distributedPower;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0002B089 File Offset: 0x00029289
	public int BonusCombatCardPower
	{
		get
		{
			return this._bonusCombatCardPower;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0002B091 File Offset: 0x00029291
	private int _totalPower
	{
		get
		{
			return this._distributedPower + this._bonusCombatCardPower;
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00065894 File Offset: 0x00063A94
	public void Awake()
	{
		this._panelPowerDistribution.DistributedPowerChanged += this.OnDistributedPowerChanged;
		this._combatPanelPlayerOwnedCards.CardSelected += this.OnCombatCardSelected;
		this._panelPlayerSelectedCards.PowerCardDeselected += this.OnPowerCardDeselected;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000658E8 File Offset: 0x00063AE8
	public override void Reset()
	{
		base.Reset();
		this._distributedPower = 0;
		this._bonusCombatCardPower = 0;
		this._selectedCards = new List<CombatCard>();
		this._panelPlayerSelectedCards.Reset();
		this._combatPanelPlayerOwnedCards.Reset();
		this._panelPowerDistribution.Reset();
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00065938 File Offset: 0x00063B38
	public override void SetUpPart(Player player, bool preparation)
	{
		base.SetUpPart(player, preparation);
		this._actualPlayer = player;
		int possibleAmountOfCombatCardsToUse = GameController.GameManager.combatManager.GetPossibleAmountOfCombatCardsToUse(player);
		if (preparation)
		{
			this._combatPanelPlayerOwnedCards.gameObject.SetActive(true);
			this._panelPowerDistribution.gameObject.SetActive(true);
			this._panelPlayerSelectedCards.gameObject.SetActive(true);
			this._combatPanelPlayerOwnedCards.SetUp(possibleAmountOfCombatCardsToUse, player.combatCards.ToArray());
			this._panelPowerDistribution.Init(player.Power);
			this._panelPlayerSelectedCards.PrepareSlots(possibleAmountOfCombatCardsToUse);
			return;
		}
		this._combatPanelPlayerOwnedCards.gameObject.SetActive(false);
		this._panelPowerDistribution.gameObject.SetActive(false);
		this._panelPlayerSelectedCards.gameObject.SetActive(false);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0002B0A0 File Offset: 0x000292A0
	public void LockState()
	{
		this._combatPanelPlayerOwnedCards.LockState();
		this._panelPlayerSelectedCards.LockState();
		this._panelPowerDistribution.LockState();
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00065A04 File Offset: 0x00063C04
	private void OnCombatCardSelected(int cardBonusPower)
	{
		this._panelPlayerSelectedCards.CombatCardSelected(cardBonusPower);
		this._combatPanelPlayerOwnedCards.ChangeNumberOfAvailableSlots(this._panelPlayerSelectedCards.AvailableSlots);
		this._bonusCombatCardPower += cardBonusPower;
		this._selectedCards.Add(this._actualPlayer.combatCards.Find((CombatCard card) => card.CombatBonus == cardBonusPower && !this._selectedCards.Contains(card)));
		if (this.TotalPowerChanged != null)
		{
			this.TotalPowerChanged(this._totalPower);
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00065AA0 File Offset: 0x00063CA0
	private void OnPowerCardDeselected(int cardBonusPower)
	{
		this._combatPanelPlayerOwnedCards.OnCardDeselected(cardBonusPower);
		this._combatPanelPlayerOwnedCards.ChangeNumberOfAvailableSlots(this._panelPlayerSelectedCards.AvailableSlots);
		this._bonusCombatCardPower -= cardBonusPower;
		this._selectedCards.Remove(this._actualPlayer.combatCards.Find((CombatCard card) => card.CombatBonus == cardBonusPower && this._selectedCards.Contains(card)));
		if (this.TotalPowerChanged != null)
		{
			this.TotalPowerChanged(this._totalPower);
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0002B0C3 File Offset: 0x000292C3
	private void OnDistributedPowerChanged(int distributedPower)
	{
		this._distributedPower = distributedPower;
		if (this.TotalPowerChanged != null)
		{
			this.TotalPowerChanged(this._totalPower);
		}
	}

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private CombatPanelPlayerOwnedCards _combatPanelPlayerOwnedCards;

	// Token: 0x04000407 RID: 1031
	[SerializeField]
	private CombatPanelPlayerSelectedCards _panelPlayerSelectedCards;

	// Token: 0x04000408 RID: 1032
	[SerializeField]
	private CombatPanelPowerDistribution _panelPowerDistribution;

	// Token: 0x04000409 RID: 1033
	private Player _actualPlayer;

	// Token: 0x0400040A RID: 1034
	private List<CombatCard> _selectedCards = new List<CombatCard>();

	// Token: 0x0400040B RID: 1035
	private int _distributedPower;

	// Token: 0x0400040C RID: 1036
	private int _bonusCombatCardPower;
}
