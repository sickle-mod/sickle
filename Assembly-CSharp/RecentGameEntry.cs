using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004B RID: 75
public class RecentGameEntry : MonoBehaviour
{
	// Token: 0x06000271 RID: 625 RVA: 0x0005C774 File Offset: 0x0005A974
	public void Initialize(PlayerStats playerStats, GameHistoryEntry gameHistoryEntry, PlayerStatsScreen playerStatsScreen)
	{
		this.playerStatsScreen = playerStatsScreen;
		this.factionLogo.sprite = playerStatsScreen.FactionSprites()[gameHistoryEntry.Faction];
		if (gameHistoryEntry.Leaver)
		{
			this.playerPlaceAndPoints.text = ScriptLocalization.Get("Lobby/Abandoned");
			this.playerPlaceAndPoints.color = this.placeLeaver;
			this.usedTime.text = "";
			this.usedTimeLabel.gameObject.SetActive(false);
		}
		else
		{
			this.playerPlaceAndPoints.color = this.placeNormalColor;
			this.playerPlaceAndPoints.text = ScriptLocalization.Get("Statistics/Place");
			TextMeshProUGUI textMeshProUGUI = this.playerPlaceAndPoints;
			textMeshProUGUI.text = textMeshProUGUI.text + "<color=#" + ColorUtility.ToHtmlStringRGBA((gameHistoryEntry.Place == 1) ? this.firstPlaceColor : this.placeNormalColor) + "> ";
			TextMeshProUGUI textMeshProUGUI2 = this.playerPlaceAndPoints;
			textMeshProUGUI2.text = textMeshProUGUI2.text + gameHistoryEntry.Place.ToString() + "/" + gameHistoryEntry.PlayersStats.Count.ToString();
			TextMeshProUGUI textMeshProUGUI3 = this.playerPlaceAndPoints;
			textMeshProUGUI3.text = string.Concat(new string[]
			{
				textMeshProUGUI3.text,
				"</color> (",
				ScriptLocalization.Get("Statistics/Points"),
				": ",
				gameHistoryEntry.Points.ToString(),
				")"
			});
			if (!gameHistoryEntry.Asynchronous)
			{
				this.usedTime.text = string.Concat(new string[]
				{
					(gameHistoryEntry.UsedTime / this.toMinutes).ToString(),
					"/",
					(gameHistoryEntry.PlayerClock / this.toMinutes).ToString(),
					" ",
					ScriptLocalization.Get("Lobby/MinutesAbbreviation")
				});
			}
			else
			{
				this.usedTime.text = string.Concat(new string[]
				{
					(gameHistoryEntry.UsedTime / this.toHours).ToString(),
					"/",
					(gameHistoryEntry.PlayerClock / this.toHours).ToString(),
					" ",
					ScriptLocalization.Get("Lobby/Hours")
				});
			}
		}
		this.playerFactionAndPlayerMat.text = ScriptLocalization.Get("FactionMat/" + ((Faction)gameHistoryEntry.Faction).ToString()) + " (" + ScriptLocalization.Get("PlayerMat/" + ((PlayerMatType)gameHistoryEntry.PlayerMat).ToString()) + ")";
		this.gameMode.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(gameHistoryEntry.Asynchronous ? this.playAndGoColor : this.playAndStayColor) + "> ";
		TextMeshProUGUI textMeshProUGUI4 = this.gameMode;
		textMeshProUGUI4.text += (gameHistoryEntry.Asynchronous ? ScriptLocalization.Get("Lobby/AsynchronousGames") : ScriptLocalization.Get("Lobby/SynchronousGames"));
		this.rankingMode.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(gameHistoryEntry.Ranked ? this.rankedModeColor : this.normalModeColor) + "> ";
		TextMeshProUGUI textMeshProUGUI5 = this.rankingMode;
		textMeshProUGUI5.text = textMeshProUGUI5.text + "(" + (gameHistoryEntry.Ranked ? ScriptLocalization.Get("Lobby/Ranked") : ScriptLocalization.Get("Lobby/Normal")) + ")";
		string text = "";
		this.DLCFactions.SetActive(gameHistoryEntry.IFA);
		if (gameHistoryEntry.IFA)
		{
			text += ScriptLocalization.Get("Lobby/InvadersFromAfarAbbreviation");
		}
		this.usedDLC.text = text;
		List<PlayerEndGameStats> endGameStats = gameHistoryEntry.GetEndGameStats();
		for (int i = 0; i < endGameStats.Count; i++)
		{
			global::UnityEngine.Object.Instantiate<RecentGamePlayerEntry>(this.recentGamePlayerEntryPrefab, this.detailedInfo).Initialize(endGameStats[i], this);
		}
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0002975F File Offset: 0x0002795F
	public Sprite FactionLogo(int i)
	{
		return this.factionLogos[i];
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00029769 File Offset: 0x00027969
	public void OnEndgameStatsButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		if (this.detailedInfo.gameObject.activeSelf)
		{
			this.HidePlayersList();
			return;
		}
		this.ShowPlayersList();
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0005CB70 File Offset: 0x0005AD70
	public void ShowPlayersList()
	{
		this.detailedInfo.gameObject.SetActive(true);
		Vector3 localEulerAngles = this.detailedInfoButton.transform.localEulerAngles;
		localEulerAngles.z = 180f;
		this.detailedInfoButton.transform.localEulerAngles = localEulerAngles;
		this.playerStatsScreen.RedrawList();
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0005CBC8 File Offset: 0x0005ADC8
	public void HidePlayersList()
	{
		this.detailedInfo.gameObject.SetActive(false);
		Vector3 localEulerAngles = this.detailedInfoButton.transform.localEulerAngles;
		localEulerAngles.z = 0f;
		this.detailedInfoButton.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	private Image factionLogo;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private TextMeshProUGUI playerPlaceAndPoints;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private TextMeshProUGUI playerFactionAndPlayerMat;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	private TextMeshProUGUI gameMode;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private TextMeshProUGUI rankingMode;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	private TextMeshProUGUI usedTimeLabel;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	private TextMeshProUGUI usedTime;

	// Token: 0x040001D9 RID: 473
	[SerializeField]
	private TextMeshProUGUI usedDLC;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private GameObject DLCFactions;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private Button detailedInfoButton;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private Transform detailedInfo;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private RecentGamePlayerEntry recentGamePlayerEntryPrefab;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	private Color placeNormalColor = Color.white;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	private Color firstPlaceColor = Color.blue;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	private Color placeLeaver = Color.red;

	// Token: 0x040001E1 RID: 481
	[SerializeField]
	private Color playAndStayColor = Color.green;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	private Color playAndGoColor = Color.yellow;

	// Token: 0x040001E3 RID: 483
	[SerializeField]
	private Color normalModeColor = Color.white;

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	private Color rankedModeColor = Color.blue;

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	private Sprite[] factionLogos;

	// Token: 0x040001E6 RID: 486
	private PlayerStatsScreen playerStatsScreen;

	// Token: 0x040001E7 RID: 487
	private int toMinutes = 60;

	// Token: 0x040001E8 RID: 488
	private int toHours = 3600;
}
