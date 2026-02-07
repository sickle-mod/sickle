using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B9 RID: 1721
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
	public class CreaseShading : PostEffectsBase
	{
		// Token: 0x060034ED RID: 13549 RVA: 0x00139E28 File Offset: 0x00138028
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			this.depthFetchMaterial = base.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
			this.creaseApplyMaterial = base.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x00139E9C File Offset: 0x0013809C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			float num = 1f * (float)width / (1f * (float)height);
			float num2 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
			Graphics.Blit(source, temporary, this.depthFetchMaterial);
			Graphics.Blit(temporary, renderTexture);
			for (int i = 0; i < this.softness; i++)
			{
				RenderTexture renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(0f, this.spread * num2, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
				renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = renderTexture2;
			}
			this.creaseApplyMaterial.SetTexture("_HrDepthTex", temporary);
			this.creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture);
			this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
			Graphics.Blit(source, destination, this.creaseApplyMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040025E8 RID: 9704
		public float intensity = 0.5f;

		// Token: 0x040025E9 RID: 9705
		public int softness = 1;

		// Token: 0x040025EA RID: 9706
		public float spread = 1f;

		// Token: 0x040025EB RID: 9707
		public Shader blurShader;

		// Token: 0x040025EC RID: 9708
		private Material blurMaterial;

		// Token: 0x040025ED RID: 9709
		public Shader depthFetchShader;

		// Token: 0x040025EE RID: 9710
		private Material depthFetchMaterial;

		// Token: 0x040025EF RID: 9711
		public Shader creaseApplyShader;

		// Token: 0x040025F0 RID: 9712
		private Material creaseApplyMaterial;
	}
}
