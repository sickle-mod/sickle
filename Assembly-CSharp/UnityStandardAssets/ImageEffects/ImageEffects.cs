using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C9 RID: 1737
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x0600351F RID: 13599 RVA: 0x0013C67C File Offset: 0x0013A87C
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			if (source.texelSize.y < 0f)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", matrix4x);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.017453292f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00049C02 File Offset: 0x00047E02
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x00049C0B File Offset: 0x00047E0B
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
