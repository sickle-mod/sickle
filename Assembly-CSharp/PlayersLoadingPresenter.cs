using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using TMPro;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class PlayersLoadingPresenter : MonoBehaviour
{
	// Token: 0x060006B0 RID: 1712 RVA: 0x0002C4B2 File Offset: 0x0002A6B2
	private void OnEnable()
	{
		if (MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous)
		{
			PlayersLoadingPresenter.oldDate = LoadingScreenPresenterMobile.currentDate;
			this.playAndStayConnectingTimerActive = true;
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00071370 File Offset: 0x0006F570
	private void Update()
	{
		if (GameController.GameManager.IsMultiplayer && !GameController.GameManager.GameStarted && !MultiplayerController.Instance.AllPlayersLoaded())
		{
			this.textField.text = ScriptLocalization.Get("GameScene/PlayerLoading") + ":" + Environment.NewLine;
			using (IEnumerator<PlayerData> enumerator = MultiplayerController.Instance.GetPlayersInGame().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerData playerData = enumerator.Current;
					if (!playerData.MapLoaded)
					{
						if (MultiplayerController.Instance.IsMultiplayer && !MultiplayerController.Instance.SpectatorMode && !MultiplayerController.Instance.Asynchronous && this.playAndStayConnectingTimerActive)
						{
							this.difference = PlayersLoadingPresenter.oldDate.Subtract(DateTime.UtcNow).Duration();
							this.secondsDifferenceToShow = (float)((double)LoadingScreenPresenterMobile.loadTimeoutForDateTimer - this.difference.TotalSeconds);
							if (this.difference.TotalSeconds < (double)LoadingScreenPresenterMobile.loadTimeoutForDateTimer)
							{
								TextMeshProUGUI textMeshProUGUI = this.textField;
								textMeshProUGUI.text = string.Concat(new string[]
								{
									textMeshProUGUI.text,
									ScriptLocalization.Get("MainMenu/ConnectingAccount"),
									" ",
									this.GetPlayerTimer(),
									" ",
									ScriptLocalization.Get("FactionMat/" + ((Faction)playerData.Faction).ToString()),
									Environment.NewLine
								});
							}
							else
							{
								TextMeshProUGUI textMeshProUGUI = this.textField;
								textMeshProUGUI.text = string.Concat(new string[]
								{
									textMeshProUGUI.text,
									ScriptLocalization.Get("MainMenu/ConnectingAccount"),
									" 0:00 ",
									ScriptLocalization.Get("FactionMat/" + ((Faction)playerData.Faction).ToString()),
									Environment.NewLine
								});
							}
						}
						else
						{
							TextMeshProUGUI textMeshProUGUI2 = this.textField;
							textMeshProUGUI2.text = textMeshProUGUI2.text + ScriptLocalization.Get("FactionMat/" + ((Faction)playerData.Faction).ToString()) + Environment.NewLine;
						}
					}
				}
				return;
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000715D8 File Offset: 0x0006F7D8
	private string GetPlayerTimer()
	{
		this.minutes = Mathf.FloorToInt(this.secondsDifferenceToShow / 60f);
		this.seconds = Mathf.FloorToInt(this.secondsDifferenceToShow - (float)(this.minutes * 60));
		return string.Format("{0:0}:{1:00}", this.minutes, this.seconds);
	}

	// Token: 0x040005F3 RID: 1523
	[SerializeField]
	private TextMeshProUGUI textField;

	// Token: 0x040005F4 RID: 1524
	private int seconds;

	// Token: 0x040005F5 RID: 1525
	private int minutes;

	// Token: 0x040005F6 RID: 1526
	public static DateTime oldDate;

	// Token: 0x040005F7 RID: 1527
	private TimeSpan difference;

	// Token: 0x040005F8 RID: 1528
	private float secondsDifferenceToShow;

	// Token: 0x040005F9 RID: 1529
	private bool playAndStayConnectingTimerActive;
}
