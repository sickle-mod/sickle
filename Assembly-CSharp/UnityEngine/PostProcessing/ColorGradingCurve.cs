using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077E RID: 1918
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x060037A6 RID: 14246 RVA: 0x0004BB3C File Offset: 0x00049D3C
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x00147890 File Offset: 0x00145A90
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe keyframe = this.curve[length - 1];
			keyframe.time -= this.m_Range;
			Keyframe keyframe2 = this.curve[0];
			keyframe2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(keyframe);
			this.m_InternalLoopingCurve.AddKey(keyframe2);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x00147940 File Offset: 0x00145B40
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x040029C8 RID: 10696
		public AnimationCurve curve;

		// Token: 0x040029C9 RID: 10697
		[SerializeField]
		private bool m_Loop;

		// Token: 0x040029CA RID: 10698
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x040029CB RID: 10699
		[SerializeField]
		private float m_Range;

		// Token: 0x040029CC RID: 10700
		private AnimationCurve m_InternalLoopingCurve;
	}
}
