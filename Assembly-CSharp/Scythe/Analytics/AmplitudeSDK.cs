using System;
using System.Collections.Generic;

namespace Scythe.Analytics
{
	// Token: 0x0200063B RID: 1595
	public class AmplitudeSDK : IAnalyticsService
	{
		// Token: 0x060032B6 RID: 12982 RVA: 0x00048017 File Offset: 0x00046217
		public AmplitudeSDK(string apiKey)
		{
			Amplitude.Instance.init(apiKey);
			Amplitude.Instance.logging = false;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00048035 File Offset: 0x00046235
		public void LogEvent(AnalyticsEventTypes eventType, IDictionary<string, object> headerProperties, IDictionary<string, object> eventProperties)
		{
			Amplitude.Instance.logEvent(eventType.ToString(), eventProperties);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x0004804F File Offset: 0x0004624F
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			Amplitude.Instance.setUserProperties(userProperties);
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x0004805C File Offset: 0x0004625C
		public void SetUserId(string userId)
		{
			Amplitude.Instance.setUserId(userId);
		}
	}
}
