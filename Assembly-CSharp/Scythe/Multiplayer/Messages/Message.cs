using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000304 RID: 772
	[JsonObject(MemberSerialization.Fields)]
	public abstract class Message
	{
		// Token: 0x06001676 RID: 5750 RVA: 0x00037658 File Offset: 0x00035858
		public int GetCounter()
		{
			return this.counter;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00037660 File Offset: 0x00035860
		public void SetCounter(int counter)
		{
			this.counter = counter;
		}

		// Token: 0x04001088 RID: 4232
		private int counter;
	}
}
