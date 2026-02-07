using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002EC RID: 748
	public class PlayerLeftMessage : Message, IExecutableMessage
	{
		// Token: 0x0600162E RID: 5678 RVA: 0x0009F5A8 File Offset: 0x0009D7A8
		public void Execute(GameManager gameManager)
		{
			PlayerData playerData = MultiplayerController.Instance.GetPlayerData(this.faction);
			string text = ((playerData != null) ? playerData.Name : " ");
			bool flag = false;
			bool flag2 = false;
			string text2 = "^";
			Faction faction = (Faction)this.faction;
			ChatMessage chatMessage = new ChatMessage(flag, flag2, text2 + faction.ToString(), text, "<color=red>" + ScriptLocalization.Get("GameScene/PlayerLeft") + "</color>");
			MultiplayerController.Instance.ReceivedChatMessage(chatMessage);
			gameManager.OverridePlayerWithAI(this.faction);
			MultiplayerController.Instance.OverridePlayerWithAi(this.faction);
			if (!gameManager.GameStarted)
			{
				MultiplayerController.Instance.PlayerMapLoaded(this.id);
			}
		}

		// Token: 0x04001063 RID: 4195
		private Guid id;

		// Token: 0x04001064 RID: 4196
		private int faction;
	}
}
