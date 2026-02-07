using System;
using System.Collections;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004BA RID: 1210
	public class SingleClockPresenter : MonoBehaviour
	{
		// Token: 0x06002674 RID: 9844 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00040890 File Offset: 0x0003EA90
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x00040898 File Offset: 0x0003EA98
		public void DetermineVisibility()
		{
			this.SetActive(GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000408AA File Offset: 0x0003EAAA
		public void UpdateSize()
		{
			this.SetContentSizeFitterEnabled(true);
			base.StartCoroutine(this.ResizeAndDisableContentFitter());
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000408C0 File Offset: 0x0003EAC0
		private IEnumerator ResizeAndDisableContentFitter()
		{
			yield return new WaitForEndOfFrame();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			yield return new WaitForEndOfFrame();
			this.SetContentSizeFitterEnabled(false);
			yield break;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000E4B0C File Offset: 0x000E2D0C
		private void Refresh()
		{
			if (GameController.GameManager.GameStarted && MultiplayerController.Instance.AllPlayersLoaded())
			{
				if (!this.playerData.IsBot())
				{
					this.UpdateTimeText();
					this.UpdateStatusDot(this.playerData.IsOnline);
					return;
				}
				this.SetActive(false);
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000408CF File Offset: 0x0003EACF
		public void SetPlayerData(PlayerData playerData)
		{
			this.playerData = playerData;
			this.UpdatePlayerData();
			this.UpdateTimeText();
			this.UpdateStatusDot(playerData.IsOnline);
			if (base.gameObject.activeSelf)
			{
				this.UpdateSize();
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000E4B60 File Offset: 0x000E2D60
		public void UpdatePlayerData()
		{
			if (this.clockId >= MultiplayerController.Instance.playersInGame.Count)
			{
				this.SetActive(false);
				return;
			}
			this.SetFactionLogo();
			if (this.playerData.IsBot())
			{
				this.SetActive(false);
				return;
			}
			this.SetActive(true);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000E4BB0 File Offset: 0x000E2DB0
		private void UpdateTimeText()
		{
			int playerClock = this.playerData.PlayerClock;
			if (MultiplayerController.Instance.Asynchronous)
			{
				this.text.text = this.GetTimeInPlayAndGoFormat(playerClock);
				return;
			}
			this.text.text = this.GetTimeForPlayAndStay(playerClock);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x00040903 File Offset: 0x0003EB03
		private void UpdateStatusDot(bool isOnline)
		{
			this.statusDot.color = (isOnline ? this.STATUS_COLOR_ACTIVE : this.STATUS_COLOR_INACTIVE);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x00040921 File Offset: 0x0003EB21
		private string GetTimeInPlayAndGoFormat(int timeLeft)
		{
			return string.Format("{0} {1}:{2}:<size=60%>{3}", new object[]
			{
				this.GetDaysString(timeLeft),
				this.GetHoursString(timeLeft),
				this.GetMinutesString(timeLeft),
				this.GetSecondsString(timeLeft)
			});
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0004095B File Offset: 0x0003EB5B
		private string GetTimeForPlayAndStay(int timeLeft)
		{
			return string.Format("{0}:{1}:<size=60%>{2}", this.GetHoursString(timeLeft), this.GetMinutesString(timeLeft), this.GetSecondsString(timeLeft));
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000E4BFC File Offset: 0x000E2DFC
		private string GetDaysString(int timeLeft)
		{
			int days = this.GetDays(timeLeft);
			return string.Format("{0}d", days);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000E4C24 File Offset: 0x000E2E24
		private string GetHoursString(int timeLeft)
		{
			int hours = this.GetHours(timeLeft);
			return string.Format("{0}{1}", (hours < 10) ? "0" : string.Empty, hours);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000E4C5C File Offset: 0x000E2E5C
		private string GetMinutesString(int timeLeft)
		{
			int minutes = this.GetMinutes(timeLeft);
			return string.Format("{0}{1}", (minutes < 10) ? "0" : string.Empty, minutes);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000E4C94 File Offset: 0x000E2E94
		private string GetSecondsString(int timeLeft)
		{
			int seconds = this.GetSeconds(timeLeft);
			return string.Format("{0}{1}", (seconds < 10) ? "0" : string.Empty, seconds);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0004097C File Offset: 0x0003EB7C
		private int GetDays(int timeLeft)
		{
			return timeLeft / 86400;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x00040985 File Offset: 0x0003EB85
		private int GetHours(int timeLeft)
		{
			return timeLeft / 3600 % 24;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x00040991 File Offset: 0x0003EB91
		private int GetMinutes(int timeLeft)
		{
			return timeLeft / 60 % 60;
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0004099A File Offset: 0x0003EB9A
		private int GetSeconds(int timeLeft)
		{
			return timeLeft % 60;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000409A0 File Offset: 0x0003EBA0
		private void SetFactionLogo()
		{
			this.factionLogo.sprite = GameController.factionInfo[(Faction)this.playerData.Faction].logo;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000409C7 File Offset: 0x0003EBC7
		private void SetContentSizeFitterEnabled(bool enabled)
		{
			base.GetComponent<ContentSizeFitter>().enabled = enabled;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000409D5 File Offset: 0x0003EBD5
		private void UpdateRectTransform()
		{
			base.GetComponent<RectTransform>().ForceUpdateRectTransforms();
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000409E2 File Offset: 0x0003EBE2
		public int GetClockId()
		{
			return this.clockId;
		}

		// Token: 0x04001B87 RID: 7047
		private readonly Color STATUS_COLOR_ACTIVE = Color.green;

		// Token: 0x04001B88 RID: 7048
		private readonly Color STATUS_COLOR_INACTIVE = Color.red;

		// Token: 0x04001B89 RID: 7049
		[SerializeField]
		private int clockId;

		// Token: 0x04001B8A RID: 7050
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04001B8B RID: 7051
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001B8C RID: 7052
		[SerializeField]
		private Image statusDot;

		// Token: 0x04001B8D RID: 7053
		private PlayerData playerData;
	}
}
