using System;
using DG.Tweening;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x0200046C RID: 1132
	public class BottomBar : SingletonMono<BottomBar>
	{
		// Token: 0x060023B7 RID: 9143 RVA: 0x000D38E8 File Offset: 0x000D1AE8
		private void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.originalPlayerMatAnchoredPosition = this.playerMat.anchoredPosition;
				this.originalRightPanelAnchoredPosition = this.rightPanel.anchoredPosition;
				this.originalBigButtonsAnchoredPosition = this.bigButtons.anchoredPosition;
				this.originalBackgroundAnchoredPosition = this.background.anchoredPosition;
				this.originalObjectivesPreviewAnchoredPosition = this.objectivesPreview.anchoredPosition;
				this.originalMatPreviewButtonAnchoredPosition = this.matPreviewButton.anchoredPosition;
			}
			this.AnimateToState(BottomBar.BottomBarState.Hidden, 0f);
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0003ED02 File Offset: 0x0003CF02
		private void OnEnable()
		{
			ActionManager.DisableActionInput += this.OnActionFinished;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0003ED15 File Offset: 0x0003CF15
		private void OnDisable()
		{
			ActionManager.DisableActionInput -= this.OnActionFinished;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000D3970 File Offset: 0x000D1B70
		public void AnimateToState(BottomBar.BottomBarState newState, float duration = 0.25f)
		{
			if (this.currentState == BottomBar.BottomBarState.HiddenForHelp && !this.restore)
			{
				return;
			}
			if (!PlatformManager.IsStandalone)
			{
				DOTween.Kill(this, false);
				this.previousState = this.currentState;
				this.currentState = newState;
				this.AnimatePlayerMat(duration);
				this.AnimateRightPanel(duration);
				this.AnimateBigButtons(duration);
				this.AnimateBackground(duration);
				this.AnimateObjectivesPreview(duration);
				this.AnimateMatPreviewButton(duration);
			}
			else
			{
				base.GetComponent<Animator>().SetTrigger(newState.ToString());
			}
			if (this.restore)
			{
				this.restore = false;
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0003ED28 File Offset: 0x0003CF28
		public void RestorePreviousState()
		{
			this.restore = true;
			this.AnimateToState(this.previousState, 0.25f);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000D3A04 File Offset: 0x000D1C04
		private void AnimatePlayerMat(float duration)
		{
			if (this.currentState == BottomBar.BottomBarState.Expanded || this.currentState == BottomBar.BottomBarState.Peeked)
			{
				this.playerMat.DOScale(1.25f, duration).SetEase(Ease.InOutCubic);
			}
			else
			{
				this.playerMat.DOScale(1f, duration).SetEase(Ease.InOutCubic);
			}
			if (this.currentState == BottomBar.BottomBarState.Hidden || this.currentState == BottomBar.BottomBarState.HiddenForHelp)
			{
				this.playerMat.DOAnchorPos(new Vector2(this.originalPlayerMatAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			if (this.currentState == BottomBar.BottomBarState.Expanded)
			{
				this.playerMat.DOAnchorPos(new Vector2(-400f, this.originalPlayerMatAnchoredPosition.y), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			if (this.currentState == BottomBar.BottomBarState.Peeked)
			{
				this.playerMat.DOAnchorPos(new Vector2(-285.24f, this.originalPlayerMatAnchoredPosition.y), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.playerMat.DOAnchorPos(this.originalPlayerMatAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000D3B18 File Offset: 0x000D1D18
		private void AnimateRightPanel(float duration)
		{
			if (this.currentState == BottomBar.BottomBarState.Hidden || this.currentState == BottomBar.BottomBarState.Expanded || this.currentState == BottomBar.BottomBarState.Peeked)
			{
				this.rightPanel.DOAnchorPos(new Vector2(this.originalRightPanelAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.rightPanel.DOAnchorPos(this.originalRightPanelAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000D3B88 File Offset: 0x000D1D88
		private void AnimateBigButtons(float duration)
		{
			if (this.currentState == BottomBar.BottomBarState.Hidden || this.currentState == BottomBar.BottomBarState.Expanded || this.currentState == BottomBar.BottomBarState.Peeked)
			{
				this.bigButtons.DOAnchorPos(new Vector2(this.originalBigButtonsAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.bigButtons.DOAnchorPos(this.originalBigButtonsAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000D3BF8 File Offset: 0x000D1DF8
		private void AnimateBackground(float duration)
		{
			if (this.currentState == BottomBar.BottomBarState.Expanded)
			{
				this.background.DOAnchorPos(new Vector2(-1000f, this.originalBackgroundAnchoredPosition.y), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			if (this.currentState == BottomBar.BottomBarState.Hidden || this.currentState == BottomBar.BottomBarState.HiddenForHelp || this.currentState == BottomBar.BottomBarState.Peeked)
			{
				this.background.DOAnchorPos(new Vector2(this.originalBackgroundAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.background.DOAnchorPos(this.originalBackgroundAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000D3C9C File Offset: 0x000D1E9C
		private void AnimateObjectivesPreview(float duration)
		{
			if (this.currentState != BottomBar.BottomBarState.Visible && this.currentState != BottomBar.BottomBarState.HiddenForHelp)
			{
				this.objectivesPreview.DOAnchorPos(new Vector2(this.originalObjectivesPreviewAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.objectivesPreview.DOAnchorPos(this.originalObjectivesPreviewAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000D3D04 File Offset: 0x000D1F04
		private void AnimateMatPreviewButton(float duration)
		{
			if (this.currentState == BottomBar.BottomBarState.Hidden || this.currentState == BottomBar.BottomBarState.Peeked || this.currentState == BottomBar.BottomBarState.HiddenForHelp)
			{
				this.matPreviewButton.DOAnchorPos(this.originalMatPreviewButtonAnchoredPosition, duration, false).SetEase(Ease.InOutCubic);
				return;
			}
			this.matPreviewButton.DOAnchorPos(new Vector2(this.originalMatPreviewButtonAnchoredPosition.x, -200f), duration, false).SetEase(Ease.InOutCubic);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0003ED42 File Offset: 0x0003CF42
		private void OnActionFinished()
		{
			this.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
		}

		// Token: 0x040018D3 RID: 6355
		private const float ANIMATION_DURATION = 0.25f;

		// Token: 0x040018D4 RID: 6356
		[SerializeField]
		private RectTransform playerMat;

		// Token: 0x040018D5 RID: 6357
		[SerializeField]
		private RectTransform rightPanel;

		// Token: 0x040018D6 RID: 6358
		[SerializeField]
		private RectTransform bigButtons;

		// Token: 0x040018D7 RID: 6359
		[SerializeField]
		private RectTransform background;

		// Token: 0x040018D8 RID: 6360
		[SerializeField]
		private RectTransform objectivesPreview;

		// Token: 0x040018D9 RID: 6361
		[SerializeField]
		private RectTransform matPreviewButton;

		// Token: 0x040018DA RID: 6362
		private BottomBar.BottomBarState previousState;

		// Token: 0x040018DB RID: 6363
		private BottomBar.BottomBarState currentState;

		// Token: 0x040018DC RID: 6364
		private bool restore;

		// Token: 0x040018DD RID: 6365
		private Vector2 originalPlayerMatAnchoredPosition;

		// Token: 0x040018DE RID: 6366
		private Vector2 originalRightPanelAnchoredPosition;

		// Token: 0x040018DF RID: 6367
		private Vector2 originalBigButtonsAnchoredPosition;

		// Token: 0x040018E0 RID: 6368
		private Vector2 originalBackgroundAnchoredPosition;

		// Token: 0x040018E1 RID: 6369
		private Vector2 originalObjectivesPreviewAnchoredPosition;

		// Token: 0x040018E2 RID: 6370
		private Vector2 originalMatPreviewButtonAnchoredPosition;

		// Token: 0x0200046D RID: 1133
		public enum BottomBarState
		{
			// Token: 0x040018E4 RID: 6372
			Hidden,
			// Token: 0x040018E5 RID: 6373
			HiddenForHelp,
			// Token: 0x040018E6 RID: 6374
			Visible,
			// Token: 0x040018E7 RID: 6375
			Expanded,
			// Token: 0x040018E8 RID: 6376
			Peeked
		}
	}
}
