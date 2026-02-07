using System;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000269 RID: 617
	public class DropdownItem : MonoBehaviour
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x0003481E File Offset: 0x00032A1E
		protected virtual void Start()
		{
			if (!this.player.IsOptionAvailable(base.transform.GetSiblingIndex() - 1))
			{
				this.label.alpha = 0.3f;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0003484A File Offset: 0x00032A4A
		public void OnOptionChose()
		{
			this.player.OnOptionChose(base.transform.GetSiblingIndex() - 1);
		}

		// Token: 0x04000E3D RID: 3645
		[SerializeField]
		private PlayerListEntry player;

		// Token: 0x04000E3E RID: 3646
		[SerializeField]
		private TextMeshProUGUI label;
	}
}
