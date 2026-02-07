using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016D RID: 365
public class ToggleChangeFontOnValueChanged : MonoBehaviour
{
	// Token: 0x06000A77 RID: 2679 RVA: 0x0002F269 File Offset: 0x0002D469
	private void Awake()
	{
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		this.OnToggleValueChanged(this.toggle.isOn);
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0002F298 File Offset: 0x0002D498
	private void OnToggleValueChanged(bool toggleValue)
	{
		this.text.font = (toggleValue ? this.isOnFont : this.isOffFont);
	}

	// Token: 0x04000929 RID: 2345
	[SerializeField]
	private Toggle toggle;

	// Token: 0x0400092A RID: 2346
	[SerializeField]
	private TMP_Text text;

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	private TMP_FontAsset isOnFont;

	// Token: 0x0400092C RID: 2348
	[SerializeField]
	private TMP_FontAsset isOffFont;
}
