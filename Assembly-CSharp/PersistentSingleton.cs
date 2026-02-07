using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0007B85C File Offset: 0x00079A5C
	public static T Instance
	{
		get
		{
			object obj = PersistentSingleton<T>.lockObj;
			lock (obj)
			{
				if (PersistentSingleton<T>.instance == null)
				{
					if (PersistentSingleton<T>.shuttingDown)
					{
						string text = "[Singleton] Instance '";
						Type typeFromHandle = typeof(T);
						Debug.LogWarning(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + "' already destroyed. Returning null.");
						return default(T);
					}
					PersistentSingleton<T>.instance = (T)((object)global::UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (PersistentSingleton<T>.instance == null)
					{
						GameObject gameObject = new GameObject();
						PersistentSingleton<T>.instance = gameObject.AddComponent<T>();
						gameObject.name = typeof(T).ToString() + " (Singleton)";
						global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
			}
			return PersistentSingleton<T>.instance;
		}
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0002E87E File Offset: 0x0002CA7E
	private void OnApplicationQuit()
	{
		PersistentSingleton<T>.shuttingDown = true;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0002E87E File Offset: 0x0002CA7E
	private void OnDestroy()
	{
		PersistentSingleton<T>.shuttingDown = true;
	}

	// Token: 0x04000898 RID: 2200
	private static T instance;

	// Token: 0x04000899 RID: 2201
	private static bool shuttingDown = false;

	// Token: 0x0400089A RID: 2202
	private static object lockObj = new object();
}
