using System;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000303 RID: 771
	public class MapLoadedMessage : Message, IExecutableMessage
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x0003762E File Offset: 0x0003582E
		public MapLoadedMessage()
		{
			this.playerFaction = PlayerInfo.me.Faction;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00037646 File Offset: 0x00035846
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.PlayerMapLoadedByFaction(this.playerFaction);
		}

		// Token: 0x04001087 RID: 4231
		private int playerFaction;
	}
}
