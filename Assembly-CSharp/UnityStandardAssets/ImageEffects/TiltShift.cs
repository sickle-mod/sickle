using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D9 RID: 1753
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	internal class TiltShift : PostEffectsBase
	{
		// Token: 0x06003560 RID: 13664 RVA: 0x00049EAC File Offset: 0x000480AC
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x0013EB1C File Offset: 0x0013CD1C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize < 0f) ? 0f : this.maxBlurSize);
			this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
			source.filterMode = FilterMode.Bilinear;
			RenderTexture renderTexture = destination;
			if ((float)this.downsample > 0f)
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
			}
			int num = (int)this.quality;
			num *= 2;
			Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode == TiltShift.TiltShiftMode.TiltShiftMode) ? num : (num + 1));
			if (this.downsample > 0)
			{
				this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
				Graphics.Blit(source, destination, this.tiltShiftMaterial, 8);
			}
			if (renderTexture != destination)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040026DA RID: 9946
		public TiltShift.TiltShiftMode mode;

		// Token: 0x040026DB RID: 9947
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x040026DC RID: 9948
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x040026DD RID: 9949
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x040026DE RID: 9950
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040026DF RID: 9951
		public Shader tiltShiftShader;

		// Token: 0x040026E0 RID: 9952
		private Material tiltShiftMaterial;

		// Token: 0x020006DA RID: 1754
		public enum TiltShiftMode
		{
			// Token: 0x040026E2 RID: 9954
			TiltShiftMode,
			// Token: 0x040026E3 RID: 9955
			IrisMode
		}

		// Token: 0x020006DB RID: 1755
		public enum TiltShiftQuality
		{
			// Token: 0x040026E5 RID: 9957
			Preview,
			// Token: 0x040026E6 RID: 9958
			Low,
			// Token: 0x040026E7 RID: 9959
			Normal,
			// Token: 0x040026E8 RID: 9960
			High
		}
	}
}
