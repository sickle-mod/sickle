using System;
using Scythe.Platforms;

// Token: 0x02000062 RID: 98
public static class GameServiceFactory
{
	// Token: 0x06000350 RID: 848 RVA: 0x0002A2F3 File Offset: 0x000284F3
	public static IGameService GetGameServiceForCurrentPlatform()
	{
		return new SteamService();
	}
}
