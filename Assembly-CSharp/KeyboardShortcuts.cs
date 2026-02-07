using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000057 RID: 87
public class KeyboardShortcuts : MonoBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00029AC0 File Offset: 0x00027CC0
	// (set) Token: 0x060002B1 RID: 689 RVA: 0x00029AC7 File Offset: 0x00027CC7
	public static KeyboardShortcuts Instance { get; set; }

	// Token: 0x060002B2 RID: 690 RVA: 0x0005D658 File Offset: 0x0005B858
	private void Start()
	{
		KeyboardShortcuts.Instance = this;
		foreach (KeyboardShortcut keyboardShortcut in this.keyboardShortcuts)
		{
			if (!this.usedKeys.Contains(keyboardShortcut.keyCode))
			{
				this.usedKeys.Add(keyboardShortcut.keyCode);
			}
		}
		this.matPlayerPresenter = this.playerMat.GetComponent<MatPlayerPresenter>();
		this.exPanelPres = this.exchangePanel.GetComponent<ExchangePanelPresenter>();
		GameController.OnGameLoaded += this.GameController_OnGameLoaded;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00029ACF File Offset: 0x00027CCF
	private void OnDestroy()
	{
		GameController.OnGameLoaded -= this.GameController_OnGameLoaded;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0005D704 File Offset: 0x0005B904
	private void Update()
	{
		if (GameController.GameManager.SpectatorMode || this.shortcutsLocked)
		{
			return;
		}
		if (OptionsManager.IsShotcutsActive() && !this.introductionPanel.activeSelf)
		{
			this.KeysDownScanner();
			this.KeysPressedScanner();
			this.KeysUpScanner();
			if (!this.chat.GetComponent<Chat>().chatElements.activeSelf || !this.chat.GetComponent<Chat>().isFocused)
			{
				if (GameController.Instance.menu.GetComponent<GameMenu>().IsMenuElementVisible() || (!GameController.Instance.menu.GetComponent<GameMenu>().IsMenuElementVisible() && PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SHORTCUTS_ENABLED, 1) != 1))
				{
					if (Input.GetKeyDown(KeyCode.Escape))
					{
						if (this.LoadSaveContainer.activeSelf)
						{
							this.SaveLoadButton.GetComponent<Button>().onClick.Invoke();
						}
						else
						{
							GameController.Instance.MenuOpening();
							this.isShowiHideOptions = true;
						}
						this.chat.GetComponent<Chat>().isFocused = false;
						CameraControler.CameraMovementBlocked = false;
					}
				}
				else if (PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SHORTCUTS_ENABLED, 1) == 1)
				{
					this.KeyInterpreter();
				}
			}
			this.ShowHideKeyInfo();
		}
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0005D82C File Offset: 0x0005BA2C
	public void ShowHideKeyInfo()
	{
		if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && !this.chat.GetComponent<Chat>().isFocused && PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_SHORTCUTS_ENABLED, 1) == 1)
		{
			foreach (GameObject gameObject in this.keyInfo)
			{
				if (gameObject.name != "KeyInfoRevealCombatCards")
				{
					gameObject.SetActive(true);
				}
				if (gameObject.name == "KeyInfoRevealCombatCards")
				{
					if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode && GameController.Instance.AmmoCardsInvisible())
					{
						gameObject.SetActive(true);
					}
					else
					{
						gameObject.SetActive(false);
					}
				}
			}
			foreach (GameObject gameObject2 in this.FactionLineKeyInfo)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(true);
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt) || (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)))
		{
			GameObject[] array = this.keyInfo;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			foreach (GameObject gameObject3 in this.FactionLineKeyInfo)
			{
				if (gameObject3 != null)
				{
					gameObject3.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0005D9F0 File Offset: 0x0005BBF0
	public void RemoveUnnecessaryFactionKeyInfo()
	{
		for (int i = this.FactionLineKeyInfo.Count - 1; i > 0; i--)
		{
			if (this.FactionLineKeyInfo[i] == null)
			{
				this.FactionLineKeyInfo.Remove(this.FactionLineKeyInfo[i]);
			}
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00029AE2 File Offset: 0x00027CE2
	public void SortActionKeys(List<MatPlayerSectionPresenter> matSection)
	{
		matSection[0].topActionPresenter.gainActionButton[1].onClick.Invoke();
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0005DA44 File Offset: 0x0005BC44
	private void KeysDownScanner()
	{
		foreach (KeyCode keyCode in this.usedKeys)
		{
			if (Input.GetKeyDown(keyCode) && !this.keysDown.Contains(keyCode))
			{
				this.keysDown.Add(keyCode);
			}
			else if (!Input.GetKeyDown(keyCode) && this.keysDown.Contains(keyCode))
			{
				this.keysDown.Remove(keyCode);
			}
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0005DAD8 File Offset: 0x0005BCD8
	private void KeysPressedScanner()
	{
		foreach (KeyCode keyCode in this.usedKeys)
		{
			if (Input.GetKey(keyCode) && !this.keysPressed.Contains(keyCode))
			{
				this.keysPressed.Add(keyCode);
			}
			else if (!Input.GetKey(keyCode) && this.keysPressed.Contains(keyCode))
			{
				this.keysPressed.Remove(keyCode);
			}
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0005DB6C File Offset: 0x0005BD6C
	private void KeysUpScanner()
	{
		foreach (KeyCode keyCode in this.usedKeys)
		{
			if (Input.GetKeyUp(keyCode) && !this.keysUp.Contains(keyCode))
			{
				this.keysUp.Add(keyCode);
			}
			else if (!Input.GetKeyUp(keyCode) && this.keysUp.Contains(keyCode))
			{
				this.keysUp.Remove(keyCode);
			}
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0005DC00 File Offset: 0x0005BE00
	private void KeyInterpreter()
	{
		foreach (KeyboardShortcut keyboardShortcut in this.keyboardShortcuts)
		{
			foreach (KeyCode keyCode in this.keysDown)
			{
				if (keyboardShortcut.keyCode == keyCode)
				{
					this.KeyDownActionLauncher(keyboardShortcut.action);
				}
			}
			foreach (KeyCode keyCode2 in this.keysPressed)
			{
				if (keyboardShortcut.keyCode == keyCode2)
				{
					this.KeyPressedActionLauncher(keyboardShortcut.action);
				}
			}
			foreach (KeyCode keyCode3 in this.keysUp)
			{
				if (keyboardShortcut.keyCode == keyCode3)
				{
					this.KeyUpActionLauncher(keyboardShortcut.action);
				}
			}
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0005DD48 File Offset: 0x0005BF48
	private void KeyDownActionLauncher(KeyboardShortcut.ActionsEnum _action)
	{
		this.bombsAllocated = this.combatPanel.GetComponent<CombatPreperationPresenter>().selectedCards.Count;
		this.availableBombSlots = this.combatPanel.GetComponent<CombatPreperationPresenter>().numberOfCardsToUse;
		switch (_action)
		{
		case KeyboardShortcut.ActionsEnum.CameraUp:
			if (this.combatPanel.activeSelf && !this.isCombatFinished && !this.maximizeButton.activeSelf)
			{
				Slider component = this.leftPowerSlider.GetComponent<Slider>();
				float num = component.value;
				component.value = num + 1f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraDown:
			if (this.combatPanel.activeSelf && !this.isCombatFinished && !this.maximizeButton.activeSelf)
			{
				Slider component2 = this.leftPowerSlider.GetComponent<Slider>();
				float num = component2.value;
				component2.value = num - 1f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraLeft:
			if (this.combatPanel.activeSelf && !this.isCombatFinished && !this.maximizeButton.activeSelf)
			{
				Slider component3 = this.leftPowerSlider.GetComponent<Slider>();
				float num = component3.value;
				component3.value = num - 1f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraRight:
			if (this.combatPanel.activeSelf && !this.isCombatFinished && !this.maximizeButton.activeSelf)
			{
				Slider component4 = this.leftPowerSlider.GetComponent<Slider>();
				float num = component4.value;
				component4.value = num + 1f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraZoomIn:
			this.isCameraZoomIn = true;
			return;
		case KeyboardShortcut.ActionsEnum.CameraZoomOut:
			this.isCameraZoomOut = true;
			return;
		case KeyboardShortcut.ActionsEnum.CameraRorateLeft:
			CameraControler.Instance.rotationSnapStart = Time.time - 10f;
			this.isCameraRotateLeft = true;
			return;
		case KeyboardShortcut.ActionsEnum.CameraRotateRight:
			CameraControler.Instance.rotationSnapStart = Time.time - 10f;
			this.isCameraRotateRight = true;
			return;
		case KeyboardShortcut.ActionsEnum.ActionAccept:
			if (this.startButton.activeInHierarchy)
			{
				this.startButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("1");
				return;
			}
			if (this.clickToUncoverButton.activeSelf && this.encounterCard.activeSelf)
			{
				this.clickToUncoverButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("2");
				return;
			}
			if (this.simpleAmountPresenter.activeSelf)
			{
				this.takeButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("3");
				return;
			}
			if (this.actionAcceptButton.transform.parent.gameObject.activeSelf && !this.isYesNoButtonVisible)
			{
				this.actionAcceptButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("4");
				return;
			}
			if (this.combatPanel.activeSelf && !this.TurnInfoPanel.activeSelf)
			{
				if (this.fightButton.GetComponent<Button>().interactable && this.combatPanel.GetComponent<CombatPanel>().State != CombatPanelAction.wait && this.combatPanel.GetComponent<CombatPanel>().State != CombatPanelAction.wait_for_opponent)
				{
					this.fightButton.GetComponent<Button>().onClick.Invoke();
				}
				Debug.Log("5");
				return;
			}
			if (this.recruitBonusAcceptButton.GetComponent<Button>().interactable)
			{
				this.recruitBonusAcceptButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("6");
				return;
			}
			if (this.diversionPanel.activeSelf && !this.TurnInfoPanel.activeSelf)
			{
				this.diversionPanelYes.GetComponent<Button>().onClick.Invoke();
				Debug.Log("7");
				return;
			}
			if (this.OptionSelectionPanel.activeSelf && !this.TurnInfoPanel.activeSelf)
			{
				this.OptionSelectionPanelAcceptButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("8");
				return;
			}
			if (this.PeekCombatCardPresenter.activeSelf && this.PeekCardOKButton.activeSelf)
			{
				this.PeekCardOKButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("9");
				return;
			}
			if (this.gainWorkerPresenter.activeSelf && this.gainWokrerOptionSelection.activeSelf)
			{
				this.gainWorkerAcceptButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("10");
				return;
			}
			if (this.noMoreCardsPanel.activeSelf)
			{
				this.noMoreCardsButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.dismissButton.transform.parent.gameObject.activeSelf)
			{
				Debug.Log("Dismiss encounter");
				this.dismissButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			this.DoMouseDown();
			return;
		case KeyboardShortcut.ActionsEnum.EndTurn:
			if (GameController.GameManager.combatManager.IsPlayerInCombat() || (GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsMyTurn()))
			{
				return;
			}
			if (this.endTradeButton.transform.parent.gameObject.activeSelf && this.endTradeButton.GetComponent<Button>().IsInteractable())
			{
				this.endTradeButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.EndProductionButton.activeSelf && this.EndProductionButton.GetComponent<Button>().IsInteractable())
			{
				this.EndProductionButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.EndMoveButton.activeSelf && this.EndMoveButton.GetComponent<Button>().IsInteractable())
			{
				this.EndMoveButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.endTurnButton.GetComponent<Button>().IsInteractable())
			{
				this.endTurnButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.SelectAnother:
		case KeyboardShortcut.ActionsEnum.SlowCameraMovementDown:
			break;
		case KeyboardShortcut.ActionsEnum.ShowiHideOptions:
			if (this.encounterCard.activeSelf && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				if (this.gainWorkerPresenter.activeSelf)
				{
					this.gainWorkerRejectButton.GetComponent<Button>().onClick.Invoke();
					Debug.Log("Gain worker reject");
					return;
				}
				Debug.Log("dismiss encounter");
				this.dismissEncounterButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			else
			{
				if (this.winnerPreviewPresenter.activeSelf)
				{
					Debug.Log("Close score preview panel");
					this.winnerPreviewPresenterExitButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.finalWinnerPreviewPresenter.activeSelf)
				{
					Debug.Log("Close final score preview panel");
					this.finalWinnerPreviewPresenterExitButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.diversionPanel.activeSelf)
				{
					Debug.Log("steal combat card by Crimea");
					this.diversionPanelYes.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.OptionSelectionPanel.activeSelf)
				{
					Debug.Log("option selection panel");
					this.OptionSelectionPanelRejectButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.gainWorkerPresenter.activeSelf)
				{
					Debug.Log("gain worker");
					this.gainWorkerRejectButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (!this.isYesNoButtonVisible)
				{
					GameController.Instance.MenuOpening();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Undo:
			if (!this.darkenUI.GetComponent<Image>().IsActive() && this.undoButton.GetComponent<Button>().interactable)
			{
				this.undoButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.ShowHideHUD:
			this.isShowHideHUD = true;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideStarTab:
			if (!this.starTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.starTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.starTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideScoreTab:
			if (!this.scoreTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.scoreTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.scoreTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideStructureTab:
			if (!this.structureTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.structureTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.structureTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideObjectiveTab:
			if (!this.objectiveTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.objectiveTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.objectiveTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideRecruitmentTab:
			if (!this.recruitmentTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.recruitmentTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.recruitmentTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideMechTab:
			if (!this.mechTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.mechTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.mechTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ShowHideFactionTab:
			if (!this.factionTabToggle.GetComponent<Toggle2>().isOn)
			{
				this.factionTabToggle.GetComponent<Toggle2>().isOn = true;
				return;
			}
			this.factionTabToggle.GetComponent<Toggle2>().isOn = false;
			return;
		case KeyboardShortcut.ActionsEnum.ExpandTopPanel:
			this.stats.GetComponent<Scythe.UI.PlayerStatsPresenter>().Expand();
			return;
		case KeyboardShortcut.ActionsEnum.ExpandDownTopPanel:
			this.stats.GetComponent<Scythe.UI.PlayerStatsPresenter>().ExpandDown();
			return;
		case KeyboardShortcut.ActionsEnum.FactoryTop1:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.factoryCardSlot.GetComponent<FactoryMatPlayerSectionPresenter>().topActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.FactoryTop2:
			if (this.bottomBar.activeSelf)
			{
				if (this.matPlayerPresenter.factoryCardSlot.GetComponent<FactoryMatPlayerSectionPresenter>().topActionPresenter.gainActionButton.Length > 1)
				{
					this.matPlayerPresenter.factoryCardSlot.GetComponent<FactoryMatPlayerSectionPresenter>().topActionPresenter.gainActionButton[1].onClick.Invoke();
				}
				else
				{
					this.matPlayerPresenter.factoryCardSlot.GetComponent<FactoryMatPlayerSectionPresenter>().topActionPresenter.gainActionButton[0].onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.FactoryBottom:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.factoryCardSlot.GetComponent<FactoryMatPlayerSectionPresenter>().downActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section1Top1:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[0].topActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section1Top2:
			if (this.bottomBar.activeSelf)
			{
				if (this.matPlayerPresenter.matSection[0].topActionPresenter.gainActionButton.Length > 1)
				{
					this.matPlayerPresenter.matSection[0].topActionPresenter.gainActionButton[1].onClick.Invoke();
				}
				else
				{
					this.matPlayerPresenter.matSection[0].topActionPresenter.gainActionButton[0].onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section1Bottom:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[0].downActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section2Top1:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[1].topActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section2Top2:
			if (this.bottomBar.activeSelf)
			{
				if (this.matPlayerPresenter.matSection[1].topActionPresenter.gainActionButton.Length > 1)
				{
					this.matPlayerPresenter.matSection[1].topActionPresenter.gainActionButton[1].onClick.Invoke();
				}
				else
				{
					this.matPlayerPresenter.matSection[1].topActionPresenter.gainActionButton[0].onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section2Bottom:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[1].downActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section3Top1:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[2].topActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section3Top2:
			if (this.bottomBar.activeSelf)
			{
				if (this.matPlayerPresenter.matSection[2].topActionPresenter.gainActionButton.Length > 1)
				{
					this.matPlayerPresenter.matSection[2].topActionPresenter.gainActionButton[1].onClick.Invoke();
				}
				else
				{
					this.matPlayerPresenter.matSection[2].topActionPresenter.gainActionButton[0].onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section3Bottom:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[2].downActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section4Top1:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[3].topActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section4Top2:
			if (this.bottomBar.activeSelf)
			{
				if (this.matPlayerPresenter.matSection[3].topActionPresenter.gainActionButton.Length > 1)
				{
					this.matPlayerPresenter.matSection[3].topActionPresenter.gainActionButton[1].onClick.Invoke();
				}
				else
				{
					this.matPlayerPresenter.matSection[3].topActionPresenter.gainActionButton[0].onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Section4Bottom:
			if (this.bottomBar.activeSelf)
			{
				this.matPlayerPresenter.matSection[3].downActionPresenter.gainActionButton[0].onClick.Invoke();
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadEverything:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (!this.isLoadedAllToMech)
				{
					this.exPanelPres.PassAllToUnit();
					this.exPanelPres.LoadAllWorkersToMech();
					this.isLoadedAllToMech = true;
					return;
				}
				this.exPanelPres.PassAllToField();
				this.exPanelPres.UnloadAllWorkersFromMech();
				this.isLoadedAllToMech = false;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadAllUnloadOneWorker:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.GetContext().HexWorkers.Count > 0 && this.exPanelPres.GetContext().LoadedWorkers.Count == 0)
				{
					this.exPanelPres.LoadAllWorkersToMech();
					return;
				}
				this.exPanelPres.UnloadWorkerFromMech(0);
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadWood:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.exchangePanels[ResourceType.wood].Value == 0)
				{
					this.allToUnitWood.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.oneToFieldWood.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadFood:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.exchangePanels[ResourceType.food].Value == 0)
				{
					this.allToUnitFood.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.oneToFieldFood.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadOil:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.exchangePanels[ResourceType.oil].Value == 0)
				{
					this.allToUnitOil.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.oneToFieldOil.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadMetal:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.exchangePanels[ResourceType.metal].Value == 0)
				{
					this.allToUnitMetal.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.oneToFieldMetal.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.LoadUnloadAllResources:
			if (this.exchangePanel.gameObject.activeSelf && !GameController.Instance.hook.IsSomethingDragged())
			{
				if (this.exPanelPres.exchangePanels[ResourceType.oil].Value == 0 && this.exPanelPres.exchangePanels[ResourceType.metal].Value == 0 && this.exPanelPres.exchangePanels[ResourceType.wood].Value == 0 && this.exPanelPres.exchangePanels[ResourceType.food].Value == 0)
				{
					this.loadAllResources.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.unloadAllResources.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.SendChatMsg:
			this.isSendingChatMsg = true;
			return;
		case KeyboardShortcut.ActionsEnum.OpenCloseChat:
			Debug.Log("Open Close Chat");
			this.isCloseChatWindow = true;
			this.chat.GetComponent<Chat>().isFocused = false;
			CameraControler.CameraMovementBlocked = false;
			if (this.chatButton.activeSelf)
			{
				this.chatButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.ExpandShrinkBottomPanel:
			if (this.bottomBar.activeSelf)
			{
				if (this.bottomShrinkButton.activeSelf)
				{
					this.bottomShrinkButton.GetComponent<Button>().onClick.Invoke();
				}
				else
				{
					this.bottomExpandButton.GetComponent<Button>().onClick.Invoke();
				}
				this.HideUsedLastTurnText();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.FastForward:
			this.fastForwardButton.GetComponent<Button>().onClick.Invoke();
			return;
		case KeyboardShortcut.ActionsEnum.Choice1:
			if (this.factionMat.GetComponent<MatFactionPresenter>().mechButton[0].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().mechButton[0].onClick.Invoke();
				return;
			}
			if (this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[0].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[0].onClick.Invoke();
				return;
			}
			if (this.recruitmentBonus1.activeSelf)
			{
				this.recruitmentBonus1.GetComponent<Button>().onClick.Invoke();
				Debug.Log("3");
				return;
			}
			if (this.factionTab.activeSelf)
			{
				this.riverwalkButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(0);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			if (this.PayAnyResourcePanel.activeSelf)
			{
				this.GainOilButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.combatPanel.activeSelf && !this.maximizeButton.activeSelf && GameController.GameManager.combatManager.IsPlayerInCombat() && GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				this.RemoveAllBombs();
				return;
			}
			if (this.encounterCard.activeSelf && !this.simpleAmountPresenter.activeSelf && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				Button button = this.encounterCard.GetComponent<EncounterCardPresenter>().optionButtons[0];
				if (button.interactable && !this.encounterCard.GetComponent<EncounterCardPresenter>().minimized)
				{
					button.onClick.Invoke();
					return;
				}
				if (this.PeekCombatCardPresenter.activeSelf && this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection.Length >= 1)
				{
					this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection[0].onClick.Invoke();
					return;
				}
			}
			else
			{
				if (this.matPlayerPresenter.matSection[0].topActionPresenter.buildingButton.gameObject.activeSelf)
				{
					this.matPlayerPresenter.matSection[0].topActionPresenter.buildingButton.onClick.Invoke();
					return;
				}
				if (this.Objective1Button.GetComponent<Button>().interactable)
				{
					this.Objective1Button.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.scoreTab.activeSelf)
				{
					this.ScorePreviewButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice2:
			if (this.factionMat.GetComponent<MatFactionPresenter>().mechButton[1].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().mechButton[1].onClick.Invoke();
				return;
			}
			if (this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[1].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[1].onClick.Invoke();
				return;
			}
			if (this.recruitmentBonus2.activeSelf)
			{
				this.recruitmentBonus2.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(1);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			if (this.PayAnyResourcePanel.activeSelf)
			{
				this.GainMetalButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.combatPanel.activeSelf && this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[0].gameObject.activeSelf && !this.maximizeButton.activeSelf && GameController.GameManager.combatManager.IsPlayerInCombat() && GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				if (this.bombsAllocated < this.availableBombSlots)
				{
					this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[0].onClick.Invoke();
					return;
				}
			}
			else if (this.encounterCard.activeSelf && !this.simpleAmountPresenter.activeSelf && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				Button button2 = this.encounterCard.GetComponent<EncounterCardPresenter>().optionButtons[1];
				if (button2.interactable && !this.encounterCard.GetComponent<EncounterCardPresenter>().minimized)
				{
					button2.onClick.Invoke();
					return;
				}
				if (this.PeekCombatCardPresenter.activeSelf && this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection.Length >= 2)
				{
					this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection[1].onClick.Invoke();
					return;
				}
			}
			else
			{
				if (this.matPlayerPresenter.matSection[1].topActionPresenter.buildingButton.gameObject.activeSelf)
				{
					this.matPlayerPresenter.matSection[1].topActionPresenter.buildingButton.onClick.Invoke();
					return;
				}
				if (this.Objective2Button.GetComponent<Button>().interactable)
				{
					this.Objective2Button.GetComponent<Button>().onClick.Invoke();
					return;
				}
				if (this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[0].gameObject.activeSelf)
				{
					this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[0].onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice3:
			if (this.factionMat.GetComponent<MatFactionPresenter>().mechButton[2].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().mechButton[2].onClick.Invoke();
				return;
			}
			if (this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[2].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[2].onClick.Invoke();
				return;
			}
			if (this.recruitmentBonus3.activeSelf)
			{
				this.recruitmentBonus3.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(2);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			if (this.PayAnyResourcePanel.activeSelf)
			{
				this.GainWoodButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.combatPanel.activeSelf && this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[1].gameObject.activeSelf && !this.maximizeButton.activeSelf && GameController.GameManager.combatManager.IsPlayerInCombat() && GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				if (this.bombsAllocated < this.availableBombSlots)
				{
					this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[1].onClick.Invoke();
					return;
				}
			}
			else if (this.encounterCard.activeSelf && !this.simpleAmountPresenter.activeSelf && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				Button button3 = this.encounterCard.GetComponent<EncounterCardPresenter>().optionButtons[2];
				if (button3.interactable && !this.encounterCard.GetComponent<EncounterCardPresenter>().minimized)
				{
					button3.onClick.Invoke();
					return;
				}
				if (this.PeekCombatCardPresenter.activeSelf && this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection.Length >= 3)
				{
					this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection[2].onClick.Invoke();
					return;
				}
			}
			else
			{
				if (this.matPlayerPresenter.matSection[2].topActionPresenter.buildingButton.gameObject.activeSelf)
				{
					this.matPlayerPresenter.matSection[2].topActionPresenter.buildingButton.onClick.Invoke();
					return;
				}
				if (this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[1].gameObject.activeSelf)
				{
					this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[1].onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice4:
			if (this.factionMat.GetComponent<MatFactionPresenter>().mechButton[3].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().mechButton[3].onClick.Invoke();
				return;
			}
			if (this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[3].interactable)
			{
				this.factionMat.GetComponent<MatFactionPresenter>().recruitButton[3].onClick.Invoke();
				return;
			}
			if (this.recruitmentBonus4.activeSelf)
			{
				this.recruitmentBonus4.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || !GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(3);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			if (this.PayAnyResourcePanel.activeSelf)
			{
				this.GainFoodButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.combatPanel.activeSelf && this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[2].gameObject.activeSelf && !this.maximizeButton.activeSelf && GameController.GameManager.combatManager.IsPlayerInCombat() && GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				if (this.bombsAllocated < this.availableBombSlots)
				{
					this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[2].onClick.Invoke();
					return;
				}
			}
			else if (this.encounterCard.activeSelf && !this.simpleAmountPresenter.activeSelf && (!GameController.GameManager.IsMultiplayer || !GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				if (this.PeekCombatCardPresenter.activeSelf && this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection.Length >= 4)
				{
					this.PeekCombatCardPresenter.GetComponent<PeekCombatCardPresenter>().factionSelection[3].onClick.Invoke();
					return;
				}
			}
			else
			{
				if (this.matPlayerPresenter.matSection[3].topActionPresenter.buildingButton.gameObject.activeSelf)
				{
					this.matPlayerPresenter.matSection[3].topActionPresenter.buildingButton.onClick.Invoke();
					return;
				}
				if (this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[2].gameObject.activeSelf)
				{
					this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[2].onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice5:
			if (this.combatPanel.activeSelf && this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[3].gameObject.activeSelf && !this.maximizeButton.activeSelf && GameController.GameManager.combatManager.IsPlayerInCombat() && GameController.GameManager.combatManager.GetActualStage() == CombatStage.Preparation)
			{
				if (this.bombsAllocated < this.availableBombSlots)
				{
					this.combatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[3].onClick.Invoke();
					return;
				}
			}
			else
			{
				if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
				{
					this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(4);
					this.RemoveUnnecessaryFactionKeyInfo();
					return;
				}
				if (this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[3].gameObject.activeSelf)
				{
					this.BottomBarCombatCardPresenter.GetComponent<CombatCardsPanelPresenter>().bombs[3].onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice6:
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(5);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice7:
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(6);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.Choice8:
			if (this.FactoryCardSelectionContainer.transform.childCount > 0 && (!GameController.GameManager.IsMultiplayer || (GameController.GameManager.IsMyTurn() && GameController.GameManager.IsMultiplayer)))
			{
				this.FactoryCardPresenter.GetComponent<FactoryCardsPresenter>().ChooseCard(7);
				this.RemoveUnnecessaryFactionKeyInfo();
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.MinMaxOdrerPanel:
			if (this.combatPanel.activeSelf)
			{
				if (this.maximizeButton.activeSelf)
				{
					this.maximizeButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
				this.centerMinimizeButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			else
			{
				if (this.encounterCard.activeSelf)
				{
					this.encounterCard.GetComponent<EncounterCardPresenter>().KeyboardShortcut_ChangeSize();
					return;
				}
				if (this.winnerPreviewPresenter.activeSelf)
				{
					if (!this.winnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn)
					{
						this.winnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn = true;
						return;
					}
					this.winnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn = false;
					return;
				}
				else if (this.finalWinnerPreviewPresenter.activeSelf)
				{
					if (!this.finalWinnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn)
					{
						this.finalWinnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn = true;
						return;
					}
					this.finalWinnerPreviewShowMoreToggle.GetComponent<Toggle>().isOn = false;
					return;
				}
				else
				{
					if (this.bottomBar.transform.localScale.x <= 1f)
					{
						this.expandOrderPanelButton.GetComponent<Button>().onClick.Invoke();
						return;
					}
					this.shrinkOrderPanelButton.GetComponent<Button>().onClick.Invoke();
					return;
				}
			}
			break;
		case KeyboardShortcut.ActionsEnum.ChatFocus:
			if (this.chat.GetComponent<Chat>().chatElements.activeSelf)
			{
				Debug.Log("activate input field");
				this.messageToSend.GetComponent<InputField>().ActivateInputField();
				this.chat.GetComponent<Chat>().OnInputFieldEditBegin();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0005FF14 File Offset: 0x0005E114
	private void KeyPressedActionLauncher(KeyboardShortcut.ActionsEnum _action)
	{
		switch (_action)
		{
		case KeyboardShortcut.ActionsEnum.CameraUp:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.WorldCamera.GetComponent<CameraControler>().mouseLastMoved = Time.realtimeSinceStartup;
				if (Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode))
				{
					this.verticalMovement = Input.GetAxis("Vertical") / 4f;
					return;
				}
				this.verticalMovement = Input.GetAxis("Vertical");
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraDown:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.WorldCamera.GetComponent<CameraControler>().mouseLastMoved = Time.realtimeSinceStartup;
				if (Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode))
				{
					this.verticalMovement = Input.GetAxis("Vertical") / 4f;
					return;
				}
				this.verticalMovement = Input.GetAxis("Vertical");
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraLeft:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.WorldCamera.GetComponent<CameraControler>().mouseLastMoved = Time.realtimeSinceStartup;
				if (Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode))
				{
					this.horizontalMovement = Input.GetAxis("Horizontal") / 4f;
					return;
				}
				this.horizontalMovement = Input.GetAxis("Horizontal");
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraRight:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.WorldCamera.GetComponent<CameraControler>().mouseLastMoved = Time.realtimeSinceStartup;
				if (Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode))
				{
					this.horizontalMovement = Input.GetAxis("Horizontal") / 4f;
					return;
				}
				this.horizontalMovement = Input.GetAxis("Horizontal");
				return;
			}
			break;
		default:
			switch (_action)
			{
			case KeyboardShortcut.ActionsEnum.Choice1:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview1.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(0);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice2:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview7.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(1);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice3:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview6.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(2);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice4:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview5.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(3);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice5:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview4.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(4);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice6:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview3.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(5);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice7:
				if ((Input.GetKey(this.keyboardShortcuts[8].keyCode) || Input.GetKey(this.keyboardShortcuts[9].keyCode)) && this.FactionPreview2.activeSelf)
				{
					this.factionOrder.GetComponent<PlayerOrder>().LogoHovered(6);
					return;
				}
				break;
			case KeyboardShortcut.ActionsEnum.Choice8:
			case KeyboardShortcut.ActionsEnum.MinMaxOdrerPanel:
			case KeyboardShortcut.ActionsEnum.ChatFocus:
				break;
			case KeyboardShortcut.ActionsEnum.RevealCombatCards:
				if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode)
				{
					GameController.Instance.ShowCombatCards();
				}
				break;
			default:
				return;
			}
			break;
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0006041C File Offset: 0x0005E61C
	private void KeyUpActionLauncher(KeyboardShortcut.ActionsEnum _action)
	{
		switch (_action)
		{
		case KeyboardShortcut.ActionsEnum.CameraUp:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.verticalMovement = 0f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraDown:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.verticalMovement = 0f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraLeft:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.horizontalMovement = 0f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraRight:
			if (!this.combatPanel.activeSelf || this.maximizeButton.activeSelf)
			{
				this.horizontalMovement = 0f;
				return;
			}
			break;
		case KeyboardShortcut.ActionsEnum.CameraZoomIn:
			this.isCameraZoomIn = false;
			return;
		case KeyboardShortcut.ActionsEnum.CameraZoomOut:
			this.isCameraZoomOut = false;
			return;
		case KeyboardShortcut.ActionsEnum.CameraRorateLeft:
			CameraControler.Instance.rotationKeyUsed = true;
			this.WorldCamera.GetComponent<CameraControler>().SmoothRotationReturn();
			this.isCameraRotateLeft = false;
			return;
		case KeyboardShortcut.ActionsEnum.CameraRotateRight:
			CameraControler.Instance.rotationKeyUsed = true;
			this.WorldCamera.GetComponent<CameraControler>().SmoothRotationReturn();
			this.isCameraRotateRight = false;
			return;
		case KeyboardShortcut.ActionsEnum.ActionAccept:
			this.DoMouseUp();
			return;
		default:
			switch (_action)
			{
			case KeyboardShortcut.ActionsEnum.Choice1:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(0);
				return;
			case KeyboardShortcut.ActionsEnum.Choice2:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(1);
				return;
			case KeyboardShortcut.ActionsEnum.Choice3:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(2);
				return;
			case KeyboardShortcut.ActionsEnum.Choice4:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(3);
				return;
			case KeyboardShortcut.ActionsEnum.Choice5:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(4);
				return;
			case KeyboardShortcut.ActionsEnum.Choice6:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(5);
				return;
			case KeyboardShortcut.ActionsEnum.Choice7:
				this.factionOrder.GetComponent<PlayerOrder>().LogoReleased(6);
				return;
			case KeyboardShortcut.ActionsEnum.Choice8:
			case KeyboardShortcut.ActionsEnum.MinMaxOdrerPanel:
			case KeyboardShortcut.ActionsEnum.ChatFocus:
				break;
			case KeyboardShortcut.ActionsEnum.RevealCombatCards:
				if (GameController.GameManager.IsHotSeat && GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.SpectatorMode)
				{
					GameController.Instance.HideCombatCards();
				}
				break;
			default:
				return;
			}
			break;
		}
	}

	// Token: 0x060002BF RID: 703
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

	// Token: 0x060002C0 RID: 704 RVA: 0x00060638 File Offset: 0x0005E838
	public void DoMouseClick()
	{
		uint num = (uint)Input.mousePosition.x;
		uint num2 = (uint)Input.mousePosition.y;
		KeyboardShortcuts.mouse_event(6U, num, num2, 0U, 0U);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00060668 File Offset: 0x0005E868
	public void DoMouseDown()
	{
		uint num = (uint)Input.mousePosition.x;
		uint num2 = (uint)Input.mousePosition.y;
		KeyboardShortcuts.mouse_event(2U, num, num2, 0U, 0U);
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00060698 File Offset: 0x0005E898
	public void DoMouseUp()
	{
		uint num = (uint)Input.mousePosition.x;
		uint num2 = (uint)Input.mousePosition.y;
		KeyboardShortcuts.mouse_event(4U, num, num2, 0U, 0U);
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x000606C8 File Offset: 0x0005E8C8
	public void RemoveAllBombs()
	{
		foreach (GameObject gameObject in this.bombSpaces)
		{
			if (gameObject.activeSelf)
			{
				gameObject.GetComponent<Button>().onClick.Invoke();
			}
		}
		this.availableBombSlots = 0;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00029B01 File Offset: 0x00027D01
	public void HideUsedLastTurnText()
	{
		if (GameController.GameManager.PlayerCurrent.topActionFinished || GameController.GameManager.PlayerCurrent.topActionInProgress)
		{
			GameController.Instance.matPlayer.ShowSectionMatActionReloading(false);
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00029B35 File Offset: 0x00027D35
	private void GameController_OnGameLoaded()
	{
		this.shortcutsLocked = false;
	}

	// Token: 0x04000206 RID: 518
	public bool shortcutsOptionIsOn;

	// Token: 0x04000207 RID: 519
	public List<KeyboardShortcut> keyboardShortcuts = new List<KeyboardShortcut>();

	// Token: 0x04000208 RID: 520
	public List<KeyCode> usedKeys = new List<KeyCode>();

	// Token: 0x04000209 RID: 521
	public List<KeyCode> keysDown = new List<KeyCode>();

	// Token: 0x0400020A RID: 522
	public List<KeyCode> keysUp = new List<KeyCode>();

	// Token: 0x0400020B RID: 523
	public List<KeyCode> keysPressed = new List<KeyCode>();

	// Token: 0x0400020C RID: 524
	public GameObject[] keyInfo;

	// Token: 0x0400020D RID: 525
	public List<GameObject> FactionLineKeyInfo;

	// Token: 0x0400020E RID: 526
	public GameObject introductionPanel;

	// Token: 0x0400020F RID: 527
	[Header("Camera Actions")]
	public GameObject WorldCamera;

	// Token: 0x04000210 RID: 528
	public float verticalMovement;

	// Token: 0x04000211 RID: 529
	public float horizontalMovement;

	// Token: 0x04000212 RID: 530
	public bool isCameraZoomIn;

	// Token: 0x04000213 RID: 531
	public bool isCameraZoomOut;

	// Token: 0x04000214 RID: 532
	public bool isCameraRotateLeft;

	// Token: 0x04000215 RID: 533
	public bool isCameraRotateRight;

	// Token: 0x04000216 RID: 534
	public bool isCameraRotationSmoothing;

	// Token: 0x04000217 RID: 535
	[Header("Function Keys")]
	public GameObject darkenUI;

	// Token: 0x04000218 RID: 536
	public GameObject startButton;

	// Token: 0x04000219 RID: 537
	public GameObject endTurnButton;

	// Token: 0x0400021A RID: 538
	public GameObject actionAcceptButton;

	// Token: 0x0400021B RID: 539
	public GameObject endTradeButton;

	// Token: 0x0400021C RID: 540
	public GameObject EndMoveButton;

	// Token: 0x0400021D RID: 541
	public GameObject EndProductionButton;

	// Token: 0x0400021E RID: 542
	public GameObject recruitBonusAcceptButton;

	// Token: 0x0400021F RID: 543
	public GameObject diversionPanel;

	// Token: 0x04000220 RID: 544
	public GameObject diversionPanelYes;

	// Token: 0x04000221 RID: 545
	public GameObject diversionPaelNo;

	// Token: 0x04000222 RID: 546
	public GameObject bottomBar;

	// Token: 0x04000223 RID: 547
	public GameObject expandOrderPanelButton;

	// Token: 0x04000224 RID: 548
	public GameObject shrinkOrderPanelButton;

	// Token: 0x04000225 RID: 549
	public GameObject OptionSelectionPanel;

	// Token: 0x04000226 RID: 550
	public GameObject OptionSelectionPanelAcceptButton;

	// Token: 0x04000227 RID: 551
	public GameObject OptionSelectionPanelRejectButton;

	// Token: 0x04000228 RID: 552
	public GameObject Objective1Button;

	// Token: 0x04000229 RID: 553
	public GameObject Objective2Button;

	// Token: 0x0400022A RID: 554
	public GameObject scoreTab;

	// Token: 0x0400022B RID: 555
	public GameObject ScorePreviewButton;

	// Token: 0x0400022C RID: 556
	public GameObject factionTab;

	// Token: 0x0400022D RID: 557
	public GameObject riverwalkButton;

	// Token: 0x0400022E RID: 558
	public GameObject winnerPreviewPresenter;

	// Token: 0x0400022F RID: 559
	public GameObject winnerPreviewPresenterExitButton;

	// Token: 0x04000230 RID: 560
	public GameObject winnerPreviewShowMoreToggle;

	// Token: 0x04000231 RID: 561
	public GameObject finalWinnerPreviewPresenter;

	// Token: 0x04000232 RID: 562
	public GameObject finalWinnerPreviewPresenterExitButton;

	// Token: 0x04000233 RID: 563
	public GameObject finalWinnerPreviewShowMoreToggle;

	// Token: 0x04000234 RID: 564
	public GameObject gainWorkerPresenter;

	// Token: 0x04000235 RID: 565
	public GameObject gainWokrerOptionSelection;

	// Token: 0x04000236 RID: 566
	public GameObject gainWorkerAcceptButton;

	// Token: 0x04000237 RID: 567
	public GameObject gainWorkerRejectButton;

	// Token: 0x04000238 RID: 568
	public GameObject TurnInfoPanel;

	// Token: 0x04000239 RID: 569
	public GameObject noMoreCardsPanel;

	// Token: 0x0400023A RID: 570
	public GameObject noMoreCardsButton;

	// Token: 0x0400023B RID: 571
	public bool isYesNoButtonVisible;

	// Token: 0x0400023C RID: 572
	public bool isShowiHideOptions;

	// Token: 0x0400023D RID: 573
	public bool isShowHideHUD;

	// Token: 0x0400023E RID: 574
	public GameObject fastForwardButton;

	// Token: 0x0400023F RID: 575
	public GameObject LoadSaveContainer;

	// Token: 0x04000240 RID: 576
	public GameObject SaveLoadButton;

	// Token: 0x04000241 RID: 577
	[Header("Top row panels")]
	public GameObject starTabToggle;

	// Token: 0x04000242 RID: 578
	public GameObject scoreTabToggle;

	// Token: 0x04000243 RID: 579
	public GameObject structureTabToggle;

	// Token: 0x04000244 RID: 580
	public GameObject objectiveTabToggle;

	// Token: 0x04000245 RID: 581
	public GameObject recruitmentTabToggle;

	// Token: 0x04000246 RID: 582
	public GameObject mechTabToggle;

	// Token: 0x04000247 RID: 583
	public GameObject factionTabToggle;

	// Token: 0x04000248 RID: 584
	public GameObject stats;

	// Token: 0x04000249 RID: 585
	[Header("Top Right buttons")]
	public GameObject undoButton;

	// Token: 0x0400024A RID: 586
	public GameObject menuButton;

	// Token: 0x0400024B RID: 587
	[Header("Orders")]
	public GameObject factionMat;

	// Token: 0x0400024C RID: 588
	public GameObject playerMat;

	// Token: 0x0400024D RID: 589
	public MatPlayerPresenter matPlayerPresenter;

	// Token: 0x0400024E RID: 590
	public GameObject bottomExpandButton;

	// Token: 0x0400024F RID: 591
	public GameObject bottomShrinkButton;

	// Token: 0x04000250 RID: 592
	public GameObject recruitmentBonus1;

	// Token: 0x04000251 RID: 593
	public GameObject recruitmentBonus2;

	// Token: 0x04000252 RID: 594
	public GameObject recruitmentBonus3;

	// Token: 0x04000253 RID: 595
	public GameObject recruitmentBonus4;

	// Token: 0x04000254 RID: 596
	public GameObject FactoryCardPresenter;

	// Token: 0x04000255 RID: 597
	public GameObject FactoryCardSelectionContainer;

	// Token: 0x04000256 RID: 598
	public GameObject PayAnyResourcePanel;

	// Token: 0x04000257 RID: 599
	public GameObject GainFoodButton;

	// Token: 0x04000258 RID: 600
	public GameObject GainMetalButton;

	// Token: 0x04000259 RID: 601
	public GameObject GainOilButton;

	// Token: 0x0400025A RID: 602
	public GameObject GainWoodButton;

	// Token: 0x0400025B RID: 603
	public GameObject PeekCombatCardPresenter;

	// Token: 0x0400025C RID: 604
	public GameObject PeekCardOKButton;

	// Token: 0x0400025D RID: 605
	public GameObject BottomBarCombatCardPresenter;

	// Token: 0x0400025E RID: 606
	public GameObject factionOrder;

	// Token: 0x0400025F RID: 607
	public GameObject FactionPreview1;

	// Token: 0x04000260 RID: 608
	public GameObject FactionPreview2;

	// Token: 0x04000261 RID: 609
	public GameObject FactionPreview3;

	// Token: 0x04000262 RID: 610
	public GameObject FactionPreview4;

	// Token: 0x04000263 RID: 611
	public GameObject FactionPreview5;

	// Token: 0x04000264 RID: 612
	public GameObject FactionPreview6;

	// Token: 0x04000265 RID: 613
	public GameObject FactionPreview7;

	// Token: 0x04000266 RID: 614
	[Header("Transporting workers and resources")]
	public GameObject exchangePanel;

	// Token: 0x04000267 RID: 615
	public ExchangePanelPresenter exPanelPres;

	// Token: 0x04000268 RID: 616
	public GameObject allToUnitOil;

	// Token: 0x04000269 RID: 617
	public GameObject allToUnitMetal;

	// Token: 0x0400026A RID: 618
	public GameObject allToUnitWood;

	// Token: 0x0400026B RID: 619
	public GameObject allToUnitFood;

	// Token: 0x0400026C RID: 620
	public GameObject oneToFieldOil;

	// Token: 0x0400026D RID: 621
	public GameObject oneToFieldMetal;

	// Token: 0x0400026E RID: 622
	public GameObject oneToFieldWood;

	// Token: 0x0400026F RID: 623
	public GameObject oneToFieldFood;

	// Token: 0x04000270 RID: 624
	public GameObject loadAllResources;

	// Token: 0x04000271 RID: 625
	public GameObject unloadAllResources;

	// Token: 0x04000272 RID: 626
	public bool isLoadedAllToMech;

	// Token: 0x04000273 RID: 627
	public bool isUnloadOneInactiveWorker;

	// Token: 0x04000274 RID: 628
	public bool isLoadAllActiveWorker;

	// Token: 0x04000275 RID: 629
	public bool isUnloadOneActiveWorker;

	// Token: 0x04000276 RID: 630
	[Header("Combat")]
	public bool isCombatFinished;

	// Token: 0x04000277 RID: 631
	public GameObject combatCardPresenter;

	// Token: 0x04000278 RID: 632
	public GameObject combatPanel;

	// Token: 0x04000279 RID: 633
	public GameObject leftPowerSlider;

	// Token: 0x0400027A RID: 634
	public GameObject fightButton;

	// Token: 0x0400027B RID: 635
	public GameObject centerMinimizeButton;

	// Token: 0x0400027C RID: 636
	public GameObject maximizeButton;

	// Token: 0x0400027D RID: 637
	public List<GameObject> bombSpaces;

	// Token: 0x0400027E RID: 638
	public int bombsAllocated;

	// Token: 0x0400027F RID: 639
	public int availableBombSlots;

	// Token: 0x04000280 RID: 640
	[Header("Encounter")]
	public GameObject encounterCard;

	// Token: 0x04000281 RID: 641
	public GameObject dismissEncounterButton;

	// Token: 0x04000282 RID: 642
	public GameObject clickToUncoverButton;

	// Token: 0x04000283 RID: 643
	public GameObject simpleAmountPresenter;

	// Token: 0x04000284 RID: 644
	public GameObject takeButton;

	// Token: 0x04000285 RID: 645
	public GameObject dismissButton;

	// Token: 0x04000286 RID: 646
	public GameObject dismissYesButton;

	// Token: 0x04000287 RID: 647
	public GameObject siamiaaNoButton;

	// Token: 0x04000288 RID: 648
	[Header("Chat shortcuts")]
	public GameObject chat;

	// Token: 0x04000289 RID: 649
	public bool isSendingChatMsg;

	// Token: 0x0400028A RID: 650
	public bool isCloseChatWindow;

	// Token: 0x0400028B RID: 651
	public GameObject chatButton;

	// Token: 0x0400028C RID: 652
	public GameObject messageToSend;

	// Token: 0x0400028D RID: 653
	private bool shortcutsLocked = true;

	// Token: 0x0400028E RID: 654
	private const int MOUSEEVENTF_LEFTDOWN = 2;

	// Token: 0x0400028F RID: 655
	private const int MOUSEEVENTF_LEFTUP = 4;

	// Token: 0x04000290 RID: 656
	private const int MOUSEEVENTF_RIGHTDOWN = 8;

	// Token: 0x04000291 RID: 657
	private const int MOUSEEVENTF_RIGHTUP = 16;
}
