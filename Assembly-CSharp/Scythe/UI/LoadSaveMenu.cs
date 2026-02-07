using System;
using System.Globalization;
using Common.GameSaves;
using I2.Loc;
using Scythe.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003C6 RID: 966
	public class LoadSaveMenu : MonoBehaviour
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x0003A6C6 File Offset: 0x000388C6
		private string DefaultSaveName
		{
			get
			{
				return "[ " + ScriptLocalization.Get("Common/EmptySlot") + " ]";
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000B0AA8 File Offset: 0x000AECA8
		private void OnEnable()
		{
			this.InstantiateSlots();
			this.SetTitle();
			this.SetSlotsStartingInteractivity();
			this.SaveButton.SetActive(!this.LoadGameMode);
			this.SaveButton.GetComponent<Button>().interactable = false;
			this.LoadButton.SetActive(this.LoadGameMode);
			this.LoadButton.GetComponent<Button>().interactable = false;
			this.DeleteButton.GetComponent<Button>().interactable = false;
			this.ReadSavedSlots();
			GameObject[] array = this.saveSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponentInChildren<InputField>().DeactivateInputField();
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000B0B48 File Offset: 0x000AED48
		private void OnDisable()
		{
			this.DeactivateSaveLoadWindow();
			GameObject[] array = this.saveSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponentInChildren<InputField>().DeactivateInputField();
			}
			for (int j = this.saveSlots.Length - 1; j >= 0; j--)
			{
				global::UnityEngine.Object.Destroy(this.saveSlots[j]);
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000B0BA0 File Offset: 0x000AEDA0
		public void ActivateSaveLoadWindow(bool loadingGameMode)
		{
			Screens screens = (loadingGameMode ? Screens.load_game : Screens.save_game);
			Contexts contexts = ((SceneManager.GetActiveScene().buildIndex == 1) ? Contexts.ingame : Contexts.outgame);
			AnalyticsEventLogger.Instance.LogScreenDisplay(screens, contexts);
			this.LoadGameMode = loadingGameMode;
			if (this.WindowContainer != null)
			{
				this.WindowContainer.SetActive(true);
			}
			base.gameObject.SetActive(true);
			GameObject[] array = this.saveSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponentInChildren<InputField>().readOnly = loadingGameMode;
			}
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0003A6E1 File Offset: 0x000388E1
		public void OnXButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			this.DeactivateSaveLoadWindow();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0003A6FB File Offset: 0x000388FB
		public void OnBackButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			this.DeactivateSaveLoadWindow();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000B0C30 File Offset: 0x000AEE30
		public void DeactivateSaveLoadWindow()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			if (this.selectedSlot != -1)
			{
				this.saveSlots[this.selectedSlot].transform.GetChild(0).GetComponent<Toggle>().isOn = false;
				this.selectedSlot = -1;
			}
			if (this.MainMenuScript != null)
			{
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.play_local_window, Contexts.outgame);
				this.MainMenuScript.OnLoadSaveClosed();
			}
			if (this.WindowContainer != null)
			{
				this.WindowContainer.SetActive(false);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			EventSystem.current.SetSelectedGameObject(null);
			this.ReadSavedSlots();
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000B0CDC File Offset: 0x000AEEDC
		private void InstantiateSlots()
		{
			this.saveSlots = new GameObject[this.NumberOfSlots];
			this.isSlotOccupied = new bool[this.NumberOfSlots];
			ToggleGroup component = this.Slots.GetComponent<ToggleGroup>();
			for (int i = 0; i < this.NumberOfSlots; i++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.SaveSlotPrefab, this.Slots.transform);
				gameObject.transform.localScale = Vector3.one;
				this.saveSlots[i] = gameObject;
				this.isSlotOccupied[i] = false;
				this.SetSlotNamePlaceholderText(i, this.DefaultSaveName);
				this.ChangeSlotDate(i, "--/--/-- --:--:--");
				this.saveSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
				this.saveSlots[i].transform.GetChild(0).GetComponent<Toggle>().group = component;
				int slotIndex = i;
				this.saveSlots[i].transform.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool on)
				{
					this.OnSlotClicked(slotIndex);
				});
				this.saveSlots[i].transform.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool value)
				{
					this.OnSlotToggleClicked(slotIndex, value);
				});
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.saveSlots[i].transform.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000B0E44 File Offset: 0x000AF044
		private void SetSlotsStartingInteractivity()
		{
			for (int i = 0; i < this.NumberOfSlots; i++)
			{
				if (this.GetSlotDate(i) == "--/--/-- --:--:--")
				{
					this.SetSlotNamePlaceholderText(i, this.DefaultSaveName);
				}
				if (!this.LoadGameMode && i == this.NumberOfSlots - 1)
				{
					this.ChangeSlotInteractivity(i, false);
				}
				else
				{
					this.ChangeSlotInteractivity(i, !this.LoadGameMode);
					this.saveSlots[i].transform.GetChild(0).GetComponent<Toggle>().isOn = false;
				}
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0003A709 File Offset: 0x00038909
		private void SetTitle()
		{
			this.WindowTitleLoad.gameObject.SetActive(this.LoadGameMode);
			this.WindowTitleSave.gameObject.SetActive(!this.LoadGameMode);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0003A73A File Offset: 0x0003893A
		private void ReadSavedSlots()
		{
			this.UpdateSlotsData();
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000B0ED0 File Offset: 0x000AF0D0
		private void UpdateSlotsData()
		{
			foreach (SaveMetadata saveMetadata in GameSavesManager.GetSaves())
			{
				int num = saveMetadata.SaveSlotId;
				bool flag = !this.LoadGameMode || GameSavesManager.IsSaveSlotAccessible(saveMetadata.SaveSlotId);
				if (num >= 0 && num < this.NumberOfSlots - 1 && this.saveSlots[num] != null)
				{
					this.ChangeSlotName(num, saveMetadata.SaveName);
					this.ChangeSlotDate(num, saveMetadata.LastWriteDate);
					this.ChangeSlotInteractivity(num, flag);
					this.isSlotOccupied[num] = true;
				}
				else if (saveMetadata.SaveSlotId == GameSavesManager.GetAutomaticSaveSlotId())
				{
					num = GameSavesManager.GetAutomaticSaveSlotId();
					this.ChangeSlotName(num, ScriptLocalization.Get("GameScene/AutomaticSave"));
					this.ChangeSlotDate(num, saveMetadata.LastWriteDate);
					this.ChangeSlotInteractivity(num, this.LoadGameMode);
					this.isSlotOccupied[num] = true;
				}
				this.ChangeSlotUnavalivableReason(num, flag);
			}
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000B0FDC File Offset: 0x000AF1DC
		private void ChangeSlotName(int slotId, string name)
		{
			this.saveSlots[slotId].transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
			this.saveSlots[slotId].transform.GetChild(0).GetChild(3).GetComponent<InputField>()
				.text = name;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000B1034 File Offset: 0x000AF234
		private void SetSlotNamePlaceholderText(int slotId, string placeholderName)
		{
			this.saveSlots[slotId].transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
			this.saveSlots[slotId].transform.GetChild(0).GetChild(3).GetComponent<InputField>()
				.placeholder.GetComponent<Text>().text = placeholderName;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0003A742 File Offset: 0x00038942
		private void ChangeSlotDate(int slotId, DateTime date)
		{
			this.ChangeSlotDate(slotId, date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x0003A75C File Offset: 0x0003895C
		private void ChangeSlotDate(int slotId, string date)
		{
			this.saveSlots[slotId].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>()
				.text = date;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x0003A782 File Offset: 0x00038982
		private void ChangeSlotInteractivity(int slotId, bool interactable)
		{
			this.saveSlots[slotId].transform.GetChild(0).GetComponent<Toggle>().interactable = interactable;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000B1094 File Offset: 0x000AF294
		private void ChangeSlotUnavalivableReason(int slotId, bool accessable)
		{
			if (slotId < 0 || slotId > this.saveSlots.Length - (this.LoadGameMode ? 1 : 2))
			{
				return;
			}
			EventTrigger component = this.saveSlots[slotId].transform.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = component.triggers.Find((EventTrigger.Entry x) => x.eventID == EventTriggerType.PointerEnter);
			EventTrigger.Entry entry2 = component.triggers.Find((EventTrigger.Entry x) => x.eventID == EventTriggerType.PointerExit);
			if (!accessable)
			{
				int id = slotId;
				entry.callback.RemoveAllListeners();
				entry.callback.AddListener(delegate(BaseEventData eventData)
				{
					this.ActivateDLCInfo(id, true);
				});
				entry2.callback.RemoveAllListeners();
				entry2.callback.AddListener(delegate(BaseEventData eventData)
				{
					this.ActivateDLCInfo(id, false);
				});
				return;
			}
			entry.callback.RemoveAllListeners();
			entry2.callback.RemoveAllListeners();
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x0003A7A2 File Offset: 0x000389A2
		private void ActivateDLCInfo(int slotId, bool activate)
		{
			this.saveSlots[slotId].transform.GetChild(0).GetChild(4).gameObject.SetActive(activate);
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0003A7C8 File Offset: 0x000389C8
		private string GetSlotName(int slotId)
		{
			return this.saveSlots[slotId].GetComponentInChildren<InputField>().text;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0003A7DC File Offset: 0x000389DC
		private string GetSlotDate(int slotId)
		{
			return this.saveSlots[slotId].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>()
				.text;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000B1198 File Offset: 0x000AF398
		private void EnableSlotInput(int slotId)
		{
			this.selectedSlot = slotId;
			this.saveSlots[slotId].transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
			InputField component = this.saveSlots[slotId].transform.GetChild(0).GetChild(3).GetComponent<InputField>();
			this.selectedSlotLastName = component.text;
			component.interactable = true;
			component.ActivateInputField();
			component.Select();
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000B1210 File Offset: 0x000AF410
		private void DisableSlotInput(int slotId, int disableOption = 0)
		{
			if (slotId == -1)
			{
				return;
			}
			InputField component = this.saveSlots[slotId].transform.GetChild(0).GetChild(3).GetComponent<InputField>();
			if (disableOption > 0)
			{
				component.text = this.selectedSlotLastName;
			}
			component.DeactivateInputField();
			if (disableOption != 2)
			{
				component.interactable = false;
			}
			if (!this.LoadGameMode)
			{
				this.SaveButton.GetComponent<Button>().interactable = false;
			}
			if (this.LoadGameMode)
			{
				this.LoadButton.GetComponent<Button>().interactable = false;
			}
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000B1294 File Offset: 0x000AF494
		private void DisableSlotInput(InputField inputField)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.DisableSlotInput(this.selectedSlot, 1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.SaveGame(this.selectedSlot);
				this.DisableSlotInput(this.selectedSlot, 0);
				return;
			}
			if (this.MouseNotOverSaveButton())
			{
				this.DisableSlotInput(this.selectedSlot, 2);
			}
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000B12FC File Offset: 0x000AF4FC
		private bool MouseNotOverSaveButton()
		{
			bool flag = !RectTransformUtility.RectangleContainsScreenPoint(this.SaveButton.GetComponent<RectTransform>(), Input.mousePosition);
			return !this.SaveButton.activeInHierarchy || flag;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000B1338 File Offset: 0x000AF538
		public void OnSlotClicked(int slotId)
		{
			bool isOn = this.saveSlots[slotId].transform.GetChild(0).GetComponent<Toggle>().isOn;
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			string slotDate = this.GetSlotDate(slotId);
			this.DeleteButton.GetComponent<Button>().interactable = isOn & (slotDate != "--/--/-- --:--:--");
			if (this.LoadGameMode)
			{
				this.selectedSlot = slotId;
				this.LoadButton.GetComponent<Button>().interactable = isOn;
				return;
			}
			this.SaveButton.GetComponent<Button>().interactable = isOn;
			if (isOn)
			{
				this.selectedSlot = slotId;
				this.EnableSlotInput(slotId);
				return;
			}
			this.DisableSlotInput(slotId, 0);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x0003A801 File Offset: 0x00038A01
		public void OnSlotToggleClicked(int slotId, bool dynamicBool)
		{
			if (!dynamicBool)
			{
				if (this.isSlotOccupied[slotId])
				{
					this.ReadSavedSlots();
					return;
				}
				this.saveSlots[slotId].transform.GetChild(0).GetChild(3).GetComponent<InputField>()
					.text = "";
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000B13E0 File Offset: 0x000AF5E0
		public void OnSaveButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_save_game_button);
			int num = this.selectedSlot;
			if (this.selectedSlot != -1)
			{
				this.SaveGame(this.selectedSlot);
			}
			this.SaveButton.GetComponent<Button>().interactable = false;
			if (num != -1)
			{
				this.DisableSlotInput(num, 0);
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x0003A83F File Offset: 0x00038A3F
		public void OnLoadButtonClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_load_game_button);
			this.LoadGame(this.selectedSlot, false);
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000B1430 File Offset: 0x000AF630
		public void OnDeleteButtonClicked()
		{
			this.saveSlots[this.selectedSlot].transform.GetChild(0).GetComponent<Toggle>().isOn = false;
			this.saveSlots[this.selectedSlot].transform.GetChild(0).GetComponent<Toggle>().interactable = false;
			this.DeleteButton.GetComponent<Button>().interactable = false;
			GameSavesManager.DeleteGame(GameSavesManager.GetSaveSlotName(this.selectedSlot));
			this.SetSlotNamePlaceholderText(this.selectedSlot, this.DefaultSaveName);
			this.ChangeSlotName(this.selectedSlot, string.Empty);
			this.ChangeSlotDate(this.selectedSlot, "--/--/-- --:--:--");
			this.SetSlotsStartingInteractivity();
			this.ReadSavedSlots();
			if (this.LoadGameMode)
			{
				this.LoadButton.GetComponent<Button>().interactable = false;
				this.isSlotOccupied[this.selectedSlot] = false;
				this.selectedSlot = -1;
			}
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000B1514 File Offset: 0x000AF714
		public void LoadGame(int slotId, bool crashPopupCall = false)
		{
			try
			{
				string saveSlotName = GameSavesManager.GetSaveSlotName(slotId);
				if (this.MainMenuScript != null)
				{
					if (PlatformManager.IsStandalone)
					{
						LoadingScreenPresenter.LoadingSave = true;
					}
					else
					{
						LoadingScreenPresenterMobile.LoadingSave = true;
					}
					this.MainMenuScript.LoadGame(saveSlotName);
				}
				else
				{
					if (PlatformManager.IsStandalone)
					{
						LoadingScreenPresenter.LoadingSave = true;
					}
					else
					{
						LoadingScreenPresenterMobile.LoadingSave = true;
					}
					SceneController.SetLoadGameFile(saveSlotName);
					GameController.Instance.GameIsLoaded = true;
					GameController.Instance.ExitGame();
					this.LoadGame(saveSlotName);
				}
			}
			catch (Exception ex)
			{
				this.ShowInfoMessage(ScriptLocalization.Get("GameScene/GameLoadFailed"));
				Debug.LogError(ex);
			}
			if (!crashPopupCall)
			{
				this.DeactivateSaveLoadWindow();
			}
			this.CloseAdditionalWindow();
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0003A855 File Offset: 0x00038A55
		public void LoadGame(string fileName)
		{
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.load);
			GameController.gameFromSave = true;
			GameController.GameManager.IsMultiplayer = false;
			GameController.GameManager.missionId = -1;
			GameSavesManager.LoadGame(fileName);
			GameController.GameManager.IsHotSeat = true;
			this.StartGame();
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x0003A890 File Offset: 0x00038A90
		public void StartGame()
		{
			SceneController.Instance.LoadScene(SceneController.SCENE_MAIN_NAME);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000B15C8 File Offset: 0x000AF7C8
		private void SaveGame(int slotId)
		{
			try
			{
				string text = this.GetSlotName(slotId);
				if (string.IsNullOrEmpty(text) || text == this.DefaultSaveName)
				{
					text = ScriptLocalization.Get("Common/Game") + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
				}
				GameSavesManager.SaveGame(slotId, text, GameController.Game.GetGameId());
				this.saveSlots[slotId].transform.GetComponentInChildren<InputField>().text = text;
				this.isSlotOccupied[slotId] = true;
				this.ShowInfoMessage(ScriptLocalization.Get("GameScene/GameSaved") + ": " + text);
			}
			catch (Exception ex)
			{
				this.ShowInfoMessage(ScriptLocalization.Get("GameScene/GameSaveFailed"));
				Debug.LogError(ex);
			}
			this.DeactivateSaveLoadWindow();
			this.CloseAdditionalWindow();
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x0003A8A1 File Offset: 0x00038AA1
		private void CloseAdditionalWindow()
		{
			if (this.WindowToBeClosedAfterLoadSaveAction != null)
			{
				this.WindowToBeClosedAfterLoadSaveAction.SetActive(false);
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000B16A4 File Offset: 0x000AF8A4
		private void ShowInfoMessage(string message)
		{
			if (this.ActionInfo == null)
			{
				return;
			}
			this.ActionInfo.GetComponent<Text>().text = message;
			this.ResetAlphaOfTheMessage();
			this.ActionInfo.SetActive(false);
			this.ActionInfo.SetActive(true);
			this.ActionInfo.GetComponent<Text>().CrossFadeAlpha(0f, 2.5f, false);
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000B170C File Offset: 0x000AF90C
		private void ResetAlphaOfTheMessage()
		{
			Color color = this.ActionInfo.GetComponent<Text>().color;
			color.a = 1f;
			this.ActionInfo.GetComponent<Text>().color = color;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x0003A8BD File Offset: 0x00038ABD
		public bool AutomaticSaveFileCanBeLoaded()
		{
			return GameSavesManager.SaveSlotExists("ScytheSaveTmp.xml") && GameSavesManager.IsSaveSlotAccessible(GameSavesManager.GetAutomaticSaveSlotId());
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000B1748 File Offset: 0x000AF948
		public void OnResumeGameClicked()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_resume_button);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			string text = "ScytheSaveTmp.xml";
			try
			{
				if (this.MainMenuScript != null)
				{
					if (PlatformManager.IsStandalone)
					{
						LoadingScreenPresenter.LoadingSave = true;
					}
					else
					{
						LoadingScreenPresenterMobile.LoadingSave = true;
					}
					this.MainMenuScript.LoadGame(text);
				}
				else
				{
					GameController.Instance.LoadGame(text);
					this.ShowInfoMessage(ScriptLocalization.Get("GameScene/GameLoaded") + ": " + ScriptLocalization.Get("GameScene/AutomaticSave"));
				}
			}
			catch (Exception ex)
			{
				this.ShowInfoMessage(ScriptLocalization.Get("GameScene/GameLoadFailed"));
				Debug.LogError(ex);
			}
		}

		// Token: 0x04001450 RID: 5200
		public bool LoadGameMode;

		// Token: 0x04001451 RID: 5201
		public TextMeshProUGUI WindowTitleLoad;

		// Token: 0x04001452 RID: 5202
		public TextMeshProUGUI WindowTitleSave;

		// Token: 0x04001453 RID: 5203
		public GameObject SaveButton;

		// Token: 0x04001454 RID: 5204
		public GameObject LoadButton;

		// Token: 0x04001455 RID: 5205
		public GameObject DeleteButton;

		// Token: 0x04001456 RID: 5206
		public GameObject Slots;

		// Token: 0x04001457 RID: 5207
		public GameObject SaveSlotPrefab;

		// Token: 0x04001458 RID: 5208
		[Range(1f, 8f)]
		public int NumberOfSlots = 1;

		// Token: 0x04001459 RID: 5209
		public GameObject WindowContainer;

		// Token: 0x0400145A RID: 5210
		public GameObject ActionInfo;

		// Token: 0x0400145B RID: 5211
		public GameObject WindowToBeClosedAfterLoadSaveAction;

		// Token: 0x0400145C RID: 5212
		public MainMenu MainMenuScript;

		// Token: 0x0400145D RID: 5213
		private const string DEFAULT_DATE = "--/--/-- --:--:--";

		// Token: 0x0400145E RID: 5214
		public const string SAVE_FOLDER = "Saves";

		// Token: 0x0400145F RID: 5215
		private GameObject[] saveSlots;

		// Token: 0x04001460 RID: 5216
		private int selectedSlot = -1;

		// Token: 0x04001461 RID: 5217
		private string selectedSlotLastName = "";

		// Token: 0x04001462 RID: 5218
		private bool[] isSlotOccupied;
	}
}
