using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003A RID: 58
public class InvitationGameRoomSlotState : GameRoomSlotState
{
	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060001BA RID: 442 RVA: 0x00059E0C File Offset: 0x0005800C
	// (remove) Token: 0x060001BB RID: 443 RVA: 0x00059E44 File Offset: 0x00058044
	public event Action OnAddBotButtonClick;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x060001BC RID: 444 RVA: 0x00059E7C File Offset: 0x0005807C
	// (remove) Token: 0x060001BD RID: 445 RVA: 0x00059EB4 File Offset: 0x000580B4
	public event Action OnInvitePlayerButtonClick;

	// Token: 0x060001BE RID: 446 RVA: 0x00028EC4 File Offset: 0x000270C4
	public void AddBotButtonClick()
	{
		UniversalInvocator.Event_Invocator(this.OnAddBotButtonClick);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00028ED1 File Offset: 0x000270D1
	public void InvitePlayerButtonClick()
	{
		UniversalInvocator.Event_Invocator(this.OnInvitePlayerButtonClick);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00028EDE File Offset: 0x000270DE
	public void SetInteractableAddBotButton(bool value)
	{
		if (this.addBotButton)
		{
			this.addBotButton.interactable = value;
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00028EF9 File Offset: 0x000270F9
	public void SetInteractableInvitePlayerButton(bool value)
	{
		if (this.invitePlayerButton)
		{
			this.invitePlayerButton.interactable = value;
		}
	}

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private Button addBotButton;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	private Button invitePlayerButton;
}
