using System;
using Newtonsoft.Json;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E3 RID: 739
	[JsonObject(MemberSerialization.Fields)]
	public class ExecutedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001618 RID: 5656 RVA: 0x000372D2 File Offset: 0x000354D2
		public void Execute(GameManager gameManager)
		{
			RequestController.RemoveMessageFromQueue(this.messageCounter);
		}

		// Token: 0x04001054 RID: 4180
		private int messageCounter;
	}
}
