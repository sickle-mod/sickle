using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000623 RID: 1571
	public class GainUpgrade : GainAction
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x00047564 File Offset: 0x00045764
		// (set) Token: 0x060031C9 RID: 12745 RVA: 0x0004756C File Offset: 0x0004576C
		public GainAction GainToUpgrade { get; private set; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x00047575 File Offset: 0x00045775
		// (set) Token: 0x060031CB RID: 12747 RVA: 0x0004757D File Offset: 0x0004577D
		public PayAction PayToUpgrade { get; private set; }

		// Token: 0x060031CC RID: 12748 RVA: 0x00047586 File Offset: 0x00045786
		public GainUpgrade()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Upgrade;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0004759A File Offset: 0x0004579A
		public GainUpgrade(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Upgrade;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000475AF File Offset: 0x000457AF
		public GainUpgrade(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Upgrade;
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000475C5 File Offset: 0x000457C5
		public bool SetPayAndGainActions(GainAction gainAction, PayAction payAction)
		{
			if (!this.CheckLogic(gainAction, payAction))
			{
				return false;
			}
			this.GainToUpgrade = gainAction;
			this.PayToUpgrade = payAction;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000475E9 File Offset: 0x000457E9
		public override bool CanExecute()
		{
			return this.CheckLogic(this.GainToUpgrade, this.PayToUpgrade);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x00047602 File Offset: 0x00045802
		private bool CheckLogic(GainAction gainAction, PayAction payAction)
		{
			return this.GainAvaliable() && gainAction != null && payAction != null && gainAction.CanUpgrade() && payAction.CanUpgrade();
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00047629 File Offset: 0x00045829
		public override bool GainAvaliable()
		{
			return this.player.matPlayer.UpgradesDone != 6;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00047641 File Offset: 0x00045841
		public override bool IsMaxReached()
		{
			return this.player.matPlayer.UpgradesDone == 6;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x0012D00C File Offset: 0x0012B20C
		public override LogInfo GetLogInfo()
		{
			UpgradeLogInfo upgradeLogInfo = new UpgradeLogInfo(this.gameManager);
			upgradeLogInfo.Type = LogInfoType.Upgrade;
			upgradeLogInfo.IsEncounter = base.IsEncounter;
			upgradeLogInfo.PlayerAssigned = this.player.matFaction.faction;
			if (this.GainToUpgrade != null && this.PayToUpgrade != null)
			{
				upgradeLogInfo.TopAction = this.GainToUpgrade.GetGainType();
				upgradeLogInfo.DownAction = this.PayToUpgrade.GetPayType();
				if (this.PayToUpgrade.GetPayType() == PayType.Resource)
				{
					upgradeLogInfo.Resource = ((PayResource)this.PayToUpgrade).ResourceToPay;
				}
			}
			return upgradeLogInfo;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x0012D0A8 File Offset: 0x0012B2A8
		public override void Execute()
		{
			base.Gained = true;
			this.GainToUpgrade.Upgrade();
			this.PayToUpgrade.Upgrade();
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.SendAction();
			}
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				UpgradeEnemyActionInfo upgradeEnemyActionInfo = new UpgradeEnemyActionInfo();
				upgradeEnemyActionInfo.fromEncounter = base.IsEncounter;
				upgradeEnemyActionInfo.TopAction = this.GainToUpgrade.GetGainType();
				upgradeEnemyActionInfo.DownAction = this.PayToUpgrade.GetPayType();
				upgradeEnemyActionInfo.DownResource = ((PayResource)this.PayToUpgrade).ResourceToPay;
				upgradeEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				upgradeEnemyActionInfo.actionType = LogInfoType.Upgrade;
				this.gameManager.EnemyUpgradeMat(upgradeEnemyActionInfo);
			}
			this.player.matPlayer.IncreaseUpgradeCounter();
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x00047656 File Offset: 0x00045856
		public override void Clear()
		{
			base.Gained = false;
			this.GainToUpgrade = null;
			this.PayToUpgrade = null;
			base.ActionSelected = false;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x0012D1EC File Offset: 0x0012B3EC
		private void SendAction()
		{
			if (this.GainToUpgrade == null)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSection playerMatSection = this.gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i);
				if (playerMatSection.ActionTop.GetGainAction(0) == this.GainToUpgrade)
				{
					num = i;
					num3 = 0;
				}
				else if (playerMatSection.ActionTop.GetNumberOfGainActions() > 1 && playerMatSection.ActionTop.GetGainAction(1) == this.GainToUpgrade)
				{
					num = i;
					num3 = 1;
				}
				if (playerMatSection.ActionDown.GetPayAction(0) == this.PayToUpgrade)
				{
					num2 = i;
				}
			}
			this.gameManager.OnActionSent(new GainUpgradeMessage(num, num2, num3, base.IsEncounter));
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0012D2A0 File Offset: 0x0012B4A0
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("TopId") == null)
			{
				return;
			}
			int num = int.Parse(reader.GetAttribute("TopId"));
			int num2 = int.Parse(reader.GetAttribute("DownId"));
			int num3 = int.Parse(reader.GetAttribute("Index"));
			this.GainToUpgrade = this.player.matPlayer.GetPlayerMatSection(num).ActionTop.GetGainAction(num3);
			this.PayToUpgrade = this.player.matPlayer.GetPlayerMatSection(num2).ActionDown.GetPayAction(0);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x0012D33C File Offset: 0x0012B53C
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.GainToUpgrade == null)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSection playerMatSection = this.gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i);
				if (playerMatSection.ActionTop.GetGainAction(0) == this.GainToUpgrade)
				{
					num = i;
					num3 = 0;
				}
				else if (playerMatSection.ActionTop.GetNumberOfGainActions() > 1 && playerMatSection.ActionTop.GetGainAction(1) == this.GainToUpgrade)
				{
					num = i;
					num3 = 1;
				}
				if (playerMatSection.ActionDown.GetPayAction(0) == this.PayToUpgrade)
				{
					num2 = i;
				}
			}
			writer.WriteAttributeString("TopId", num.ToString());
			writer.WriteAttributeString("DownId", num2.ToString());
			writer.WriteAttributeString("Index", num3.ToString());
		}
	}
}
