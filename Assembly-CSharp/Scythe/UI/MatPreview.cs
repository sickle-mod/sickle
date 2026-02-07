using System;
using DG.Tweening;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000485 RID: 1157
	public class MatPreview : MonoBehaviour
	{
		// Token: 0x060024B6 RID: 9398 RVA: 0x000D9DFC File Offset: 0x000D7FFC
		public void Visibility(bool show, Player player)
		{
			if (player != null)
			{
				if (!show)
				{
					GameObject[] array = this.myUI;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(true);
					}
					if (PlatformManager.IsStandalone)
					{
						this.hideSequence = DOTween.Sequence();
						this.hideSequence.Append(this.canvasGroup.DOFade(0f, this.matFadeTime)).AppendCallback(delegate
						{
							base.gameObject.SetActive(false);
						});
						return;
					}
				}
				else
				{
					if (this.hideSequence != null)
					{
						this.hideSequence.Kill(false);
						this.hideSequence = null;
					}
					if (PlatformManager.IsStandalone)
					{
						this.canvasGroup.DOFade(1f, this.matFadeTime);
					}
					base.gameObject.SetActive(true);
					GameObject[] array = this.myUI;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(false);
					}
					this.matPlayer.UpdateMat(player, false);
					this.matPlayer.UpdateRecruits(player);
				}
			}
		}

		// Token: 0x040019B6 RID: 6582
		public GameObject[] myUI;

		// Token: 0x040019B7 RID: 6583
		public MatPlayerPresenter matPlayer;

		// Token: 0x040019B8 RID: 6584
		public CanvasGroup canvasGroup;

		// Token: 0x040019B9 RID: 6585
		public float matFadeTime;

		// Token: 0x040019BA RID: 6586
		private Sequence hideSequence;
	}
}
