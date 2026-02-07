using System;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004DA RID: 1242
	public class ChangePasswordSuccess : MonoBehaviour
	{
		// Token: 0x06002780 RID: 10112 RVA: 0x000E8890 File Offset: 0x000E6A90
		private void OnEnable()
		{
			this.okButton.onClick.AddListener(new UnityAction(this.CloseWindow));
			this.supportButton.onClick.AddListener(new UnityAction(this.Support));
			if (!PlatformManager.IsMobile)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.CloseWindow));
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000E88F8 File Offset: 0x000E6AF8
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.CloseWindow));
			this.supportButton.onClick.RemoveListener(new UnityAction(this.Support));
			if (!PlatformManager.IsMobile)
			{
				this.closeButton.onClick.RemoveListener(new UnityAction(this.CloseWindow));
			}
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x00029172 File Offset: 0x00027372
		private void CloseWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x00040EC1 File Offset: 0x0003F0C1
		private void Support()
		{
			SingletonMono<MainMenu>.Instance.SupportPanelOpen();
		}

		// Token: 0x04001C5A RID: 7258
		[SerializeField]
		private Button closeButton;

		// Token: 0x04001C5B RID: 7259
		[SerializeField]
		private Button okButton;

		// Token: 0x04001C5C RID: 7260
		[SerializeField]
		private Button supportButton;
	}
}
