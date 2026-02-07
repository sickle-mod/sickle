using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006AE RID: 1710
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x000495A2 File Offset: 0x000477A2
		protected Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = HideFlags.DontSave;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000495D2 File Offset: 0x000477D2
		protected void OnDisable()
		{
			if (Blur.m_Material)
			{
				global::UnityEngine.Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000495EA File Offset: 0x000477EA
		protected void Start()
		{
			if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x001380AC File Offset: 0x001362AC
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00138118 File Offset: 0x00136318
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x0013817C File Offset: 0x0013637C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int num = source.width / 4;
			int num2 = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num, num2, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, this.rt);
			Graphics.Blit(source, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04002580 RID: 9600
		[Range(0f, 10f)]
		public int iterations = 3;

		// Token: 0x04002581 RID: 9601
		[Range(0f, 1f)]
		public float blurSpread = 0.6f;

		// Token: 0x04002582 RID: 9602
		public Shader blurShader;

		// Token: 0x04002583 RID: 9603
		public RenderTexture rt;

		// Token: 0x04002584 RID: 9604
		private static Material m_Material;
	}
}
