using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class ExitScreenPresenter : MonoBehaviour
{
	// Token: 0x060003DF RID: 991 RVA: 0x0002A6AC File Offset: 0x000288AC
	public void Show(bool leavingMainScene = true)
	{
		base.gameObject.SetActive(true);
		this.UpdateLeavingEuropiaTextVisibility(leavingMainScene);
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00029172 File Offset: 0x00027372
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0002A6C1 File Offset: 0x000288C1
	private void UpdateLeavingEuropiaTextVisibility(bool leavingMainScene)
	{
		this.leavingEuropiaText.SetActive(leavingMainScene);
	}

	// Token: 0x04000355 RID: 853
	[SerializeField]
	private GameObject leavingEuropiaText;
}
