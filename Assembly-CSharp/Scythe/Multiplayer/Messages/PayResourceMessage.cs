using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002BF RID: 703
	public class PayResourceMessage : Message, IExecutableMessage
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x0009E768 File Offset: 0x0009C968
		public PayResourceMessage(int cardToPay, List<ResourceBundle> resources)
		{
			this.cardToPay = cardToPay;
			this.posX = new List<int>(resources.Count);
			this.posY = new List<int>(resources.Count);
			this.types = new List<int>(resources.Count);
			this.amounts = new List<int>(resources.Count);
			foreach (ResourceBundle resourceBundle in resources)
			{
				this.posX.Add(resourceBundle.gameHex.posX);
				this.posY.Add(resourceBundle.gameHex.posY);
				this.types.Add((int)resourceBundle.resourceType);
				this.amounts.Add(resourceBundle.amount);
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0009E850 File Offset: 0x0009CA50
		public void Execute(GameManager gameManager)
		{
			PayResource payResource = gameManager.actionManager.GetLastPayAction() as PayResource;
			List<ResourceBundle> list = new List<ResourceBundle>();
			for (int i = 0; i < this.posX.Count; i++)
			{
				ResourceBundle resourceBundle = new ResourceBundle
				{
					amount = this.amounts[i],
					gameHex = gameManager.gameBoard.hexMap[this.posX[i], this.posY[i]],
					resourceType = (ResourceType)this.types[i]
				};
				list.Add(resourceBundle);
			}
			payResource.SetPayed(true);
			payResource.SetResources(list, (this.cardToPay != -1) ? payResource.GetPlayer().combatCards.Find((CombatCard card) => card.CombatBonus == this.cardToPay) : null);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04001007 RID: 4103
		private int cardToPay;

		// Token: 0x04001008 RID: 4104
		private List<int> posX;

		// Token: 0x04001009 RID: 4105
		private List<int> posY;

		// Token: 0x0400100A RID: 4106
		private List<int> types;

		// Token: 0x0400100B RID: 4107
		private List<int> amounts;
	}
}
