using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x0200069C RID: 1692
	[ExecuteInEditMode]
	public class WaterBase : MonoBehaviour
	{
		// Token: 0x06003499 RID: 13465 RVA: 0x001364EC File Offset: 0x001346EC
		public void UpdateShader()
		{
			if (this.waterQuality > WaterQuality.Medium)
			{
				this.sharedMaterial.shader.maximumLOD = 501;
			}
			else if (this.waterQuality > WaterQuality.Low)
			{
				this.sharedMaterial.shader.maximumLOD = 301;
			}
			else
			{
				this.sharedMaterial.shader.maximumLOD = 201;
			}
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.edgeBlend = false;
			}
			if (this.edgeBlend)
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_ON");
				Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
				if (Camera.main)
				{
					Camera.main.depthTextureMode |= DepthTextureMode.Depth;
					return;
				}
			}
			else
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
				Shader.DisableKeyword("WATER_EDGEBLEND_ON");
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x00049438 File Offset: 0x00047638
		public void WaterTileBeingRendered(Transform tr, Camera currentCam)
		{
			if (currentCam && this.edgeBlend)
			{
				currentCam.depthTextureMode |= DepthTextureMode.Depth;
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x00049458 File Offset: 0x00047658
		public void Update()
		{
			if (this.sharedMaterial)
			{
				this.UpdateShader();
			}
		}

		// Token: 0x040024F0 RID: 9456
		public Material sharedMaterial;

		// Token: 0x040024F1 RID: 9457
		public WaterQuality waterQuality = WaterQuality.High;

		// Token: 0x040024F2 RID: 9458
		public bool edgeBlend = true;
	}
}
