using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C4 RID: 1732
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	public class Fisheye : PostEffectsBase
	{
		// Token: 0x06003510 RID: 13584 RVA: 0x00049A30 File Offset: 0x00047C30
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x0013C064 File Offset: 0x0013A264
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 0.15625f;
			float num2 = (float)source.width * 1f / ((float)source.height * 1f);
			this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
			Graphics.Blit(source, destination, this.fisheyeMaterial);
		}

		// Token: 0x0400265C RID: 9820
		[Range(0f, 1.5f)]
		public float strengthX = 0.05f;

		// Token: 0x0400265D RID: 9821
		[Range(0f, 1.5f)]
		public float strengthY = 0.05f;

		// Token: 0x0400265E RID: 9822
		public Shader fishEyeShader;

		// Token: 0x0400265F RID: 9823
		private Material fisheyeMaterial;
	}
}
