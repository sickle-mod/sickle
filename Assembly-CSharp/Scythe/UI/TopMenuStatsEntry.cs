using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004AA RID: 1194
	public class TopMenuStatsEntry : MonoBehaviour
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000402D1 File Offset: 0x0003E4D1
		public TextMeshProUGUI TextPower
		{
			get
			{
				return this.powerAmount;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000402D9 File Offset: 0x0003E4D9
		public TextMeshProUGUI TextCombatCards
		{
			get
			{
				return this.combatCardsAmount;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000402E1 File Offset: 0x0003E4E1
		public Faction Faction
		{
			get
			{
				if (this.player == null)
				{
					return Faction.Polania;
				}
				return this.player.matFaction.faction;
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000402FD File Offset: 0x0003E4FD
		public void InitializeEntry(Player player, Color backgroundColor)
		{
			this.player = player;
			this.backgroundImage.color = backgroundColor;
			this.factionLogo.sprite = GameController.factionInfo[player.matFaction.faction].logo;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00040337 File Offset: 0x0003E537
		public void UpdateEntry(Player player)
		{
			this.player = player;
			this.UpdateStats();
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000E12E4 File Offset: 0x000DF4E4
		public void UpdateVisibility(bool[] visibilityArray)
		{
			if (visibilityArray.Length == this.cells.Length)
			{
				for (int i = 0; i < this.cells.Length; i++)
				{
					this.cells[i].SetActive(visibilityArray[i]);
				}
				return;
			}
			Debug.LogError(string.Concat(new string[]
			{
				"Visibility array length (",
				visibilityArray.Length.ToString(),
				") doesn't match cells length (",
				this.cells.Length.ToString(),
				")"
			}));
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000E136C File Offset: 0x000DF56C
		private void UpdateStats()
		{
			Dictionary<ResourceType, int> dictionary = this.player.Resources(false);
			this.starsAmount.text = this.player.GetNumberOfStars().ToString();
			this.coinsAmount.text = this.player.Coins.ToString();
			this.popularityAmount.text = this.player.Popularity.ToString();
			this.powerAmount.text = this.player.Power.ToString();
			this.combatCardsAmount.text = this.player.GetCombatCardsCount().ToString();
			this.ironAmount.text = dictionary[ResourceType.metal].ToString();
			this.woodAmount.text = dictionary[ResourceType.wood].ToString();
			this.oilAmount.text = dictionary[ResourceType.oil].ToString();
			this.foodAmount.text = dictionary[ResourceType.food].ToString();
			this.upgradesAmount.text = this.player.matPlayer.UpgradesDone.ToString();
			this.mechsAmount.text = this.player.matFaction.mechs.Count.ToString();
			this.buildingsAmount.text = this.player.matPlayer.buildings.Count.ToString();
			this.recruitsAmount.text = this.player.matPlayer.RecruitsEnlisted.ToString();
			this.workersAmount.text = this.player.matPlayer.workers.Count.ToString();
			this.objectivesAmount.text = this.player.ObjectivesDone.ToString();
			this.victoriesAmount.text = this.player.Victories.ToString();
		}

		// Token: 0x04001ABF RID: 6847
		[SerializeField]
		private Image backgroundImage;

		// Token: 0x04001AC0 RID: 6848
		[SerializeField]
		private GameObject[] cells;

		// Token: 0x04001AC1 RID: 6849
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04001AC2 RID: 6850
		[SerializeField]
		private TextMeshProUGUI starsAmount;

		// Token: 0x04001AC3 RID: 6851
		[SerializeField]
		private TextMeshProUGUI coinsAmount;

		// Token: 0x04001AC4 RID: 6852
		[SerializeField]
		private TextMeshProUGUI popularityAmount;

		// Token: 0x04001AC5 RID: 6853
		[SerializeField]
		private TextMeshProUGUI powerAmount;

		// Token: 0x04001AC6 RID: 6854
		[SerializeField]
		private TextMeshProUGUI combatCardsAmount;

		// Token: 0x04001AC7 RID: 6855
		[SerializeField]
		private TextMeshProUGUI ironAmount;

		// Token: 0x04001AC8 RID: 6856
		[SerializeField]
		private TextMeshProUGUI woodAmount;

		// Token: 0x04001AC9 RID: 6857
		[SerializeField]
		private TextMeshProUGUI oilAmount;

		// Token: 0x04001ACA RID: 6858
		[SerializeField]
		private TextMeshProUGUI foodAmount;

		// Token: 0x04001ACB RID: 6859
		[SerializeField]
		private TextMeshProUGUI upgradesAmount;

		// Token: 0x04001ACC RID: 6860
		[SerializeField]
		private TextMeshProUGUI mechsAmount;

		// Token: 0x04001ACD RID: 6861
		[SerializeField]
		private TextMeshProUGUI buildingsAmount;

		// Token: 0x04001ACE RID: 6862
		[SerializeField]
		private TextMeshProUGUI recruitsAmount;

		// Token: 0x04001ACF RID: 6863
		[SerializeField]
		private TextMeshProUGUI workersAmount;

		// Token: 0x04001AD0 RID: 6864
		[SerializeField]
		private TextMeshProUGUI objectivesAmount;

		// Token: 0x04001AD1 RID: 6865
		[SerializeField]
		private TextMeshProUGUI victoriesAmount;

		// Token: 0x04001AD2 RID: 6866
		private Player player;
	}
}
