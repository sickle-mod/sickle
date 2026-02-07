using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005A0 RID: 1440
	public class AiRecipe
	{
		// Token: 0x06002DAF RID: 11695 RVA: 0x00044857 File Offset: 0x00042A57
		public AiRecipe(AiAction action, string description)
		{
			this.action = action;
			this.description = description;
		}

		// Token: 0x04001EF4 RID: 7924
		public AiAction action;

		// Token: 0x04001EF5 RID: 7925
		public string description;

		// Token: 0x04001EF6 RID: 7926
		public AiAction.ActionExecute moveAction;

		// Token: 0x04001EF7 RID: 7927
		public ResourceType[] tradeResource;
	}
}
