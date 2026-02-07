using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x020001F9 RID: 505
	public class PlayerListEntryDropdownMobile : MonoBehaviour
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0008B634 File Offset: 0x00089834
		public void ShowHide(bool show)
		{
			if (show)
			{
				if (this.playerListEntryMobile)
				{
					if (this.inviteToGameButton)
					{
						this.inviteToGameButton.interactable = this.playerListEntryMobile.IsOptionAvailable(0);
					}
					if (this.seaStatisticsButton)
					{
						this.seaStatisticsButton.interactable = this.playerListEntryMobile.IsOptionAvailable(1);
					}
					if (this.addBuddyButton)
					{
						this.addBuddyButton.interactable = this.playerListEntryMobile.IsOptionAvailable(2);
					}
					if (this.removeBuddyButton)
					{
						this.removeBuddyButton.interactable = this.playerListEntryMobile.IsOptionAvailable(3);
					}
					if (this.spectateButton)
					{
						this.spectateButton.interactable = this.playerListEntryMobile.IsOptionAvailable(2);
					}
				}
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000B9C RID: 2972
		[SerializeField]
		private PlayerListEntryMobile playerListEntryMobile;

		// Token: 0x04000B9D RID: 2973
		[SerializeField]
		private Button inviteToGameButton;

		// Token: 0x04000B9E RID: 2974
		[SerializeField]
		private Button seaStatisticsButton;

		// Token: 0x04000B9F RID: 2975
		[SerializeField]
		private Button addBuddyButton;

		// Token: 0x04000BA0 RID: 2976
		[SerializeField]
		private Button removeBuddyButton;

		// Token: 0x04000BA1 RID: 2977
		[SerializeField]
		private Button spectateButton;
	}
}
