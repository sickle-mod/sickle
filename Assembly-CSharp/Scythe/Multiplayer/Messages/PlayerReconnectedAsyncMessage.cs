using System;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002ED RID: 749
	[JsonObject(MemberSerialization.Fields)]
	public class PlayerReconnectedAsyncMessage : Message, IExecutableMessage
	{
		// Token: 0x06001630 RID: 5680 RVA: 0x000373D7 File Offset: 0x000355D7
		public PlayerReconnectedAsyncMessage(Guid id)
		{
			this.id = id;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0009F658 File Offset: 0x0009D858
		public void Execute(GameManager gameManager)
		{
			PlayerData playerDataById = MultiplayerController.Instance.GetPlayerDataById(this.id);
			playerDataById.IsOnline = true;
			ChatMessage chatMessage = new ChatMessage(false, false, "^" + ((Faction)playerDataById.Faction).ToString(), playerDataById.Name, "<color=green>" + ScriptLocalization.Get("GameScene/PlayerReturned") + "</color>");
			MultiplayerController.Instance.ReceivedChatMessage(chatMessage);
		}

		// Token: 0x04001065 RID: 4197
		private Guid id;
	}
}
