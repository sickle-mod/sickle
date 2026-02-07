using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000841 RID: 2113
	public static class Ease
	{
		// Token: 0x06003BD1 RID: 15313 RVA: 0x00153CE0 File Offset: 0x00151EE0
		public static Easer FromType(EaseType type)
		{
			switch (type)
			{
			case EaseType.Linear:
				return Ease.Linear;
			case EaseType.QuadIn:
				return Ease.QuadIn;
			case EaseType.QuadOut:
				return Ease.QuadOut;
			case EaseType.QuadInOut:
				return Ease.QuadInOut;
			case EaseType.CubeIn:
				return Ease.CubeIn;
			case EaseType.CubeOut:
				return Ease.CubeOut;
			case EaseType.CubeInOut:
				return Ease.CubeInOut;
			case EaseType.BackIn:
				return Ease.BackIn;
			case EaseType.BackOut:
				return Ease.BackOut;
			case EaseType.BackInOut:
				return Ease.BackInOut;
			case EaseType.ExpoIn:
				return Ease.ExpoIn;
			case EaseType.ExpoOut:
				return Ease.ExpoOut;
			case EaseType.ExpoInOut:
				return Ease.ExpoInOut;
			case EaseType.SineIn:
				return Ease.SineIn;
			case EaseType.SineOut:
				return Ease.SineOut;
			case EaseType.SineInOut:
				return Ease.SineInOut;
			case EaseType.ElasticIn:
				return Ease.ElasticIn;
			case EaseType.ElasticOut:
				return Ease.ElasticOut;
			case EaseType.ElasticInOut:
				return Ease.ElasticInOut;
			default:
				return Ease.Linear;
			}
		}

		// Token: 0x04002D7B RID: 11643
		public static readonly Easer Linear = (float t) => t;

		// Token: 0x04002D7C RID: 11644
		public static readonly Easer QuadIn = (float t) => t * t;

		// Token: 0x04002D7D RID: 11645
		public static readonly Easer QuadOut = (float t) => 1f - Ease.QuadIn(1f - t);

		// Token: 0x04002D7E RID: 11646
		public static readonly Easer QuadInOut = delegate(float t)
		{
			if (t > 0.5f)
			{
				return Ease.QuadOut(t * 2f - 1f) / 2f + 0.5f;
			}
			return Ease.QuadIn(t * 2f) / 2f;
		};

		// Token: 0x04002D7F RID: 11647
		public static readonly Easer CubeIn = (float t) => t * t * t;

		// Token: 0x04002D80 RID: 11648
		public static readonly Easer CubeOut = (float t) => 1f - Ease.CubeIn(1f - t);

		// Token: 0x04002D81 RID: 11649
		public static readonly Easer CubeInOut = delegate(float t)
		{
			if (t > 0.5f)
			{
				return Ease.CubeOut(t * 2f - 1f) / 2f + 0.5f;
			}
			return Ease.CubeIn(t * 2f) / 2f;
		};

		// Token: 0x04002D82 RID: 11650
		public static readonly Easer BackIn = (float t) => t * t * (2.70158f * t - 1.70158f);

		// Token: 0x04002D83 RID: 11651
		public static readonly Easer BackOut = (float t) => 1f - Ease.BackIn(1f - t);

		// Token: 0x04002D84 RID: 11652
		public static readonly Easer BackInOut = delegate(float t)
		{
			if (t > 0.5f)
			{
				return Ease.BackOut(t * 2f - 1f) / 2f + 0.5f;
			}
			return Ease.BackIn(t * 2f) / 2f;
		};

		// Token: 0x04002D85 RID: 11653
		public static readonly Easer ExpoIn = (float t) => Mathf.Pow(2f, 10f * (t - 1f));

		// Token: 0x04002D86 RID: 11654
		public static readonly Easer ExpoOut = (float t) => 1f - Ease.ExpoIn(t);

		// Token: 0x04002D87 RID: 11655
		public static readonly Easer ExpoInOut = delegate(float t)
		{
			if (t >= 0.5f)
			{
				return Ease.ExpoOut(t * 2f) / 2f;
			}
			return Ease.ExpoIn(t * 2f) / 2f;
		};

		// Token: 0x04002D88 RID: 11656
		public static readonly Easer SineIn = (float t) => -Mathf.Cos(1.5707964f * t) + 1f;

		// Token: 0x04002D89 RID: 11657
		public static readonly Easer SineOut = (float t) => Mathf.Sin(1.5707964f * t);

		// Token: 0x04002D8A RID: 11658
		public static readonly Easer SineInOut = (float t) => -Mathf.Cos(3.1415927f * t) / 2f + 0.5f;

		// Token: 0x04002D8B RID: 11659
		public static readonly Easer ElasticIn = (float t) => 1f - Ease.ElasticOut(1f - t);

		// Token: 0x04002D8C RID: 11660
		public static readonly Easer ElasticOut = (float t) => Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - 0.075f) * 6.2831855f / 0.3f) + 1f;

		// Token: 0x04002D8D RID: 11661
		public static readonly Easer ElasticInOut = delegate(float t)
		{
			if (t > 0.5f)
			{
				return Ease.ElasticOut(t * 2f - 1f) / 2f + 0.5f;
			}
			return Ease.ElasticIn(t * 2f) / 2f;
		};
	}
}
