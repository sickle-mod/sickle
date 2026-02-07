using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D9 RID: 729
	public class ForceStartMessage : Message, IExecutableMessage
	{
		// Token: 0x06001608 RID: 5640 RVA: 0x00037200 File Offset: 0x00035400
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.ForceStart();
		}
	}
}
