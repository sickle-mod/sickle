using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005F6 RID: 1526
	public class EnemyActionInfo
	{
		// Token: 0x040020EF RID: 8431
		public Faction actionOwner;

		// Token: 0x040020F0 RID: 8432
		public LogInfoType actionType;

		// Token: 0x040020F1 RID: 8433
		public ActionPositionType ActionPlacement;

		// Token: 0x040020F2 RID: 8434
		public bool fromEncounter;

		// Token: 0x040020F3 RID: 8435
		public List<LogInfo> PayLogInfos = new List<LogInfo>();

		// Token: 0x040020F4 RID: 8436
		public List<LogInfo> AdditionalGain = new List<LogInfo>();
	}
}
