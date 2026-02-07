using System;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000494 RID: 1172
	public class FactionSummaryEndGame : MonoBehaviour
	{
		// Token: 0x06002533 RID: 9523 RVA: 0x000DE068 File Offset: 0x000DC268
		public void SetupPlayerSummaryInfo(int placeOnPodium, Faction faction, int score, string nicknameText, Sprite placeLaurel, Sprite factionBackground)
		{
			switch (placeOnPodium)
			{
			case 1:
				this.laurelImage.sprite = placeLaurel;
				break;
			case 2:
				this.laurelImage.sprite = placeLaurel;
				break;
			case 3:
				this.laurelImage.sprite = placeLaurel;
				break;
			default:
				this.laurelImage.sprite = placeLaurel;
				break;
			}
			if (placeOnPodium == 0)
			{
				this.winnerBorder.SetActive(true);
			}
			else
			{
				this.winnerBorder.SetActive(false);
			}
			switch (faction)
			{
			default:
				this.factionBackgroundImage.sprite = factionBackground;
				this.nickname.text = nicknameText;
				this.scoreValue.text = score.ToString();
				return;
			}
		}

		// Token: 0x04001A1C RID: 6684
		[SerializeField]
		private GameObject winnerBorder;

		// Token: 0x04001A1D RID: 6685
		[SerializeField]
		private Image laurelImage;

		// Token: 0x04001A1E RID: 6686
		[SerializeField]
		private Image factionBackgroundImage;

		// Token: 0x04001A1F RID: 6687
		[SerializeField]
		private TextMeshProUGUI scoreValue;

		// Token: 0x04001A20 RID: 6688
		[SerializeField]
		private TextMeshProUGUI nickname;
	}
}
