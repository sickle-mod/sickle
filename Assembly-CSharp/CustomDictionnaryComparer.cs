using System;
using System.Collections.Generic;

// Token: 0x02000005 RID: 5
public class CustomDictionnaryComparer : IEqualityComparer<KeyValuePair<string, string>>
{
	// Token: 0x06000012 RID: 18 RVA: 0x00027E33 File Offset: 0x00026033
	public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
	{
		return x.Key.Equals(y.Key);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00027E48 File Offset: 0x00026048
	public int GetHashCode(KeyValuePair<string, string> obj)
	{
		return obj.Key.GetHashCode();
	}
}
