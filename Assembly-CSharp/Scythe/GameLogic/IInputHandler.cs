using System;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005C7 RID: 1479
	public interface IInputHandler
	{
		// Token: 0x06002F29 RID: 12073
		void EnableInputForPayAction(PayAction action);

		// Token: 0x06002F2A RID: 12074
		void EnableInputForGainAction(GainAction action);

		// Token: 0x06002F2B RID: 12075
		void OnBreakSectionAction();

		// Token: 0x06002F2C RID: 12076
		void OnInputEnded();
	}
}
