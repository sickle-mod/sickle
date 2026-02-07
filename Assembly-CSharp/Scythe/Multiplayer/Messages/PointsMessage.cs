using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002EF RID: 751
	public class PointsMessage : Message, IExecutableMessage
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x0009F75C File Offset: 0x0009D95C
		public void Execute(GameManager gameManager)
		{
			gameManager.CheckStars();
			List<PlayerEndGameStats> list = new List<PlayerEndGameStats>();
			foreach (string text in this.playerStats)
			{
				PlayerEndGameStats playerEndGameStats = PlayerEndGameStats.Deserialize(text, gameManager);
				list.Add(playerEndGameStats);
			}
			gameManager.OnShowPoints(list);
		}

		// Token: 0x0400106A RID: 4202
		private List<string> playerStats;
	}
}
