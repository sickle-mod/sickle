using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003C RID: 60
public class SelectFactionAndMatGameRoomSlotState : GameRoomSlotState
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060001CD RID: 461 RVA: 0x00028FE2 File Offset: 0x000271E2
	// (set) Token: 0x060001CE RID: 462 RVA: 0x00028FEA File Offset: 0x000271EA
	public int SelectedFactionID { get; private set; }

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060001CF RID: 463 RVA: 0x00028FF3 File Offset: 0x000271F3
	// (set) Token: 0x060001D0 RID: 464 RVA: 0x00028FFB File Offset: 0x000271FB
	public int SelectedMatID { get; private set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060001D1 RID: 465 RVA: 0x00029004 File Offset: 0x00027204
	// (set) Token: 0x060001D2 RID: 466 RVA: 0x0002900C File Offset: 0x0002720C
	public bool IsSelected { get; set; }

	// Token: 0x060001D3 RID: 467 RVA: 0x00029015 File Offset: 0x00027215
	public void Reset()
	{
		this.SetPlayerFaction(-1);
		this.SetPlayerMat(-1);
		this.IsSelected = false;
		this.SetActiveGoldFrame(false);
		this.SetActiveTimer(false);
		this.SetValueTimer(0);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00029041 File Offset: 0x00027241
	public void SetPlayerName(string name)
	{
		if (this.playerNameLabel)
		{
			this.playerNameLabel.text = name;
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00059F5C File Offset: 0x0005815C
	public void SetPlayerMat(int matID)
	{
		this.SelectedMatID = matID;
		if (matID == -1)
		{
			this.SetPlayerMatName(ScriptLocalization.Get("GameScene/Random"));
			return;
		}
		string text = "PlayerMat/";
		PlayerMatType playerMatType = (PlayerMatType)matID;
		this.SetPlayerMatName(ScriptLocalization.Get(text + playerMatType.ToString()));
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00059FAC File Offset: 0x000581AC
	public void SetPlayerFaction(int factionID)
	{
		this.SelectedFactionID = factionID;
		if (this.factionEmblems != null)
		{
			if (factionID >= 0 && factionID < this.factionEmblems.Count)
			{
				this.SetPlayerFactionImage(this.factionEmblems[factionID]);
			}
			else
			{
				this.SetPlayerFactionImage(null);
			}
		}
		if (PlayerInfo.me.CurrentLobbyRoom.Players > 5)
		{
			this.abilityChangeInfo.Activate(factionID);
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0002905C File Offset: 0x0002725C
	public void SetActiveGoldFrame(bool value)
	{
		if (this.goldFrame)
		{
			this.goldFrame.SetActive(value);
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00029077 File Offset: 0x00027277
	public void SetActiveTimer(bool value)
	{
		if (this.timerLabel)
		{
			this.timerLabel.gameObject.SetActive(value);
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00029097 File Offset: 0x00027297
	public void SetValueTimer(int timerValue)
	{
		if (this.timerLabel)
		{
			this.timerLabel.text = timerValue.ToString();
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x000290B8 File Offset: 0x000272B8
	private void SetPlayerMatName(string matName)
	{
		if (this.playerMatNameLabel)
		{
			if (string.IsNullOrEmpty(matName))
			{
				this.playerMatNameLabel.text = ScriptLocalization.Get("GameScene/Random").ToUpper();
				return;
			}
			this.playerMatNameLabel.text = matName;
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0005A014 File Offset: 0x00058214
	private void SetPlayerFactionImage(Sprite sprite)
	{
		if (this.playerFactionImage)
		{
			if (sprite == null)
			{
				this.playerFactionImage.gameObject.SetActive(false);
				return;
			}
			this.playerFactionImage.gameObject.SetActive(true);
			this.playerFactionImage.sprite = sprite;
		}
	}

	// Token: 0x0400016B RID: 363
	[SerializeField]
	private List<Sprite> factionEmblems;

	// Token: 0x0400016C RID: 364
	[SerializeField]
	private TextMeshProUGUI playerNameLabel;

	// Token: 0x0400016D RID: 365
	[SerializeField]
	private TextMeshProUGUI playerMatNameLabel;

	// Token: 0x0400016E RID: 366
	[SerializeField]
	private Image playerFactionImage;

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private GameObject goldFrame;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private TextMeshProUGUI timerLabel;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	private HotseatAbilityChangeInfo abilityChangeInfo;
}
