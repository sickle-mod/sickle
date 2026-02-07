using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class OverviewStatsPresenter : StatsWindow
{
	// Token: 0x0600024E RID: 590 RVA: 0x0002968C File Offset: 0x0002788C
	protected override void UpdateView()
	{
		base.CalculateValues();
		this.UpdateTextValues();
		this.UpdateEloGraph(this.playerStats);
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0005B8A4 File Offset: 0x00059AA4
	private void UpdateTextValues()
	{
		this.eloPoints.text = this.playerStats.ELO.ToString();
		this.rankingPosition.text = this.playerStats.RankingPosition.ToString();
		this.totalGameTime.text = this.GetTotalTime(this.playerStats);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0005B900 File Offset: 0x00059B00
	private void UpdateEloGraph(PlayerStats playerStats)
	{
		this.eloGraph.Init();
		List<Vector2> list = new List<Vector2>();
		List<string> list2 = new List<string>();
		int num = playerStats.ELOList.Max();
		if (num > 10)
		{
			int num2 = num % 5;
			if (num2 == 0)
			{
				this.eloGraph.yAxis.AxisMaxValue = (float)num;
			}
			else
			{
				this.eloGraph.yAxis.AxisMaxValue = (float)(num + 5 - num2);
			}
			this.eloGraph.yAxis.AxisNumTicks = 6;
		}
		else if (num < 2)
		{
			this.eloGraph.yAxis.AxisMaxValue = 1f;
			this.eloGraph.yAxis.AxisNumTicks = 2;
		}
		else if (num == 7)
		{
			this.eloGraph.yAxis.AxisMaxValue = 8f;
			this.eloGraph.yAxis.AxisNumTicks = 5;
		}
		else if (num % 5 == 0)
		{
			this.eloGraph.yAxis.AxisMaxValue = (float)num;
			this.eloGraph.yAxis.AxisNumTicks = 6;
		}
		else if (num % 4 == 0)
		{
			this.eloGraph.yAxis.AxisMaxValue = (float)num;
			this.eloGraph.yAxis.AxisNumTicks = 5;
		}
		else if (num % 3 == 0)
		{
			this.eloGraph.yAxis.AxisMaxValue = (float)num;
			this.eloGraph.yAxis.AxisNumTicks = 4;
		}
		else if (num % 2 == 0)
		{
			this.eloGraph.yAxis.AxisMaxValue = (float)num;
			this.eloGraph.yAxis.AxisNumTicks = 3;
		}
		int i = 0;
		if (20 < playerStats.ELOList.Count)
		{
			i = playerStats.ELOList.Count - 20;
		}
		int num3 = 0;
		while (i < playerStats.ELOList.Count)
		{
			list.Add(new Vector2((float)(num3 + 1), (float)playerStats.ELOList[i]));
			list2.Add((playerStats.GamesList[i] + 1).ToString());
			num3++;
			i++;
		}
		if (this.eloGraphTitle != null)
		{
			this.eloGraphTitle.text = ScriptLocalization.Get("Statistics/EloPointsProgress").ToUpper();
		}
		this.eloGraph.useGroups = true;
		this.eloGraph.groups.SetList(list2);
		this.eloGraph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
		if (list2.Count > 1)
		{
			this.eloGraph.xAxis.AxisNumTicks = list2.Count;
		}
		else
		{
			this.eloGraph.xAxis.AxisNumTicks = 2;
			this.eloGraph.xAxis.AxisMaxValue = 2f;
		}
		this.eloSeries.seriesName = "ELO";
		this.eloSeries.UseXDistBetweenToSpace = true;
		this.eloSeries.ManuallySetXDistBetween = false;
		this.eloSeries.pointValues.SetList(list);
		this.eloGraph.Refresh();
		this.eloGraph.ManualResize();
		if (this.eloGraph.gameObject.activeInHierarchy)
		{
			this.eloGraph.theTooltip.tooltipLabeler = new WMG_Graph_Tooltip.TooltipLabeler(this.ELOPointsLabeler);
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0005BC14 File Offset: 0x00059E14
	private string ELOPointsLabeler(WMG_Series aSeries, WMG_Node aNode)
	{
		Vector2 nodeValue = aSeries.getNodeValue(aNode);
		float num = Mathf.Pow(10f, (float)aSeries.theGraph.tooltipNumberDecimals);
		int num2 = Mathf.RoundToInt(Mathf.Round(nodeValue.x * num) / num) - 1;
		string text = this.eloGraph.groups[num2];
		string text2 = (Mathf.Round(nodeValue.y * num) / num).ToString();
		string text3 = string.Concat(new string[] { "(", text, ", ", text2, ")" });
		if (aSeries.theGraph.tooltipDisplaySeriesName)
		{
			text3 = aSeries.seriesName + ": " + text3;
		}
		return text3;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x000296A6 File Offset: 0x000278A6
	private string GetTotalTime(PlayerStats playerStats)
	{
		return this.totalPlayTime.ToString();
	}

	// Token: 0x06000253 RID: 595 RVA: 0x000296B9 File Offset: 0x000278B9
	protected override void ResetValues()
	{
		this.totalPlayTime = new TimeSpan(0, 0, 0);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x000296C9 File Offset: 0x000278C9
	protected override void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumTotalPlayTimeNormal(faction, playerMat, playerStats);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0005BCD0 File Offset: 0x00059ED0
	private void SumTotalPlayTimeNormal(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalPlayTime = this.totalPlayTime.Add(new TimeSpan(playerStats.PlayerFactionStats[faction][playerMat].TotalHours, playerStats.PlayerFactionStats[faction][playerMat].TotalMinutes, playerStats.PlayerFactionStats[faction][playerMat].TotalSeconds));
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000296D4 File Offset: 0x000278D4
	protected override void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumTotalPlayTimeRanked(faction, playerMat, playerStats);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0005BD20 File Offset: 0x00059F20
	private void SumTotalPlayTimeRanked(int faction, int playerMat, PlayerStats playerStats)
	{
		this.totalPlayTime = this.totalPlayTime.Add(new TimeSpan(playerStats.PlayerFactionStats[faction][playerMat].RankedTotalHours, playerStats.PlayerFactionStats[faction][playerMat].RankedTotalMinutes, playerStats.PlayerFactionStats[faction][playerMat].RankedTotalSeconds));
	}

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	private TextMeshProUGUI eloGraphTitle;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	private WMG_Axis_Graph eloGraph;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private WMG_Series eloSeries;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private TextMeshProUGUI eloPoints;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	private TextMeshProUGUI rankingPosition;

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private TextMeshProUGUI totalGameTime;

	// Token: 0x040001B6 RID: 438
	private TimeSpan totalPlayTime = new TimeSpan(0, 0, 0);
}
