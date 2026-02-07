using System;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005BC RID: 1468
	public class FactoryCard : MatPlayerSection
	{
		// Token: 0x06002ED2 RID: 11986 RVA: 0x0004541B File Offset: 0x0004361B
		public FactoryCard(int id, GameManager gameManager)
		{
			this.cardId = id;
			this.SetActions(gameManager);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x0011BC74 File Offset: 0x00119E74
		private void SetActions(GameManager gameManager)
		{
			base.ActionDown = new DownAction(gameManager, DownActionType.Factory);
			switch (this.cardId)
			{
			case 1:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCoin(gameManager, 1, 0, false, false),
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainProduce(gameManager, 2, 0, false, false)
				}, false);
				return;
			case 2:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayPopularity(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainRecruit(gameManager, 1, 0, false),
					new GainUpgrade(gameManager, 1, 0, false)
				}, true);
				return;
			case 3:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainPopularity(gameManager, 2, 0, false, false, false)
				}, false);
				return;
			case 4:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainCoin(gameManager, 3, 0, false, false, false)
				}, false);
				return;
			case 5:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayPower(gameManager, 1, 0, false)
				}, new GainAction[]
				{
					new GainPopularity(gameManager, 2, 0, false, false, false)
				}, false);
				return;
			case 6:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayPower(gameManager, 1, 0, false)
				}, new GainAction[]
				{
					new GainCoin(gameManager, 3, 0, false, false, false)
				}, false);
				return;
			case 7:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayPopularity(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainMech(gameManager, 1, 0, false),
					new GainBuilding(gameManager, 1, 0, false)
				}, true);
				return;
			case 8:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainAnyResource(gameManager, 3, 0, false)
				}, false);
				return;
			case 9:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCoin(gameManager, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainUpgrade(gameManager, 1, 0, false),
					new GainPopularity(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 10:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCoin(gameManager, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainMech(gameManager, 1, 0, false),
					new GainPower(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 11:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCoin(gameManager, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainBuilding(gameManager, 1, 0, false),
					new GainPopularity(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 12:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCoin(gameManager, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainRecruit(gameManager, 1, 0, false),
					new GainPower(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 13:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayResource(gameManager, true, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainMech(gameManager, 1, 0, false),
					new GainBuilding(gameManager, 1, 0, false)
				}, true);
				return;
			case 14:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainWorker(gameManager, 1, 0, false),
					new GainCoin(gameManager, 2, 0, false, false, false)
				}, false);
				return;
			case 15:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayResource(gameManager, false, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainCombatCard(gameManager, 1, 0, false, false, false),
					new GainPower(gameManager, 2, 0, false, false, false)
				}, false);
				return;
			case 16:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayResource(gameManager, false, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainPower(gameManager, 1, 0, false, false, false),
					new GainPopularity(gameManager, 1, 0, false, false, false),
					new GainCoin(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 17:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayCombatCard(gameManager, 1, 0, false, false)
				}, new GainAction[]
				{
					new GainUpgrade(gameManager, 1, 0, false),
					new GainPower(gameManager, 1, 0, false, false, false)
				}, false);
				return;
			case 18:
				base.ActionTop = new TopAction(gameManager, new PayAction[]
				{
					new PayResource(gameManager, true, 2, 0, false, false)
				}, new GainAction[]
				{
					new GainRecruit(gameManager, 1, 0, false),
					new GainUpgrade(gameManager, 1, 0, false)
				}, true);
				return;
			default:
				return;
			}
		}
	}
}
