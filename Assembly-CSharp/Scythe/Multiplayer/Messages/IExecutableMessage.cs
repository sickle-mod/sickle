using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002F1 RID: 753
	public interface IExecutableMessage
	{
		// Token: 0x06001639 RID: 5689
		void Execute(GameManager gameManager);
	}
}
