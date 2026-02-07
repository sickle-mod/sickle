using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000153 RID: 339
public abstract class SerializableDictionaryBase
{
	// Token: 0x02000154 RID: 340
	public abstract class Storage
	{
	}

	// Token: 0x02000155 RID: 341
	protected class Dictionary<TKey, TValue> : global::System.Collections.Generic.Dictionary<TKey, TValue>
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x0002EC70 File Offset: 0x0002CE70
		public Dictionary()
		{
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002EC78 File Offset: 0x0002CE78
		public Dictionary(IDictionary<TKey, TValue> dict)
			: base(dict)
		{
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002EC81 File Offset: 0x0002CE81
		public Dictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
