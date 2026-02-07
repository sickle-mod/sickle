using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B6 RID: 1206
	public class PlayerStatsPresenter : MonoBehaviour
	{
		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06002649 RID: 9801 RVA: 0x000E3EF8 File Offset: 0x000E20F8
		// (remove) Token: 0x0600264A RID: 9802 RVA: 0x000E3F2C File Offset: 0x000E212C
		public static event PlayerStatsPresenter.BottomArrowExpand BottomArrowExpanded;

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x0600264B RID: 9803 RVA: 0x000E3F60 File Offset: 0x000E2160
		// (remove) Token: 0x0600264C RID: 9804 RVA: 0x000E3F94 File Offset: 0x000E2194
		public static event PlayerStatsPresenter.RightArrowExpand RightArrowExpanded;

		// Token: 0x0600264D RID: 9805 RVA: 0x00040808 File Offset: 0x0003EA08
		private void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.expanded = true;
			}
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x00040818 File Offset: 0x0003EA18
		private void OnDisable()
		{
			this.StopStatAnimations();
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000E3FC8 File Offset: 0x000E21C8
		private void StopStatAnimations()
		{
			this.header.StopStatsAnimation();
			PlayerStatsLine[] array = this.lines;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].StopStatsAnimation();
			}
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000E4000 File Offset: 0x000E2200
		private void UpdateTracks()
		{
			int length = Enum.GetValues(typeof(Faction)).Length;
			int length2 = Enum.GetValues(typeof(StarType)).Length;
			for (int i = 0; i < length; i++)
			{
				Player playerByFaction = GameController.GameManager.GetPlayerByFaction((Faction)i);
				if (playerByFaction == null)
				{
					if (this.trackMarkersPopularity[i] != null)
					{
						this.trackMarkersPopularity[i].gameObject.SetActive(false);
					}
					if (this.trackMarkersPower[i] != null)
					{
						this.trackMarkersPower[i].gameObject.SetActive(false);
					}
					for (int j = 0; j < length2; j++)
					{
						if (this.trumpTrack != null)
						{
							Transform child = this.trumpTrack.GetChild(j).GetChild(i);
							for (int k = 0; k < child.childCount; k++)
							{
								child.GetChild(k).gameObject.SetActive(false);
							}
						}
					}
				}
				else
				{
					if (this.trackMarkersPopularity[i] != null && this.trackMarkersPower[i] != null)
					{
						this.trackMarkersPopularity[i].gameObject.SetActive(true);
						this.trackMarkersPower[i].gameObject.SetActive(true);
						Vector3 vector = this.trackMarkersPopularity[i].localPosition;
						vector.y = -7.18f + 0.87f * (float)playerByFaction.Popularity;
						this.trackMarkersPopularity[i].localPosition = vector;
						vector = this.trackMarkersPower[i].localPosition;
						vector.y = -1.8f + 0.22f * (float)playerByFaction.Power;
						this.trackMarkersPower[i].localPosition = vector;
					}
					for (int l = 0; l < length2; l++)
					{
						if (this.trumpTrack != null)
						{
							Transform child2 = this.trumpTrack.GetChild(l).GetChild(i);
							for (int m = 0; m < child2.childCount; m++)
							{
								child2.GetChild(m).gameObject.SetActive(m < playerByFaction.stars[(StarType)l]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x00040820 File Offset: 0x0003EA20
		public void UpdateAllStats(Player player, Sprite logo)
		{
			this.UpdatePlayerStats(player, logo);
			this.player = player;
			if (!PlatformManager.IsStandalone && GameController.GameManager.PlayerCurrent == player)
			{
				SingletonMono<TopMenuPanelsManager>.Instance.UpdateStats(player);
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000E422C File Offset: 0x000E242C
		public void UpdatePlayerStats(Player player, Sprite logo)
		{
			this.header.UpdateLine(player, logo, this.starInactive, this.player != player);
			this.header.SetMiniStarsVisibility(!this.expandedDown);
			this.UpdateBottomLines(-1);
			this.UpdateTracks();
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000E427C File Offset: 0x000E247C
		public void Expand()
		{
			if (!PlatformManager.IsStandalone)
			{
				return;
			}
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltRoll, AudioSourceType.Buttons);
			this.expanded = !this.expanded;
			if (PlayerStatsPresenter.RightArrowExpanded != null)
			{
				PlayerStatsPresenter.RightArrowExpanded();
			}
			this.header.extension.SetActive(this.expanded);
			for (int i = 0; i < this.lines.Length; i++)
			{
				this.lines[i].extension.SetActive(this.expanded);
			}
			if (this.expanded)
			{
				base.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(578f, 0f);
				this.expandButtonLabel.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			}
			else
			{
				base.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(248f, 0f);
				this.expandButtonLabel.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			}
			this.UpdatePlayerClock();
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000E439C File Offset: 0x000E259C
		private void UpdateBottomLines(int sortId = -1)
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			switch (sortId)
			{
			case 0:
				players.Sort((Player y, Player x) => x.GetNumberOfStars().CompareTo(y.GetNumberOfStars()));
				break;
			case 1:
				players.Sort((Player y, Player x) => x.Coins.CompareTo(y.Coins));
				break;
			case 2:
				players.Sort((Player y, Player x) => x.Popularity.CompareTo(y.Popularity));
				break;
			case 3:
				players.Sort((Player y, Player x) => x.Power.CompareTo(y.Power));
				break;
			case 4:
				players.Sort((Player y, Player x) => x.GetCombatCardsCount().CompareTo(y.GetCombatCardsCount()));
				break;
			case 5:
				players.Sort((Player y, Player x) => x.matPlayer.UpgradesDone.CompareTo(y.matPlayer.UpgradesDone));
				break;
			case 6:
				players.Sort((Player y, Player x) => x.matFaction.mechs.Count.CompareTo(y.matFaction.mechs.Count));
				break;
			case 7:
				players.Sort((Player y, Player x) => x.matPlayer.buildings.Count.CompareTo(y.matPlayer.buildings.Count));
				break;
			case 8:
				players.Sort((Player y, Player x) => x.matPlayer.RecruitsEnlisted.CompareTo(y.matPlayer.RecruitsEnlisted));
				break;
			case 9:
				players.Sort((Player y, Player x) => x.ObjectivesDone.CompareTo(y.ObjectivesDone));
				break;
			case 10:
				players.Sort((Player y, Player x) => x.Victories.CompareTo(y.Victories));
				break;
			case 11:
				players.Sort((Player y, Player x) => x.matPlayer.workers.Count.CompareTo(y.matPlayer.workers.Count));
				break;
			case 12:
				players.Sort((Player y, Player x) => x.Resources(false)[ResourceType.oil].CompareTo(y.Resources(false)[ResourceType.oil]));
				break;
			case 13:
				players.Sort((Player y, Player x) => x.Resources(false)[ResourceType.metal].CompareTo(y.Resources(false)[ResourceType.metal]));
				break;
			case 14:
				players.Sort((Player y, Player x) => x.Resources(false)[ResourceType.wood].CompareTo(y.Resources(false)[ResourceType.wood]));
				break;
			case 15:
				players.Sort((Player y, Player x) => x.Resources(false)[ResourceType.food].CompareTo(y.Resources(false)[ResourceType.food]));
				break;
			}
			for (int i = 0; i < this.lines.Length; i++)
			{
				this.lines[i].SetMiniStarsVisibility(false);
				if (this.expandedDown && i < GameController.GameManager.PlayerCount)
				{
					this.lines[i].gameObject.SetActive(true);
					this.lines[i].UpdateLine(players[i], GameController.factionInfo[players[i].matFaction.faction].logo, this.starInactive, true);
					this.lines[i].SetIconsVisibility(false);
				}
				else
				{
					this.lines[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000E4740 File Offset: 0x000E2940
		public void ExpandDown()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltRoll, AudioSourceType.Buttons);
			if (PlayerStatsPresenter.BottomArrowExpanded != null)
			{
				PlayerStatsPresenter.BottomArrowExpanded();
			}
			this.expandedDown = !this.expandedDown;
			if (this.expandDownLabel != null)
			{
				this.expandDownLabel.transform.localEulerAngles = new Vector3(0f, 0f, (float)(this.expandedDown ? 0 : 180));
			}
			this.header.SetLabelVisibility(!this.expandedDown);
			this.header.SetMiniStarsVisibility(!this.expandedDown);
			this.UpdateBottomLines(-1);
			this.UpdatePlayerClock();
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x00040850 File Offset: 0x0003EA50
		public void Sort(int id)
		{
			this.OnClick();
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000E47E8 File Offset: 0x000E29E8
		public void UpdatePlayerClock()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				this.playerClock.timeLeftText1.gameObject.SetActive(!this.expanded || !this.expandedDown);
				this.playerClock.timeLeftText2.gameObject.SetActive(this.expanded && this.expandedDown);
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x00040858 File Offset: 0x0003EA58
		public void OnClick()
		{
			if (!this.expanded)
			{
				this.Expand();
				return;
			}
			if (!this.expandedDown)
			{
				this.ExpandDown();
				return;
			}
			this.Expand();
			this.ExpandDown();
		}

		// Token: 0x04001B67 RID: 7015
		public PlayerStatsLine header;

		// Token: 0x04001B68 RID: 7016
		public PlayerStatsLine[] lines;

		// Token: 0x04001B69 RID: 7017
		public Color starInactive;

		// Token: 0x04001B6A RID: 7018
		public bool expanded;

		// Token: 0x04001B6B RID: 7019
		public bool expandedDown;

		// Token: 0x04001B6C RID: 7020
		public Image expandButtonLabel;

		// Token: 0x04001B6D RID: 7021
		public GameObject expandDownLabel;

		// Token: 0x04001B6E RID: 7022
		public MatPreview matPreview;

		// Token: 0x04001B6F RID: 7023
		public Transform[] trackMarkersPopularity;

		// Token: 0x04001B70 RID: 7024
		public Transform[] trackMarkersPower;

		// Token: 0x04001B71 RID: 7025
		public Transform trumpTrack;

		// Token: 0x04001B72 RID: 7026
		public PlayerClockPresenter playerClock;

		// Token: 0x04001B73 RID: 7027
		private Player player;

		// Token: 0x020004B7 RID: 1207
		// (Invoke) Token: 0x0600265B RID: 9819
		public delegate void BottomArrowExpand();

		// Token: 0x020004B8 RID: 1208
		// (Invoke) Token: 0x0600265F RID: 9823
		public delegate void RightArrowExpand();
	}
}
