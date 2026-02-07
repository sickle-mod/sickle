using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000449 RID: 1097
	public class PlaceFlagPanel : TokenPanelPresenter
	{
		// Token: 0x0600222C RID: 8748 RVA: 0x000CC704 File Offset: 0x000CA904
		private new void OnEnable()
		{
			base.OnEnable();
			this.flagAmount.text = GameController.GameManager.PlayerMaster.matFaction.FactionTokens.GetUnplacedTokensCount().ToString();
			if (PlatformManager.IsMobile)
			{
				this.flagsPanelMobile.UpdateFlagsPanels();
			}
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x0003DD3D File Offset: 0x0003BF3D
		private void EnableRotateTowardsCamera(bool enable)
		{
			this.rotateTowardsCamera.enabled = enable;
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x0003DD4B File Offset: 0x0003BF4B
		public void ShowPanel(GameHexPresenter attachedGameHex, Unit initiator)
		{
			base.Clear();
			base.FinishPreviousAnimations(true);
			this.initiator = GameController.GetUnitPresenter(initiator);
			this.AttachFlagsPanel(attachedGameHex);
			base.ShowPanelTriggerAnimation();
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x0003DD73 File Offset: 0x0003BF73
		public void AttachFlagsPanel(GameHexPresenter attachedGameHex)
		{
			this.attachedGameHex = attachedGameHex;
			base.UpdatePosition();
			base.SetActive(true);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x0003DD89 File Offset: 0x0003BF89
		protected override void OnShowTokenPanelComplete()
		{
			base.OnShowTokenPanelComplete();
			if (PlatformManager.IsMobile)
			{
				this.flagsPanelMobile.UpdateFlagsPanels();
				return;
			}
			this.placeFlagButton.interactable = true;
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x0003DDB0 File Offset: 0x0003BFB0
		protected override void PreHideTokenPanel()
		{
			if (PlatformManager.IsMobile)
			{
				this.flagsPanelMobile.PreHideTokenPanel();
				return;
			}
			this.placeFlagButton.interactable = false;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0003DDD1 File Offset: 0x0003BFD1
		public void Hide()
		{
			if (!PlatformManager.IsMobile)
			{
				base.HidePanelTriggerAnimation();
				return;
			}
			if (SingletonMono<FlagsPanelMobile>.Instance.isPreview)
			{
				base.HidePanelMobile(false);
				return;
			}
			base.HidePanelMobile(true);
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000CC758 File Offset: 0x000CA958
		public void TokenSelected()
		{
			GameHex gameHexLogic = this.attachedGameHex.GetGameHexLogic();
			base.tokenManager.GetLastMovedUnit();
			base.tokenManager.PlaceToken(gameHexLogic);
			this.tokensMenuController.TokenPlaced();
			this.Hide();
		}

		// Token: 0x040017C6 RID: 6086
		[SerializeField]
		private Transform flagSprite;

		// Token: 0x040017C7 RID: 6087
		[SerializeField]
		private Button placeFlagButton;

		// Token: 0x040017C8 RID: 6088
		[SerializeField]
		private TextMeshProUGUI flagAmount;

		// Token: 0x040017C9 RID: 6089
		[SerializeField]
		private FlagsPanelMobile flagsPanelMobile;
	}
}
