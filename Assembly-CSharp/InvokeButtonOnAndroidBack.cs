using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F6 RID: 246
public class InvokeButtonOnAndroidBack : MonoBehaviour
{
	// Token: 0x06000809 RID: 2057 RVA: 0x0002D5B7 File Offset: 0x0002B7B7
	private void OnEnable()
	{
		if (PlatformManager.IsAndroid)
		{
			AndroidBackButtonChecker.AddButtonToList(this);
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x0002D5C6 File Offset: 0x0002B7C6
	private void OnDisable()
	{
		if (PlatformManager.IsAndroid)
		{
			AndroidBackButtonChecker.RemoveButtonFromList(this);
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0002D5D5 File Offset: 0x0002B7D5
	public void ClickButton()
	{
		this.button.onClick.Invoke();
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0002D5E7 File Offset: 0x0002B7E7
	private void Reset()
	{
		if (this.button == null)
		{
			this.button = base.gameObject.GetComponent<Button>();
		}
	}

	// Token: 0x040006D0 RID: 1744
	[SerializeField]
	private Button button;
}
