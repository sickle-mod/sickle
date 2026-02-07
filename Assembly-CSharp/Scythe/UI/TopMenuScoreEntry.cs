using System;
using System.Linq;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004A2 RID: 1186
	public class TopMenuScoreEntry : MonoBehaviour
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x00040093 File Offset: 0x0003E293
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

		// Token: 0x060025B5 RID: 9653 RVA: 0x000400AF File Offset: 0x0003E2AF
		public void InitializeEntry(Player player, Color backgroundColor)
		{
			this.player = player;
			this.entryBackground.color = backgroundColor;
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000E03D8 File Offset: 0x000DE5D8
		public void UpdateEntry(PlayerEndGameStats playerStats, bool basic = true)
		{
			bool gameFinished = GameController.GameManager.GameFinished;
			bool isMultiplayer = GameController.GameManager.IsMultiplayer;
			bool isRanked = GameController.GameManager.IsRanked;
			bool isHuman = playerStats.player.IsHuman;
			bool flag = GameController.GameManager.GetPlayerByFaction(Faction.Polania) != null;
			bool flag2 = GameController.GameManager.PlayerCount > 5;
			string popularityRichTextColor = this.GetPopularityRichTextColor(playerStats.player.Popularity);
			if (gameFinished)
			{
				this.SetLaurel(this.placeOnPodium);
				this.laurelCell.SetActive(true);
			}
			else
			{
				this.laurelCell.SetActive(false);
			}
			if (isMultiplayer)
			{
				this.nicknameText.text = MultiplayerController.Instance.GetPlayersInGame().ToList<PlayerData>().Find((PlayerData player) => player.Faction == (int)playerStats.player.matFaction.faction)
					.Name;
			}
			else
			{
				this.nicknameText.text = playerStats.player.Name;
			}
			this.factionLogo.sprite = GameController.factionInfo[this.player.matFaction.faction].logo;
			this.factionNameText.text = ScriptLocalization.Get("FactionMat/" + playerStats.player.matFaction.faction.ToString());
			if (flag && flag2)
			{
				this.polaniaBonusCell.SetActive(true);
				if (this.player.matFaction.faction == Faction.Polania)
				{
					this.polaniaBonusText.text = "+" + new FindWinner(GameController.GameManager).CalculatePolaniaBonusPoints(playerStats.player, GameController.GameManager.players.Count<Player>()).ToString();
					this.polaniaBonusObject.SetActive(true);
				}
				else
				{
					this.polaniaBonusObject.SetActive(false);
				}
			}
			else
			{
				this.polaniaBonusCell.SetActive(false);
			}
			this.tierText.text = (basic ? this.GetTierString(PopularityTrack.PopularityTier(playerStats.player.Popularity)) : this.GetColorizedString(playerStats.player.Popularity.ToString() + " " + this.GetTierString(PopularityTrack.PopularityTier(playerStats.player.Popularity)), popularityRichTextColor));
			int num = PopularityTrack.StarsMultiplier(playerStats.player.Popularity);
			this.starValue.text = (basic ? playerStats.starPoints.ToString() : this.GetEquation(playerStats.starPoints, num, popularityRichTextColor));
			num = PopularityTrack.TerritoryMultiplier(playerStats.player.Popularity);
			this.hexValue.text = (basic ? playerStats.territoryPoints.ToString() : this.GetEquation(playerStats.territoryPoints, num, popularityRichTextColor));
			num = PopularityTrack.ResourceMultiplier(playerStats.player.Popularity);
			this.resourcesValue.text = playerStats.resourcePoints.ToString();
			this.resourcesValue.text = (basic ? playerStats.resourcePoints.ToString() : this.GetEquation(playerStats.resourcePoints, num, popularityRichTextColor));
			this.coinsValue.text = playerStats.coinPoints.ToString();
			this.structureBonusValue.text = playerStats.structurePoints.ToString();
			this.totalValue.text = playerStats.totalPoints.ToString();
			if (isMultiplayer && isRanked && gameFinished)
			{
				this.eloValue.text = (isHuman ? this.GetELOString(playerStats.oldRating, playerStats.rating) : "");
				this.eloCell.SetActive(true);
				return;
			}
			this.eloCell.SetActive(false);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000E07E0 File Offset: 0x000DE9E0
		public void UpdateEntryLanguage(PlayerEndGameStats playerStats, bool basic)
		{
			string popularityRichTextColor = this.GetPopularityRichTextColor(playerStats.player.Popularity);
			this.factionNameText.text = ScriptLocalization.Get("FactionMat/" + playerStats.player.matFaction.faction.ToString());
			this.tierText.text = (basic ? this.GetTierString(PopularityTrack.PopularityTier(playerStats.player.Popularity)) : this.GetColorizedString(playerStats.player.Popularity.ToString() + " " + this.GetTierString(PopularityTrack.PopularityTier(playerStats.player.Popularity)), popularityRichTextColor));
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000400C4 File Offset: 0x0003E2C4
		public void SetPlaceOnPodium(int? playerPlace)
		{
			this.placeOnPodium = playerPlace;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000E0894 File Offset: 0x000DEA94
		private void SetLaurel(int? place)
		{
			int? num = place + 1;
			if (num != null)
			{
				switch (num.GetValueOrDefault())
				{
				case 1:
					this.laurelImage.sprite = this.goldLaurel;
					this.laurelImage.enabled = true;
					goto IL_00C1;
				case 2:
					this.laurelImage.sprite = this.silverLaurel;
					this.laurelImage.enabled = true;
					goto IL_00C1;
				case 3:
					this.laurelImage.sprite = this.bronzeLaurel;
					this.laurelImage.enabled = true;
					goto IL_00C1;
				}
			}
			this.laurelImage.sprite = null;
			this.laurelImage.enabled = false;
			IL_00C1:
			this.laurelCell.SetActive(true);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x000400CD File Offset: 0x0003E2CD
		private string GetEquation(int score, int multiplier, string color)
		{
			return string.Format("{0} x {1}", this.GetColorizedString(multiplier.ToString(), color), score / multiplier);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x0003F0AD File Offset: 0x0003D2AD
		private string GetColorizedString(string data, string color)
		{
			return string.Format("<color={0}>{1}</color>", color, data);
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000E0970 File Offset: 0x000DEB70
		private string GetTierString(int level)
		{
			string text = ScriptLocalization.Get("Statistics/Tier");
			string text2;
			switch (level)
			{
			case 1:
				text2 = " I";
				break;
			case 2:
				text2 = " II";
				break;
			case 3:
				text2 = " III";
				break;
			default:
				text2 = " I";
				break;
			}
			return text + text2;
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000400EF File Offset: 0x0003E2EF
		private string GetPopularityRichTextColor(int popularity)
		{
			if (PopularityTrack.LowTier(popularity))
			{
				return "#61C6E9FF";
			}
			if (PopularityTrack.MediumTier(popularity))
			{
				return "#FEBE37FF";
			}
			if (PopularityTrack.HighTier(popularity))
			{
				return "#0DFE2AFF";
			}
			return "\"white\"";
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x0003F0DD File Offset: 0x0003D2DD
		private string GetELOString(int oldRating, int rating)
		{
			return string.Format("{0} ({1})", rating, rating - oldRating);
		}

		// Token: 0x04001A8B RID: 6795
		[Header("Laurel Sprites")]
		[SerializeField]
		private Sprite goldLaurel;

		// Token: 0x04001A8C RID: 6796
		[SerializeField]
		private Sprite silverLaurel;

		// Token: 0x04001A8D RID: 6797
		[SerializeField]
		private Sprite bronzeLaurel;

		// Token: 0x04001A8E RID: 6798
		[Header("Cells References")]
		[SerializeField]
		private GameObject laurelCell;

		// Token: 0x04001A8F RID: 6799
		[SerializeField]
		private GameObject polaniaBonusCell;

		// Token: 0x04001A90 RID: 6800
		[SerializeField]
		private GameObject polaniaBonusObject;

		// Token: 0x04001A91 RID: 6801
		[SerializeField]
		private GameObject eloCell;

		// Token: 0x04001A92 RID: 6802
		[Header("Values References")]
		[SerializeField]
		private Image laurelImage;

		// Token: 0x04001A93 RID: 6803
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04001A94 RID: 6804
		[SerializeField]
		private TextMeshProUGUI factionNameText;

		// Token: 0x04001A95 RID: 6805
		[SerializeField]
		private TextMeshProUGUI nicknameText;

		// Token: 0x04001A96 RID: 6806
		[SerializeField]
		private TextMeshProUGUI polaniaBonusText;

		// Token: 0x04001A97 RID: 6807
		[SerializeField]
		private TextMeshProUGUI tierText;

		// Token: 0x04001A98 RID: 6808
		[SerializeField]
		private TextMeshProUGUI starValue;

		// Token: 0x04001A99 RID: 6809
		[SerializeField]
		private TextMeshProUGUI hexValue;

		// Token: 0x04001A9A RID: 6810
		[SerializeField]
		private TextMeshProUGUI resourcesValue;

		// Token: 0x04001A9B RID: 6811
		[SerializeField]
		private TextMeshProUGUI coinsValue;

		// Token: 0x04001A9C RID: 6812
		[SerializeField]
		private TextMeshProUGUI structureBonusValue;

		// Token: 0x04001A9D RID: 6813
		[SerializeField]
		private TextMeshProUGUI totalValue;

		// Token: 0x04001A9E RID: 6814
		[SerializeField]
		private TextMeshProUGUI eloValue;

		// Token: 0x04001A9F RID: 6815
		[SerializeField]
		private Image entryBackground;

		// Token: 0x04001AA0 RID: 6816
		private Player player;

		// Token: 0x04001AA1 RID: 6817
		private int? placeOnPodium;
	}
}
