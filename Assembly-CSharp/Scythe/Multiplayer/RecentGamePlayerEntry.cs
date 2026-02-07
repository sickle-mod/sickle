using System;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200029C RID: 668
	public class RecentGamePlayerEntry : MonoBehaviour
	{
		// Token: 0x0600151F RID: 5407 RVA: 0x0009D130 File Offset: 0x0009B330
		public void Initialize(PlayerEndGameStats playerEndGameStats, RecentGameEntry main)
		{
			this.background.color = this.factionColors[playerEndGameStats.faction];
			this.factionLogo.sprite = main.FactionLogo(playerEndGameStats.faction);
			TMP_Text tmp_Text = this.factionName;
			string text = "FactionMat/";
			Faction faction = (Faction)playerEndGameStats.faction;
			tmp_Text.text = ScriptLocalization.Get(text + faction.ToString());
			this.playerName.text = playerEndGameStats.name;
			this.popularityTier.text = ScriptLocalization.Get("Statistics/Tier") + " " + this.PopularityTier(playerEndGameStats.popularity).ToString();
			this.stars.text = playerEndGameStats.starPoints.ToString();
			this.hexes.text = playerEndGameStats.territoryPoints.ToString();
			this.resources.text = playerEndGameStats.resourcePoints.ToString();
			this.coins.text = playerEndGameStats.coinPoints.ToString();
			this.buildings.text = playerEndGameStats.structurePoints.ToString();
			this.total.text = playerEndGameStats.totalPoints.ToString();
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x000364C6 File Offset: 0x000346C6
		private int PopularityTier(int popularity)
		{
			if (this.LowTier(popularity))
			{
				return 1;
			}
			if (this.MediumTier(popularity))
			{
				return 2;
			}
			if (this.HighTier(popularity))
			{
				return 3;
			}
			return 0;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000364EA File Offset: 0x000346EA
		public bool LowTier(int amount)
		{
			return amount >= 0 && amount <= 6;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000364F9 File Offset: 0x000346F9
		public bool MediumTier(int amount)
		{
			return amount >= 7 && amount <= 12;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00036509 File Offset: 0x00034709
		public bool HighTier(int amount)
		{
			return amount >= 13 && amount <= 18;
		}

		// Token: 0x04000F73 RID: 3955
		[SerializeField]
		private Image background;

		// Token: 0x04000F74 RID: 3956
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private TextMeshProUGUI factionName;

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04000F77 RID: 3959
		[SerializeField]
		private TextMeshProUGUI popularityTier;

		// Token: 0x04000F78 RID: 3960
		[SerializeField]
		private TextMeshProUGUI stars;

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private TextMeshProUGUI hexes;

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private TextMeshProUGUI resources;

		// Token: 0x04000F7B RID: 3963
		[SerializeField]
		private TextMeshProUGUI coins;

		// Token: 0x04000F7C RID: 3964
		[SerializeField]
		private TextMeshProUGUI buildings;

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private TextMeshProUGUI total;

		// Token: 0x04000F7E RID: 3966
		[SerializeField]
		private Color[] factionColors;
	}
}
