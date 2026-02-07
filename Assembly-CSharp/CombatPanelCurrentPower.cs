using System;
using TMPro;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class CombatPanelCurrentPower : MonoBehaviour
{
	// Token: 0x060004BC RID: 1212 RVA: 0x0002AF7E File Offset: 0x0002917E
	public void SetUp(int power, int powerCards)
	{
		this._powerValueText.text = power.ToString();
		this._ownedCardsQuantityText.text = powerCards.ToString();
	}

	// Token: 0x040003E8 RID: 1000
	[SerializeField]
	private TMP_Text _powerValueText;

	// Token: 0x040003E9 RID: 1001
	[SerializeField]
	private TMP_Text _ownedCardsQuantityText;
}
