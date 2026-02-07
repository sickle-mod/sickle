using System;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000865 RID: 2149
	public static class TimeSpanExtension
	{
		// Token: 0x06003C6F RID: 15471 RVA: 0x00155528 File Offset: 0x00153728
		public static string ToStringExtended(this TimeSpan timeSpan, string format)
		{
			TimeSpan timeSpan2 = TimeSpan.FromSeconds(timeSpan.TotalSeconds);
			string text = string.Empty;
			if (timeSpan2.TotalDays >= 7.0)
			{
				int num = (int)timeSpan2.TotalDays / 7;
				int num2 = num;
				text = num2.ToString() + " week" + ((num >= 2) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromDays((double)(num * 7)));
			}
			if (timeSpan2.TotalDays >= 1.0)
			{
				text = ((int)timeSpan2.TotalDays).ToString() + " day" + ((timeSpan2.TotalDays >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromDays((double)((int)timeSpan2.TotalDays)));
			}
			if (timeSpan2.TotalHours >= 1.0)
			{
				text = ((int)timeSpan2.TotalHours).ToString() + " hour" + ((timeSpan2.TotalHours >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromHours((double)((int)timeSpan2.TotalHours)));
			}
			if (timeSpan2.TotalMinutes >= 1.0)
			{
				text = ((int)timeSpan2.TotalMinutes).ToString() + " minute" + ((timeSpan2.TotalMinutes >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromMinutes((double)((int)timeSpan2.TotalMinutes)));
			}
			if (timeSpan2.TotalSeconds >= 1.0)
			{
				text = ((int)timeSpan2.TotalSeconds).ToString() + " second" + ((timeSpan2.TotalSeconds >= 2.0) ? "s" : "");
			}
			return text;
		}
	}
}
