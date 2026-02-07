using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x020001F7 RID: 503
	public class LobbyCounters : MonoBehaviour
	{
		// Token: 0x06000ED4 RID: 3796 RVA: 0x00031B5A File Offset: 0x0002FD5A
		private void OnEnable()
		{
			this.ResetCounters();
			base.StartCoroutine(this.RefreshCountersCoroutine());
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00031B6F File Offset: 0x0002FD6F
		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00031B77 File Offset: 0x0002FD77
		private IEnumerator RefreshCountersCoroutine()
		{
			for (;;)
			{
				LobbyRestAPI.GetAmountOfConnectedPlayers(new Action<string>(this.RefreshConnectedPlayersAmount), null);
				LobbyRestAPI.GetAmountOfOngoingGames(new Action<string>(this.RefreshOngoingGamesAmount), null);
				yield return new WaitForSeconds(60f);
			}
			yield break;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00031B86 File Offset: 0x0002FD86
		private void ResetCounters()
		{
			this.connectedPlayersText.text = ScriptLocalization.Get("Lobby/ConnectedPlayers");
			this.ongoingGamesText.text = ScriptLocalization.Get("Lobby/OngoingGames");
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0008B544 File Offset: 0x00089744
		private void RefreshConnectedPlayersAmount(string amount)
		{
			int num;
			if (int.TryParse(amount, out num))
			{
				this.connectedPlayersText.text = ScriptLocalization.Get("Lobby/ConnectedPlayers") + " " + num.ToString();
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0008B584 File Offset: 0x00089784
		private void RefreshOngoingGamesAmount(string amount)
		{
			int num;
			if (int.TryParse(amount, out num))
			{
				this.ongoingGamesText.text = ScriptLocalization.Get("Lobby/OngoingGames") + " " + num.ToString();
			}
		}

		// Token: 0x04000B96 RID: 2966
		[SerializeField]
		private TextMeshProUGUI connectedPlayersText;

		// Token: 0x04000B97 RID: 2967
		[SerializeField]
		private TextMeshProUGUI ongoingGamesText;

		// Token: 0x04000B98 RID: 2968
		private const float REFRESH_TIME = 60f;
	}
}
