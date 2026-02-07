using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000159 RID: 345
[Serializable]
public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
	// Token: 0x06000A23 RID: 2595 RVA: 0x0002EE6F File Offset: 0x0002D06F
	public SerializableDictionary()
	{
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0002EE77 File Offset: 0x0002D077
	public SerializableDictionary(IDictionary<TKey, TValue> dict)
		: base(dict)
	{
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002EE80 File Offset: 0x0002D080
	protected SerializableDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002EE8A File Offset: 0x0002D08A
	protected override TValue GetValue(TValue[] storage, int i)
	{
		return storage[i];
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0002EE93 File Offset: 0x0002D093
	protected override void SetValue(TValue[] storage, int i, TValue value)
	{
		storage[i] = value;
	}
}
