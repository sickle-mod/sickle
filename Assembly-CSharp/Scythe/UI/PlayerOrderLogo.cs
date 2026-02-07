using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200046A RID: 1130
	public class PlayerOrderLogo : MonoBehaviour
	{
		// Token: 0x060023A7 RID: 9127 RVA: 0x000D347C File Offset: 0x000D167C
		public void SetupStarsImage(Sprite factionStar)
		{
			for (int i = 0; i < this.stars.Length; i++)
			{
				this.stars[i].sprite = factionStar;
			}
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0003ECEC File Offset: 0x0003CEEC
		public void SetupEmblemImage(Sprite emblem)
		{
			this.factioEmblem.sprite = emblem;
		}

		// Token: 0x040018CB RID: 6347
		[SerializeField]
		private Image[] stars;

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private Image factioEmblem;
	}
}
