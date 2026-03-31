using System;

namespace Scythe.GameLogic
{
	// Token: 0x020005AD RID: 1453
	public class AiRecipe
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x000448B4 File Offset: 0x00042AB4
		public AiRecipe(AiAction action, string description)
		{
			this.action = action;
			this.description = description;
		}

		// Token: 0x04001F13 RID: 7955
		public AiAction action;

		// Token: 0x04001F14 RID: 7956
		public string description;

		// Token: 0x04001F15 RID: 7957
		public AiAction.ActionExecute moveAction;

		// Token: 0x04001F16 RID: 7958
		public ResourceType[] tradeResource;
	}
}
