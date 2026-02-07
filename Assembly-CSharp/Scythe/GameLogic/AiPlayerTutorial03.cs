using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x0200059F RID: 1439
	public class AiPlayerTutorial03 : AiPlayer
	{
		// Token: 0x06002DAB RID: 11691 RVA: 0x000447EF File Offset: 0x000429EF
		public AiPlayerTutorial03(Player player, GameManager gameManager)
			: base(player, gameManager)
		{
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x0010CC34 File Offset: 0x0010AE34
		public override AiRecipe Bot()
		{
			SortedList<int, AiRecipe> sortedList = new SortedList<int, AiRecipe>(new InvertedComparer());
			base.StrategicAnalysisRun();
			this.TradeForResource(sortedList, 120);
			this.ActionCoin(sortedList, 110);
			return sortedList.Values[0];
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000447F9 File Offset: 0x000429F9
		private void TradeForResource(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (base.CanPlayTopAction(GainType.AnyResource) && this.strategicAnalysis.resourceHighestPriority != ResourceType.combatCard)
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.AnyResource], "Trade for resource"));
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x0004482F File Offset: 0x00042A2F
		private void ActionCoin(SortedList<int, AiRecipe> actionOptions, int priority)
		{
			if (base.CanPlayTopAction(GainType.Coin))
			{
				actionOptions.Add(priority, new AiRecipe(this.AiTopActions[GainType.Coin], "Get Coin"));
			}
		}
	}
}
