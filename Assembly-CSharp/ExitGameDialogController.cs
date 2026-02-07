using System;
using I2.Loc;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D5 RID: 213
public class ExitGameDialogController : MonoBehaviour
{
	// Token: 0x06000644 RID: 1604 RVA: 0x0002BEA8 File Offset: 0x0002A0A8
	private void OnEnable()
	{
		OptionsManager.OnLanguageChanged += this.Localize;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002BEBB File Offset: 0x0002A0BB
	private void OnDisable()
	{
		OptionsManager.OnLanguageChanged -= this.Localize;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0002BECE File Offset: 0x0002A0CE
	public void Init()
	{
		this.verticalLayoutRectTransform = base.GetComponentInChildren<VerticalLayoutGroup>().GetComponent<RectTransform>();
		this.Localize();
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0006F6D8 File Offset: 0x0006D8D8
	private void Localize()
	{
		if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsAsynchronous && !GameController.GameManager.SpectatorMode)
		{
			if (GameController.GameManager.IsRanked)
			{
				this.exitConfirmationText.text = string.Concat(new string[]
				{
					ScriptLocalization.Get("GameScene/ExitGameQuestion"),
					" ",
					ScriptLocalization.Get("GameScene/ExitGameReconnectWarning"),
					" ",
					ScriptLocalization.Get("GameScene/ExitGamePenaltyWarning")
				});
			}
			else
			{
				this.exitConfirmationText.text = ScriptLocalization.Get("GameScene/ExitGameQuestion") + " " + ScriptLocalization.Get("GameScene/ExitGameReconnectWarning");
			}
		}
		else
		{
			this.exitConfirmationText.text = ScriptLocalization.Get("GameScene/ExitGameQuestion");
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.verticalLayoutRectTransform);
	}

	// Token: 0x04000561 RID: 1377
	[SerializeField]
	private Text exitConfirmationText;

	// Token: 0x04000562 RID: 1378
	private RectTransform verticalLayoutRectTransform;
}
