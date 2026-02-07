using System;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000436 RID: 1078
	[Serializable]
	public class CombatControls
	{
		// Token: 0x06002118 RID: 8472 RVA: 0x0003D238 File Offset: 0x0003B438
		public Transform FactionUnits(Faction faction)
		{
			return this.units.GetChild((int)faction);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0003D246 File Offset: 0x0003B446
		public Image FactionCharacter(Faction faction)
		{
			return this.characters.GetChild((int)faction).GetComponent<Image>();
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0003D259 File Offset: 0x0003B459
		public Transform Bomb(int id)
		{
			return this.bombs.GetChild(id);
		}

		// Token: 0x0400172B RID: 5931
		public Image factionLogo;

		// Token: 0x0400172C RID: 5932
		public Transform units;

		// Token: 0x0400172D RID: 5933
		public Transform characters;

		// Token: 0x0400172E RID: 5934
		public Image worker;

		// Token: 0x0400172F RID: 5935
		public TextMeshProUGUI label;

		// Token: 0x04001730 RID: 5936
		public Text overallPower;

		// Token: 0x04001731 RID: 5937
		public Text possiblePowerRange;

		// Token: 0x04001732 RID: 5938
		public Transform bombs;

		// Token: 0x04001733 RID: 5939
		public Slider powerSlider;

		// Token: 0x04001734 RID: 5940
		public Slider powerUnavaliableSlider;
	}
}
