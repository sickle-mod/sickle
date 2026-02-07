using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000251 RID: 593
	public class InvitationPanel : MonoBehaviour
	{
		// Token: 0x0600120E RID: 4622 RVA: 0x0009623C File Offset: 0x0009443C
		public void Activate(InvitationReceived data)
		{
			string text = ScriptLocalization.Get("Lobby/InvitationTemplate").Replace("{[GAME_NAME]}", data.RoomName).Replace("{[PLAYER_NAME]}", data.Name);
			this.inviteText.text = text;
			this.accept.onClick.RemoveAllListeners();
			this.accept.onClick.AddListener(delegate
			{
				this.AcceptInvitation(data.RoomId);
			});
			base.gameObject.SetActive(true);
			this.accept.gameObject.SetActive(true);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00033B48 File Offset: 0x00031D48
		public void AcceptInvitation(string roomId)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.inviteReceived);
			this.lobby.JoinRoom(roomId);
			this.Close();
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00033B6E File Offset: 0x00031D6E
		public void DeclineInvitation()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.Close();
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00029172 File Offset: 0x00027372
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private Lobby lobby;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		private TextMeshProUGUI inviteText;

		// Token: 0x04000DEA RID: 3562
		[SerializeField]
		private Button accept;
	}
}
