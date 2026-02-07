using System;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200022B RID: 555
	public class ForfeitGamePanel : MonoBehaviour
	{
		// Token: 0x0600106E RID: 4206 RVA: 0x0008FD7C File Offset: 0x0008DF7C
		public void Activate(string gameId, string gameName, bool isRanked, Lobby lobby)
		{
			this.gameId = gameId;
			this.isRanked = isRanked;
			this.lobby = lobby;
			this.forfeitContentText.text = ScriptLocalization.Get("Lobby/ForfeitQuestion").Replace("{[GAME_NAME]}", gameName) + " " + (isRanked ? ScriptLocalization.Get("Lobby/ForfeitWarning") : "");
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0008FDEC File Offset: 0x0008DFEC
		public void ForfeitGame()
		{
			LobbyRestAPI.ForfeitGame(this.gameId, this.isRanked, delegate(string str)
			{
				this.lobby.RefreshGameList();
			}, delegate(Exception exception)
			{
				Debug.LogError("Forfeit failed with error " + ((exception != null) ? exception.ToString() : null));
			});
			this.Close();
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00032C2B File Offset: 0x00030E2B
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000CBB RID: 3259
		[SerializeField]
		private TextMeshProUGUI forfeitContentText;

		// Token: 0x04000CBC RID: 3260
		private string gameId;

		// Token: 0x04000CBD RID: 3261
		private bool isRanked;

		// Token: 0x04000CBE RID: 3262
		private Lobby lobby;
	}
}
