using System;
using I2.Loc;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002EE RID: 750
	public class PlayerReconnectedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001632 RID: 5682 RVA: 0x0009F6D0 File Offset: 0x0009D8D0
		public void Execute(GameManager gameManager)
		{
			bool flag = false;
			bool flag2 = false;
			string text = "^";
			Faction faction = (Faction)this.faction;
			ChatMessage chatMessage = new ChatMessage(flag, flag2, text + faction.ToString(), this.name, "<color=green>" + ScriptLocalization.Get("GameScene/PlayerReturned") + "</color>");
			MultiplayerController.Instance.ReceivedChatMessage(chatMessage);
			gameManager.OverrideAIWithPlayer(this.faction);
			MultiplayerController.Instance.OverrideAiWithPlayer(this.name, this.id, this.faction, this.playerClock);
		}

		// Token: 0x04001066 RID: 4198
		private string name;

		// Token: 0x04001067 RID: 4199
		private Guid id;

		// Token: 0x04001068 RID: 4200
		private int faction;

		// Token: 0x04001069 RID: 4201
		private int playerClock;
	}
}
