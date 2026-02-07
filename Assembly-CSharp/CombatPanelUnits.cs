using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class CombatPanelUnits : MonoBehaviour
{
	// Token: 0x06000519 RID: 1305 RVA: 0x00066360 File Offset: 0x00064560
	public void ShowUnits(Player player, GameHex hex)
	{
		bool flag = player.character.position == hex;
		int num = 0;
		using (List<Mech>.Enumerator enumerator = player.matFaction.mechs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.position == hex)
				{
					num++;
				}
			}
		}
		switch (player.matFaction.faction)
		{
		case Faction.Polania:
			this._polaniaUnits.ShowUnits(num, flag);
			this._polaniaUnits.gameObject.SetActive(true);
			return;
		case Faction.Albion:
			this._albionUnits.ShowUnits(num, flag);
			this._albionUnits.gameObject.SetActive(true);
			return;
		case Faction.Nordic:
			this._nordicUnits.ShowUnits(num, flag);
			this._nordicUnits.gameObject.SetActive(true);
			return;
		case Faction.Rusviet:
			this._rusvietUnits.ShowUnits(num, flag);
			this._rusvietUnits.gameObject.SetActive(true);
			return;
		case Faction.Togawa:
			this._togawaUnits.ShowUnits(num, flag);
			this._togawaUnits.gameObject.SetActive(true);
			return;
		case Faction.Crimea:
			this._crimeaUnits.ShowUnits(num, flag);
			this._crimeaUnits.gameObject.SetActive(true);
			return;
		case Faction.Saxony:
			this._saxonyUnits.ShowUnits(num, flag);
			this._saxonyUnits.gameObject.SetActive(true);
			return;
		default:
			throw new ArgumentOutOfRangeException("faction", player.matFaction.faction, null);
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x000664F4 File Offset: 0x000646F4
	public void HideUnits()
	{
		this._crimeaUnits.HideUnits();
		this._nordicUnits.HideUnits();
		this._rusvietUnits.HideUnits();
		this._polaniaUnits.HideUnits();
		this._saxonyUnits.HideUnits();
		this._albionUnits.HideUnits();
		this._togawaUnits.HideUnits();
		this._crimeaUnits.gameObject.SetActive(false);
		this._polaniaUnits.gameObject.SetActive(false);
		this._saxonyUnits.gameObject.SetActive(false);
		this._nordicUnits.gameObject.SetActive(false);
		this._rusvietUnits.gameObject.SetActive(false);
		this._albionUnits.gameObject.SetActive(false);
		this._togawaUnits.gameObject.SetActive(false);
	}

	// Token: 0x0400042B RID: 1067
	[SerializeField]
	private CombatPanelUnitPresenter _crimeaUnits;

	// Token: 0x0400042C RID: 1068
	[SerializeField]
	private CombatPanelUnitPresenter _nordicUnits;

	// Token: 0x0400042D RID: 1069
	[SerializeField]
	private CombatPanelUnitPresenter _saxonyUnits;

	// Token: 0x0400042E RID: 1070
	[SerializeField]
	private CombatPanelUnitPresenter _rusvietUnits;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private CombatPanelUnitPresenter _polaniaUnits;

	// Token: 0x04000430 RID: 1072
	[SerializeField]
	private CombatPanelUnitPresenter _albionUnits;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private CombatPanelUnitPresenter _togawaUnits;
}
