using System;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class CombatPanelPart : MonoBehaviour
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x000653B8 File Offset: 0x000635B8
	public virtual void SetUpPart(Player player, bool preparation)
	{
		GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
		bool flag = GameController.GameManager.combatManager.GetAttacker() == player;
		if (preparation)
		{
			this._combatPanelCurrentPower.gameObject.SetActive(true);
			this._combatPanelCurrentPower.SetUp(player.Power, player.combatCards.Count);
		}
		else
		{
			this._combatPanelCurrentPower.gameObject.SetActive(false);
		}
		this._combatPanelUnits.ShowUnits(player, GameController.GameManager.combatManager.GetSelectedBattlefield());
		this._combatPanelBottomInformation.SetUpInformation(factionInfo, flag);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0002AFAC File Offset: 0x000291AC
	public virtual void Reset()
	{
		this._combatPanelUnits.HideUnits();
	}

	// Token: 0x040003ED RID: 1005
	[SerializeField]
	private CombatPanelCurrentPower _combatPanelCurrentPower;

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	private CombatPanelBottomInformation _combatPanelBottomInformation;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private CombatPanelUnits _combatPanelUnits;
}
