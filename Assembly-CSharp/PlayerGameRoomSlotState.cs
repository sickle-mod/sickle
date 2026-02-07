using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class PlayerGameRoomSlotState : GameRoomSlotState
{
	// Token: 0x14000017 RID: 23
	// (add) Token: 0x060001C3 RID: 451 RVA: 0x00059EEC File Offset: 0x000580EC
	// (remove) Token: 0x060001C4 RID: 452 RVA: 0x00059F24 File Offset: 0x00058124
	public event Action OnKickPlayerButtonClick;

	// Token: 0x060001C5 RID: 453 RVA: 0x00028F14 File Offset: 0x00027114
	public void KickPlayerButtonClick()
	{
		UniversalInvocator.Event_Invocator(this.OnKickPlayerButtonClick);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00028F21 File Offset: 0x00027121
	public void SetPlayerName(string name)
	{
		if (this.playerNameLabel)
		{
			this.playerNameLabel.text = name;
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00028F3C File Offset: 0x0002713C
	public void SetPlayerELO(int elo)
	{
		if (this.playerELOLabel)
		{
			this.playerELOLabel.text = elo.ToString() + " " + ScriptLocalization.Get("Lobby/Elo");
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00028F71 File Offset: 0x00027171
	public void SetActivePendingPlate(bool value)
	{
		if (this.pendingPlate)
		{
			this.pendingPlate.SetActive(value);
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00028F8C File Offset: 0x0002718C
	public void SetActiveReadyPlate(bool value)
	{
		if (this.readyPlate)
		{
			this.readyPlate.SetActive(value);
		}
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00028FA7 File Offset: 0x000271A7
	public void SetActiveKickPlayerButton(bool value)
	{
		if (this.kickPlayerButton)
		{
			this.kickPlayerButton.gameObject.SetActive(value);
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00028FC7 File Offset: 0x000271C7
	public void SetInteractableKickPlayerButton(bool value)
	{
		if (this.kickPlayerButton)
		{
			this.kickPlayerButton.interactable = value;
		}
	}

	// Token: 0x04000165 RID: 357
	private const string eloLocalizationTerm = "Lobby/Elo";

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private TextMeshProUGUI playerNameLabel;

	// Token: 0x04000167 RID: 359
	[SerializeField]
	private TextMeshProUGUI playerELOLabel;

	// Token: 0x04000168 RID: 360
	[SerializeField]
	private GameObject pendingPlate;

	// Token: 0x04000169 RID: 361
	[SerializeField]
	private GameObject readyPlate;

	// Token: 0x0400016A RID: 362
	[SerializeField]
	private Button kickPlayerButton;
}
