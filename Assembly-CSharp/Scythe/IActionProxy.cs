using System;
using Scythe.GameLogic.Actions;

namespace Scythe
{
	// Token: 0x020001CC RID: 460
	public interface IActionProxy
	{
		// Token: 0x06000D6A RID: 3434
		void SectionActionFinished();

		// Token: 0x06000D6B RID: 3435
		void SectionActionSelected(SectionAction action, int gainActionId = -1);
	}
}
