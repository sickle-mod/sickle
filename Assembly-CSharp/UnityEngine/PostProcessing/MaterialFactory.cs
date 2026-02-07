using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000780 RID: 1920
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x0004BB8D File Offset: 0x00049D8D
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x00147CD0 File Offset: 0x00145ED0
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = HideFlags.DontSave
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x00147D4C File Offset: 0x00145F4C
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				GraphicsUtils.Destroy(keyValuePair.Value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x040029CF RID: 10703
		private Dictionary<string, Material> m_Materials;
	}
}
