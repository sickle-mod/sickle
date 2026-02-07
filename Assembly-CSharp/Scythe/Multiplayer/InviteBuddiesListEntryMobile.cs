using System;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001F6 RID: 502
	public class InviteBuddiesListEntryMobile : MonoBehaviour
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00031AEB File Offset: 0x0002FCEB
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00031AF3 File Offset: 0x0002FCF3
		public PlayerInfo PlayerInfo
		{
			get
			{
				return this.playerInfo;
			}
			set
			{
				this.SetPlayerInfo(value);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00031AFC File Offset: 0x0002FCFC
		public bool IsSelected
		{
			get
			{
				return this.selectionToggle.isOn;
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00031B09 File Offset: 0x0002FD09
		public void SetPlayerNick(string nick)
		{
			if (this.playerNickLabel)
			{
				this.playerNickLabel.text = nick;
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00031B24 File Offset: 0x0002FD24
		public void SetPlayerStatus(string status)
		{
			if (this.playerStatusLabel)
			{
				this.playerStatusLabel.text = status;
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00031B3F File Offset: 0x0002FD3F
		public void SetSelection(bool value)
		{
			if (this.selectionToggle)
			{
				this.selectionToggle.isOn = value;
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0008B45C File Offset: 0x0008965C
		private void SetPlayerInfo(PlayerInfo playerInfo)
		{
			this.playerInfo = playerInfo;
			if (playerInfo != null)
			{
				this.SetPlayerNick(playerInfo.PlayerStats.Name);
				this.SetPlayerStatus(InviteBuddiesListEntryMobile.PlayerStatusToLocalizedString(this.PlayerInfo.PlayerStatus));
				this.SetSelection(false);
				return;
			}
			this.SetPlayerNick("name");
			this.SetPlayerStatus("status");
			this.SetSelection(false);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0008B4C0 File Offset: 0x000896C0
		private static string PlayerStatusToLocalizedString(PlayerStatus playerStatus)
		{
			switch (playerStatus)
			{
			case PlayerStatus.Offline:
				return "<color=#A54043>" + ScriptLocalization.Get("Lobby/Offline") + "</color>";
			case PlayerStatus.InLobby:
				return ScriptLocalization.Get("Lobby/InLobby");
			case PlayerStatus.InRoom:
				return ScriptLocalization.Get("Lobby/InRoom");
			case PlayerStatus.InGame:
				return ScriptLocalization.Get("Lobby/InGame");
			default:
				Debug.LogError("[InviteBuddiesListEntryMobile]: There is no localized string for PlayerStatus " + playerStatus.ToString());
				return "";
			}
		}

		// Token: 0x04000B92 RID: 2962
		[SerializeField]
		private TextMeshProUGUI playerNickLabel;

		// Token: 0x04000B93 RID: 2963
		[SerializeField]
		private TextMeshProUGUI playerStatusLabel;

		// Token: 0x04000B94 RID: 2964
		[SerializeField]
		private Toggle2 selectionToggle;

		// Token: 0x04000B95 RID: 2965
		private PlayerInfo playerInfo;
	}
}
