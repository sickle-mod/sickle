using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000662 RID: 1634
	public class AutoMoveAndRotate : MonoBehaviour
	{
		// Token: 0x060033B3 RID: 13235 RVA: 0x00048A71 File Offset: 0x00046C71
		private void Start()
		{
			this.m_LastRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x00132C38 File Offset: 0x00130E38
		private void Update()
		{
			float num = Time.deltaTime;
			if (this.ignoreTimescale)
			{
				num = Time.realtimeSinceStartup - this.m_LastRealTime;
				this.m_LastRealTime = Time.realtimeSinceStartup;
			}
			base.transform.Translate(this.moveUnitsPerSecond.value * num, this.moveUnitsPerSecond.space);
			base.transform.Rotate(this.rotateDegreesPerSecond.value * num, this.moveUnitsPerSecond.space);
		}

		// Token: 0x040023EE RID: 9198
		public AutoMoveAndRotate.Vector3andSpace moveUnitsPerSecond;

		// Token: 0x040023EF RID: 9199
		public AutoMoveAndRotate.Vector3andSpace rotateDegreesPerSecond;

		// Token: 0x040023F0 RID: 9200
		public bool ignoreTimescale;

		// Token: 0x040023F1 RID: 9201
		private float m_LastRealTime;

		// Token: 0x02000663 RID: 1635
		[Serializable]
		public class Vector3andSpace
		{
			// Token: 0x040023F2 RID: 9202
			public Vector3 value;

			// Token: 0x040023F3 RID: 9203
			public Space space = Space.Self;
		}
	}
}
