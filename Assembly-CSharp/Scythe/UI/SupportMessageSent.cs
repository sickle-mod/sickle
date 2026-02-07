using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004F0 RID: 1264
	public class SupportMessageSent : MonoBehaviour
	{
		// Token: 0x06002897 RID: 10391 RVA: 0x000EAFB4 File Offset: 0x000E91B4
		private void OnEnable()
		{
			this.okButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000EB000 File Offset: 0x000E9200
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			if (PlatformManager.IsStandalone)
			{
				this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x00029172 File Offset: 0x00027372
		private void ExitWindow()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001D17 RID: 7447
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001D18 RID: 7448
		[SerializeField]
		private Button okButton;
	}
}
