using System;
using UnityEngine;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000866 RID: 2150
	public static class TransformExtension
	{
		// Token: 0x06003C70 RID: 15472 RVA: 0x0015570C File Offset: 0x0015390C
		public static void RemoveAllChildren(this Transform transform)
		{
			foreach (object obj in transform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			transform.DetachChildren();
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00155768 File Offset: 0x00153968
		public static void Show(this Transform transform, bool show)
		{
			Renderer component = transform.gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.enabled = show;
			}
			CanvasRenderer component2 = transform.gameObject.GetComponent<CanvasRenderer>();
			if (component2 != null)
			{
				component2.cull = !show;
			}
			Canvas component3 = transform.gameObject.GetComponent<Canvas>();
			if (component3 != null)
			{
				component3.enabled = show;
				return;
			}
			foreach (object obj in transform)
			{
				((Transform)obj).Show(show);
			}
		}
	}
}
