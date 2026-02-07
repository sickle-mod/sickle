using System;
using Scythe.Multiplayer;
using UnityEngine;

// Token: 0x0200011E RID: 286
public static class BuildVersionUtility
{
	// Token: 0x06000903 RID: 2307 RVA: 0x0002E23C File Offset: 0x0002C43C
	public static string GetBuildVersionWithEndpoint()
	{
		return BuildVersionUtility.GetBuildVersion() + " (" + BuildVersionUtility.GetEndpoint() + ")";
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0002E257 File Offset: 0x0002C457
	public static string GetBuildVersion()
	{
		return Application.version;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0007AA60 File Offset: 0x00078C60
	public static string GetEndpoint()
	{
		return ServerEndpoints.EndpointType.ToString();
	}
}
