using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class LobbyGameTypeShrinker : MonoBehaviour
{
	// Token: 0x06000952 RID: 2386 RVA: 0x0002E64A File Offset: 0x0002C84A
	private void Start()
	{
		if (PlatformManager.ScreenAspectRatio > 2f)
		{
			base.GetComponent<RectTransform>().sizeDelta = new Vector2(base.GetComponent<RectTransform>().sizeDelta.x, 0f);
		}
	}

	// Token: 0x04000882 RID: 2178
	private const float MINIMUM_WIDE_SCREEN_ASPECT_RATION = 2f;
}
