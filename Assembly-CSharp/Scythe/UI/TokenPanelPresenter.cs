using System;
using System.Collections;
using DG.Tweening;
using Reworked.Main.DLC;
using Reworked.Main.UI;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x0200044B RID: 1099
	public class TokenPanelPresenter : MonoBehaviour
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x0003DE69 File Offset: 0x0003C069
		protected TokenManager tokenManager
		{
			get
			{
				return GameController.GameManager.tokenManager;
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0003DE75 File Offset: 0x0003C075
		protected void Start()
		{
			this.rotateTowardsCamera = base.GetComponent<RotateTowardsCamera>();
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000CCAD0 File Offset: 0x000CACD0
		protected virtual void OnEnable()
		{
			if (PlatformManager.IsStandalone)
			{
				TrapPointerEvents trapPointerEvents = this.tokenPanelEvents;
				trapPointerEvents.OnExit = (Action)Delegate.Combine(trapPointerEvents.OnExit, new Action(this.OnPointerExitPanel));
				TrapPointerEvents trapPointerEvents2 = this.tokenPanelTriggerEvents;
				trapPointerEvents2.OnEnter = (Action)Delegate.Combine(trapPointerEvents2.OnEnter, new Action(this.OnPointerEnterIcon));
				TrapPointerEvents trapPointerEvents3 = this.tokenPanelTriggerEvents;
				trapPointerEvents3.OnExit = (Action)Delegate.Combine(trapPointerEvents3.OnExit, new Action(this.OnPointerExitIcon));
			}
			TrapPointerEvents trapPointerEvents4 = this.tokenPanelTriggerEvents;
			trapPointerEvents4.OnClick = (Action)Delegate.Combine(trapPointerEvents4.OnClick, new Action(this.OnPointerClickIcon));
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000CCB80 File Offset: 0x000CAD80
		protected void OnDisable()
		{
			if (PlatformManager.IsStandalone)
			{
				TrapPointerEvents trapPointerEvents = this.tokenPanelEvents;
				trapPointerEvents.OnExit = (Action)Delegate.Remove(trapPointerEvents.OnExit, new Action(this.OnPointerExitPanel));
				TrapPointerEvents trapPointerEvents2 = this.tokenPanelTriggerEvents;
				trapPointerEvents2.OnEnter = (Action)Delegate.Remove(trapPointerEvents2.OnEnter, new Action(this.OnPointerEnterIcon));
				TrapPointerEvents trapPointerEvents3 = this.tokenPanelTriggerEvents;
				trapPointerEvents3.OnExit = (Action)Delegate.Remove(trapPointerEvents3.OnExit, new Action(this.OnPointerExitIcon));
			}
			TrapPointerEvents trapPointerEvents4 = this.tokenPanelTriggerEvents;
			trapPointerEvents4.OnClick = (Action)Delegate.Remove(trapPointerEvents4.OnClick, new Action(this.OnPointerClickIcon));
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x0003DE83 File Offset: 0x0003C083
		protected void Update()
		{
			this.UpdatePosition();
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0003DE8B File Offset: 0x0003C08B
		public void SetTokensMenuController(TokensController tokensMenuController)
		{
			this.tokensMenuController = tokensMenuController;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x0003DE94 File Offset: 0x0003C094
		protected void UpdatePosition()
		{
			if (this.attachedGameHex == null || this.initiator == null)
			{
				return;
			}
			base.transform.position = this.initiator.transform.position;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0003DEC8 File Offset: 0x0003C0C8
		private void OnPointerEnterIcon()
		{
			if (this.state == TokenPanelState.Hidden || this.state == TokenPanelState.HideAnimation)
			{
				this.ShowTokenPanelAnimation();
			}
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000CCC30 File Offset: 0x000CAE30
		private void OnPointerClickIcon()
		{
			if (PlatformManager.IsMobile)
			{
				this.ShowTokenPanelMobile();
				return;
			}
			if (this.state == TokenPanelState.Visible || this.state == TokenPanelState.ShowAnimation)
			{
				return;
			}
			if (GameController.GameManager.moveManager.GetActualAction() != null && (this.initiator == null || (GameController.GameManager.moveManager.GetSelectedUnit() != this.initiator.UnitLogic && GameController.GameManager.moveManager.GetSelectedUnit() != null)))
			{
				return;
			}
			if (this.state == TokenPanelState.Hidden || this.state == TokenPanelState.HideAnimation)
			{
				this.ShowTokenPanelAnimation();
			}
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0003DEE1 File Offset: 0x0003C0E1
		private void OnPointerExitIcon()
		{
			base.StartCoroutine(this.TryToClosePanel());
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x0003DEF0 File Offset: 0x0003C0F0
		protected IEnumerator TryToClosePanel()
		{
			yield return new WaitForEndOfFrame();
			if (this.tokenPanelEvents == null || (this.tokenPanelEvents.CanClose && this.tokenPanelTriggerEvents.CanClose && this.tokenPanel.gameObject.activeSelf))
			{
				this.HideTokenPanelAnimation();
			}
			yield break;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x0003DEFF File Offset: 0x0003C0FF
		public void ExitPanelMobile()
		{
			this.tokenPanel.gameObject.SetActive(false);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x0003DF12 File Offset: 0x0003C112
		protected IEnumerator TryToClosePanelMobile()
		{
			yield return new WaitForEndOfFrame();
			this.HideTokenPanelAnimation();
			yield break;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0003DF21 File Offset: 0x0003C121
		private void OnPointerExitPanel()
		{
			base.StartCoroutine(this.TryToClosePanel2());
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0003DF30 File Offset: 0x0003C130
		protected IEnumerator TryToClosePanel2()
		{
			yield return new WaitForEndOfFrame();
			if (this.tokenPanelEvents == null || (this.tokenPanelEvents.CanClose && this.tokenPanelTriggerEvents.CanClose && this.tokenPanel.gameObject.activeSelf))
			{
				this.HideTokenPanelAnimation();
			}
			yield break;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0003DF3F File Offset: 0x0003C13F
		protected void FinishPreviousAnimations(bool withCallbacks = true)
		{
			if (this.panelSequence != null)
			{
				this.panelSequence.Complete(withCallbacks);
			}
			if (this.panelTriggerSequence != null)
			{
				this.panelTriggerSequence.Complete(withCallbacks);
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000CCCC4 File Offset: 0x000CAEC4
		protected void ShowPanelTriggerAnimation()
		{
			if (this.panelTriggerSequence != null)
			{
				this.panelTriggerSequence.Complete(true);
			}
			this.SetActive(true);
			base.transform.localScale = Vector3.zero;
			this.panelTriggerSequence = DOTween.Sequence();
			this.panelTriggerSequence.Append(base.transform.DOScale(this.normalScale.x, 0.5f).SetEase(Ease.OutBounce).OnComplete(delegate
			{
				this.OnShowPanelTriggerComplete();
			}));
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x0003DF69 File Offset: 0x0003C169
		protected void OnShowPanelTriggerComplete()
		{
			this.triggerPulseAnimation.StartPlaying();
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000CCD48 File Offset: 0x000CAF48
		protected void HidePanelTriggerAnimation()
		{
			this.triggerPulseAnimation.StopPlaying();
			if (this.panelTriggerSequence != null)
			{
				this.panelTriggerSequence.Complete(true);
			}
			this.panelTriggerSequence = DOTween.Sequence();
			this.panelTriggerSequence.Append(base.transform.DOScale(new Vector3(0f, 0f, this.normalScale.z), 0.5f).SetEase(Ease.OutExpo).OnComplete(delegate
			{
				this.OnHidePanelTriggerComplete();
			}));
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0003DF76 File Offset: 0x0003C176
		protected void HidePanelMobile(bool hideTrigger = true)
		{
			this.tokenPanel.gameObject.SetActive(false);
			if (hideTrigger)
			{
				this.SetActive(false);
				this.Clear();
			}
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x0003DF99 File Offset: 0x0003C199
		protected void OnHidePanelTriggerComplete()
		{
			this.SetActive(false);
			this.Clear();
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000CCDD0 File Offset: 0x000CAFD0
		protected void ShowTokenPanelAnimation()
		{
			this.PreShowTokenPanel();
			this.state = TokenPanelState.ShowAnimation;
			if (this.panelSequence != null)
			{
				this.panelSequence.Complete(true);
			}
			this.tokenPanel.gameObject.SetActive(true);
			this.tokenPanel.localScale = Vector3.zero;
			if (!PlatformManager.IsMobile)
			{
				this.tokenPanel.position = this.tokenPanelTrigger.transform.position;
			}
			this.panelSequence = DOTween.Sequence();
			this.panelSequence.Append(this.tokenPanel.DOScale(1f, 0.5f).SetEase(Ease.OutBounce));
			if (!PlatformManager.IsMobile)
			{
				this.panelSequence.Join(this.tokenPanel.DOLocalMoveY(this.tokenYFinalPosition, 0.5f, false).SetEase(Ease.OutBounce));
			}
			this.panelSequence.OnComplete(delegate
			{
				this.OnShowTokenPanelComplete();
			});
			this.panelSequence.PlayForward();
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000CCEC8 File Offset: 0x000CB0C8
		protected void ShowTokenPanelMobile()
		{
			this.tokenPanel.gameObject.SetActive(true);
			if (GameController.GameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Togawa))
			{
				SingletonMono<TrapsPanelMobile>.Instance.WindowPreview(false);
				return;
			}
			if (GameController.GameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Albion))
			{
				SingletonMono<FlagsPanelMobile>.Instance.WindowPreview(false);
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void PreShowTokenPanel()
		{
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0003DFA8 File Offset: 0x0003C1A8
		protected virtual void OnShowTokenPanelComplete()
		{
			this.state = TokenPanelState.Visible;
			this.panelSequence = null;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void PreHideTokenPanel()
		{
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000CCF4C File Offset: 0x000CB14C
		protected void HideTokenPanelAnimation()
		{
			this.PreHideTokenPanel();
			if (this.panelSequence != null)
			{
				this.panelSequence.Complete(true);
			}
			this.state = TokenPanelState.HideAnimation;
			this.panelSequence = DOTween.Sequence();
			this.panelSequence.Append(this.tokenPanel.DOScale(new Vector3(0f, 0f, 1f), 0.5f).SetEase(Ease.OutExpo));
			this.panelSequence.Join(this.tokenPanel.DOLocalMoveY(this.tokenPanelTrigger.transform.position.y, 0.5f, false).SetEase(Ease.OutBounce));
			this.panelSequence.OnComplete(delegate
			{
				this.OnHideTokenPanelComplete();
			});
			this.panelSequence.PlayForward();
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x0003DFB8 File Offset: 0x0003C1B8
		protected void OnHideTokenPanelComplete()
		{
			this.state = TokenPanelState.Hidden;
			this.tokenPanel.gameObject.SetActive(false);
			this.panelSequence = null;
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0003DFD9 File Offset: 0x0003C1D9
		public void Clear()
		{
			base.transform.localScale = this.normalScale;
			this.attachedGameHex = null;
			this.initiator = null;
		}

		// Token: 0x040017D6 RID: 6102
		[SerializeField]
		protected GameObject tokenPanelTrigger;

		// Token: 0x040017D7 RID: 6103
		[SerializeField]
		private PulseAnimation triggerPulseAnimation;

		// Token: 0x040017D8 RID: 6104
		[SerializeField]
		protected Transform tokenPanel;

		// Token: 0x040017D9 RID: 6105
		[SerializeField]
		protected float tokenYFinalPosition = -34f;

		// Token: 0x040017DA RID: 6106
		[SerializeField]
		private TrapPointerEvents tokenPanelEvents;

		// Token: 0x040017DB RID: 6107
		[SerializeField]
		private TrapPointerEvents tokenPanelTriggerEvents;

		// Token: 0x040017DC RID: 6108
		protected TokensController tokensMenuController;

		// Token: 0x040017DD RID: 6109
		protected RotateTowardsCamera rotateTowardsCamera;

		// Token: 0x040017DE RID: 6110
		protected TokenPanelState state;

		// Token: 0x040017DF RID: 6111
		protected GameHexPresenter attachedGameHex;

		// Token: 0x040017E0 RID: 6112
		protected UnitPresenter initiator;

		// Token: 0x040017E1 RID: 6113
		protected Vector3 normalScale = new Vector3(0.02f, 0.02f, 0.02f);

		// Token: 0x040017E2 RID: 6114
		protected Vector3 animationScale = new Vector3(0.03f, 0.03f, 0.03f);

		// Token: 0x040017E3 RID: 6115
		protected Vector3 maxZoomOutScale = new Vector3(0.4f, 0.4f, 0.4f);

		// Token: 0x040017E4 RID: 6116
		private Sequence panelTriggerSequence;

		// Token: 0x040017E5 RID: 6117
		private Sequence panelSequence;
	}
}
