using System;
using Scythe.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000056 RID: 86
public class HideCardsPointerHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060002AD RID: 685 RVA: 0x00029A4B File Offset: 0x00027C4B
	private void Awake()
	{
		if (!GameController.GameManager.IsHotSeat || GameController.GameManager.GetPlayersWithoutAICount() < 2 || GameController.GameManager.SpectatorMode)
		{
			base.transform.GetComponent<Image>().raycastTarget = false;
		}
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00029A83 File Offset: 0x00027C83
	public void OnPointerClick(PointerEventData eventData)
	{
		if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode && GameController.Instance.AmmoCardsInvisible())
		{
			GameController.Instance.ShowCombatCards();
		}
	}
}
