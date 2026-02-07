using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000665 RID: 1637
	[Serializable]
	public class CurveControlledBob
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x00132D20 File Offset: 0x00130F20
		public void Setup(Camera camera, float bobBaseInterval)
		{
			this.m_BobBaseInterval = bobBaseInterval;
			this.m_OriginalCameraPosition = camera.transform.localPosition;
			this.m_Time = this.Bobcurve[this.Bobcurve.length - 1].time;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x00132D6C File Offset: 0x00130F6C
		public Vector3 DoHeadBob(float speed)
		{
			float num = this.m_OriginalCameraPosition.x + this.Bobcurve.Evaluate(this.m_CyclePositionX) * this.HorizontalBobRange;
			float num2 = this.m_OriginalCameraPosition.y + this.Bobcurve.Evaluate(this.m_CyclePositionY) * this.VerticalBobRange;
			this.m_CyclePositionX += speed * Time.deltaTime / this.m_BobBaseInterval;
			this.m_CyclePositionY += speed * Time.deltaTime / this.m_BobBaseInterval * this.VerticaltoHorizontalRatio;
			if (this.m_CyclePositionX > this.m_Time)
			{
				this.m_CyclePositionX -= this.m_Time;
			}
			if (this.m_CyclePositionY > this.m_Time)
			{
				this.m_CyclePositionY -= this.m_Time;
			}
			return new Vector3(num, num2, 0f);
		}

		// Token: 0x040023F9 RID: 9209
		public float HorizontalBobRange = 0.33f;

		// Token: 0x040023FA RID: 9210
		public float VerticalBobRange = 0.33f;

		// Token: 0x040023FB RID: 9211
		public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f),
			new Keyframe(1.5f, -1f),
			new Keyframe(2f, 0f)
		});

		// Token: 0x040023FC RID: 9212
		public float VerticaltoHorizontalRatio = 1f;

		// Token: 0x040023FD RID: 9213
		private float m_CyclePositionX;

		// Token: 0x040023FE RID: 9214
		private float m_CyclePositionY;

		// Token: 0x040023FF RID: 9215
		private float m_BobBaseInterval;

		// Token: 0x04002400 RID: 9216
		private Vector3 m_OriginalCameraPosition;

		// Token: 0x04002401 RID: 9217
		private float m_Time;
	}
}
