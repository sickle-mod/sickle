using System;
using Common.GameSaves;
using HoneyFramework;
using Scythe.Analytics;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D6 RID: 214
public class GameMenu : MonoBehaviour
{
	// Token: 0x06000649 RID: 1609 RVA: 0x0002BEE7 File Offset: 0x0002A0E7
	private void OnApplicationQuit()
	{
		if (!PlatformManager.IsStandalone)
		{
			this.OnExitGameDialogShow();
		}
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0002BEF6 File Offset: 0x0002A0F6
	private void OnEnable()
	{
		CameraControler.BlockEverything = true;
		if (PlatformManager.IsStandalone)
		{
			CameraControler.CameraMovementBlocked = true;
		}
		if (GameController.GameManager.IsCampaign)
		{
			AnalyticsEventData.TutorialStepStoped();
		}
		if (!GameController.GameManager.IsMultiplayer)
		{
			this.PauseGame();
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0006F7B8 File Offset: 0x0006D9B8
	private void OnDisable()
	{
		this.UnpauseGame();
		if (!this.exitGameDialog.activeSelf)
		{
			if (!GameController.Instance.GameIsLoaded)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay((Screens)Enum.Parse(typeof(Screens), this.mainScreen), Contexts.ingame);
			}
			AnalyticsEventData.TutorialStepStarted();
		}
		if (PlatformManager.IsStandalone)
		{
			if (!this.IsMenuElementVisible())
			{
				CameraControler.BlockEverything = false;
				CameraControler.CameraMovementBlocked = false;
				return;
			}
		}
		else
		{
			CameraControler.BlockEverything = false;
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0002BF2E File Offset: 0x0002A12E
	private void PauseGame()
	{
		Time.timeScale = 0f;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0002BF3A File Offset: 0x0002A13A
	private void UnpauseGame()
	{
		if (!GameController.GameManager.IsMultiplayer)
		{
			if (!GameController.GameManager.IsCampaign)
			{
				if (!this.IsMenuElementVisible())
				{
					ShowEnemyMoves.Instance.OnPauseEnd();
					return;
				}
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0002BF71 File Offset: 0x0002A171
	public void OnLoadButtonClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_load_game_window_button);
		WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0002BF82 File Offset: 0x0002A182
	public void OnSaveButtonClicked()
	{
		WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_save_game_window_button);
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002BF93 File Offset: 0x0002A193
	public void OnGameSaved()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002BFA2 File Offset: 0x0002A1A2
	public void OnEnableMenuClicked(bool enable)
	{
		if (!enable)
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
		}
		else
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.mainScreen = AnalyticsEventData.ScreenCurrent();
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_ingame_menu_button);
		}
		this.ShowMenu(enable);
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0006F830 File Offset: 0x0006DA30
	public void ShowMenu(bool show)
	{
		if (this.OptionsMenu.activeInHierarchy)
		{
			bool flag = false;
			foreach (Dropdown dropdown in this.OptionsMenu.GetComponentsInChildren<Dropdown>())
			{
				if (dropdown.GetComponentsInChildren<ScrollRect>(false).Length != 0)
				{
					dropdown.Hide();
					flag = true;
				}
			}
			if (!flag)
			{
				this.OptionsMenu.SetActive(false);
				base.gameObject.SetActive(true);
				return;
			}
		}
		else
		{
			if (show)
			{
				this.LoadSaveWindow.SetActive(false);
				this.ButtonsPanel.SetActive(true);
				this.LoadButton.gameObject.SetActive(!GameController.GameManager.IsMultiplayer && GameController.GameManager.missionId < 0);
				this.SaveButton.gameObject.SetActive(!GameController.GameManager.IsMultiplayer && GameController.GameManager.missionId < 0);
				if (PlatformManager.IsMobile)
				{
					this.OptionsButton.gameObject.SetActive(GameController.GameManager.missionId < 0);
				}
				if (!GameController.GameManager.IsMultiplayer)
				{
					this.LoadButton.interactable = GameSavesManager.SaveSlotExists("ScytheSaveTmp.xml");
					if (GameController.GameManager.CanGameBeSaved())
					{
						this.SaveButton.interactable = true;
						this.blockSaveExplanation.SetActive(false);
						this.dividerText.SetActive(false);
					}
					else
					{
						this.SaveButton.interactable = false;
						this.blockSaveExplanation.SetActive(true);
						this.dividerText.SetActive(true);
					}
				}
				else
				{
					this.dividerText.SetActive(false);
				}
				if (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsAsynchronous && !GameController.GameManager.SpectatorMode)
				{
					this.ForfeitButton.gameObject.SetActive(true);
				}
				else
				{
					this.ForfeitButton.gameObject.SetActive(false);
				}
				if (CameraControler.Instance.tooltip != null)
				{
					CameraControler.Instance.tooltip.gameObject.SetActive(false);
				}
				CameraControler.Instance.mouselastPos = Vector3.zero;
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.ingame_menu, Contexts.ingame);
			}
			this.OptionsMenu.SetActive(false);
			base.gameObject.SetActive(show);
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
	public void ShowOptions()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_option_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.settings, Contexts.ingame);
		this.OptionsMenu.SetActive(true);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0002C006 File Offset: 0x0002A206
	public void HideOptions()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.ingame_menu, Contexts.ingame);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
		this.OptionsMenu.SetActive(false);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0002C033 File Offset: 0x0002A233
	public void OnHideSaveLoadMenu()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.ingame_menu, Contexts.ingame);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0002C042 File Offset: 0x0002A242
	public void OnBackButtonClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
		this.ShowMenu(true);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0006FA60 File Offset: 0x0006DC60
	public void OnExitGameDialogShow()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_exit_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.exit_game_popup, Contexts.ingame);
		this.exitGameDialog.SetActive(true);
		this.exitGameDialog.GetComponent<ExitGameDialogController>().Init();
		this.ShowMenu(false);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002C051 File Offset: 0x0002A251
	public void OnExitGameDialogRefusal()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_no_button);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		this.exitGameDialog.SetActive(false);
		this.ShowMenu(true);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0002C079 File Offset: 0x0002A279
	public void OnForfeitGameDialogShow()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_surrender_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.surrender_game_popup, Contexts.ingame);
		this.surrenderGameDialog.SetActive(true);
		this.ShowMenu(false);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0002C0AE File Offset: 0x0002A2AE
	public void OnForfeitGameDialogRefusal()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_no_button);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		this.surrenderGameDialog.SetActive(false);
		this.ShowMenu(true);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0002C0D6 File Offset: 0x0002A2D6
	public void OnExitGameDialogClose()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		this.CloseExitGameDialog();
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
	public void CloseExitGameDialog()
	{
		AnalyticsEventLogger.Instance.LogScreenDisplay((Screens)Enum.Parse(typeof(Screens), this.mainScreen), Contexts.ingame);
		this.exitGameDialog.SetActive(false);
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0006FAB0 File Offset: 0x0006DCB0
	public bool IsMenuElementVisible()
	{
		return base.gameObject.activeInHierarchy || this.LoadSaveWindow.activeInHierarchy || this.exitGameDialog.activeInHierarchy || this.surrenderGameDialog.activeInHierarchy || this.OptionsMenu.activeInHierarchy;
	}

	// Token: 0x04000563 RID: 1379
	public GameObject LoadSaveWindow;

	// Token: 0x04000564 RID: 1380
	public Button LoadButton;

	// Token: 0x04000565 RID: 1381
	public Button SaveButton;

	// Token: 0x04000566 RID: 1382
	public Button OptionsButton;

	// Token: 0x04000567 RID: 1383
	public Button ExitButton;

	// Token: 0x04000568 RID: 1384
	public Button ForfeitButton;

	// Token: 0x04000569 RID: 1385
	public GameObject blockSaveExplanation;

	// Token: 0x0400056A RID: 1386
	public GameObject dividerText;

	// Token: 0x0400056B RID: 1387
	public GameObject exitGameDialog;

	// Token: 0x0400056C RID: 1388
	public GameObject surrenderGameDialog;

	// Token: 0x0400056D RID: 1389
	public GameObject ButtonsPanel;

	// Token: 0x0400056E RID: 1390
	public GameObject OptionsMenu;

	// Token: 0x0400056F RID: 1391
	private string mainScreen = "in_game";
}
