using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000224 RID: 548
	public class PlayerTimeChooseMobile : MonoBehaviour
	{
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06001040 RID: 4160 RVA: 0x0008F9F4 File Offset: 0x0008DBF4
		// (remove) Token: 0x06001041 RID: 4161 RVA: 0x0008FA2C File Offset: 0x0008DC2C
		public event PlayerTimeChooseMobile.PlayerTimeChange OnPlayerTimeChange;

		// Token: 0x06001042 RID: 4162 RVA: 0x000328FF File Offset: 0x00030AFF
		public void OpenAsync(int currentDays, int currentHour)
		{
			this.isAsynchronous = true;
			this.minutesScroll.Deactivate();
			base.gameObject.SetActive(true);
			this.daysScroll.Activate(currentDays);
			this.hoursScroll.Activate(currentHour);
			this.StartOpenAnimation();
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0003293D File Offset: 0x00030B3D
		public void OpenSync(int currentMinutes)
		{
			this.isAsynchronous = false;
			this.daysScroll.Deactivate();
			this.hoursScroll.Deactivate();
			base.gameObject.SetActive(true);
			this.minutesScroll.Activate(currentMinutes);
			this.StartOpenAnimation();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003297A File Offset: 0x00030B7A
		public void OnCancelButtonClicked()
		{
			this.Close();
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00032982 File Offset: 0x00030B82
		public void OnOKButtonClicked()
		{
			if (this.OnPlayerTimeChange != null)
			{
				this.OnPlayerTimeChange(this.daysScroll.GetChosenValue(), this.hoursScroll.GetChosenValue(), this.minutesScroll.GetChosenValue());
			}
			this.Close();
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x000329BE File Offset: 0x00030BBE
		private void StartOpenAnimation()
		{
			this.content.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f, false).SetEase(Ease.Linear)
				.OnComplete(new TweenCallback(this.OnStartingAnimationComplete));
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x000329F3 File Offset: 0x00030BF3
		private void OnStartingAnimationComplete()
		{
			base.StartCoroutine(this.MoveToChosenPosition());
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00032A02 File Offset: 0x00030C02
		private IEnumerator MoveToChosenPosition()
		{
			yield return new WaitForEndOfFrame();
			if (this.isAsynchronous)
			{
				this.daysScroll.OnStartingAnimationComplete();
				this.hoursScroll.OnStartingAnimationComplete();
			}
			else
			{
				this.minutesScroll.OnStartingAnimationComplete();
			}
			yield break;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0008FA64 File Offset: 0x0008DC64
		private void Close()
		{
			this.content.GetComponent<RectTransform>().DOAnchorPosY(30f - this.content.GetComponent<RectTransform>().rect.height, 0.1f, false).SetEase(Ease.Linear)
				.OnComplete(new TweenCallback(this.OnClosingAnimationComplete));
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00029172 File Offset: 0x00027372
		private void OnClosingAnimationComplete()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000C97 RID: 3223
		[SerializeField]
		private PlayerTimeScroll daysScroll;

		// Token: 0x04000C98 RID: 3224
		[SerializeField]
		private PlayerTimeScroll hoursScroll;

		// Token: 0x04000C99 RID: 3225
		[SerializeField]
		private PlayerTimeScroll minutesScroll;

		// Token: 0x04000C9A RID: 3226
		[SerializeField]
		private GameObject content;

		// Token: 0x04000C9C RID: 3228
		private bool isAsynchronous;

		// Token: 0x02000225 RID: 549
		// (Invoke) Token: 0x0600104D RID: 4173
		public delegate void PlayerTimeChange(int days, int hours, int minutes);
	}
}
