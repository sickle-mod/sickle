using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000227 RID: 551
	public class PlayerTimeMobile : MonoBehaviour
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x0008FB34 File Offset: 0x0008DD34
		public void Init(bool isAsynchronous)
		{
			this.isAsynchronous = isAsynchronous;
			this.daysText = ScriptLocalization.Get("Lobby/Days");
			this.hoursText = ScriptLocalization.Get("Lobby/Hours");
			this.minutesText = ScriptLocalization.Get("Lobby/MinutesAbbreviation");
			this.playerTimeChooseMobile.OnPlayerTimeChange += this.PlayerTimeChooseMobile_OnPlayerTimeChange;
			this.UpdateText();
			this.SetFrameWidth();
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0008FB9C File Offset: 0x0008DD9C
		private void SetFrameWidth()
		{
			Rect rect = this.frame.GetComponent<RectTransform>().rect;
			if (this.isAsynchronous)
			{
				rect.width = 180f;
			}
			else
			{
				rect.width = 75f;
			}
			this.frame.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00032A28 File Offset: 0x00030C28
		private void PlayerTimeChooseMobile_OnPlayerTimeChange(int days, int hours, int minutes)
		{
			if (this.isAsynchronous)
			{
				if (days == 0 && hours == 0)
				{
					days = 1;
				}
				this.currentDays = days;
				this.currentHours = hours;
			}
			else
			{
				this.currentMinutes = minutes;
			}
			this.UpdateText();
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00032A58 File Offset: 0x00030C58
		public void OnChooseTimeButtonClicked()
		{
			if (this.isAsynchronous)
			{
				this.playerTimeChooseMobile.OpenAsync(this.currentDays, this.currentHours);
				return;
			}
			this.playerTimeChooseMobile.OpenSync(this.currentMinutes);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00032A8B File Offset: 0x00030C8B
		public int GetTimeInMinutes()
		{
			if (this.isAsynchronous)
			{
				return this.currentDays * 1440 + this.currentHours * 60;
			}
			return this.currentMinutes;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0008FC00 File Offset: 0x0008DE00
		private void UpdateText()
		{
			if (this.isAsynchronous)
			{
				this.text.text = string.Format("{0} {1} {2} {3}", new object[] { this.currentDays, this.daysText, this.currentHours, this.hoursText });
				return;
			}
			this.text.text = string.Format("{0} {1}", this.currentMinutes, this.minutesText);
		}

		// Token: 0x04000CA0 RID: 3232
		private const int ASYNCHRONOUS_FRAME_WIDTH = 180;

		// Token: 0x04000CA1 RID: 3233
		private const int SYNCHRONOUS_FRAME_WIDTH = 75;

		// Token: 0x04000CA2 RID: 3234
		[SerializeField]
		private PlayerTimeChooseMobile playerTimeChooseMobile;

		// Token: 0x04000CA3 RID: 3235
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000CA4 RID: 3236
		[SerializeField]
		private Image frame;

		// Token: 0x04000CA5 RID: 3237
		private string daysText;

		// Token: 0x04000CA6 RID: 3238
		private string hoursText;

		// Token: 0x04000CA7 RID: 3239
		private string minutesText;

		// Token: 0x04000CA8 RID: 3240
		private int currentDays = 1;

		// Token: 0x04000CA9 RID: 3241
		private int currentHours;

		// Token: 0x04000CAA RID: 3242
		private int currentMinutes = 30;

		// Token: 0x04000CAB RID: 3243
		private bool isAsynchronous;
	}
}
