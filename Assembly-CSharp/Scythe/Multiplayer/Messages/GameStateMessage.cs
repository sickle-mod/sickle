using System;
using Scythe.GameLogic;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E8 RID: 744
	public class GameStateMessage : Message, IExecutableMessage
	{
		// Token: 0x06001626 RID: 5670 RVA: 0x00037376 File Offset: 0x00035576
		public GameStateMessage(string state)
		{
			this.state = state;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00037385 File Offset: 0x00035585
		public void Execute(GameManager gameManager)
		{
			gameManager = GameSerializer.DeserializeObject<GameManager>(this.state);
			gameManager.SynchronizeGame();
			MultiplayerController.Instance.NextTurn();
			PlayerClock.StartTimer();
		}

		// Token: 0x04001061 RID: 4193
		private string state;
	}
}
