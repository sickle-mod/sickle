using System;
using System.Linq;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200049C RID: 1180
	public class SpectatorFactionSwitcherButton : MonoBehaviour
	{
		// Token: 0x06002571 RID: 9585 RVA: 0x0003FD65 File Offset: 0x0003DF65
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			this.image = base.GetComponent<Image>();
			SingletonMono<SpectatorFactionSwitcher>.Instance.SpectatorFactionSwitched += this.OnSpectatorFactionSwitched;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000DE92C File Offset: 0x000DCB2C
		public void Initialize(Faction faction, UnityAction onClick)
		{
			this.faction = faction;
			this.image.sprite = this.factionLogoSprites[(int)faction];
			this.button.onClick.AddListener(onClick);
			this.button.interactable = GameController.GameManager.players.Any((Player p) => p.matFaction.faction == faction);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x0003FD95 File Offset: 0x0003DF95
		private void OnSpectatorFactionSwitched()
		{
			this.highlight.gameObject.SetActive(GameController.GameManager.PlayerOwner.matFaction.faction == this.faction);
		}

		// Token: 0x04001A3B RID: 6715
		[SerializeField]
		private Sprite[] factionLogoSprites;

		// Token: 0x04001A3C RID: 6716
		[SerializeField]
		private RectTransform highlight;

		// Token: 0x04001A3D RID: 6717
		private Faction faction;

		// Token: 0x04001A3E RID: 6718
		private Button button;

		// Token: 0x04001A3F RID: 6719
		private Image image;
	}
}
