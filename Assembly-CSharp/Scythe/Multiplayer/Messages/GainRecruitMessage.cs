using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B2 RID: 690
	public class GainRecruitMessage : Message, IExecutableMessage
	{
		// Token: 0x060015A5 RID: 5541 RVA: 0x00036BBC File Offset: 0x00034DBC
		public GainRecruitMessage(int bonusType, int downType, bool encounter)
		{
			this.bonusType = bonusType;
			this.downType = downType;
			this.encounter = encounter;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0009DF08 File Offset: 0x0009C108
		public void Execute(GameManager gameManager)
		{
			GainRecruit gainRecruit = gameManager.actionManager.GetLastSelectedGainAction() as GainRecruit;
			GainAction oneTimeBonus = gainRecruit.GetPlayer().matFaction.GetOneTimeBonus((GainType)this.bonusType);
			switch (this.bonusType)
			{
			case 0:
				(oneTimeBonus as GainCoin).SetCoins(2);
				break;
			case 1:
				(oneTimeBonus as GainPopularity).SetPopularity(2);
				break;
			case 2:
				(oneTimeBonus as GainPower).SetPower(2);
				break;
			case 3:
				(oneTimeBonus as GainCombatCard).SetCards(2);
				break;
			}
			gainRecruit.SetSectionAndBonus((DownActionType)this.downType, oneTimeBonus);
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FDC RID: 4060
		private int bonusType;

		// Token: 0x04000FDD RID: 4061
		private int downType;

		// Token: 0x04000FDE RID: 4062
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}
