using System;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C5 RID: 709
	public class ChooseSectionMessage : Message, IExecutableMessage
	{
		// Token: 0x060015DF RID: 5599 RVA: 0x00036EAB File Offset: 0x000350AB
		public ChooseSectionMessage(int faction, int section)
		{
			this.faction = faction;
			this.section = section;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00036EC1 File Offset: 0x000350C1
		public void Execute(GameManager gameManager)
		{
			gameManager.ChooseSection(this.section);
		}

		// Token: 0x0400101B RID: 4123
		private int faction;

		// Token: 0x0400101C RID: 4124
		private int section;
	}
}
