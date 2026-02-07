using System;
using Scythe.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200004E RID: 78
public class AmmoCoverBorder : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06000294 RID: 660 RVA: 0x000298F5 File Offset: 0x00027AF5
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		if (!pointerEventData.fullyExited)
		{
			return;
		}
		this.CheckAndHideCombatCards();
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00029906 File Offset: 0x00027B06
	private void CheckAndHideCombatCards()
	{
		if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode)
		{
			GameController.Instance.HideCombatCards();
		}
	}
}
