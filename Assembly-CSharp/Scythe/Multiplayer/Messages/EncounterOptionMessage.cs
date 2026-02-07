using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002D7 RID: 727
	public class EncounterOptionMessage : Message, IExecutableMessage
	{
		// Token: 0x06001604 RID: 5636 RVA: 0x00037155 File Offset: 0x00035355
		public EncounterOptionMessage(int option)
		{
			this.option = option;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00037164 File Offset: 0x00035364
		public void Execute(GameManager gameManager)
		{
			gameManager.actionManager.BreakSectionAction(false);
			gameManager.actionManager.SetSectionAction(gameManager.LastEncounterCard.GetAction(this.option), null, this.option);
			gameManager.EncounterOption(this.option);
		}

		// Token: 0x04001041 RID: 4161
		private int option;
	}
}
