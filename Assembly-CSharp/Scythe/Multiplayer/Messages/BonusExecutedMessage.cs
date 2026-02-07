using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C2 RID: 706
	public class BonusExecutedMessage : Message, IExecutableMessage
	{
		// Token: 0x060015CE RID: 5582 RVA: 0x00036DA5 File Offset: 0x00034FA5
		public BonusExecutedMessage(int faction, int gainType, short amount)
		{
			this.faction = faction;
			this.gainType = gainType;
			this.amount = amount;
			MultiplayerController.Instance.SetCurrentPlayer();
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0009EC30 File Offset: 0x0009CE30
		public void Execute(GameManager gameManager)
		{
			MultiplayerController.Instance.SetCurrentPlayer();
			GainAction gainAction = null;
			switch (this.gainType)
			{
			case 0:
				gainAction = new GainCoin(gameManager, this.amount, 0, false, false, false);
				(gainAction as GainCoin).SetCoins(this.amount);
				break;
			case 1:
				gainAction = new GainPopularity(gameManager, this.amount, 0, false, false, false);
				(gainAction as GainPopularity).SetPopularity(this.amount);
				break;
			case 2:
				gainAction = new GainPower(gameManager, this.amount, 0, false, false, false);
				(gainAction as GainPower).SetPower(this.amount);
				break;
			case 3:
				if (gameManager.IsMyTurn())
				{
					gameManager.OnInputEnabled(true);
				}
				gameManager.actionManager.PrepareNextAction();
				return;
			}
			gainAction.SetPlayer(gameManager.GetPlayerByFaction((Faction)this.faction));
			gameManager.actionManager.SetBonusAction(gainAction);
			gameManager.actionManager.PrepareNextAction();
			if (gameManager.IsMyTurn() && gameManager.actionManager.GetLastBonusAction() == null)
			{
				gameManager.OnInputEnabled(true);
			}
			gameManager.OnActionFinished();
		}

		// Token: 0x04001010 RID: 4112
		private int faction;

		// Token: 0x04001011 RID: 4113
		private int gainType;

		// Token: 0x04001012 RID: 4114
		private short amount;
	}
}
