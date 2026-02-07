using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x0200015E RID: 350
[Serializable]
public abstract class SerializableHashSet<T> : SerializableHashSetBase, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x06000A32 RID: 2610 RVA: 0x0002EF06 File Offset: 0x0002D106
	public SerializableHashSet()
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>();
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0002EF19 File Offset: 0x0002D119
	public SerializableHashSet(ISet<T> set)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(set);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0007CD00 File Offset: 0x0007AF00
	public void CopyFrom(ISet<T> set)
	{
		this.m_hashSet.Clear();
		foreach (T t in set)
		{
			this.m_hashSet.Add(t);
		}
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0007CD5C File Offset: 0x0007AF5C
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null)
		{
			this.m_hashSet.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_hashSet.Add(this.m_keys[i]);
			}
			this.m_keys = null;
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0007CDB0 File Offset: 0x0007AFB0
	public void OnBeforeSerialize()
	{
		int count = this.m_hashSet.Count;
		this.m_keys = new T[count];
		int num = 0;
		foreach (T t in this.m_hashSet)
		{
			this.m_keys[num] = t;
			num++;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002EF2D File Offset: 0x0002D12D
	public int Count
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).Count;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002EF3A File Offset: 0x0002D13A
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).IsReadOnly;
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0002EF47 File Offset: 0x0002D147
	public bool Add(T item)
	{
		return ((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0002EF55 File Offset: 0x0002D155
	public void ExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).ExceptWith(other);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0002EF63 File Offset: 0x0002D163
	public void IntersectWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).IntersectWith(other);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0002EF71 File Offset: 0x0002D171
	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSubsetOf(other);
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0002EF7F File Offset: 0x0002D17F
	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSupersetOf(other);
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0002EF8D File Offset: 0x0002D18D
	public bool IsSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSubsetOf(other);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0002EF9B File Offset: 0x0002D19B
	public bool IsSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSupersetOf(other);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0002EFA9 File Offset: 0x0002D1A9
	public bool Overlaps(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).Overlaps(other);
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0002EFB7 File Offset: 0x0002D1B7
	public bool SetEquals(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).SetEquals(other);
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0002EFC5 File Offset: 0x0002D1C5
	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).SymmetricExceptWith(other);
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0002EFD3 File Offset: 0x0002D1D3
	public void UnionWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).UnionWith(other);
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0002EFE1 File Offset: 0x0002D1E1
	void ICollection<T>.Add(T item)
	{
		((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0002EFF0 File Offset: 0x0002D1F0
	public void Clear()
	{
		((ICollection<T>)this.m_hashSet).Clear();
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0002EFFD File Offset: 0x0002D1FD
	public bool Contains(T item)
	{
		return ((ICollection<T>)this.m_hashSet).Contains(item);
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0002F00B File Offset: 0x0002D20B
	public void CopyTo(T[] array, int arrayIndex)
	{
		((ICollection<T>)this.m_hashSet).CopyTo(array, arrayIndex);
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0002F01A File Offset: 0x0002D21A
	public bool Remove(T item)
	{
		return ((ICollection<T>)this.m_hashSet).Remove(item);
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x0002F028 File Offset: 0x0002D228
	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0002F028 File Offset: 0x0002D228
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0002F035 File Offset: 0x0002D235
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_hashSet).OnDeserialization(sender);
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0002F043 File Offset: 0x0002D243
	protected SerializableHashSet(SerializationInfo info, StreamingContext context)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(info, context);
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0002F058 File Offset: 0x0002D258
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_hashSet).GetObjectData(info, context);
	}

	// Token: 0x040008F3 RID: 2291
	private SerializableHashSetBase.HashSet<T> m_hashSet;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	private T[] m_keys;
}
