using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C2 RID: 1730
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x0600350A RID: 13578 RVA: 0x0013BE60 File Offset: 0x0013A060
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x00049A1A File Offset: 0x00047C1A
		private new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x0013BEC4 File Offset: 0x0013A0C4
		private void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
				return;
			}
			if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x00049A28 File Offset: 0x00047C28
		private void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0013BF1C File Offset: 0x0013A11C
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
			this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
			this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
			this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
			this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
			this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
			this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
			Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
		}

		// Token: 0x0400264B RID: 9803
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x0400264C RID: 9804
		public float sensitivityDepth = 1f;

		// Token: 0x0400264D RID: 9805
		public float sensitivityNormals = 1f;

		// Token: 0x0400264E RID: 9806
		public float lumThreshold = 0.2f;

		// Token: 0x0400264F RID: 9807
		public float edgeExp = 1f;

		// Token: 0x04002650 RID: 9808
		public float sampleDist = 1f;

		// Token: 0x04002651 RID: 9809
		public float edgesOnly;

		// Token: 0x04002652 RID: 9810
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x04002653 RID: 9811
		public Shader edgeDetectShader;

		// Token: 0x04002654 RID: 9812
		private Material edgeDetectMaterial;

		// Token: 0x04002655 RID: 9813
		private EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x020006C3 RID: 1731
		public enum EdgeDetectMode
		{
			// Token: 0x04002657 RID: 9815
			TriangleDepthNormals,
			// Token: 0x04002658 RID: 9816
			RobertsCrossDepthNormals,
			// Token: 0x04002659 RID: 9817
			SobelDepth,
			// Token: 0x0400265A RID: 9818
			SobelDepthThin,
			// Token: 0x0400265B RID: 9819
			TriangleLuminance
		}
	}
}
