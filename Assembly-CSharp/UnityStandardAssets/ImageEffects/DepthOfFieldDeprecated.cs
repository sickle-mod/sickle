using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BD RID: 1725
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
	public class DepthOfFieldDeprecated : PostEffectsBase
	{
		// Token: 0x060034F9 RID: 13561 RVA: 0x0013AE38 File Offset: 0x00139038
		private void CreateMaterials()
		{
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x0013AEBC File Offset: 0x001390BC
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000499E0 File Offset: 0x00047BE0
		private void OnDisable()
		{
			Quads.Cleanup();
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000499E7 File Offset: 0x00047BE7
		private void OnEnable()
		{
			this._camera = base.GetComponent<Camera>();
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x0013AF5C File Offset: 0x0013915C
		private float FocalDistance01(float worldDist)
		{
			return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x0013AFC8 File Offset: 0x001391C8
		private int GetDividerBasedOnQuality()
		{
			int num = 1;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
			{
				num = 2;
			}
			else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0013AFF0 File Offset: 0x001391F0
		private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
		{
			int num = baseDivider;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
			{
				num *= 2;
			}
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x0013B01C File Offset: 0x0013921C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.smoothness < 0.1f)
			{
				this.smoothness = 0.1f;
			}
			this.bokeh = this.bokeh && this.bokehSupport;
			float num = (this.bokeh ? DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR : 1f);
			bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
			float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
			if (this.simpleTweakMode)
			{
				this.focalDistance01 = (this.objectFocus ? (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane) : this.FocalDistance01(this.focalPoint));
				this.focalStartCurve = this.focalDistance01 * this.smoothness;
				this.focalEndCurve = this.focalStartCurve;
				flag = flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon;
			}
			else
			{
				if (this.objectFocus)
				{
					Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
					vector.z /= this._camera.farClipPlane;
					this.focalDistance01 = vector.z;
				}
				else
				{
					this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
				}
				this.focalStartCurve = this.focalZStartCurve;
				this.focalEndCurve = this.focalZEndCurve;
				flag = flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon;
			}
			this.widthOverHeight = 1f * (float)source.width / (1f * (float)source.height);
			this.oneOverBaseSize = 0.001953125f;
			this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
			this.dofMaterial.SetVector("_CurveParams", new Vector4(this.simpleTweakMode ? (1f / this.focalStartCurve) : this.focalStartCurve, this.simpleTweakMode ? (1f / this.focalEndCurve) : this.focalEndCurve, num2 * 0.5f, this.focalDistance01));
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), 0f, 0f));
			int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
			int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
			this.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
			Graphics.Blit(source, source, this.dofMaterial, 3);
			this.Downsample(source, this.mediumRezWorkTexture);
			this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
				Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
				Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
			}
			else
			{
				this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
			}
			this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
			this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
			}
			this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
			this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(source, flag ? this.foregroundTexture : destination, this.dofMaterial, this.visualize ? 2 : 0);
			if (flag)
			{
				Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
				this.Downsample(source, this.mediumRezWorkTexture);
				this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
					Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
					Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
				}
				else
				{
					this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
				}
				Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
				this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
				Graphics.Blit(source, destination, this.dofMaterial, this.visualize ? 1 : 4);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
				}
			}
			this.ReleaseTextures();
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0013B5DC File Offset: 0x001397DC
		private void Blur(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0013B71C File Offset: 0x0013991C
		private void BlurFg(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			this.dofBlurMaterial.SetTexture("_TapHigh", from);
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0013B870 File Offset: 0x00139A70
		private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
		{
			this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0013B98C File Offset: 0x00139B8C
		private void Downsample(RenderTexture from, RenderTexture to)
		{
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)to.width), 1f / (1f * (float)to.height), 0f, 0f));
			Graphics.Blit(from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0013B9F0 File Offset: 0x00139BF0
		private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
		{
			if (this.bokehMaterial)
			{
				Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
				RenderTexture.active = tempTex;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
				GL.PushMatrix();
				GL.LoadIdentity();
				bokehInfo.filterMode = FilterMode.Point;
				float num = (float)bokehInfo.width * 1f / ((float)bokehInfo.height * 1f);
				float num2 = 2f / (1f * (float)bokehInfo.width);
				num2 += this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
				this.bokehMaterial.SetTexture("_Source", bokehInfo);
				this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
				this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
				this.bokehMaterial.SetPass(0);
				foreach (Mesh mesh in meshes)
				{
					if (mesh)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
				GL.PopMatrix();
				Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
				bokehInfo.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0013BB54 File Offset: 0x00139D54
		private void ReleaseTextures()
		{
			if (this.foregroundTexture)
			{
				RenderTexture.ReleaseTemporary(this.foregroundTexture);
			}
			if (this.finalDefocus)
			{
				RenderTexture.ReleaseTemporary(this.finalDefocus);
			}
			if (this.mediumRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
			}
			if (this.lowRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
			}
			if (this.bokehSource)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource);
			}
			if (this.bokehSource2)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource2);
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0013BBF4 File Offset: 0x00139DF4
		private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
		{
			this.foregroundTexture = null;
			if (blurForeground)
			{
				this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			}
			this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
			this.bokehSource = null;
			this.bokehSource2 = null;
			if (this.bokeh)
			{
				this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource.filterMode = FilterMode.Bilinear;
				this.bokehSource2.filterMode = FilterMode.Bilinear;
				RenderTexture.active = this.bokehSource2;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			source.filterMode = FilterMode.Bilinear;
			this.finalDefocus.filterMode = FilterMode.Bilinear;
			this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
			this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
			if (this.foregroundTexture)
			{
				this.foregroundTexture.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x04002611 RID: 9745
		private static int SMOOTH_DOWNSAMPLE_PASS = 6;

		// Token: 0x04002612 RID: 9746
		private static float BOKEH_EXTRA_BLUR = 2f;

		// Token: 0x04002613 RID: 9747
		public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;

		// Token: 0x04002614 RID: 9748
		public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;

		// Token: 0x04002615 RID: 9749
		public bool simpleTweakMode = true;

		// Token: 0x04002616 RID: 9750
		public float focalPoint = 1f;

		// Token: 0x04002617 RID: 9751
		public float smoothness = 0.5f;

		// Token: 0x04002618 RID: 9752
		public float focalZDistance;

		// Token: 0x04002619 RID: 9753
		public float focalZStartCurve = 1f;

		// Token: 0x0400261A RID: 9754
		public float focalZEndCurve = 1f;

		// Token: 0x0400261B RID: 9755
		private float focalStartCurve = 2f;

		// Token: 0x0400261C RID: 9756
		private float focalEndCurve = 2f;

		// Token: 0x0400261D RID: 9757
		private float focalDistance01 = 0.1f;

		// Token: 0x0400261E RID: 9758
		public Transform objectFocus;

		// Token: 0x0400261F RID: 9759
		public float focalSize;

		// Token: 0x04002620 RID: 9760
		public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;

		// Token: 0x04002621 RID: 9761
		public float maxBlurSpread = 1.75f;

		// Token: 0x04002622 RID: 9762
		public float foregroundBlurExtrude = 1.15f;

		// Token: 0x04002623 RID: 9763
		public Shader dofBlurShader;

		// Token: 0x04002624 RID: 9764
		private Material dofBlurMaterial;

		// Token: 0x04002625 RID: 9765
		public Shader dofShader;

		// Token: 0x04002626 RID: 9766
		private Material dofMaterial;

		// Token: 0x04002627 RID: 9767
		public bool visualize;

		// Token: 0x04002628 RID: 9768
		public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;

		// Token: 0x04002629 RID: 9769
		private float widthOverHeight = 1.25f;

		// Token: 0x0400262A RID: 9770
		private float oneOverBaseSize = 0.001953125f;

		// Token: 0x0400262B RID: 9771
		public bool bokeh;

		// Token: 0x0400262C RID: 9772
		public bool bokehSupport = true;

		// Token: 0x0400262D RID: 9773
		public Shader bokehShader;

		// Token: 0x0400262E RID: 9774
		public Texture2D bokehTexture;

		// Token: 0x0400262F RID: 9775
		public float bokehScale = 2.4f;

		// Token: 0x04002630 RID: 9776
		public float bokehIntensity = 0.15f;

		// Token: 0x04002631 RID: 9777
		public float bokehThresholdContrast = 0.1f;

		// Token: 0x04002632 RID: 9778
		public float bokehThresholdLuminance = 0.55f;

		// Token: 0x04002633 RID: 9779
		public int bokehDownsample = 1;

		// Token: 0x04002634 RID: 9780
		private Material bokehMaterial;

		// Token: 0x04002635 RID: 9781
		private Camera _camera;

		// Token: 0x04002636 RID: 9782
		private RenderTexture foregroundTexture;

		// Token: 0x04002637 RID: 9783
		private RenderTexture mediumRezWorkTexture;

		// Token: 0x04002638 RID: 9784
		private RenderTexture finalDefocus;

		// Token: 0x04002639 RID: 9785
		private RenderTexture lowRezWorkTexture;

		// Token: 0x0400263A RID: 9786
		private RenderTexture bokehSource;

		// Token: 0x0400263B RID: 9787
		private RenderTexture bokehSource2;

		// Token: 0x020006BE RID: 1726
		public enum Dof34QualitySetting
		{
			// Token: 0x0400263D RID: 9789
			OnlyBackground = 1,
			// Token: 0x0400263E RID: 9790
			BackgroundAndForeground
		}

		// Token: 0x020006BF RID: 1727
		public enum DofResolution
		{
			// Token: 0x04002640 RID: 9792
			High = 2,
			// Token: 0x04002641 RID: 9793
			Medium,
			// Token: 0x04002642 RID: 9794
			Low
		}

		// Token: 0x020006C0 RID: 1728
		public enum DofBlurriness
		{
			// Token: 0x04002644 RID: 9796
			Low = 1,
			// Token: 0x04002645 RID: 9797
			High,
			// Token: 0x04002646 RID: 9798
			VeryHigh = 4
		}

		// Token: 0x020006C1 RID: 1729
		public enum BokehDestination
		{
			// Token: 0x04002648 RID: 9800
			Background = 1,
			// Token: 0x04002649 RID: 9801
			Foreground,
			// Token: 0x0400264A RID: 9802
			BackgroundAndForeground
		}
	}
}
