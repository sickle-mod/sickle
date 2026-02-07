using System;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000275 RID: 629
	public class RemoveFriendPanel : MonoBehaviour
	{
		// Token: 0x06001357 RID: 4951 RVA: 0x0009876C File Offset: 0x0009696C
		public void Activate(PlayerListEntry playerListEntry)
		{
			if (PlatformManager.IsStandalone)
			{
				this.playerListEntry = playerListEntry;
			}
			else
			{
				this.playerListEntryMobile = (PlayerListEntryMobile)playerListEntry;
			}
			this.removeContentText.text = ScriptLocalization.Get("Lobby/RemoveQuestion").Replace("{[PLAYER_NAME]}", playerListEntry.playerData.PlayerStats.Name);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00034EB5 File Offset: 0x000330B5
		public void RemoveFriend()
		{
			if (PlatformManager.IsStandalone)
			{
				this.playerListEntry.RemoveFromFriends();
			}
			else
			{
				this.playerListEntryMobile.RemoveBuddy();
			}
			this.Close();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00032C2B File Offset: 0x00030E2B
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E82 RID: 3714
		[SerializeField]
		private TextMeshProUGUI removeContentText;

		// Token: 0x04000E83 RID: 3715
		private PlayerListEntry playerListEntry;

		// Token: 0x04000E84 RID: 3716
		private PlayerListEntryMobile playerListEntryMobile;
	}
}
