using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class MatFlashAnimation : MonoBehaviour
{
	// Token: 0x060005D0 RID: 1488 RVA: 0x0006BBFC File Offset: 0x00069DFC
	private void OnEnable()
	{
		this.imageToAnimate.SetActive(true);
		if (this.enlistAnimation)
		{
			this.imageToAnimate.transform.DOLocalMoveY(-91.5f, this.animationDuration, false).OnComplete(new TweenCallback(this.FlashAnimationEnd));
			return;
		}
		this.imageToAnimate.transform.DOLocalMoveY(-34f, this.animationDuration, false).OnComplete(new TweenCallback(this.FlashAnimationEnd));
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0006BC7C File Offset: 0x00069E7C
	private void FlashAnimationEnd()
	{
		this.imageToAnimate.SetActive(false);
		if (this.enlistAnimation)
		{
			this.imageToAnimate.transform.localPosition = new Vector3(this.imageToAnimate.transform.localPosition.x, 95f);
			return;
		}
		this.imageToAnimate.transform.localPosition = new Vector3(this.imageToAnimate.transform.localPosition.x, 34f);
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0002B949 File Offset: 0x00029B49
	public float GetAnimationDuration()
	{
		return this.animationDuration;
	}

	// Token: 0x0400050A RID: 1290
	public GameObject imageToAnimate;

	// Token: 0x0400050B RID: 1291
	public bool enlistAnimation;

	// Token: 0x0400050C RID: 1292
	private float animationDuration = 0.75f;
}
