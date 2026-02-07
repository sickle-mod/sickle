using System;
using DG.Tweening;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000497 RID: 1175
	public class MultiplayerClocksController : MonoBehaviour
	{
		// Token: 0x06002541 RID: 9537 RVA: 0x0003FA64 File Offset: 0x0003DC64
		private void Awake()
		{
			this.clocksLayout = this.enemyPlayersClocks.GetComponent<VerticalLayoutGroup>();
			this.clocksRectOffset = this.clocksLayout.padding;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000DE314 File Offset: 0x000DC514
		private void Start()
		{
			this.SetupClocks();
			if (GameController.GameManager.IsMultiplayer)
			{
				MultiplayerController.OnPlayerReconnected += this.SetupClocks;
				if (!MultiplayerController.Instance.ReturningToStartedGame)
				{
					CameraMovementEffects.Instance.OnFactionPresentationEnd += this.TurnClocksOnAfterCameraAnimation;
					base.gameObject.SetActive(false);
				}
				else
				{
					this.TurnClocksOnAfterCameraAnimation();
				}
			}
			if (GameController.GameManager.SpectatorMode)
			{
				base.GetComponent<VerticalLayoutGroup>().padding.right = 32;
			}
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x0003FA88 File Offset: 0x0003DC88
		private void OnDestroy()
		{
			if (GameController.GameManager != null && GameController.GameManager.IsMultiplayer)
			{
				MultiplayerController.OnPlayerReconnected -= this.SetupClocks;
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000DE398 File Offset: 0x000DC598
		private void SetupClocks()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.ownerClock.SetPlayerData(this.GetPlayerDataForClock(this.ownerClock.GetClockId()));
				for (int i = 0; i < MultiplayerController.Instance.playersInGame.Count - 1; i++)
				{
					this.enemyClocks[i].SetPlayerData(this.GetPlayerDataForClock(this.enemyClocks[i].GetClockId()));
				}
			}
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x0003FAAE File Offset: 0x0003DCAE
		private PlayerData GetPlayerDataForClock(int clockId)
		{
			return MultiplayerController.Instance.GetPlayerAtOffsetFromOwner(clockId);
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x0003FABB File Offset: 0x0003DCBB
		public void OnOwnerClockClicked()
		{
			if (this.currentAnimation != null)
			{
				return;
			}
			if (this.enemyPlayersClocks.activeSelf)
			{
				this.StartHideEnemyClocksAnimation();
				return;
			}
			this.StartShowEnemyClocksAnimation();
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000DE40C File Offset: 0x000DC60C
		private void StartShowEnemyClocksAnimation()
		{
			this.enemyPlayersClocks.gameObject.SetActive(true);
			this.enemyPlayersClocks.GetComponent<RectTransform>().ForceUpdateRectTransforms();
			int num = 0;
			float num2 = 0.5f;
			this.currentAnimation = DOTween.To(() => this.clocksLayout.padding.right, delegate(int x)
			{
				this.clocksLayout.padding = new RectOffset(this.clocksRectOffset.left, x, this.clocksRectOffset.top, this.clocksRectOffset.bottom);
			}, num, num2).OnComplete(delegate
			{
				this.OnEnemyClocksAnimationComplete(true);
			});
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000DE478 File Offset: 0x000DC678
		private void StartHideEnemyClocksAnimation()
		{
			int num = -(int)this.enemyPlayersClocks.GetComponent<RectTransform>().rect.width;
			float num2 = 0.5f;
			this.currentAnimation = DOTween.To(() => this.clocksLayout.padding.right, delegate(int x)
			{
				this.clocksLayout.padding = new RectOffset(this.clocksRectOffset.left, x, this.clocksRectOffset.top, this.clocksRectOffset.bottom);
			}, num, num2).OnComplete(delegate
			{
				this.OnEnemyClocksAnimationComplete(false);
			});
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x0003FAE0 File Offset: 0x0003DCE0
		private void OnEnemyClocksAnimationComplete(bool show)
		{
			this.currentAnimation = null;
			if (!show)
			{
				this.enemyPlayersClocks.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x0003FAFD File Offset: 0x0003DCFD
		private void TurnClocksOnAfterCameraAnimation()
		{
			base.gameObject.SetActive(true);
			CameraMovementEffects.Instance.OnFactionPresentationEnd -= this.TurnClocksOnAfterCameraAnimation;
		}

		// Token: 0x04001A28 RID: 6696
		[SerializeField]
		private SingleClockPresenter ownerClock;

		// Token: 0x04001A29 RID: 6697
		[SerializeField]
		private SingleClockPresenter[] enemyClocks;

		// Token: 0x04001A2A RID: 6698
		[SerializeField]
		private GameObject enemyPlayersClocks;

		// Token: 0x04001A2B RID: 6699
		private Tween currentAnimation;

		// Token: 0x04001A2C RID: 6700
		private VerticalLayoutGroup clocksLayout;

		// Token: 0x04001A2D RID: 6701
		private RectOffset clocksRectOffset;
	}
}
