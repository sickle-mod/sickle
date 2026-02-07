using System;

namespace Scythe.Utilities
{
	// Token: 0x020001D4 RID: 468
	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00030F2F File Offset: 0x0002F12F
		public static T Instance
		{
			get
			{
				if (Singleton<T>.instance == null)
				{
					Singleton<T>.instance = new T();
				}
				return Singleton<T>.instance;
			}
		}

		// Token: 0x04000ACD RID: 2765
		private static T instance;
	}
}
