using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D0 RID: 1744
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x00049D77 File Offset: 0x00047F77
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0013DEBC File Offset: 0x0013C0BC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector4 vector = new Vector4(1f, 0f, 0f, 1f);
			this.overlayMaterial.SetVector("_UV_Transform", vector);
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			this.overlayMaterial.SetTexture("_Overlay", this.texture);
			Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
		}

		// Token: 0x040026A3 RID: 9891
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x040026A4 RID: 9892
		public float intensity = 1f;

		// Token: 0x040026A5 RID: 9893
		public Texture2D texture;

		// Token: 0x040026A6 RID: 9894
		public Shader overlayShader;

		// Token: 0x040026A7 RID: 9895
		private Material overlayMaterial;

		// Token: 0x020006D1 RID: 1745
		public enum OverlayBlendMode
		{
			// Token: 0x040026A9 RID: 9897
			Additive,
			// Token: 0x040026AA RID: 9898
			ScreenBlend,
			// Token: 0x040026AB RID: 9899
			Multiply,
			// Token: 0x040026AC RID: 9900
			Overlay,
			// Token: 0x040026AD RID: 9901
			AlphaBlend
		}
	}
}
