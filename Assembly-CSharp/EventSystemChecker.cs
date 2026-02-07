using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000009 RID: 9
public class EventSystemChecker : MonoBehaviour
{
	// Token: 0x0600001C RID: 28 RVA: 0x00027E95 File Offset: 0x00026095
	private void Awake()
	{
		if (!global::UnityEngine.Object.FindObjectOfType<EventSystem>())
		{
			GameObject gameObject = new GameObject("EventSystem");
			gameObject.AddComponent<EventSystem>();
			gameObject.AddComponent<StandaloneInputModule>().forceModuleActive = true;
		}
	}
}
