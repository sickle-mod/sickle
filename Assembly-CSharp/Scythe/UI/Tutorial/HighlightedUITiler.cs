using System;
using System.Collections;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000504 RID: 1284
	public class HighlightedUITiler : MonoBehaviour
	{
		// Token: 0x06002908 RID: 10504 RVA: 0x00042AC6 File Offset: 0x00040CC6
		private IEnumerator Start()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			yield return null;
			this.AdjustDimensions();
			yield break;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000EBC90 File Offset: 0x000E9E90
		private void AdjustDimensions()
		{
			Vector2 size = this.rectTransform.rect.size;
			this.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			this.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			this.rectTransform.anchoredPosition = Vector2.zero;
			size.x = this.CalculateDimension(size.x);
			size.y = this.CalculateDimension(size.y);
			this.rectTransform.sizeDelta = size;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000EBD28 File Offset: 0x000E9F28
		private float CalculateDimension(float startDimension)
		{
			startDimension -= 2f * (float)this.cornerSize;
			startDimension /= (float)this.tileSize;
			startDimension = (float)Mathf.RoundToInt(startDimension);
			startDimension *= (float)this.tileSize;
			startDimension += 2f * (float)this.cornerSize;
			return startDimension;
		}

		// Token: 0x04001D69 RID: 7529
		[SerializeField]
		private int cornerSize;

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		private int tileSize;

		// Token: 0x04001D6B RID: 7531
		private RectTransform rectTransform;
	}
}
