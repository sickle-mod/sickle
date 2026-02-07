using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CD RID: 1741
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x06003532 RID: 13618 RVA: 0x0013D26C File Offset: 0x0013B46C
		protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				base.enabled = false;
				return null;
			}
			if (s.isSupported && m2Create && m2Create.shader == s)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				this.NotSupported();
				Debug.Log(string.Concat(new string[]
				{
					"The shader ",
					s.ToString(),
					" on effect ",
					this.ToString(),
					" is not supported on this platform!"
				}));
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x0013D328 File Offset: 0x0013B528
		protected Material CreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				return null;
			}
			if (m2Create && m2Create.shader == s && s.isSupported)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00049C81 File Offset: 0x00047E81
		private void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00049C8A File Offset: 0x00047E8A
		private void OnDestroy()
		{
			this.RemoveCreatedMaterials();
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00049C92 File Offset: 0x00047E92
		private void RemoveCreatedMaterials()
		{
			while (this.createdMaterials.Count > 0)
			{
				global::UnityEngine.Object @object = this.createdMaterials[0];
				this.createdMaterials.RemoveAt(0);
				global::UnityEngine.Object.Destroy(@object);
			}
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00049CC1 File Offset: 0x00047EC1
		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00049CCA File Offset: 0x00047ECA
		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00049CEC File Offset: 0x00047EEC
		protected void Start()
		{
			this.CheckResources();
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x0013D39C File Offset: 0x0013B59C
		protected bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			this.supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.NotSupported();
				return false;
			}
			if (needDepth)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			return true;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x00049CF5 File Offset: 0x00047EF5
		protected bool CheckSupport(bool needDepth, bool needHdr)
		{
			if (!this.CheckSupport(needDepth))
			{
				return false;
			}
			if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				return false;
			}
			return true;
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x00049D16 File Offset: 0x00047F16
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x00049D1E File Offset: 0x00047F1E
		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0013D400 File Offset: 0x0013B600
		private bool CheckShader(Shader s)
		{
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
			}));
			if (!s.isSupported)
			{
				this.NotSupported();
				return false;
			}
			return false;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x00049D3A File Offset: 0x00047F3A
		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x0013D458 File Offset: 0x0013B658
		protected void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float num;
				float num2;
				if (flag)
				{
					num = 1f;
					num2 = 0f;
				}
				else
				{
					num = 0f;
					num2 = 1f;
				}
				float num3 = 0f;
				float num4 = 0f + 1f / ((float)dest.width * 1f);
				float num5 = 0f;
				float num6 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				float num7 = 1f - 1f / ((float)dest.width * 1f);
				num4 = 1f;
				num5 = 0f;
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num7, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num7, num6, 0.1f);
				float num8 = 0f;
				num4 = 1f;
				num5 = 0f;
				num6 = 0f + 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num8, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num8, num6, 0.1f);
				float num9 = 0f;
				num4 = 1f;
				num5 = 1f - 1f / ((float)dest.height * 1f);
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num9, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num9, num6, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x0400269D RID: 9885
		protected bool supportHDRTextures = true;

		// Token: 0x0400269E RID: 9886
		protected bool supportDX11;

		// Token: 0x0400269F RID: 9887
		protected bool isSupported = true;

		// Token: 0x040026A0 RID: 9888
		private List<Material> createdMaterials = new List<Material>();
	}
}
