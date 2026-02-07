using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000038 RID: 56
public class BotGameRoomSlotState : GameRoomSlotState
{
	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060001AB RID: 427 RVA: 0x00059C90 File Offset: 0x00057E90
	// (remove) Token: 0x060001AC RID: 428 RVA: 0x00059CC8 File Offset: 0x00057EC8
	public event Action<int, int> OnBotLevelButtonClick;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x060001AD RID: 429 RVA: 0x00059D00 File Offset: 0x00057F00
	// (remove) Token: 0x060001AE RID: 430 RVA: 0x00059D38 File Offset: 0x00057F38
	public event Action OnKickBotButtonClick;

	// Token: 0x060001AF RID: 431 RVA: 0x00059D70 File Offset: 0x00057F70
	public void BotLevelButtonClick()
	{
		int num = this.botLevel;
		int num2 = num + 1;
		if (num2 > 3)
		{
			num2 = 1;
		}
		UniversalInvocator.Event_Invocator<int, int>(this.OnBotLevelButtonClick, new object[] { num, num2 });
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00028DEF File Offset: 0x00026FEF
	public void KickBotButtonClick()
	{
		UniversalInvocator.Event_Invocator(this.OnKickBotButtonClick);
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00028DFC File Offset: 0x00026FFC
	public void SetBotName(string name)
	{
		if (this.botNameLabel)
		{
			this.botNameLabel.text = name;
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00028E17 File Offset: 0x00027017
	public void SetBotLevel(int level)
	{
		if (level >= 1 && level <= 3)
		{
			this.botLevel = level;
			this.botLevelButton.image.sprite = this.botLevelSprites[this.botLevel - 1];
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00059DB4 File Offset: 0x00057FB4
	public void SetInteractableBotLevelButton(bool value)
	{
		if (this.botLevelButton)
		{
			this.botLevelButton.interactable = value;
			if (this.botLevelLeftArrowImage)
			{
				this.botLevelLeftArrowImage.enabled = value;
			}
			if (this.botLevelRightArrowImage)
			{
				this.botLevelRightArrowImage.enabled = value;
			}
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00028E4B File Offset: 0x0002704B
	public void SetActivePendingPlate(bool value)
	{
		if (this.pendingPlate)
		{
			this.pendingPlate.SetActive(value);
		}
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00028E66 File Offset: 0x00027066
	public void SetActiveReadyPlate(bool value)
	{
		if (this.readyPlate)
		{
			this.readyPlate.SetActive(value);
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00028E81 File Offset: 0x00027081
	public void SetActiveKickBotButton(bool value)
	{
		if (this.kickBotButton)
		{
			this.kickBotButton.gameObject.SetActive(value);
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00028EA1 File Offset: 0x000270A1
	public void SetInteractableKickBotButton(bool value)
	{
		if (this.kickBotButton)
		{
			this.kickBotButton.interactable = value;
		}
	}

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private TextMeshProUGUI botNameLabel;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private Button botLevelButton;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private Image botLevelLeftArrowImage;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private Image botLevelRightArrowImage;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private int botLevel;

	// Token: 0x0400015C RID: 348
	[SerializeField]
	private List<Sprite> botLevelSprites;

	// Token: 0x0400015D RID: 349
	[SerializeField]
	private GameObject pendingPlate;

	// Token: 0x0400015E RID: 350
	[SerializeField]
	private GameObject readyPlate;

	// Token: 0x0400015F RID: 351
	[SerializeField]
	private Button kickBotButton;
}
