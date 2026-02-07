using System;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008D RID: 141
public class CombatPanelOpponentPart : CombatPanelPart
{
	// Token: 0x060004BE RID: 1214 RVA: 0x00065274 File Offset: 0x00063474
	public override void SetUpPart(Player player, bool preparation)
	{
		base.SetUpPart(player, preparation);
		int possibleAmountOfCombatCardsToUse = GameController.GameManager.combatManager.GetPossibleAmountOfCombatCardsToUse(player);
		if (preparation)
		{
			for (int i = 0; i < Mathf.Min(player.combatCards.Count, this._ownedCards.Length); i++)
			{
				this._ownedCards[i].gameObject.SetActive(true);
			}
			for (int j = 0; j < possibleAmountOfCombatCardsToUse; j++)
			{
				this._selectedCards[j].gameObject.SetActive(true);
			}
			this._combatDistribution.SetActive(true);
			return;
		}
		for (int k = 0; k < this._ownedCards.Length; k++)
		{
			this._ownedCards[k].gameObject.SetActive(false);
		}
		for (int l = 0; l < this._selectedCards.Length; l++)
		{
			this._selectedCards[l].gameObject.SetActive(false);
		}
		this._combatDistribution.SetActive(false);
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0006535C File Offset: 0x0006355C
	public override void Reset()
	{
		base.Reset();
		for (int i = 0; i < this._selectedCards.Length; i++)
		{
			this._selectedCards[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this._ownedCards.Length; j++)
		{
			this._ownedCards[j].SetActive(false);
		}
	}

	// Token: 0x040003EA RID: 1002
	[SerializeField]
	private Image[] _selectedCards;

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	private GameObject[] _ownedCards;

	// Token: 0x040003EC RID: 1004
	[SerializeField]
	private GameObject _combatDistribution;
}
