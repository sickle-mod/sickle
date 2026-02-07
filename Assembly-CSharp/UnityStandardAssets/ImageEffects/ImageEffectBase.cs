using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C8 RID: 1736
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x0600351B RID: 13595 RVA: 0x00049B91 File Offset: 0x00047D91
		protected virtual void Start()
		{
			if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x00049BB4 File Offset: 0x00047DB4
		protected Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x00049BE8 File Offset: 0x00047DE8
		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}

		// Token: 0x04002674 RID: 9844
		public Shader shader;

		// Token: 0x04002675 RID: 9845
		private Material m_Material;
	}
}
