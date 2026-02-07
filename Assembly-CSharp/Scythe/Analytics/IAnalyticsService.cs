using System;
using System.Collections.Generic;

namespace Scythe.Analytics
{
	// Token: 0x0200065C RID: 1628
	public interface IAnalyticsService
	{
		// Token: 0x060033A9 RID: 13225
		void LogEvent(AnalyticsEventTypes eventType, IDictionary<string, object> headerProperties, IDictionary<string, object> eventProperties);

		// Token: 0x060033AA RID: 13226
		void UpdateUserProperties(IDictionary<string, object> userProperties);

		// Token: 0x060033AB RID: 13227
		void SetUserId(string userId);
	}
}
