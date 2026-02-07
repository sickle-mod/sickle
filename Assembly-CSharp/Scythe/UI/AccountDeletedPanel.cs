using System;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C7 RID: 1223
	public class AccountDeletedPanel : MonoBehaviour
	{
		// Token: 0x06002702 RID: 9986 RVA: 0x000E6D90 File Offset: 0x000E4F90
		private void OnEnable()
		{
			this.backToLoginButton.onClick.AddListener(new UnityAction(this.BackToLogin));
			if (this.supportButton != null)
			{
				this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			}
			if (!PlatformManager.IsMobile)
			{
				this.exitButton.onClick.AddListener(new UnityAction(this.BackToLogin));
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000E6E08 File Offset: 0x000E5008
		private void OnDisable()
		{
			this.backToLoginButton.onClick.RemoveListener(new UnityAction(this.BackToLogin));
			if (this.supportButton != null)
			{
				this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			}
			if (!PlatformManager.IsMobile)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.BackToLogin));
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x00029172 File Offset: 0x00027372
		private void BackToLogin()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001BE0 RID: 7136
		[SerializeField]
		private Button backToLoginButton;

		// Token: 0x04001BE1 RID: 7137
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001BE2 RID: 7138
		[SerializeField]
		private Button supportButton;
	}
}
