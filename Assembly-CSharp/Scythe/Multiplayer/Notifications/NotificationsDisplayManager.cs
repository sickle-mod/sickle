using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x020002A2 RID: 674
	public class NotificationsDisplayManager : MonoBehaviour
	{
		// Token: 0x06001560 RID: 5472 RVA: 0x000367A5 File Offset: 0x000349A5
		private void Awake()
		{
			this.notificationsToDisplay = new Queue<Notification>();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x000367B2 File Offset: 0x000349B2
		public void EnqueueNotificationForDisplay(Notification newNotification)
		{
			this.notificationsToDisplay.Enqueue(newNotification);
			this.CheckIfDisplayAnotherNotification();
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x000367C6 File Offset: 0x000349C6
		private void CheckIfDisplayAnotherNotification()
		{
			if (!GenericSingletonClass<NotificationLogic>.Instance.CanDisplay())
			{
				base.Invoke("CheckIfDisplayAnotherNotification", 1f);
				return;
			}
			if (this.displaying)
			{
				return;
			}
			if (this.notificationsToDisplay.Count == 0)
			{
				return;
			}
			this.DisplayNotification();
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0009D648 File Offset: 0x0009B848
		private void DisplayNotification()
		{
			this.currentNotification = this.notificationsToDisplay.Dequeue();
			if (GenericSingletonClass<NotificationLogic>.Instance.IsClickable(this.currentNotification))
			{
				this.ChooseNotificationDisplay(true).DisplayNotification(this.currentNotification);
			}
			else
			{
				this.ChooseNotificationDisplay(false).DisplayNotification(this.currentNotification);
			}
			this.displaying = true;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0009D6A8 File Offset: 0x0009B8A8
		private NotificationUI ChooseNotificationDisplay(bool isClickable)
		{
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MAIN_NAME)
			{
				if (!isClickable)
				{
					return this.ingameInfoNotificationUI;
				}
				return this.ingameClickableNotificationUI;
			}
			else
			{
				if (!isClickable)
				{
					return this.defaultInfoNotificationUI;
				}
				return this.defaultClickableNotificationUI;
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00036802 File Offset: 0x00034A02
		public void NotificationDisplayFinished()
		{
			this.displaying = false;
			this.CheckIfDisplayAnotherNotification();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00036811 File Offset: 0x00034A11
		public void NotificationClicked()
		{
			GenericSingletonClass<NotificationLogic>.Instance.NotificationClicked(this.currentNotification);
		}

		// Token: 0x04000F95 RID: 3989
		private Queue<Notification> notificationsToDisplay;

		// Token: 0x04000F96 RID: 3990
		[SerializeField]
		private NotificationUI defaultInfoNotificationUI;

		// Token: 0x04000F97 RID: 3991
		[SerializeField]
		private NotificationUI defaultClickableNotificationUI;

		// Token: 0x04000F98 RID: 3992
		[SerializeField]
		private NotificationUI ingameInfoNotificationUI;

		// Token: 0x04000F99 RID: 3993
		[SerializeField]
		private NotificationUI ingameClickableNotificationUI;

		// Token: 0x04000F9A RID: 3994
		private bool displaying;

		// Token: 0x04000F9B RID: 3995
		private Notification currentNotification;
	}
}
