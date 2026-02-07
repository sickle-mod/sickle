using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B7 RID: 695
	public class UnloadAllResourcesMessage : Message, IExecutableMessage
	{
		// Token: 0x060015AF RID: 5551 RVA: 0x00036C62 File Offset: 0x00034E62
		public void Execute(GameManager gameManager)
		{
			gameManager.moveManager.UnloadResources();
		}
	}
}
