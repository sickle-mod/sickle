using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200040C RID: 1036
	public class YesNoDialog : MonoBehaviour
	{
		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06001FB7 RID: 8119 RVA: 0x000C2164 File Offset: 0x000C0364
		// (remove) Token: 0x06001FB8 RID: 8120 RVA: 0x000C219C File Offset: 0x000C039C
		public event YesNoDialog.OnClick OnYesClick;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06001FB9 RID: 8121 RVA: 0x000C21D4 File Offset: 0x000C03D4
		// (remove) Token: 0x06001FBA RID: 8122 RVA: 0x000C220C File Offset: 0x000C040C
		public event YesNoDialog.OnClick OnNoClick;

		// Token: 0x06001FBB RID: 8123 RVA: 0x0003C5B8 File Offset: 0x0003A7B8
		private void OnEnable()
		{
			GameController.OnEndTurnClick += this.OffDialogObject;
			if (PlatformManager.IsStandalone)
			{
				KeyboardShortcuts.Instance.isYesNoButtonVisible = true;
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0003C5DD File Offset: 0x0003A7DD
		private void OnDisable()
		{
			GameController.OnEndTurnClick -= this.OffDialogObject;
			if (PlatformManager.IsStandalone)
			{
				KeyboardShortcuts.Instance.isYesNoButtonVisible = false;
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00029172 File Offset: 0x00027372
		private void OffDialogObject()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0003C602 File Offset: 0x0003A802
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.SetResult(true);
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.SetResult(false);
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x000C2244 File Offset: 0x000C0444
		public void SetResult(bool value)
		{
			if (this.darken != null)
			{
				this.darken.enabled = false;
			}
			if (value)
			{
				if (this.OnYesClick != null)
				{
					this.OnYesClick();
				}
			}
			else if (this.OnNoClick != null)
			{
				this.OnNoClick();
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.HideCoroutine());
			}
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0003C624 File Offset: 0x0003A824
		private IEnumerator HideCoroutine()
		{
			yield return new WaitForEndOfFrame();
			this.Hide();
			yield break;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0003C633 File Offset: 0x0003A833
		public void Hide()
		{
			if (this.darken != null)
			{
				this.darken.enabled = false;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000C22B8 File Offset: 0x000C04B8
		public void Show(Sprite logo, YesNoDialog.OnClick OnYesClick, YesNoDialog.OnClick OnNoClick = null)
		{
			this.OnYesClick = OnYesClick;
			this.OnNoClick = OnNoClick;
			if (this.logo != null)
			{
				this.logo.sprite = logo;
			}
			if (this.darken != null)
			{
				this.darken.enabled = true;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0003C65B File Offset: 0x0003A85B
		public GameObject GetTitle()
		{
			return base.transform.GetChild(0).GetChild(0).gameObject;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0003C674 File Offset: 0x0003A874
		public GameObject GetFirstDescriptionLine()
		{
			return base.transform.GetChild(0).GetChild(1).GetChild(0)
				.gameObject;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0003C693 File Offset: 0x0003A893
		public GameObject GetSecondDescriptionLine()
		{
			return base.transform.GetChild(0).GetChild(1).GetChild(1)
				.gameObject;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0003C6B2 File Offset: 0x0003A8B2
		public GameObject GetYesButton()
		{
			return base.transform.GetChild(0).GetChild(2).gameObject;
		}

		// Token: 0x04001646 RID: 5702
		public Image logo;

		// Token: 0x04001647 RID: 5703
		public Image darken;

		// Token: 0x0200040D RID: 1037
		// (Invoke) Token: 0x06001FC9 RID: 8137
		public delegate void OnClick();
	}
}
