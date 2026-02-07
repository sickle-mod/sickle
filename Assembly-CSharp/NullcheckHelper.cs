using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013C RID: 316
public static class NullcheckHelper
{
	// Token: 0x0600095B RID: 2395 RVA: 0x0002E689 File Offset: 0x0002C889
	public static void SetGameObjectActiveIfNotNull(GameObject gameObject, bool isActive)
	{
		if (gameObject != null)
		{
			gameObject.SetActive(isActive);
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002E69B File Offset: 0x0002C89B
	public static void SetComponentEnabledIfNotNull(MonoBehaviour component, bool enabled)
	{
		if (component != null)
		{
			component.enabled = enabled;
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002E6AD File Offset: 0x0002C8AD
	public static void SetTextValueIfNotNull(Text text, string value)
	{
		if (text != null)
		{
			text.text = value;
		}
	}
}
