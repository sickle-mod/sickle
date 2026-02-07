using System;

namespace Scythe.GameLogic
{
	// Token: 0x0200055C RID: 1372
	public class ChallengesLogicStarter
	{
		// Token: 0x06002BF6 RID: 11254 RVA: 0x000F42A4 File Offset: 0x000F24A4
		public void InitChallenge(GameManager gameManager, int challengeId, int campaignId = 0)
		{
			gameManager.IsChallenge = true;
			gameManager.IsMultiplayer = false;
			gameManager.challengeId = challengeId;
			gameManager.TurnCount = 0;
			gameManager.UndoType = 2;
			this.currenChallengeId = challengeId;
			switch (gameManager.challengeId)
			{
			case 0:
				new ChallengeLogic1().InitChallenge1(gameManager);
				return;
			case 1:
				new ChallengeLogic2().InitChallenge2(gameManager);
				return;
			case 2:
				new ChallengeLogic3().InitChallenge3(gameManager);
				return;
			case 3:
				new ChallengeLogic4().InitChallenge4(gameManager);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00044028 File Offset: 0x00042228
		public int GetCurrentChallengeId()
		{
			return this.currenChallengeId;
		}

		// Token: 0x04001E43 RID: 7747
		private int currenChallengeId = -1;

		// Token: 0x0200055D RID: 1373
		public enum UndoTypes
		{
			// Token: 0x04001E45 RID: 7749
			SingleTurn,
			// Token: 0x04001E46 RID: 7750
			Unlimited,
			// Token: 0x04001E47 RID: 7751
			Off
		}
	}
}
