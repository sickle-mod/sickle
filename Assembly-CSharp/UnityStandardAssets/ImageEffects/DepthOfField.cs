using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BA RID: 1722
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")]
	public class DepthOfField : PostEffectsBase
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x0013A028 File Offset: 0x00138228
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofHdrMaterial = base.CheckShaderAndCreateMaterial(this.dofHdrShader, this.dofHdrMaterial);
			if (this.supportDX11 && this.blurType == DepthOfField.BlurType.DX11)
			{
				this.dx11bokehMaterial = base.CheckShaderAndCreateMaterial(this.dx11BokehShader, this.dx11bokehMaterial);
				this.CreateComputeResources();
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x00049989 File Offset: 0x00047B89
		private void OnEnable()
		{
			this.cachedCamera = base.GetComponent<Camera>();
			this.cachedCamera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x0013A098 File Offset: 0x00138298
		private void OnDisable()
		{
			this.ReleaseComputeResources();
			if (this.dofHdrMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.dofHdrMaterial);
			}
			this.dofHdrMaterial = null;
			if (this.dx11bokehMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.dx11bokehMaterial);
			}
			this.dx11bokehMaterial = null;
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000499AA File Offset: 0x00047BAA
		private void ReleaseComputeResources()
		{
			if (this.cbDrawArgs != null)
			{
				this.cbDrawArgs.Release();
			}
			this.cbDrawArgs = null;
			if (this.cbPoints != null)
			{
				this.cbPoints.Release();
			}
			this.cbPoints = null;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x0013A0EC File Offset: 0x001382EC
		private void CreateComputeResources()
		{
			if (this.cbDrawArgs == null)
			{
				this.cbDrawArgs = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
				int[] array = new int[] { 0, 1, 0, 0 };
				this.cbDrawArgs.SetData(array);
			}
			if (this.cbPoints == null)
			{
				this.cbPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
			}
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x0013A154 File Offset: 0x00138354
		private float FocalDistance01(float worldDist)
		{
			return this.cachedCamera.WorldToViewportPoint((worldDist - this.cachedCamera.nearClipPlane) * this.cachedCamera.transform.forward + this.cachedCamera.transform.position).z / (this.cachedCamera.farClipPlane - this.cachedCamera.nearClipPlane);
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x0013A1C0 File Offset: 0x001383C0
		private void WriteCoc(RenderTexture fromTo, bool fgDilate)
		{
			this.dofHdrMaterial.SetTexture("_FgOverlap", null);
			if (this.nearBlur && fgDilate)
			{
				int num = fromTo.width / 2;
				int num2 = fromTo.height / 2;
				RenderTexture renderTexture = RenderTexture.GetTemporary(num, num2, 0, fromTo.format);
				Graphics.Blit(fromTo, renderTexture, this.dofHdrMaterial, 4);
				float num3 = this.internalBlurWidth * this.foregroundOverlap;
				this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num3, 0f, num3));
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, fromTo.format);
				Graphics.Blit(renderTexture, temporary, this.dofHdrMaterial, 2);
				RenderTexture.ReleaseTemporary(renderTexture);
				this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num3, 0f, 0f, num3));
				renderTexture = RenderTexture.GetTemporary(num, num2, 0, fromTo.format);
				Graphics.Blit(temporary, renderTexture, this.dofHdrMaterial, 2);
				RenderTexture.ReleaseTemporary(temporary);
				this.dofHdrMaterial.SetTexture("_FgOverlap", renderTexture);
				fromTo.MarkRestoreExpected();
				Graphics.Blit(fromTo, fromTo, this.dofHdrMaterial, 13);
				RenderTexture.ReleaseTemporary(renderTexture);
				return;
			}
			fromTo.MarkRestoreExpected();
			Graphics.Blit(fromTo, fromTo, this.dofHdrMaterial, 0);
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x0013A2F0 File Offset: 0x001384F0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.aperture < 0f)
			{
				this.aperture = 0f;
			}
			if (this.maxBlurSize < 0.1f)
			{
				this.maxBlurSize = 0.1f;
			}
			this.focalSize = Mathf.Clamp(this.focalSize, 0f, 2f);
			this.internalBlurWidth = Mathf.Max(this.maxBlurSize, 0f);
			this.focalDistance01 = (this.focalTransform ? (this.cachedCamera.WorldToViewportPoint(this.focalTransform.position).z / this.cachedCamera.farClipPlane) : this.FocalDistance01(this.focalLength));
			this.dofHdrMaterial.SetVector("_CurveParams", new Vector4(1f, this.focalSize, 1f / (1f - this.aperture) - 1f, this.focalDistance01));
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			float num = this.internalBlurWidth * this.foregroundOverlap;
			if (this.visualizeFocus)
			{
				this.WriteCoc(source, true);
				Graphics.Blit(source, destination, this.dofHdrMaterial, 16);
			}
			else if (this.blurType == DepthOfField.BlurType.DX11 && this.dx11bokehMaterial)
			{
				if (this.highResolution)
				{
					this.internalBlurWidth = ((this.internalBlurWidth < 0.1f) ? 0.1f : this.internalBlurWidth);
					num = this.internalBlurWidth * this.foregroundOverlap;
					renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
					RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
					this.WriteCoc(source, false);
					RenderTexture renderTexture3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
					RenderTexture renderTexture4 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
					Graphics.Blit(source, renderTexture3, this.dofHdrMaterial, 15);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
					Graphics.Blit(renderTexture3, renderTexture4, this.dofHdrMaterial, 19);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
					Graphics.Blit(renderTexture4, renderTexture3, this.dofHdrMaterial, 19);
					if (this.nearBlur)
					{
						Graphics.Blit(source, renderTexture4, this.dofHdrMaterial, 4);
					}
					this.dx11bokehMaterial.SetTexture("_BlurredColor", renderTexture3);
					this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
					this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
					this.dx11bokehMaterial.SetTexture("_FgCocMask", this.nearBlur ? renderTexture4 : null);
					Graphics.SetRandomWriteTarget(1, this.cbPoints);
					Graphics.Blit(source, renderTexture, this.dx11bokehMaterial, 0);
					Graphics.ClearRandomWriteTargets();
					if (this.nearBlur)
					{
						this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num, 0f, num));
						Graphics.Blit(renderTexture4, renderTexture3, this.dofHdrMaterial, 2);
						this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num, 0f, 0f, num));
						Graphics.Blit(renderTexture3, renderTexture4, this.dofHdrMaterial, 2);
						Graphics.Blit(renderTexture4, renderTexture, this.dofHdrMaterial, 3);
					}
					Graphics.Blit(renderTexture, temporary, this.dofHdrMaterial, 20);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0f, 0f, this.internalBlurWidth));
					Graphics.Blit(renderTexture, source, this.dofHdrMaterial, 5);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0f, this.internalBlurWidth));
					Graphics.Blit(source, temporary, this.dofHdrMaterial, 21);
					Graphics.SetRenderTarget(temporary);
					ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
					this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
					this.dx11bokehMaterial.SetTexture("_MainTex", this.dx11BokehTexture);
					this.dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), this.internalBlurWidth));
					this.dx11bokehMaterial.SetPass(2);
					Graphics.DrawProceduralIndirectNow(MeshTopology.Points, this.cbDrawArgs, 0);
					Graphics.Blit(temporary, destination);
					RenderTexture.ReleaseTemporary(temporary);
					RenderTexture.ReleaseTemporary(renderTexture3);
					RenderTexture.ReleaseTemporary(renderTexture4);
				}
				else
				{
					renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
					renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
					num = this.internalBlurWidth * this.foregroundOverlap;
					this.WriteCoc(source, false);
					source.filterMode = FilterMode.Bilinear;
					Graphics.Blit(source, renderTexture, this.dofHdrMaterial, 6);
					RenderTexture renderTexture3 = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format);
					RenderTexture renderTexture4 = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format);
					Graphics.Blit(renderTexture, renderTexture3, this.dofHdrMaterial, 15);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
					Graphics.Blit(renderTexture3, renderTexture4, this.dofHdrMaterial, 19);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
					Graphics.Blit(renderTexture4, renderTexture3, this.dofHdrMaterial, 19);
					RenderTexture renderTexture5 = null;
					if (this.nearBlur)
					{
						renderTexture5 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
						Graphics.Blit(source, renderTexture5, this.dofHdrMaterial, 4);
					}
					this.dx11bokehMaterial.SetTexture("_BlurredColor", renderTexture3);
					this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
					this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
					this.dx11bokehMaterial.SetTexture("_FgCocMask", renderTexture5);
					Graphics.SetRandomWriteTarget(1, this.cbPoints);
					Graphics.Blit(renderTexture, renderTexture2, this.dx11bokehMaterial, 0);
					Graphics.ClearRandomWriteTargets();
					RenderTexture.ReleaseTemporary(renderTexture3);
					RenderTexture.ReleaseTemporary(renderTexture4);
					if (this.nearBlur)
					{
						this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, num, 0f, num));
						Graphics.Blit(renderTexture5, renderTexture, this.dofHdrMaterial, 2);
						this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num, 0f, 0f, num));
						Graphics.Blit(renderTexture, renderTexture5, this.dofHdrMaterial, 2);
						Graphics.Blit(renderTexture5, renderTexture2, this.dofHdrMaterial, 3);
					}
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0f, 0f, this.internalBlurWidth));
					Graphics.Blit(renderTexture2, renderTexture, this.dofHdrMaterial, 5);
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0f, this.internalBlurWidth));
					Graphics.Blit(renderTexture, renderTexture2, this.dofHdrMaterial, 5);
					Graphics.SetRenderTarget(renderTexture2);
					ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
					this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
					this.dx11bokehMaterial.SetTexture("_MainTex", this.dx11BokehTexture);
					this.dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * (float)renderTexture2.width), 1f / (1f * (float)renderTexture2.height), this.internalBlurWidth));
					this.dx11bokehMaterial.SetPass(1);
					Graphics.DrawProceduralIndirectNow(MeshTopology.Points, this.cbDrawArgs, 0);
					this.dofHdrMaterial.SetTexture("_LowRez", renderTexture2);
					this.dofHdrMaterial.SetTexture("_FgOverlap", renderTexture5);
					this.dofHdrMaterial.SetVector("_Offsets", 1f * (float)source.width / (1f * (float)renderTexture2.width) * this.internalBlurWidth * Vector4.one);
					Graphics.Blit(source, destination, this.dofHdrMaterial, 9);
					if (renderTexture5)
					{
						RenderTexture.ReleaseTemporary(renderTexture5);
					}
				}
			}
			else
			{
				source.filterMode = FilterMode.Bilinear;
				if (this.highResolution)
				{
					this.internalBlurWidth *= 2f;
				}
				this.WriteCoc(source, true);
				renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
				int num2 = ((this.blurSampleCount == DepthOfField.BlurSampleCount.High || this.blurSampleCount == DepthOfField.BlurSampleCount.Medium) ? 17 : 11);
				if (this.highResolution)
				{
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0.025f, this.internalBlurWidth));
					Graphics.Blit(source, destination, this.dofHdrMaterial, num2);
				}
				else
				{
					this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0.1f, this.internalBlurWidth));
					Graphics.Blit(source, renderTexture, this.dofHdrMaterial, 6);
					Graphics.Blit(renderTexture, renderTexture2, this.dofHdrMaterial, num2);
					this.dofHdrMaterial.SetTexture("_LowRez", renderTexture2);
					this.dofHdrMaterial.SetTexture("_FgOverlap", null);
					this.dofHdrMaterial.SetVector("_Offsets", Vector4.one * (1f * (float)source.width / (1f * (float)renderTexture2.width)) * this.internalBlurWidth);
					Graphics.Blit(source, destination, this.dofHdrMaterial, (this.blurSampleCount == DepthOfField.BlurSampleCount.High) ? 18 : 12);
				}
			}
			if (renderTexture)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			if (renderTexture2)
			{
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
		}

		// Token: 0x040025F1 RID: 9713
		public bool visualizeFocus;

		// Token: 0x040025F2 RID: 9714
		public float focalLength = 10f;

		// Token: 0x040025F3 RID: 9715
		public float focalSize = 0.05f;

		// Token: 0x040025F4 RID: 9716
		public float aperture = 0.5f;

		// Token: 0x040025F5 RID: 9717
		public Transform focalTransform;

		// Token: 0x040025F6 RID: 9718
		public float maxBlurSize = 2f;

		// Token: 0x040025F7 RID: 9719
		public bool highResolution;

		// Token: 0x040025F8 RID: 9720
		public DepthOfField.BlurType blurType;

		// Token: 0x040025F9 RID: 9721
		public DepthOfField.BlurSampleCount blurSampleCount = DepthOfField.BlurSampleCount.High;

		// Token: 0x040025FA RID: 9722
		public bool nearBlur;

		// Token: 0x040025FB RID: 9723
		public float foregroundOverlap = 1f;

		// Token: 0x040025FC RID: 9724
		public Shader dofHdrShader;

		// Token: 0x040025FD RID: 9725
		private Material dofHdrMaterial;

		// Token: 0x040025FE RID: 9726
		public Shader dx11BokehShader;

		// Token: 0x040025FF RID: 9727
		private Material dx11bokehMaterial;

		// Token: 0x04002600 RID: 9728
		public float dx11BokehThreshold = 0.5f;

		// Token: 0x04002601 RID: 9729
		public float dx11SpawnHeuristic = 0.0875f;

		// Token: 0x04002602 RID: 9730
		public Texture2D dx11BokehTexture;

		// Token: 0x04002603 RID: 9731
		public float dx11BokehScale = 1.2f;

		// Token: 0x04002604 RID: 9732
		public float dx11BokehIntensity = 2.5f;

		// Token: 0x04002605 RID: 9733
		private float focalDistance01 = 10f;

		// Token: 0x04002606 RID: 9734
		private ComputeBuffer cbDrawArgs;

		// Token: 0x04002607 RID: 9735
		private ComputeBuffer cbPoints;

		// Token: 0x04002608 RID: 9736
		private float internalBlurWidth = 1f;

		// Token: 0x04002609 RID: 9737
		private Camera cachedCamera;

		// Token: 0x020006BB RID: 1723
		public enum BlurType
		{
			// Token: 0x0400260B RID: 9739
			DiscBlur,
			// Token: 0x0400260C RID: 9740
			DX11
		}

		// Token: 0x020006BC RID: 1724
		public enum BlurSampleCount
		{
			// Token: 0x0400260E RID: 9742
			Low,
			// Token: 0x0400260F RID: 9743
			Medium,
			// Token: 0x04002610 RID: 9744
			High
		}
	}
}
