using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200084B RID: 2123
	public class LCGRandomGenerator : RandomGenerator
	{
		// Token: 0x06003BFD RID: 15357 RVA: 0x001544B8 File Offset: 0x001526B8
		public LCGRandomGenerator()
		{
			ulong num = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			this._seed = (uint)(num & (ulong)(-1));
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0004ED6C File Offset: 0x0004CF6C
		public LCGRandomGenerator(uint seed)
		{
			this._seed = seed;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x001544FC File Offset: 0x001526FC
		public override uint Rand()
		{
			ulong num = 1664525UL * (ulong)this._seed + 1013904223UL;
			this._seed = (uint)(num & (ulong)(-1));
			return this._seed;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00154530 File Offset: 0x00152730
		public override uint Range(uint min, uint max)
		{
			uint num = (uint)Math.Abs((long)((ulong)(max - min)));
			return this.Rand() % num + min;
		}
	}
}
