using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x020006E8 RID: 1768
	public class RenderTextureUtility
	{
		// Token: 0x06003584 RID: 13700 RVA: 0x0013FC40 File Offset: 0x0013DE40
		public RenderTexture GetTemporaryRenderTexture(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, FilterMode filterMode = FilterMode.Bilinear)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
			temporary.filterMode = filterMode;
			temporary.wrapMode = TextureWrapMode.Clamp;
			temporary.name = "RenderTextureUtilityTempTexture";
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x0013FC80 File Offset: 0x0013DE80
		public void ReleaseTemporaryRenderTexture(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				Debug.LogErrorFormat("Attempting to remove texture that was not allocated: {0}", new object[] { rt });
				return;
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x0013FCD0 File Offset: 0x0013DED0
		public void ReleaseAllTemporaryRenderTextures()
		{
			for (int i = 0; i < this.m_TemporaryRTs.Count; i++)
			{
				RenderTexture.ReleaseTemporary(this.m_TemporaryRTs[i]);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x0400272C RID: 10028
		private List<RenderTexture> m_TemporaryRTs = new List<RenderTexture>();
	}
}
