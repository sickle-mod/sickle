using System;
using UnityEngine;

namespace Scythe.Utilities
{
	// Token: 0x020001D5 RID: 469
	public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00085C3C File Offset: 0x00083E3C
		public static T Instance
		{
			get
			{
				if (SingletonMono<T>.instance == null)
				{
					SingletonMono<T>.instance = global::UnityEngine.Object.FindObjectOfType<T>();
					if (SingletonMono<T>.instance == null)
					{
						Debug.LogError(string.Format("[{0}] instance is null!", typeof(T)));
					}
				}
				return SingletonMono<T>.instance;
			}
		}

		// Token: 0x04000ACE RID: 2766
		private static T instance;
	}
}
