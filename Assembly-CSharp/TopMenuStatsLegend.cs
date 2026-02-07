using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class TopMenuStatsLegend : MonoBehaviour
{
	// Token: 0x060006AE RID: 1710 RVA: 0x000712E8 File Offset: 0x0006F4E8
	public void UpdateVisibility(bool[] visibilityArray)
	{
		if (visibilityArray.Length == this.cells.Length)
		{
			for (int i = 0; i < this.cells.Length; i++)
			{
				this.cells[i].SetActive(visibilityArray[i]);
			}
			return;
		}
		Debug.LogError(string.Concat(new string[]
		{
			"Visibility array length (",
			visibilityArray.Length.ToString(),
			") doesn't match cells length (",
			this.cells.Length.ToString(),
			")"
		}));
	}

	// Token: 0x040005F2 RID: 1522
	[SerializeField]
	private GameObject[] cells;
}
