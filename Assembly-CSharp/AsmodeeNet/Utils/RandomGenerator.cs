using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000851 RID: 2129
	public abstract class RandomGenerator
	{
		// Token: 0x06003C09 RID: 15369
		public abstract uint Rand();

		// Token: 0x06003C0A RID: 15370
		public abstract uint Range(uint min, uint max);

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06003C0B RID: 15371 RVA: 0x0004EDD1 File Offset: 0x0004CFD1
		// (set) Token: 0x06003C0C RID: 15372 RVA: 0x0004EDD9 File Offset: 0x0004CFD9
		public uint Seed
		{
			get
			{
				return this._seed;
			}
			set
			{
				this._seed = value;
				this.SeedUpdated();
			}
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected virtual void SeedUpdated()
		{
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00154668 File Offset: 0x00152868
		public int RandomIndex<T>(IList<T> list)
		{
			int count = list.Count;
			if (count == 0)
			{
				throw new ArgumentOutOfRangeException("Empty list");
			}
			return (int)((ulong)this.Rand() % (ulong)((long)count));
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00154698 File Offset: 0x00152898
		public T ObjectAtRandomIndex<T>(IList<T> list)
		{
			T t;
			try
			{
				int num = this.RandomIndex<T>(list);
				t = list[num];
			}
			catch
			{
				t = default(T);
			}
			return t;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x001546D8 File Offset: 0x001528D8
		public void Shuffle<T>(IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				int num = (int)((ulong)this.Rand() % (ulong)((long)i--));
				T t = list[i];
				list[i] = list[num];
				list[num] = t;
			}
		}

		// Token: 0x04002D99 RID: 11673
		protected uint _seed;
	}
}
