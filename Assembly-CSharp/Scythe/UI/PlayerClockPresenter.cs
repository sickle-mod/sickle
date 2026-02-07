using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004B3 RID: 1203
	public class PlayerClockPresenter : MonoBehaviour
	{
		// Token: 0x0600262E RID: 9774 RVA: 0x000406F3 File Offset: 0x0003E8F3
		private void Start()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0004071A File Offset: 0x0003E91A
		private void OnEnable()
		{
			if (MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous)
			{
				PlayerClockPresenter.oldDate = LoadingScreenPresenter.currentDate;
				this.playAndStayConnectingTimerActive = true;
			}
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000E2C60 File Offset: 0x000E0E60
		private void Update()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.timeLeftText1.text = string.Empty;
				if (GameController.GameManager.GameStarted || MultiplayerController.Instance.AllPlayersLoaded())
				{
					using (IEnumerator<PlayerData> enumerator = MultiplayerController.Instance.GetPlayersInGame().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PlayerData playerData = enumerator.Current;
							int num = playerData.PlayerClock;
							int num2 = Mathf.FloorToInt((float)(num / 86400));
							num -= num2 * 60 * 60 * 24;
							int num3 = Mathf.FloorToInt((float)(num / 3600));
							num -= num3 * 60 * 60;
							int num4 = Mathf.FloorToInt((float)(num / 60));
							num -= num4 * 60;
							string text = string.Concat(new string[]
							{
								(num2 > 0) ? (num2.ToString() + "d ") : "",
								num3.ToString().PadLeft(2, '0'),
								":",
								num4.ToString().PadLeft(2, '0'),
								":",
								num.ToString().PadLeft(2, '0')
							});
							if (!playerData.IsBot())
							{
								TextMeshProUGUI textMeshProUGUI = this.timeLeftText1;
								textMeshProUGUI.text = string.Concat(new string[]
								{
									textMeshProUGUI.text,
									ScriptLocalization.Get("FactionMat/" + ((Faction)playerData.Faction).ToString()),
									": ",
									text,
									Environment.NewLine
								});
							}
						}
						goto IL_03AA;
					}
				}
				this.timeLeftText1.text = ScriptLocalization.Get("GameScene/PlayerLoading") + ":" + Environment.NewLine;
				foreach (PlayerData playerData2 in MultiplayerController.Instance.GetPlayersInGame())
				{
					if (!playerData2.MapLoaded)
					{
						if (MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous && this.playAndStayConnectingTimerActive)
						{
							this.difference = PlayerClockPresenter.oldDate.Subtract(DateTime.UtcNow).Duration();
							this.secondsDifferenceToShow = (float)((double)LoadingScreenPresenter.loadTimeoutForDateTimer - this.difference.TotalSeconds);
							if (this.difference.TotalSeconds < (double)LoadingScreenPresenter.loadTimeoutForDateTimer)
							{
								TextMeshProUGUI textMeshProUGUI = this.timeLeftText1;
								textMeshProUGUI.text = string.Concat(new string[]
								{
									textMeshProUGUI.text,
									ScriptLocalization.Get("MainMenu/ConnectingAccount"),
									" ",
									this.GetPlayerTimer(),
									" ",
									ScriptLocalization.Get("FactionMat/" + ((Faction)playerData2.Faction).ToString()),
									Environment.NewLine
								});
							}
							else
							{
								TextMeshProUGUI textMeshProUGUI = this.timeLeftText1;
								textMeshProUGUI.text = string.Concat(new string[]
								{
									textMeshProUGUI.text,
									ScriptLocalization.Get("MainMenu/ConnectingAccount"),
									" 0:00 ",
									ScriptLocalization.Get("FactionMat/" + ((Faction)playerData2.Faction).ToString()),
									Environment.NewLine
								});
							}
						}
						else
						{
							TextMeshProUGUI textMeshProUGUI2 = this.timeLeftText1;
							textMeshProUGUI2.text = textMeshProUGUI2.text + ScriptLocalization.Get("FactionMat/" + ((Faction)playerData2.Faction).ToString()) + Environment.NewLine;
						}
					}
				}
				IL_03AA:
				this.timeLeftText2.text = this.timeLeftText1.text;
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000E3064 File Offset: 0x000E1264
		private string GetPlayerTimer()
		{
			this.minutes = Mathf.FloorToInt(this.secondsDifferenceToShow / 60f);
			this.seconds = Mathf.FloorToInt(this.secondsDifferenceToShow - (float)(this.minutes * 60));
			return string.Format("{0:0}:{1:00}", this.minutes, this.seconds);
		}

		// Token: 0x04001B1E RID: 6942
		public TextMeshProUGUI timeLeftText1;

		// Token: 0x04001B1F RID: 6943
		public TextMeshProUGUI timeLeftText2;

		// Token: 0x04001B20 RID: 6944
		private int seconds;

		// Token: 0x04001B21 RID: 6945
		private int minutes;

		// Token: 0x04001B22 RID: 6946
		public static DateTime oldDate;

		// Token: 0x04001B23 RID: 6947
		private TimeSpan difference;

		// Token: 0x04001B24 RID: 6948
		private float secondsDifferenceToShow;

		// Token: 0x04001B25 RID: 6949
		private bool playAndStayConnectingTimerActive;
	}
}
