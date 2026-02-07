using System;
using System.Collections;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000468 RID: 1128
	public class PlayerOrder : MonoBehaviour
	{
		// Token: 0x06002393 RID: 9107 RVA: 0x0003EC44 File Offset: 0x0003CE44
		private void Awake()
		{
			this.blockPassCoins = false;
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0003EC4D File Offset: 0x0003CE4D
		public void OnUndo()
		{
			this.passCoinsPresenter.OnUndo();
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0003EC5A File Offset: 0x0003CE5A
		public Image[] GetBasicLogos()
		{
			return this.logos;
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x000D3084 File Offset: 0x000D1284
		public void UpdateStatus()
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			int playerCurrentId = GameController.GameManager.PlayerCurrentId;
			for (int i = 0; i < this.logos.Length; i++)
			{
				if (i >= players.Count)
				{
					this.logos[i].gameObject.SetActive(false);
				}
				else
				{
					Player player = players[(playerCurrentId + i) % players.Count];
					this.logos[i].gameObject.SetActive(true);
					this.logos[i].sprite = GameController.factionInfo[player.matFaction.faction].logo;
					this.logos[i].transform.GetChild(0).GetComponent<Image>().sprite = this.logoSprites[(int)player.matFaction.faction];
					int numberOfStars = player.GetNumberOfStars();
					if (PlatformManager.IsMobile)
					{
						for (int j = 0; j < 6; j++)
						{
							this.logos[i].GetComponent<PlayerOrderLogo>().SetupStarsImage(this.factionStars[(int)player.matFaction.faction]);
						}
					}
					for (int k = 0; k < 6; k++)
					{
						this.logos[i].transform.GetChild(1).GetChild(k).gameObject.SetActive(k < numberOfStars);
					}
				}
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0003EC62 File Offset: 0x0003CE62
		public void HidePassCoinPresenter()
		{
			this.passCoinsPresenter.Dismiss();
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000D31D8 File Offset: 0x000D13D8
		public void LogoPressed(int id)
		{
			if (!GameController.GameManager.SpectatorMode && !this.blockPassCoins)
			{
				if (PlatformManager.IsStandalone)
				{
					int num = (GameController.GameManager.PlayerCurrentId + id) % GameController.GameManager.players.Count;
					this.passCoinsPresenter.Show(num);
					return;
				}
				if (!this.enemyInfoPreview.IsEnemyInfoVisible())
				{
					if (this.delayShowPlayerDashboard != null)
					{
						base.StopCoroutine(this.delayShowPlayerDashboard);
						this.delayShowPlayerDashboard = null;
					}
					this.passCoinsPresenter.Show();
				}
			}
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0003EC6F File Offset: 0x0003CE6F
		public void LogoHovered(int id)
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardHold, AudioSourceType.Buttons);
			this.idPressed = id;
			this.delayShowPlayerDashboard = base.StartCoroutine(this.PushDelayed());
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0003EC92 File Offset: 0x0003CE92
		public void LogoReleased(int id)
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease, AudioSourceType.Buttons);
			this.idPressed = -1;
			this.LogoPush(id, false);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0003ECAB File Offset: 0x0003CEAB
		private IEnumerator PushDelayed()
		{
			if (PlatformManager.IsMobile && !this.enemyInfoPreview.IsEnemyInfoVisible())
			{
				yield return new WaitForSecondsRealtime(0.2f);
			}
			else
			{
				yield return new WaitForSecondsRealtime(0.2f);
			}
			if (this.idPressed != -1)
			{
				this.LogoPush(this.idPressed, true);
			}
			yield break;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000D3260 File Offset: 0x000D1460
		private void LogoPush(int id, bool down)
		{
			if (down)
			{
				this.delayShowPlayerDashboard = null;
			}
			int playerCurrentId = GameController.GameManager.PlayerCurrentId;
			List<Player> players = GameController.GameManager.GetPlayers();
			Player player = players[(playerCurrentId + id) % players.Count];
			this.matPreview.Visibility(down, player);
			if (down)
			{
				this.LogoHoldActions(down, player);
				return;
			}
			this.LogoReleaseClenup(down, player);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0003ECBA File Offset: 0x0003CEBA
		private void MobileInputRelease()
		{
			EnemyInfoPreview.Instance.TurnOffEnemyPlayerInfo();
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000D32C0 File Offset: 0x000D14C0
		private void LogoHoldActions(bool down, Player player)
		{
			if (ActionLogInterpreter.IsSupported)
			{
				SingletonMono<ActionLogInterpreter>.Instance.HideMarkers();
				SingletonMono<ActionLogInterpreter>.Instance.ShowMarkers(new List<GameHex>(player.OwnedFields(false)));
				SingletonMono<ActionLogInterpreter>.Instance.ShowOutlineOnUnits(player.GetAllUnits());
				if (PlatformManager.IsMobile)
				{
					SingletonMono<ActionLogVisibilityMobile>.Instance.ActionLogActive(false);
				}
				else
				{
					this.actionLog.SetActive(false);
				}
			}
			if (this.dashboard != null)
			{
				this.dashboard.Show(player);
			}
			if (this.enemyInfoPreview != null)
			{
				this.enemyInfoPreview.Visibility(down, player);
			}
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000D335C File Offset: 0x000D155C
		private void LogoReleaseClenup(bool down, Player player)
		{
			if (ActionLogInterpreter.IsSupported)
			{
				SingletonMono<ActionLogInterpreter>.Instance.HideMarkers();
				SingletonMono<ActionLogInterpreter>.Instance.HideOutlineOnUnits();
				if (!this.panelToggleGroup.AnyTogglesOn() && !PlatformManager.IsMobile)
				{
					this.actionLog.SetActive(true);
				}
			}
			if (this.dashboard != null)
			{
				this.dashboard.Hide();
			}
			if (this.enemyInfoPreview != null)
			{
				this.enemyInfoPreview.Visibility(down, player);
			}
		}

		// Token: 0x040018BC RID: 6332
		public Image[] logos;

		// Token: 0x040018BD RID: 6333
		public Sprite[] logoSprites;

		// Token: 0x040018BE RID: 6334
		public MatPreview matPreview;

		// Token: 0x040018BF RID: 6335
		public PlayerDashboardPresenter dashboard;

		// Token: 0x040018C0 RID: 6336
		public GameObject actionLog;

		// Token: 0x040018C1 RID: 6337
		public Toggle2Group panelToggleGroup;

		// Token: 0x040018C2 RID: 6338
		public bool blockPassCoins;

		// Token: 0x040018C3 RID: 6339
		private int idPressed = -1;

		// Token: 0x040018C4 RID: 6340
		[SerializeField]
		private PassCoinsPresenter passCoinsPresenter;

		// Token: 0x040018C5 RID: 6341
		[SerializeField]
		private EnemyInfoPreview enemyInfoPreview;

		// Token: 0x040018C6 RID: 6342
		[SerializeField]
		private Sprite[] factionStars;

		// Token: 0x040018C7 RID: 6343
		private Coroutine delayShowPlayerDashboard;
	}
}
