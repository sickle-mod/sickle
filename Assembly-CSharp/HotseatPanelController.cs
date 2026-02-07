using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000F0 RID: 240
public class HotseatPanelController : MonoBehaviour
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x00074AF8 File Offset: 0x00072CF8
	public static int MAXIMUM_NUMBER_OF_PLAYERS
	{
		get
		{
			int num = 7;
			if (PlatformManager.IsMobile && !GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				num = 5;
			}
			return num;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
	public static int NUMBER_OF_FACTIONS
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
	public static int NUMBER_OF_PLAYER_MATS
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002CB0F File Offset: 0x0002AD0F
	private void Awake()
	{
		OptionsManager.OnLanguageChanged += this.UpdateTexts;
		this.factionDetailedSelectionWindow.OnPlayerFactionSelection += this.FactionDetailedSelectionWindow_OnFactionSelection;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002CB39 File Offset: 0x0002AD39
	private void OnEnable()
	{
		this.Initialize();
		this.exitButton.onClick.AddListener(new UnityAction(this.HideHotseatMenu));
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002CB5D File Offset: 0x0002AD5D
	private void OnDestroy()
	{
		OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		this.factionDetailedSelectionWindow.OnPlayerFactionSelection -= this.FactionDetailedSelectionWindow_OnFactionSelection;
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00074B20 File Offset: 0x00072D20
	private void OnDisable()
	{
		this.SaveHotseatPrefs();
		this.exitButton.onClick.RemoveListener(new UnityAction(this.HideHotseatMenu));
		if (this.factionDetailedSelectionWindow != null)
		{
			this.factionDetailedSelectionWindow.gameObject.SetActive(false);
		}
		if (this.playerMatDetailedSelectionWindow != null)
		{
			this.playerMatDetailedSelectionWindow.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0002CB87 File Offset: 0x0002AD87
	public void Initialize()
	{
		this.InitializeHotseatControls();
		this.LoadHotseatPrefs();
		this.UpdateTexts();
		this.CheckInvadersFromAfarOwningState();
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00074B90 File Offset: 0x00072D90
	public void SaveHotseatPrefs()
	{
		List<MatAndFactionSelection.PlayerEntry> playersAndTheirSettings = this.GetPlayersAndTheirSettings();
		this.SaveHotseatPrefs(playersAndTheirSettings);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00074BAC File Offset: 0x00072DAC
	public void SaveHotseatPrefs(List<MatAndFactionSelection.PlayerEntry> players)
	{
		this.factionDetailedSelectionWindow.SaveSelectedFactions();
		this.playerMatDetailedSelectionWindow.SaveSelectedMats();
		PlayerPrefs.SetInt("HotseatPlayersAmount", players.Count);
		PlayerPrefs.SetInt("HotseatPromoCards", this.promoCardsToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("HotseatBalance", this.matBalanceToggle.isOn ? 1 : 0);
		for (int i = 0; i < players.Count; i++)
		{
			PlayerPrefs.SetString("HotseatPlayerName" + i.ToString(), players[i].Name);
			PlayerPrefs.SetInt("HotseatPlayerType" + i.ToString(), players[i].Type);
			PlayerPrefs.SetInt("HotseatPlayerFaction" + i.ToString(), players[i].Faction);
			PlayerPrefs.SetInt("HotseatPlayerMat" + i.ToString(), players[i].PlayerMat);
		}
		for (int j = players.Count; j < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; j++)
		{
			PlayerPrefs.DeleteKey("HotseatPlayerName" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerType" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerFaction" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerMat" + j.ToString());
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00074D28 File Offset: 0x00072F28
	private void LoadHotseatPrefs()
	{
		int num = PlayerPrefs.GetInt("HotseatPlayersAmount", 0);
		if (num == 0)
		{
			return;
		}
		if (num > HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS)
		{
			num = HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS;
		}
		this.firstAvaliableSlot = num;
		this.startButton.interactable = num >= 2;
		this.TurnOffPlayers();
		this.promoCardsToggle.isOn = PlayerPrefs.GetInt("HotseatPromoCards", 1) == 1;
		this.matBalanceToggle.isOn = PlayerPrefs.GetInt("HotseatBalance", 1) == 1;
		this.factionDetailedSelectionWindow.EnableIFASelection(this.InvadersFromAfarUnlocked());
		this.playerMatDetailedSelectionWindow.EnableIFASelection(this.InvadersFromAfarUnlocked());
		for (int i = 0; i < this.firstAvaliableSlot; i++)
		{
			this.hotseatPlayerSetups[i].LoadHotseatPlayerPrefs(i);
		}
		this.CheckAbilityChange();
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00074DF0 File Offset: 0x00072FF0
	private void TurnOffPlayers()
	{
		for (int i = 0; i < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; i++)
		{
			this.hotseatPlayerSetups[i].UpdateEntryState(false);
		}
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00074E20 File Offset: 0x00073020
	private void ResetPlayerSlots()
	{
		this.TurnOffPlayers();
		this.firstAvaliableSlot = 2;
		this.factionDetailedSelectionWindow.ResetFactions();
		this.playerMatDetailedSelectionWindow.ResetPlayerMats();
		for (int i = 0; i < this.firstAvaliableSlot; i++)
		{
			int num = i;
			this.hotseatPlayerSetups[num].Initialize(num, this);
			this.hotseatPlayerSetups[num].UpdateEntryState(true);
			if (i > 0)
			{
				this.hotseatPlayerSetups[num].SetRemoveable(true);
			}
			else
			{
				this.hotseatPlayerSetups[num].SetRemoveable(false);
			}
			this.UpdateIFASlot(num);
		}
		for (int j = this.firstAvaliableSlot; j < this.hotseatPlayerSetups.Count; j++)
		{
			this.hotseatPlayerSetups[j].UpdateEntryState(false);
		}
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x0002CBA1 File Offset: 0x0002ADA1
	public void UpdateFactionMatSelection(int faction, bool selected)
	{
		if (faction != HotseatPanelController.NUMBER_OF_FACTIONS)
		{
			this.factionDetailedSelectionWindow.UpdateSelectedFactions(faction, selected);
		}
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0002CBB8 File Offset: 0x0002ADB8
	public void UpdatePlayerMatSelection(int playerMat, bool selected)
	{
		if (playerMat != HotseatPanelController.NUMBER_OF_PLAYER_MATS)
		{
			this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(playerMat, selected);
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00074EE8 File Offset: 0x000730E8
	private void InitializeHotseatControls()
	{
		this.firstAvaliableSlot = 2;
		int num = 0;
		foreach (HotseatPlayerSetup hotseatPlayerSetup in this.hotseatPlayerSetups)
		{
			int num2 = num;
			hotseatPlayerSetup.Initialize(num2, this);
			if (num < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS)
			{
				if (num > 0)
				{
					hotseatPlayerSetup.SetRemoveable(true);
				}
				else
				{
					hotseatPlayerSetup.SetRemoveable(false);
				}
				if (num < this.firstAvaliableSlot)
				{
					hotseatPlayerSetup.UpdateEntryState(true);
				}
				else
				{
					hotseatPlayerSetup.UpdateEntryState(false);
				}
				this.UpdateIFASlot(num);
			}
			else
			{
				hotseatPlayerSetup.UpdateEntryState(false);
			}
			num++;
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0002CBCF File Offset: 0x0002ADCF
	private void UpdateIFASlot(int id)
	{
		if (id > 4)
		{
			if (this.InvadersFromAfarUnlocked() && GameServiceController.Instance.InvadersFromAfarUnlocked())
			{
				this.hotseatPlayerSetups[id].EnableIFASlot(true);
				return;
			}
			this.hotseatPlayerSetups[id].EnableIFASlot(false);
		}
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00074F90 File Offset: 0x00073190
	private void UpdateTexts()
	{
		for (int i = 0; i < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; i++)
		{
			this.hotseatPlayerSetups[i].UpdateTexts();
		}
		if (this.newGameStep1Label != null)
		{
			this.newGameStep1Label.text = ScriptLocalization.Get("MainMenu/NewGame") + " 1<font=\"LeagueGothic-Regular SDF Black\">/</font>2";
		}
		if (this.newGameStep2Label != null)
		{
			this.newGameStep2Label.text = ScriptLocalization.Get("MainMenu/NewGame") + " 2<font=\"LeagueGothic-Regular SDF Black\">/</font>2";
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0002CC0E File Offset: 0x0002AE0E
	public int ChangeFactionSelection(int id, int currentFaction, bool next)
	{
		return this.factionDetailedSelectionWindow.ChangeFactionSelection(id, currentFaction, next);
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0002CC1E File Offset: 0x0002AE1E
	public void UpdateFactionSelections(int playerId, int faction)
	{
		this.hotseatPlayerSetups[playerId].UpdateFaction(faction);
		this.CheckAbilityChange();
		if (PlatformManager.IsStandalone)
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
		}
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0002CC4B File Offset: 0x0002AE4B
	public void EnableDetailedFactionSelection(int id, int faction)
	{
		this.factionDetailedSelectionWindow.EnableDetailedFactionSelection(id, this.hotseatPlayerSetups[id].GetPlayerFaction());
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0002CC6A File Offset: 0x0002AE6A
	public int ChangePlayerMat(int id, int currentPlayerMat, bool next)
	{
		return this.playerMatDetailedSelectionWindow.ChangePlayerMat(id, currentPlayerMat, next);
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0002CC7A File Offset: 0x0002AE7A
	public void EnableDetailedPlayerMatSelection(int id, int playerMat)
	{
		this.playerMatDetailedSelectionWindow.EnableDetailedPlayerMatSelection(id, this.hotseatPlayerSetups[id].GetPlayerMat());
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0002CC99 File Offset: 0x0002AE99
	private bool UnbalancedFactionAndPlayerMatCombination(int slotId)
	{
		return (this.matBalanceToggle.isOn && this.hotseatPlayerSetups[slotId].UnbalancedFactionAndMat()) || this.TheLastAndOnlySlotWithRandomValues(slotId);
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0002CCC4 File Offset: 0x0002AEC4
	private bool UnbalancedFactionAndMat(int faction, int matType)
	{
		return faction != HotseatPanelController.NUMBER_OF_FACTIONS && matType != HotseatPanelController.NUMBER_OF_PLAYER_MATS && MatAndFactionSelection.FactionIsOverpoweredWithPlayerMat((Faction)faction, (PlayerMatType)matType);
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0002CCDF File Offset: 0x0002AEDF
	private bool TheLastAndOnlySlotWithRandomValues(int slotId)
	{
		return this.hotseatPlayerSetups[slotId].SlotEnabled() && (this.FullRandom(slotId) || this.RusvietFactionSelected(slotId) || this.IndustrialPlayerMatSelected(slotId));
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00075018 File Offset: 0x00073218
	private bool FullRandom(int slotId)
	{
		return this.hotseatPlayerSetups[slotId].GetPlayerMat() == HotseatPanelController.NUMBER_OF_PLAYER_MATS && this.hotseatPlayerSetups[slotId].GetPlayerFaction() == HotseatPanelController.NUMBER_OF_FACTIONS && ((this.factionDetailedSelectionWindow.IsLastAvaliableFaction(3) && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(0)) || (this.factionDetailedSelectionWindow.IsLastAvaliableFaction(5) && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(2)));
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00075094 File Offset: 0x00073294
	private bool RusvietFactionSelected(int slotId)
	{
		return this.hotseatPlayerSetups[slotId].GetPlayerMat() == HotseatPanelController.NUMBER_OF_PLAYER_MATS && ((this.hotseatPlayerSetups[slotId].GetPlayerFaction() == 3 && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(0)) || (this.hotseatPlayerSetups[slotId].GetPlayerFaction() == 5 && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(2)));
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00075104 File Offset: 0x00073304
	private bool IndustrialPlayerMatSelected(int slotId)
	{
		return this.hotseatPlayerSetups[slotId].GetPlayerFaction() == HotseatPanelController.NUMBER_OF_FACTIONS && ((this.hotseatPlayerSetups[slotId].GetPlayerMat() == 0 && this.factionDetailedSelectionWindow.IsLastAvaliableFaction(3)) || (this.hotseatPlayerSetups[slotId].GetPlayerMat() == 2 && this.factionDetailedSelectionWindow.IsLastAvaliableFaction(5)));
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00075170 File Offset: 0x00073370
	public void UpdatePlayerMatSelections()
	{
		int playerId = this.playerMatDetailedSelectionWindow.GetPlayerId();
		this.hotseatPlayerSetups[playerId].UpdatePlayerMat(this.playerMatDetailedSelectionWindow.GetCurrentPlayerMat());
		if (PlatformManager.IsStandalone)
		{
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000751B8 File Offset: 0x000733B8
	public List<MatAndFactionSelection.PlayerEntry> GetPlayersAndTheirSettings()
	{
		bool flag = GameServiceController.Instance.InvadersFromAfarUnlocked();
		List<MatAndFactionSelection.PlayerEntry> list = new List<MatAndFactionSelection.PlayerEntry>();
		for (int i = 0; i < this.hotseatPlayerSetups.Count; i++)
		{
			this.hotseatPlayerSetups[i].CheckPlayerName();
			if (this.hotseatPlayerSetups[i].SlotEnabled())
			{
				list.Add(new MatAndFactionSelection.PlayerEntry(this.hotseatPlayerSetups[i].GetPlayerName().Replace("\r", "").Replace("\n", ""), this.hotseatPlayerSetups[i].GetPlayerType(), flag, this.hotseatPlayerSetups[i].GetPlayerFaction(), this.hotseatPlayerSetups[i].GetPlayerMat(), default(Guid)));
			}
		}
		return list;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0002CD11 File Offset: 0x0002AF11
	public Sprite GetFactionSprite(int id)
	{
		return this.factionIcons[id];
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002CD1B File Offset: 0x0002AF1B
	public Sprite GetFactionBackgroundSprite(int id)
	{
		return this.factionBackgrounds[id];
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0002CD25 File Offset: 0x0002AF25
	public Sprite GetPlayerTypeSprite(int id)
	{
		if (id == -1)
		{
			Debug.LogError("Type id is negative: " + id.ToString());
			return this.playerTypeIcons[2];
		}
		return this.playerTypeIcons[id];
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x00075290 File Offset: 0x00073490
	public void PlayerRemoved(int slotId)
	{
		int num = (PlatformManager.IsMobile ? slotId : (slotId + 1));
		if (slotId == this.firstAvaliableSlot - 1 && num < this.GetNumberOfAvaliablePlayerSlots())
		{
			this.firstAvaliableSlot = slotId;
			if (this.firstAvaliableSlot + 1 != this.GetNumberOfAvaliablePlayerSlots())
			{
				this.hotseatPlayerSetups[this.firstAvaliableSlot + 1].UpdateEntryState(false);
			}
			if (slotId == 1)
			{
				this.startButton.interactable = false;
			}
		}
		else
		{
			HotseatPlayerSetup hotseatPlayerSetup = this.hotseatPlayerSetups[slotId];
			this.hotseatPlayerSetups.RemoveAt(slotId);
			this.hotseatPlayerSetups.Add(hotseatPlayerSetup);
			hotseatPlayerSetup.transform.SetAsLastSibling();
			for (int i = 0; i < this.hotseatPlayerSetups.Count; i++)
			{
				int num2 = i;
				this.hotseatPlayerSetups[i].SetSlotId(num2);
			}
			this.firstAvaliableSlot--;
			if (!this.InvadersFromAfarUnlocked())
			{
				if (slotId < 5)
				{
					this.hotseatPlayerSetups[4].EnableIFASlot(true);
				}
				if (slotId < 6)
				{
					this.hotseatPlayerSetups[6].EnableIFASlot(false);
				}
			}
		}
		this.CheckAbilityChange();
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x000753AC File Offset: 0x000735AC
	public void PlayerAdded(int slotId)
	{
		this.firstAvaliableSlot++;
		if (this.firstAvaliableSlot != this.GetNumberOfAvaliablePlayerSlots())
		{
			this.hotseatPlayerSetups[this.firstAvaliableSlot].UpdateEntryState(false);
		}
		this.CheckAbilityChange();
		if (!this.startButton.interactable)
		{
			this.startButton.interactable = true;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0007540C File Offset: 0x0007360C
	private void CheckAbilityChange()
	{
		foreach (HotseatPlayerSetup hotseatPlayerSetup in this.hotseatPlayerSetups)
		{
			hotseatPlayerSetup.UpdateAbilitiesChange(this.NewAbilitiesEnabled());
		}
		this.factionDetailedSelectionWindow.MoreThat5Players(this.NewAbilitiesEnabled());
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002CD52 File Offset: 0x0002AF52
	public bool NewAbilitiesEnabled()
	{
		return this.firstAvaliableSlot > 5;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0002CD5D File Offset: 0x0002AF5D
	public int GetNumberOfAvaliablePlayerSlots()
	{
		if (!GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.InvadersFromAfarUnlocked())
		{
			return 5;
		}
		return 7;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0002CD76 File Offset: 0x0002AF76
	public bool IsFirstAvaliableSlot(int slotId)
	{
		return slotId == this.firstAvaliableSlot;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0002CD81 File Offset: 0x0002AF81
	public void OnBalanceCheckboxValueChanged(bool state)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		this.CheckBalance();
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00075474 File Offset: 0x00073674
	public void CheckBalance()
	{
		bool isOn = this.matBalanceToggle.isOn;
		this.ActivateMatBalanceInfo(false);
		for (int i = 0; i < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; i++)
		{
			if (this.UnbalancedFactionAndPlayerMatCombination(i) && isOn)
			{
				this.hotseatPlayerSetups[i].SetPlayerMatTextColor(this.bannedMatColor);
				this.hotseatPlayerSetups[i].SetFactionTextColor(this.bannedMatColor);
				this.matBalanceTooltip.color = this.bannedMatColor;
				this.ActivateMatBalanceInfo(true);
			}
			else
			{
				this.hotseatPlayerSetups[i].SetPlayerMatTextColor(this.matTextColor);
				this.hotseatPlayerSetups[i].SetFactionTextColor(this.matTextColor);
				this.matBalanceTooltip.color = this.matTextColor;
			}
		}
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0002CD95 File Offset: 0x0002AF95
	private void ActivateMatBalanceInfo(bool state)
	{
		this.startButton.gameObject.SetActive(!state);
		this.startButtonInfo.gameObject.SetActive(state);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0002CDBC File Offset: 0x0002AFBC
	public bool PromoCardsUnlocked()
	{
		return this.promoCardsToggle.isOn;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
	public void PromoCardsClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x0002CDC9 File Offset: 0x0002AFC9
	public bool InvadersFromAfarUnlocked()
	{
		if (PlatformManager.IsMobile)
		{
			return GameServiceController.Instance.InvadersFromAfarUnlocked();
		}
		return this.ifaToggle.isOn;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0007555C File Offset: 0x0007375C
	public void CheckInvadersFromAfarOwningState()
	{
		if (PlatformManager.IsMobile)
		{
			this.factionDetailedSelectionWindow.EnableIFASelection(this.InvadersFromAfarUnlocked());
			this.playerMatDetailedSelectionWindow.EnableIFASelection(this.InvadersFromAfarUnlocked());
			return;
		}
		if (GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			this.IFABanner.SetActive(false);
			this.IFACheckNowButton.gameObject.SetActive(false);
			this.ifaToggle.interactable = true;
			this.ifaToggle.isOn = true;
			return;
		}
		this.IFABanner.SetActive(true);
		this.IFACheckNowButton.gameObject.SetActive(true);
		this.ifaToggle.interactable = false;
		this.ifaToggle.isOn = false;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
	public int GetUndoType()
	{
		if (this.undoLastAction.isOn)
		{
			return 0;
		}
		if (this.undoUnlimited.isOn)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0002CE09 File Offset: 0x0002B009
	public bool Balanced()
	{
		return this.matBalanceToggle.isOn;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002CE16 File Offset: 0x0002B016
	public void OnDLCCheckInvadersClicked()
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_dlc_check_now_invaders);
		this.mainMenu.OpenInvadersFromAfarStoreSite();
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0007560C File Offset: 0x0007380C
	public void OnResetMats()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		for (int i = 0; i < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; i++)
		{
			if (this.hotseatPlayerSetups[i].SlotEnabled())
			{
				this.hotseatPlayerSetups[i].ResetSelection();
			}
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0007565C File Offset: 0x0007385C
	public void OnRandomizeMats()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		int num;
		int num2;
		int num3;
		int num4;
		do
		{
			this.OnResetMats();
			bool flag = GameServiceController.Instance.InvadersFromAfarUnlocked() && this.InvadersFromAfarUnlocked();
			List<int> list = MatAndFactionSelection.SetAvailableFactions(flag);
			List<int> list2 = MatAndFactionSelection.SetAvailablePlayerMats(flag);
			num = -1;
			num2 = -1;
			num3 = -1;
			num4 = -1;
			for (int i = 0; i < HotseatPanelController.MAXIMUM_NUMBER_OF_PLAYERS; i++)
			{
				if (this.hotseatPlayerSetups[i].SlotEnabled())
				{
					int num5 = global::UnityEngine.Random.Range(0, list.Count);
					if (list[num5] == 3)
					{
						num = i;
					}
					if (list[num5] == 5)
					{
						num3 = i;
					}
					this.hotseatPlayerSetups[i].UpdateFaction(list[num5]);
					this.factionDetailedSelectionWindow.UpdateSelectedFactions(list[num5], true);
					list.RemoveAt(num5);
					int num6 = global::UnityEngine.Random.Range(0, list2.Count);
					if (list2[num6] == 0)
					{
						num2 = i;
					}
					if (list2[num6] == 2)
					{
						num4 = i;
					}
					this.hotseatPlayerSetups[i].UpdatePlayerMat(list2[num6]);
					this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(list2[num6], true);
					list2.RemoveAt(num6);
				}
			}
		}
		while ((num == num2 || num3 == num4) && this.matBalanceToggle.isOn);
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002CE2A File Offset: 0x0002B02A
	public void OnStart()
	{
		this.mainMenu.OnStartGame();
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0002CE37 File Offset: 0x0002B037
	public void OnIFAChanged(bool state)
	{
		if (state)
		{
			this.EnableIFA();
		}
		else
		{
			this.DisableIFA();
		}
		this.factionDetailedSelectionWindow.EnableIFASelection(state);
		this.playerMatDetailedSelectionWindow.EnableIFASelection(state);
		this.CheckBalance();
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000757C4 File Offset: 0x000739C4
	private void EnableIFA()
	{
		int num = 6;
		this.UpdateIFASlot(num);
		this.UpdateIFASlot(num - 1);
		foreach (HotseatPlayerSetup hotseatPlayerSetup in this.hotseatPlayerSetups)
		{
			hotseatPlayerSetup.UpdateIFAData(true);
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00075828 File Offset: 0x00073A28
	private void DisableIFA()
	{
		int num = 6;
		foreach (HotseatPlayerSetup hotseatPlayerSetup in this.hotseatPlayerSetups)
		{
			hotseatPlayerSetup.UpdateIFAData(false);
		}
		if (this.hotseatPlayerSetups[num].SlotEnabled())
		{
			this.hotseatPlayerSetups[num].RemovePlayer();
		}
		if (this.hotseatPlayerSetups[num - 1].SlotEnabled())
		{
			this.hotseatPlayerSetups[num - 1].RemovePlayer();
		}
		this.UpdateIFASlot(num);
		this.UpdateIFASlot(num - 1);
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0002CE68 File Offset: 0x0002B068
	public void NextPanel()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		this.gameOptions.SetActive(false);
		this.playersPanel.SetActive(true);
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0002CE8E File Offset: 0x0002B08E
	public void BackToOptions()
	{
		this.gameOptions.SetActive(true);
		this.playersPanel.SetActive(false);
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0002CEB4 File Offset: 0x0002B0B4
	public void HideHotseatMenu()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.play_local_window, Contexts.outgame);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
	private void FactionDetailedSelectionWindow_OnFactionSelection(int playerId, int faction)
	{
		this.UpdateFactionSelections(playerId, faction);
	}

	// Token: 0x04000678 RID: 1656
	[SerializeField]
	private MainMenu mainMenu;

	// Token: 0x04000679 RID: 1657
	public GameObject hotseatWindow;

	// Token: 0x0400067A RID: 1658
	public Transform hotseatPanel;

	// Token: 0x0400067B RID: 1659
	public FactionSelectionWindow factionDetailedSelectionWindow;

	// Token: 0x0400067C RID: 1660
	public PlayerMatSelectionWindow playerMatDetailedSelectionWindow;

	// Token: 0x0400067D RID: 1661
	public ToggleGroup undoToggleGroup;

	// Token: 0x0400067E RID: 1662
	public Toggle undoLastAction;

	// Token: 0x0400067F RID: 1663
	public Toggle undoUnlimited;

	// Token: 0x04000680 RID: 1664
	public Toggle matBalanceToggle;

	// Token: 0x04000681 RID: 1665
	public Toggle promoCardsToggle;

	// Token: 0x04000682 RID: 1666
	public Toggle ifaToggle;

	// Token: 0x04000683 RID: 1667
	public Button startButton;

	// Token: 0x04000684 RID: 1668
	public GameObject startButtonInfo;

	// Token: 0x04000685 RID: 1669
	[SerializeField]
	private GameObject IFAExpansion;

	// Token: 0x04000686 RID: 1670
	[SerializeField]
	private GameObject IFABanner;

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private Button IFACheckNowButton;

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private List<HotseatPlayerSetup> hotseatPlayerSetups;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	private Sprite[] factionIcons = new Sprite[8];

	// Token: 0x0400068A RID: 1674
	[SerializeField]
	private Sprite[] factionBackgrounds = new Sprite[10];

	// Token: 0x0400068B RID: 1675
	[SerializeField]
	private Sprite[] playerTypeIcons;

	// Token: 0x0400068C RID: 1676
	[Header("Mobile only")]
	[SerializeField]
	private GameObject gameOptions;

	// Token: 0x0400068D RID: 1677
	[SerializeField]
	private GameObject playersPanel;

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	private TextMeshProUGUI newGameStep1Label;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private TextMeshProUGUI newGameStep2Label;

	// Token: 0x04000690 RID: 1680
	[SerializeField]
	private Button exitButton;

	// Token: 0x04000691 RID: 1681
	public const int BACKGROUND_EMPTY = 9;

	// Token: 0x04000692 RID: 1682
	private int firstAvaliableSlot = 2;

	// Token: 0x04000693 RID: 1683
	public const string HOTSEAT_PLAYERS_AMOUNT = "HotseatPlayersAmount";

	// Token: 0x04000694 RID: 1684
	public const string HOTSEAT_PLAYER_NAME = "HotseatPlayerName";

	// Token: 0x04000695 RID: 1685
	public const string HOTSEAT_PLAYER_TYPE = "HotseatPlayerType";

	// Token: 0x04000696 RID: 1686
	private Color32 matTextColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 175);

	// Token: 0x04000697 RID: 1687
	private Color32 bannedMatColor = new Color32(211, 47, 47, byte.MaxValue);

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	private Text matBalanceTooltip;
}
