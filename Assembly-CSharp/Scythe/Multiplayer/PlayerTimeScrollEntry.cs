using System;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200022A RID: 554
	public class PlayerTimeScrollEntry : MonoBehaviour
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x00032C03 File Offset: 0x00030E03
		public void Init(int time)
		{
			this.text.text = time.ToString().PadLeft(2, '0');
			base.gameObject.SetActive(true);
		}

		// Token: 0x04000CBA RID: 3258
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
