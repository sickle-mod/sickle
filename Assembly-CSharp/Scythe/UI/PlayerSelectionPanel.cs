using System;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003FB RID: 1019
	public class PlayerSelectionPanel : MonoBehaviour
	{
		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06001EFF RID: 7935 RVA: 0x000BEA8C File Offset: 0x000BCC8C
		// (remove) Token: 0x06001F00 RID: 7936 RVA: 0x000BEAC4 File Offset: 0x000BCCC4
		public event Action<Player> PlayerSelected = delegate
		{
		};

		// Token: 0x06001F01 RID: 7937 RVA: 0x0003C0CF File Offset: 0x0003A2CF
		private void Awake()
		{
			this.SpawnPlayerButtons();
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0003C0D7 File Offset: 0x0003A2D7
		public void Show()
		{
			this.RefreshPlayerButtons();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x000BEAFC File Offset: 0x000BCCFC
		private void SpawnPlayerButtons()
		{
			this.playerButtons = new Button[GameController.GameManager.PlayerCount];
			for (int i = 0; i < this.playerButtons.Length; i++)
			{
				int playerID = i;
				Player player = GameController.GameManager.players[playerID];
				this.playerButtons[playerID] = global::UnityEngine.Object.Instantiate<Button>(this.playerButtonPrefab, this.playerButtonContainer);
				this.playerButtons[playerID].onClick.AddListener(delegate
				{
					this.OnPlayerButtonClick(playerID);
				});
				this.playerButtons[playerID].GetComponentInChildren<Image>().sprite = this.factionLogoSprites[(int)player.matFaction.faction];
				this.playerButtons[playerID].GetComponentInChildren<TextMeshProUGUI>().text = player.Name;
			}
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000BEBE8 File Offset: 0x000BCDE8
		private void RefreshPlayerButtons()
		{
			for (int i = 0; i < this.playerButtons.Length; i++)
			{
				this.playerButtons[i].gameObject.SetActive(i != GameController.GameManager.PlayerCurrentId);
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000BEC2C File Offset: 0x000BCE2C
		private void OnPlayerButtonClick(int i)
		{
			Player player = GameController.GameManager.players[i];
			this.PlayerSelected(player);
		}

		// Token: 0x040015E0 RID: 5600
		[SerializeField]
		private Transform playerButtonContainer;

		// Token: 0x040015E1 RID: 5601
		[SerializeField]
		private Button playerButtonPrefab;

		// Token: 0x040015E2 RID: 5602
		[SerializeField]
		private Sprite[] factionLogoSprites;

		// Token: 0x040015E3 RID: 5603
		private Button[] playerButtons;
	}
}
