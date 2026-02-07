using System;
using I2.Loc;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000495 RID: 1173
	public class FlagsPanelMobile : SingletonMono<FlagsPanelMobile>
	{
		// Token: 0x06002535 RID: 9525 RVA: 0x0003F997 File Offset: 0x0003DB97
		public void WindowPreview(bool enablePreview)
		{
			this.isPreview = enablePreview;
			if (enablePreview)
			{
				this.flagsText.text = ScriptLocalization.Get("GameScene/Flags");
				return;
			}
			this.flagsText.text = ScriptLocalization.Get("GameScene/PlaceFlag");
			this.TurnOnButtons();
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000DE130 File Offset: 0x000DC330
		public void UpdateFlagsPanels()
		{
			int unplacedTokensCount = GameController.GameManager.PlayerCurrent.matFaction.FactionTokens.GetUnplacedTokensCount();
			for (int i = 0; i < 4; i++)
			{
				if (i < unplacedTokensCount)
				{
					this.placeFlagButtons[i].interactable = true;
				}
				else
				{
					this.placeFlagButtons[i].interactable = false;
					this.placeFlagButtonsImages[i].color = new Color(this.placeFlagButtonsImages[i].color.r, this.placeFlagButtonsImages[i].color.g, this.placeFlagButtonsImages[i].color.b, 0.3f);
				}
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000DE1D4 File Offset: 0x000DC3D4
		public void PreHideTokenPanel()
		{
			for (int i = 0; i < this.placeFlagButtons.Length - 1; i++)
			{
				this.placeFlagButtons[i].interactable = false;
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000DE204 File Offset: 0x000DC404
		public void TurnOffButtons()
		{
			int unplacedTokensCount = GameController.GameManager.PlayerMaster.matFaction.FactionTokens.GetUnplacedTokensCount();
			for (int i = 0; i < 4; i++)
			{
				this.placeFlagButtons[i].interactable = false;
				if (i >= unplacedTokensCount)
				{
					this.placeFlagButtonsImages[i].color = new Color(this.placeFlagButtonsImages[i].color.r, this.placeFlagButtonsImages[i].color.g, this.placeFlagButtonsImages[i].color.b, 0.3f);
				}
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000DE298 File Offset: 0x000DC498
		public void TurnOnButtons()
		{
			for (int i = 0; i < 4; i++)
			{
				this.placeFlagButtons[i].interactable = true;
			}
		}

		// Token: 0x04001A21 RID: 6689
		[SerializeField]
		private Button[] placeFlagButtons;

		// Token: 0x04001A22 RID: 6690
		[SerializeField]
		private Image[] placeFlagButtonsImages;

		// Token: 0x04001A23 RID: 6691
		[SerializeField]
		private TextMeshProUGUI flagsText;

		// Token: 0x04001A24 RID: 6692
		public bool isPreview;
	}
}
