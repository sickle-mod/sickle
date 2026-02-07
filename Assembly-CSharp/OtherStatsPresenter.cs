using System;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class OtherStatsPresenter : StatsWindow
{
	// Token: 0x06000237 RID: 567 RVA: 0x0005B254 File Offset: 0x00059454
	protected override void UpdateView()
	{
		base.CalculateValues();
		this.workersBanished.text = this.GetSumWorkersBanished().ToString();
		this.objectiveCompleted.text = this.GetSumObjectiveCompleted().ToString();
		this.distanceTraveled.text = this.GetSumDistanceTravelled().ToString();
		if (this.sumOfAllOthers != null)
		{
			this.sumOfAllOthers.text = (this.GetSumDistanceTravelled() + this.GetSumObjectiveCompleted() + this.GetSumWorkersBanished()).ToString();
		}
		int[] sumBuildings = this.GetSumBuildings();
		this.totalBuildings[0].text = sumBuildings[0].ToString();
		this.totalBuildings[1].text = sumBuildings[1].ToString();
		this.totalBuildings[2].text = sumBuildings[2].ToString();
		this.totalBuildings[3].text = sumBuildings[3].ToString();
		this.sumOfAllBuldings.text = (sumBuildings[0] + sumBuildings[1] + sumBuildings[2] + sumBuildings[3]).ToString();
		int[] sumResources = this.GetSumResources();
		this.totalResources[0].text = sumResources[0].ToString();
		this.totalResources[1].text = sumResources[1].ToString();
		this.totalResources[2].text = sumResources[2].ToString();
		this.totalResources[3].text = sumResources[3].ToString();
		this.sumOfAllResources.text = (sumResources[0] + sumResources[1] + sumResources[2] + sumResources[3]).ToString();
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0005B404 File Offset: 0x00059604
	private void UpdateOthersScreen(PlayerStats playerStats)
	{
		this.workersBanished.text = this.GetSumWorkersBanished().ToString();
		this.objectiveCompleted.text = this.GetSumObjectiveCompleted().ToString();
		this.distanceTraveled.text = this.GetSumDistanceTravelled().ToString();
		this.sumOfAllOthers.text = (this.GetSumDistanceTravelled() + this.GetSumObjectiveCompleted() + this.GetSumWorkersBanished()).ToString();
		int[] sumBuildings = this.GetSumBuildings();
		this.totalBuildings[0].text = sumBuildings[0].ToString();
		this.totalBuildings[1].text = sumBuildings[1].ToString();
		this.totalBuildings[2].text = sumBuildings[2].ToString();
		this.totalBuildings[3].text = sumBuildings[3].ToString();
		this.totalBuildings[4].text = sumBuildings[4].ToString();
		int[] sumResources = this.GetSumResources();
		this.totalResources[0].text = sumResources[0].ToString();
		this.totalResources[1].text = sumResources[1].ToString();
		this.totalResources[2].text = sumResources[2].ToString();
		this.totalResources[3].text = sumResources[3].ToString();
		this.totalResources[4].text = sumResources[4].ToString();
		if (PlatformManager.IsStandalone)
		{
			if (sumResources[5] != 0)
			{
				this.averageResources[0].text = ((float)sumResources[0] / (float)sumResources[5]).ToString("n2");
				this.averageResources[1].text = ((float)sumResources[1] / (float)sumResources[5]).ToString("n2");
				this.averageResources[2].text = ((float)sumResources[2] / (float)sumResources[5]).ToString("n2");
				this.averageResources[3].text = ((float)sumResources[3] / (float)sumResources[5]).ToString("n2");
				return;
			}
			this.averageResources[0].text = "0.0";
			this.averageResources[1].text = "0.0";
			this.averageResources[2].text = "0.0";
			this.averageResources[3].text = "0.0";
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00029458 File Offset: 0x00027658
	private int GetSumDistanceTravelled()
	{
		return this.totalNumberOfTraveledDistance;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00029460 File Offset: 0x00027660
	private int GetSumObjectiveCompleted()
	{
		return this.totalNumberOfCompletedObjectives;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00029468 File Offset: 0x00027668
	private int GetSumWorkersBanished()
	{
		return this.totalNumberOfWorkersBanished;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00029470 File Offset: 0x00027670
	private int[] GetSumBuildings()
	{
		this.totalNumberOfBuildings[4] = this.totalNumberOfBuildings[0] + this.totalNumberOfBuildings[1] + this.totalNumberOfBuildings[2] + this.totalNumberOfBuildings[3];
		return this.totalNumberOfBuildings;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000294A3 File Offset: 0x000276A3
	private int[] GetSumResources()
	{
		this.totalNumberOfResources[4] = this.totalNumberOfResources[0] + this.totalNumberOfResources[1] + this.totalNumberOfResources[2] + this.totalNumberOfResources[3];
		return this.totalNumberOfResources;
	}

	// Token: 0x0600023E RID: 574 RVA: 0x000294D6 File Offset: 0x000276D6
	protected override void ResetValues()
	{
		this.totalNumberOfTraveledDistance = 0;
		this.totalNumberOfCompletedObjectives = 0;
		this.totalNumberOfWorkersBanished = 0;
		this.totalNumberOfBuildings = new int[5];
		this.totalNumberOfResources = new int[6];
		this.numberOfGames = 0;
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0002950C File Offset: 0x0002770C
	protected override void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumTraveledDistanceNormal(faction, playerMat, playerStats);
		this.SumCompletedObjectivesNormal(faction, playerMat, playerStats);
		this.SumBanishedWorkersNormal(faction, playerMat, playerStats);
		this.SumNumberOfGamesNormal(faction, playerMat, playerStats);
		this.SumTotalNumberOfBuildingsNormal(faction, playerMat, playerStats);
		this.SumTotalResourcesNormal(faction, playerMat, playerStats);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00029544 File Offset: 0x00027744
	private void SumTraveledDistanceNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfTraveledDistance += playerStats.PlayerFactionStats[faction][playerMat].TotalDistanceTravelled;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00029562 File Offset: 0x00027762
	private void SumCompletedObjectivesNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfCompletedObjectives += playerStats.PlayerFactionStats[faction][playerMat].TotalObjectivesDone;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00029580 File Offset: 0x00027780
	private void SumBanishedWorkersNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfWorkersBanished += playerStats.PlayerFactionStats[faction][playerMat].CombatWorkersChased;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0005B674 File Offset: 0x00059874
	private void SumTotalNumberOfBuildingsNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfBuildings[0] += playerStats.PlayerFactionStats[faction][playerMat].TotalMills;
		this.totalNumberOfBuildings[1] += playerStats.PlayerFactionStats[faction][playerMat].TotalMines;
		this.totalNumberOfBuildings[2] += playerStats.PlayerFactionStats[faction][playerMat].TotalArmories;
		this.totalNumberOfBuildings[3] += playerStats.PlayerFactionStats[faction][playerMat].TotalMonuments;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0005B700 File Offset: 0x00059900
	private void SumTotalResourcesNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfResources[0] += playerStats.PlayerFactionStats[faction][playerMat].TotalFood;
		this.totalNumberOfResources[1] += playerStats.PlayerFactionStats[faction][playerMat].TotalMetal;
		this.totalNumberOfResources[2] += playerStats.PlayerFactionStats[faction][playerMat].TotalWood;
		this.totalNumberOfResources[3] += playerStats.PlayerFactionStats[faction][playerMat].TotalOil;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0002959E File Offset: 0x0002779E
	private void SumNumberOfGamesNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.numberOfGames += playerStats.PlayerFactionStats[faction][playerMat].GamesAmount;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x000295BC File Offset: 0x000277BC
	protected override void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumTraveledDistanceRanked(faction, playerMat, playerStats);
		this.SumCompletedObjectivesRanked(faction, playerMat, playerStats);
		this.SumBanishedWorkersRanked(faction, playerMat, playerStats);
		this.SumNumberOfGamesRanked(faction, playerMat, playerStats);
		this.SumTotalNumberOfBuildingsRanked(faction, playerMat, playerStats);
		this.SumTotalResourcesRanked(faction, playerMat, playerStats);
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000295F4 File Offset: 0x000277F4
	private void SumTraveledDistanceRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfTraveledDistance += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalDistanceTravelled;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00029612 File Offset: 0x00027812
	private void SumCompletedObjectivesRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfCompletedObjectives += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalObjectivesDone;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00029630 File Offset: 0x00027830
	private void SumBanishedWorkersRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfWorkersBanished += playerStats.PlayerFactionStats[faction][playerMat].RankedCombatWorkersChased;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0005B78C File Offset: 0x0005998C
	private void SumTotalNumberOfBuildingsRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfBuildings[0] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalMills;
		this.totalNumberOfBuildings[1] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalMines;
		this.totalNumberOfBuildings[2] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalArmories;
		this.totalNumberOfBuildings[3] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalMonuments;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0005B818 File Offset: 0x00059A18
	private void SumTotalResourcesRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalNumberOfResources[0] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalFood;
		this.totalNumberOfResources[1] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalMetal;
		this.totalNumberOfResources[2] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalWood;
		this.totalNumberOfResources[3] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalOil;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0002964E File Offset: 0x0002784E
	private void SumNumberOfGamesRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.numberOfGames += playerStats.PlayerFactionStats[faction][playerMat].RankedGamesAmount;
	}

	// Token: 0x040001A1 RID: 417
	[SerializeField]
	private TextMeshProUGUI sumOfAllBuldings;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private TextMeshProUGUI sumOfAllResources;

	// Token: 0x040001A3 RID: 419
	[SerializeField]
	private TextMeshProUGUI sumOfAllOthers;

	// Token: 0x040001A4 RID: 420
	[SerializeField]
	private TextMeshProUGUI[] totalBuildings;

	// Token: 0x040001A5 RID: 421
	[SerializeField]
	private TextMeshProUGUI[] totalResources;

	// Token: 0x040001A6 RID: 422
	[SerializeField]
	private TextMeshProUGUI[] averageResources;

	// Token: 0x040001A7 RID: 423
	[SerializeField]
	private TextMeshProUGUI workersBanished;

	// Token: 0x040001A8 RID: 424
	[SerializeField]
	private TextMeshProUGUI objectiveCompleted;

	// Token: 0x040001A9 RID: 425
	[SerializeField]
	private TextMeshProUGUI distanceTraveled;

	// Token: 0x040001AA RID: 426
	private int totalNumberOfTraveledDistance;

	// Token: 0x040001AB RID: 427
	private int totalNumberOfCompletedObjectives;

	// Token: 0x040001AC RID: 428
	private int totalNumberOfWorkersBanished;

	// Token: 0x040001AD RID: 429
	private int[] totalNumberOfBuildings = new int[4];

	// Token: 0x040001AE RID: 430
	private int[] totalNumberOfResources = new int[4];

	// Token: 0x040001AF RID: 431
	private int numberOfGames;
}
