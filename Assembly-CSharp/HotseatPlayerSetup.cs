using System;
using System.Text.RegularExpressions;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F1 RID: 241
public class HotseatPlayerSetup : MonoBehaviour
{
	// Token: 0x0600078C RID: 1932 RVA: 0x0002CEEA File Offset: 0x0002B0EA
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002CEF2 File Offset: 0x0002B0F2
	private void OnEnable()
	{
		this.AddPlayerLocalize();
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0002CEFA File Offset: 0x0002B0FA
	private void Update()
	{
		this.FactionLongClickUpdate();
		this.MatLongClickUpdate();
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00075944 File Offset: 0x00073B44
	public void Initialize(int id, HotseatPanelController hotseatPanel)
	{
		this.Initialize();
		this.SetSlotId(id);
		this.SetHotseatPanel(hotseatPanel);
		this.playerMat = HotseatPanelController.NUMBER_OF_PLAYER_MATS;
		this.factionMat = HotseatPanelController.NUMBER_OF_FACTIONS;
		if (id == 0)
		{
			this.playerType = 0;
		}
		else
		{
			this.playerType = 2;
		}
		this.playerName.onEndEdit.RemoveAllListeners();
		this.playerName.onEndEdit.AddListener(delegate
		{
			this.CheckPlayerName();
		});
		if (PlatformManager.IsMobile)
		{
			EventTrigger eventTrigger = (EventTrigger)this.playerName.gameObject.AddComponent(typeof(EventTrigger));
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener(delegate(BaseEventData a)
			{
				this.OnPlayerNameInputClicked();
			});
			eventTrigger.triggers.Add(entry);
		}
		this.UpdateFaction(this.factionMat);
		this.UpdatePlayerMat(this.playerMat);
		this.UpdatePlayerType(this.playerType);
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00075A34 File Offset: 0x00073C34
	private void Initialize()
	{
		if (PlatformManager.IsStandalone)
		{
			this.InitializeBackground();
			this.InitializeIFAInfo();
			this.InitializePlayerName();
			this.InitializeAddPlayerGroup();
			this.InitializePlayerDetailedGroup();
			this.InitializeListeners();
		}
		if (PlatformManager.IsMobile)
		{
			this.InitializePlayerType();
			this.InitializePlayerFactionMobile();
			this.PlayerMatTypeChangeArrows();
			this.InitializeAddGroupButtons();
			this.InitializePlayerDetailesGroupButtonMobile();
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002CF08 File Offset: 0x0002B108
	private void InitializeBackground()
	{
		if (this.background != null)
		{
			return;
		}
		this.background = base.transform.GetChild(0).GetComponent<Image>();
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002CF30 File Offset: 0x0002B130
	private void InitializeIFAInfo()
	{
		if (this.IFAExpansionInfo != null)
		{
			return;
		}
		this.IFAExpansionInfo = base.transform.GetChild(1).gameObject;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00075A94 File Offset: 0x00073C94
	private void InitializePlayerName()
	{
		if (this.playerName != null)
		{
			return;
		}
		this.playerName = base.transform.GetChild(2).GetChild(1).GetComponent<InputField>();
		this.playerNameBackground = base.transform.GetChild(2).GetChild(0).GetComponent<Image>();
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00075AEC File Offset: 0x00073CEC
	private void InitializeAddPlayerGroup()
	{
		if (this.addPlayerGroup == null)
		{
			this.addPlayerGroup = base.transform.GetChild(3);
		}
		if (this.addHumanPlayer == null)
		{
			this.addHumanPlayer = this.addPlayerGroup.GetChild(0).GetComponent<Button>();
		}
		if (this.addBotEasy == null)
		{
			this.addBotEasy = this.addPlayerGroup.GetChild(1).GetComponent<Button>();
		}
		if (this.addBotMedium == null)
		{
			this.addBotMedium = this.addPlayerGroup.GetChild(2).GetComponent<Button>();
		}
		if (this.addBotHard == null)
		{
			this.addBotHard = this.addPlayerGroup.GetChild(3).GetComponent<Button>();
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002CF58 File Offset: 0x0002B158
	private void InitializePlayerDetailedGroup()
	{
		if (this.playerDetailedGroup == null)
		{
			this.playerDetailedGroup = base.transform.GetChild(4);
		}
		this.InitializePlayerRemove();
		this.InitializePlayerType();
		this.InitializePlayerFaction();
		this.InitializePlayerMat();
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0002CF92 File Offset: 0x0002B192
	private void InitializePlayerRemove()
	{
		if (this.removePlayer == null)
		{
			this.removePlayer = this.playerDetailedGroup.GetChild(0).GetComponent<Button>();
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00075BB0 File Offset: 0x00073DB0
	private void InitializePlayerType()
	{
		if (this.previousPlayerType == null)
		{
			this.previousPlayerType = this.playerDetailedGroup.GetChild(1).GetChild(0).GetComponent<Button>();
		}
		if (this.nextPlayerType == null)
		{
			this.nextPlayerType = this.playerDetailedGroup.GetChild(1).GetChild(1).GetComponent<Button>();
		}
		if (this.playerTypeImage == null)
		{
			this.playerTypeImage = this.playerDetailedGroup.GetChild(1).GetChild(2).GetComponent<Image>();
		}
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00075C40 File Offset: 0x00073E40
	private void InitializePlayerFaction()
	{
		if (this.previousFactionMat == null)
		{
			this.previousFactionMat = this.playerDetailedGroup.GetChild(2).GetChild(0).GetComponent<Button>();
		}
		if (this.nextFactionMat == null)
		{
			this.nextFactionMat = this.playerDetailedGroup.GetChild(2).GetChild(1).GetComponent<Button>();
		}
		if (this.factionMatName == null)
		{
			this.factionMatName = this.playerDetailedGroup.GetChild(2).GetChild(2).GetChild(0)
				.GetComponent<TextMeshProUGUI>();
		}
		if (this.factionImage == null)
		{
			this.factionImage = this.playerDetailedGroup.GetChild(2).GetChild(2).GetChild(1)
				.GetComponent<Image>();
		}
		if (this.factionMatDetailsButton == null)
		{
			this.factionMatDetailsButton = this.playerDetailedGroup.GetChild(2).GetChild(2).GetChild(1)
				.GetComponent<Button>();
		}
		if (this.factionMatDetailsButton2 == null)
		{
			this.factionMatDetailsButton2 = this.playerDetailedGroup.GetChild(2).GetChild(2).GetChild(1)
				.GetChild(1)
				.GetComponent<Button>();
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00075D70 File Offset: 0x00073F70
	private void InitializePlayerFactionMobile()
	{
		if (this.previousFactionMat == null)
		{
			this.previousFactionMat = this.playerDetailedGroup.GetChild(1).GetChild(0).GetComponent<Button>();
		}
		if (this.nextFactionMat == null)
		{
			this.nextFactionMat = this.playerDetailedGroup.GetChild(1).GetChild(1).GetComponent<Button>();
		}
		if (this.previousPlayerMat == null)
		{
			this.previousPlayerMat = base.transform.GetChild(4).GetChild(1).GetChild(0)
				.GetComponent<Button>();
		}
		if (this.nextPlayerMat == null)
		{
			this.nextPlayerMat = base.transform.GetChild(4).GetChild(1).GetChild(1)
				.GetComponent<Button>();
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00075E38 File Offset: 0x00074038
	private void InitializePlayerMat()
	{
		if (this.previousPlayerMat == null)
		{
			this.previousPlayerMat = base.transform.GetChild(5).GetChild(0).GetComponent<Button>();
		}
		if (this.nextPlayerMat == null)
		{
			this.nextPlayerMat = base.transform.GetChild(5).GetChild(1).GetComponent<Button>();
		}
		if (this.playerMatName == null)
		{
			this.playerMatName = base.transform.GetChild(5).GetChild(2).GetChild(0)
				.GetComponent<TextMeshProUGUI>();
		}
		if (this.playerMatDetailsButton == null)
		{
			this.playerMatDetailsButton = base.transform.GetChild(5).GetChild(2).GetComponent<Button>();
		}
		if (this.playerMatDetailsButton2 == null)
		{
			this.playerMatDetailsButton2 = base.transform.GetChild(5).GetChild(2).GetChild(1)
				.GetComponent<Button>();
		}
		if (this.playerMatDetails == null)
		{
			this.playerMatDetails = base.transform.GetChild(5).GetChild(2).gameObject;
		}
		if (this.abilityChangeInfo == null)
		{
			this.abilityChangeInfo = this.playerDetailedGroup.GetChild(3).gameObject;
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002CFB9 File Offset: 0x0002B1B9
	private void InitializeListeners()
	{
		this.InitializeAddGroupButtons();
		this.InitializePlayerDetailesGroupButton();
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00075F78 File Offset: 0x00074178
	private void InitializeAddGroupButtons()
	{
		if (this.addHumanPlayer.onClick.GetPersistentEventCount() == 0)
		{
			this.addHumanPlayer.onClick.RemoveAllListeners();
			this.addHumanPlayer.onClick.AddListener(delegate
			{
				this.OnPlayerAdded(0);
			});
		}
		if (this.addBotEasy.onClick.GetPersistentEventCount() == 0)
		{
			this.addBotEasy.onClick.RemoveAllListeners();
			this.addBotEasy.onClick.AddListener(delegate
			{
				this.OnPlayerAdded(1);
			});
		}
		if (PlatformManager.IsStandalone)
		{
			if (this.addBotMedium.onClick.GetPersistentEventCount() == 0)
			{
				this.addBotMedium.onClick.RemoveAllListeners();
				this.addBotMedium.onClick.AddListener(delegate
				{
					this.OnPlayerAdded(2);
				});
			}
			if (this.addBotHard.onClick.GetPersistentEventCount() == 0)
			{
				this.addBotHard.onClick.RemoveAllListeners();
				this.addBotHard.onClick.AddListener(delegate
				{
					this.OnPlayerAdded(3);
				});
			}
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00076084 File Offset: 0x00074284
	private void InitializePlayerDetailesGroupButton()
	{
		if (this.removePlayer.onClick.GetPersistentEventCount() == 0)
		{
			this.removePlayer.onClick.RemoveAllListeners();
			this.removePlayer.onClick.AddListener(new UnityAction(this.OnPlayerRemoved));
		}
		if (this.previousPlayerType.onClick.GetPersistentEventCount() == 0)
		{
			this.previousPlayerType.onClick.RemoveAllListeners();
			this.previousPlayerType.onClick.AddListener(new UnityAction(this.OnPreviousPlayerTypeClicked));
		}
		if (this.nextPlayerType.onClick.GetPersistentEventCount() == 0)
		{
			this.nextPlayerType.onClick.RemoveAllListeners();
			this.nextPlayerType.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
		}
		if (this.previousFactionMat.onClick.GetPersistentEventCount() == 0)
		{
			this.previousFactionMat.onClick.RemoveAllListeners();
			this.previousFactionMat.onClick.AddListener(new UnityAction(this.OnPreviousFactionButtonClicked));
		}
		if (this.nextFactionMat.onClick.GetPersistentEventCount() == 0)
		{
			this.nextFactionMat.onClick.RemoveAllListeners();
			this.nextFactionMat.onClick.AddListener(new UnityAction(this.OnNextFactionButtonClicked));
		}
		if (PlatformManager.IsStandalone)
		{
			if (this.factionMatDetailsButton2.onClick.GetPersistentEventCount() == 0)
			{
				this.factionMatDetailsButton2.onClick.RemoveAllListeners();
				this.factionMatDetailsButton2.onClick.AddListener(new UnityAction(this.OnFactionDetailsClicked));
			}
			if (this.factionMatDetailsButton.onClick.GetPersistentEventCount() == 0)
			{
				this.factionMatDetailsButton.onClick.RemoveAllListeners();
				this.factionMatDetailsButton.onClick.AddListener(new UnityAction(this.OnFactionDetailsClicked));
			}
		}
		if (PlatformManager.IsStandalone)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.ShowFactionMatDetailsButton(true);
			});
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.ShowFactionMatDetailsButton(false);
			});
			this.factionImage.gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
			this.factionImage.gameObject.GetComponent<EventTrigger>().triggers.Add(entry2);
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			entry3.eventID = EventTriggerType.PointerEnter;
			entry3.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.PointerOnFactionDetailsButton(true);
			});
			EventTrigger.Entry entry4 = new EventTrigger.Entry();
			entry4.eventID = EventTriggerType.PointerExit;
			entry4.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.PointerOnFactionDetailsButton(false);
			});
			this.factionMatDetailsButton2.gameObject.AddComponent<EventTrigger>().triggers.Add(entry3);
			this.factionMatDetailsButton2.gameObject.GetComponent<EventTrigger>().triggers.Add(entry4);
		}
		if (this.previousPlayerMat.onClick.GetPersistentEventCount() == 0)
		{
			this.previousPlayerMat.onClick.RemoveAllListeners();
			this.previousPlayerMat.onClick.AddListener(new UnityAction(this.OnPreviousPlayerMatButtonClicked));
		}
		if (this.nextPlayerMat.onClick.GetPersistentEventCount() == 0)
		{
			this.nextPlayerMat.onClick.RemoveAllListeners();
			this.nextPlayerMat.onClick.AddListener(new UnityAction(this.OnNextPlayerMatButtonClicked));
		}
		if (PlatformManager.IsStandalone)
		{
			if (this.playerMatDetailsButton2.onClick.GetPersistentEventCount() == 0)
			{
				this.playerMatDetailsButton2.onClick.RemoveAllListeners();
				this.playerMatDetailsButton2.onClick.AddListener(new UnityAction(this.OnPlayerMatDetailsClicked));
			}
			if (this.playerMatDetailsButton.onClick.GetPersistentEventCount() == 0)
			{
				this.playerMatDetailsButton.onClick.RemoveAllListeners();
				this.playerMatDetailsButton.onClick.AddListener(new UnityAction(this.OnPlayerMatDetailsClicked));
			}
			this.playerMatDetails.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry5 = new EventTrigger.Entry();
			entry5.eventID = EventTriggerType.PointerEnter;
			entry5.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.ShowPlayerMatDetailsButton(true);
			});
			EventTrigger.Entry entry6 = new EventTrigger.Entry();
			entry6.eventID = EventTriggerType.PointerExit;
			entry6.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.ShowPlayerMatDetailsButton(false);
			});
			this.playerMatDetails.gameObject.AddComponent<EventTrigger>().triggers.Add(entry5);
			this.playerMatDetails.gameObject.GetComponent<EventTrigger>().triggers.Add(entry6);
			this.playerMatDetailsButton2.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry7 = new EventTrigger.Entry();
			entry7.eventID = EventTriggerType.PointerEnter;
			entry7.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.PointerOnPlayerMatDetailsButton(true);
			});
			EventTrigger.Entry entry8 = new EventTrigger.Entry();
			entry8.eventID = EventTriggerType.PointerExit;
			entry8.callback.AddListener(delegate(BaseEventData eventData)
			{
				this.PointerOnPlayerMatDetailsButton(false);
			});
			this.playerMatDetailsButton2.gameObject.AddComponent<EventTrigger>().triggers.Add(entry7);
			this.playerMatDetailsButton2.gameObject.GetComponent<EventTrigger>().triggers.Add(entry8);
			return;
		}
		this.playerMatDetailsButton.onClick.RemoveAllListeners();
		this.playerMatDetailsButton.onClick.AddListener(new UnityAction(this.OnNextPlayerMatButtonClicked));
		this.factionMatDetailsButton.onClick.RemoveAllListeners();
		this.factionMatDetailsButton.onClick.AddListener(new UnityAction(this.OnNextFactionButtonClicked));
		this.playerTypeButton.onClick.RemoveAllListeners();
		this.playerTypeButton.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00076608 File Offset: 0x00074808
	private void InitializePlayerDetailesGroupButtonMobile()
	{
		if (this.removePlayer.onClick.GetPersistentEventCount() == 0)
		{
			this.removePlayer.onClick.RemoveAllListeners();
			this.removePlayer.onClick.AddListener(new UnityAction(this.OnPlayerRemoved));
		}
		if (this.nextPlayerType.onClick.GetPersistentEventCount() == 0)
		{
			this.nextPlayerType.onClick.RemoveAllListeners();
			this.nextPlayerType.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
		}
		this.playerMatDetailsButton.onClick.RemoveAllListeners();
		this.playerMatDetailsButton.onClick.AddListener(new UnityAction(this.OnNextPlayerMatButtonClicked));
		this.factionMatDetailsButton.onClick.RemoveAllListeners();
		this.factionMatDetailsButton.onClick.AddListener(new UnityAction(this.OnNextFactionButtonClicked));
		this.playerTypeButton.onClick.RemoveAllListeners();
		this.playerTypeButton.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00076718 File Offset: 0x00074918
	private void PlayerMatTypeChangeArrows()
	{
		if (this.previousPlayerType.onClick.GetPersistentEventCount() == 0)
		{
			this.previousPlayerType.onClick.RemoveAllListeners();
			this.previousPlayerType.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
		}
		if (this.nextPlayerType.onClick.GetPersistentEventCount() == 0)
		{
			this.nextPlayerType.onClick.RemoveAllListeners();
			this.nextPlayerType.onClick.AddListener(new UnityAction(this.OnNextPlayerTypeClicked));
		}
		if (this.previousFactionMat.onClick.GetPersistentEventCount() == 0)
		{
			this.previousFactionMat.onClick.RemoveAllListeners();
			this.previousFactionMat.onClick.AddListener(new UnityAction(this.OnNextFactionButtonClicked));
		}
		if (this.nextFactionMat.onClick.GetPersistentEventCount() == 0)
		{
			this.nextFactionMat.onClick.RemoveAllListeners();
			this.nextFactionMat.onClick.AddListener(new UnityAction(this.OnNextFactionButtonClicked));
		}
		if (this.previousPlayerMat.onClick.GetPersistentEventCount() == 0)
		{
			this.previousPlayerMat.onClick.RemoveAllListeners();
			this.previousPlayerMat.onClick.AddListener(new UnityAction(this.OnNextPlayerMatButtonClicked));
		}
		if (this.nextPlayerMat.onClick.GetPersistentEventCount() == 0)
		{
			this.nextPlayerMat.onClick.RemoveAllListeners();
			this.nextPlayerMat.onClick.AddListener(new UnityAction(this.OnNextPlayerMatButtonClicked));
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0007689C File Offset: 0x00074A9C
	public void LoadHotseatPlayerPrefs(int index)
	{
		if (PlayerPrefs.GetString("HotseatPlayerName" + index.ToString(), string.Empty) == string.Empty)
		{
			return;
		}
		this.LoadPlayerType(index);
		this.LoadPlayerName(index);
		this.LoadPlayerFaction(index);
		this.LoadPlayerMat(index);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000768F0 File Offset: 0x00074AF0
	private void LoadPlayerType(int index)
	{
		int num = PlayerPrefs.GetInt("HotseatPlayerType" + index.ToString(), 2);
		if (index == 0)
		{
			num = 0;
		}
		this.UpdatePlayerType(num);
		this.UpdateEntryState(true);
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00076928 File Offset: 0x00074B28
	private void LoadPlayerName(int index)
	{
		if (!this.SlotEnabled())
		{
			return;
		}
		if (this.playerType != -1)
		{
			if (this.playerType == 0)
			{
				this.SetPlayerName(PlayerPrefs.GetString("HotseatPlayerName" + index.ToString(), this.GenerateHumanPlayerName(index)));
			}
			else
			{
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(index, this.playerType));
			}
		}
		this.CheckPlayerName();
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0007698C File Offset: 0x00074B8C
	private void LoadPlayerFaction(int index)
	{
		int num = PlayerPrefs.GetInt("HotseatPlayerFaction" + index.ToString(), HotseatPanelController.NUMBER_OF_FACTIONS);
		if (num == -1)
		{
			num = HotseatPanelController.NUMBER_OF_FACTIONS;
		}
		if (num != HotseatPanelController.NUMBER_OF_FACTIONS)
		{
			this.hotseatPanel.UpdateFactionMatSelection(num, true);
		}
		this.UpdateFaction(num);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x000769DC File Offset: 0x00074BDC
	private void LoadPlayerMat(int index)
	{
		int num = PlayerPrefs.GetInt("HotseatPlayerMat" + index.ToString(), HotseatPanelController.NUMBER_OF_PLAYER_MATS);
		if (num == -1)
		{
			num = HotseatPanelController.NUMBER_OF_PLAYER_MATS;
		}
		if (num != HotseatPanelController.NUMBER_OF_PLAYER_MATS)
		{
			this.hotseatPanel.UpdatePlayerMatSelection(num, true);
		}
		this.UpdatePlayerMat(num);
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002CFC7 File Offset: 0x0002B1C7
	public void SetSlotId(int slotId)
	{
		this.slotId = slotId;
		this.CheckPlayerName();
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0002920A File Offset: 0x0002740A
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00076A2C File Offset: 0x00074C2C
	public void EnableIFASlot(bool enabled)
	{
		if (enabled)
		{
			this.EnablePlayerDetails(false, false);
			if (this.IFAExpansionInfo)
			{
				this.IFAExpansionInfo.SetActive(false);
				return;
			}
		}
		else
		{
			this.EnablePlayerDetails(false, true);
			if (this.IFAExpansionInfo)
			{
				this.IFAExpansionInfo.SetActive(true);
			}
		}
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002CFD6 File Offset: 0x0002B1D6
	public void UpdateEntryState(bool state)
	{
		if (!state)
		{
			this.OnSlotDisable();
			return;
		}
		this.OnSlotEnable();
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
	public bool SlotEnabled()
	{
		return this.playerDetailedGroup.gameObject.activeSelf;
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002CFFA File Offset: 0x0002B1FA
	private void OnSlotEnable()
	{
		this.EnablePlayerDetails(true, false);
		this.UpdateFaction(this.factionMat);
		this.UpdatePlayerMat(this.playerMat);
		this.CheckPlayerName();
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00076A80 File Offset: 0x00074C80
	private void OnSlotDisable()
	{
		this.ResetSelection();
		this.playerType = 1;
		this.SetPlayerName(string.Empty);
		this.SetPlayerMatTextColor(this.matTextColor);
		this.SetBackgroundSprite(this.hotseatPanel.GetFactionBackgroundSprite(9));
		this.EnablePlayerDetails(false, false);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00076AD4 File Offset: 0x00074CD4
	private void EnablePlayerDetails(bool active, bool doNotEnableAddPlayer = false)
	{
		if (active)
		{
			this.addPlayerGroup.gameObject.SetActive(false);
			this.playerDetailedGroup.gameObject.SetActive(true);
			this.SetTrasnparency(true);
			if (PlatformManager.IsStandalone)
			{
				this.playerMatDetailsButton2.interactable = true;
			}
			else
			{
				this.playerNameBackground.enabled = true;
				this.playerName.transform.parent.gameObject.SetActive(true);
				this.playerMatDetails.SetActive(true);
			}
			this.playerName.interactable = true;
			this.playerName.placeholder.gameObject.SetActive(true);
			this.previousPlayerMat.gameObject.SetActive(true);
			this.nextPlayerMat.gameObject.SetActive(true);
			this.playerMatName.gameObject.SetActive(true);
			if (PlatformManager.IsStandalone)
			{
				this.playerMatDetailsButton2.interactable = true;
			}
			this.playerMatDetailsButton.interactable = true;
			return;
		}
		this.playerDetailedGroup.gameObject.SetActive(false);
		this.previousPlayerMat.gameObject.SetActive(false);
		this.nextPlayerMat.gameObject.SetActive(false);
		this.playerMatName.gameObject.SetActive(false);
		if (PlatformManager.IsStandalone)
		{
			this.playerMatDetailsButton2.interactable = false;
		}
		else
		{
			this.playerMatDetails.SetActive(false);
			this.playerName.transform.parent.gameObject.SetActive(false);
		}
		this.playerMatDetailsButton.interactable = false;
		if (this.hotseatPanel.IsFirstAvaliableSlot(this.slotId) && !doNotEnableAddPlayer)
		{
			this.AddPlayerLocalize();
			this.addPlayerGroup.gameObject.SetActive(true);
			this.SetTrasnparency(true);
			this.playerName.interactable = false;
			this.playerName.placeholder.gameObject.SetActive(false);
			return;
		}
		this.addPlayerGroup.gameObject.SetActive(false);
		if (!PlatformManager.IsStandalone)
		{
			this.playerNameBackground.enabled = false;
		}
		this.SetTrasnparency(false);
		this.playerName.interactable = false;
		this.playerName.placeholder.gameObject.SetActive(false);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00076D00 File Offset: 0x00074F00
	private void SetTrasnparency(bool set)
	{
		if (!PlatformManager.IsStandalone)
		{
			return;
		}
		Color white = Color.white;
		if (!set)
		{
			white.a = 0.6f;
		}
		this.background.color = white;
		this.playerName.transform.parent.GetChild(0).GetComponent<Image>().color = white;
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0002D022 File Offset: 0x0002B222
	public void ResetSelection()
	{
		this.ResetPlayerMat();
		this.ResetFactionMat();
		this.UpdatePlayerMat(this.playerMat);
		this.UpdateFaction(this.factionMat);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0002D048 File Offset: 0x0002B248
	private void ResetPlayerMat()
	{
		this.hotseatPanel.UpdatePlayerMatSelection(this.playerMat, false);
		this.playerMat = HotseatPanelController.NUMBER_OF_PLAYER_MATS;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0002D067 File Offset: 0x0002B267
	private void ResetFactionMat()
	{
		this.hotseatPanel.UpdateFactionMatSelection(this.factionMat, false);
		this.factionMat = HotseatPanelController.NUMBER_OF_FACTIONS;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0002D086 File Offset: 0x0002B286
	public void SetRemoveable(bool removable)
	{
		if (removable)
		{
			this.removePlayer.gameObject.SetActive(true);
			return;
		}
		this.removePlayer.gameObject.SetActive(false);
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00076D58 File Offset: 0x00074F58
	public void UpdateIFAData(bool ifaOn)
	{
		if (!this.SlotEnabled())
		{
			return;
		}
		if (!ifaOn)
		{
			if (this.factionMat == 1 || this.factionMat == 4)
			{
				this.ResetFactionMat();
			}
			if (this.playerMat == 5 || this.playerMat == 6)
			{
				this.ResetPlayerMat();
				this.UpdatePlayerMat(this.playerMat);
			}
		}
		this.UpdateFaction(this.factionMat);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x00076DBC File Offset: 0x00074FBC
	public void UpdateAbilitiesChange(bool moreThan5Players)
	{
		if (PlatformManager.IsMobile)
		{
			if (moreThan5Players)
			{
				this.abilityChangeInfoMobile.Activate(this.factionMat);
				return;
			}
			this.abilityChangeInfoMobile.Deactivate();
			return;
		}
		else
		{
			if (!moreThan5Players)
			{
				if (this.abilityChangeInfo != null)
				{
					this.abilityChangeInfo.transform.GetChild(0).gameObject.SetActive(false);
					this.abilityChangeInfo.transform.GetChild(1).gameObject.SetActive(false);
				}
				return;
			}
			if (this.factionMat == 0)
			{
				this.abilityChangeInfo.gameObject.SetActive(true);
				this.abilityChangeInfo.transform.GetChild(0).gameObject.SetActive(true);
				this.abilityChangeInfo.transform.GetChild(1).gameObject.SetActive(false);
				return;
			}
			if (this.factionMat == 5)
			{
				this.abilityChangeInfo.gameObject.SetActive(true);
				this.abilityChangeInfo.transform.GetChild(1).gameObject.SetActive(true);
				this.abilityChangeInfo.transform.GetChild(0).gameObject.SetActive(false);
				return;
			}
			this.abilityChangeInfo.transform.GetChild(0).gameObject.SetActive(false);
			this.abilityChangeInfo.transform.GetChild(1).gameObject.SetActive(false);
			return;
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0002D0AE File Offset: 0x0002B2AE
	public void SetHotseatPanel(HotseatPanelController hotseatPanel)
	{
		this.hotseatPanel = hotseatPanel;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0002D0B7 File Offset: 0x0002B2B7
	public void SetPlayerName(string name)
	{
		this.playerName.text = name;
		this.SePlayerPreviousName(name);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0002D0CC File Offset: 0x0002B2CC
	private void SePlayerPreviousName(string name)
	{
		this.playerPreviousName = name;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002D0D5 File Offset: 0x0002B2D5
	public void SetBackgroundSprite(Sprite background)
	{
		if (PlatformManager.IsStandalone)
		{
			this.background.sprite = background;
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002D0EA File Offset: 0x0002B2EA
	public string GetPlayerName()
	{
		return this.playerName.text;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002D0F7 File Offset: 0x0002B2F7
	public int GetPlayerType()
	{
		return this.playerType;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002D0FF File Offset: 0x0002B2FF
	public int GetPlayerFaction()
	{
		return this.factionMat;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0002D107 File Offset: 0x0002B307
	public int GetPlayerMat()
	{
		return this.playerMat;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00076F1C File Offset: 0x0007511C
	public void CheckPlayerName()
	{
		if (!this.SlotEnabled())
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.playerPreviousName) && string.IsNullOrEmpty(this.GetPlayerName()))
		{
			this.SetPlayerName(this.playerPreviousName);
		}
		if (this.playerType != -1 && (string.IsNullOrEmpty(this.GetPlayerName()) || this.GenericNameOnEntry()))
		{
			switch (this.playerType)
			{
			case 0:
				this.SetPlayerName(this.GenerateHumanPlayerName(this.slotId));
				break;
			case 1:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 1));
				break;
			case 2:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 2));
				break;
			case 3:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 3));
				break;
			}
		}
		this.SePlayerPreviousName(this.playerName.text);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00077000 File Offset: 0x00075200
	public bool GenericNameOnEntry()
	{
		Regex regex = new Regex("\\d");
		string text = this.GetPlayerName();
		text = regex.Replace(text, "");
		return text == "" || text == regex.Replace(this.GenerateHumanPlayerName(this.slotId), "") || text == regex.Replace(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 1), "") || text == regex.Replace(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 2), "") || text == regex.Replace(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 3), "");
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00074340 File Offset: 0x00072540
	private string GenerateHumanPlayerName(int id)
	{
		return ScriptLocalization.Get("Common/Player") + " " + (id + 1).ToString();
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x000770B8 File Offset: 0x000752B8
	public static string GenerateAIPlayerName(int id, int difficulty)
	{
		string text = ScriptLocalization.Get("Common/Bot") + " " + id.ToString();
		switch (difficulty)
		{
		case 1:
			return text + " " + ScriptLocalization.Get("Common/Easy");
		case 2:
			return text + " " + ScriptLocalization.Get("Common/Medium");
		case 3:
			return text + " " + ScriptLocalization.Get("Common/Hard");
		default:
			return text;
		}
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0007713C File Offset: 0x0007533C
	private void AddPlayerLocalize()
	{
		if (PlatformManager.IsStandalone)
		{
			this.addHumanPlayerText.text = "+ " + ScriptLocalization.Get("Common/Player");
			this.addBotEasyText.text = "+ " + ScriptLocalization.Get("Common/Bot") + " " + ScriptLocalization.Get("Common/Easy");
			this.addBotMediumText.text = "+ " + ScriptLocalization.Get("Common/Bot") + " " + ScriptLocalization.Get("Common/Medium");
			this.addBotHardText.text = "+ " + ScriptLocalization.Get("Common/Bot") + " " + ScriptLocalization.Get("Common/Hard");
			return;
		}
		this.addHumanPlayerText.text = ScriptLocalization.Get("Common/Player");
		this.addBotEasyText.text = ScriptLocalization.Get("Common/Bot") + " " + ScriptLocalization.Get("Common/Easy");
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0007723C File Offset: 0x0007543C
	public void UpdateTexts()
	{
		if (this.SlotEnabled())
		{
			switch (this.playerType)
			{
			case 0:
				this.SetPlayerName(this.GenerateHumanPlayerName(this.slotId));
				break;
			case 1:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 1));
				break;
			case 2:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 2));
				break;
			case 3:
				this.SetPlayerName(HotseatPlayerSetup.GenerateAIPlayerName(this.slotId, 3));
				break;
			}
		}
		this.UpdateFactionText();
		this.UpdatePlayerMat(this.playerMat);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000772D8 File Offset: 0x000754D8
	public void OnPlayerAdded(int playerType)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.playerMat = HotseatPanelController.NUMBER_OF_PLAYER_MATS;
		this.factionMat = HotseatPanelController.NUMBER_OF_FACTIONS;
		this.UpdatePlayerType(playerType);
		this.UpdateEntryState(true);
		this.hotseatPanel.PlayerAdded(this.slotId);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0002D10F File Offset: 0x0002B30F
	public void OnPlayerRemoved()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.RemovePlayer();
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002D123 File Offset: 0x0002B323
	public void RemovePlayer()
	{
		this.hotseatPanel.PlayerRemoved(this.slotId);
		this.UpdateEntryState(false);
		this.hotseatPanel.CheckBalance();
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002D148 File Offset: 0x0002B348
	public bool UnbalancedFactionAndMat()
	{
		return this.factionMat != HotseatPanelController.NUMBER_OF_FACTIONS && this.playerMat != HotseatPanelController.NUMBER_OF_PLAYER_MATS && MatAndFactionSelection.FactionIsOverpoweredWithPlayerMat((Faction)this.factionMat, (PlayerMatType)this.playerMat);
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00077328 File Offset: 0x00075528
	public void OnPreviousPlayerTypeClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		int num = 0;
		int num2 = 3;
		this.playerType--;
		if (this.playerType < num)
		{
			this.playerType = num2;
		}
		this.UpdatePlayerType(this.playerType);
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00077370 File Offset: 0x00075570
	public void OnNextPlayerTypeClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		int num = 0;
		int num2 = 3;
		this.playerType++;
		if (this.playerType > num2)
		{
			this.playerType = num;
		}
		this.UpdatePlayerType(this.playerType);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0002D177 File Offset: 0x0002B377
	public void UpdatePlayerType(int playerType)
	{
		this.playerType = playerType;
		this.SetPlayerTypeSprite(this.hotseatPanel.GetPlayerTypeSprite(playerType));
		this.CheckPlayerName();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002D198 File Offset: 0x0002B398
	public void SetPlayerTypeSprite(Sprite playerTypeSprite)
	{
		this.playerTypeImage.sprite = playerTypeSprite;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000773B8 File Offset: 0x000755B8
	public void UpdateFaction(int faction)
	{
		this.factionMat = faction;
		this.SetFactionSprite(this.hotseatPanel.GetFactionSprite(faction));
		this.UpdateFactionText();
		if (faction != HotseatPanelController.NUMBER_OF_FACTIONS)
		{
			this.SetBackgroundSprite(this.hotseatPanel.GetFactionBackgroundSprite(faction));
		}
		else if (faction == HotseatPanelController.NUMBER_OF_FACTIONS)
		{
			this.SetBackgroundSprite(this.hotseatPanel.GetFactionBackgroundSprite((GameServiceController.Instance.InvadersFromAfarUnlocked() && this.hotseatPanel.InvadersFromAfarUnlocked()) ? (faction + 1) : faction));
		}
		this.hotseatPanel.CheckBalance();
		this.UpdateAbilitiesChange(this.hotseatPanel.NewAbilitiesEnabled());
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00077454 File Offset: 0x00075654
	private void UpdateFactionText()
	{
		this.SetFactionText((this.factionMat != HotseatPanelController.NUMBER_OF_FACTIONS) ? ScriptLocalization.Get("FactionMat/" + Enum.GetName(typeof(Faction), this.factionMat)) : ScriptLocalization.Get("GameScene/Random"));
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0002D1A6 File Offset: 0x0002B3A6
	public void SetFactionSprite(Sprite factionLogo)
	{
		this.factionImage.sprite = factionLogo;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
	public void SetFactionText(string factionName)
	{
		this.factionMatName.text = factionName;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0002D1C2 File Offset: 0x0002B3C2
	public void OnPreviousFactionButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdateFaction(this.hotseatPanel.ChangeFactionSelection(this.slotId, this.factionMat, false));
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002D1EE File Offset: 0x0002B3EE
	public void OnNextFactionButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdateFaction(this.hotseatPanel.ChangeFactionSelection(this.slotId, this.factionMat, true));
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0002D21A File Offset: 0x0002B41A
	public void OnFactionDetailsClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_faction_details);
		this.hotseatPanel.EnableDetailedFactionSelection(this.slotId, this.factionMat);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002D23A File Offset: 0x0002B43A
	public void PlayerFactionButtonMobileDown()
	{
		this.isFactionButtonPressed = true;
		this.longClickFactionTimer = 0f;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002D24E File Offset: 0x0002B44E
	public void PlayerFactionButtonMobileUp()
	{
		this.isFactionButtonPressed = false;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x000774AC File Offset: 0x000756AC
	private void FactionLongClickUpdate()
	{
		if (this.isFactionButtonPressed)
		{
			this.longClickFactionTimer += Time.unscaledDeltaTime;
			if (this.longClickFactionTimer >= this.longClickTime)
			{
				this.longClickFactionTimer = 0f;
				this.isFactionButtonPressed = false;
				if (this.hotseatPanel.factionDetailedSelectionWindow.gameObject.activeSelf || this.hotseatPanel.playerMatDetailedSelectionWindow.gameObject.activeSelf)
				{
					return;
				}
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_faction_details);
				this.hotseatPanel.EnableDetailedFactionSelection(this.slotId, this.factionMat);
			}
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002D257 File Offset: 0x0002B457
	public void ShowFactionMatDetailsButton(bool show)
	{
		if (PlatformManager.IsStandalone)
		{
			if (!this.factionMatDetailsButton.interactable)
			{
				return;
			}
			if (!this.onFactionMatDetailsButton)
			{
				this.factionMatDetailsButton2.gameObject.SetActive(show);
			}
		}
		this.onFactionMatTrigger = show;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002D28E File Offset: 0x0002B48E
	public void PointerOnFactionDetailsButton(bool enter)
	{
		this.onFactionMatDetailsButton = enter;
		if (PlatformManager.IsStandalone && !this.onFactionMatTrigger && !enter)
		{
			this.factionMatDetailsButton2.gameObject.SetActive(false);
		}
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00077550 File Offset: 0x00075750
	public void UpdatePlayerMat(int playerMat)
	{
		this.playerMat = playerMat;
		this.SetPlayerMatTex((playerMat != HotseatPanelController.NUMBER_OF_PLAYER_MATS) ? ScriptLocalization.Get("PlayerMat/" + Enum.GetName(typeof(PlayerMatType), playerMat)) : ScriptLocalization.Get("GameScene/Random"));
		this.hotseatPanel.CheckBalance();
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002D2BA File Offset: 0x0002B4BA
	public void SetPlayerMatTex(string playerMatName)
	{
		this.playerMatName.text = playerMatName;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
	public void SetPlayerMatTextColor(Color color)
	{
		this.playerMatName.color = color;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002D2D6 File Offset: 0x0002B4D6
	public void SetFactionTextColor(Color color)
	{
		this.factionMatName.color = color;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
	public void OnPlayerNameInputClicked()
	{
		this.playerName.text = string.Empty;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002D2F6 File Offset: 0x0002B4F6
	public void OnPreviousPlayerMatButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMat(this.hotseatPanel.ChangePlayerMat(this.slotId, this.playerMat, false));
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0002D322 File Offset: 0x0002B522
	public void OnNextPlayerMatButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMat(this.hotseatPanel.ChangePlayerMat(this.slotId, this.playerMat, true));
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002D34E File Offset: 0x0002B54E
	public void OnPlayerMatDetailsClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_player_mat_details);
		this.hotseatPanel.EnableDetailedPlayerMatSelection(this.slotId, this.playerMat);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0002D36E File Offset: 0x0002B56E
	public void PlayerMatButtonMobileDown()
	{
		this.isMatButtonPressed = true;
		this.longClickMatTimer = 0f;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x0002D382 File Offset: 0x0002B582
	public void PlayerMatButtonMobileUp()
	{
		this.isMatButtonPressed = false;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x000775B0 File Offset: 0x000757B0
	private void MatLongClickUpdate()
	{
		if (this.isMatButtonPressed)
		{
			this.longClickMatTimer += Time.unscaledDeltaTime;
			if (this.longClickMatTimer >= this.longClickTime)
			{
				this.longClickMatTimer = 0f;
				this.isMatButtonPressed = false;
				if (this.hotseatPanel.factionDetailedSelectionWindow.gameObject.activeSelf || this.hotseatPanel.playerMatDetailedSelectionWindow.gameObject.activeSelf)
				{
					return;
				}
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_player_mat_details);
				this.hotseatPanel.EnableDetailedPlayerMatSelection(this.slotId, this.playerMat);
			}
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0002D38B File Offset: 0x0002B58B
	public void ShowPlayerMatDetailsButton(bool show)
	{
		if (!this.playerMatDetailsButton.interactable)
		{
			return;
		}
		if (!this.onPlayerMatDetailsButton)
		{
			this.playerMatDetailsButton2.gameObject.SetActive(show);
		}
		this.onPlayerMatTrigger = show;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002D3BB File Offset: 0x0002B5BB
	public void PointerOnPlayerMatDetailsButton(bool enter)
	{
		this.onPlayerMatDetailsButton = enter;
		if (!this.onPlayerMatTrigger && !enter)
		{
			this.playerMatDetailsButton2.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000699 RID: 1689
	[Header("Always visible")]
	[SerializeField]
	private HotseatPanelController hotseatPanel;

	// Token: 0x0400069A RID: 1690
	[SerializeField]
	private Image background;

	// Token: 0x0400069B RID: 1691
	[SerializeField]
	private InputField playerName;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private Image playerNameBackground;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private GameObject IFAExpansionInfo;

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	private TextMeshProUGUI playerMatName;

	// Token: 0x0400069F RID: 1695
	[SerializeField]
	private Button previousPlayerMat;

	// Token: 0x040006A0 RID: 1696
	[SerializeField]
	private Button nextPlayerMat;

	// Token: 0x040006A1 RID: 1697
	[SerializeField]
	private Button playerMatDetailsButton2;

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	private Button playerMatDetailsButton;

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	private GameObject playerMatDetails;

	// Token: 0x040006A4 RID: 1700
	[Header("Hotseat slot pre-setup")]
	[SerializeField]
	private Transform addPlayerGroup;

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	private Button addHumanPlayer;

	// Token: 0x040006A6 RID: 1702
	[SerializeField]
	private Button addBotEasy;

	// Token: 0x040006A7 RID: 1703
	[SerializeField]
	private Button addBotMedium;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private Button addBotHard;

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	private TextMeshProUGUI addHumanPlayerText;

	// Token: 0x040006AA RID: 1706
	[SerializeField]
	private TextMeshProUGUI addBotEasyText;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	private TextMeshProUGUI addBotMediumText;

	// Token: 0x040006AC RID: 1708
	[SerializeField]
	private TextMeshProUGUI addBotHardText;

	// Token: 0x040006AD RID: 1709
	[Header("Hotseat detailed setup")]
	[SerializeField]
	private Transform playerDetailedGroup;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private Button removePlayer;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private Image playerTypeImage;

	// Token: 0x040006B0 RID: 1712
	[SerializeField]
	private Button previousPlayerType;

	// Token: 0x040006B1 RID: 1713
	[SerializeField]
	private Button nextPlayerType;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private Button playerTypeButton;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	private Image factionImage;

	// Token: 0x040006B4 RID: 1716
	[SerializeField]
	private TextMeshProUGUI factionMatName;

	// Token: 0x040006B5 RID: 1717
	[SerializeField]
	private Button previousFactionMat;

	// Token: 0x040006B6 RID: 1718
	[SerializeField]
	private Button nextFactionMat;

	// Token: 0x040006B7 RID: 1719
	[SerializeField]
	private Button factionMatDetailsButton2;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	private Button factionMatDetailsButton;

	// Token: 0x040006B9 RID: 1721
	[SerializeField]
	private GameObject abilityChangeInfo;

	// Token: 0x040006BA RID: 1722
	[SerializeField]
	private HotseatAbilityChangeInfo abilityChangeInfoMobile;

	// Token: 0x040006BB RID: 1723
	private int slotId = -1;

	// Token: 0x040006BC RID: 1724
	private int playerType = -1;

	// Token: 0x040006BD RID: 1725
	private int factionMat = -1;

	// Token: 0x040006BE RID: 1726
	private int playerMat = -1;

	// Token: 0x040006BF RID: 1727
	private bool onFactionMatDetailsButton;

	// Token: 0x040006C0 RID: 1728
	private bool onPlayerMatDetailsButton;

	// Token: 0x040006C1 RID: 1729
	private bool onFactionMatTrigger;

	// Token: 0x040006C2 RID: 1730
	private bool onPlayerMatTrigger;

	// Token: 0x040006C3 RID: 1731
	private bool isMatButtonPressed;

	// Token: 0x040006C4 RID: 1732
	private bool isFactionButtonPressed;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private float longClickTime = 0.25f;

	// Token: 0x040006C6 RID: 1734
	private float longClickFactionTimer;

	// Token: 0x040006C7 RID: 1735
	private float longClickMatTimer;

	// Token: 0x040006C8 RID: 1736
	private Color32 matTextColor = new Color32(214, 207, 193, byte.MaxValue);

	// Token: 0x040006C9 RID: 1737
	private string playerPreviousName = string.Empty;
}
