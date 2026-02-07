using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class ToggleChangeSpriteOnValueChanged : MonoBehaviour
{
	// Token: 0x06000A7A RID: 2682 RVA: 0x0002F2B6 File Offset: 0x0002D4B6
	private void Awake()
	{
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		this.OnToggleValueChanged(this.toggle.isOn);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0002F2E5 File Offset: 0x0002D4E5
	private void OnToggleValueChanged(bool toggleValue)
	{
		this.image.sprite = (toggleValue ? this.isOnSprite : this.isOffSprite);
	}

	// Token: 0x0400092D RID: 2349
	[SerializeField]
	private Toggle toggle;

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private Image image;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private Sprite isOnSprite;

	// Token: 0x04000930 RID: 2352
	[SerializeField]
	private Sprite isOffSprite;
}
