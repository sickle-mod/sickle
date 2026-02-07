using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000324 RID: 804
	public class InGameSlot
	{
		// Token: 0x060016FF RID: 5887 RVA: 0x00037B43 File Offset: 0x00035D43
		public InGameSlot()
		{
			this.isTaken = false;
			this.isTakenByAI = false;
			this.playerId = -1;
			this.canAddBot = false;
			this.canRemoveBot = false;
			this.name = string.Empty;
		}

		// Token: 0x040010F5 RID: 4341
		public bool isTaken;

		// Token: 0x040010F6 RID: 4342
		public bool isTakenByAI;

		// Token: 0x040010F7 RID: 4343
		public int playerId;

		// Token: 0x040010F8 RID: 4344
		public bool canAddBot;

		// Token: 0x040010F9 RID: 4345
		public bool canRemoveBot;

		// Token: 0x040010FA RID: 4346
		public string name;
	}
}
