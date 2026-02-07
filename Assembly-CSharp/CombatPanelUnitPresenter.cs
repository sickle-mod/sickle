using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class CombatPanelUnitPresenter : MonoBehaviour
{
	// Token: 0x06000516 RID: 1302 RVA: 0x000662E0 File Offset: 0x000644E0
	public void ShowUnits(int mechCount, bool heroPresent)
	{
		for (int i = 0; i < Mathf.Min(this._mechs.Length, mechCount); i++)
		{
			this._mechs[i].SetActive(true);
		}
		this._hero.SetActive(heroPresent);
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00066320 File Offset: 0x00064520
	public void HideUnits()
	{
		for (int i = 0; i < this._mechs.Length; i++)
		{
			this._mechs[i].gameObject.SetActive(false);
		}
		this._hero.SetActive(false);
	}

	// Token: 0x04000429 RID: 1065
	[SerializeField]
	private GameObject[] _mechs;

	// Token: 0x0400042A RID: 1066
	[SerializeField]
	private GameObject _hero;
}
