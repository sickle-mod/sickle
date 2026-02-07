using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000229 RID: 553
	public class PlayerTimeScrollData : MonoBehaviour
	{
		// Token: 0x06001067 RID: 4199 RVA: 0x00032BCD File Offset: 0x00030DCD
		public void GenerateDataIfNeeded(int entriesVisibleAtOnce)
		{
			if (!this.dataGenerated)
			{
				this.GenerateData(entriesVisibleAtOnce);
			}
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00032BDE File Offset: 0x00030DDE
		public int GetValueAtIndex(int index)
		{
			return this.data[index];
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0008FC88 File Offset: 0x0008DE88
		public int GetIndexOfValue(int value)
		{
			for (int i = 0; i < this.data.Count; i++)
			{
				if (this.data[i] == value)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0008FCC0 File Offset: 0x0008DEC0
		private void GenerateData(int entriesVisibleAtOnce)
		{
			int num = entriesVisibleAtOnce / 2;
			for (int i = 0; i < num; i++)
			{
				new GameObject("Empty").transform.SetParent(this.content.transform);
			}
			this.data = new List<int>();
			for (int j = this.MIN_VALUE; j <= this.MAX_VALUE; j += this.STEP)
			{
				global::UnityEngine.Object.Instantiate<PlayerTimeScrollEntry>(this.scrollEntry, this.content.transform).Init(j);
				this.data.Add(j);
			}
			for (int k = 0; k < num; k++)
			{
				new GameObject("Empty").transform.SetParent(this.content.transform);
			}
			this.dataGenerated = true;
		}

		// Token: 0x04000CB3 RID: 3251
		[SerializeField]
		private int MIN_VALUE;

		// Token: 0x04000CB4 RID: 3252
		[SerializeField]
		private int MAX_VALUE = 30;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		private int STEP = 1;

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		private PlayerTimeScrollEntry scrollEntry;

		// Token: 0x04000CB7 RID: 3255
		[SerializeField]
		private VerticalLayoutGroup content;

		// Token: 0x04000CB8 RID: 3256
		private List<int> data;

		// Token: 0x04000CB9 RID: 3257
		private bool dataGenerated;
	}
}
