using System;
using System.Collections.Generic;
using System.Linq;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000956 RID: 2390
	public class NotificationCenter
	{
		// Token: 0x06004038 RID: 16440 RVA: 0x0015D8F8 File Offset: 0x0015BAF8
		public void AddObserver(object observer, string notificationName, NotificationHandler action, object subject = null)
		{
			NotificationCenter.NotificationEntry notificationEntry;
			if (!this._notificationNameToEntry.TryGetValue(notificationName, out notificationEntry))
			{
				notificationEntry = new NotificationCenter.NotificationEntry(notificationName);
				this._notificationNameToEntry[notificationName] = notificationEntry;
			}
			NotificationCenter.NotificationInvoker notificationInvoker = new NotificationCenter.NotificationInvoker(notificationEntry, subject, observer, action);
			notificationEntry.AddInvoker(notificationInvoker);
			if (!this._observerToInvokers.ContainsKey(observer))
			{
				this._observerToInvokers.Add(observer, new List<NotificationCenter.NotificationInvoker>());
			}
			this._observerToInvokers[observer].Add(notificationInvoker);
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0015D96C File Offset: 0x0015BB6C
		public void RemoveObserver(object observer)
		{
			List<NotificationCenter.NotificationInvoker> list;
			if (this._observerToInvokers.TryGetValue(observer, out list))
			{
				foreach (NotificationCenter.NotificationInvoker notificationInvoker in list)
				{
					notificationInvoker.entry.RemoveInvoker(notificationInvoker);
					if (notificationInvoker.entry.IsEmpty())
					{
						this._notificationNameToEntry.Remove(notificationInvoker.entry.NotificationName);
					}
				}
				this._observerToInvokers.Remove(observer);
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0015DA00 File Offset: 0x0015BC00
		public void RemoveObserver(object observer, string notificationName, object subject = null)
		{
			List<NotificationCenter.NotificationInvoker> list;
			if (this._observerToInvokers.TryGetValue(observer, out list))
			{
				List<NotificationCenter.NotificationInvoker> list2 = new List<NotificationCenter.NotificationInvoker>();
				foreach (NotificationCenter.NotificationInvoker notificationInvoker in list)
				{
					if ((subject == null || notificationInvoker.subject == subject) && notificationInvoker.entry != null && notificationInvoker.entry.NotificationName == notificationName)
					{
						notificationInvoker.entry.RemoveInvoker(notificationInvoker);
						if (notificationInvoker.entry.IsEmpty())
						{
							this._notificationNameToEntry.Remove(notificationInvoker.entry.NotificationName);
						}
						list2.Add(notificationInvoker);
					}
				}
				foreach (NotificationCenter.NotificationInvoker notificationInvoker2 in list2)
				{
					list.Remove(notificationInvoker2);
				}
				if (!list.Any<NotificationCenter.NotificationInvoker>())
				{
					this._observerToInvokers.Remove(observer);
				}
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x000514A6 File Offset: 0x0004F6A6
		public void RemoveAllObservers()
		{
			this._notificationNameToEntry.Clear();
			this._observerToInvokers.Clear();
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0015DB14 File Offset: 0x0015BD14
		public bool HasObserver(object observer, string notificationName, object subject = null)
		{
			List<NotificationCenter.NotificationInvoker> list;
			if (this._observerToInvokers.TryGetValue(observer, out list))
			{
				foreach (NotificationCenter.NotificationInvoker notificationInvoker in list)
				{
					if ((subject == null || notificationInvoker.subject == subject) && notificationInvoker.entry != null && notificationInvoker.entry.NotificationName == notificationName)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x000514BE File Offset: 0x0004F6BE
		public void PostNotification(string notificationName, object subject = null)
		{
			this.PostNotification(new Notification(notificationName, subject));
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0015DB9C File Offset: 0x0015BD9C
		public void PostNotification(Notification notification)
		{
			NotificationCenter.NotificationEntry notificationEntry;
			if (this._notificationNameToEntry.TryGetValue(notification.Name, out notificationEntry))
			{
				notificationEntry.PostNotification(notification, notification.Subject);
			}
		}

		// Token: 0x040030F1 RID: 12529
		private Dictionary<string, NotificationCenter.NotificationEntry> _notificationNameToEntry = new Dictionary<string, NotificationCenter.NotificationEntry>();

		// Token: 0x040030F2 RID: 12530
		private Dictionary<object, List<NotificationCenter.NotificationInvoker>> _observerToInvokers = new Dictionary<object, List<NotificationCenter.NotificationInvoker>>();

		// Token: 0x02000957 RID: 2391
		private class NotificationInvoker
		{
			// Token: 0x06004040 RID: 16448 RVA: 0x000514EB File Offset: 0x0004F6EB
			public NotificationInvoker(NotificationCenter.NotificationEntry entry, object subject, object observer, NotificationHandler action)
			{
				this.entry = entry;
				this.subject = subject;
				this.observer = observer;
				this.action = action;
			}

			// Token: 0x06004041 RID: 16449 RVA: 0x00051510 File Offset: 0x0004F710
			public void Invoke(Notification notification)
			{
				if (this.action != null)
				{
					this.action(notification);
				}
			}

			// Token: 0x040030F3 RID: 12531
			public NotificationCenter.NotificationEntry entry;

			// Token: 0x040030F4 RID: 12532
			public object subject;

			// Token: 0x040030F5 RID: 12533
			public object observer;

			// Token: 0x040030F6 RID: 12534
			public NotificationHandler action;
		}

		// Token: 0x02000958 RID: 2392
		private class NotificationEntry
		{
			// Token: 0x170005FA RID: 1530
			// (get) Token: 0x06004042 RID: 16450 RVA: 0x00051526 File Offset: 0x0004F726
			// (set) Token: 0x06004043 RID: 16451 RVA: 0x0005152E File Offset: 0x0004F72E
			public string NotificationName { get; private set; }

			// Token: 0x06004044 RID: 16452 RVA: 0x00051537 File Offset: 0x0004F737
			public NotificationEntry(string notificationName)
			{
				this.NotificationName = notificationName;
			}

			// Token: 0x06004045 RID: 16453 RVA: 0x0005155C File Offset: 0x0004F75C
			public bool IsEmpty()
			{
				return !this._subjectToInvokers.Any<KeyValuePair<object, List<NotificationCenter.NotificationInvoker>>>() && !this._globalInvokers.Any<NotificationCenter.NotificationInvoker>();
			}

			// Token: 0x06004046 RID: 16454 RVA: 0x0015DBCC File Offset: 0x0015BDCC
			public void AddInvoker(NotificationCenter.NotificationInvoker invoker)
			{
				if (invoker.subject != null)
				{
					List<NotificationCenter.NotificationInvoker> list;
					if (!this._subjectToInvokers.TryGetValue(invoker.subject, out list))
					{
						list = new List<NotificationCenter.NotificationInvoker>();
						this._subjectToInvokers[invoker.subject] = list;
					}
					list.Add(invoker);
					return;
				}
				this._globalInvokers.Add(invoker);
			}

			// Token: 0x06004047 RID: 16455 RVA: 0x0015DC24 File Offset: 0x0015BE24
			public void RemoveInvoker(NotificationCenter.NotificationInvoker invoker)
			{
				if (invoker.subject != null)
				{
					List<NotificationCenter.NotificationInvoker> list;
					if (this._subjectToInvokers.TryGetValue(invoker.subject, out list))
					{
						list.Remove(invoker);
						if (!list.Any<NotificationCenter.NotificationInvoker>())
						{
							this._subjectToInvokers.Remove(invoker.subject);
							return;
						}
					}
				}
				else
				{
					this._globalInvokers.Remove(invoker);
				}
			}

			// Token: 0x06004048 RID: 16456 RVA: 0x0015DC80 File Offset: 0x0015BE80
			public void PostNotification(Notification notification, object subject)
			{
				List<NotificationCenter.NotificationInvoker> list;
				if (subject != null && this._subjectToInvokers.TryGetValue(subject, out list))
				{
					foreach (NotificationCenter.NotificationInvoker notificationInvoker in new List<NotificationCenter.NotificationInvoker>(list))
					{
						notificationInvoker.Invoke(notification);
					}
				}
				foreach (NotificationCenter.NotificationInvoker notificationInvoker2 in new List<NotificationCenter.NotificationInvoker>(this._globalInvokers))
				{
					notificationInvoker2.Invoke(notification);
				}
			}

			// Token: 0x040030F8 RID: 12536
			private Dictionary<object, List<NotificationCenter.NotificationInvoker>> _subjectToInvokers = new Dictionary<object, List<NotificationCenter.NotificationInvoker>>();

			// Token: 0x040030F9 RID: 12537
			private List<NotificationCenter.NotificationInvoker> _globalInvokers = new List<NotificationCenter.NotificationInvoker>();
		}
	}
}
