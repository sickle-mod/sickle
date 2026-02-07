using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000502 RID: 1282
	public class HighlightedUIAnimation : MonoBehaviour
	{
		// Token: 0x060028FE RID: 10494 RVA: 0x00042A78 File Offset: 0x00040C78
		private IEnumerator Start()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.image = base.GetComponent<Image>();
			yield return null;
			this.Animate();
			yield break;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x00042A87 File Offset: 0x00040C87
		private void OnDestroy()
		{
			DOTween.KillAll(this);
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000EBBA0 File Offset: 0x000E9DA0
		private void Animate()
		{
			Rect rect = this.rectTransform.rect;
			float num = 1f + (float)this.enlargeRadius * 2f / Mathf.Sqrt(rect.width * rect.width + rect.height * rect.height);
			this.rectTransform.DOScale(num, this.duration).SetLoops(-1, LoopType.Yoyo);
			this.image.DOFade(1f, this.duration).SetLoops(-1, LoopType.Yoyo);
		}

		// Token: 0x04001D62 RID: 7522
		[SerializeField]
		private float duration = 0.4f;

		// Token: 0x04001D63 RID: 7523
		[SerializeField]
		private int enlargeRadius = 5;

		// Token: 0x04001D64 RID: 7524
		private RectTransform rectTransform;

		// Token: 0x04001D65 RID: 7525
		private Image image;
	}
}
