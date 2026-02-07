using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004E2 RID: 1250
	public class ExpansionAvailablePopup : MonoBehaviour
	{
		// Token: 0x0600281A RID: 10266 RVA: 0x00041CB2 File Offset: 0x0003FEB2
		public void Activate(DLCs dlc, ExpansionPaymentPopupsController paymentController)
		{
			this.availableDLC = dlc;
			this.paymentController = paymentController;
			this.UpdateLocalization();
			OptionsManager.OnLanguageChanged += this.UpdateLocalization;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00041CD9 File Offset: 0x0003FED9
		private void OnDisable()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateLocalization;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00041CEC File Offset: 0x0003FEEC
		private void UpdateLocalization()
		{
			this.expansionAvailableText.text = this.GetExpansionAvailableText();
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x00041CFF File Offset: 0x0003FEFF
		private string GetExpansionAvailableText()
		{
			if (this.availableDLC == DLCs.InvadersFromAfar)
			{
				return ScriptLocalization.Get("MainMenu/InvadersFromAfarAvaliable");
			}
			throw new ArgumentException("Invalid expansion: " + this.availableDLC.ToString());
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x00041D34 File Offset: 0x0003FF34
		public void CheckNowButton_OnClick()
		{
			this.paymentController.ActivatePopup(this.availableDLC);
		}

		// Token: 0x04001CCA RID: 7370
		[SerializeField]
		private TextMeshProUGUI expansionAvailableText;

		// Token: 0x04001CCB RID: 7371
		[SerializeField]
		private Button checkNowButton;

		// Token: 0x04001CCC RID: 7372
		private ExpansionPaymentPopupsController paymentController;

		// Token: 0x04001CCD RID: 7373
		private DLCs availableDLC;
	}
}
