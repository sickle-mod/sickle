using System;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000861 RID: 2145
	public static class DateTimeExtension
	{
		// Token: 0x06003C65 RID: 15461 RVA: 0x001552B0 File Offset: 0x001534B0
		public static DateTime UnixTimeStampToDateTime(this DateTime dateTime, double unixTimeStamp)
		{
			DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime2 = dateTime2.AddMilliseconds(unixTimeStamp).ToLocalTime();
			return dateTime2;
		}
	}
}
