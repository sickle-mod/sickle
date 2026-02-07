using System;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004E4 RID: 1252
	public class ExpansionPaymentPopupsController : MonoBehaviour
	{
		// Token: 0x06002828 RID: 10280 RVA: 0x000EA430 File Offset: 0x000E8630
		private void OnEnable()
		{
			PaymentServiceController.Instance.ProductUnlocked += this.OnProductUnlocked;
			PaymentServiceController.Instance.ProductPurchasedFailed += this.OnPurchaseFailed;
			PaymentServiceController.Instance.ProductsRestored += this.OnProductsRestored;
			PaymentServiceController.Instance.ProductsRestoreFailed += this.OnProductsRestoreFailed;
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000EA498 File Offset: 0x000E8698
		private void OnDisable()
		{
			PaymentServiceController.Instance.ProductUnlocked -= this.OnProductUnlocked;
			PaymentServiceController.Instance.ProductPurchasedFailed -= this.OnPurchaseFailed;
			PaymentServiceController.Instance.ProductsRestored -= this.OnProductsRestored;
			PaymentServiceController.Instance.ProductsRestoreFailed -= this.OnProductsRestoreFailed;
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x00041D9A File Offset: 0x0003FF9A
		public void ActivatePopup(DLCs dlc)
		{
			base.gameObject.SetActive(true);
			if (PaymentServiceController.Instance.IsConnected())
			{
				this.ShowPopup(dlc);
				return;
			}
			this.ConnectToPaymentServiceAndShowPopup(dlc);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x00041DC3 File Offset: 0x0003FFC3
		public void BuyProduct(string productId)
		{
			this.loadingPanel.SetActive(true);
			PaymentServiceController.Instance.BuyProduct(productId);
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x00041DDC File Offset: 0x0003FFDC
		public void RestorePurchases()
		{
			this.loadingPanel.SetActive(true);
			PaymentServiceController.Instance.RestorePurchases();
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x00041DF4 File Offset: 0x0003FFF4
		public void HideAllPopups()
		{
			this.invadersFromAfarPopup.Hide();
			this.noConnectionPanel.gameObject.SetActive(false);
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x00041E12 File Offset: 0x00040012
		public void OnErrorPanelClosedButton_Clicked()
		{
			this.errorPanel.SetActive(false);
			this.Hide();
			this.HideAllPopups();
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x00041E2C File Offset: 0x0004002C
		public void OnNoConnectionPanelButton_Clicked()
		{
			this.HideAllPopups();
			this.Hide();
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000EA500 File Offset: 0x000E8700
		private void ConnectToPaymentServiceAndShowPopup(DLCs dlc)
		{
			this.loadingPanel.SetActive(true);
			PaymentServiceController.Instance.AuthenticatePaymentService(delegate
			{
				this.loadingPanel.SetActive(false);
				this.ShowPopup(dlc);
			}, new Action<string>(this.OnPurchaseFailed));
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x00041E3A File Offset: 0x0004003A
		private void ShowPopup(DLCs dlc)
		{
			this.GetPopupToActivate(dlc).Show();
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x00041E48 File Offset: 0x00040048
		public void ShowNoInternetConnectionPopup()
		{
			this.noConnectionPanel.gameObject.SetActive(true);
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x00041E5B File Offset: 0x0004005B
		private void OnProductUnlocked(string productId)
		{
			this.loadingPanel.SetActive(false);
			Debug.Log("[ExpansionPaymentPopupsController] OnProductUnlocked");
			this.Hide();
			this.HideAllPopups();
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x00041E7F File Offset: 0x0004007F
		private void OnPurchaseFailed(string error)
		{
			this.errorPanel.SetActive(true);
			this.errorText.text = error;
			this.loadingPanel.SetActive(false);
			this.HideAllPopups();
			Debug.Log("[ExpansionPaymentPopupsController] OnPurchaseFailed");
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x00041EB5 File Offset: 0x000400B5
		private void OnProductsRestored()
		{
			this.loadingPanel.SetActive(false);
			Debug.Log("[ExpansionPaymentPopupsController] OnProductsRestored");
			this.Hide();
			this.HideAllPopups();
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x00041ED9 File Offset: 0x000400D9
		private void OnProductsRestoreFailed(string error)
		{
			this.loadingPanel.SetActive(false);
			this.errorPanel.SetActive(true);
			this.errorText.text = error;
			this.HideAllPopups();
			Debug.Log("[ExpansionPaymentPopupsController] OnProductsRestoreFailed");
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x00041F0F File Offset: 0x0004010F
		private ExpansionPaymentPopup GetPopupToActivate(DLCs dlc)
		{
			if (dlc == DLCs.InvadersFromAfar)
			{
				return this.invadersFromAfarPopup;
			}
			throw new ArgumentOutOfRangeException("Invalid dlc " + dlc.ToString());
		}

		// Token: 0x04001CD5 RID: 7381
		[SerializeField]
		private GameObject loadingPanel;

		// Token: 0x04001CD6 RID: 7382
		[SerializeField]
		private GameObject errorPanel;

		// Token: 0x04001CD7 RID: 7383
		[SerializeField]
		private GameObject noConnectionPanel;

		// Token: 0x04001CD8 RID: 7384
		[SerializeField]
		private TextMeshProUGUI errorText;

		// Token: 0x04001CD9 RID: 7385
		[SerializeField]
		private ExpansionPaymentPopup invadersFromAfarPopup;
	}
}
