using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000779 RID: 1913
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06003790 RID: 14224
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06003791 RID: 14225
		public abstract string GetName();

		// Token: 0x06003792 RID: 14226
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
