using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CA RID: 1738
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
	[RequireComponent(typeof(Camera))]
	public class MotionBlur : ImageEffectBase
	{
		// Token: 0x06003523 RID: 13603 RVA: 0x00049C15 File Offset: 0x00047E15
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x00049C1D File Offset: 0x00047E1D
		protected override void OnDisable()
		{
			base.OnDisable();
			global::UnityEngine.Object.DestroyImmediate(this.accumTexture);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x0013C728 File Offset: 0x0013A928
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.accumTexture == null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
			{
				global::UnityEngine.Object.DestroyImmediate(this.accumTexture);
				this.accumTexture = new RenderTexture(source.width, source.height, 0);
				this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
				Graphics.Blit(source, this.accumTexture);
			}
			if (this.extraBlur)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
				this.accumTexture.MarkRestoreExpected();
				Graphics.Blit(this.accumTexture, temporary);
				Graphics.Blit(temporary, this.accumTexture);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
			base.material.SetTexture("_MainTex", this.accumTexture);
			base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
			this.accumTexture.MarkRestoreExpected();
			Graphics.Blit(source, this.accumTexture, base.material);
			Graphics.Blit(this.accumTexture, destination);
		}

		// Token: 0x04002676 RID: 9846
		[Range(0f, 0.92f)]
		public float blurAmount = 0.8f;

		// Token: 0x04002677 RID: 9847
		public bool extraBlur;

		// Token: 0x04002678 RID: 9848
		private RenderTexture accumTexture;
	}
}
