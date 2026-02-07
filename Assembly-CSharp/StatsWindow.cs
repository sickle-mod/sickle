using System;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004D RID: 77
public class StatsWindow : MonoBehaviour
{
	// Token: 0x06000287 RID: 647 RVA: 0x0002920A File Offset: 0x0002740A
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0002989F File Offset: 0x00027A9F
	public virtual void Show(PlayerStatsPresenter statsPresenter, PlayerStats playerStats)
	{
		this.SetActive(true);
		this.statsPresenter = statsPresenter;
		this.playerStats = playerStats;
		this.UpdateView();
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000298BC File Offset: 0x00027ABC
	public void Hide()
	{
		this.SetActive(false);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x000298C5 File Offset: 0x00027AC5
	public void RankedFilterChanged()
	{
		this.UpdateView();
	}

	// Token: 0x0600028B RID: 651 RVA: 0x000298C5 File Offset: 0x00027AC5
	public void FactionSelectionChanged()
	{
		this.UpdateView();
	}

	// Token: 0x0600028C RID: 652 RVA: 0x000298C5 File Offset: 0x00027AC5
	public void PlayerMatSelectionChanged()
	{
		this.UpdateView();
	}

	// Token: 0x0600028D RID: 653 RVA: 0x000298CD File Offset: 0x00027ACD
	protected void CalculateValues()
	{
		this.ResetValues();
		this.IterateOverFactionAndMats(new Action<int, int, PlayerStats>(this.CalculateNormalValues), new Action<int, int, PlayerStats>(this.CalculateRankedValues));
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00027EF0 File Offset: 0x000260F0
	protected virtual void UpdateView()
	{
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00027EF0 File Offset: 0x000260F0
	protected virtual void ResetValues()
	{
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00027EF0 File Offset: 0x000260F0
	protected virtual void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00027EF0 File Offset: 0x000260F0
	protected virtual void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0005D220 File Offset: 0x0005B420
	protected void IterateOverFactionAndMats(Action<int, int, PlayerStats> normalFunction, Action<int, int, PlayerStats> rankedFunction)
	{
		Toggle[] factionToggles = this.statsPresenter.GetFactionToggles();
		Toggle[] playerMatsToggles = this.statsPresenter.GetPlayerMatsToggles();
		RankedFilter currentFilter = this.statsPresenter.GetCurrentFilter();
		for (int i = 0; i < 7; i++)
		{
			if (factionToggles[i].isOn || this.statsPresenter.AllFactionTogglesOff())
			{
				for (int j = 0; j < 7; j++)
				{
					if (playerMatsToggles[j].isOn || this.statsPresenter.AllMatsTogglesOff())
					{
						if (currentFilter == RankedFilter.All)
						{
							normalFunction(i, j, this.playerStats);
							rankedFunction(i, j, this.playerStats);
						}
						if (currentFilter == RankedFilter.Ranked)
						{
							rankedFunction(i, j, this.playerStats);
						}
						else if (currentFilter == RankedFilter.Normal)
						{
							normalFunction(i, j, this.playerStats);
						}
					}
				}
			}
		}
	}

	// Token: 0x040001F2 RID: 498
	protected PlayerStatsPresenter statsPresenter;

	// Token: 0x040001F3 RID: 499
	protected PlayerStats playerStats;
}
