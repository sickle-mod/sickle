using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004D3 RID: 1235
	public class AlbumMobileTab : MonoBehaviour
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x00041249 File Offset: 0x0003F449
		public int CardCount
		{
			get
			{
				return this.cards.Length;
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000E80C4 File Offset: 0x000E62C4
		public void OpenTab()
		{
			this.tabContent.SetActive(true);
			this.itemIndexText.text = string.Format("{0}/{1}", 1, this.CardCount);
			this.cardImage.sprite = this.cards[0];
			this.UpdateHint(0);
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000E8120 File Offset: 0x000E6320
		public void ChooseNext(int nextIndex)
		{
			this.itemIndexText.text = string.Format("{0}/{1}", nextIndex + 1, this.CardCount);
			this.UpdateHint(nextIndex + 1);
			this.cardImage.sprite = (this.IsCardUnlocked(nextIndex + 1) ? this.cards[nextIndex] : this.lockedCard);
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000E8120 File Offset: 0x000E6320
		public void ChoosePrevious(int previousIndex)
		{
			this.itemIndexText.text = string.Format("{0}/{1}", previousIndex + 1, this.CardCount);
			this.UpdateHint(previousIndex + 1);
			this.cardImage.sprite = (this.IsCardUnlocked(previousIndex + 1) ? this.cards[previousIndex] : this.lockedCard);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000E8184 File Offset: 0x000E6384
		private bool IsCardUnlocked(int index)
		{
			switch (this.tabType)
			{
			case AlbumMobileTab.AlbumTabType.ENCOUNTERS:
				return AchievementManager.EncounterCardUnlocked(index);
			case AlbumMobileTab.AlbumTabType.FACTORY:
				return AchievementManager.FactoryCardUnlocked(index);
			case AlbumMobileTab.AlbumTabType.OBJECTIVES:
				return AchievementManager.ObjectiveCardUnlocked(index);
			default:
				return false;
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000E81C4 File Offset: 0x000E63C4
		private string GetCardHint(int index)
		{
			switch (this.tabType)
			{
			case AlbumMobileTab.AlbumTabType.ENCOUNTERS:
				return AchievementManager.GetAchievementDescriptionForEncounterCard(index);
			case AlbumMobileTab.AlbumTabType.FACTORY:
				return AchievementManager.GetAchievementDescriptionForFactoryCard(index);
			case AlbumMobileTab.AlbumTabType.OBJECTIVES:
				return AchievementManager.GetAchievementDescriptionForObjectiveCard(index);
			default:
				return string.Empty;
			}
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000E8208 File Offset: 0x000E6408
		private void UpdateHint(int cardIndex)
		{
			bool flag = this.IsCardUnlocked(cardIndex);
			string cardHint = this.GetCardHint(cardIndex);
			this.UpdateHint(flag, cardHint);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x00041253 File Offset: 0x0003F453
		private void UpdateHint(bool cardUnlocked, string hint)
		{
			this.hintText.gameObject.SetActive(!cardUnlocked);
			this.hintText.text = hint;
		}

		// Token: 0x04001C2F RID: 7215
		[SerializeField]
		private GameObject tabContent;

		// Token: 0x04001C30 RID: 7216
		[SerializeField]
		private Image cardImage;

		// Token: 0x04001C31 RID: 7217
		[SerializeField]
		private Sprite[] cards;

		// Token: 0x04001C32 RID: 7218
		[SerializeField]
		private Sprite lockedCard;

		// Token: 0x04001C33 RID: 7219
		[SerializeField]
		private AlbumMobileTab.AlbumTabType tabType;

		// Token: 0x04001C34 RID: 7220
		[SerializeField]
		private TMP_Text itemIndexText;

		// Token: 0x04001C35 RID: 7221
		[SerializeField]
		private TMP_Text hintText;

		// Token: 0x020004D4 RID: 1236
		private enum AlbumTabType
		{
			// Token: 0x04001C37 RID: 7223
			ENCOUNTERS,
			// Token: 0x04001C38 RID: 7224
			FACTORY,
			// Token: 0x04001C39 RID: 7225
			OBJECTIVES
		}
	}
}
