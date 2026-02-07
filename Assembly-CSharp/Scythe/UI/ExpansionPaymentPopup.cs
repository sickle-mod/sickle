using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004E3 RID: 1251
	public class ExpansionPaymentPopup : MonoBehaviour
	{
		// Token: 0x06002820 RID: 10272 RVA: 0x00041D47 File Offset: 0x0003FF47
		public void Show()
		{
			this.restoreButton.gameObject.SetActive(PlatformManager.IsIOS);
			base.gameObject.SetActive(true);
			this.UpdateCost();
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000EA39C File Offset: 0x000E859C
		private void UpdateCost()
		{
			string text = PaymentServiceController.Instance.DLCId(this.dlc);
			string text2 = PaymentServiceController.Instance.ProductPrice(text);
			string text3 = ScriptLocalization.Get("MainMenu/Buy");
			string text4 = string.Format("{0} {1}", text3, text2);
			this.buyButtonText.text = text4;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x00041D70 File Offset: 0x0003FF70
		private bool HasInternetConnection()
		{
			return Application.internetReachability > NetworkReachability.NotReachable;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000EA3EC File Offset: 0x000E85EC
		public void BuyExpansionButton_OnClick()
		{
			string text = PaymentServiceController.Instance.DLCId(this.dlc);
			if (this.HasInternetConnection())
			{
				this.paymentsController.BuyProduct(text);
				return;
			}
			this.paymentsController.ShowNoInternetConnectionPopup();
			this.Hide();
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x00041D7A File Offset: 0x0003FF7A
		public void RestorePurchasesButton_OnClick()
		{
			this.paymentsController.RestorePurchases();
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x00041D87 File Offset: 0x0003FF87
		public void CloseButton_OnClick()
		{
			this.paymentsController.Hide();
			this.Hide();
		}

		// Token: 0x04001CCE RID: 7374
		private const string BUY_TERM = "MainMenu/Buy";

		// Token: 0x04001CCF RID: 7375
		[SerializeField]
		private Button buyButton;

		// Token: 0x04001CD0 RID: 7376
		[SerializeField]
		private Button restoreButton;

		// Token: 0x04001CD1 RID: 7377
		[SerializeField]
		private TextMeshProUGUI buyButtonText;

		// Token: 0x04001CD2 RID: 7378
		[SerializeField]
		private DLCs dlc;

		// Token: 0x04001CD3 RID: 7379
		[SerializeField]
		private GameObject noInternetConnectionPopup;

		// Token: 0x04001CD4 RID: 7380
		[SerializeField]
		private ExpansionPaymentPopupsController paymentsController;
	}
}
