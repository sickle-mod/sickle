using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000849 RID: 2121
	public static class Format
	{
		// Token: 0x06003BF9 RID: 15353 RVA: 0x0004ED48 File Offset: 0x0004CF48
		public static string HexStringFromString(string str)
		{
			return Format.HexStringFromUInt64(ulong.Parse(str));
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0004ED55 File Offset: 0x0004CF55
		public static string HexStringFromUInt64(ulong i)
		{
			return string.Format("{0:X}", i).ToLower();
		}
	}
}
