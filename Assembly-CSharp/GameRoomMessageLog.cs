using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class GameRoomMessageLog : MonoBehaviour
{
	// Token: 0x0600010E RID: 270 RVA: 0x000566E0 File Offset: 0x000548E0
	public void LogMessage(string user, string message)
	{
		TextMeshProUGUI textMeshProUGUI = global::UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.messagePrefab, this.content);
		this.messagesList.Add(textMeshProUGUI);
		textMeshProUGUI.text = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGB(this.userColor),
			">",
			user,
			":</color> ",
			message
		});
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00028A2A File Offset: 0x00026C2A
	public void LogState(string state)
	{
		this.StopClearStateCoroutineIfExists();
		this.stateLabel.text = state;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00028A3E File Offset: 0x00026C3E
	public void LogState(string state, float timeToClear)
	{
		this.LogState(state);
		this.ClearState(timeToClear);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00028A4E File Offset: 0x00026C4E
	public void ClearAll()
	{
		this.ClearMessages();
		this.ClearState();
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0005674C File Offset: 0x0005494C
	public void ClearMessages()
	{
		foreach (TextMeshProUGUI textMeshProUGUI in this.messagesList)
		{
			global::UnityEngine.Object.Destroy(textMeshProUGUI.gameObject);
		}
		this.messagesList.Clear();
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00028A5C File Offset: 0x00026C5C
	public void ClearState()
	{
		this.stateLabel.text = "";
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00028A6E File Offset: 0x00026C6E
	public void ClearState(float timeToClear)
	{
		this.StopClearStateCoroutineIfExists();
		this.clearStateCoroutine = this.ClearStateAfterDelay(timeToClear);
		base.StartCoroutine(this.clearStateCoroutine);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00028A90 File Offset: 0x00026C90
	private IEnumerator ClearStateAfterDelay(float timeToClear)
	{
		yield return new WaitForSeconds(timeToClear);
		this.ClearState();
		yield break;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00028AA6 File Offset: 0x00026CA6
	private void StopClearStateCoroutineIfExists()
	{
		if (this.clearStateCoroutine != null)
		{
			base.StopCoroutine(this.clearStateCoroutine);
			this.clearStateCoroutine = null;
		}
	}

	// Token: 0x04000111 RID: 273
	[SerializeField]
	private RectTransform content;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	private TextMeshProUGUI messagePrefab;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private TextMeshProUGUI stateLabel;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	private Color userColor;

	// Token: 0x04000115 RID: 277
	private List<TextMeshProUGUI> messagesList = new List<TextMeshProUGUI>();

	// Token: 0x04000116 RID: 278
	private IEnumerator clearStateCoroutine;
}
