using System;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004A4 RID: 1188
	public class TopMenuScoreLegend : MonoBehaviour
	{
		// Token: 0x060025C2 RID: 9666 RVA: 0x000E09CC File Offset: 0x000DEBCC
		public void UpdateLegend()
		{
			bool gameFinished = GameController.GameManager.GameFinished;
			bool isMultiplayer = GameController.GameManager.IsMultiplayer;
			bool isRanked = GameController.GameManager.IsRanked;
			bool flag = GameController.GameManager.GetPlayerByFaction(Faction.Polania) != null;
			bool flag2 = GameController.GameManager.PlayerCount > 5;
			if (gameFinished)
			{
				this.laurelCell.SetActive(true);
			}
			else
			{
				this.laurelCell.SetActive(false);
			}
			if (flag && flag2)
			{
				this.polaniaBonusCell.SetActive(true);
			}
			else
			{
				this.polaniaBonusCell.SetActive(false);
			}
			if (isMultiplayer && isRanked && gameFinished)
			{
				this.eloCell.SetActive(true);
				return;
			}
			this.eloCell.SetActive(false);
		}

		// Token: 0x04001AA3 RID: 6819
		[Header("Cells References")]
		[SerializeField]
		private GameObject laurelCell;

		// Token: 0x04001AA4 RID: 6820
		[SerializeField]
		private GameObject polaniaBonusCell;

		// Token: 0x04001AA5 RID: 6821
		[SerializeField]
		private GameObject eloCell;
	}
}
