using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200015B RID: 347
public abstract class SerializableHashSetBase
{
	// Token: 0x0200015C RID: 348
	public abstract class Storage
	{
	}

	// Token: 0x0200015D RID: 349
	protected class HashSet<TValue> : global::System.Collections.Generic.HashSet<TValue>
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x0002EEEB File Offset: 0x0002D0EB
		public HashSet()
		{
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002EEF3 File Offset: 0x0002D0F3
		public HashSet(ISet<TValue> set)
			: base(set)
		{
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002EEFC File Offset: 0x0002D0FC
		public HashSet(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
