using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class ScoreStatsPresenter : StatsWindow
{
	// Token: 0x06000277 RID: 631 RVA: 0x0005CC88 File Offset: 0x0005AE88
	protected override void UpdateView()
	{
		base.CalculateValues();
		this.averageScore.text = ((int)this.GetAverageScore()).ToString();
		this.highScore.text = this.GetTopScore().ToString();
		this.UpdateStarsGraph(this.playerStats);
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0005CCF4 File Offset: 0x0005AEF4
	private void UpdateStarsGraph(PlayerStats playerStats)
	{
		this.starsGraph.Init();
		List<string> list = new List<string>();
		int[] sumStars = this.GetSumStars(playerStats);
		List<WMG_Series> list2 = new List<WMG_Series>();
		foreach (object obj in this.starsGraph.seriesParent.transform)
		{
			Transform transform = (Transform)obj;
			list2.Add(transform.GetComponent<WMG_Series>());
		}
		int num = sumStars.Max();
		if (num > 10)
		{
			int num2 = num % 5;
			if (num2 == 0)
			{
				this.starsGraph.yAxis.AxisMaxValue = (float)num;
			}
			else
			{
				this.starsGraph.yAxis.AxisMaxValue = (float)(num + 5 - num2);
			}
			this.starsGraph.yAxis.AxisNumTicks = 6;
		}
		else if (num < 2)
		{
			this.starsGraph.yAxis.AxisMaxValue = 1f;
			this.starsGraph.yAxis.AxisNumTicks = 2;
		}
		else if (num == 7)
		{
			this.starsGraph.yAxis.AxisMaxValue = 8f;
			this.starsGraph.yAxis.AxisNumTicks = 5;
		}
		else if (num % 5 == 0)
		{
			this.starsGraph.yAxis.AxisMaxValue = (float)num;
			this.starsGraph.yAxis.AxisNumTicks = 6;
		}
		else if (num % 4 == 0)
		{
			this.starsGraph.yAxis.AxisMaxValue = (float)num;
			this.starsGraph.yAxis.AxisNumTicks = 5;
		}
		else if (num % 3 == 0)
		{
			this.starsGraph.yAxis.AxisMaxValue = (float)num;
			this.starsGraph.yAxis.AxisNumTicks = 4;
		}
		else if (num % 2 == 0)
		{
			this.starsGraph.yAxis.AxisMaxValue = (float)num;
			this.starsGraph.yAxis.AxisNumTicks = 3;
		}
		if (this.scoreGraphTitle != null)
		{
			this.scoreGraphTitle.text = ScriptLocalization.Get("Statistics/GainedStars");
		}
		this.starsGraph.graphTitleString = ScriptLocalization.Get("Statistics/GainedStars").ToUpper();
		this.starsGraph.autoAnimationsEnabled = false;
		this.starsGraph.xAxis.AxisMaxValue = 2f;
		this.starsGraph.xAxis.AxisNumTicks = 2;
		for (int i = 0; i < sumStars.Length; i++)
		{
			List<Vector2> list3 = new List<Vector2>
			{
				new Vector2(1f, (float)sumStars[i])
			};
			list2[i].pointValues.SetList(list3);
			list2[i].UseXDistBetweenToSpace = true;
			list2[i].ManuallySetXDistBetween = false;
		}
		this.starsGraph.useGroups = true;
		list.Add("");
		this.starsGraph.groups.SetList(list);
		this.starsGraph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
		this.starsGraph.Refresh();
		this.SetStarsImagesPositions();
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0005CFF8 File Offset: 0x0005B1F8
	private void SetStarsImagesPositions()
	{
		Transform transform = this.starsGraph.seriesParent.transform;
		this.starsGraph.xAxis.transform.position;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).Find("nodeParent").childCount == 0)
			{
				return;
			}
			Transform child = transform.GetChild(i).Find("nodeParent").GetChild(0);
			Transform transform2 = this.starsImagesParent.GetChild(i).transform;
			Vector2 vector = child.GetComponent<RectTransform>().position;
			transform2.transform.position = vector;
			vector = transform2.GetComponent<RectTransform>().localPosition;
			vector.x += child.GetComponent<RectTransform>().rect.width / 2f;
			vector.y -= transform2.GetComponent<RectTransform>().rect.height / 2f + 10f;
			transform2.GetComponent<RectTransform>().localPosition = vector;
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00029796 File Offset: 0x00027996
	private double GetAverageScore()
	{
		if (this.numberOfAvarageScoreData == 0)
		{
			return 0.0;
		}
		return this.sumOfAvarageScore / (double)this.numberOfAvarageScoreData;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x000297B8 File Offset: 0x000279B8
	private int GetTopScore()
	{
		return this.topScore;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000297C0 File Offset: 0x000279C0
	private int[] GetSumStars(PlayerStats playerStats)
	{
		return this.sumOfStars;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x000297C8 File Offset: 0x000279C8
	protected override void ResetValues()
	{
		this.sumOfAvarageScore = 0.0;
		this.numberOfAvarageScoreData = 0;
		this.topScore = 0;
		this.sumOfStars = new int[9];
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000297F4 File Offset: 0x000279F4
	protected override void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumAverageScoreNormal(faction, playerMat, playerStats);
		this.FindTopScoreNormal(faction, playerMat, playerStats);
		this.SumNumberOfStarsNormal(faction, playerMat, playerStats);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0005D124 File Offset: 0x0005B324
	private void SumAverageScoreNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.sumOfAvarageScore += playerStats.PlayerFactionStats[faction][playerMat].RankedAverageScore;
		if (playerStats.PlayerFactionStats[faction][playerMat].RankedAverageScore > 5E-324)
		{
			this.numberOfAvarageScoreData++;
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00029811 File Offset: 0x00027A11
	private void FindTopScoreNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		if (this.topScore < playerStats.PlayerFactionStats[faction][playerMat].TopScore)
		{
			this.topScore = playerStats.PlayerFactionStats[faction][playerMat].TopScore;
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0005D178 File Offset: 0x0005B378
	private void SumNumberOfStarsNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		for (int i = 0; i < this.sumOfStars.Length; i++)
		{
			if (playerStats.PlayerFactionStats[faction][playerMat].TotalStars.Length != 0)
			{
				this.sumOfStars[i] += playerStats.PlayerFactionStats[faction][playerMat].TotalStars[i];
			}
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0002983F File Offset: 0x00027A3F
	protected override void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumAverageScoreRanked(faction, playerMat, playerStats);
		this.FindTopScoreRanked(faction, playerMat, playerStats);
		this.SumNumberOfStarsRanked(faction, playerMat, playerStats);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0005D124 File Offset: 0x0005B324
	private void SumAverageScoreRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.sumOfAvarageScore += playerStats.PlayerFactionStats[faction][playerMat].RankedAverageScore;
		if (playerStats.PlayerFactionStats[faction][playerMat].RankedAverageScore > 5E-324)
		{
			this.numberOfAvarageScoreData++;
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0002985C File Offset: 0x00027A5C
	private void FindTopScoreRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		if (this.topScore < playerStats.PlayerFactionStats[faction][playerMat].RankedTopScore)
		{
			this.topScore = playerStats.PlayerFactionStats[faction][playerMat].RankedTopScore;
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0005D1CC File Offset: 0x0005B3CC
	private void SumNumberOfStarsRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		for (int i = 0; i < this.sumOfStars.Length; i++)
		{
			if (playerStats.PlayerFactionStats[faction][playerMat].RankedTotalStars.Length != 0)
			{
				this.sumOfStars[i] += playerStats.PlayerFactionStats[faction][playerMat].RankedTotalStars[i];
			}
		}
	}

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	private TextMeshProUGUI scoreGraphTitle;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	private TextMeshProUGUI highScore;

	// Token: 0x040001EB RID: 491
	[SerializeField]
	private TextMeshProUGUI averageScore;

	// Token: 0x040001EC RID: 492
	[SerializeField]
	private WMG_Axis_Graph starsGraph;

	// Token: 0x040001ED RID: 493
	[SerializeField]
	private Transform starsImagesParent;

	// Token: 0x040001EE RID: 494
	private double sumOfAvarageScore;

	// Token: 0x040001EF RID: 495
	private int numberOfAvarageScoreData;

	// Token: 0x040001F0 RID: 496
	private int topScore;

	// Token: 0x040001F1 RID: 497
	private int[] sumOfStars = new int[9];
}
