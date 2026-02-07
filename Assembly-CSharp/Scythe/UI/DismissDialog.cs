using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003DD RID: 989
	public class DismissDialog : MonoBehaviour
	{
		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06001D58 RID: 7512 RVA: 0x000B6814 File Offset: 0x000B4A14
		// (remove) Token: 0x06001D59 RID: 7513 RVA: 0x000B684C File Offset: 0x000B4A4C
		public event DismissDialog.OnClick OnDismissClick;

		// Token: 0x06001D5A RID: 7514 RVA: 0x0003B0C5 File Offset: 0x000392C5
		private void OnEnable()
		{
			GameController.OnEndTurnClick += this.OffDialogObject;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0003B0D8 File Offset: 0x000392D8
		private void OnDisable()
		{
			GameController.OnEndTurnClick -= this.OffDialogObject;
			this.confirmDialog.gameObject.SetActive(false);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x00029172 File Offset: 0x00027372
		private void OffDialogObject()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0003B0FC File Offset: 0x000392FC
		public void OnCloseButtonClicked()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.Hide());
			}
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0003B118 File Offset: 0x00039318
		public void SetResult()
		{
			this.confirmDialog.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
			{
				this.OnDismissClick();
				if (base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.Hide());
				}
			}, null);
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0003B155 File Offset: 0x00039355
		public IEnumerator Hide()
		{
			yield return new WaitForEndOfFrame();
			if (this.darken != null)
			{
				this.darken.enabled = false;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000B6884 File Offset: 0x000B4A84
		public void Show(Sprite logo, DismissDialog.OnClick OnDismissClick)
		{
			this.OnDismissClick = OnDismissClick;
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

		// Token: 0x04001516 RID: 5398
		public Image logo;

		// Token: 0x04001517 RID: 5399
		public Image darken;

		// Token: 0x04001519 RID: 5401
		public YesNoDialog confirmDialog;

		// Token: 0x020003DE RID: 990
		// (Invoke) Token: 0x06001D64 RID: 7524
		public delegate void OnClick();
	}
}
