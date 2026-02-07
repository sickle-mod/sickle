using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000018 RID: 24
public class EndPointsCountingElement : MonoBehaviour
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600005E RID: 94 RVA: 0x000282F1 File Offset: 0x000264F1
	// (set) Token: 0x0600005F RID: 95 RVA: 0x000282F9 File Offset: 0x000264F9
	public int ScoreTarget { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000060 RID: 96 RVA: 0x00028302 File Offset: 0x00026502
	// (set) Token: 0x06000061 RID: 97 RVA: 0x0002830A File Offset: 0x0002650A
	public float ScoreCurrent { get; set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000062 RID: 98 RVA: 0x00028313 File Offset: 0x00026513
	// (set) Token: 0x06000063 RID: 99 RVA: 0x0002831B File Offset: 0x0002651B
	public List<GameObject> Stars { get; set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00028324 File Offset: 0x00026524
	// (set) Token: 0x06000065 RID: 101 RVA: 0x000536A4 File Offset: 0x000518A4
	public int LitPopularityIconLevel
	{
		get
		{
			return this.litPopularityIconLevel;
		}
		set
		{
			this.litPopularityIconLevel = value;
			for (int i = 0; i < this.popularityIcon.transform.childCount; i++)
			{
				this.popularityIcon.transform.GetChild(i).gameObject.SetActive(i == value);
			}
		}
	}

	// Token: 0x0400004F RID: 79
	public Faction faction;

	// Token: 0x04000050 RID: 80
	public GameObject logo;

	// Token: 0x04000051 RID: 81
	public GameObject popularityGate;

	// Token: 0x04000052 RID: 82
	public GameObject popularityIcon;

	// Token: 0x04000053 RID: 83
	public Text popularityText;

	// Token: 0x04000054 RID: 84
	public Text multiplierText;

	// Token: 0x04000055 RID: 85
	public Text scoreText;

	// Token: 0x04000056 RID: 86
	public GameObject hexCounter;

	// Token: 0x04000057 RID: 87
	public GameObject resourcesCounter;

	// Token: 0x04000058 RID: 88
	public GameObject structureCounter;

	// Token: 0x04000059 RID: 89
	public GameObject coinCounter;

	// Token: 0x0400005A RID: 90
	public GameObject trapsCounter;

	// Token: 0x0400005B RID: 91
	public GameObject flagsCounter;

	// Token: 0x0400005C RID: 92
	public GameObject encountersCounter;

	// Token: 0x0400005D RID: 93
	private int litPopularityIconLevel;
}
