using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class CombatPanelPlayerSelectedCards : MonoBehaviour
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x060004F1 RID: 1265 RVA: 0x00065B3C File Offset: 0x00063D3C
	// (remove) Token: 0x060004F2 RID: 1266 RVA: 0x00065B74 File Offset: 0x00063D74
	public event Action<int> PowerCardDeselected;

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0002B141 File Offset: 0x00029341
	public int AvailableSlots
	{
		get
		{
			return this._combatCardSlots.Where((CombatPanelCardSlot slot) => slot.IsAvailable && slot.gameObject.activeInHierarchy).Count<CombatPanelCardSlot>();
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00065BAC File Offset: 0x00063DAC
	public void PrepareSlots(int availableSlots)
	{
		for (int i = 0; i < availableSlots; i++)
		{
			this._combatCardSlots[i].gameObject.SetActive(true);
			this._combatCardSlots[i].DeselectSlot();
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00065BE8 File Offset: 0x00063DE8
	public void CombatCardSelected(int bonusPower)
	{
		CombatPanelCardSlot combatPanelCardSlot = this._combatCardSlots[this._combatSlotCardDictionary.Count];
		combatPanelCardSlot.SlotClicked += this.OnCombatCardDeselected;
		combatPanelCardSlot.SelectSlot(bonusPower);
		this._combatSlotCardDictionary.Add(combatPanelCardSlot, bonusPower);
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00065C30 File Offset: 0x00063E30
	public void Reset()
	{
		for (int i = 0; i < this._combatCardSlots.Length; i++)
		{
			this._combatCardSlots[i].DeselectSlot();
			this._combatCardSlots[i].gameObject.SetActive(false);
			this._combatCardSlots[i].SlotClicked -= this.OnCombatCardDeselected;
		}
		this._combatSlotCardDictionary.Clear();
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00065C94 File Offset: 0x00063E94
	public void LockState()
	{
		for (int i = 0; i < this._combatCardSlots.Length; i++)
		{
			this._combatCardSlots[i].DetachEvent();
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00065CC4 File Offset: 0x00063EC4
	private void OnCombatCardDeselected(CombatPanelCardSlot slot)
	{
		slot.SlotClicked -= this.OnCombatCardDeselected;
		slot.DeselectSlot();
		if (this.PowerCardDeselected != null)
		{
			this.PowerCardDeselected(this._combatSlotCardDictionary[slot]);
		}
		this._combatSlotCardDictionary.Remove(slot);
		this.OrganizeSelection();
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00065D1C File Offset: 0x00063F1C
	private void OrganizeSelection()
	{
		int[] array = this._combatSlotCardDictionary.Values.ToArray<int>();
		for (int i = 0; i < this._combatCardSlots.Length; i++)
		{
			this._combatCardSlots[i].SlotClicked -= this.OnCombatCardDeselected;
			this._combatCardSlots[i].DeselectSlot();
			this._combatSlotCardDictionary.Remove(this._combatCardSlots[i]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			this.CombatCardSelected(array[j]);
		}
	}

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private CombatPanelCardSlot[] _combatCardSlots;

	// Token: 0x04000413 RID: 1043
	private Dictionary<CombatPanelCardSlot, int> _combatSlotCardDictionary = new Dictionary<CombatPanelCardSlot, int>();
}
