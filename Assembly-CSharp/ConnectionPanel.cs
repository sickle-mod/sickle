using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class ConnectionPanel : MonoBehaviour
{
	// Token: 0x06000208 RID: 520 RVA: 0x0002920A File Offset: 0x0002740A
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00029218 File Offset: 0x00027418
	public void HideConnectionWindow()
	{
		base.StopCoroutine(this.AnimateDotText());
		this.SetActive(false);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0005A880 File Offset: 0x00058A80
	public void ShowConnectingWindow()
	{
		this.dotText.text = "";
		SceneController.Instance.EnableCanvas(true);
		this.SetActive(true);
		this.ChangeConnectingWindowText(ScriptLocalization.Get(LocalizationManager.GetRandomTextFromCategory("LoadingTexts")));
		base.StartCoroutine(this.AnimateDotText());
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0005A8D4 File Offset: 0x00058AD4
	public void ShowCallApiWindow()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.dotText.text = "";
			SceneController.Instance.EnableCanvas(true);
			this.SetActive(true);
			base.StartCoroutine(this.AnimateDotText());
		}
		this.ChangeConnectingWindowText(ScriptLocalization.Get(LocalizationManager.GetRandomTextFromCategory("LoadingTexts")));
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0002922D File Offset: 0x0002742D
	private void ChangeConnectingWindowText(string newText)
	{
		this.connectingText.text = newText;
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0002923B File Offset: 0x0002743B
	private IEnumerator AnimateDotText()
	{
		int currentDotsAmount = 0;
		while (base.gameObject.activeInHierarchy)
		{
			if (currentDotsAmount < 3)
			{
				int num = currentDotsAmount;
				currentDotsAmount = num + 1;
				TMP_Text tmp_Text = this.dotText;
				tmp_Text.text += ".";
			}
			else
			{
				currentDotsAmount = 0;
				this.dotText.text = string.Empty;
			}
			yield return new WaitForSeconds(0.4f);
		}
		yield break;
	}

	// Token: 0x04000188 RID: 392
	[SerializeField]
	private TMP_Text dotText;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	private TMP_Text connectingText;

	// Token: 0x0400018A RID: 394
	private const string LOADING_TEXTS_LOCALIZATION_CATEGORY = "LoadingTexts";
}
