using System;
using System.Collections.Generic;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009DB RID: 2523
	public class AmplitudeSDK : IAnalyticsService
	{
		// Token: 0x06004237 RID: 16951 RVA: 0x00052653 File Offset: 0x00050853
		public AmplitudeSDK(string apiKey)
		{
			Amplitude.Instance.init(apiKey);
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00052666 File Offset: 0x00050866
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			Amplitude.Instance.logEvent(eventType, eventProperties);
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0004804F File Offset: 0x0004624F
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			Amplitude.Instance.setUserProperties(userProperties);
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0004805C File Offset: 0x0004625C
		public void SetUserId(string userId)
		{
			Amplitude.Instance.setUserId(userId);
		}
	}
}
