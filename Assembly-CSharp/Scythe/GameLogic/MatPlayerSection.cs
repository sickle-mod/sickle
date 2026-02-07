using System;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005DF RID: 1503
	public class MatPlayerSection : Card
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x00045AEB File Offset: 0x00043CEB
		// (set) Token: 0x06002FA7 RID: 12199 RVA: 0x00045AF3 File Offset: 0x00043CF3
		public TopAction ActionTop { get; protected set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x00045AFC File Offset: 0x00043CFC
		// (set) Token: 0x06002FA9 RID: 12201 RVA: 0x00045B04 File Offset: 0x00043D04
		public DownAction ActionDown { get; protected set; }

		// Token: 0x06002FAA RID: 12202 RVA: 0x00044A18 File Offset: 0x00042C18
		public MatPlayerSection()
		{
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x00045B0D File Offset: 0x00043D0D
		public MatPlayerSection(TopAction topAction, DownAction downAction)
		{
			this.ActionTop = topAction;
			this.ActionDown = downAction;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x00121E24 File Offset: 0x00120024
		public MatPlayerSection(PlayerMatType type, int sectionId, GameManager gameManager)
		{
			switch (type)
			{
			case PlayerMatType.Industrial:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 3);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 1, 1);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 0);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Engineering:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 3);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 1);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Patriotic:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 1);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 3, 3);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Mechanical:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 1, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Agricultural:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 1);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Militant:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 2, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 3);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 1, 1);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Innovative:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 0, 3);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 1);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 3, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 0);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Campaign00:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 1, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Campaign01:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionTop.GetGainAction(1).Upgrade();
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 1, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionTop.GetGainAction(0).Upgrade();
					this.ActionTop.GetGainAction(1).Upgrade();
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 1, 0, 3);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 1, 0, 3);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionTop.GetGainAction(0).Upgrade();
					this.ActionTop.GetGainAction(1).Upgrade();
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 1, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial01:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 1, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial02Crimea:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 4, 0, 1);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 1);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 2, 0, 3);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 0, 1);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial02Saxony:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 4, 0, 1);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 1);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 0, 1);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 0, 1);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial02Polania:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 4, 0, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionTop.AddPayAction(new PayPopularity(gameManager, 1, 0, false, false));
					this.ActionTop.AddPayAction(new PayPower(gameManager, 1, 0, false));
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 0, 0);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial03:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 1, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial04:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 1, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial01Crimea:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial01Saxony:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 0, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 0, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial01StarsA:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 3);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial01StarsB:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 3);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 1, 1);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial02Stars:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial03Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 2, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial03Enemy:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 2, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial04Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial05Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionTop.AddPayAction(new PayPower(gameManager, 1, 0, false));
					this.ActionTop.AddPayAction(new PayPopularity(gameManager, 1, 0, false, false));
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial06Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial07Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial08Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 1, 3);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 0);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial09Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial09AINordic:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial09AISaxony:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 1, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial10Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 2, 2);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 1, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial11Player:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 1, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Tutorial11Enemy:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 2, 0, 1);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 4, 2, 0);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 4, 2, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 3, 2, 3);
					return;
				default:
					return;
				}
				break;
			case PlayerMatType.Challenge1:
				switch (sectionId)
				{
				case 0:
					this.ActionTop = new TopAction(gameManager, TopActionType.Trade);
					this.ActionDown = new DownAction(gameManager, DownActionType.Upgrade, 3, 1, 0);
					return;
				case 1:
					this.ActionTop = new TopAction(gameManager, TopActionType.Bolster);
					this.ActionDown = new DownAction(gameManager, DownActionType.Deploy, 3, 2, 2);
					return;
				case 2:
					this.ActionTop = new TopAction(gameManager, TopActionType.MoveGain);
					this.ActionDown = new DownAction(gameManager, DownActionType.Build, 3, 1, 2);
					return;
				case 3:
					this.ActionTop = new TopAction(gameManager, TopActionType.Produce);
					this.ActionDown = new DownAction(gameManager, DownActionType.Enlist, 4, 2, 2);
					return;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x00045B23 File Offset: 0x00043D23
		public void SetPlayer(Player player)
		{
			this.ActionTop.SetPlayer(player);
			this.ActionDown.SetPlayer(player);
		}
	}
}
