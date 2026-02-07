using System;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000940 RID: 2368
	public class EventManager : MonoBehaviour, IEventManager
	{
		// Token: 0x06003F98 RID: 16280 RVA: 0x00050D47 File Offset: 0x0004EF47
		private void OnEnable()
		{
			if (EventManager.Instance == null)
			{
				EventManager.Instance = this;
			}
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0015C9C8 File Offset: 0x0015ABC8
		public void QueueEvent(Action action)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, null));
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0015CA14 File Offset: 0x0015AC14
		public void QueueEvent<T>(Action<T> action, T parameter)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, parameter));
			}
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0015CA68 File Offset: 0x0015AC68
		public void QueueEvent<T1, T2>(Action<T1, T2> action, T1 parameter1, T2 parameter2)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, new object[] { parameter1, parameter2 }));
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0015CACC File Offset: 0x0015ACCC
		private void Update()
		{
			this.MoveQueuedEventsToExecuting();
			while (this._executingEvents.Count > 0)
			{
				AsmodeeNet.Utils.Tuple<Delegate, object> tuple = this._executingEvents[0];
				this._executingEvents.RemoveAt(0);
				try
				{
					if (tuple.Item2 != null && tuple.Item2 is Array)
					{
						Array array = tuple.Item2 as Array;
						if (array.Length == 2)
						{
							tuple.Item1.DynamicInvoke(new object[]
							{
								array.GetValue(0),
								array.GetValue(1)
							});
						}
						else if (array.Length == 3)
						{
							tuple.Item1.DynamicInvoke(new object[]
							{
								array.GetValue(0),
								array.GetValue(1),
								array.GetValue(2)
							});
						}
					}
					else if (tuple.Item2 != null)
					{
						tuple.Item1.DynamicInvoke(new object[] { tuple.Item2 });
					}
					else
					{
						tuple.Item1.DynamicInvoke(Array.Empty<object>());
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					throw;
				}
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0015CBF0 File Offset: 0x0015ADF0
		private void MoveQueuedEventsToExecuting()
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				while (this._queuedEvents.Count > 0)
				{
					AsmodeeNet.Utils.Tuple<Delegate, object> tuple = this._queuedEvents[0];
					this._executingEvents.Add(tuple);
					this._queuedEvents.RemoveAt(0);
				}
			}
		}

		// Token: 0x040030AE RID: 12462
		private const string _debugModuleName = "EventManager";

		// Token: 0x040030AF RID: 12463
		public static IEventManager Instance;

		// Token: 0x040030B0 RID: 12464
		private object _queueLock = new object();

		// Token: 0x040030B1 RID: 12465
		private List<AsmodeeNet.Utils.Tuple<Delegate, object>> _queuedEvents = new List<AsmodeeNet.Utils.Tuple<Delegate, object>>();

		// Token: 0x040030B2 RID: 12466
		private List<AsmodeeNet.Utils.Tuple<Delegate, object>> _executingEvents = new List<AsmodeeNet.Utils.Tuple<Delegate, object>>();
	}
}
