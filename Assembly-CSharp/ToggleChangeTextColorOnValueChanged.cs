using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016F RID: 367
public class ToggleChangeTextColorOnValueChanged : MonoBehaviour
{
	// Token: 0x06000A7D RID: 2685 RVA: 0x0002F303 File Offset: 0x0002D503
	private void Awake()
	{
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		this.OnToggleValueChanged(this.toggle.isOn);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0002F332 File Offset: 0x0002D532
	private void OnToggleValueChanged(bool toggleValue)
	{
		this.text.color = (toggleValue ? this.isOnColor : this.isOffColor);
	}

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private Toggle toggle;

	// Token: 0x04000932 RID: 2354
	[SerializeField]
	private TMP_Text text;

	// Token: 0x04000933 RID: 2355
	[SerializeField]
	private Color isOnColor;

	// Token: 0x04000934 RID: 2356
	[SerializeField]
	private Color isOffColor;
}
