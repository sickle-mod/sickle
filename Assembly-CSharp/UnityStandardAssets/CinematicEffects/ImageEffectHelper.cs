using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x020006E6 RID: 1766
	public static class ImageEffectHelper
	{
		// Token: 0x06003580 RID: 13696 RVA: 0x0013FBC8 File Offset: 0x0013DDC8
		public static bool IsSupported(Shader s, bool needDepth, bool needHdr, MonoBehaviour effect)
		{
			if (s == null || !s.isSupported)
			{
				Debug.LogWarningFormat("Missing shader for image effect {0}", new object[] { effect });
				return false;
			}
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				Debug.LogWarningFormat("Depth textures aren't supported on this device ({0})", new object[] { effect });
				return false;
			}
			if (needHdr && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
			{
				Debug.LogWarningFormat("Floating point textures aren't supported on this device ({0})", new object[] { effect });
				return false;
			}
			return true;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0004A0AE File Offset: 0x000482AE
		public static Material CheckShaderAndCreateMaterial(Shader s)
		{
			if (s == null || !s.isSupported)
			{
				return null;
			}
			return new Material(s)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x0004A0D1 File Offset: 0x000482D1
		public static bool supportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}
	}
}
