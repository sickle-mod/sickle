using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000223 RID: 547
	public class PlayerTime : MonoBehaviour
	{
		// Token: 0x06001036 RID: 4150 RVA: 0x0008F83C File Offset: 0x0008DA3C
		public void Init(bool asynchronous)
		{
			this.synchronousTime = ScriptLocalization.Get("Lobby/MinutesAbbreviation");
			this.asynchronousTime = ScriptLocalization.Get("Lobby/Days");
			this.asynchronous = asynchronous;
			if (asynchronous)
			{
				this.maxTime = 30;
				this.minTime = 1;
				this.currentTime = 1;
				this.time = this.asynchronousTime;
			}
			else
			{
				this.maxTime = 90;
				this.minTime = 10;
				this.currentTime = 45;
				this.time = this.synchronousTime;
			}
			this.CheckTime();
			this.UpdateTimeText();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0008F8C8 File Offset: 0x0008DAC8
		private void CheckTime()
		{
			this.increaseTime.interactable = true;
			this.decreaseTime.interactable = true;
			if (this.currentTime >= this.maxTime)
			{
				this.increaseTime.interactable = false;
				this.currentTime = this.maxTime;
			}
			if (this.currentTime <= this.minTime)
			{
				this.decreaseTime.interactable = false;
				this.currentTime = this.minTime;
			}
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003285D File Offset: 0x00030A5D
		private void UpdateTimeText()
		{
			this.text.text = string.Format("{0} {1}", this.currentTime, this.time);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0008F93C File Offset: 0x0008DB3C
		public void IncreaseTime()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			if (this.currentTime == this.maxTime)
			{
				return;
			}
			if (this.asynchronous)
			{
				this.currentTime++;
			}
			else
			{
				this.currentTime += 5;
			}
			this.CheckTime();
			this.UpdateTimeText();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0008F998 File Offset: 0x0008DB98
		public void DecreaseTime()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			if (this.currentTime == this.minTime)
			{
				return;
			}
			if (this.asynchronous)
			{
				this.currentTime--;
			}
			else
			{
				this.currentTime -= 5;
			}
			this.CheckTime();
			this.UpdateTimeText();
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00032885 File Offset: 0x00030A85
		public void OnInputStart()
		{
			this.text.text = this.currentTime.ToString();
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0003289D File Offset: 0x00030A9D
		public void OnValueChanged(string newValue)
		{
			this.currentTime = int.Parse(newValue);
			this.CheckTime();
			this.UpdateTimeText();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000328B7 File Offset: 0x00030AB7
		public int GetTimeInMinutes()
		{
			if (this.asynchronous)
			{
				return this.currentTime * 1440;
			}
			return this.currentTime;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000328D4 File Offset: 0x00030AD4
		public static int GetDefaultTimeInMinutes(bool asynchronous)
		{
			if (asynchronous)
			{
				return 1440;
			}
			return 45;
		}

		// Token: 0x04000C84 RID: 3204
		[SerializeField]
		private TMP_InputField text;

		// Token: 0x04000C85 RID: 3205
		[SerializeField]
		private Button decreaseTime;

		// Token: 0x04000C86 RID: 3206
		[SerializeField]
		private Button increaseTime;

		// Token: 0x04000C87 RID: 3207
		public const int MinutesPerDay = 1440;

		// Token: 0x04000C88 RID: 3208
		public const int MinutesPerHour = 60;

		// Token: 0x04000C89 RID: 3209
		private const float DelayBetweenChangingTime = 0.2f;

		// Token: 0x04000C8A RID: 3210
		private const int MinSynchronous = 10;

		// Token: 0x04000C8B RID: 3211
		private const int DefaultSynchronous = 45;

		// Token: 0x04000C8C RID: 3212
		private const int MaxSynchronous = 90;

		// Token: 0x04000C8D RID: 3213
		private const int MinAsynchronous = 1;

		// Token: 0x04000C8E RID: 3214
		private const int DefaultAsynchronous = 1;

		// Token: 0x04000C8F RID: 3215
		private const int MaxAsynchronous = 30;

		// Token: 0x04000C90 RID: 3216
		private string synchronousTime = string.Empty;

		// Token: 0x04000C91 RID: 3217
		private string asynchronousTime = string.Empty;

		// Token: 0x04000C92 RID: 3218
		private bool asynchronous;

		// Token: 0x04000C93 RID: 3219
		private int currentTime;

		// Token: 0x04000C94 RID: 3220
		private int maxTime;

		// Token: 0x04000C95 RID: 3221
		private int minTime;

		// Token: 0x04000C96 RID: 3222
		private string time;
	}
}
