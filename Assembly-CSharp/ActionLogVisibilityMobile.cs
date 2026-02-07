using System;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D3 RID: 211
public class ActionLogVisibilityMobile : SingletonMono<ActionLogVisibilityMobile>
{
	// Token: 0x0600063D RID: 1597 RVA: 0x0006F504 File Offset: 0x0006D704
	public void ActionLogActive(bool actionLogActive)
	{
		WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
		this.openActionLogButton.gameObject.SetActive(!actionLogActive);
		if (GameController.GameManager.IsMultiplayer)
		{
			this.openChatButton.gameObject.SetActive(!actionLogActive);
		}
		if (actionLogActive)
		{
			base.StartCoroutine(ActionLogPresenter.Instance.MoveLogToTheBottom());
		}
		this.actionLogUI.SetActive(actionLogActive);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0002BE84 File Offset: 0x0002A084
	public void OpenActionLogButtonActive(bool active)
	{
		this.openActionLogButton.gameObject.SetActive(active);
	}

	// Token: 0x0400055B RID: 1371
	[SerializeField]
	private Button openChatButton;

	// Token: 0x0400055C RID: 1372
	[SerializeField]
	private Button openActionLogButton;

	// Token: 0x0400055D RID: 1373
	[SerializeField]
	private GameObject actionLogUI;
}
