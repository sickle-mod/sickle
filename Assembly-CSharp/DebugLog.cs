using System;
using UnityEngine;

// Token: 0x0200012B RID: 299
public static class DebugLog
{
	// Token: 0x06000924 RID: 2340 RVA: 0x0002E3BA File Offset: 0x0002C5BA
	public static void Log(string s)
	{
		Debug.Log(s);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0002E3C2 File Offset: 0x0002C5C2
	public static void LogWarning(string s)
	{
		Debug.LogWarning(s);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0002E3CA File Offset: 0x0002C5CA
	public static void LogError(string s)
	{
		Debug.LogError(s);
	}

	// Token: 0x04000860 RID: 2144
	public const bool IS_ON = true;
}
