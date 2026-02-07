using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000778 RID: 1912
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x0004BA60 File Offset: 0x00049C60
		// (set) Token: 0x0600378C RID: 14220 RVA: 0x0004BA68 File Offset: 0x00049C68
		public T model { get; internal set; }

		// Token: 0x0600378D RID: 14221 RVA: 0x0004BA71 File Offset: 0x00049C71
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0004BA81 File Offset: 0x00049C81
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
