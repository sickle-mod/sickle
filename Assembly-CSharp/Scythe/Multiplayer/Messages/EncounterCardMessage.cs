using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E2 RID: 738
	public class EncounterCardMessage : Message, IExecutableMessage
	{
		// Token: 0x06001616 RID: 5654 RVA: 0x000372A3 File Offset: 0x000354A3
		public EncounterCardMessage(string id, int faction)
		{
			this.cardId = id;
			this.faction = faction;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x000372B9 File Offset: 0x000354B9
		public void Execute(GameManager gameManager)
		{
			gameManager.ShowEncounterCard(int.Parse(this.cardId), this.faction);
		}

		// Token: 0x04001052 RID: 4178
		private string cardId;

		// Token: 0x04001053 RID: 4179
		private int faction;
	}
}
