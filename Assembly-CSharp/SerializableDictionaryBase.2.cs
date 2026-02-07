using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000156 RID: 342
[Serializable]
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : SerializableDictionaryBase, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x060009FB RID: 2555 RVA: 0x0002EC8B File Offset: 0x0002CE8B
	public SerializableDictionaryBase()
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0002EC9E File Offset: 0x0002CE9E
	public SerializableDictionaryBase(IDictionary<TKey, TValue> dict)
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>(dict);
	}

	// Token: 0x060009FD RID: 2557
	protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);

	// Token: 0x060009FE RID: 2558
	protected abstract TValue GetValue(TValueStorage[] storage, int i);

	// Token: 0x060009FF RID: 2559 RVA: 0x0007CB74 File Offset: 0x0007AD74
	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		this.m_dict.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			this.m_dict[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0007CBDC File Offset: 0x0007ADDC
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null && this.m_values != null && this.m_keys.Length == this.m_values.Length)
		{
			this.m_dict.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_dict[this.m_keys[i]] = this.GetValue(this.m_values, i);
			}
			this.m_keys = null;
			this.m_values = null;
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0007CC60 File Offset: 0x0007AE60
	public void OnBeforeSerialize()
	{
		int count = this.m_dict.Count;
		this.m_keys = new TKey[count];
		this.m_values = new TValueStorage[count];
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this.m_dict)
		{
			this.m_keys[num] = keyValuePair.Key;
			this.SetValue(this.m_values, num, keyValuePair.Value);
			num++;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0002ECB2 File Offset: 0x0002CEB2
	public ICollection<TKey> Keys
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Keys;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002ECBF File Offset: 0x0002CEBF
	public ICollection<TValue> Values
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Values;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0002ECCC File Offset: 0x0002CECC
	public int Count
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Count;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002ECD9 File Offset: 0x0002CED9
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).IsReadOnly;
		}
	}

	// Token: 0x17000098 RID: 152
	public TValue this[TKey key]
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict)[key];
		}
		set
		{
			((IDictionary<TKey, TValue>)this.m_dict)[key] = value;
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0002ED03 File Offset: 0x0002CF03
	public void Add(TKey key, TValue value)
	{
		((IDictionary<TKey, TValue>)this.m_dict).Add(key, value);
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0002ED12 File Offset: 0x0002CF12
	public bool ContainsKey(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).ContainsKey(key);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0002ED20 File Offset: 0x0002CF20
	public bool Remove(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).Remove(key);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0002ED2E File Offset: 0x0002CF2E
	public bool TryGetValue(TKey key, out TValue value)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).TryGetValue(key, out value);
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0002ED3D File Offset: 0x0002CF3D
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Add(item);
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002ED4B File Offset: 0x0002CF4B
	public void Clear()
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Clear();
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0002ED58 File Offset: 0x0002CF58
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Contains(item);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0002ED66 File Offset: 0x0002CF66
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).CopyTo(array, arrayIndex);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002ED75 File Offset: 0x0002CF75
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Remove(item);
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002ED83 File Offset: 0x0002CF83
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002ED83 File Offset: 0x0002CF83
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002ED90 File Offset: 0x0002CF90
	public bool IsFixedSize
	{
		get
		{
			return ((IDictionary)this.m_dict).IsFixedSize;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002ED9D File Offset: 0x0002CF9D
	ICollection IDictionary.Keys
	{
		get
		{
			return ((IDictionary)this.m_dict).Keys;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002EDAA File Offset: 0x0002CFAA
	ICollection IDictionary.Values
	{
		get
		{
			return ((IDictionary)this.m_dict).Values;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002EDB7 File Offset: 0x0002CFB7
	public bool IsSynchronized
	{
		get
		{
			return ((ICollection)this.m_dict).IsSynchronized;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002EDC4 File Offset: 0x0002CFC4
	public object SyncRoot
	{
		get
		{
			return ((ICollection)this.m_dict).SyncRoot;
		}
	}

	// Token: 0x1700009E RID: 158
	public object this[object key]
	{
		get
		{
			return ((IDictionary)this.m_dict)[key];
		}
		set
		{
			((IDictionary)this.m_dict)[key] = value;
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0002EDEE File Offset: 0x0002CFEE
	public void Add(object key, object value)
	{
		((IDictionary)this.m_dict).Add(key, value);
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0002EDFD File Offset: 0x0002CFFD
	public bool Contains(object key)
	{
		return ((IDictionary)this.m_dict).Contains(key);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0002EE0B File Offset: 0x0002D00B
	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		return ((IDictionary)this.m_dict).GetEnumerator();
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0002EE18 File Offset: 0x0002D018
	public void Remove(object key)
	{
		((IDictionary)this.m_dict).Remove(key);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0002EE26 File Offset: 0x0002D026
	public void CopyTo(Array array, int index)
	{
		((ICollection)this.m_dict).CopyTo(array, index);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0002EE35 File Offset: 0x0002D035
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_dict).OnDeserialization(sender);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0002EE43 File Offset: 0x0002D043
	protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context)
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>(info, context);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0002EE58 File Offset: 0x0002D058
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_dict).GetObjectData(info, context);
	}

	// Token: 0x040008EF RID: 2287
	private SerializableDictionaryBase.Dictionary<TKey, TValue> m_dict;

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private TKey[] m_keys;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private TValueStorage[] m_values;
}
