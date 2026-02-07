using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class CombatPanelResult : MonoBehaviour
{
	// Token: 0x06000507 RID: 1287 RVA: 0x00065FCC File Offset: 0x000641CC
	public void OnEnable()
	{
		for (int i = 0; i < this._playerCards.Length; i++)
		{
			this._playerCards[i].IsAvailable = false;
		}
		for (int j = 0; j < this._opponentCards.Length; j++)
		{
			this._opponentCards[j].IsAvailable = false;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0006601C File Offset: 0x0006421C
	public void SetResult(Player player, Player opponent, Dictionary<Player, PowerSelected> powerSelected)
	{
		TMP_Text playerPowerText = this._playerPowerText;
		PowerSelected powerSelected2 = powerSelected[player];
		playerPowerText.text = powerSelected2.selectedPower.ToString();
		TMP_Text opponentPowerText = this._opponentPowerText;
		powerSelected2 = powerSelected[opponent];
		opponentPowerText.text = powerSelected2.selectedPower.ToString();
		this._combatResultText.text = LocalizationManager.GetTranslation("GameScene/Victory" + GameController.GameManager.combatManager.GetWinner().matFaction.faction.ToString(), true, 0, true, false, null, null);
		for (int i = 0; i < powerSelected[player].selectedCards.Count; i++)
		{
			this._playerCards[i].IsAvailable = true;
			this._playerCards[i].SelectPowerValueSprite(powerSelected[player].selectedCards[i].CombatBonus);
		}
		for (int j = 0; j < powerSelected[opponent].selectedCards.Count; j++)
		{
			this._opponentCards[j].IsAvailable = true;
			this._opponentCards[j].SelectPowerValueSprite(powerSelected[opponent].selectedCards[j].CombatBonus);
		}
	}

	// Token: 0x0400041D RID: 1053
	private const string _victoryLocalizationPrefix = "GameScene/Victory";

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private TMP_Text _playerPowerText;

	// Token: 0x0400041F RID: 1055
	[SerializeField]
	private TMP_Text _opponentPowerText;

	// Token: 0x04000420 RID: 1056
	[SerializeField]
	private TMP_Text _combatResultText;

	// Token: 0x04000421 RID: 1057
	[SerializeField]
	private CombatPanelPlayerCard[] _playerCards;

	// Token: 0x04000422 RID: 1058
	[SerializeField]
	private CombatPanelPlayerCard[] _opponentCards;
}
