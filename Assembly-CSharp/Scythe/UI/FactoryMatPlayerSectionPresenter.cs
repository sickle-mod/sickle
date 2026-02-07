using System;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200047C RID: 1148
	public class FactoryMatPlayerSectionPresenter : MatPlayerSectionPresenter
	{
		// Token: 0x06002460 RID: 9312 RVA: 0x000D7534 File Offset: 0x000D5734
		private void Start()
		{
			if (this.keyInfoFactoryCard != null)
			{
				this.keyInfoFactoryCard.GetComponentInChildren<TextMeshProUGUI>().text = (this.sectionID + 1).ToString();
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x0003F30F File Offset: 0x0003D50F
		// (set) Token: 0x06002462 RID: 9314 RVA: 0x0003F317 File Offset: 0x0003D517
		public bool ShowingChoosableCard { get; private set; }

		// Token: 0x06002463 RID: 9315 RVA: 0x0003F320 File Offset: 0x0003D520
		public void ShowChoosableCardPreview(FactoryCard factoryCard)
		{
			this.ShowingChoosableCard = true;
			this.pickFactoryCardButton.gameObject.SetActive(true);
			this.pickFactoryCardButton.enabled = true;
			this.ShowCardPreview(factoryCard);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000D7570 File Offset: 0x000D5770
		public void ShowCardPreview(FactoryCard factoryCard)
		{
			AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_factory");
			this.cardPreviewParent.SetActive(true);
			this.cardPreviewImage.sprite = assetBundle.LoadAsset<Sprite>("factory_" + factoryCard.CardId.ToString().PadLeft(2, '0'));
			this.factoryCardNumber.text = factoryCard.CardId.ToString();
			this.cardPreviewDescription.text = PlayerActionPresenter.CreateFactoryCardDescription(factoryCard.ActionTop);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0003F34D File Offset: 0x0003D54D
		public void ChooseFactoryCard()
		{
			GameController.Instance.factoryCardPresenter.ChooseCard(this.sectionID);
			WorldSFXManager.PlaySound(SoundEnum.FactoryCardClick, AudioSourceType.WorldSfx);
		}

		// Token: 0x04001962 RID: 6498
		[Header("Factory Mat section")]
		[SerializeField]
		private Button pickFactoryCardButton;

		// Token: 0x04001963 RID: 6499
		[SerializeField]
		private GameObject cardPreviewParent;

		// Token: 0x04001964 RID: 6500
		[SerializeField]
		private Image cardPreviewImage;

		// Token: 0x04001965 RID: 6501
		[SerializeField]
		private TextMeshProUGUI cardPreviewDescription;

		// Token: 0x04001966 RID: 6502
		[SerializeField]
		public GameObject keyInfoFactoryCard;

		// Token: 0x04001967 RID: 6503
		[SerializeField]
		private TextMeshProUGUI factoryCardNumber;
	}
}
