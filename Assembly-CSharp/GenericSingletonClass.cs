using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class GenericSingletonClass<T> : MonoBehaviour where T : Component
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x0007AE0C File Offset: 0x0007900C
	public static T Instance
	{
		get
		{
			if (GenericSingletonClass<T>.instance == null)
			{
				GenericSingletonClass<T>.instance = global::UnityEngine.Object.FindObjectOfType<T>();
				if (GenericSingletonClass<T>.instance == null)
				{
					GenericSingletonClass<T>.instance = new GameObject
					{
						name = typeof(T).Name
					}.AddComponent<T>();
				}
			}
			return GenericSingletonClass<T>.instance;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0002E49F File Offset: 0x0002C69F
	public virtual void Awake()
	{
		if (GenericSingletonClass<T>.instance == null)
		{
			GenericSingletonClass<T>.instance = this as T;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400086C RID: 2156
	private static T instance;
}
