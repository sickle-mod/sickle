using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AA RID: 682
	public class GainBuildingMessage : Message, IExecutableMessage
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x00036A0B File Offset: 0x00034C0B
		public GainBuildingMessage(GameHex hex, int sectionId, bool encounter)
		{
			this.x = hex.posX;
			this.y = hex.posY;
			this.sectionId = sectionId;
			this.encounter = encounter;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0009DDC8 File Offset: 0x0009BFC8
		public void Execute(GameManager gameManager)
		{
			GainBuilding gainBuilding = gameManager.actionManager.GetLastSelectedGainAction() as GainBuilding;
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			Building structure = gameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.sectionId).ActionTop.Structure;
			gainBuilding.SetStructureAndLocation(structure, gameHex);
			gameManager.actionManager.PrepareNextAction();
			gameManager.OnActionFinished();
		}

		// Token: 0x04000FC6 RID: 4038
		private int x;

		// Token: 0x04000FC7 RID: 4039
		private int y;

		// Token: 0x04000FC8 RID: 4040
		private int sectionId;

		// Token: 0x04000FC9 RID: 4041
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}
