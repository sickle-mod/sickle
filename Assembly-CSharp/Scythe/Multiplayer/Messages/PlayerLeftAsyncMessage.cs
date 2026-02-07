using System;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002EB RID: 747
	[JsonObject(MemberSerialization.Fields)]
	public class PlayerLeftAsyncMessage : Message, IExecutableMessage
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x000373C8 File Offset: 0x000355C8
		public PlayerLeftAsyncMessage(Guid id)
		{
			this.id = id;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0009F51C File Offset: 0x0009D71C
		public void Execute(GameManager gameManager)
		{
			PlayerData playerDataById = MultiplayerController.Instance.GetPlayerDataById(this.id);
			if (!playerDataById.IsOnline)
			{
				return;
			}
			playerDataById.IsOnline = false;
			string text = ((playerDataById != null) ? playerDataById.Name : " ");
			ChatMessage chatMessage = new ChatMessage(false, false, "^" + ((Faction)playerDataById.Faction).ToString(), text, "<color=red>" + ScriptLocalization.Get("GameScene/PlayerLeft") + "</color>");
			MultiplayerController.Instance.ReceivedChatMessage(chatMessage);
		}

		// Token: 0x04001062 RID: 4194
		private Guid id;
	}
}
