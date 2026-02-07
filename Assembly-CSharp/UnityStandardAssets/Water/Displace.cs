using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000694 RID: 1684
	[ExecuteInEditMode]
	[RequireComponent(typeof(WaterBase))]
	public class Displace : MonoBehaviour
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x000491E8 File Offset: 0x000473E8
		public void Awake()
		{
			if (base.enabled)
			{
				this.OnEnable();
				return;
			}
			this.OnDisable();
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000491FF File Offset: 0x000473FF
		public void OnEnable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x00049215 File Offset: 0x00047415
		public void OnDisable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		}
	}
}
