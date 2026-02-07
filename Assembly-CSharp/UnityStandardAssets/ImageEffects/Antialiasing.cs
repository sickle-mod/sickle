using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200069F RID: 1695
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x060034A1 RID: 13473 RVA: 0x0013669C File Offset: 0x0013489C
		public Material CurrentAAMaterial()
		{
			Material material;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				material = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				material = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				material = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				material = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				material = this.nfaa;
				break;
			case AAMode.SSAA:
				material = this.ssaa;
				break;
			case AAMode.DLAA:
				material = this.dlaa;
				break;
			default:
				material = null;
				break;
			}
			return material;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x00136718 File Offset: 0x00134918
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x001367F4 File Offset: 0x001349F4
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
				return;
			}
			if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
				return;
			}
			if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
				return;
			}
			if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
				return;
			}
			if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
				return;
			}
			if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, this.dlaaSharp ? 2 : 1);
				RenderTexture.ReleaseTemporary(temporary);
				return;
			}
			if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, this.showGeneratedNormals ? 1 : 0);
				return;
			}
			Graphics.Blit(source, destination);
		}

		// Token: 0x040024FD RID: 9469
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x040024FE RID: 9470
		public bool showGeneratedNormals;

		// Token: 0x040024FF RID: 9471
		public float offsetScale = 0.2f;

		// Token: 0x04002500 RID: 9472
		public float blurRadius = 18f;

		// Token: 0x04002501 RID: 9473
		public float edgeThresholdMin = 0.05f;

		// Token: 0x04002502 RID: 9474
		public float edgeThreshold = 0.2f;

		// Token: 0x04002503 RID: 9475
		public float edgeSharpness = 4f;

		// Token: 0x04002504 RID: 9476
		public bool dlaaSharp;

		// Token: 0x04002505 RID: 9477
		public Shader ssaaShader;

		// Token: 0x04002506 RID: 9478
		private Material ssaa;

		// Token: 0x04002507 RID: 9479
		public Shader dlaaShader;

		// Token: 0x04002508 RID: 9480
		private Material dlaa;

		// Token: 0x04002509 RID: 9481
		public Shader nfaaShader;

		// Token: 0x0400250A RID: 9482
		private Material nfaa;

		// Token: 0x0400250B RID: 9483
		public Shader shaderFXAAPreset2;

		// Token: 0x0400250C RID: 9484
		private Material materialFXAAPreset2;

		// Token: 0x0400250D RID: 9485
		public Shader shaderFXAAPreset3;

		// Token: 0x0400250E RID: 9486
		private Material materialFXAAPreset3;

		// Token: 0x0400250F RID: 9487
		public Shader shaderFXAAII;

		// Token: 0x04002510 RID: 9488
		private Material materialFXAAII;

		// Token: 0x04002511 RID: 9489
		public Shader shaderFXAAIII;

		// Token: 0x04002512 RID: 9490
		private Material materialFXAAIII;
	}
}
