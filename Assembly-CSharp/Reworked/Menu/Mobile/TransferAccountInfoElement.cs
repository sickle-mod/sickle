using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Reworked.Menu.Mobile
{
	// Token: 0x02000193 RID: 403
	public class TransferAccountInfoElement : MonoBehaviour
	{
		// Token: 0x06000BD1 RID: 3025 RVA: 0x0002FF45 File Offset: 0x0002E145
		private void Start()
		{
			this.moreInfoButton.onClick.AddListener(new UnityAction(this.OnMoreInfoButtonClick));
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002FF63 File Offset: 0x0002E163
		private void OnMoreInfoButtonClick()
		{
			this.infoPopup.Show();
		}

		// Token: 0x0400098C RID: 2444
		[SerializeField]
		private Button moreInfoButton;

		// Token: 0x0400098D RID: 2445
		[SerializeField]
		private AccountMigrationInfoPopup infoPopup;
	}
}
