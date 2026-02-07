using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004BE RID: 1214
	public class ChangeFactionPanel : MonoBehaviour
	{
		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06002698 RID: 9880 RVA: 0x000E4DFC File Offset: 0x000E2FFC
		// (remove) Token: 0x06002699 RID: 9881 RVA: 0x000E4E30 File Offset: 0x000E3030
		public static event Action spectatingFactionSwitched;

		// Token: 0x0600269A RID: 9882 RVA: 0x000E4E64 File Offset: 0x000E3064
		public void Init()
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			Player player = players.First<Player>();
			Player player2 = players.Last<Player>();
			foreach (Player player3 in players)
			{
				this.InstantiateButton(player3.matFaction.faction, this.GetButton(player3, player, player2));
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x00040A86 File Offset: 0x0003EC86
		private ChangeFactionButton GetButton(Player player, Player firstPlayer, Player lastPlayer)
		{
			if (player == firstPlayer)
			{
				return this.buttonUp;
			}
			if (player == lastPlayer)
			{
				return this.buttonBottom;
			}
			return this.buttonMiddle;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000E4EE8 File Offset: 0x000E30E8
		private void InstantiateButton(Faction faction, ChangeFactionButton prefab)
		{
			ChangeFactionButton changeFactionButton = global::UnityEngine.Object.Instantiate<ChangeFactionButton>(prefab, base.transform);
			changeFactionButton.gameObject.SetActive(true);
			changeFactionButton.Init(faction);
			changeFactionButton.buttonClicked = (Action)Delegate.Combine(changeFactionButton.buttonClicked, new Action(this.OnButtonClicked));
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x00040AA4 File Offset: 0x0003ECA4
		private void OnButtonClicked()
		{
			if (ChangeFactionPanel.spectatingFactionSwitched != null)
			{
				ChangeFactionPanel.spectatingFactionSwitched();
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x00040AB7 File Offset: 0x0003ECB7
		public void Disable()
		{
			ChangeFactionPanel.spectatingFactionSwitched = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001B95 RID: 7061
		[SerializeField]
		private ChangeFactionButton buttonUp;

		// Token: 0x04001B96 RID: 7062
		[SerializeField]
		private ChangeFactionButton buttonMiddle;

		// Token: 0x04001B97 RID: 7063
		[SerializeField]
		private ChangeFactionButton buttonBottom;
	}
}
