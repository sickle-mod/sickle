using System;

namespace Scythe.Utils.Extensions
{
	// Token: 0x020001CD RID: 461
	public static class GuidExtensions
	{
		// Token: 0x06000D6C RID: 3436 RVA: 0x00030E77 File Offset: 0x0002F077
		public static Guid BotGuid(this Guid @this)
		{
			return Guid.Parse("00000000-0000-0000-0000-000000000001");
		}
	}
}
