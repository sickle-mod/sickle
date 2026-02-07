using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200096E RID: 2414
	public abstract class AnalyticsContext
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060040EA RID: 16618 RVA: 0x00051BAE File Offset: 0x0004FDAE
		public float LifeTime
		{
			get
			{
				return Time.unscaledTime - this._startContextTime;
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00051BBC File Offset: 0x0004FDBC
		public AnalyticsContext()
		{
			this._startContextTime = Time.unscaledTime;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00051BCF File Offset: 0x0004FDCF
		public virtual void Pause()
		{
			this._startPauseTime = Time.unscaledTime;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00051BDC File Offset: 0x0004FDDC
		public virtual void Resume()
		{
			this._startContextTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Quit()
		{
		}

		// Token: 0x04003138 RID: 12600
		private float _startContextTime;

		// Token: 0x04003139 RID: 12601
		protected float _startPauseTime;
	}
}
