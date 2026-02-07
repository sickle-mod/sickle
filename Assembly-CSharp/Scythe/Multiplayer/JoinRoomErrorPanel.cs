using System;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000253 RID: 595
	public class JoinRoomErrorPanel : MonoBehaviour
	{
		// Token: 0x06001215 RID: 4629 RVA: 0x000962E8 File Offset: 0x000944E8
		public void Open(JoinRoomErrorStatus joinRoomErrorStatus)
		{
			this.tittleText.text = ScriptLocalization.Get("GameScene/ConnectionProblem");
			this.descriptionText.text = this.GetErrorLocalizationString(joinRoomErrorStatus);
			this.confirmButtonText.text = ScriptLocalization.Get("Common/Ok");
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00033B9A File Offset: 0x00031D9A
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00096340 File Offset: 0x00094540
		private string GetErrorLocalizationString(JoinRoomErrorStatus joinRoomErrorStatus)
		{
			switch (joinRoomErrorStatus)
			{
			case JoinRoomErrorStatus.RoomDoesNotExists:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusRoomNotExists");
			case JoinRoomErrorStatus.GameAlreadyStarted:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusGameAlreadyStarted");
			case JoinRoomErrorStatus.RoomIsFull:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusRoomIsFull");
			case JoinRoomErrorStatus.YouAreAlreadyInThisRoom:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusPlayerIsInRoom");
			case JoinRoomErrorStatus.PlayersAreChoosingMats:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusChoosingMats");
			case JoinRoomErrorStatus.YourEloIsTooLow:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusLowElo");
			case JoinRoomErrorStatus.YourEloIsTooHigh:
				return ScriptLocalization.Get("Lobby/JoinRoomErrorStatusHighElo");
			}
			return ScriptLocalization.Get("Lobby/UnknownErrorContent");
		}

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		private TextMeshProUGUI tittleText;

		// Token: 0x04000DEE RID: 3566
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04000DEF RID: 3567
		[SerializeField]
		private TextMeshProUGUI confirmButtonText;
	}
}
