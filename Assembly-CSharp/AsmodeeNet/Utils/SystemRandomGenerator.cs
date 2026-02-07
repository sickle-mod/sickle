using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000857 RID: 2135
	public class SystemRandomGenerator : RandomGenerator
	{
		// Token: 0x06003C3C RID: 15420 RVA: 0x0004EFDE File Offset: 0x0004D1DE
		public SystemRandomGenerator()
		{
			this._random = new Random();
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0004EFF1 File Offset: 0x0004D1F1
		public SystemRandomGenerator(uint seed)
		{
			base.Seed = seed;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0004F000 File Offset: 0x0004D200
		public override uint Rand()
		{
			this._seed = (uint)this._random.Next();
			return this._seed;
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x0004F019 File Offset: 0x0004D219
		public override uint Range(uint min, uint max)
		{
			this._seed = (uint)this._random.Next((int)min, (int)max);
			return this._seed;
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x0004F034 File Offset: 0x0004D234
		protected override void SeedUpdated()
		{
			this._random = new Random((int)this._seed);
		}

		// Token: 0x04002DAF RID: 11695
		private Random _random;
	}
}
