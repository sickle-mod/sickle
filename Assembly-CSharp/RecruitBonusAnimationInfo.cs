using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CC RID: 204
[CreateAssetMenu(fileName = "NewRecruitBonusInfo", menuName = "RecruitBonusInfo")]
public class RecruitBonusAnimationInfo : ScriptableObject
{
	// Token: 0x0400050D RID: 1293
	public List<RecruitModelPosition> bonusMarkerPositions = new List<RecruitModelPosition>();
}
