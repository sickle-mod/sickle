using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077F RID: 1919
	public static class GraphicsUtils
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060037A9 RID: 14249 RVA: 0x0004BB66 File Offset: 0x00049D66
		public static bool isLinearColorSpace
		{
			get
			{
				return QualitySettings.activeColorSpace == ColorSpace.Linear;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060037AA RID: 14250 RVA: 0x0004A0D1 File Offset: 0x000482D1
		public static bool supportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060037AB RID: 14251 RVA: 0x00147990 File Offset: 0x00145B90
		public static Texture2D whiteTexture
		{
			get
			{
				if (GraphicsUtils.s_WhiteTexture != null)
				{
					return GraphicsUtils.s_WhiteTexture;
				}
				GraphicsUtils.s_WhiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				GraphicsUtils.s_WhiteTexture.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
				GraphicsUtils.s_WhiteTexture.Apply();
				return GraphicsUtils.s_WhiteTexture;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060037AC RID: 14252 RVA: 0x001479F4 File Offset: 0x00145BF4
		public static Mesh quad
		{
			get
			{
				if (GraphicsUtils.s_Quad != null)
				{
					return GraphicsUtils.s_Quad;
				}
				Vector3[] array = new Vector3[]
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f)
				};
				Vector2[] array2 = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f),
					new Vector2(0f, 1f)
				};
				int[] array3 = new int[] { 0, 1, 2, 1, 0, 3 };
				GraphicsUtils.s_Quad = new Mesh
				{
					vertices = array,
					uv = array2,
					triangles = array3
				};
				GraphicsUtils.s_Quad.RecalculateNormals();
				GraphicsUtils.s_Quad.RecalculateBounds();
				return GraphicsUtils.s_Quad;
			}
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x00147B30 File Offset: 0x00145D30
		public static void Blit(Material material, int pass)
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00147BEC File Offset: 0x00145DEC
		public static void ClearAndBlit(Texture source, RenderTexture destination, Material material, int pass, bool clearColor = true, bool clearDepth = false)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = destination;
			GL.Clear(false, clearColor, Color.clear);
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetTexture("_MainTex", source);
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
			RenderTexture.active = active;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0004BB70 File Offset: 0x00049D70
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
				Object.Destroy(obj);
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x0004BB81 File Offset: 0x00049D81
		public static void Dispose()
		{
			GraphicsUtils.Destroy(GraphicsUtils.s_Quad);
		}

		// Token: 0x040029CD RID: 10701
		private static Texture2D s_WhiteTexture;

		// Token: 0x040029CE RID: 10702
		private static Mesh s_Quad;
	}
}
