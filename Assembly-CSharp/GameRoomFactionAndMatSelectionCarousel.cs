using System;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002E RID: 46
public class GameRoomFactionAndMatSelectionCarousel : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000F6 RID: 246 RVA: 0x00056208 File Offset: 0x00054408
	// (remove) Token: 0x060000F7 RID: 247 RVA: 0x00056240 File Offset: 0x00054440
	public event Action OnFactionChangeNext;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060000F8 RID: 248 RVA: 0x00056278 File Offset: 0x00054478
	// (remove) Token: 0x060000F9 RID: 249 RVA: 0x000562B0 File Offset: 0x000544B0
	public event Action OnFactionChangePrevious;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060000FA RID: 250 RVA: 0x000562E8 File Offset: 0x000544E8
	// (remove) Token: 0x060000FB RID: 251 RVA: 0x00056320 File Offset: 0x00054520
	public event Action OnMatChangeNext;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060000FC RID: 252 RVA: 0x00056358 File Offset: 0x00054558
	// (remove) Token: 0x060000FD RID: 253 RVA: 0x00056390 File Offset: 0x00054590
	public event Action OnMatChangePrevious;

	// Token: 0x060000FE RID: 254 RVA: 0x00028913 File Offset: 0x00026B13
	public void Reset()
	{
		this.SetPlayerType(GameRoomFactionAndMatSelectionCarousel.PlayerType.Player);
		this.SetPlayerName("");
		this.SetActiveTimer(false);
		this.SetInvadersMatsUnlocked(false);
		this.SetFaction(-1);
		this.SetFactionInteractable(false);
		this.SetMat(-1);
		this.SetMatInteractable(false);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000563C8 File Offset: 0x000545C8
	public void SetPlayerType(GameRoomFactionAndMatSelectionCarousel.PlayerType playerType)
	{
		switch (playerType)
		{
		case GameRoomFactionAndMatSelectionCarousel.PlayerType.Player:
			this.playerTypeImage.sprite = this.playerIcon;
			return;
		case GameRoomFactionAndMatSelectionCarousel.PlayerType.BotEasy:
			this.playerTypeImage.sprite = this.botsIcons[0];
			return;
		case GameRoomFactionAndMatSelectionCarousel.PlayerType.BotMedium:
			this.playerTypeImage.sprite = this.botsIcons[1];
			return;
		case GameRoomFactionAndMatSelectionCarousel.PlayerType.BotHard:
			this.playerTypeImage.sprite = this.botsIcons[2];
			return;
		default:
			return;
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00028951 File Offset: 0x00026B51
	public void SetPlayerName(string newName)
	{
		this.playerNameLabel.text = newName;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0002895F File Offset: 0x00026B5F
	public void SetActiveTimer(bool active)
	{
		this.timerObject.SetActive(active);
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0002896D File Offset: 0x00026B6D
	public void SetValueTimer(int timerValue)
	{
		this.timerLabel.text = timerValue.ToString();
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00028981 File Offset: 0x00026B81
	public void SetInvadersMatsUnlocked(bool invadersMatsUnlocked)
	{
		this.invadersMatsUnlocked = invadersMatsUnlocked;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0005643C File Offset: 0x0005463C
	public void SetFaction(int factionId)
	{
		if (factionId == -1)
		{
			if (this.invadersMatsUnlocked)
			{
				this.factionImage.sprite = this.factionImages[8];
			}
			else
			{
				this.factionImage.sprite = this.factionImages[7];
			}
			this.factionEmblem.sprite = this.factionEmblems[7];
			this.factionName.text = ScriptLocalization.Get("GameScene/Random");
			return;
		}
		this.factionImage.sprite = this.factionImages[factionId];
		this.factionEmblem.sprite = this.factionEmblems[factionId];
		TMP_Text tmp_Text = this.factionName;
		string text = "FactionMat/";
		Faction faction = (Faction)factionId;
		tmp_Text.text = ScriptLocalization.Get(text + faction.ToString());
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0002898A File Offset: 0x00026B8A
	public void SetFactionInteractable(bool interactable)
	{
		this.factionButton.interactable = interactable;
		this.leftFactionArrow.SetActive(interactable);
		this.rightFactionArrow.SetActive(interactable);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000564F8 File Offset: 0x000546F8
	public void SetMat(int matId)
	{
		if (matId == -1)
		{
			this.matLabel.text = ScriptLocalization.Get("GameScene/Random");
			return;
		}
		TMP_Text tmp_Text = this.matLabel;
		string text = "PlayerMat/";
		PlayerMatType playerMatType = (PlayerMatType)matId;
		tmp_Text.text = ScriptLocalization.Get(text + playerMatType.ToString());
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000289B0 File Offset: 0x00026BB0
	public void SetMatInteractable(bool interactable)
	{
		this.matButton.interactable = interactable;
		this.leftMatArrow.SetActive(interactable);
		this.rightMatArrow.SetActive(interactable);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000289D6 File Offset: 0x00026BD6
	public void NextFaction_OnClick()
	{
		if (this.OnFactionChangeNext != null)
		{
			this.OnFactionChangeNext();
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000289EB File Offset: 0x00026BEB
	public void PreviousFaction_OnClick()
	{
		if (this.OnFactionChangePrevious != null)
		{
			this.OnFactionChangePrevious();
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00028A00 File Offset: 0x00026C00
	public void NextMat_OnClick()
	{
		if (this.OnMatChangeNext != null)
		{
			this.OnMatChangeNext();
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00028A15 File Offset: 0x00026C15
	public void PreviousMat_OnClick()
	{
		if (this.OnMatChangePrevious != null)
		{
			this.OnMatChangePrevious();
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00056548 File Offset: 0x00054748
	public void DebugDelegates()
	{
		string text = "OnFactionChangeNext " + this.OnFactionChangeNext.GetInvocationList().Length.ToString();
		foreach (Delegate @delegate in this.OnFactionChangeNext.GetInvocationList())
		{
			string text2 = text;
			string text3 = "\t";
			Delegate delegate2 = @delegate;
			text = text2 + text3 + ((delegate2 != null) ? delegate2.ToString() : null);
		}
		text = text + "\nOnFactionChangePrevious " + this.OnFactionChangePrevious.GetInvocationList().Length.ToString();
		foreach (Delegate delegate3 in this.OnFactionChangePrevious.GetInvocationList())
		{
			string text4 = text;
			string text5 = "\t";
			Delegate delegate4 = delegate3;
			text = text4 + text5 + ((delegate4 != null) ? delegate4.ToString() : null);
		}
		text = text + "\nOnMatChangeNext " + this.OnMatChangeNext.GetInvocationList().Length.ToString();
		foreach (Delegate delegate5 in this.OnMatChangeNext.GetInvocationList())
		{
			string text6 = text;
			string text7 = "\t";
			Delegate delegate6 = delegate5;
			text = text6 + text7 + ((delegate6 != null) ? delegate6.ToString() : null);
		}
		text = text + "\nOnMatChangePrevious " + this.OnMatChangePrevious.GetInvocationList().Length.ToString();
		foreach (Delegate delegate7 in this.OnMatChangePrevious.GetInvocationList())
		{
			string text8 = text;
			string text9 = "\t";
			Delegate delegate8 = delegate7;
			text = text8 + text9 + ((delegate8 != null) ? delegate8.ToString() : null);
		}
		Debug.Log("[" + Time.time.ToString() + "] Sir_Maciej " + text);
	}

	// Token: 0x040000F5 RID: 245
	[Header("References")]
	[SerializeField]
	private Image playerTypeImage;

	// Token: 0x040000F6 RID: 246
	[SerializeField]
	private TextMeshProUGUI playerNameLabel;

	// Token: 0x040000F7 RID: 247
	[SerializeField]
	private Button factionButton;

	// Token: 0x040000F8 RID: 248
	[SerializeField]
	private Image factionImage;

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	private Image factionEmblem;

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private TextMeshProUGUI factionName;

	// Token: 0x040000FB RID: 251
	[SerializeField]
	private GameObject leftFactionArrow;

	// Token: 0x040000FC RID: 252
	[SerializeField]
	private GameObject rightFactionArrow;

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private GameObject timerObject;

	// Token: 0x040000FE RID: 254
	[SerializeField]
	private TextMeshProUGUI timerLabel;

	// Token: 0x040000FF RID: 255
	[SerializeField]
	private Button matButton;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private TextMeshProUGUI matLabel;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private GameObject leftMatArrow;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private GameObject rightMatArrow;

	// Token: 0x04000103 RID: 259
	[Header("Graphics")]
	[Tooltip("0-Polania, 1-Albion, 2-Nordic, 3-Rusviet, 4-Togawa, 5-Crimea, 6-Saxony, 7-Random, 8-RandomIFA")]
	[SerializeField]
	private Sprite[] factionImages;

	// Token: 0x04000104 RID: 260
	[Tooltip("0-Polania, 1-Albion, 2-Nordic, 3-Rusviet, 4-Togawa, 5-Crimea, 6-Saxony, 7-Random")]
	[SerializeField]
	private Sprite[] factionEmblems;

	// Token: 0x04000105 RID: 261
	[SerializeField]
	private Sprite playerIcon;

	// Token: 0x04000106 RID: 262
	[Tooltip("0-Easy, 1-Medium, 2-Hard")]
	[SerializeField]
	private Sprite[] botsIcons;

	// Token: 0x04000107 RID: 263
	private bool invadersMatsUnlocked;

	// Token: 0x0200002F RID: 47
	public enum PlayerType
	{
		// Token: 0x0400010D RID: 269
		Player,
		// Token: 0x0400010E RID: 270
		BotEasy,
		// Token: 0x0400010F RID: 271
		BotMedium,
		// Token: 0x04000110 RID: 272
		BotHard
	}
}
