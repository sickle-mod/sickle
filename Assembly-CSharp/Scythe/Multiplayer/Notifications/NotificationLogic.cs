using System;
using System.Collections;
using AsmodeeNet.Foundation;
using I2.Loc;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x020002A0 RID: 672
	public class NotificationLogic : GenericSingletonClass<NotificationLogic>
	{
		// Token: 0x0600153F RID: 5439 RVA: 0x00036634 File Offset: 0x00034834
		private void OnEnable()
		{
			this.LoadNotificationsSetting();
			SceneManager.activeSceneChanged += this.SceneManager_activeSceneChanged;
			OptionsManager.OnNotificationsSettingChanged += this.LoadNotificationsSetting;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0003665E File Offset: 0x0003485E
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.SceneManager_activeSceneChanged;
			OptionsManager.OnNotificationsSettingChanged -= this.LoadNotificationsSetting;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0009D268 File Offset: 0x0009B468
		public bool CanDisplay()
		{
			return !(SceneManager.GetActiveScene().name == "boot") && !this.isNotificationFlowRunning;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00036682 File Offset: 0x00034882
		private void LoadNotificationsSetting()
		{
			this.currentNotificationsSetting = (NotificationsSetting)PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_NOTIFICATIONS, 1);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0009D29C File Offset: 0x0009B49C
		public bool IsDisplayable(Notification notification)
		{
			if (this.currentNotificationsSetting == NotificationsSetting.None)
			{
				return false;
			}
			if (notification is CombatNotification)
			{
				return this.IsDisplayable((CombatNotification)notification);
			}
			if (notification is GameOverNotification)
			{
				return this.IsDisplayable((GameOverNotification)notification);
			}
			return notification is YourTurnNotification && this.IsDisplayable((YourTurnNotification)notification);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00036695 File Offset: 0x00034895
		private bool IsDisplayable(CombatNotification notification)
		{
			return this.currentNotificationsSetting != NotificationsSetting.InvitesOnly && !this.IsPlayerInAGame(notification.GameId);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x000366B3 File Offset: 0x000348B3
		private bool IsDisplayable(GameOverNotification notification)
		{
			return this.currentNotificationsSetting != NotificationsSetting.InvitesOnly && !this.IsPlayerInAGame(notification.GameId);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x000366CF File Offset: 0x000348CF
		private bool IsDisplayable(YourTurnNotification notification)
		{
			return this.currentNotificationsSetting != NotificationsSetting.InvitesOnly && !this.IsPlayerInAGame(notification.GameId);
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0009D2F4 File Offset: 0x0009B4F4
		public bool IsClickable(Notification notification)
		{
			if (notification is CombatNotification)
			{
				return this.IsClickable((CombatNotification)notification);
			}
			if (notification is GameOverNotification)
			{
				return this.IsClickable((GameOverNotification)notification);
			}
			return notification is YourTurnNotification && this.IsClickable((YourTurnNotification)notification);
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0002A1D9 File Offset: 0x000283D9
		private bool IsClickable(CombatNotification notification)
		{
			return false;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x000366ED File Offset: 0x000348ED
		private bool IsClickable(GameOverNotification notification)
		{
			return this.CanNotificationBeClickedInCurrentContext();
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0002A1D9 File Offset: 0x000283D9
		private bool IsClickable(YourTurnNotification notification)
		{
			return false;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0009D344 File Offset: 0x0009B544
		private bool IsPlayerInAGame(string gameId)
		{
			return SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME && MultiplayerController.Instance != null && MultiplayerController.Instance.IsMultiplayer && PlayerInfo.me.RoomId == gameId;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x000366F5 File Offset: 0x000348F5
		private bool CanNotificationBeClickedInCurrentContext()
		{
			return !this.PlayerInBootScene() && !this.PlayerInOnlineGameScene() && !this.PlayerInLobbySceneInRoom();
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0009D390 File Offset: 0x0009B590
		private bool PlayerInBootScene()
		{
			return SceneManager.GetActiveScene().name == "boot";
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0009D3B4 File Offset: 0x0009B5B4
		private bool PlayerInOnlineGameScene()
		{
			return SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME && GameController.GameManager.IsMultiplayer;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0009D3E8 File Offset: 0x0009B5E8
		private bool PlayerInLobbySceneInRoom()
		{
			return SceneManager.GetActiveScene().name == SceneController.SCENE_LOBBY_NAME && PlayerInfo.me.CurrentLobbyRoom != null;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0009D420 File Offset: 0x0009B620
		public void NotificationClicked(Notification notification)
		{
			this.menuLoadedFromNotification = true;
			this.isNotificationFlowRunning = true;
			this.lastNotification = notification;
			if (notification is CombatNotification)
			{
				this.NotificationClicked((CombatNotification)notification);
				return;
			}
			if (notification is GameOverNotification)
			{
				this.NotificationClicked((GameOverNotification)notification);
				return;
			}
			if (notification is YourTurnNotification)
			{
				this.NotificationClicked((YourTurnNotification)notification);
				return;
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void NotificationClicked(CombatNotification notification)
		{
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0009D484 File Offset: 0x0009B684
		private void NotificationClicked(GameOverNotification notification)
		{
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MENU_NAME)
			{
				SingletonMono<MainMenu>.Instance.MultiplayerMenu();
				return;
			}
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_LOBBY_NAME)
			{
				base.StartCoroutine(this.DisplayEndGameInfoInLobby(notification.GameId));
				return;
			}
			this.LoadMenuScene();
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void NotificationClicked(YourTurnNotification notification)
		{
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00036712 File Offset: 0x00034912
		private void LoadMenuScene()
		{
			if (SceneController.Instance != null)
			{
				SceneController.Instance.LoadScene(SceneController.SCENE_MENU_NAME);
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00036730 File Offset: 0x00034930
		public void LoadLobbyScene()
		{
			SingletonMono<MainMenu>.Instance.MultiplayerMenu();
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0009D4E8 File Offset: 0x0009B6E8
		private void SceneManager_activeSceneChanged(Scene previousScene, Scene currentScene)
		{
			if (this.menuLoadedFromNotification)
			{
				string name = currentScene.name;
				if (name == SceneController.SCENE_MENU_NAME)
				{
					this.LoadLobbyScene();
					return;
				}
				if (name == SceneController.SCENE_LOBBY_NAME)
				{
					this.menuLoadedFromNotification = false;
					this.isNotificationFlowRunning = false;
					this.SuitableNotificationEffect();
					return;
				}
				if (!(name == "loading"))
				{
					this.menuLoadedFromNotification = false;
					this.isNotificationFlowRunning = false;
				}
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0003673C File Offset: 0x0003493C
		private void SuitableNotificationEffect()
		{
			if (this.lastNotification is GameOverNotification && this.lastNotification != null)
			{
				base.StartCoroutine(this.DisplayEndGameInfoInLobby(((GameOverNotification)this.lastNotification).GameId));
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00036770 File Offset: 0x00034970
		private IEnumerator DisplayEndGameInfoInLobby(string GameId)
		{
			Lobby lobby = global::UnityEngine.Object.FindObjectOfType<Lobby>();
			lobby.ShowWaitingScreen(ScriptLocalization.Get("Lobby/Loading"), true);
			while (LobbyLogic.Instance == null)
			{
				yield return new WaitForEndOfFrame();
			}
			if (LobbyLogic.Instance.PlayerStatsDownloaded)
			{
				LobbyLogic.Instance.RefreshPlayerStats();
			}
			while (!LobbyLogic.Instance.LobbyLoaded())
			{
				yield return new WaitForSeconds(0.1f);
			}
			this.menuLoadedFromNotification = false;
			this.isNotificationFlowRunning = false;
			this.lastNotification = null;
			lobby.ShowCurrentPlayerStats();
			global::UnityEngine.Object.FindObjectOfType<PlayerStatsScreen>().ScrollToRecentGameEntry(GameId);
			yield break;
		}

		// Token: 0x04000F8C RID: 3980
		[HideInInspector]
		public bool menuLoadedFromNotification;

		// Token: 0x04000F8D RID: 3981
		private bool isNotificationFlowRunning;

		// Token: 0x04000F8E RID: 3982
		private Notification lastNotification;

		// Token: 0x04000F8F RID: 3983
		private NotificationsSetting currentNotificationsSetting;
	}
}
