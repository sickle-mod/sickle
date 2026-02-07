using System;
using Scythe.GameLogic;

namespace Scythe.UI
{
	// Token: 0x020004BD RID: 1213
	public class ChangeFactionNode
	{
		// Token: 0x06002696 RID: 9878 RVA: 0x00040A48 File Offset: 0x0003EC48
		public ChangeFactionNode(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x00040A57 File Offset: 0x0003EC57
		public void SwitchFaction()
		{
			if (GameController.GameManager.PlayerOwner.matFaction.faction == this.faction)
			{
				return;
			}
			GameController.GameManager.SetOwnerIdFromFaction(this.faction);
		}

		// Token: 0x04001B94 RID: 7060
		private Faction faction;
	}
}
