using System;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000392 RID: 914
	[Serializable]
	public class BoardShaderPreset
	{
		// Token: 0x06001B4F RID: 6991 RVA: 0x000AC11C File Offset: 0x000AA31C
		public void Apply(Material mat, bool dynamicDistance)
		{
			mat.SetColor("_Tint", this.tint);
			mat.SetFloat("_Strength", this.strength);
			mat.SetFloat("_FilterBrightness", this.filterBrightness);
			mat.SetFloat("_OutputBrightness", this.outputBrightness);
			mat.SetFloat("_TintModifier", this.tintModifier);
			mat.SetFloat("_DistanceDynamic", (float)(dynamicDistance ? 1 : 0));
		}

		// Token: 0x0400133B RID: 4923
		public Color tint;

		// Token: 0x0400133C RID: 4924
		public float strength;

		// Token: 0x0400133D RID: 4925
		public float filterBrightness;

		// Token: 0x0400133E RID: 4926
		public float outputBrightness;

		// Token: 0x0400133F RID: 4927
		public float tintModifier;
	}
}
