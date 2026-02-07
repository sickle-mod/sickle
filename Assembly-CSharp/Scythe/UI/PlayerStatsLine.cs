using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B5 RID: 1205
	public class PlayerStatsLine : MonoBehaviour
	{
		// Token: 0x0600263B RID: 9787 RVA: 0x000407A8 File Offset: 0x0003E9A8
		private void Awake()
		{
			this.resourceAnimation = base.GetComponent<ResourcesGainAnimation>();
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000E3504 File Offset: 0x000E1704
		public void UpdateLine(Player player, Sprite logo, Color starInactive, bool skipAnimation)
		{
			this.player = player;
			if (this.emblem != null)
			{
				this.emblem.sprite = logo;
			}
			int numberOfStars = player.GetNumberOfStars();
			if (!PlatformManager.IsStandalone)
			{
				this.miniStarPower.sprite = ColorFactionMarks.colorFactionMarks[player.matFaction.faction].starMark;
				this.miniStarPopularity.sprite = ColorFactionMarks.colorFactionMarks[player.matFaction.faction].starMark;
			}
			for (int i = 0; i < this.stars.Length; i++)
			{
				if (PlatformManager.IsStandalone)
				{
					if (i < numberOfStars)
					{
						this.stars[i].color = Color.white;
					}
					else
					{
						this.stars[i].color = starInactive;
					}
				}
				else if (i < numberOfStars)
				{
					this.stars[i].sprite = ColorFactionMarks.colorFactionMarks[player.matFaction.faction].starMark;
					this.stars[i].color = Color.white;
				}
				else
				{
					this.stars[i].sprite = this.EmptyStarSprite;
					this.stars[i].color = Color.white;
				}
			}
			this.UpdateBasicStats(skipAnimation);
			NullcheckHelper.SetTextValueIfNotNull(this.upgradesText, player.matPlayer.UpgradesDone.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.recruitsText, player.matPlayer.RecruitsEnlisted.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.mechsText, player.matFaction.mechs.Count.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.buildingsText, player.matPlayer.buildings.Count.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.objectivesText, player.ObjectivesDone.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.victoriesText, player.Victories.ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.workersText, player.matPlayer.workers.Count.ToString());
			Dictionary<ResourceType, int> dictionary = player.Resources(false);
			NullcheckHelper.SetTextValueIfNotNull(this.foodText, dictionary[ResourceType.food].ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.metalText, dictionary[ResourceType.metal].ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.oilText, dictionary[ResourceType.oil].ToString());
			NullcheckHelper.SetTextValueIfNotNull(this.woodText, dictionary[ResourceType.wood].ToString());
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000407B6 File Offset: 0x0003E9B6
		private void UpdateBasicStats(bool skipAnimation)
		{
			this.UpdateCoins(skipAnimation);
			this.UpdatePopularity(skipAnimation);
			this.UpdatePower(skipAnimation);
			this.UpdateCombatCards(skipAnimation);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000E3788 File Offset: 0x000E1988
		private void UpdateCoins(bool skipAnimation)
		{
			if (this.cashText != null)
			{
				int num = int.Parse(this.cashText.text);
				if (num != this.player.Coins)
				{
					if ((GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn()) || (!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman))
					{
						GameController.Instance.playersFactions.HidePassCoinPresenter();
					}
					this.cashText.text = this.player.Coins.ToString();
					this.AddAmountToStats(this.player.Coins - num, GainType.Coin, skipAnimation);
				}
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000E383C File Offset: 0x000E1A3C
		private void UpdatePopularity(bool skipAnimation)
		{
			if (this.popularityText != null)
			{
				int num = int.Parse(this.popularityText.text);
				if (num != this.player.Popularity)
				{
					this.popularityText.text = this.player.Popularity.ToString();
					this.AddAmountToStats(this.player.Popularity - num, GainType.Popularity, skipAnimation);
				}
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000E38AC File Offset: 0x000E1AAC
		private void UpdatePower(bool skipAnimation)
		{
			if (this.powerText != null)
			{
				int num = int.Parse(this.powerText.text);
				if (num != this.player.Power)
				{
					this.powerText.text = this.player.Power.ToString();
					this.AddAmountToStats(this.player.Power - num, GainType.Power, skipAnimation);
				}
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000E391C File Offset: 0x000E1B1C
		private void UpdateCombatCards(bool skipAnimation)
		{
			if (this.bombsText != null)
			{
				int num = int.Parse(this.bombsText.text);
				if (num != this.player.GetCombatCardsCount())
				{
					this.bombsText.text = this.player.GetCombatCardsCount().ToString();
					this.AddAmountToStats(this.player.GetCombatCardsCount() - num, GainType.CombatCard, skipAnimation);
				}
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000E398C File Offset: 0x000E1B8C
		public void SetMiniStarsVisibility(bool show)
		{
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarVictories, show && this.player.GetNumberOfStars(StarType.Combat) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarMechs, show && this.player.GetNumberOfStars(StarType.Mechs) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarObjectives, show && this.player.GetNumberOfStars(StarType.Objective) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarPopularity, show && this.player.GetNumberOfStars(StarType.Popularity) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarPower, show && this.player.GetNumberOfStars(StarType.Power) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarRecruits, show && this.player.GetNumberOfStars(StarType.Recruits) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarBuildings, show && this.player.GetNumberOfStars(StarType.Structures) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarUpgrades, show && this.player.GetNumberOfStars(StarType.Upgrades) > 0);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.miniStarWorkers, show && this.player.GetNumberOfStars(StarType.Workers) > 0);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000E3ABC File Offset: 0x000E1CBC
		public void SetIconsVisibility(bool show)
		{
			foreach (Image image in this.icons)
			{
				if (image != null)
				{
					image.enabled = show;
				}
			}
			if (show)
			{
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starPopularity, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starPower, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starBuildings, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starMechs, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starObjectives, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starRecruits, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starUpgrades, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starVictories, false);
				NullcheckHelper.SetGameObjectActiveIfNotNull(this.starWorkers, false);
				return;
			}
			int numberOfStars = this.player.GetNumberOfStars(StarType.Combat);
			if (this.starVictories != null)
			{
				if (numberOfStars > 0)
				{
					this.starVictories.SetActive(true);
					if (numberOfStars > 1)
					{
						this.starVictories.transform.GetChild(0).GetComponent<Text>().text = numberOfStars.ToString();
					}
					else
					{
						this.starVictories.transform.GetChild(0).GetComponent<Text>().text = string.Empty;
					}
				}
				else
				{
					this.starVictories.SetActive(false);
				}
			}
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starMechs, this.player.GetNumberOfStars(StarType.Mechs) > 0);
			if (this.starObjectives != null)
			{
				int numberOfStars2 = this.player.GetNumberOfStars(StarType.Objective);
				if (numberOfStars2 > 0)
				{
					this.starObjectives.SetActive(true);
					if (numberOfStars2 > 1)
					{
						this.starObjectives.transform.GetChild(0).GetComponent<Text>().text = numberOfStars2.ToString();
					}
					else
					{
						this.starObjectives.transform.GetChild(0).GetComponent<Text>().text = string.Empty;
					}
				}
				else
				{
					this.starObjectives.SetActive(false);
				}
			}
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starPopularity, this.player.GetNumberOfStars(StarType.Popularity) > 0);
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starPower, this.player.GetNumberOfStars(StarType.Power) > 0);
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starRecruits, this.player.GetNumberOfStars(StarType.Recruits) > 0);
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starBuildings, this.player.GetNumberOfStars(StarType.Structures) > 0);
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starUpgrades, this.player.GetNumberOfStars(StarType.Upgrades) > 0);
			NullcheckHelper.SetGameObjectActiveIfNotNull(this.starWorkers, this.player.GetNumberOfStars(StarType.Workers) > 0);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000E3D20 File Offset: 0x000E1F20
		public void SetLabelVisibility(bool show)
		{
			NullcheckHelper.SetComponentEnabledIfNotNull(this.cashText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.popularityText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.powerText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.bombsText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.upgradesText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.recruitsText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.workersText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.mechsText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.buildingsText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.objectivesText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.victoriesText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.foodText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.metalText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.oilText, show);
			NullcheckHelper.SetComponentEnabledIfNotNull(this.woodText, show);
			for (int i = 0; i < this.stars.Length; i++)
			{
				this.stars[i].enabled = show;
			}
			for (int j = 0; j < this.iconPushers.Length; j++)
			{
				this.iconPushers[j].SetActive(!show);
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.iconPushers[j].transform.parent.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000407D4 File Offset: 0x0003E9D4
		public void LogoPressed(bool down)
		{
			base.GetComponentInParent<PlayerStatsPresenter>().matPreview.Visibility(down, this.player);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000E3E44 File Offset: 0x000E2044
		public void AddAmountToStats(int amount, GainType gainType, bool skipAnimation)
		{
			Image image = null;
			switch (gainType)
			{
			case GainType.Coin:
				image = this.icons[0];
				break;
			case GainType.Popularity:
				image = this.icons[1];
				break;
			case GainType.Power:
				image = this.icons[2];
				break;
			case GainType.CombatCard:
				image = this.icons[3];
				break;
			}
			if (!skipAnimation && this.player.IsHuman)
			{
				if (image != null && this.resourceAnimation != null)
				{
					this.resourceAnimation.SetupResourcesToAnimation(amount, image.sprite, image.transform.position);
				}
				if (amount > 0 && this.contextAreaGainNotification != null)
				{
					this.contextAreaGainNotification.ShowAnimation(gainType, amount);
				}
			}
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000407ED File Offset: 0x0003E9ED
		public void StopStatsAnimation()
		{
			if (this.resourceAnimation != null)
			{
				this.resourceAnimation.StopAnimation();
			}
		}

		// Token: 0x04001B3D RID: 6973
		public Sprite EmptyStarSprite;

		// Token: 0x04001B3E RID: 6974
		public Image emblem;

		// Token: 0x04001B3F RID: 6975
		public Image[] stars;

		// Token: 0x04001B40 RID: 6976
		public Text cashText;

		// Token: 0x04001B41 RID: 6977
		public Text popularityText;

		// Token: 0x04001B42 RID: 6978
		public Text powerText;

		// Token: 0x04001B43 RID: 6979
		public Text bombsText;

		// Token: 0x04001B44 RID: 6980
		public Text upgradesText;

		// Token: 0x04001B45 RID: 6981
		public Text recruitsText;

		// Token: 0x04001B46 RID: 6982
		public Text workersText;

		// Token: 0x04001B47 RID: 6983
		public Text mechsText;

		// Token: 0x04001B48 RID: 6984
		public Text buildingsText;

		// Token: 0x04001B49 RID: 6985
		public Text objectivesText;

		// Token: 0x04001B4A RID: 6986
		public Text victoriesText;

		// Token: 0x04001B4B RID: 6987
		public Text foodText;

		// Token: 0x04001B4C RID: 6988
		public Text metalText;

		// Token: 0x04001B4D RID: 6989
		public Text oilText;

		// Token: 0x04001B4E RID: 6990
		public Text woodText;

		// Token: 0x04001B4F RID: 6991
		public Image[] icons;

		// Token: 0x04001B50 RID: 6992
		public GameObject extension;

		// Token: 0x04001B51 RID: 6993
		public GameObject[] iconPushers;

		// Token: 0x04001B52 RID: 6994
		public GameObject starPopularity;

		// Token: 0x04001B53 RID: 6995
		public GameObject starPower;

		// Token: 0x04001B54 RID: 6996
		public GameObject starUpgrades;

		// Token: 0x04001B55 RID: 6997
		public GameObject starRecruits;

		// Token: 0x04001B56 RID: 6998
		public GameObject starWorkers;

		// Token: 0x04001B57 RID: 6999
		public GameObject starMechs;

		// Token: 0x04001B58 RID: 7000
		public GameObject starBuildings;

		// Token: 0x04001B59 RID: 7001
		public GameObject starObjectives;

		// Token: 0x04001B5A RID: 7002
		public GameObject starVictories;

		// Token: 0x04001B5B RID: 7003
		public Image miniStarPopularity;

		// Token: 0x04001B5C RID: 7004
		public Image miniStarPower;

		// Token: 0x04001B5D RID: 7005
		public Image miniStarUpgrades;

		// Token: 0x04001B5E RID: 7006
		public Image miniStarRecruits;

		// Token: 0x04001B5F RID: 7007
		public Image miniStarWorkers;

		// Token: 0x04001B60 RID: 7008
		public Image miniStarMechs;

		// Token: 0x04001B61 RID: 7009
		public Image miniStarBuildings;

		// Token: 0x04001B62 RID: 7010
		public Image miniStarObjectives;

		// Token: 0x04001B63 RID: 7011
		public Image miniStarVictories;

		// Token: 0x04001B64 RID: 7012
		public ContextAreaGainNotification contextAreaGainNotification;

		// Token: 0x04001B65 RID: 7013
		private ResourcesGainAnimation resourceAnimation;

		// Token: 0x04001B66 RID: 7014
		private Player player;
	}
}
