using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200066E RID: 1646
	[Serializable]
	public class LerpControlledBob
	{
		// Token: 0x060033E3 RID: 13283 RVA: 0x00048C5E File Offset: 0x00046E5E
		public float Offset()
		{
			return this.m_Offset;
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x00048C66 File Offset: 0x00046E66
		public IEnumerator DoBobCycle()
		{
			float t = 0f;
			while (t < this.BobDuration)
			{
				this.m_Offset = Mathf.Lerp(0f, this.BobAmount, t / this.BobDuration);
				t += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			t = 0f;
			while (t < this.BobDuration)
			{
				this.m_Offset = Mathf.Lerp(this.BobAmount, 0f, t / this.BobDuration);
				t += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			this.m_Offset = 0f;
			yield break;
		}

		// Token: 0x04002431 RID: 9265
		public float BobDuration;

		// Token: 0x04002432 RID: 9266
		public float BobAmount;

		// Token: 0x04002433 RID: 9267
		private float m_Offset;
	}
}
