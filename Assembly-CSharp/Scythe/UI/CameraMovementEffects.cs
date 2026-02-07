using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Utilities;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Scythe.UI
{
	// Token: 0x02000393 RID: 915
	public class CameraMovementEffects : MonoBehaviour
	{
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001B51 RID: 6993 RVA: 0x000AC194 File Offset: 0x000AA394
		// (remove) Token: 0x06001B52 RID: 6994 RVA: 0x000AC1CC File Offset: 0x000AA3CC
		public event CameraMovementEffects.OnPresentationEnd OnFactionPresentationEnd;

		// Token: 0x06001B53 RID: 6995 RVA: 0x00039CF1 File Offset: 0x00037EF1
		private void Awake()
		{
			CameraMovementEffects.Instance = this;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00039CF9 File Offset: 0x00037EF9
		private void Update()
		{
			if (this.presentationSkipDetect && Input.GetMouseButtonDown(0))
			{
				this.presentationSkip = true;
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00039D12 File Offset: 0x00037F12
		private void LateUpdate()
		{
			if (this.lookAtFactory)
			{
				base.transform.LookAt(new Vector3(0f, this.LookAtTargetHeight, 0f));
			}
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x00039D3C File Offset: 0x00037F3C
		public IEnumerator ShowPresentation()
		{
			this.StartFactionPresentation();
			base.StartCoroutine(this.EnableSkipDetection());
			yield return new WaitUntil(() => this.presenationFinished);
			yield break;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x00039D4B File Offset: 0x00037F4B
		public IEnumerator EnableSkipDetection()
		{
			yield return new WaitForEndOfFrame();
			this.presentationSkipDetect = true;
			yield break;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000AC204 File Offset: 0x000AA404
		public void StartFactionPresentation()
		{
			this.presentationSkip = false;
			this.presentationSkipDetect = false;
			WorldSFXManager.PlaySound(SoundEnum.FactionsPresentationAmbient, AudioSourceType.Loops);
			MultiplayerController.Instance.IsFactionPresentationInProgress = true;
			CameraControler.Instance.enabled = false;
			if (PlatformManager.IsStandalone)
			{
				this.playerMatBackground.SetActive(false);
			}
			else
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0f);
			}
			this.introPanel.ActivateIntroductionPanel();
			this.endCameraPosition = base.transform.position;
			this.endCameraRotation = base.transform.rotation;
			base.transform.position = this.startPosition;
			base.transform.rotation = Quaternion.Euler(this.startRotation);
			this.gameFactions = GameController.GameManager.GetPlayersFactions();
			this.FirstCameraMove();
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000AC2CC File Offset: 0x000AA4CC
		private void FirstCameraMove()
		{
			base.transform.position = this.startPosition;
			base.transform.rotation = Quaternion.Euler(this.startRotation);
			TweenCallback tweenCallback = new TweenCallback(this.MoveToNextFaction);
			if (this.presentationSkip)
			{
				tweenCallback = new TweenCallback(this.LastCamerMove);
			}
			base.transform.DOMove(this.startMmovementEndPosition, this.startMovementTime, false).SetEase(this.startMmovementAnimationCurve).OnComplete(tweenCallback);
			this.lookAtFactory = true;
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalLength = x;
			}, this.startFocalLength, this.startMmovementEndfocalLength, this.startMovementTime).SetEase(this.startMmovementAnimationCurve);
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalSize = x;
			}, this.startFocalSize, this.startMmovementEndfocalSize, this.startMovementTime).SetEase(this.startMmovementAnimationCurve);
			base.StartCoroutine(this.FirstHitSound());
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x00039D5A File Offset: 0x00037F5A
		private IEnumerator FirstHitSound()
		{
			yield return new WaitForSeconds(1.2f);
			WorldSFXManager.PlaySound(SoundEnum.BigHit, AudioSourceType.WorldSfx);
			yield break;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000AC3C0 File Offset: 0x000AA5C0
		private void MoveToNextFaction()
		{
			if (this.currentFactionNumber >= this.gameFactions.Count || this.presentationSkip)
			{
				this.LastCamerMove();
				return;
			}
			int i;
			for (i = this.iterator; i < this.presenationCameraTransforms.Count; i++)
			{
				if (this.gameFactions.Contains(this.presenationCameraTransforms[i].faction))
				{
					this.iterator = i;
					break;
				}
			}
			base.transform.DOMove(this.presenationCameraTransforms[i].position, this.movementTime, false).SetEase(this.movementAnimationCurve).OnComplete(new TweenCallback(this.MoveToNextFaction));
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalLength = x;
			}, base.GetComponent<DepthOfField>().focalLength, this.presenationCameraTransforms[i].focalLength, this.movementTime).SetEase(this.movementAnimationCurve);
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalSize = x;
			}, base.GetComponent<DepthOfField>().focalSize, this.presenationCameraTransforms[i].focalSize, this.movementTime).SetEase(this.movementAnimationCurve);
			DOTween.To(delegate(float x)
			{
				this.LookAtTargetHeight = x;
			}, this.LookAtTargetHeight, this.presenationCameraTransforms[i].lookAtTargetHeight, this.lookAtTargetLowerigTime);
			this.introPanel.ShowInfo(this.presenationCameraTransforms[i].faction, this.movementTime);
			this.iterator++;
			this.currentFactionNumber++;
			this.randomMidHit = global::UnityEngine.Random.Range(0, 3);
			switch (this.randomMidHit)
			{
			case 0:
				WorldSFXManager.PlaySound(SoundEnum.MidHit1, AudioSourceType.WorldSfx);
				return;
			case 1:
				WorldSFXManager.PlaySound(SoundEnum.MidHit2, AudioSourceType.WorldSfx);
				return;
			case 2:
				WorldSFXManager.PlaySound(SoundEnum.MidHit3, AudioSourceType.WorldSfx);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000AC5A0 File Offset: 0x000AA7A0
		private void LastCamerMove()
		{
			base.transform.DOMove(this.endCameraPosition, this.endMovementTime, false).SetEase(this.endMmovementAnimationCurve).OnComplete(new TweenCallback(this.EndPresentation));
			this.lookAtFactory = false;
			base.transform.DORotate(this.endCameraRotation.eulerAngles, this.endMovementTime, RotateMode.Fast).SetEase(this.endMmovementAnimationCurve);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000AC614 File Offset: 0x000AA814
		private void EndPresentation()
		{
			if (PlatformManager.IsStandalone)
			{
				this.playerMatBackground.SetActive(true);
			}
			else
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
			}
			WorldSFXManager.StopLoopSFX();
			this.presenationFinished = true;
			if (!GameController.Instance.isUndoLoad)
			{
				MusicManager.Instance.InitGameMusic();
			}
			if (this.OnFactionPresentationEnd != null)
			{
				this.OnFactionPresentationEnd();
			}
			if (OptionsManager.IsFastForward() && !GameController.GameManager.IsCampaign)
			{
				ShowEnemyMoves.Instance.SetupFastSpeed();
			}
			MultiplayerController.Instance.IsFactionPresentationInProgress = false;
			CameraControler.Instance.enabled = true;
			this.lookAtFactory = false;
			if (GameController.GameManager.IsMultiplayer)
			{
				if (PlatformManager.IsMobile)
				{
					if (MobileChat.IsSupported)
					{
						SingletonMono<MobileChat>.Instance.chatElements.SetActive(false);
					}
				}
				else
				{
					GameController.Instance.chat.chatElements.SetActive(false);
				}
			}
			if (GameController.GameManager.IsMultiplayer && (GameController.GameManager.PlayerCurrent != GameController.GameManager.PlayerOwner || !MultiplayerController.Instance.AllPlayersLoaded()))
			{
				ShowEnemyMoves.Instance.AnimateCamToBase(PersistentSingleton<GameLogicHandler>.Instance.GameManager.PlayerMaster.matFaction.faction);
			}
			this.introPanel.DisactivateIntroductionPanel();
			base.transform.localPosition = new Vector3(0f, 0f, 0f);
			ShowEnemyMoves.Instance.SetupButtonOnStart();
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000AC77C File Offset: 0x000AA97C
		public void MoveToMechPosition(int index)
		{
			this.lookAtFactory = true;
			base.transform.DOMove(this.showroomPresenationCameraTransforms[index].position, this.movementTime, false).SetEase(this.movementAnimationCurve).OnComplete(new TweenCallback(this.DoNothing));
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalLength = x;
			}, base.GetComponent<DepthOfField>().focalLength, this.showroomPresenationCameraTransforms[index].focalLength, this.movementTime).SetEase(this.movementAnimationCurve);
			DOTween.To(delegate(float x)
			{
				base.GetComponent<DepthOfField>().focalSize = x;
			}, base.GetComponent<DepthOfField>().focalSize, this.showroomPresenationCameraTransforms[index].focalSize, this.movementTime).SetEase(this.movementAnimationCurve);
			DOTween.To(delegate(float x)
			{
				this.LookAtTargetHeight = x;
			}, this.LookAtTargetHeight, this.showroomPresenationCameraTransforms[index].lookAtTargetHeight, this.lookAtTargetLowerigTime);
			if (!this.mechCustomisationPanelPresenter.activeSelf)
			{
				this.mechCustomisationPanelPresenter.SetActive(true);
			}
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void DoNothing()
		{
		}

		// Token: 0x04001341 RID: 4929
		public static CameraMovementEffects Instance;

		// Token: 0x04001342 RID: 4930
		[SerializeField]
		private IntroductionPanel introPanel;

		// Token: 0x04001343 RID: 4931
		[SerializeField]
		private Vector3 startPosition;

		// Token: 0x04001344 RID: 4932
		[SerializeField]
		private Vector3 startRotation;

		// Token: 0x04001345 RID: 4933
		[SerializeField]
		private float startFocalLength = 61.35f;

		// Token: 0x04001346 RID: 4934
		[SerializeField]
		private float startFocalSize = 0.3f;

		// Token: 0x04001347 RID: 4935
		[SerializeField]
		private AnimationCurve startMmovementAnimationCurve;

		// Token: 0x04001348 RID: 4936
		[SerializeField]
		private float startMovementTime = 2f;

		// Token: 0x04001349 RID: 4937
		[SerializeField]
		private Vector3 startMmovementEndPosition;

		// Token: 0x0400134A RID: 4938
		[SerializeField]
		private float startMmovementEndfocalLength = 5.2f;

		// Token: 0x0400134B RID: 4939
		[SerializeField]
		private float startMmovementEndfocalSize = 1.2f;

		// Token: 0x0400134C RID: 4940
		[SerializeField]
		private float lookAtTargetLowerigTime = 0.5f;

		// Token: 0x0400134D RID: 4941
		[SerializeField]
		private AnimationCurve movementAnimationCurve;

		// Token: 0x0400134E RID: 4942
		[SerializeField]
		private float movementTime = 2f;

		// Token: 0x0400134F RID: 4943
		[SerializeField]
		private List<CameraMovementEffects.PresenationCameraTransform> presenationCameraTransforms;

		// Token: 0x04001350 RID: 4944
		[SerializeField]
		private AnimationCurve endMmovementAnimationCurve;

		// Token: 0x04001351 RID: 4945
		[SerializeField]
		private float endMovementTime = 2f;

		// Token: 0x04001352 RID: 4946
		[SerializeField]
		private GameObject playerMatBackground;

		// Token: 0x04001353 RID: 4947
		private Vector3 endCameraPosition;

		// Token: 0x04001354 RID: 4948
		private Quaternion endCameraRotation;

		// Token: 0x04001355 RID: 4949
		private int currentFactionNumber;

		// Token: 0x04001356 RID: 4950
		private int iterator;

		// Token: 0x04001357 RID: 4951
		private List<Faction> gameFactions;

		// Token: 0x04001358 RID: 4952
		private float LookAtTargetHeight;

		// Token: 0x04001359 RID: 4953
		private bool lookAtFactory;

		// Token: 0x0400135A RID: 4954
		public bool presenationFinished;

		// Token: 0x0400135B RID: 4955
		private bool presentationSkip;

		// Token: 0x0400135C RID: 4956
		private bool presentationSkipDetect;

		// Token: 0x0400135D RID: 4957
		[SerializeField]
		private List<CameraMovementEffects.PresenationCameraTransform> showroomPresenationCameraTransforms;

		// Token: 0x0400135E RID: 4958
		public GameObject mechCustomisationPanelPresenter;

		// Token: 0x0400135F RID: 4959
		private int randomMidHit;

		// Token: 0x02000394 RID: 916
		// (Invoke) Token: 0x06001B6B RID: 7019
		public delegate void OnPresentationEnd();

		// Token: 0x02000395 RID: 917
		[Serializable]
		public struct PresenationCameraTransform
		{
			// Token: 0x04001360 RID: 4960
			public Faction faction;

			// Token: 0x04001361 RID: 4961
			public Vector3 position;

			// Token: 0x04001362 RID: 4962
			public float focalLength;

			// Token: 0x04001363 RID: 4963
			public float focalSize;

			// Token: 0x04001364 RID: 4964
			public float lookAtTargetHeight;
		}
	}
}
