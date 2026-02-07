using System;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200024C RID: 588
	public class MatChoiceTimer : MonoBehaviour
	{
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060011BB RID: 4539 RVA: 0x00095184 File Offset: 0x00093384
		// (remove) Token: 0x060011BC RID: 4540 RVA: 0x000951BC File Offset: 0x000933BC
		public event Action<int> OnTimeChanged;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060011BD RID: 4541 RVA: 0x000951F4 File Offset: 0x000933F4
		// (remove) Token: 0x060011BE RID: 4542 RVA: 0x0009522C File Offset: 0x0009342C
		public event Action OnTimePassed;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060011BF RID: 4543 RVA: 0x00095264 File Offset: 0x00093464
		// (remove) Token: 0x060011C0 RID: 4544 RVA: 0x0009529C File Offset: 0x0009349C
		public event Action OnAdditionalTimePassed;

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x000336F2 File Offset: 0x000318F2
		public int TimeLeft
		{
			get
			{
				return this.timeLeft;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x000336FA File Offset: 0x000318FA
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000952D4 File Offset: 0x000934D4
		public void Activate()
		{
			if (!this.isActive)
			{
				this.isActive = true;
				this.timeLeft = 60;
				base.InvokeRepeating("DecreaseTimeLeft", 1f, 1f);
				base.gameObject.SetActive(true);
				this.UpdateText();
				UniversalInvocator.Event_Invocator<int>(this.OnTimeChanged, new object[] { this.timeLeft });
			}
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00033702 File Offset: 0x00031902
		public void ActivateAdditionalTimerForEnemy()
		{
			this.timeLeft = 10;
			base.InvokeRepeating("DecreaseAdditionalTimeLeft", 1f, 1f);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00095340 File Offset: 0x00093540
		private void DecreaseTimeLeft()
		{
			this.timeLeft--;
			if (this.timeLeft <= 0)
			{
				if (this.OnTimePassed != null)
				{
					this.OnTimePassed();
				}
				this.ClearMainTimer();
				return;
			}
			this.UpdateText();
			UniversalInvocator.Event_Invocator<int>(this.OnTimeChanged, new object[] { this.timeLeft });
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00033721 File Offset: 0x00031921
		private void DecreaseAdditionalTimeLeft()
		{
			this.timeLeft--;
			if (this.timeLeft <= 0)
			{
				if (this.OnAdditionalTimePassed != null)
				{
					this.OnAdditionalTimePassed();
				}
				this.ClearAdditionalTimer();
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00033753 File Offset: 0x00031953
		private void UpdateText()
		{
			if (this.timeLeftText)
			{
				this.timeLeftText.text = this.timeLeft.ToString();
			}
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000953A4 File Offset: 0x000935A4
		public void Deactivate()
		{
			if (this.isActive)
			{
				this.isActive = false;
				this.OnTimeChanged = null;
				this.OnTimePassed = null;
				this.OnAdditionalTimePassed = null;
				base.CancelInvoke("DecreaseTimeLeft");
				base.CancelInvoke("DecreaseAdditionalTimeLeft");
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00033778 File Offset: 0x00031978
		private void ClearMainTimer()
		{
			this.OnTimeChanged = null;
			this.OnTimePassed = null;
			base.CancelInvoke("DecreaseTimeLeft");
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00033793 File Offset: 0x00031993
		private void ClearAdditionalTimer()
		{
			this.OnAdditionalTimePassed = null;
			base.CancelInvoke("DecreaseAdditionalTimeLeft");
			base.gameObject.SetActive(false);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000337B3 File Offset: 0x000319B3
		public void SetTime(int time)
		{
			if (time >= 0)
			{
				this.timeLeft = time;
			}
		}

		// Token: 0x04000DB2 RID: 3506
		public const int DEFAULT_CHOICE_TIME_IN_SECONDS = 60;

		// Token: 0x04000DB3 RID: 3507
		public const int DEFAULT_BOT_AUTO_CHOICE_TIME_IN_SECONDS = 57;

		// Token: 0x04000DB4 RID: 3508
		public const int DEFAULT_ADDITIONAL_TIME_FOR_ENEMY = 10;

		// Token: 0x04000DB5 RID: 3509
		[SerializeField]
		private TextMeshProUGUI timeLeftText;

		// Token: 0x04000DB9 RID: 3513
		private int timeLeft;

		// Token: 0x04000DBA RID: 3514
		private bool isActive;
	}
}
