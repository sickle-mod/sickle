using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002CB RID: 715
	public class EndNordicSkillMessage : Message, IExecutableMessage
	{
		// Token: 0x060015EB RID: 5611 RVA: 0x00036F6D File Offset: 0x0003516D
		public EndNordicSkillMessage(int faction)
		{
			this.faction = faction;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00036F86 File Offset: 0x00035186
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.SetCurrentPlayer();
			gameManager.OnActionFinished();
			gameManager.combatManager.SwitchToNextStage();
		}

		// Token: 0x04001025 RID: 4133
		private int faction;
	}
}
