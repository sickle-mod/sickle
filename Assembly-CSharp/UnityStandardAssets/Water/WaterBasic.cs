using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000693 RID: 1683
	[ExecuteInEditMode]
	public class WaterBasic : MonoBehaviour
	{
		// Token: 0x06003473 RID: 13427 RVA: 0x0013517C File Offset: 0x0013337C
		private void Update()
		{
			Renderer component = base.GetComponent<Renderer>();
			if (!component)
			{
				return;
			}
			Material sharedMaterial = component.sharedMaterial;
			if (!sharedMaterial)
			{
				return;
			}
			Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
			float @float = sharedMaterial.GetFloat("_WaveScale");
			float num = Time.time / 20f;
			Vector4 vector2 = vector * (num * @float);
			Vector4 vector3 = new Vector4(Mathf.Repeat(vector2.x, 1f), Mathf.Repeat(vector2.y, 1f), Mathf.Repeat(vector2.z, 1f), Mathf.Repeat(vector2.w, 1f));
			sharedMaterial.SetVector("_WaveOffset", vector3);
		}
	}
}
