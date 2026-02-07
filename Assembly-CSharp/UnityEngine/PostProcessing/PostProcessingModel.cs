using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077C RID: 1916
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x0004BB22 File Offset: 0x00049D22
		// (set) Token: 0x060037A1 RID: 14241 RVA: 0x0004BB2A File Offset: 0x00049D2A
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x060037A2 RID: 14242
		public abstract void Reset();

		// Token: 0x060037A3 RID: 14243 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void OnValidate()
		{
		}

		// Token: 0x040029B8 RID: 10680
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
