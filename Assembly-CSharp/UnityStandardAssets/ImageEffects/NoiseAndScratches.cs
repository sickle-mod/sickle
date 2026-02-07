using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CC RID: 1740
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x0600352C RID: 13612 RVA: 0x0013CECC File Offset: 0x0013B0CC
		protected void Start()
		{
			if (this.shaderRGB == null || this.shaderYUV == null)
			{
				Debug.Log("Noise shaders are not set up! Disabling noise effect.");
				base.enabled = false;
				return;
			}
			if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
				return;
			}
			if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600352D RID: 13613 RVA: 0x0013CF30 File Offset: 0x0013B130
		protected Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.rgbFallback || this.monochrome)
				{
					return this.m_MaterialRGB;
				}
				return this.m_MaterialYUV;
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x00049C4F File Offset: 0x00047E4F
		protected void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x0013CFBC File Offset: 0x0013B1BC
		private void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0013D088 File Offset: 0x0013B288
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = global::UnityEngine.Random.value * 2f / this.scratchFPS;
				this.scratchX = global::UnityEngine.Random.value;
				this.scratchY = global::UnityEngine.Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(global::UnityEngine.Random.value, global::UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + global::UnityEngine.Random.value * this.scratchJitter, this.scratchY + global::UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(global::UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), global::UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0400268B RID: 9867
		public bool monochrome = true;

		// Token: 0x0400268C RID: 9868
		private bool rgbFallback;

		// Token: 0x0400268D RID: 9869
		[Range(0f, 5f)]
		public float grainIntensityMin = 0.1f;

		// Token: 0x0400268E RID: 9870
		[Range(0f, 5f)]
		public float grainIntensityMax = 0.2f;

		// Token: 0x0400268F RID: 9871
		[Range(0.1f, 50f)]
		public float grainSize = 2f;

		// Token: 0x04002690 RID: 9872
		[Range(0f, 5f)]
		public float scratchIntensityMin = 0.05f;

		// Token: 0x04002691 RID: 9873
		[Range(0f, 5f)]
		public float scratchIntensityMax = 0.25f;

		// Token: 0x04002692 RID: 9874
		[Range(1f, 30f)]
		public float scratchFPS = 10f;

		// Token: 0x04002693 RID: 9875
		[Range(0f, 1f)]
		public float scratchJitter = 0.01f;

		// Token: 0x04002694 RID: 9876
		public Texture grainTexture;

		// Token: 0x04002695 RID: 9877
		public Texture scratchTexture;

		// Token: 0x04002696 RID: 9878
		public Shader shaderRGB;

		// Token: 0x04002697 RID: 9879
		public Shader shaderYUV;

		// Token: 0x04002698 RID: 9880
		private Material m_MaterialRGB;

		// Token: 0x04002699 RID: 9881
		private Material m_MaterialYUV;

		// Token: 0x0400269A RID: 9882
		private float scratchTimeLeft;

		// Token: 0x0400269B RID: 9883
		private float scratchX;

		// Token: 0x0400269C RID: 9884
		private float scratchY;
	}
}
