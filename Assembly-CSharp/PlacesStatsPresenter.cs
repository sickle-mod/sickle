using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class PlacesStatsPresenter : StatsWindow
{
	// Token: 0x06000259 RID: 601 RVA: 0x000296F5 File Offset: 0x000278F5
	protected override void UpdateView()
	{
		base.CalculateValues();
		this.UpdateOwnedPlaces();
		this.UpdatePlacesGraphs();
		if (this.statsPresenter.LocalStats())
		{
			this.UpdateForLocalStats();
		}
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0005BD70 File Offset: 0x00059F70
	private void UpdateOwnedPlaces()
	{
		this.ownedPlace1.text = (this.places2[0] + this.places3[0] + this.places4[0] + this.places5[0] + this.places6[0] + this.places7[0]).ToString();
		this.ownedPlace2.text = (this.places2[1] + this.places3[1] + this.places4[1] + this.places5[1] + this.places6[1] + this.places7[1]).ToString();
		this.ownedPlace3.text = (this.places3[2] + this.places4[2] + this.places5[2] + this.places6[2] + this.places7[2]).ToString();
		this.ownedPlace4.text = (this.places4[3] + this.places5[3] + this.places6[3] + this.places7[3]).ToString();
		this.ownedPlace5.text = (this.places5[4] + this.places6[4] + this.places7[4]).ToString();
		this.ownedPlace6.text = (this.places6[5] + this.places7[5]).ToString();
		this.ownedPlace7.text = this.places7[6].ToString();
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0005BEF0 File Offset: 0x0005A0F0
	private void UpdatePlacesGraphs()
	{
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(2), this.place2);
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(3), this.place3);
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(4), this.place4);
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(5), this.place5);
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(6), this.place6);
		this.UpdatePlacesGraph(this.GetSumOfPlacesPlayers(7), this.place7);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0005BF70 File Offset: 0x0005A170
	private void UpdatePlacesGraph(int[] places, WMG_Axis_Graph placeGraph)
	{
		placeGraph.Init();
		List<WMG_Series> list = new List<WMG_Series>();
		List<string> list2 = new List<string>();
		foreach (object obj in placeGraph.seriesParent.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform.GetComponent<WMG_Series>());
		}
		int num = places.Max();
		placeGraph.yAxis.AxisMinValue = 0f;
		if (num > 10)
		{
			int num2 = num % 5;
			if (num2 == 0)
			{
				placeGraph.yAxis.AxisMaxValue = (float)num;
			}
			else
			{
				placeGraph.yAxis.AxisMaxValue = (float)(num + 5 - num2);
			}
			placeGraph.yAxis.AxisNumTicks = 6;
		}
		else if (num < 2)
		{
			placeGraph.yAxis.AxisNumTicks = 2;
			placeGraph.yAxis.AxisMaxValue = 1f;
		}
		else if (num == 7)
		{
			placeGraph.yAxis.AxisNumTicks = 5;
			placeGraph.yAxis.AxisMaxValue = 8f;
		}
		else if (num % 5 == 0)
		{
			placeGraph.yAxis.AxisNumTicks = 6;
			placeGraph.yAxis.AxisMaxValue = (float)places.Max();
		}
		else if (num % 4 == 0)
		{
			placeGraph.yAxis.AxisNumTicks = 5;
			placeGraph.yAxis.AxisMaxValue = (float)places.Max();
		}
		else if (num % 3 == 0)
		{
			placeGraph.yAxis.AxisNumTicks = 4;
			placeGraph.yAxis.AxisMaxValue = (float)places.Max();
		}
		else if (num % 2 == 0)
		{
			placeGraph.yAxis.AxisNumTicks = 3;
			placeGraph.yAxis.AxisMaxValue = (float)places.Max();
		}
		for (int i = 0; i < places.Length; i++)
		{
			List<Vector2> list3 = new List<Vector2>
			{
				new Vector2(1f, (float)places[i])
			};
			list[i].pointValues.SetList(list3);
			list[i].seriesDataLabeler = new WMG_Series.SeriesDataLabeler(this.CustomSeriesDataLabeler);
		}
		placeGraph.xAxis.AxisMaxValue = 2f;
		placeGraph.xAxis.hideTick = true;
		placeGraph.useGroups = true;
		list2.Add("");
		placeGraph.groups.SetList(list2);
		placeGraph.Refresh();
		base.StartCoroutine(this.UpdatePositionOfLabels(places, list));
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0002971C File Offset: 0x0002791C
	private IEnumerator UpdatePositionOfLabels(int[] places, List<WMG_Series> series)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < places.Length; i++)
		{
			if (series[i].dataLabelsParent.transform.childCount != 0 && series[i].nodeParent.transform.childCount != 0)
			{
				Transform child = series[i].dataLabelsParent.transform.GetChild(0);
				Transform child2 = series[i].nodeParent.transform.GetChild(0);
				Vector2 dataLabelsOffset = series[i].dataLabelsOffset;
				dataLabelsOffset.y = -(child2.GetComponent<RectTransform>().rect.height + 20f);
				series[i].dataLabelsOffset = dataLabelsOffset;
				Vector2 sizeDelta = child.GetComponent<RectTransform>().sizeDelta;
				sizeDelta.y = 15f;
				child.GetComponent<RectTransform>().sizeDelta = sizeDelta;
			}
		}
		yield break;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0005C1C8 File Offset: 0x0005A3C8
	private string CustomSeriesDataLabeler(WMG_Series series, float val, int labelIndex)
	{
		return "#" + (series.theGraph.lineSeries.IndexOf(series.gameObject) + 1).ToString();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0005C200 File Offset: 0x0005A400
	protected override void ResetValues()
	{
		this.places2 = new int[2];
		this.places3 = new int[3];
		this.places4 = new int[4];
		this.places5 = new int[5];
		this.places6 = new int[6];
		this.places7 = new int[7];
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0005C258 File Offset: 0x0005A458
	private void UpdateForLocalStats()
	{
		this.sixthPlaceGroup.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		this.seventhPlaceGroup.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		this.sixPlayersGraphGroup.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		this.sevenPlayersGraphGroup.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00029732 File Offset: 0x00027932
	protected override void CalculateNormalValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPlacesInNormalGame(faction, playerMat, playerStats);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0005C2BC File Offset: 0x0005A4BC
	protected void SumPlacesInNormalGame(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places2);
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places3);
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places4);
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places5);
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places6);
		this.SumPlacesNormal(faction, playerMat, playerStats, ref this.places7);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0005C324 File Offset: 0x0005A524
	private void SumPlacesNormal(int faction, int playerMat, PlayerStats playerStats, ref int[] places)
	{
		for (int i = 0; i < places.Length; i++)
		{
			places[i] += this.GetNumberOfPlacesInNormalGames(places.Length, faction, playerMat, i, playerStats);
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0005C360 File Offset: 0x0005A560
	private int GetNumberOfPlacesInNormalGames(int numberOfPlayers, int faction, int playerMat, int place, PlayerStats playerStats)
	{
		switch (numberOfPlayers)
		{
		case 2:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesTwoPlayersGames[place];
		case 3:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesThreePlayersGames[place];
		case 4:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesFourPlayersGames[place];
		case 5:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesFivePlayersGames[place];
		case 6:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesSixPlayersGames[place];
		case 7:
			return playerStats.PlayerFactionStats[faction][playerMat].PlacesSevenPlayersGames[place];
		default:
			return 0;
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0002973D File Offset: 0x0002793D
	protected override void CalculateRankedValues(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPlacesInRankedGame(faction, playerMat, playerStats);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0005C408 File Offset: 0x0005A608
	protected void SumPlacesInRankedGame(int faction, int playerMat, PlayerStats playerStats)
	{
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places2);
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places3);
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places4);
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places5);
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places6);
		this.SumPlacesRanked(faction, playerMat, playerStats, ref this.places7);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0005C470 File Offset: 0x0005A670
	private void SumPlacesRanked(int faction, int playerMat, PlayerStats playerStats, ref int[] places)
	{
		for (int i = 0; i < places.Length; i++)
		{
			places[i] += this.GetNumberOfPlacesInRankedGames(places.Length, faction, playerMat, i, playerStats);
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0005C4AC File Offset: 0x0005A6AC
	private int GetNumberOfPlacesInRankedGames(int numberOfPlayers, int faction, int playerMat, int place, PlayerStats playerStats)
	{
		switch (numberOfPlayers)
		{
		case 2:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesTwoPlayersGames[place];
		case 3:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesThreePlayersGames[place];
		case 4:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesFourPlayersGames[place];
		case 5:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesFivePlayersGames[place];
		case 6:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesSixPlayersGames[place];
		case 7:
			return playerStats.PlayerFactionStats[faction][playerMat].RankedPlacesSevenPlayersGames[place];
		default:
			return 0;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0005C554 File Offset: 0x0005A754
	private int[] GetSumOfPlacesPlayers(int numberOfPlayers)
	{
		switch (numberOfPlayers)
		{
		case 2:
			return this.places2;
		case 3:
			return this.places3;
		case 4:
			return this.places4;
		case 5:
			return this.places5;
		case 6:
			return this.places6;
		case 7:
			return this.places7;
		default:
			return new int[0];
		}
	}

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	private TextMeshProUGUI ownedPlace1;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	private TextMeshProUGUI ownedPlace2;

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	private TextMeshProUGUI ownedPlace3;

	// Token: 0x040001BA RID: 442
	[SerializeField]
	private TextMeshProUGUI ownedPlace4;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	private TextMeshProUGUI ownedPlace5;

	// Token: 0x040001BC RID: 444
	[SerializeField]
	private TextMeshProUGUI ownedPlace6;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private TextMeshProUGUI ownedPlace7;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private WMG_Axis_Graph place2;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private WMG_Axis_Graph place3;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private WMG_Axis_Graph place4;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private WMG_Axis_Graph place5;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private WMG_Axis_Graph place6;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private WMG_Axis_Graph place7;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private GameObject sixthPlaceGroup;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private GameObject seventhPlaceGroup;

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private GameObject sixPlayersGraphGroup;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private GameObject sevenPlayersGraphGroup;

	// Token: 0x040001C8 RID: 456
	private int[] places2 = new int[2];

	// Token: 0x040001C9 RID: 457
	private int[] places3 = new int[3];

	// Token: 0x040001CA RID: 458
	private int[] places4 = new int[4];

	// Token: 0x040001CB RID: 459
	private int[] places5 = new int[5];

	// Token: 0x040001CC RID: 460
	private int[] places6 = new int[6];

	// Token: 0x040001CD RID: 461
	private int[] places7 = new int[7];
}
