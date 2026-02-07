using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class IFAComingSoonOnMobilePopup : MonoBehaviour
{
	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000219 RID: 537 RVA: 0x00029269 File Offset: 0x00027469
	// (set) Token: 0x0600021A RID: 538 RVA: 0x00029270 File Offset: 0x00027470
	public static IFAComingSoonOnMobilePopup Instance { get; private set; }

	// Token: 0x0600021B RID: 539 RVA: 0x00029278 File Offset: 0x00027478
	private void OnEnable()
	{
		if (IFAComingSoonOnMobilePopup.Instance == null)
		{
			IFAComingSoonOnMobilePopup.Instance = this;
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0002928D File Offset: 0x0002748D
	private void OnDisable()
	{
		if (IFAComingSoonOnMobilePopup.Instance == this)
		{
			IFAComingSoonOnMobilePopup.Instance = null;
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x000292A2 File Offset: 0x000274A2
	private void Show()
	{
		this.popupObject.SetActive(true);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000292B0 File Offset: 0x000274B0
	private void Hide()
	{
		this.popupObject.SetActive(false);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x000292BE File Offset: 0x000274BE
	public void Show_OnClick()
	{
		this.Show();
	}

	// Token: 0x06000220 RID: 544 RVA: 0x000292C6 File Offset: 0x000274C6
	public void Hide_OnClick()
	{
		this.Hide();
	}

	// Token: 0x04000191 RID: 401
	[SerializeField]
	private GameObject popupObject;
}
