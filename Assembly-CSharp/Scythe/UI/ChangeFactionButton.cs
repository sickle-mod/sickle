using System;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004BC RID: 1212
	public class ChangeFactionButton : MonoBehaviour
	{
		// Token: 0x06002693 RID: 9875 RVA: 0x00040A1F File Offset: 0x0003EC1F
		public void Init(Faction faction)
		{
			this.node = new ChangeFactionNode(faction);
			this.factionImage.sprite = GameController.factionInfo[faction].logo;
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000E4D4C File Offset: 0x000E2F4C
		public void SwitchFaction()
		{
			this.node.SwitchFaction();
			Player playerOwner = GameController.GameManager.PlayerOwner;
			GameController.FactionInfo factionInfo = GameController.factionInfo[playerOwner.matFaction.faction];
			GameController.Instance.matFaction.UpdateMat(playerOwner, factionInfo, true);
			if (!PlatformManager.IsStandalone)
			{
				SingletonMono<TopMenuPanelsManager>.Instance.UpdateInfoFromMat(playerOwner, factionInfo, true);
			}
			GameController.Instance.matPlayer.isPreview = true;
			GameController.Instance.matPlayer.UpdateMat(playerOwner, false);
			GameController.Instance.matPlayer.isPreview = false;
			GameController.Instance.UpdateStats(false, false);
			if (this.buttonClicked != null)
			{
				this.buttonClicked();
			}
		}

		// Token: 0x04001B91 RID: 7057
		[SerializeField]
		private Image factionImage;

		// Token: 0x04001B92 RID: 7058
		private ChangeFactionNode node;

		// Token: 0x04001B93 RID: 7059
		public Action buttonClicked;
	}
}
