using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000698 RID: 1688
	[RequireComponent(typeof(WaterBase))]
	[ExecuteInEditMode]
	public class SpecularLighting : MonoBehaviour
	{
		// Token: 0x0600348C RID: 13452 RVA: 0x000493F9 File Offset: 0x000475F9
		public void Start()
		{
			this.m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x001358B8 File Offset: 0x00133AB8
		public void Update()
		{
			if (!this.m_WaterBase)
			{
				this.m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
			}
			if (this.specularLight && this.m_WaterBase.sharedMaterial)
			{
				this.m_WaterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
			}
		}

		// Token: 0x040024D8 RID: 9432
		public Transform specularLight;

		// Token: 0x040024D9 RID: 9433
		private WaterBase m_WaterBase;
	}
}
