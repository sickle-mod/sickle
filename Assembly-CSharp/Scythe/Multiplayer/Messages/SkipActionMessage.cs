using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000308 RID: 776
	public class SkipActionMessage : Message, IExecutableMessage
	{
		// Token: 0x0600167F RID: 5759 RVA: 0x00036AA6 File Offset: 0x00034CA6
		public void Execute(GameManager gameManager)
		{
			gameManager.actionManager.PrepareNextAction();
		}
	}
}
