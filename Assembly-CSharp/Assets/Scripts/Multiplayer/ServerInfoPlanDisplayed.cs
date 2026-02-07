using System;

namespace Assets.Scripts.Multiplayer
{
	// Token: 0x020001AE RID: 430
	public class ServerInfoPlanDisplayed
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x000305A2 File Offset: 0x0002E7A2
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x000305AA File Offset: 0x0002E7AA
		public bool InfoPlanDisplayed { get; set; }

		// Token: 0x06000C8B RID: 3211 RVA: 0x000305B3 File Offset: 0x0002E7B3
		public static ServerInfoPlanDisplayed GetInstance()
		{
			if (ServerInfoPlanDisplayed._instance == null)
			{
				ServerInfoPlanDisplayed._instance = new ServerInfoPlanDisplayed();
			}
			return ServerInfoPlanDisplayed._instance;
		}

		// Token: 0x040009DF RID: 2527
		private static ServerInfoPlanDisplayed _instance;
	}
}
