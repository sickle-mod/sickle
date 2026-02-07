using System;
using DG.Tweening;
using UnityEngine;

namespace Reworked.Main.UI
{
	// Token: 0x02000194 RID: 404
	public class PulseAnimation : MonoBehaviour
	{
		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002FF70 File Offset: 0x0002E170
		private void Awake()
		{
			this.defaultScale = base.transform.localScale;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0007FCD0 File Offset: 0x0007DED0
		public void StartPlaying()
		{
			this.animationSequence = DOTween.Sequence();
			this.animationSequence.Append(base.transform.DOScale(this.defaultScale + this.scaleOffset, this.time).SetEase(Ease.InOutSine));
			this.animationSequence.Append(base.transform.DOScale(this.defaultScale, this.time).SetEase(Ease.InOutSine));
			this.animationSequence.SetLoops(-1);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002FF83 File Offset: 0x0002E183
		public void StopPlaying()
		{
			this.animationSequence.Kill(true);
		}

		// Token: 0x0400098E RID: 2446
		[SerializeField]
		private Vector3 scaleOffset = Vector3.zero;

		// Token: 0x0400098F RID: 2447
		[SerializeField]
		private float time = 4f;

		// Token: 0x04000990 RID: 2448
		private Vector3 defaultScale = Vector3.one;

		// Token: 0x04000991 RID: 2449
		private Sequence animationSequence;
	}
}
