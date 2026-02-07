using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Reworked.Menu
{
	// Token: 0x02000192 RID: 402
	public class AccountMigrationInfoPopup : MonoBehaviour
	{
		// Token: 0x06000BCD RID: 3021 RVA: 0x0002FF27 File Offset: 0x0002E127
		private void Start()
		{
			this.okButton.onClick.AddListener(new UnityAction(this.OnOkButtonClick));
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002D68D File Offset: 0x0002B88D
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00029172 File Offset: 0x00027372
		private void OnOkButtonClick()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400098B RID: 2443
		[SerializeField]
		private Button okButton;
	}
}
