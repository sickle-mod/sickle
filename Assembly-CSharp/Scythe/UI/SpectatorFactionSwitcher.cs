using System;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000499 RID: 1177
	public class SpectatorFactionSwitcher : SingletonMono<SpectatorFactionSwitcher>
	{
		// Token: 0x140000EE RID: 238
		// (add) Token: 0x06002563 RID: 9571 RVA: 0x000DE75C File Offset: 0x000DC95C
		// (remove) Token: 0x06002564 RID: 9572 RVA: 0x000DE794 File Offset: 0x000DC994
		public event Action SpectatorFactionSwitched = delegate
		{
		};

		// Token: 0x06002565 RID: 9573 RVA: 0x0003FCC0 File Offset: 0x0003DEC0
		private void Start()
		{
			this.SpawnButtons();
			this.DetermineVisibility();
			this.InitializeFaction();
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000DE7CC File Offset: 0x000DC9CC
		private void SpawnButtons()
		{
			foreach (object obj in Enum.GetValues(typeof(Faction)))
			{
				Faction faction = (Faction)obj;
				if (faction != Faction.Albion && faction != Faction.Togawa)
				{
					this.SpawnButton(faction);
				}
			}
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000DE838 File Offset: 0x000DCA38
		private void SpawnButton(Faction faction)
		{
			global::UnityEngine.Object.Instantiate<SpectatorFactionSwitcherButton>(this.buttonPrefab, this.buttonContainer).Initialize(faction, delegate
			{
				this.SwitchFaction(faction);
			});
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x0003FCD4 File Offset: 0x0003DED4
		private void DetermineVisibility()
		{
			base.gameObject.SetActive(GameController.GameManager.SpectatorMode);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x0003FCEB File Offset: 0x0003DEEB
		private void InitializeFaction()
		{
			if (GameController.GameManager.SpectatorMode)
			{
				this.SwitchFaction(GameController.GameManager.players[0].matFaction.faction);
			}
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000DE884 File Offset: 0x000DCA84
		private void SwitchFaction(Faction faction)
		{
			GameController.GameManager.SetOwnerIdFromFaction(faction);
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
			this.SpectatorFactionSwitched();
		}

		// Token: 0x04001A35 RID: 6709
		[SerializeField]
		private SpectatorFactionSwitcherButton buttonPrefab;

		// Token: 0x04001A36 RID: 6710
		[SerializeField]
		private RectTransform buttonContainer;
	}
}
