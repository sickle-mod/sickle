using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E9 RID: 745
	public class NoCombatCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x06001628 RID: 5672 RVA: 0x000373A9 File Offset: 0x000355A9
		public void Execute(GameManager gameManager)
		{
			gameManager.InformAboutCombatCardsAmount(0);
			MultiplayerController.Instance.CombatCardsReceived();
		}
	}
}
