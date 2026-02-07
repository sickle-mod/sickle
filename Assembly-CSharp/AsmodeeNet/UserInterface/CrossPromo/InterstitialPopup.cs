using System;
using System.Collections;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200081B RID: 2075
	public class InterstitialPopup : BaseGroupOfProductPopup
	{
		// Token: 0x06003AEA RID: 15082 RVA: 0x0015204C File Offset: 0x0015024C
		public static InterstitialPopup InstantiateInterstitial()
		{
			InterstitialPopup interstitialPopup = (InterstitialPopup)global::UnityEngine.Object.FindObjectOfType(typeof(InterstitialPopup));
			if (interstitialPopup == null)
			{
				interstitialPopup = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.InterstitialPopupPrefab).GetComponent<InterstitialPopup>();
			}
			else
			{
				AsmoLogger.Error("InterstitialPopup", "Try to InstantiateInterstitial twice", null);
			}
			return interstitialPopup;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x001520A4 File Offset: 0x001502A4
		private void Start()
		{
			this._startTime = Time.time;
			this.skipButton.interactable = false;
			this.skipButton.gameObject.SetActive(false);
			this.spinner.SetActive(true);
			CrossPromoCacheManager.CancelLoadInterstitial();
			CrossPromoCacheManager.LoadInterstitial(delegate(ShowcaseProduct[] products)
			{
				this._products = products;
				AnalyticsEvents.LogCrossPromoDisplayedEvent(this._products, CROSSPROMO_DISPLAYED.crosspromo_type.interstitial, null);
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.interstitial, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.automatic, null, null, null);
				this.skipButton.gameObject.SetActive(true);
				this.spinner.SetActive(false);
				base.ReloadProducts(products);
				base.StartCoroutine(this._ActivateCloseButton());
			}, delegate
			{
				base.Dismiss();
			});
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0004E0FB File Offset: 0x0004C2FB
		public override void Dismiss()
		{
			if (Time.time - this._startTime < (float)this.delayBeforeSkip)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				return;
			}
			base.Dismiss();
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x0004E132 File Offset: 0x0004C332
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._ActivateCloseButton());
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x0004E147 File Offset: 0x0004C347
		protected override void OnDisable()
		{
			base.OnDisable();
			base.StopAllCoroutines();
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x0004E155 File Offset: 0x0004C355
		private IEnumerator _ActivateCloseButton()
		{
			while (Time.time - this._startTime < (float)this.delayBeforeSkip)
			{
				this.textSkipButton.text = string.Format("{0} ({1})", CoreApplication.Instance.LocalizationManager.GetLocalizedText("CrossPromo.close"), this.delayBeforeSkip - Mathf.FloorToInt(Time.time - this._startTime));
				yield return null;
			}
			this.skipButton.interactable = true;
			this.textSkipButton.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("CrossPromo.close");
			yield break;
		}

		// Token: 0x04002CAC RID: 11436
		public Button skipButton;

		// Token: 0x04002CAD RID: 11437
		public TextMeshProUGUI textSkipButton;

		// Token: 0x04002CAE RID: 11438
		public GameObject spinner;

		// Token: 0x04002CAF RID: 11439
		public int delayBeforeSkip = 20;

		// Token: 0x04002CB0 RID: 11440
		private float _startTime;

		// Token: 0x04002CB1 RID: 11441
		private const string _kModuleName = "InterstitialPopup";
	}
}
