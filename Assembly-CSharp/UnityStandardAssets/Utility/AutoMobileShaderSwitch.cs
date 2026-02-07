using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200065F RID: 1631
	public class AutoMobileShaderSwitch : MonoBehaviour
	{
		// Token: 0x060033AF RID: 13231 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void OnEnable()
		{
		}

		// Token: 0x040023EA RID: 9194
		[SerializeField]
		private AutoMobileShaderSwitch.ReplacementList m_ReplacementList;

		// Token: 0x02000660 RID: 1632
		[Serializable]
		public class ReplacementDefinition
		{
			// Token: 0x040023EB RID: 9195
			public Shader original;

			// Token: 0x040023EC RID: 9196
			public Shader replacement;
		}

		// Token: 0x02000661 RID: 1633
		[Serializable]
		public class ReplacementList
		{
			// Token: 0x040023ED RID: 9197
			public AutoMobileShaderSwitch.ReplacementDefinition[] items = new AutoMobileShaderSwitch.ReplacementDefinition[0];
		}
	}
}
