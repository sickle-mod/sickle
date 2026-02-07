using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E0 RID: 736
	public class CombatCardsMessage : Message, IExecutableMessage
	{
		// Token: 0x06001612 RID: 5650 RVA: 0x0009F2F0 File Offset: 0x0009D4F0
		public void Execute(GameManager gameManager)
		{
			Faction faction = (Faction)this.faction;
			GainCombatCard.CombatCardGainType combatCardGainType = (GainCombatCard.CombatCardGainType)this.type;
			for (int i = 0; i < this.cardsIds.Count; i++)
			{
				gameManager.GetPlayerByFaction(faction).AddCombatCard(new CombatCard(int.Parse(this.cardsIds[i])));
			}
			gameManager.InformAboutCombatCardsAmount(this.cardsLeft);
			if (faction != gameManager.PlayerOwner.matFaction.faction && combatCardGainType == GainCombatCard.CombatCardGainType.Combat)
			{
				GainNonboardResourceLogInfo gainNonboardResourceLogInfo = new GainNonboardResourceLogInfo(gameManager);
				gainNonboardResourceLogInfo.Type = LogInfoType.GainCombatCard;
				gainNonboardResourceLogInfo.IsEncounter = gameManager.EncounterIsBeignResolved();
				gainNonboardResourceLogInfo.PlayerAssigned = faction;
				gainNonboardResourceLogInfo.Amount = this.cardsIds.Count;
				gainNonboardResourceLogInfo.Gained = GainType.CombatCard;
				switch (this.type)
				{
				case 0:
					gainNonboardResourceLogInfo.ActionPlacement = ActionPositionType.Other;
					break;
				case 2:
					gainNonboardResourceLogInfo.ActionPlacement = ActionPositionType.OngoingRecruitBonus;
					break;
				case 3:
					gainNonboardResourceLogInfo.ActionPlacement = ActionPositionType.Combat;
					break;
				}
				gameManager.ReportLog(gainNonboardResourceLogInfo, this.type == 0, ActionPositionType.Other);
			}
			GainCombatCard gainCombatCard = gameManager.actionManager.GetLastSelectedGainAction() as GainCombatCard;
			if (gainCombatCard != null)
			{
				gainCombatCard.SetCards(gainCombatCard.Amount);
				gameManager.actionManager.PrepareNextAction();
			}
			gameManager.OnActionFinished();
			MultiplayerController.Instance.CombatCardsReceived();
		}

		// Token: 0x0400104D RID: 4173
		private List<string> cardsIds;

		// Token: 0x0400104E RID: 4174
		private int cardsLeft;

		// Token: 0x0400104F RID: 4175
		private int faction;

		// Token: 0x04001050 RID: 4176
		private int type;
	}
}
