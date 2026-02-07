using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C9 RID: 1225
	public class AccountSuccessMigratedPanel : MonoBehaviour
	{
		// Token: 0x06002710 RID: 10000 RVA: 0x000E70EC File Offset: 0x000E52EC
		private void OnEnable()
		{
			this.okButton.onClick.AddListener(new UnityAction(this.CloseWindow));
			this.resendEmailButton.onClick.AddListener(new UnityAction(this.ResendMail));
			if (!PlatformManager.IsMobile)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.CloseWindow));
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000E7154 File Offset: 0x000E5354
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.CloseWindow));
			this.resendEmailButton.onClick.RemoveListener(new UnityAction(this.ResendMail));
			if (!PlatformManager.IsMobile)
			{
				this.closeButton.onClick.RemoveListener(new UnityAction(this.CloseWindow));
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00029172 File Offset: 0x00027372
		private void CloseWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void ResendMail()
		{
		}

		// Token: 0x04001BEC RID: 7148
		[SerializeField]
		private Button closeButton;

		// Token: 0x04001BED RID: 7149
		[SerializeField]
		private Button okButton;

		// Token: 0x04001BEE RID: 7150
		[SerializeField]
		private Button resendEmailButton;
	}
}
