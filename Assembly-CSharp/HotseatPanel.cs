using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EE RID: 238
public class HotseatPanel : MonoBehaviour
{
	// Token: 0x06000718 RID: 1816 RVA: 0x0002C93F File Offset: 0x0002AB3F
	private void Awake()
	{
		this.InitializeHotseatControls();
		this.LoadHotseatPrefs();
		OptionsManager.OnLanguageChanged += this.UpdateTexts;
		this.UpdateTexts();
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0002C964 File Offset: 0x0002AB64
	private void OnDestroy()
	{
		OptionsManager.OnLanguageChanged -= this.UpdateTexts;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000732B0 File Offset: 0x000714B0
	private void OnEnable()
	{
		int @int = PlayerPrefs.GetInt("HotseatPlayersAmount4", 0);
		for (int i = 2; i < this.hotseatPlayerName.Length; i++)
		{
			if (this.hotseatPlayerToggle[i, 0].isOn || this.hotseatPlayerToggle[i, 1].isOn || this.hotseatPlayerToggle[i, 2].isOn || this.hotseatPlayerToggle[i, 3].isOn)
			{
				this.addRemovePlayer[i].transform.GetChild(1).gameObject.SetActive(true);
				this.addRemovePlayer[i].transform.GetChild(0).gameObject.SetActive(false);
			}
			else
			{
				this.addRemovePlayer[i].transform.GetChild(0).gameObject.SetActive(true);
				this.addRemovePlayer[i].transform.GetChild(1).gameObject.SetActive(false);
			}
			if (this.HotseatPlayerToggleCount(i) > 1)
			{
				this.hotseatPlayerToggle[i, 0].isOn = false;
			}
		}
		if (@int != 0)
		{
			return;
		}
		for (int j = 0; j < 5; j++)
		{
			if (this.hotseatPlayerToggle[j, 0].isOn)
			{
				if (j == 0)
				{
					this.hotseatPlayerName[j].text = AsmodeeLogic.Instance.GetPlayerName();
				}
				else
				{
					this.hotseatPlayerName[j].text = this.GenerateHumanPlayerName(j);
				}
			}
			else if (this.hotseatPlayerToggle[j, 1].isOn)
			{
				this.hotseatPlayerName[j].text = HotseatPanel.GenerateAIPlayerName(j, 1);
			}
			else if (this.hotseatPlayerToggle[j, 2].isOn)
			{
				this.hotseatPlayerName[j].text = HotseatPanel.GenerateAIPlayerName(j, 2);
			}
			else if (this.hotseatPlayerToggle[j, 3].isOn)
			{
				this.hotseatPlayerName[j].text = HotseatPanel.GenerateAIPlayerName(j, 3);
			}
			else
			{
				this.hotseatPlayerName[j].text = "";
			}
		}
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000734BC File Offset: 0x000716BC
	private void OnDisable()
	{
		this.SaveHotseatPrefs();
		if (this.factionDetailedSelectionWindow != null)
		{
			this.factionDetailedSelectionWindow.gameObject.SetActive(false);
		}
		if (this.playerMatDetailedSelectionWindow != null)
		{
			this.playerMatDetailedSelectionWindow.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00073510 File Offset: 0x00071710
	public void SaveHotseatPrefs()
	{
		List<MatAndFactionSelection.PlayerEntry> playersAndTheirSettings = this.GetPlayersAndTheirSettings();
		this.SaveHotseatPrefs(playersAndTheirSettings);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0007352C File Offset: 0x0007172C
	public void SaveHotseatPrefs(List<MatAndFactionSelection.PlayerEntry> players)
	{
		this.factionDetailedSelectionWindow.SaveSelectedFactions();
		this.playerMatDetailedSelectionWindow.SaveSelectedMats();
		PlayerPrefs.SetInt("HotseatPlayersAmount4", players.Count);
		PlayerPrefs.SetInt("HotseatPromoCards", this.promoCardsToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("HotseatBalance", this.matBalanceToggle.isOn ? 1 : 0);
		for (int i = 0; i < players.Count; i++)
		{
			PlayerPrefs.SetString("HotseatPlayerName4" + i.ToString(), players[i].Name);
			PlayerPrefs.SetInt("HotseatPlayerType4" + i.ToString(), players[i].Type);
			PlayerPrefs.SetInt("HotseatPlayerFaction" + i.ToString(), players[i].Faction);
			PlayerPrefs.SetInt("HotseatPlayerMat" + i.ToString(), players[i].PlayerMat);
		}
		for (int j = players.Count; j < 5; j++)
		{
			PlayerPrefs.DeleteKey("HotseatPlayerName4" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerType4" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerFaction" + j.ToString());
			PlayerPrefs.DeleteKey("HotseatPlayerMat" + j.ToString());
		}
		PlayerPrefs.Save();
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x000736A4 File Offset: 0x000718A4
	private void LoadHotseatPrefs()
	{
		if (PlayerPrefs.GetInt("HotseatPlayersAmount4", 0) == 0)
		{
			return;
		}
		this.TurnOffPlayers();
		this.promoCardsToggle.isOn = PlayerPrefs.GetInt("HotseatPromoCards", 1) == 1;
		this.matBalanceToggle.isOn = PlayerPrefs.GetInt("HotseatBalance", 1) == 1;
		List<List<int>> list = new List<List<int>>
		{
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>()
		};
		List<List<int>> list2 = new List<List<int>>
		{
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>()
		};
		for (int i = 0; i < 5; i++)
		{
			this.LoadHotseatPlayerPrefs(i, list, list2);
		}
		this.CheckHotseatPlayers(list, list2);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x000737D0 File Offset: 0x000719D0
	private void TurnOffPlayers()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				this.hotseatPlayerToggle[i, j].isOn = false;
			}
			this.UpdateEntryState(i, false);
		}
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00073810 File Offset: 0x00071A10
	private void LoadHotseatPlayerPrefs(int index, List<List<int>> factionMatsAndTheirOwners, List<List<int>> playerMatsAndTheirOwners)
	{
		if (PlayerPrefs.GetString("HotseatPlayerName4" + index.ToString(), string.Empty) == string.Empty)
		{
			return;
		}
		this.LoadPlayerType(index);
		this.LoadPlayerName(index);
		factionMatsAndTheirOwners[this.LoadPlayerFaction(index)].Add(index);
		playerMatsAndTheirOwners[this.LoadPlayerMat(index)].Add(index);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0007387C File Offset: 0x00071A7C
	private void LoadPlayerName(int index)
	{
		if (index == 0 && PlatformManager.IsSteam && CoreApplication.Instance.OAuthGate.SteamManager != null && CoreApplication.Instance.OAuthGate.SteamManager.HasClient)
		{
			this.hotseatPlayerName[index].text = AsmodeeLogic.Instance.GetPlayerName();
		}
		else if (this.hotseatPlayerToggle[index, 0].isOn)
		{
			this.hotseatPlayerName[index].text = PlayerPrefs.GetString("HotseatPlayerName4" + index.ToString(), this.GenerateHumanPlayerName(index));
		}
		else if (this.hotseatPlayerToggle[index, 1].isOn)
		{
			this.hotseatPlayerName[index].text = HotseatPanel.GenerateAIPlayerName(index, 1);
		}
		else if (this.hotseatPlayerToggle[index, 2].isOn)
		{
			this.hotseatPlayerName[index].text = HotseatPanel.GenerateAIPlayerName(index, 2);
		}
		else if (this.hotseatPlayerToggle[index, 3].isOn)
		{
			this.hotseatPlayerName[index].text = HotseatPanel.GenerateAIPlayerName(index, 3);
		}
		this.CheckPlayerName(index);
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0007399C File Offset: 0x00071B9C
	private void LoadPlayerType(int index)
	{
		int num = PlayerPrefs.GetInt("HotseatPlayerType4" + index.ToString(), 2);
		if (index == 0)
		{
			num = 0;
		}
		this.hotseatPlayerToggle[index, 0].group.allowSwitchOff = true;
		for (int i = 0; i < 4; i++)
		{
			this.hotseatPlayerToggle[index, i].isOn = i == num;
		}
		if (index < 2)
		{
			this.hotseatPlayerToggle[index, 0].group.allowSwitchOff = false;
		}
		this.UpdateEntryState(index, true);
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00073A24 File Offset: 0x00071C24
	private int LoadPlayerFaction(int index)
	{
		int @int = PlayerPrefs.GetInt("HotseatPlayerFaction" + index.ToString(), 7);
		if (@int != 7)
		{
			this.factionDetailedSelectionWindow.UpdateSelectedFactions(@int, true);
		}
		this.UpdateFaction(index, @int);
		return @int;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00073A64 File Offset: 0x00071C64
	private int LoadPlayerMat(int index)
	{
		int @int = PlayerPrefs.GetInt("HotseatPlayerMat" + index.ToString(), 7);
		if (@int != 7)
		{
			this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(@int, true);
		}
		this.UpdatePlayerMat(index, @int);
		return @int;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00073AA4 File Offset: 0x00071CA4
	private void CheckHotseatPlayers(List<List<int>> factionMatsAndTheirOwners, List<List<int>> playerMatsAndTheirOwners)
	{
		for (int i = 0; i < factionMatsAndTheirOwners.Count - 1; i++)
		{
			if (factionMatsAndTheirOwners[i].Count > 1)
			{
				for (int j = 0; j < factionMatsAndTheirOwners[i].Count; j++)
				{
					this.UpdateFaction(j, 7);
				}
			}
		}
		for (int k = 0; k < playerMatsAndTheirOwners.Count - 1; k++)
		{
			if (playerMatsAndTheirOwners[k].Count > 1)
			{
				for (int l = 0; l < playerMatsAndTheirOwners[k].Count; l++)
				{
					this.UpdatePlayerMat(l, 7);
				}
			}
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00073B34 File Offset: 0x00071D34
	private void InitializeHotseatControls()
	{
		int num = 0;
		foreach (object obj in this.hotseatPanel)
		{
			Transform transform = (Transform)obj;
			int slotId = num;
			this.hotseatPlayerName[num] = transform.GetChild(0).GetChild(0).GetComponent<InputField>();
			this.hotseatPlayerName[num].onEndEdit.AddListener(delegate
			{
				this.CheckPlayerName(slotId);
			});
			for (int i = 0; i < 4; i++)
			{
				this.hotseatPlayerToggle[num, i] = transform.GetChild(2).GetChild(i).GetComponent<Toggle>();
			}
			this.hotseatSelectedFactionIcons[num] = transform.GetChild(3).GetChild(2).GetChild(0)
				.GetComponent<Image>();
			this.hotseatSelectedFactionIcons[num].sprite = this.factionIcons[7];
			this.hotseatSelectedPlayerMatText[num] = transform.GetChild(4).GetChild(2).GetChild(0)
				.GetComponent<TextMeshProUGUI>();
			this.hotseatSelectedPlayerMatText[num].text = ScriptLocalization.Get("GameScene/Random");
			if (num >= 3)
			{
				this.UpdateEntryState(num, false);
			}
			num++;
		}
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00073C98 File Offset: 0x00071E98
	private void UpdateTexts()
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.hotseatPlayerToggle[i, 1].isOn)
			{
				this.hotseatPlayerName[i].text = HotseatPanel.GenerateAIPlayerName(i, 1);
			}
			else if (this.hotseatPlayerToggle[i, 2].isOn)
			{
				this.hotseatPlayerName[i].text = HotseatPanel.GenerateAIPlayerName(i, 2);
			}
			else if (this.hotseatPlayerToggle[i, 3].isOn)
			{
				this.hotseatPlayerName[i].text = HotseatPanel.GenerateAIPlayerName(i, 3);
			}
			this.UpdatePlayerMatSelectionText(i);
		}
		for (int j = 0; j < this.undoSelection.options.Count; j++)
		{
			this.undoSelection.options[j].text = ScriptLocalization.Get("Common/Undo" + Enum.GetName(typeof(UndoTypes), (UndoTypes)j));
		}
		this.undoSelection.RefreshShownValue();
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0002C977 File Offset: 0x0002AB77
	public void OnPreviousFactionButtonClicked(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdateFaction(id, this.factionDetailedSelectionWindow.ChangeFactionSelection(id, this.hotseatPlayerFaction[id], false));
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002C9A1 File Offset: 0x0002ABA1
	public void OnNextFactionButtonClicked(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdateFaction(id, this.factionDetailedSelectionWindow.ChangeFactionSelection(id, this.hotseatPlayerFaction[id], true));
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002C9CB File Offset: 0x0002ABCB
	public void OnFactionDetailsClicked(int id)
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_faction_details);
		this.factionDetailedSelectionWindow.EnableDetailedFactionSelection(id, this.hotseatPlayerFaction[id]);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00073D9C File Offset: 0x00071F9C
	private void UpdateFaction(int id, int faction)
	{
		if ((faction == 1 || faction == 4) && !GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			faction = 7;
		}
		this.hotseatPlayerFaction[id] = faction;
		this.hotseatSelectedFactionIcons[id].sprite = this.factionIcons[this.hotseatPlayerFaction[id]];
		this.OnBalanceCheckboxValueChanged(this.matBalanceToggle.isOn);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00073DF8 File Offset: 0x00071FF8
	public void UpdateFactionSelections()
	{
		int playerId = this.factionDetailedSelectionWindow.GetPlayerId();
		this.hotseatPlayerFaction[playerId] = this.factionDetailedSelectionWindow.GetCurrentFaction();
		this.hotseatSelectedFactionIcons[playerId].sprite = this.factionIcons[this.hotseatPlayerFaction[playerId]];
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002C9E8 File Offset: 0x0002ABE8
	public void OnPreviousPlayerMatButtonClicked(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMat(id, this.playerMatDetailedSelectionWindow.ChangePlayerMat(id, this.hotseatPlayerMat[id], false));
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002CA12 File Offset: 0x0002AC12
	public void OnNextPlayerMatButtonClicked(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMat(id, this.playerMatDetailedSelectionWindow.ChangePlayerMat(id, this.hotseatPlayerMat[id], true));
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002CA3C File Offset: 0x0002AC3C
	public void OnPlayerMatDetailsClicked(int id)
	{
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_player_mat_details);
		this.playerMatDetailedSelectionWindow.EnableDetailedPlayerMatSelection(id, this.hotseatPlayerMat[id]);
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00073E4C File Offset: 0x0007204C
	private void UpdatePlayerMat(int id, int playerMat)
	{
		if ((playerMat == 5 || playerMat == 6) && !GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			playerMat = 7;
		}
		this.hotseatPlayerMat[id] = playerMat;
		this.hotseatSelectedPlayerMatText[id].text = ((this.hotseatPlayerMat[id] != 7) ? ScriptLocalization.Get("PlayerMat/" + Enum.GetName(typeof(PlayerMatType), this.hotseatPlayerMat[id])) : ScriptLocalization.Get("GameScene/Random"));
		this.OnBalanceCheckboxValueChanged(this.matBalanceToggle.isOn);
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002CA59 File Offset: 0x0002AC59
	private bool UnbalancedFactionAndPlayerMatCombination(int slotId)
	{
		return this.matBalanceToggle.isOn && (this.UnbalancedFactionAndMat(this.hotseatPlayerFaction[slotId], this.hotseatPlayerMat[slotId]) || this.TheLastAndOnlySlotWithRandomValues(slotId));
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002CA8B File Offset: 0x0002AC8B
	private bool UnbalancedFactionAndMat(int faction, int matType)
	{
		return faction != 7 && matType != 7 && MatAndFactionSelection.FactionIsOverpoweredWithPlayerMat((Faction)faction, (PlayerMatType)matType);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002CA9E File Offset: 0x0002AC9E
	private bool TheLastAndOnlySlotWithRandomValues(int slotId)
	{
		return this.HotseatPlayerToggleCount(slotId) > 0 && (this.FullRandom(slotId) || this.RusvietFactionSelected(slotId) || this.IndustrialPlayerMatSelected(slotId));
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00073EDC File Offset: 0x000720DC
	private bool FullRandom(int slotId)
	{
		return this.hotseatPlayerMat[slotId] == 7 && this.hotseatPlayerFaction[slotId] == 7 && ((this.factionDetailedSelectionWindow.IsLastAvaliableFaction(3) && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(0)) || (this.factionDetailedSelectionWindow.IsLastAvaliableFaction(5) && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(2)));
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00073F3C File Offset: 0x0007213C
	private bool RusvietFactionSelected(int slotId)
	{
		return this.hotseatPlayerMat[slotId] == 7 && ((this.hotseatPlayerFaction[slotId] == 3 && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(0)) || (this.hotseatPlayerFaction[slotId] == 5 && this.playerMatDetailedSelectionWindow.IsLastAvaliablePlayerMat(2)));
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00073F8C File Offset: 0x0007218C
	private bool IndustrialPlayerMatSelected(int slotId)
	{
		return this.hotseatPlayerFaction[slotId] == 7 && ((this.hotseatPlayerMat[slotId] == 0 && this.factionDetailedSelectionWindow.IsLastAvaliableFaction(3)) || (this.hotseatPlayerMat[slotId] == 2 && this.factionDetailedSelectionWindow.IsLastAvaliableFaction(5)));
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00073FDC File Offset: 0x000721DC
	public void UpdatePlayerMatSelections()
	{
		int playerId = this.playerMatDetailedSelectionWindow.GetPlayerId();
		this.hotseatPlayerMat[playerId] = this.playerMatDetailedSelectionWindow.GetCurrentPlayerMat();
		this.UpdatePlayerMatSelectionText(playerId);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0007401C File Offset: 0x0007221C
	private void UpdatePlayerMatSelectionText(int id)
	{
		if (this.hotseatPlayerMat[id] == 7)
		{
			this.hotseatSelectedPlayerMatText[id].text = ScriptLocalization.Get("GameScene/Random");
			return;
		}
		this.hotseatSelectedPlayerMatText[id].text = ScriptLocalization.Get("PlayerMat/" + Enum.GetName(typeof(PlayerMatType), (PlayerMatType)this.hotseatPlayerMat[id]));
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00074084 File Offset: 0x00072284
	public List<MatAndFactionSelection.PlayerEntry> GetPlayersAndTheirSettings()
	{
		List<MatAndFactionSelection.PlayerEntry> list = new List<MatAndFactionSelection.PlayerEntry>();
		bool flag = GameServiceController.Instance.InvadersFromAfarUnlocked();
		for (int i = 0; i < this.hotseatPlayerName.Length; i++)
		{
			this.CheckPlayerName(i);
			for (int j = 0; j < 4; j++)
			{
				if (this.hotseatPlayerToggle[i, j].isOn)
				{
					list.Add(new MatAndFactionSelection.PlayerEntry(this.hotseatPlayerName[i].text.Replace("\r", "").Replace("\n", ""), j, flag, this.hotseatPlayerFaction[i], this.hotseatPlayerMat[i], default(Guid)));
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00074134 File Offset: 0x00072334
	public void CheckPlayerName(int id)
	{
		if ((this.hotseatPlayerToggle[id, 0].isOn || this.hotseatPlayerToggle[id, 1].isOn || this.hotseatPlayerToggle[id, 2].isOn || this.hotseatPlayerToggle[id, 3].isOn) && this.hotseatPlayerName[id].text == "")
		{
			if (this.hotseatPlayerToggle[id, 0].isOn)
			{
				this.hotseatPlayerName[id].text = this.GenerateHumanPlayerName(id);
				return;
			}
			if (this.hotseatPlayerToggle[id, 1].isOn)
			{
				this.hotseatPlayerName[id].text = HotseatPanel.GenerateAIPlayerName(id, 1);
				return;
			}
			if (this.hotseatPlayerToggle[id, 2].isOn)
			{
				this.hotseatPlayerName[id].text = HotseatPanel.GenerateAIPlayerName(id, 2);
				return;
			}
			if (this.hotseatPlayerToggle[id, 3].isOn)
			{
				this.hotseatPlayerName[id].text = HotseatPanel.GenerateAIPlayerName(id, 3);
			}
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00074254 File Offset: 0x00072454
	public bool GenericNameOnEntry(int id)
	{
		string text = this.hotseatPlayerName[id].text;
		return text == "" || text == this.GenerateHumanPlayerName(id) || text == HotseatPanel.GenerateAIPlayerName(id, 1) || text == HotseatPanel.GenerateAIPlayerName(id, 2) || text == HotseatPanel.GenerateAIPlayerName(id, 3);
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000742B8 File Offset: 0x000724B8
	public void HotseatPlayerToggleHuman(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		if (this.GenericNameOnEntry(id) && this.hotseatPlayerToggle[id, 0].isOn)
		{
			this.hotseatPlayerName[id].text = this.GenerateHumanPlayerName(id);
		}
		if (!this.hotseatPlayerName[id].interactable)
		{
			this.UpdateEntryState(id, true);
		}
		else
		{
			this.UpdateEntryState(id, this.HotseatPlayerToggleCount(id) > 0);
		}
		this.OnBalanceCheckboxValueChanged(this.matBalanceToggle.isOn);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00074340 File Offset: 0x00072540
	private string GenerateHumanPlayerName(int id)
	{
		return ScriptLocalization.Get("Common/Player") + " " + (id + 1).ToString();
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0007436C File Offset: 0x0007256C
	private int GetPlayerToggleState(int id)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.hotseatPlayerToggle[id, i].isOn)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0007439C File Offset: 0x0007259C
	public void HotseatPlayerToggleAI(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		if (this.GenericNameOnEntry(id) && (this.hotseatPlayerToggle[id, 1].isOn || this.hotseatPlayerToggle[id, 2].isOn || this.hotseatPlayerToggle[id, 3].isOn))
		{
			this.hotseatPlayerName[id].text = HotseatPanel.GenerateAIPlayerName(id, this.GetPlayerToggleState(id));
		}
		if (!this.hotseatPlayerName[id].interactable)
		{
			this.UpdateEntryState(id, true);
		}
		else
		{
			this.UpdateEntryState(id, this.HotseatPlayerToggleCount(id) > 0);
		}
		this.OnBalanceCheckboxValueChanged(this.matBalanceToggle.isOn);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00074450 File Offset: 0x00072650
	public void HotseatPlayerAddRemovePlayer(int id)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		if (this.hotseatPanel.GetChild(id).gameObject.GetComponent<ToggleGroup>().ActiveToggles().Count<Toggle>() > 0)
		{
			using (IEnumerator<Toggle> enumerator = this.hotseatPanel.GetChild(id).gameObject.GetComponent<ToggleGroup>().ActiveToggles().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Toggle toggle = enumerator.Current;
					toggle.isOn = false;
				}
				return;
			}
		}
		this.hotseatPlayerToggle[id, 2].isOn = true;
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x000744F4 File Offset: 0x000726F4
	public static string GenerateAIPlayerName(int id, int difficulty)
	{
		string text = ScriptLocalization.Get("Common/Bot") + id.ToString();
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

	// Token: 0x06000742 RID: 1858 RVA: 0x00074574 File Offset: 0x00072774
	private int HotseatPlayerToggleCount(int id)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.hotseatPlayerToggle[id, i] != null && this.hotseatPlayerToggle[id, i].isOn)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x000745C0 File Offset: 0x000727C0
	private void UpdateEntryState(int id, bool state)
	{
		if (!state)
		{
			this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(this.hotseatPlayerMat[id], state);
			this.factionDetailedSelectionWindow.UpdateSelectedFactions(this.hotseatPlayerFaction[id], state);
			this.hotseatPlayerMat[id] = 7;
			this.hotseatPlayerFaction[id] = 7;
			this.hotseatSelectedFactionIcons[id].sprite = this.factionIcons[this.hotseatPlayerFaction[id]];
			this.hotseatSelectedPlayerMatText[id].text = ((this.hotseatPlayerMat[id] != 7) ? Enum.GetName(typeof(PlayerMatType), (PlayerMatType)this.hotseatPlayerMat[id]) : ScriptLocalization.Get("GameScene/Random"));
			this.hotseatPlayerName[id].text = string.Empty;
			this.hotseatSelectedPlayerMatText[id].color = this.matTextColor;
			if (id > 1)
			{
				this.addRemovePlayer[id].transform.GetChild(0).gameObject.SetActive(true);
				this.addRemovePlayer[id].transform.GetChild(1).gameObject.SetActive(false);
			}
		}
		if (id > 1 && state)
		{
			this.addRemovePlayer[id].transform.GetChild(0).gameObject.SetActive(false);
			this.addRemovePlayer[id].transform.GetChild(1).gameObject.SetActive(true);
		}
		this.hotseatPlayerName[id].interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(3).GetChild(0)
			.GetComponent<Button>()
			.interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(3).GetChild(1)
			.GetComponent<Button>()
			.interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(3).GetChild(2)
			.GetComponent<Button>()
			.interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(4).GetChild(0)
			.GetComponent<Button>()
			.interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(4).GetChild(1)
			.GetComponent<Button>()
			.interactable = state;
		this.hotseatPanel.GetChild(id).GetChild(4).GetChild(2)
			.GetComponent<Button>()
			.interactable = state;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x000747F8 File Offset: 0x000729F8
	public void OnBalanceCheckboxValueChanged(bool state)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		this.startButton.interactable = true;
		this.startButtonInfo.gameObject.SetActive(false);
		for (int i = 0; i < 5; i++)
		{
			if (state)
			{
				if (this.UnbalancedFactionAndPlayerMatCombination(i))
				{
					this.hotseatSelectedPlayerMatText[i].color = Color.red;
					this.startButton.interactable = false;
					this.startButtonInfo.gameObject.SetActive(true);
				}
				else if (this.hotseatSelectedPlayerMatText != null)
				{
					this.hotseatSelectedPlayerMatText[i].color = this.matTextColor;
				}
			}
			else if (this.hotseatSelectedPlayerMatText != null)
			{
				this.hotseatSelectedPlayerMatText[i].color = this.matTextColor;
			}
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0002CAC7 File Offset: 0x0002ACC7
	public bool PromoCardsUnlocked()
	{
		return this.promoCardsToggle.isOn;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
	public void PromoCardsClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x000283F8 File Offset: 0x000265F8
	private bool InvadersFromAfarUnlocked()
	{
		return true;
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0002CAE2 File Offset: 0x0002ACE2
	public int GetUndoType()
	{
		return this.undoSelection.value;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0002CAEF File Offset: 0x0002ACEF
	public bool Balanced()
	{
		return this.matBalanceToggle.isOn;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000748C0 File Offset: 0x00072AC0
	public void OnResetMats()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		for (int i = 0; i < 5; i++)
		{
			if (this.HotseatPlayerToggleCount(i) > 0)
			{
				this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(this.hotseatPlayerMat[i], false);
				this.factionDetailedSelectionWindow.UpdateSelectedFactions(this.hotseatPlayerFaction[i], false);
				this.UpdateFaction(i, 7);
				this.UpdatePlayerMat(i, 7);
			}
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00074928 File Offset: 0x00072B28
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
			List<int> list = MatAndFactionSelection.SetAvailableFactions(this.InvadersFromAfarUnlocked());
			List<int> list2 = MatAndFactionSelection.SetAvailablePlayerMats(this.InvadersFromAfarUnlocked());
			num = -1;
			num2 = -1;
			num3 = -1;
			num4 = -1;
			for (int i = 0; i < 5; i++)
			{
				if (this.HotseatPlayerToggleCount(i) > 0)
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
					this.UpdateFaction(i, list[num5]);
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
					this.UpdatePlayerMat(i, list2[num6]);
					this.playerMatDetailedSelectionWindow.UpdateSelectedPlayerMats(list2[num6], true);
					list2.RemoveAt(num6);
				}
			}
		}
		while ((num == num2 || num3 == num4) && this.matBalanceToggle.isOn);
	}

	// Token: 0x0400065E RID: 1630
	public GameObject hotseatWindow;

	// Token: 0x0400065F RID: 1631
	public Transform hotseatPanel;

	// Token: 0x04000660 RID: 1632
	public FactionSelectionWindow factionDetailedSelectionWindow;

	// Token: 0x04000661 RID: 1633
	public PlayerMatSelectionWindow playerMatDetailedSelectionWindow;

	// Token: 0x04000662 RID: 1634
	public Button startButton;

	// Token: 0x04000663 RID: 1635
	public Text startButtonInfo;

	// Token: 0x04000664 RID: 1636
	public Toggle promoCardsToggle;

	// Token: 0x04000665 RID: 1637
	public Toggle matBalanceToggle;

	// Token: 0x04000666 RID: 1638
	public Dropdown undoSelection;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private Button[] addRemovePlayer;

	// Token: 0x04000668 RID: 1640
	private InputField[] hotseatPlayerName = new InputField[5];

	// Token: 0x04000669 RID: 1641
	private Toggle[,] hotseatPlayerToggle = new Toggle[5, 4];

	// Token: 0x0400066A RID: 1642
	public Sprite[] factionIcons = new Sprite[8];

	// Token: 0x0400066B RID: 1643
	private Image[] hotseatSelectedFactionIcons = new Image[5];

	// Token: 0x0400066C RID: 1644
	private TextMeshProUGUI[] hotseatSelectedPlayerMatText = new TextMeshProUGUI[5];

	// Token: 0x0400066D RID: 1645
	public const int MAXIMUM_NUMBER_OF_PLAYERS = 5;

	// Token: 0x0400066E RID: 1646
	public const int NUMBER_OF_FACTIONS = 7;

	// Token: 0x0400066F RID: 1647
	public const int NUMBER_OF_PLAYER_MATS = 7;

	// Token: 0x04000670 RID: 1648
	private int[] hotseatPlayerFaction = Enumerable.Repeat<int>(7, 5).ToArray<int>();

	// Token: 0x04000671 RID: 1649
	private int[] hotseatPlayerMat = Enumerable.Repeat<int>(7, 5).ToArray<int>();

	// Token: 0x04000672 RID: 1650
	private const string HOTSEAT_PLAYERS_AMOUNT = "HotseatPlayersAmount4";

	// Token: 0x04000673 RID: 1651
	private const string HOTSEAT_PLAYER_NAME = "HotseatPlayerName4";

	// Token: 0x04000674 RID: 1652
	private const string HOTSEAT_PLAYER_TYPE = "HotseatPlayerType4";

	// Token: 0x04000675 RID: 1653
	private Color32 matTextColor = new Color32(214, 207, 193, byte.MaxValue);
}
