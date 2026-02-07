using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public static class RendererExtensions
{
	// Token: 0x060009A9 RID: 2473 RVA: 0x0007BB80 File Offset: 0x00079D80
	private static int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
	{
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector = camera.WorldToScreenPoint(array[i]);
			if (rect.Contains(vector))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0007BBEC File Offset: 0x00079DEC
	public static float ProtrusionHeight(RectTransform rectTransform, Camera camera)
	{
		new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		return Math.Abs(camera.WorldToScreenPoint(array[0]).y);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002E8EA File Offset: 0x0002CAEA
	public static bool IsFullyVisibleFrom(RectTransform rectTransform, Camera camera)
	{
		return global::RendererExtensions.CountCornersVisibleFrom(rectTransform, camera) == 4;
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002E8F6 File Offset: 0x0002CAF6
	public static bool IsVisibleFrom(RectTransform rectTransform, Camera camera)
	{
		return global::RendererExtensions.CountCornersVisibleFrom(rectTransform, camera) > 0;
	}
}
