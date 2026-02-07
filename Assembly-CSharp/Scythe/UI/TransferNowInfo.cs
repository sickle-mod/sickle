using System;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004EC RID: 1260
	public class TransferNowInfo : MonoBehaviour
	{
		// Token: 0x06002877 RID: 10359 RVA: 0x00042362 File Offset: 0x00040562
		public void OnEnable()
		{
			this.transferNowButton.onClick.AddListener(new UnityAction(this.TransferNowClick));
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00042380 File Offset: 0x00040580
		private void OnDisable()
		{
			this.transferNowButton.onClick.RemoveListener(new UnityAction(this.TransferNowClick));
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x0004239E File Offset: 0x0004059E
		public void TransferNowClick()
		{
			SingletonMono<MainMenu>.Instance.LoginMenuParent = SingletonMono<MainMenu>.Instance.gameObject;
			SingletonMono<MainMenu>.Instance.EnableLoginPanel(true);
		}

		// Token: 0x04001CF8 RID: 7416
		[SerializeField]
		private Button transferNowButton;
	}
}
