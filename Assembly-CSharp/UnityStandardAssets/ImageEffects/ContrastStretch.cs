using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B8 RID: 1720
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060034E3 RID: 13539 RVA: 0x00049821 File Offset: 0x00047A21
		protected Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060034E4 RID: 13540 RVA: 0x00049855 File Offset: 0x00047A55
		protected Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060034E5 RID: 13541 RVA: 0x00049889 File Offset: 0x00047A89
		protected Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060034E6 RID: 13542 RVA: 0x000498BD File Offset: 0x00047ABD
		protected Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000498F1 File Offset: 0x00047AF1
		private void Start()
		{
			if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x00139BC8 File Offset: 0x00137DC8
		private void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x00139C10 File Offset: 0x00137E10
		private void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x00139CA0 File Offset: 0x00137EA0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / 1, source.height / 1);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x00139D58 File Offset: 0x00137F58
		private void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}

		// Token: 0x040025DB RID: 9691
		[Range(0.0001f, 1f)]
		public float adaptationSpeed = 0.02f;

		// Token: 0x040025DC RID: 9692
		[Range(0f, 1f)]
		public float limitMinimum = 0.2f;

		// Token: 0x040025DD RID: 9693
		[Range(0f, 1f)]
		public float limitMaximum = 0.6f;

		// Token: 0x040025DE RID: 9694
		private RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x040025DF RID: 9695
		private int curAdaptIndex;

		// Token: 0x040025E0 RID: 9696
		public Shader shaderLum;

		// Token: 0x040025E1 RID: 9697
		private Material m_materialLum;

		// Token: 0x040025E2 RID: 9698
		public Shader shaderReduce;

		// Token: 0x040025E3 RID: 9699
		private Material m_materialReduce;

		// Token: 0x040025E4 RID: 9700
		public Shader shaderAdapt;

		// Token: 0x040025E5 RID: 9701
		private Material m_materialAdapt;

		// Token: 0x040025E6 RID: 9702
		public Shader shaderApply;

		// Token: 0x040025E7 RID: 9703
		private Material m_materialApply;
	}
}
