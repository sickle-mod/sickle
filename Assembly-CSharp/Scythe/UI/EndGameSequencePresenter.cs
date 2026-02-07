using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000477 RID: 1143
	public class EndGameSequencePresenter : MonoBehaviour
	{
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x0003EF7A File Offset: 0x0003D17A
		// (set) Token: 0x0600241E RID: 9246 RVA: 0x0003EF82 File Offset: 0x0003D182
		public List<PlayerEndGameStats> EndGameStats { get; set; }

		// Token: 0x0600241F RID: 9247 RVA: 0x000D6264 File Offset: 0x000D4464
		public void StartEndGameSequence()
		{
			if (this.playedSequence)
			{
				return;
			}
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				GameController.Instance.undoController.SetUndoInteractivity(false);
			}
			this.playedSequence = true;
			this.DisableOverlapingObjects();
			if (GameController.Instance.panelInfo != null)
			{
				for (int i = 0; i < GameController.Instance.panelInfo.transform.childCount; i++)
				{
					GameController.Instance.panelInfo.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
			ShowEnemyMoves.Instance.SetupNormalSpeed();
			CameraControler.CameraMovementBlocked = true;
			if (CameraControler.Instance.tooltip != null)
			{
				CameraControler.Instance.tooltip.gameObject.SetActive(false);
			}
			CameraControler.Instance.enabled = false;
			this.cameraTransform = CameraMovementEffects.Instance.transform;
			this.originalCameraPosition = this.cameraTransform.position;
			this.originalCameraRotation = this.cameraTransform.rotation;
			base.gameObject.SetActive(true);
			if (PlatformManager.IsStandalone)
			{
				DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
				{
					this.statsPresenter.PrepareInitial();
				})
					.AppendInterval(0.4f)
					.AppendCallback(new TweenCallback(this.SlideUi))
					.AppendInterval(0.6f)
					.AppendCallback(new TweenCallback(this.ShowPointsAnimation));
				return;
			}
			DOTween.Sequence().AppendInterval(0.2f).AppendInterval(0.4f)
				.AppendCallback(new TweenCallback(this.SlideUi))
				.AppendInterval(0.6f)
				.AppendCallback(new TweenCallback(this.ShowPointsAnimation));
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000D6428 File Offset: 0x000D4628
		public void Reset()
		{
			if (!this.playedSequence)
			{
				return;
			}
			if (PlatformManager.IsStandalone)
			{
				this.statsPresenter.TurnOff();
			}
			else
			{
				this.mobileWinnerPanel.Reset();
			}
			this.pointsCountingPresenter.Cleanup();
			base.gameObject.SetActive(false);
			if (PlatformManager.IsStandalone)
			{
				this.totalBlur.GetComponent<Button>().interactable = true;
			}
			else
			{
				this.totalBlur.SetActive(true);
			}
			if (this.fireworks != null)
			{
				global::UnityEngine.Object.Destroy(this.fireworks);
			}
			CameraControler.CameraMovementBlocked = false;
			CameraControler.Instance.enabled = true;
			this.cameraTransform.position = this.originalCameraPosition;
			this.cameraTransform.rotation = this.originalCameraRotation;
			if (this.originalUiElementsPositions != null && this.originalUiElementsPositions.Count > 0)
			{
				for (int i = 0; i < this.originalUiElementsPositions.Count; i++)
				{
					this.bottomUiToSlideAway[i].anchoredPosition = new Vector2(this.bottomUiToSlideAway[i].anchoredPosition.x, this.originalUiElementsPositions[i]);
				}
			}
			this.skipButton.gameObject.SetActive(true);
			this.playedSequence = false;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x0003EF8B File Offset: 0x0003D18B
		public WinnerPannelMobilePresenter GetMobileWinnerPanel()
		{
			return this.mobileWinnerPanel;
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000D655C File Offset: 0x000D475C
		private void SlideUi()
		{
			this.originalUiElementsPositions = new List<float>();
			foreach (RectTransform rectTransform in this.bottomUiToSlideAway)
			{
				if (rectTransform != null)
				{
					this.originalUiElementsPositions.Add(rectTransform.anchoredPosition.y);
					rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - 300f, 1.3f, false).SetEase(this.uiSlideAnimationCurve);
				}
			}
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000D65D8 File Offset: 0x000D47D8
		private void DisableOverlapingObjects()
		{
			GameController.Instance.objectiveCardPresenter.gameObject.SetActive(false);
			GameController.Instance.turnInfoPanel.gameObject.SetActive(false);
			if (GameController.Instance.combatPresenter != null)
			{
				GameController.Instance.combatPresenter.resultPanel.SetActive(false);
			}
			if (PlatformManager.IsMobile)
			{
				GameController.Instance.combatPresenterMobile.turnInfoPanelCombat.gameObject.SetActive(false);
				GameController.Instance.NoMoreBattleAmmoInfo.SetActive(false);
				GameController.Instance.objectiveContextHint.SetActive(false);
				GameController.Instance.waitInfoRecruit.SetActive(false);
				GameController.Instance.endTurnWarning.SetResult(false);
				GameController.Instance.tokensController.HidePanels();
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0003EF93 File Offset: 0x0003D193
		private IEnumerator ShowStatsSummary(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			CameraControler.Instance.enabled = true;
			if (PlatformManager.IsStandalone)
			{
				this.statsPresenter.gameObject.SetActive(true);
				if (!GameController.GameManager.IsRanked)
				{
					this.statsPresenter.ShowStats(this.EndGameStats);
				}
				else
				{
					this.statsPresenter.ShowMultiplayerStats(this.EndGameStats);
				}
			}
			else
			{
				this.mobileWinnerPanel.gameObject.SetActive(true);
				this.mobileWinnerPanel.ShowStats(this.EndGameStats, GameController.GameManager.IsMultiplayer);
			}
			if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.SpectatorMode)
			{
				if (PlatformManager.IsMobile)
				{
					if (MobileChat.IsSupported)
					{
						SingletonMono<MobileChat>.Instance.gameObject.SetActive(true);
						if (!SingletonMono<MobileChat>.Instance.chatElements.activeInHierarchy)
						{
							SingletonMono<MobileChat>.Instance.ChangeChatVisibility();
						}
					}
				}
				else
				{
					GameController.Instance.chat.gameObject.SetActive(true);
					if (!GameController.Instance.chat.chatElements.activeInHierarchy)
					{
						GameController.Instance.chat.ChangeChatVisibility();
					}
				}
			}
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				GameController.Instance.undoController.SetUndoInteractivity(true);
			}
			yield break;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0003EFA9 File Offset: 0x0003D1A9
		private void ShowPointsAnimation()
		{
			if (this.EndGameStats == null)
			{
				return;
			}
			this.pointsCountingPresenter.AnimatePointsCounting(this.EndGameStats, new Action(this.ShowWinner));
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000D66A8 File Offset: 0x000D48A8
		public void ShowWinner()
		{
			DOTween.KillAll(true);
			this.skipButton.gameObject.SetActive(false);
			if (PlatformManager.IsStandalone)
			{
				this.statsPresenter.PrepareHoldForWinnerShowcase();
			}
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.EndGameStats[0].player.character);
			if (this.turnInfoPanel != null)
			{
				this.turnInfoPanel.SetActive(false);
			}
			ShowEnemyMoves.Instance.SetupNormalSpeed();
			ShowEnemyMoves.Instance.AnimateCamToHex(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(unitPresenter.UnitLogic.position).GetWorldPosition());
			this.SpawnFireworks(unitPresenter.transform);
			if (PlatformManager.IsStandalone)
			{
				Color color = this.totalBlur.GetComponent<Image>().color;
				color.a = 0f;
				this.totalBlur.GetComponent<Image>().color = color;
			}
			else
			{
				this.totalBlur.SetActive(false);
			}
			base.StartCoroutine(this.ShowStatsSummary(4f));
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0003EFD1 File Offset: 0x0003D1D1
		private void SpawnFireworks(Transform character)
		{
			this.fireworks = global::UnityEngine.Object.Instantiate<GameObject>(this.fireworksPrefab, character.position, this.fireworksPrefab.transform.rotation);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0003EFFA File Offset: 0x0003D1FA
		public bool SequencePlayed()
		{
			return this.playedSequence;
		}

		// Token: 0x0400193A RID: 6458
		[SerializeField]
		private EndGameStatsPresenter statsPresenter;

		// Token: 0x0400193B RID: 6459
		[SerializeField]
		private PointsCountingPresenter pointsCountingPresenter;

		// Token: 0x0400193C RID: 6460
		[SerializeField]
		private GameObject fireworksPrefab;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private RectTransform[] bottomUiToSlideAway;

		// Token: 0x0400193E RID: 6462
		[SerializeField]
		private AnimationCurve uiSlideAnimationCurve;

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private GameObject turnInfoPanel;

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private WinnerPannelMobilePresenter mobileWinnerPanel;

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private Button skipButton;

		// Token: 0x04001943 RID: 6467
		private bool playedSequence;

		// Token: 0x04001944 RID: 6468
		private Transform cameraTransform;

		// Token: 0x04001945 RID: 6469
		private GameObject fireworks;

		// Token: 0x04001946 RID: 6470
		private Vector3 originalCameraPosition;

		// Token: 0x04001947 RID: 6471
		private Quaternion originalCameraRotation;

		// Token: 0x04001948 RID: 6472
		private List<float> originalUiElementsPositions;

		// Token: 0x04001949 RID: 6473
		public GameObject totalBlur;
	}
}
