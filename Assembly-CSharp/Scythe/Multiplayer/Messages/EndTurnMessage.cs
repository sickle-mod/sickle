using System;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D8 RID: 728
	public class EndTurnMessage : Message, IExecutableMessage
	{
		// Token: 0x06001606 RID: 5638 RVA: 0x000371A1 File Offset: 0x000353A1
		public EndTurnMessage(GameManager gameManager)
		{
			this.faction = PlayerInfo.me.Faction;
			this.timeLeft = MultiplayerController.Instance.GetOwnerPlayer.PlayerClock;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x000371CE File Offset: 0x000353CE
		public void Execute(GameManager gameManager)
		{
			PlayerClock.StopTimer();
			gameManager.EndTurn();
			MultiplayerController.Instance.NextTurn();
			MultiplayerController.Instance.UpdatePlayerTimeLeft(this.faction, this.timeLeft);
			PlayerClock.StartTimer();
		}

		// Token: 0x04001042 RID: 4162
		private int faction;

		// Token: 0x04001043 RID: 4163
		private int timeLeft;
	}
}
