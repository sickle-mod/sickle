using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000781 RID: 1921
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x0004BBA0 File Offset: 0x00049DA0
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00147D90 File Offset: 0x00145F90
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, baseRenderTexture.sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x00147DD8 File Offset: 0x00145FD8
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, RenderTextureReadWrite rw = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format, rw);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0004BBB3 File Offset: 0x00049DB3
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x00147E18 File Offset: 0x00146018
		public void ReleaseAll()
		{
			foreach (RenderTexture renderTexture in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0004BBF1 File Offset: 0x00049DF1
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x040029D0 RID: 10704
		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}
