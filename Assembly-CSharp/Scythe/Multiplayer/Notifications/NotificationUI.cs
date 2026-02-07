using System;
using System.Collections;
using AsmodeeNet.Foundation;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x020002A4 RID: 676
	public class NotificationUI : MonoBehaviour, IEndDragHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06001568 RID: 5480 RVA: 0x00036823 File Offset: 0x00034A23
		private void Display()
		{
			this.notificationRect.gameObject.SetActive(true);
			this.clicked = false;
			base.StartCoroutine(this.AnimateNotification());
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0009D6F0 File Offset: 0x0009B8F0
		public void DisplayNotification(Notification notification)
		{
			if (notification is CombatNotification)
			{
				this.DisplayNotification((CombatNotification)notification);
				return;
			}
			if (notification is GameOverNotification)
			{
				this.DisplayNotification((GameOverNotification)notification);
				return;
			}
			if (notification is YourTurnNotification)
			{
				this.DisplayNotification((YourTurnNotification)notification);
				return;
			}
			this.notificationBody.text = ((notification.Name != null) ? notification.Name : "") + "\n" + ((notification.Subject != null) ? notification.Subject.ToString() : "");
			this.notificationClickableInfo.text = "";
			this.Display();
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0009D798 File Offset: 0x0009B998
		private void DisplayNotification(CombatNotification notification)
		{
			this.notificationBody.text = ScriptLocalization.Get("Notifications/CombatInfo").Replace("{[GAME_NAME]}", notification.GameName);
			this.notificationClickableInfo.text = "<color=#F2C94C>" + ScriptLocalization.Get("Notifications/CombatClick") + "</color>";
			this.Display();
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0009D7F4 File Offset: 0x0009B9F4
		private void DisplayNotification(GameOverNotification notification)
		{
			this.notificationBody.text = ScriptLocalization.Get("Notifications/EndOfGameInfo").Replace("{[GAME_NAME]}", notification.GameName).Replace("{[PLAYER_PLACE]}", this.PlaceLocalizedString(notification.PlayerPlace));
			this.notificationClickableInfo.text = "<color=#F2C94C>" + ScriptLocalization.Get("Notifications/EndOfGameClick") + "</color>";
			this.Display();
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0009D868 File Offset: 0x0009BA68
		private void DisplayNotification(YourTurnNotification notification)
		{
			this.notificationBody.text = ScriptLocalization.Get("Notifications/YourTurnInfo").Replace("{[GAME_NAME]}", notification.GameName);
			this.notificationClickableInfo.text = "<color=#F2C94C>" + ScriptLocalization.Get("Notifications/YourTurnClick") + "</color>";
			this.Display();
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0009D8C4 File Offset: 0x0009BAC4
		private string PlaceLocalizedString(int place)
		{
			switch (place)
			{
			case 0:
				return "last place";
			case 1:
				return ScriptLocalization.Get("Statistics/1Place");
			case 2:
				return ScriptLocalization.Get("Statistics/2Place");
			case 3:
				return ScriptLocalization.Get("Statistics/3Place");
			case 4:
				return ScriptLocalization.Get("Statistics/4Place");
			case 5:
				return ScriptLocalization.Get("Statistics/5Place");
			case 6:
				return ScriptLocalization.Get("Statistics/6Place");
			case 7:
				return ScriptLocalization.Get("Statistics/7Place");
			default:
				throw new ArgumentOutOfRangeException("Not a valid place number. " + place.ToString());
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0003684A File Offset: 0x00034A4A
		private IEnumerator AnimateNotification()
		{
			this.notificationRect.anchoredPosition = this.hiddenPosition;
			float timer = 0f;
			this.fastForwardAnimation = false;
			while (timer < this.appearingTime)
			{
				yield return new WaitForEndOfFrame();
				this.notificationRect.anchoredPosition = Vector2.Lerp(this.hiddenPosition, this.visiblePosition, timer / this.appearingTime);
				timer += Time.unscaledDeltaTime;
			}
			this.notificationRect.anchoredPosition = this.visiblePosition;
			for (timer = 0f; timer < this.showTime; timer += (this.fastForwardAnimation ? this.showTime : Time.unscaledDeltaTime))
			{
				yield return new WaitForEndOfFrame();
			}
			for (timer = 0f; timer < this.disappearingTime; timer += (this.fastForwardAnimation ? (Time.unscaledDeltaTime * 5f) : Time.unscaledDeltaTime))
			{
				yield return new WaitForEndOfFrame();
				this.notificationRect.anchoredPosition = Vector2.Lerp(this.visiblePosition, this.hiddenPosition, timer / this.disappearingTime);
			}
			this.notificationRect.gameObject.SetActive(false);
			this.notificationsDisplayManager.NotificationDisplayFinished();
			yield break;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00036859 File Offset: 0x00034A59
		public void FollowNotification_OnClick()
		{
			if (!this.clicked)
			{
				this.clicked = true;
				this.fastForwardAnimation = true;
				this.notificationsDisplayManager.NotificationClicked();
			}
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0003687C File Offset: 0x00034A7C
		public void HideNotification_OnClick()
		{
			this.fastForwardAnimation = true;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0009D964 File Offset: 0x0009BB64
		public void OnEndDrag(PointerEventData eventData)
		{
			Vector3 vector = (eventData.position - eventData.pressPosition).normalized;
			if (this.GetDragDirection(vector) == this.swipeDirection)
			{
				this.fastForwardAnimation = true;
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0009D9A8 File Offset: 0x0009BBA8
		private NotificationUI.SwipeDirection GetDragDirection(Vector3 dragVector)
		{
			float num = Mathf.Abs(dragVector.x);
			float num2 = Mathf.Abs(dragVector.y);
			NotificationUI.SwipeDirection swipeDirection;
			if (num > num2)
			{
				swipeDirection = ((dragVector.x > 0f) ? NotificationUI.SwipeDirection.Right : NotificationUI.SwipeDirection.Left);
			}
			else
			{
				swipeDirection = ((dragVector.y > 0f) ? NotificationUI.SwipeDirection.Up : NotificationUI.SwipeDirection.Down);
			}
			return swipeDirection;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x04000FA0 RID: 4000
		private const string CLICKABLE_COLOR_TAG = "<color=#F2C94C>";

		// Token: 0x04000FA1 RID: 4001
		private const string COLOR_END_TAG = "</color>";

		// Token: 0x04000FA2 RID: 4002
		[Header("References")]
		[SerializeField]
		private NotificationsDisplayManager notificationsDisplayManager;

		// Token: 0x04000FA3 RID: 4003
		[SerializeField]
		private RectTransform notificationRect;

		// Token: 0x04000FA4 RID: 4004
		[SerializeField]
		private Vector2 hiddenPosition;

		// Token: 0x04000FA5 RID: 4005
		[SerializeField]
		private Vector2 visiblePosition;

		// Token: 0x04000FA6 RID: 4006
		[SerializeField]
		private TextMeshProUGUI notificationBody;

		// Token: 0x04000FA7 RID: 4007
		[SerializeField]
		private TextMeshProUGUI notificationClickableInfo;

		// Token: 0x04000FA8 RID: 4008
		[Header("Parameters")]
		[SerializeField]
		private float appearingTime = 2f;

		// Token: 0x04000FA9 RID: 4009
		[SerializeField]
		private float showTime = 5f;

		// Token: 0x04000FAA RID: 4010
		[SerializeField]
		private float disappearingTime = 2f;

		// Token: 0x04000FAB RID: 4011
		[SerializeField]
		private NotificationUI.SwipeDirection swipeDirection;

		// Token: 0x04000FAC RID: 4012
		private const float FAST_FORWARD_TIME_MULTIPLIER = 5f;

		// Token: 0x04000FAD RID: 4013
		private bool fastForwardAnimation;

		// Token: 0x04000FAE RID: 4014
		private bool clicked;

		// Token: 0x020002A5 RID: 677
		[Serializable]
		private enum SwipeDirection
		{
			// Token: 0x04000FB0 RID: 4016
			Disabled,
			// Token: 0x04000FB1 RID: 4017
			Up,
			// Token: 0x04000FB2 RID: 4018
			Down,
			// Token: 0x04000FB3 RID: 4019
			Left,
			// Token: 0x04000FB4 RID: 4020
			Right
		}
	}
}
