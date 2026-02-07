using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x02000307 RID: 775
	public class RecruitBonusMessage : Message, IExecutableMessage
	{
		// Token: 0x0600167D RID: 5757 RVA: 0x000376B0 File Offset: 0x000358B0
		public RecruitBonusMessage(int senderFaction, int toFaction, int gainType)
		{
			this.fromFaction = senderFaction;
			this.toFaction = toFaction;
			this.gainType = gainType;
			MultiplayerController.Instance.SetCurrentPlayer(toFaction);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0009FA94 File Offset: 0x0009DC94
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.SetCurrentPlayer(this.toFaction);
			if (this.toFaction != (int)gameManager.PlayerOwner.matFaction.faction || gameManager.SpectatorMode)
			{
				return;
			}
			GainAction gainAction = null;
			switch (this.gainType)
			{
			case 0:
				gainAction = new GainCoin(gameManager, 1, 0, false, false, true);
				break;
			case 1:
				gainAction = new GainPopularity(gameManager, 1, 0, false, false, true);
				break;
			case 2:
				gainAction = new GainPower(gameManager, 1, 0, false, false, true);
				break;
			case 3:
				gainAction = new GainCombatCard(gameManager, 1, 0, false, false, true);
				break;
			}
			gainAction.SetPlayer(gameManager.PlayerOwner);
			gameManager.OnGainOngoingRecruitBonus(gainAction);
			gameManager.OnActionFinished();
		}

		// Token: 0x0400108E RID: 4238
		private int fromFaction;

		// Token: 0x0400108F RID: 4239
		private int toFaction;

		// Token: 0x04001090 RID: 4240
		private int gainType;
	}
}
