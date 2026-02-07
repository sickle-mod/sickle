using System;
using System.Collections.Generic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000310 RID: 784
	public class Action : Game
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x0003792E File Offset: 0x00035B2E
		// (set) Token: 0x060016C4 RID: 5828 RVA: 0x00037936 File Offset: 0x00035B36
		public List<string> Actions { get; set; }

		// Token: 0x060016C5 RID: 5829 RVA: 0x0003793F File Offset: 0x00035B3F
		public Action()
		{
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00037947 File Offset: 0x00035B47
		public Action(string message)
		{
			this.Actions = new List<string> { message };
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00037961 File Offset: 0x00035B61
		public Action(List<string> messages)
		{
			this.Actions = messages;
		}
	}
}
