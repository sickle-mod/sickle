using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Scythe.Multiplayer
{
	// Token: 0x02000228 RID: 552
	[RequireComponent(typeof(PlayerTimeScrollData))]
	[RequireComponent(typeof(ScrollSnap))]
	public class PlayerTimeScroll : MonoBehaviour
	{
		// Token: 0x0600105D RID: 4189 RVA: 0x00032AC9 File Offset: 0x00030CC9
		public void Activate(int currentValue)
		{
			this.startingAnimationDone = false;
			base.gameObject.SetActive(true);
			this.timeUnitText.gameObject.SetActive(true);
			if (!this.initDone)
			{
				this.Init();
			}
			this.currentPage = currentValue;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00032B04 File Offset: 0x00030D04
		public void OnStartingAnimationComplete()
		{
			this.startingAnimationDone = true;
			this.MoveToCurrentPage();
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00032B13 File Offset: 0x00030D13
		public void Deactivate()
		{
			this.timeUnitText.gameObject.SetActive(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00032B32 File Offset: 0x00030D32
		public int GetChosenValue()
		{
			if (base.gameObject.activeInHierarchy)
			{
				return this.playerTimeScrollData.GetValueAtIndex(this.currentPage);
			}
			return 0;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00032B54 File Offset: 0x00030D54
		public void MoveToCurrentPage()
		{
			this.SetPage(this.GetIndexOfValue(this.currentPage));
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00032B68 File Offset: 0x00030D68
		private void SetPage(int page)
		{
			this.scrollSnap.ChangePage(page);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00032B76 File Offset: 0x00030D76
		private int GetIndexOfValue(int value)
		{
			return this.playerTimeScrollData.GetIndexOfValue(value);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00032B84 File Offset: 0x00030D84
		private void Init()
		{
			this.playerTimeScrollData.GenerateDataIfNeeded(7);
			this.scrollSnap.ItemsVisibleAtOnce = 7;
			this.scrollSnap.onPageChange += this.ScrollSnap_OnPageChange;
			this.initDone = true;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00032BBC File Offset: 0x00030DBC
		private void ScrollSnap_OnPageChange(int page)
		{
			if (this.startingAnimationDone)
			{
				this.currentPage = page;
			}
		}

		// Token: 0x04000CAC RID: 3244
		[SerializeField]
		private const int ENTRIES_VISIBLE_AT_ONCE = 7;

		// Token: 0x04000CAD RID: 3245
		[SerializeField]
		private PlayerTimeScrollData playerTimeScrollData;

		// Token: 0x04000CAE RID: 3246
		[SerializeField]
		private ScrollSnap scrollSnap;

		// Token: 0x04000CAF RID: 3247
		[SerializeField]
		private TextMeshProUGUI timeUnitText;

		// Token: 0x04000CB0 RID: 3248
		private int currentPage;

		// Token: 0x04000CB1 RID: 3249
		private bool initDone;

		// Token: 0x04000CB2 RID: 3250
		private bool startingAnimationDone;
	}
}
