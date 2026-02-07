using System;
using Scythe.Multiplayer;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class TransferNowInfoController : MonoBehaviour
{
	// Token: 0x06000866 RID: 2150 RVA: 0x0002DA8A File Offset: 0x0002BC8A
	private void OnEnable()
	{
		this.ShowAvailableExpansions();
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0002DA92 File Offset: 0x0002BC92
	private void ShowAvailableExpansions()
	{
		if (DateTime.Today < this.transferDeadline && !Singleton<LoginController>.Instance.IsPlayerLoggedIn)
		{
			this.TransferInfoPanelActivate(true);
			return;
		}
		this.TransferInfoPanelActivate(false);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0002DAC1 File Offset: 0x0002BCC1
	private void TransferInfoPanelActivate(bool activate)
	{
		this.transferNowInfoPanel.gameObject.SetActive(activate);
	}

	// Token: 0x04000712 RID: 1810
	private DateTime transferDeadline = new DateTime(2023, 1, 15);

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private TransferNowInfo transferNowInfoPanel;
}
