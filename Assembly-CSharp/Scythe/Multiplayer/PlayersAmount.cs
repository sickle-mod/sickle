using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000222 RID: 546
	public class PlayersAmount : MonoBehaviour
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x000327D9 File Offset: 0x000309D9
		public void Init(bool ifa)
		{
			this.SetImagesAmount(ifa);
			if (int.Parse(this.text.text) > this.maxActiveAmount)
			{
				this.text.text = this.maxActiveAmount.ToString();
			}
			this.ChangeImagesOpacity();
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0008F778 File Offset: 0x0008D978
		private void SetImagesAmount(bool ifa)
		{
			this.maxActiveAmount = (ifa ? 7 : 5);
			for (int i = 5; i < 7; i++)
			{
				this.images[i].gameObject.SetActive(ifa);
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00032816 File Offset: 0x00030A16
		public void SetPlayersAmount(int amount)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.text.text = amount.ToString();
			this.ChangeImagesOpacity();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0003283C File Offset: 0x00030A3C
		public void SetDefaultPlayersAmount(bool ifa)
		{
			this.SetPlayersAmount(ifa ? 7 : 5);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0008F7B4 File Offset: 0x0008D9B4
		private void ChangeImagesOpacity()
		{
			int num = int.Parse(this.text.text);
			for (int i = 0; i < num; i++)
			{
				this.SetOpactity(this.images[i], 1f);
			}
			for (int j = num; j < this.maxActiveAmount; j++)
			{
				this.SetOpactity(this.images[j], 0.2f);
			}
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0008F818 File Offset: 0x0008DA18
		private void SetOpactity(Image image, float opacity)
		{
			Color color = image.color;
			color.a = opacity;
			image.color = color;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0003284B File Offset: 0x00030A4B
		public int GetPlayersAmount()
		{
			return int.Parse(this.text.text);
		}

		// Token: 0x04000C7D RID: 3197
		[SerializeField]
		private Image[] images;

		// Token: 0x04000C7E RID: 3198
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000C7F RID: 3199
		private const int ActiveImagesDefault = 5;

		// Token: 0x04000C80 RID: 3200
		private const int ActiveImagesIFA = 7;

		// Token: 0x04000C81 RID: 3201
		private int maxActiveAmount;

		// Token: 0x04000C82 RID: 3202
		private const float ActiveOpactity = 1f;

		// Token: 0x04000C83 RID: 3203
		private const float DeactiveOpacity = 0.2f;
	}
}
