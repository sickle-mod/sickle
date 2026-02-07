using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class CombatStatsPresenter : StatsWindow
{
	// Token: 0x06000222 RID: 546 RVA: 0x0005AA84 File Offset: 0x00058C84
	protected override void UpdateView()
	{
		base.CalculateValues();
		this.totalBattles.text = this.totalNumberOfCombats.ToString();
		this.averageFights.text = Math.Round(this.GetAverageFightsPerGame(), 0, MidpointRounding.AwayFromZero).ToString();
		this.averagePower.text = Math.Round(this.GetAveragePowerPerFight(), 0, MidpointRounding.AwayFromZero).ToString();
		this.maxPowerUsed.text = this.GetMaxPowerUsed().ToString();
		this.winsValue.text = this.winsAndLoses[0].ToString();
		this.loosesValue.text = this.winsAndLoses[1].ToString();
		this.UpdateCombatPieGraph();
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0005AB44 File Offset: 0x00058D44
	private void UpdateCombatPieGraph()
	{
		this.fightPieGraph.Init();
		float[] sumOfWinsAndLoses = this.GetSumOfWinsAndLoses();
		if (sumOfWinsAndLoses.Max() != 0f)
		{
			this.fightPieGraph.maxNumberSlices = 2;
			this.fightPieGraph.sliceValues.SetList(sumOfWinsAndLoses);
		}
		else
		{
			this.fightPieGraph.sliceValues.SetList(new float[0]);
			this.fightPieGraph.maxNumberSlices = 0;
		}
		this.fightPieGraph.sliceLabels.SetList(new List<string>
		{
			ScriptLocalization.Get("Statistics/Wins"),
			ScriptLocalization.Get("Statistics/Losses")
		});
		this.fightPieGraph.Refresh();
	}

	// Token: 0x06000224 RID: 548 RVA: 0x000292CE File Offset: 0x000274CE
	protected override void ResetValues()
	{
		this.numberOfGames = 0;
		this.totalUsedPower = 0;
		this.totalNumberOfCombats = 0;
		this.maxPowerUsedInCombat = 0;
		this.winsAndLoses = new float[2];
	}

	// Token: 0x06000225 RID: 549 RVA: 0x000292F8 File Offset: 0x000274F8
	protected override void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPowerPerFightNormal(faction, playerMat, playerStats);
		this.SumNumberOfCombatsNormal(faction, playerMat, playerStats);
		this.FindMaxPowerNormal(faction, playerMat, playerStats);
		this.SumWinAndLosesNormal(faction, playerMat, playerStats);
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0005ABF4 File Offset: 0x00058DF4
	private void SumPowerPerFightNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalUsedPower += playerStats.PlayerFactionStats[faction][playerMat].CombatPowerSpent;
		this.totalNumberOfCombats += playerStats.PlayerFactionStats[faction][playerMat].CombatLostAmount + playerStats.PlayerFactionStats[faction][playerMat].CombatWonAmount;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0002931E File Offset: 0x0002751E
	private void SumNumberOfCombatsNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.numberOfGames += playerStats.PlayerFactionStats[faction][playerMat].GamesAmount;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0002933C File Offset: 0x0002753C
	private void FindMaxPowerNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		if (playerStats.PlayerFactionStats[faction][playerMat].CombatMaxPowerUsed > this.maxPowerUsedInCombat)
		{
			this.maxPowerUsedInCombat = playerStats.PlayerFactionStats[faction][playerMat].CombatMaxPowerUsed;
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0005AC4C File Offset: 0x00058E4C
	private void SumWinAndLosesNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.winsAndLoses[0] += (float)playerStats.PlayerFactionStats[faction][playerMat].CombatWonAmount;
		this.winsAndLoses[1] += (float)playerStats.PlayerFactionStats[faction][playerMat].CombatLostAmount;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0002936A File Offset: 0x0002756A
	protected override void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPowerPerFightRanked(faction, playerMat, playerStats);
		this.SumNumberOfCombatRanked(faction, playerMat, playerStats);
		this.FindMaxPowerRanked(faction, playerMat, playerStats);
		this.SumWinAndLosesRanked(faction, playerMat, playerStats);
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0005AC9C File Offset: 0x00058E9C
	private void SumPowerPerFightRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalUsedPower += playerStats.PlayerFactionStats[faction][playerMat].RankedCombatPowerSpent;
		this.totalNumberOfCombats += playerStats.PlayerFactionStats[faction][playerMat].RankedCombatLostAmount + playerStats.PlayerFactionStats[faction][playerMat].RankedCombatWonAmount;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00029390 File Offset: 0x00027590
	private void SumNumberOfCombatRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.numberOfGames += playerStats.PlayerFactionStats[faction][playerMat].RankedGamesAmount;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x000293AE File Offset: 0x000275AE
	private void FindMaxPowerRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		if (playerStats.PlayerFactionStats[faction][playerMat].RankedCombatMaxPowerUsed > this.maxPowerUsedInCombat)
		{
			this.maxPowerUsedInCombat = playerStats.PlayerFactionStats[faction][playerMat].RankedCombatMaxPowerUsed;
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0005ACF4 File Offset: 0x00058EF4
	private void SumWinAndLosesRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.winsAndLoses[0] += (float)playerStats.PlayerFactionStats[faction][playerMat].RankedCombatWonAmount;
		this.winsAndLoses[1] += (float)playerStats.PlayerFactionStats[faction][playerMat].RankedCombatLostAmount;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x000293DC File Offset: 0x000275DC
	private double GetAverageFightsPerGame()
	{
		if (this.numberOfGames == 0)
		{
			return 0.0;
		}
		return (double)this.totalNumberOfCombats / (double)this.numberOfGames;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x000293FF File Offset: 0x000275FF
	private double GetAveragePowerPerFight()
	{
		if (this.totalNumberOfCombats == 0)
		{
			return 0.0;
		}
		return (double)this.totalUsedPower / (double)this.totalNumberOfCombats;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00029422 File Offset: 0x00027622
	private int GetMaxPowerUsed()
	{
		return this.maxPowerUsedInCombat;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0002942A File Offset: 0x0002762A
	private float[] GetSumOfWinsAndLoses()
	{
		return this.winsAndLoses;
	}

	// Token: 0x04000192 RID: 402
	[SerializeField]
	private WMG_Pie_Graph fightPieGraph;

	// Token: 0x04000193 RID: 403
	[SerializeField]
	private TextMeshProUGUI totalBattles;

	// Token: 0x04000194 RID: 404
	[SerializeField]
	private TextMeshProUGUI averageFights;

	// Token: 0x04000195 RID: 405
	[SerializeField]
	private TextMeshProUGUI averagePower;

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private TextMeshProUGUI maxPowerUsed;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private GameObject winsObject;

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private GameObject loosesObject;

	// Token: 0x04000199 RID: 409
	[SerializeField]
	private TextMeshProUGUI winsValue;

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private TextMeshProUGUI loosesValue;

	// Token: 0x0400019B RID: 411
	private int numberOfGames;

	// Token: 0x0400019C RID: 412
	private int totalUsedPower;

	// Token: 0x0400019D RID: 413
	private int totalNumberOfCombats;

	// Token: 0x0400019E RID: 414
	private int maxPowerUsedInCombat;

	// Token: 0x0400019F RID: 415
	private float[] winsAndLoses = new float[2];
}
