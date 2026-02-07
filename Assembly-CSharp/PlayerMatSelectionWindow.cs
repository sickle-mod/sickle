using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FE RID: 254
public class PlayerMatSelectionWindow : MonoBehaviour
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000845 RID: 2117 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
	private int NUMBER_OF_PLAYER_MATS
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0002D918 File Offset: 0x0002BB18
	private void Start()
	{
		this.InitPlayerMatsList();
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00077EF0 File Offset: 0x000760F0
	private void InitPlayerMatsList()
	{
		this.playerMats = new List<MatPlayer>();
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Industrial));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Engineering));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Patriotic));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Mechanical));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Agricultural));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Militant));
		this.playerMats.Add(new MatPlayer(null, PlayerMatType.Innovative));
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0002D920 File Offset: 0x0002BB20
	private void SetNumberOfPlayerMats()
	{
		this.currentPlayerMat = this.NUMBER_OF_PLAYER_MATS;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0002C795 File Offset: 0x0002A995
	private bool InvadersFromAfarUnlocked()
	{
		return GameServiceController.Instance.InvadersFromAfarUnlocked();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0002D92E File Offset: 0x0002BB2E
	public void EnableIFASelection(bool active)
	{
		if (PlatformManager.IsMobile && this.InvadersFromAfarUnlocked())
		{
			active = true;
		}
		this.IFASelectionUnlocked = active;
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0002D949 File Offset: 0x0002BB49
	public void OnNextMatClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMatSelection(true);
		this.UpdatePlayerMatControls();
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0002D964 File Offset: 0x0002BB64
	public void OnPreviousMatClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdatePlayerMatSelection(false);
		this.UpdatePlayerMatControls();
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0002D97F File Offset: 0x0002BB7F
	public void OnAcceptClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_accept_button);
		this.CloseWindow(false);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0002D99A File Offset: 0x0002BB9A
	public void OnXButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		this.CloseWindow(true);
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00077F88 File Offset: 0x00076188
	public int ChangePlayerMat(int playerId, int currentPlayerMat, bool next)
	{
		this.playerId = playerId;
		this.previouslySelectedPlayerMat = currentPlayerMat;
		this.currentPlayerMat = currentPlayerMat;
		this.UpdatePlayerMatSelection(next);
		return this.currentPlayerMat;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00077FBC File Offset: 0x000761BC
	public void EnableDetailedPlayerMatSelection(int playerId, int currentPlayerMat)
	{
		this.playerId = playerId;
		this.previouslySelectedPlayerMat = currentPlayerMat;
		this.currentPlayerMat = currentPlayerMat;
		this.UpdatePlayerMatControls();
		base.gameObject.SetActive(true);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.player_mat_selection_window, Contexts.outgame);
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00078000 File Offset: 0x00076200
	private void UpdatePlayerMatSelection(bool next)
	{
		if (this.currentPlayerMat != this.NUMBER_OF_PLAYER_MATS)
		{
			this.UpdateBitInValue(ref this.selectedPlayerMats, this.currentPlayerMat, false);
		}
		do
		{
			if (this.currentPlayerMat == this.NUMBER_OF_PLAYER_MATS && next)
			{
				this.currentPlayerMat = 0;
			}
			else if (this.currentPlayerMat == 0 && !next)
			{
				this.currentPlayerMat = this.NUMBER_OF_PLAYER_MATS;
			}
			else
			{
				this.currentPlayerMat += (next ? 1 : (-1));
				if (this.currentPlayerMat != this.NUMBER_OF_PLAYER_MATS && (this.currentPlayerMat == 5 || this.currentPlayerMat == 6) && (!this.InvadersFromAfarUnlocked() || !this.IFASelectionUnlocked))
				{
					this.currentPlayerMat += (next ? 2 : (-2));
				}
			}
			if (this.currentPlayerMat == this.NUMBER_OF_PLAYER_MATS)
			{
				return;
			}
		}
		while (this.ValueBitSet(this.selectedPlayerMats, this.currentPlayerMat));
		this.UpdateBitInValue(ref this.selectedPlayerMats, this.currentPlayerMat, true);
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000780F4 File Offset: 0x000762F4
	private void UpdatePlayerMatControls()
	{
		AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_backgrounds");
		this.background.sprite = assetBundle.LoadAsset<Sprite>(this.playerMatSpriteNames[this.currentPlayerMat]);
		if (this.currentPlayerMat != this.NUMBER_OF_PLAYER_MATS)
		{
			this.playerMatPresenter.gameObject.SetActive(true);
			this.SetActiveRandomPlayerMat(false);
			if (this.playerMats == null)
			{
				this.InitPlayerMatsList();
			}
			this.playerMatName.text = ScriptLocalization.Get("PlayerMat/" + Enum.GetName(typeof(PlayerMatType), this.playerMats[this.currentPlayerMat].matType));
			this.playerMatPresenter.UpdateMat(this.playerMats[this.currentPlayerMat]);
			this.priorityValue.text = this.GetMatPriority(this.playerMats[this.currentPlayerMat].matType);
			this.popularityBonus.text = this.playerMats[this.currentPlayerMat].StartingPopularity.ToString();
			this.coinBonus.text = this.playerMats[this.currentPlayerMat].StartingCoins.ToString();
			return;
		}
		this.SetActiveRandomPlayerMat(true);
		this.playerMatPresenter.gameObject.SetActive(false);
		this.playerMatName.text = ScriptLocalization.Get("GameScene/Random");
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0002D9B5 File Offset: 0x0002BBB5
	private void SetActiveRandomPlayerMat(bool state)
	{
		if (PlatformManager.IsMobile && GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			this.randomPlayerMatIFA.SetActive(state);
			return;
		}
		this.randomPlayerMat.SetActive(state);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00078268 File Offset: 0x00076468
	private string GetMatPriority(PlayerMatType playerMatType)
	{
		switch (playerMatType)
		{
		case PlayerMatType.Industrial:
			return "1";
		case PlayerMatType.Engineering:
			return "2";
		case PlayerMatType.Patriotic:
			return "3";
		case PlayerMatType.Mechanical:
			return "4";
		case PlayerMatType.Agricultural:
			return "5";
		case PlayerMatType.Militant:
			return "2A";
		case PlayerMatType.Innovative:
			return "3A";
		default:
			return "0";
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0002D9E3 File Offset: 0x0002BBE3
	public void UpdateSelectedPlayerMats(int playerMat, bool state)
	{
		if (playerMat == this.NUMBER_OF_PLAYER_MATS)
		{
			return;
		}
		this.UpdateBitInValue(ref this.selectedPlayerMats, playerMat, state);
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002C74E File Offset: 0x0002A94E
	private void UpdateBitInValue(ref int value, int position, bool set)
	{
		if (set)
		{
			value |= 1 << position;
			return;
		}
		value &= ~(1 << position);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002C76B File Offset: 0x0002A96B
	private bool ValueBitSet(int value, int position)
	{
		return (1 & (value >> position)) == 1;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002D9FD File Offset: 0x0002BBFD
	public bool IsPlayerMatAvaliable(int playerMat)
	{
		return playerMat == this.NUMBER_OF_PLAYER_MATS || !this.ValueBitSet(this.selectedPlayerMats, playerMat);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0002DA1A File Offset: 0x0002BC1A
	public int GetPlayerId()
	{
		return this.playerId;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0002DA22 File Offset: 0x0002BC22
	public int GetSelectedPlayerMats()
	{
		return this.selectedPlayerMats;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0002DA2A File Offset: 0x0002BC2A
	public int GetCurrentPlayerMat()
	{
		return this.currentPlayerMat;
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0002DA32 File Offset: 0x0002BC32
	public void SaveSelectedMats()
	{
		PlayerPrefs.SetInt("HotseatSelectedPlayerMats", this.selectedPlayerMats);
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0002DA44 File Offset: 0x0002BC44
	public void LoadSelectedMats()
	{
		this.selectedPlayerMats = PlayerPrefs.GetInt("HotseatSelectedPlayerMats", this.selectedPlayerMats);
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000782C8 File Offset: 0x000764C8
	public bool IsLastAvaliablePlayerMat(int playerMat)
	{
		int num = (1 << this.NUMBER_OF_PLAYER_MATS) - 1;
		num &= ~(1 << playerMat);
		if (!GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.IFASelectionUnlocked)
		{
			num &= -33;
			num &= -65;
		}
		return this.selectedPlayerMats == num;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00078314 File Offset: 0x00076514
	public void CloseWindow(bool revertChanges = true)
	{
		if (this.currentPlayerMat != this.previouslySelectedPlayerMat && revertChanges)
		{
			this.UpdateSelectedPlayerMats(this.currentPlayerMat, !revertChanges);
			this.UpdateSelectedPlayerMats(this.previouslySelectedPlayerMat, revertChanges);
			this.currentPlayerMat = this.previouslySelectedPlayerMat;
			this.UpdatePlayerMatControls();
		}
		base.gameObject.SetActive(false);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0002DA5C File Offset: 0x0002BC5C
	public void ResetPlayerMats()
	{
		this.currentPlayerMat = 7;
		this.selectedPlayerMats = 0;
		this.previouslySelectedPlayerMat = 0;
	}

	// Token: 0x04000701 RID: 1793
	public MatPlayerPresenter playerMatPresenter;

	// Token: 0x04000702 RID: 1794
	public GameObject randomPlayerMat;

	// Token: 0x04000703 RID: 1795
	public GameObject randomPlayerMatIFA;

	// Token: 0x04000704 RID: 1796
	public Image background;

	// Token: 0x04000705 RID: 1797
	public TextMeshProUGUI playerMatName;

	// Token: 0x04000706 RID: 1798
	public TextMeshProUGUI priorityValue;

	// Token: 0x04000707 RID: 1799
	public TextMeshProUGUI popularityBonus;

	// Token: 0x04000708 RID: 1800
	public TextMeshProUGUI coinBonus;

	// Token: 0x04000709 RID: 1801
	public string[] playerMatSpriteNames;

	// Token: 0x0400070A RID: 1802
	private List<MatPlayer> playerMats;

	// Token: 0x0400070B RID: 1803
	private int currentPlayerMat = 7;

	// Token: 0x0400070C RID: 1804
	private int selectedPlayerMats;

	// Token: 0x0400070D RID: 1805
	private int previouslySelectedPlayerMat;

	// Token: 0x0400070E RID: 1806
	private int playerId;

	// Token: 0x0400070F RID: 1807
	private bool IFASelectionUnlocked;
}
