using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
public static class RectTransformExtensions
{
	// Token: 0x06000A89 RID: 2697 RVA: 0x0002F409 File Offset: 0x0002D609
	public static void Reset(this RectTransform rectTransform)
	{
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0007D950 File Offset: 0x0007BB50
	public static Rect GetScreenRect(this RectTransform rectTransform)
	{
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		Rect rect = new Rect(new Vector2(array[0].x, array[0].y), new Vector2(array[3].x - array[0].x, array[1].y - array[0].y));
		Component componentInParent = rectTransform.GetComponentInParent<Canvas>();
		Vector3[] array2 = new Vector3[4];
		componentInParent.GetComponent<RectTransform>().GetWorldCorners(array2);
		Rect rect2 = new Rect(new Vector2(array2[0].x, array2[0].y), new Vector2(array2[3].x - array2[0].x, array2[1].y - array2[0].y));
		int width = Screen.width;
		int height = Screen.height;
		Vector2 vector = new Vector2((float)width / rect2.size.x * rect.size.x, (float)height / rect2.size.y * rect.size.y);
		return new Rect((float)width * ((rect.x - rect2.x) / rect2.size.x), (float)height * ((-rect2.y + rect.y) / rect2.size.y), vector.x, vector.y);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0007DADC File Offset: 0x0007BCDC
	public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		Vector3 lossyScale = transform.lossyScale;
		transform.localScale = new Vector3(globalScale.x / lossyScale.x, globalScale.y / lossyScale.y, globalScale.z / lossyScale.z);
	}
}
