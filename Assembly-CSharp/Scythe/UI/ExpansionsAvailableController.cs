using System;
using Scythe.Multiplayer;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004E6 RID: 1254
	public class ExpansionsAvailableController : SingletonMono<ExpansionsAvailableController>
	{
		// Token: 0x0600283C RID: 10300 RVA: 0x00041F5B File Offset: 0x0004015B
		private void OnEnable()
		{
			PaymentServiceController.Instance.ProductUnlocked += this.OnProductUnlocked;
			PaymentServiceController.Instance.ProductsRestored += this.OnRestorePurchaseUnlocked;
			this.ShowAvailableExpansions();
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00041F8F File Offset: 0x0004018F
		private void OnDisable()
		{
			PaymentServiceController.Instance.ProductUnlocked -= this.OnProductUnlocked;
			PaymentServiceController.Instance.ProductsRestored -= this.OnRestorePurchaseUnlocked;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000EA550 File Offset: 0x000E8750
		public void ShowAvailableExpansions()
		{
			this.ClearAvailableExpansions();
			if (!GameServiceController.Instance.InvadersFromAfarUnlocked() && (PlatformManager.IsStandalone || (PlatformManager.IsMobile && GameServiceController.Instance.IsPlayerSignedIn())))
			{
				this.ShowAvailableExpansion(DLCs.InvadersFromAfar);
			}
			if (DateTime.Today < this.transferDeadline && !Singleton<LoginController>.Instance.IsPlayerLoggedIn)
			{
				this.ShowTransferNowInfoPanel();
			}
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000EA5B4 File Offset: 0x000E87B4
		private void ClearAvailableExpansions()
		{
			foreach (object obj in this.expansionsLayout.transform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x00041FBD File Offset: 0x000401BD
		private void ShowAvailableExpansion(DLCs dlc)
		{
			global::UnityEngine.Object.Instantiate<ExpansionAvailablePopup>(this.popupPrefab, this.expansionsLayout.transform).Activate(dlc, this.paymentController);
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x00041FE1 File Offset: 0x000401E1
		private void ShowTransferNowInfoPanel()
		{
			global::UnityEngine.Object.Instantiate<TransferNowInfo>(this.transferNowInfoPrefab, this.expansionsLayout.transform);
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x00041FFA File Offset: 0x000401FA
		private void OnProductUnlocked(string productId)
		{
			this.ShowAvailableExpansions();
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x00041FFA File Offset: 0x000401FA
		private void OnRestorePurchaseUnlocked()
		{
			this.ShowAvailableExpansions();
		}

		// Token: 0x04001CDC RID: 7388
		[SerializeField]
		private ExpansionAvailablePopup popupPrefab;

		// Token: 0x04001CDD RID: 7389
		[SerializeField]
		private LayoutGroup expansionsLayout;

		// Token: 0x04001CDE RID: 7390
		[SerializeField]
		private TransferNowInfo transferNowInfoPrefab;

		// Token: 0x04001CDF RID: 7391
		[SerializeField]
		private ExpansionPaymentPopupsController paymentController;

		// Token: 0x04001CE0 RID: 7392
		private DateTime transferDeadline = new DateTime(2023, 1, 15);
	}
}
