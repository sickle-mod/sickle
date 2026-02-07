using System;
using Newtonsoft.Json;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000954 RID: 2388
	[JsonObject(MemberSerialization.Fields)]
	public class Notification
	{
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x0005146E File Offset: 0x0004F66E
		// (set) Token: 0x06004030 RID: 16432 RVA: 0x00051476 File Offset: 0x0004F676
		public string Name { get; private set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x0005147F File Offset: 0x0004F67F
		// (set) Token: 0x06004032 RID: 16434 RVA: 0x00051487 File Offset: 0x0004F687
		public object Subject { get; private set; }

		// Token: 0x06004033 RID: 16435 RVA: 0x00051490 File Offset: 0x0004F690
		public Notification(string name, object subject = null)
		{
			this.Name = name;
			this.Subject = subject;
		}
	}
}
