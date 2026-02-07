using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000053 RID: 83
[CreateAssetMenu(fileName = "NewFactionData", menuName = "FactionData")]
public class FactionsInfoData : ScriptableObject
{
	// Token: 0x040001FE RID: 510
	public List<FactionNameColor> FactionNameColors = new List<FactionNameColor>();
}
