using System;
using System.Collections.Generic;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009DC RID: 2524
	public interface IAnalyticsService
	{
		// Token: 0x0600423B RID: 16955
		void LogEvent(string eventType, IDictionary<string, object> eventProperties);

		// Token: 0x0600423C RID: 16956
		void UpdateUserProperties(IDictionary<string, object> userProperties);

		// Token: 0x0600423D RID: 16957
		void SetUserId(string userId);
	}
}
