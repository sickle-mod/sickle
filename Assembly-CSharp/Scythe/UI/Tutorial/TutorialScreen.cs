using System;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200051F RID: 1311
	public class TutorialScreen : MonoBehaviour
	{
		// Token: 0x060029D8 RID: 10712 RVA: 0x0004346E File Offset: 0x0004166E
		private void Awake()
		{
			this.nextButton = base.GetComponentInChildren<Button>();
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000EDA98 File Offset: 0x000EBC98
		public void Show(Action onNextButtonClicked = null)
		{
			base.gameObject.SetActive(true);
			if (this.nextButton != null && onNextButtonClicked != null)
			{
				this.onNextButtonClicked = onNextButtonClicked;
				SingletonMono<InputBlockerController>.Instance.UnblockUI(this);
				this.nextButton.onClick.RemoveAllListeners();
				this.nextButton.onClick.AddListener(delegate
				{
					this.OnButtonClicked();
				});
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x0004347C File Offset: 0x0004167C
		private void OnButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.onNextButtonClicked();
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x00043495 File Offset: 0x00041695
		public void Hide()
		{
			base.gameObject.SetActive(false);
			if (this.nextButton != null)
			{
				this.nextButton.onClick.RemoveAllListeners();
			}
		}

		// Token: 0x04001DBE RID: 7614
		private Button nextButton;

		// Token: 0x04001DBF RID: 7615
		private Action onNextButtonClicked;
	}
}
