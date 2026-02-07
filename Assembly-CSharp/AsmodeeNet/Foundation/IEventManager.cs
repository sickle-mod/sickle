using System;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000941 RID: 2369
	public interface IEventManager
	{
		// Token: 0x06003F9F RID: 16287
		void QueueEvent(Action action);

		// Token: 0x06003FA0 RID: 16288
		void QueueEvent<T>(Action<T> action, T parameter);

		// Token: 0x06003FA1 RID: 16289
		void QueueEvent<T1, T2>(Action<T1, T2> action, T1 parameter1, T2 parameter2);
	}
}
