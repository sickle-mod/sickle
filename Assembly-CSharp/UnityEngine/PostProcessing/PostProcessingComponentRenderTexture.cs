using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077A RID: 1914
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06003794 RID: 14228 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Prepare(Material material)
		{
		}
	}
}
