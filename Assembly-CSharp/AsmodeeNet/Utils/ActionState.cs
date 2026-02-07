using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000854 RID: 2132
	[Serializable]
	public class ActionState
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x0004EED6 File Offset: 0x0004D0D6
		// (set) Token: 0x06003C28 RID: 15400 RVA: 0x0004EEDE File Offset: 0x0004D0DE
		public string Name { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x0004EEE7 File Offset: 0x0004D0E7
		// (set) Token: 0x06003C2A RID: 15402 RVA: 0x0004EEEF File Offset: 0x0004D0EF
		public Action ActionEnter { get; set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x0004EEF8 File Offset: 0x0004D0F8
		// (set) Token: 0x06003C2C RID: 15404 RVA: 0x0004EF00 File Offset: 0x0004D100
		public Action ActionUpdate { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x0004EF09 File Offset: 0x0004D109
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x0004EF11 File Offset: 0x0004D111
		public Action ActionExit { get; set; }

		// Token: 0x06003C2F RID: 15407 RVA: 0x0004EF1A File Offset: 0x0004D11A
		public ActionState(string name, Action actionEnter, Action actionUpdate, Action actionExit)
		{
			this.Name = name;
			this.ActionEnter = actionEnter;
			this.ActionUpdate = actionUpdate;
			this.ActionExit = actionExit;
		}
	}
}
