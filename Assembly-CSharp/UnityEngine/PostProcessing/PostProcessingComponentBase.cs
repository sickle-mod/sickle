using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000777 RID: 1911
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06003785 RID: 14213 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06003786 RID: 14214
		public abstract bool active { get; }

		// Token: 0x06003787 RID: 14215 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void OnEnable()
		{
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void OnDisable()
		{
		}

		// Token: 0x06003789 RID: 14217
		public abstract PostProcessingModel GetModel();

		// Token: 0x040029B1 RID: 10673
		public PostProcessingContext context;
	}
}
