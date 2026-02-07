using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class ColorFactionMarks : MonoBehaviour
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002C1C1 File Offset: 0x0002A3C1
	// (set) Token: 0x0600066E RID: 1646 RVA: 0x0002C1C8 File Offset: 0x0002A3C8
	public static Dictionary<Faction, ColorFactionMarks.ColorMarks> colorFactionMarks { get; private set; }

	// Token: 0x0600066F RID: 1647 RVA: 0x0006FD00 File Offset: 0x0006DF00
	private void Awake()
	{
		if (ColorFactionMarks.colorFactionMarks == null)
		{
			ColorFactionMarks.colorFactionMarks = new Dictionary<Faction, ColorFactionMarks.ColorMarks>();
		}
		else
		{
			ColorFactionMarks.colorFactionMarks.Clear();
		}
		for (int i = 0; i < this.colorMarks.Length; i++)
		{
			ColorFactionMarks.colorFactionMarks.Add(this.colorMarks[i].faction, this.colorMarks[i]);
		}
	}

	// Token: 0x0400057C RID: 1404
	public ColorFactionMarks.ColorMarks[] colorMarks;

	// Token: 0x020000DB RID: 219
	[Serializable]
	public class ColorMarks
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x0006FD5C File Offset: 0x0006DF5C
		public Sprite GetProperStarsImage(int numberOfStars)
		{
			switch (numberOfStars)
			{
			case 1:
				return this.starMark;
			case 2:
				return this.starsMarks[0];
			case 3:
				return this.starsMarks[1];
			case 4:
				return this.starsMarks[2];
			case 5:
				return this.starsMarks[3];
			case 6:
				return this.starsMarks[4];
			default:
				return this.starMark;
			}
		}

		// Token: 0x0400057D RID: 1405
		public Faction faction;

		// Token: 0x0400057E RID: 1406
		public Sprite starMark;

		// Token: 0x0400057F RID: 1407
		public Sprite powerMark;

		// Token: 0x04000580 RID: 1408
		public Sprite popularityMark;

		// Token: 0x04000581 RID: 1409
		public Sprite[] starsMarks;
	}
}
