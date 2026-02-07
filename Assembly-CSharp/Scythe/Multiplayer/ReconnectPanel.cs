using System;
using System.Collections;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000272 RID: 626
	public class ReconnectPanel : MonoBehaviour
	{
		// Token: 0x06001345 RID: 4933 RVA: 0x000984C8 File Offset: 0x000966C8
		public void Activate(string gameId, int timeLeft, GameType gameType, bool rankedGame)
		{
			this.gameId = gameId;
			this.rankedGame = rankedGame;
			timeLeft -= 5;
			if (timeLeft <= 0)
			{
				return;
			}
			this.timeLeft = timeLeft;
			if (gameType == GameType.Asynchronous)
			{
				this.quit.onClick.AddListener(new UnityAction(this.Close));
				this.reconnect.onClick.AddListener(new UnityAction(this.Resume));
			}
			else
			{
				this.quit.onClick.AddListener(new UnityAction(this.Abandon));
				this.reconnect.onClick.AddListener(new UnityAction(this.Reconnect));
				if (this.karmaEloLabel)
				{
					string text = string.Empty;
					if (rankedGame)
					{
						text = "Lobby/KarmaEloLose";
					}
					else
					{
						text = "Lobby/KarmaLose";
					}
					string text2 = ScriptLocalization.Get(text);
					if (string.IsNullOrEmpty(text2))
					{
						text2 = "[LOC KEY NOT FOUND: " + text + "]";
					}
					this.karmaEloLabel.text = text2;
				}
			}
			base.gameObject.SetActive(true);
			if (this.timeLeftText)
			{
				this.UpdateTimeLeftText();
				base.StartCoroutine(this.DecreaseTimeLeft());
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000985EC File Offset: 0x000967EC
		private void Reconnect()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			int @int = PlayerPrefs.GetInt("PlayerClock", -1);
			this.lobby.Reconnect(this.gameId, @int, this.rankedGame, GameType.Synchronous);
			this.Close();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00034E1C File Offset: 0x0003301C
		private void Resume()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.lobby.Reconnect(this.gameId, -1, this.rankedGame, GameType.Asynchronous);
			this.Close();
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00098630 File Offset: 0x00096830
		private void Abandon()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			LobbyRestAPI.AbandonGame(this.gameId, delegate(string response)
			{
			}, delegate(Exception error)
			{
			});
			this.Close();
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00098698 File Offset: 0x00096898
		private void UpdateTimeLeftText()
		{
			int num = this.timeLeft / 60;
			int num2 = this.timeLeft % 60;
			this.timeLeftText.text = string.Format("{0}:{1}", num, num2.ToString().PadLeft(2, '0'));
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00034E49 File Offset: 0x00033049
		private IEnumerator DecreaseTimeLeft()
		{
			while (this.timeLeft > 0)
			{
				yield return new WaitForSeconds(1f);
				this.timeLeft--;
				if (this.timeLeft <= 0)
				{
					this.quit.onClick.Invoke();
					yield break;
				}
				this.UpdateTimeLeftText();
			}
			yield break;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00034E58 File Offset: 0x00033058
		public void Close()
		{
			this.quit.onClick.RemoveAllListeners();
			this.reconnect.onClick.RemoveAllListeners();
			base.StopCoroutine(this.DecreaseTimeLeft());
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E72 RID: 3698
		private const string timeLeftFormat = "{0}:{1}";

		// Token: 0x04000E73 RID: 3699
		[SerializeField]
		private Lobby lobby;

		// Token: 0x04000E74 RID: 3700
		[SerializeField]
		private Button quit;

		// Token: 0x04000E75 RID: 3701
		[SerializeField]
		private Button reconnect;

		// Token: 0x04000E76 RID: 3702
		[SerializeField]
		private TextMeshProUGUI timeLeftText;

		// Token: 0x04000E77 RID: 3703
		[SerializeField]
		private TextMeshProUGUI descLabel;

		// Token: 0x04000E78 RID: 3704
		[SerializeField]
		private TextMeshProUGUI karmaEloLabel;

		// Token: 0x04000E79 RID: 3705
		private int timeLeft;

		// Token: 0x04000E7A RID: 3706
		private string gameId;

		// Token: 0x04000E7B RID: 3707
		private bool rankedGame;
	}
}
