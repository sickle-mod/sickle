using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200015A RID: 346
[Serializable]
public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
	// Token: 0x06000A28 RID: 2600 RVA: 0x0002EE9D File Offset: 0x0002D09D
	public SerializableDictionary()
	{
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002EEA5 File Offset: 0x0002D0A5
	public SerializableDictionary(IDictionary<TKey, TValue> dict)
		: base(dict)
	{
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0002EEAE File Offset: 0x0002D0AE
	protected SerializableDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0002EEB8 File Offset: 0x0002D0B8
	protected override TValue GetValue(TValueStorage[] storage, int i)
	{
		return storage[i].data;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0002EECB File Offset: 0x0002D0CB
	protected override void SetValue(TValueStorage[] storage, int i, TValue value)
	{
		storage[i] = new TValueStorage();
		storage[i].data = value;
	}
}
