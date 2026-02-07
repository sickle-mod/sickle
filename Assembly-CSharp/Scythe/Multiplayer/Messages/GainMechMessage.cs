using System;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002AC RID: 684
	public class GainMechMessage : Message, IExecutableMessage
	{
		// Token: 0x06001598 RID: 5528 RVA: 0x00036A78 File Offset: 0x00034C78
		public GainMechMessage(GameHex hex, int skillIndex, bool encounter)
		{
			this.x = hex.posX;
			this.y = hex.posY;
			this.skillIndex = skillIndex;
			this.encounter = encounter;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0009DE3C File Offset: 0x0009C03C
		public void Execute(GameManager gameManager)
		{
			GainMech gainMech = gameManager.actionManager.GetLastSelectedGainAction() as GainMech;
			Mech mech = new Mech(gameManager, gameManager.PlayerCurrent, 1);
			GameHex gameHex = gameManager.gameBoard.hexMap[this.x, this.y];
			gainMech.SetMechAndLocation(mech, gameHex, this.skillIndex);
			gameManager.actionManager.PrepareNextAction();
			gameManager.OnActionFinished();
		}

		// Token: 0x04000FCC RID: 4044
		private int x;

		// Token: 0x04000FCD RID: 4045
		private int y;

		// Token: 0x04000FCE RID: 4046
		private int skillIndex;

		// Token: 0x04000FCF RID: 4047
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}
