using System;
using System.Collections.Generic;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200061D RID: 1565
	public class GainPeekCombatCards : GainAction
	{
		// Token: 0x06003169 RID: 12649 RVA: 0x0004709F File Offset: 0x0004529F
		public GainPeekCombatCards(short amount = 0, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.PeekCombatCards;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000470B4 File Offset: 0x000452B4
		public GainPeekCombatCards(GameManager gameManager, short amount = 0, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.PeekCombatCards;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x000470CB File Offset: 0x000452CB
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x000470D3 File Offset: 0x000452D3
		public Faction PeekFaction { get; private set; }

		// Token: 0x0600316D RID: 12653 RVA: 0x000470DC File Offset: 0x000452DC
		public void SetStartingPeekFaction()
		{
			this.PeekFaction = this.player.matFaction.faction;
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x000470F4 File Offset: 0x000452F4
		public bool SetFaction(Faction faction)
		{
			this.PeekFaction = faction;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x00047105 File Offset: 0x00045305
		public List<CombatCard> PeekCombatCards()
		{
			return this.gameManager.GetPlayerByFaction(this.PeekFaction).combatCards;
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool CanExecute()
		{
			return true;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x0012BC8C File Offset: 0x00129E8C
		public override LogInfo GetLogInfo()
		{
			return new SneakPeakLogInfo(this.gameManager)
			{
				Type = LogInfoType.SpyPlayer,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				SpiedFaction = this.PeekFaction
			};
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x0012BCDC File Offset: 0x00129EDC
		public override void Execute()
		{
			base.Gained = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainPeekCombatCardsMessage(this.PeekFaction));
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0004711D File Offset: 0x0004531D
		public override void Clear()
		{
			base.Gained = false;
			base.ActionSelected = false;
		}
	}
}
