using System;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.UI;
using Scythe.Utilities;
using TMPro;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class ActionLogInterpreter : SingletonMono<ActionLogInterpreter>
{
	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0002B977 File Offset: 0x00029B77
	public static bool IsSupported
	{
		get
		{
			return SingletonMono<ActionLogInterpreter>.Instance != null;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0002B984 File Offset: 0x00029B84
	private string Gained
	{
		get
		{
			return ScriptLocalization.Get("GameScene/Gained") + ":" + Environment.NewLine;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0002B99F File Offset: 0x00029B9F
	private string Paid
	{
		get
		{
			return ScriptLocalization.Get("GameScene/Paid") + ":" + Environment.NewLine;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0002B9BA File Offset: 0x00029BBA
	private string Nothing
	{
		get
		{
			return ScriptLocalization.Get("GameScene/Nothing");
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0006BCFC File Offset: 0x00069EFC
	public void InterpretLog(LogInfo log, GameObject singleLog)
	{
		if (singleLog.transform.childCount == 1)
		{
			this.HideInterpretation(singleLog);
		}
		if (this.hint != null)
		{
			this.hint.SetActive(false);
		}
		this.hint = singleLog.transform.GetChild(1).gameObject;
		this.hint.transform.SetParent(base.transform);
		this.hint.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.GetFactionAndActionName(log);
		this.hint.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = this.factionColors[(int)log.PlayerAssigned];
		string[] array = this.ReportParser(log, false);
		string text = this.GetPayInfo(log) + array[0] + ((array[1].Length != 0) ? Environment.NewLine : "") + array[1];
		this.hint.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
		this.hint.SetActive(true);
		if (PlatformManager.IsMobile)
		{
			this.hintRectTransform = this.hint.GetComponent<RectTransform>();
			this.isFullyVisible = global::RendererExtensions.IsFullyVisibleFrom(this.hintRectTransform, this.parentCanvas.worldCamera);
			if (!this.isFullyVisible)
			{
				this.hintHeightOffset = global::RendererExtensions.ProtrusionHeight(this.hintRectTransform, this.parentCanvas.worldCamera);
				this.hint.transform.localPosition = new Vector3(this.hint.transform.localPosition.x, this.hint.transform.localPosition.y + this.hintHeightOffset, this.hint.transform.localPosition.z);
			}
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0006BEC4 File Offset: 0x0006A0C4
	public void HideInterpretation(GameObject singleLog)
	{
		this.hint.transform.SetParent(singleLog.transform);
		if (PlatformManager.IsMobile)
		{
			this.hint.transform.localPosition = new Vector3(-101f, 12f, 0f);
		}
		else
		{
			this.hint.transform.localPosition = new Vector3(80f, 12f, 0f);
		}
		this.hint.SetActive(false);
		this.HideOutlineOnUnits();
		this.HideOutlineOnBuilding();
		this.HideMarkers();
		this.HidePath();
		this.isFullyVisible = true;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0006BF64 File Offset: 0x0006A164
	private string[] ReportParser(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		switch (logInfo.Type)
		{
		case LogInfoType.GainCoin:
		case LogInfoType.GainPopularity:
		case LogInfoType.GainPower:
		case LogInfoType.GainCombatCard:
			array = this.GainNonboardResourceReport(logInfo, additionalGain);
			break;
		case LogInfoType.Move:
			array = this.MoveReport(logInfo);
			break;
		case LogInfoType.MoveCoins:
			array = this.GainNonboardResourceReport(logInfo, additionalGain);
			break;
		case LogInfoType.TradeResources:
			array = this.TradeResourcesReport(logInfo, additionalGain);
			break;
		case LogInfoType.TradePopularity:
			array = this.GainNonboardResourceReport(logInfo, additionalGain);
			break;
		case LogInfoType.BolsterPower:
			array = this.GainNonboardResourceReport(logInfo, additionalGain);
			break;
		case LogInfoType.BolsterCombatCard:
			array = this.GainNonboardResourceReport(logInfo, additionalGain);
			break;
		case LogInfoType.Produce:
			array = this.ProduceReport(logInfo, false);
			break;
		case LogInfoType.Upgrade:
			array = this.UpgradeReport(logInfo, additionalGain);
			break;
		case LogInfoType.Deploy:
			array = this.DeployReport(logInfo, additionalGain);
			break;
		case LogInfoType.Build:
			array = this.BuildReport(logInfo, additionalGain);
			break;
		case LogInfoType.Enlist:
			array = this.EnlistReport(logInfo, additionalGain);
			break;
		case LogInfoType.ForceWorkersToRetreat:
			array = this.ForceWorkersToRetreatReport(logInfo);
			break;
		case LogInfoType.Combat:
			array = this.CombatReport(logInfo);
			break;
		case LogInfoType.SpyPlayer:
			array = this.SpyPlayerReport(logInfo);
			break;
		case LogInfoType.FactoryCardGain:
			array = this.FactoryCardReport(logInfo);
			break;
		case LogInfoType.GainStar:
			array = this.GainedStarReport(logInfo);
			break;
		case LogInfoType.FactoryTopAction:
			array = this.FactoryActionReport(logInfo);
			break;
		case LogInfoType.GainWorker:
			array = this.GainWorkerReport(logInfo, additionalGain);
			break;
		case LogInfoType.TokenAction:
			array = this.TokenActionReport(logInfo);
			break;
		case LogInfoType.PassCoins:
			array = this.PassCoinsActionReport(logInfo);
			break;
		}
		return array;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0006C124 File Offset: 0x0006A324
	public void Clear()
	{
		if (this.unitsWithOutline.Count != 0)
		{
			foreach (Unit unit in this.unitsWithOutline)
			{
				if (GameController.GetUnitPresenter(unit) != null)
				{
					GameController.GetUnitPresenter(unit).ForceToHideMovesHighlight();
				}
			}
		}
		this.unitsWithOutline.Clear();
		if (this.hexesHighlight.Count != 0)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesHighlight)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.Move, 0f, false);
			}
		}
		this.hexesHighlight.Clear();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002B9C6 File Offset: 0x00029BC6
	public ActionLogPresenter GetActionLogPresenter()
	{
		return base.transform.parent.GetComponentInChildren<ActionLogPresenter>();
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002B9D8 File Offset: 0x00029BD8
	private string GetTMPSprite(string name)
	{
		return "<sprite name=\"" + name + "\">";
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002B9EA File Offset: 0x00029BEA
	private string GetTMPSprite(string spritesheet, string name)
	{
		return string.Concat(new string[] { "<sprite=\"", spritesheet, "\" name=\"", name, "\">" });
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002BA17 File Offset: 0x00029C17
	private string GetFactionName(Faction faction)
	{
		return ScriptLocalization.Get("FactionMat/" + faction.ToString());
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0006C200 File Offset: 0x0006A400
	private void ShowPath(List<GameHex> hexPositions)
	{
		if (hexPositions.Count < 2)
		{
			this.pathLine.enabled = false;
			return;
		}
		List<Vector3> list = new List<Vector3>();
		foreach (GameHex gameHex in hexPositions)
		{
			list.Add(HexCoordinates.HexToWorld3D(this.GetGameHexPresenter(gameHex.posX, gameHex.posY).position) + 0.6f * Vector3.up);
		}
		list.Reverse();
		this.pathLine.enabled = true;
		this.pathLine.positionCount = list.Count;
		this.pathLine.SetPositions(list.ToArray());
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002BA35 File Offset: 0x00029C35
	private void HidePath()
	{
		this.pathLine.enabled = false;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0006C2D0 File Offset: 0x0006A4D0
	private static string FirstToUpper(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		return char.ToUpper(s[0]).ToString() + s.Substring(1);
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002BA43 File Offset: 0x00029C43
	private Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(int posX, int posY)
	{
		return GameController.Instance.gameBoardPresenter.GetGameHexPresenter(posX, posY);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0006C30C File Offset: 0x0006A50C
	private string GetPlayerMatGainActionName(GainType type)
	{
		switch (type)
		{
		case GainType.Coin:
		case GainType.Move:
			return ActionLogInterpreter.FirstToUpper(ScriptLocalization.Get("PlayerMat/Move").ToLower());
		case GainType.Popularity:
		case GainType.AnyResource:
			return ScriptLocalization.Get("PlayerMat/Trade");
		case GainType.Power:
			return ScriptLocalization.Get("PlayerMat/BolsterPower");
		case GainType.CombatCard:
			return ScriptLocalization.Get("PlayerMat/BolsterAmmo");
		case GainType.Produce:
			return ScriptLocalization.Get("PlayerMat/Produce");
		}
		return "";
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0006C388 File Offset: 0x0006A588
	private string GetPlayerMatPayActionName(ResourceType type)
	{
		switch (type)
		{
		case ResourceType.oil:
			return ScriptLocalization.Get("PlayerMat/Upgrade");
		case ResourceType.metal:
			return ScriptLocalization.Get("PlayerMat/Deploy");
		case ResourceType.food:
			return ScriptLocalization.Get("PlayerMat/Enlist");
		case ResourceType.wood:
			return ScriptLocalization.Get("PlayerMat/Build");
		default:
			return "";
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002BA56 File Offset: 0x00029C56
	private string GetMechAbilityName(Faction player, int mechAbility)
	{
		return ScriptLocalization.Get(string.Concat(new object[]
		{
			"FactionMat/",
			player.ToString(),
			"MechAbilityTitle",
			mechAbility + 1
		}));
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002BA93 File Offset: 0x00029C93
	private string GetBuildingName(BuildingType building)
	{
		return ScriptLocalization.Get("PlayerMat/" + building.ToString());
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002BAB1 File Offset: 0x00029CB1
	private string GetDownActionName(DownActionType action)
	{
		return ScriptLocalization.Get("PlayerMat/" + action.ToString());
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0006C3E0 File Offset: 0x0006A5E0
	private UnitPresenter GetUnitPresenter(Unit unit)
	{
		return GameController.GetUnitPresenter(GameController.GameManager.GetPlayerByFaction(unit.Owner.matFaction.faction).GetAllUnits().Find((Unit x) => x.Id == unit.Id && x.UnitType == unit.UnitType));
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0002BACF File Offset: 0x00029CCF
	private string GetFactionAndActionName(LogInfo logInfo)
	{
		return this.GetTMPSprite(logInfo.PlayerAssigned.ToString() + "Logo") + " " + ActionLogInterpreter.GetActionName(logInfo);
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0006C434 File Offset: 0x0006A634
	public static string GetActionName(LogInfo logInfo)
	{
		if (logInfo.IsEncounter)
		{
			return ScriptLocalization.Get("Encounters/Encounter");
		}
		if (logInfo.ActionPlacement == ActionPositionType.BuildingBonus)
		{
			return ScriptLocalization.Get("GameScene/StructureBonus");
		}
		if (logInfo.ActionPlacement == ActionPositionType.OngoingRecruitBonus)
		{
			return ScriptLocalization.Get("GameScene/RecruitBonus");
		}
		if (logInfo.ActionPlacement == ActionPositionType.Combat)
		{
			return ScriptLocalization.Get("GameScene/CombatBonus");
		}
		switch (logInfo.Type)
		{
		case LogInfoType.GainCoin:
		case LogInfoType.MoveCoins:
			return ScriptLocalization.Get("PlayerMat/EarnCash");
		case LogInfoType.GainPopularity:
		case LogInfoType.TradePopularity:
			return ScriptLocalization.Get("PlayerMat/GainPopularity");
		case LogInfoType.GainPower:
		case LogInfoType.BolsterPower:
			return ScriptLocalization.Get("PlayerMat/BolsterPower");
		case LogInfoType.GainCombatCard:
		case LogInfoType.BolsterCombatCard:
			return ScriptLocalization.Get("PlayerMat/BolsterAmmo");
		case LogInfoType.Move:
			return ScriptLocalization.Get("PlayerMat/Move");
		case LogInfoType.TradeResources:
			return ScriptLocalization.Get("PlayerMat/Trade");
		case LogInfoType.Produce:
			return ScriptLocalization.Get("PlayerMat/Produce");
		case LogInfoType.Upgrade:
			return ScriptLocalization.Get("PlayerMat/Upgrade");
		case LogInfoType.Deploy:
			return ScriptLocalization.Get("PlayerMat/Deploy");
		case LogInfoType.Build:
			return ScriptLocalization.Get("PlayerMat/Build");
		case LogInfoType.Enlist:
			return ScriptLocalization.Get("PlayerMat/Enlist");
		case LogInfoType.ForceWorkersToRetreat:
			return ScriptLocalization.Get("PlayerMat/Retreat");
		case LogInfoType.Combat:
			return ScriptLocalization.Get("Statistics/Combat");
		case LogInfoType.FactoryCardGain:
			return ScriptLocalization.Get("GameScene/FactoryCard");
		case LogInfoType.GainStar:
			return ScriptLocalization.Get("GameScene/Star");
		case LogInfoType.FactoryTopAction:
			return ScriptLocalization.Get("PlayerMat/FactoryAction");
		case LogInfoType.TokenAction:
			return ScriptLocalization.Get("GameScene/Token");
		case LogInfoType.PassCoins:
			return ScriptLocalization.Get("GameScene/PassCoinsTitle");
		}
		return logInfo.Type.ToString();
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0006C610 File Offset: 0x0006A810
	private string GetPayInfo(LogInfo logInfo)
	{
		string text = "";
		if (logInfo.PayLogInfos != null && logInfo.PayLogInfos.Count > 0)
		{
			text = this.Paid;
			int num = 0;
			foreach (LogInfo logInfo2 in logInfo.PayLogInfos)
			{
				if (logInfo2 is PayNonboardResourceLogInfo)
				{
					if (num == this.gainPerLine)
					{
						text += Environment.NewLine;
						num = 0;
					}
					PayNonboardResourceLogInfo payNonboardResourceLogInfo = logInfo2 as PayNonboardResourceLogInfo;
					text = string.Concat(new object[]
					{
						text,
						this.GetTMPSprite(payNonboardResourceLogInfo.Resource.ToString()),
						" ",
						payNonboardResourceLogInfo.Amount,
						" "
					});
					num++;
				}
				else if (logInfo2 is PayResourceLogInfo)
				{
					PayResourceLogInfo payResourceLogInfo = logInfo2 as PayResourceLogInfo;
					foreach (ResourceType resourceType in payResourceLogInfo.Resources.Keys)
					{
						if (num == this.gainPerLine)
						{
							text += Environment.NewLine;
							num = 0;
						}
						text = string.Concat(new object[]
						{
							text,
							this.GetTMPSprite(ActionLogInterpreter.FirstToUpper(resourceType.ToString())),
							" ",
							payResourceLogInfo.Resources[resourceType],
							" "
						});
						num++;
					}
				}
			}
			text += Environment.NewLine;
		}
		return text;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0006C7F0 File Offset: 0x0006A9F0
	private string[] GainNonboardResourceReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		GainNonboardResourceLogInfo gainNonboardResourceLogInfo = logInfo as GainNonboardResourceLogInfo;
		string text = "";
		if (gainNonboardResourceLogInfo.Amount != 0)
		{
			text = ((!additionalGain) ? this.Gained : "");
			text = string.Concat(new object[]
			{
				text,
				this.GetTMPSprite(gainNonboardResourceLogInfo.Gained.ToString()),
				" ",
				gainNonboardResourceLogInfo.Amount,
				" "
			});
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			if (additionalGain)
			{
				return array;
			}
			text = this.Gained + this.Nothing;
		}
		array[0] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, gainNonboardResourceLogInfo.Amount == 0);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0006C8DC File Offset: 0x0006AADC
	private string[] MoveReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		HexUnitResourceLogInfo hexUnitResourceLogInfo = logInfo as HexUnitResourceLogInfo;
		if (hexUnitResourceLogInfo.Units.Count != 0)
		{
			this.ShowPath(hexUnitResourceLogInfo.Hexes);
			this.ShowOutlineOnUnits(hexUnitResourceLogInfo.Units);
			this.ShowMarkers(hexUnitResourceLogInfo.Hexes);
		}
		return array;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0006C938 File Offset: 0x0006AB38
	private string[] TradeResourcesReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		HexUnitResourceLogInfo hexUnitResourceLogInfo = logInfo as HexUnitResourceLogInfo;
		Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
		for (int i = 0; i < hexUnitResourceLogInfo.Hexes.Count; i++)
		{
			foreach (ResourceType resourceType in hexUnitResourceLogInfo.Resources[i].Keys)
			{
				if (!dictionary.ContainsKey(resourceType))
				{
					dictionary[resourceType] = hexUnitResourceLogInfo.Resources[i][resourceType];
				}
				else
				{
					Dictionary<ResourceType, int> dictionary2 = dictionary;
					ResourceType resourceType2 = resourceType;
					dictionary2[resourceType2] += hexUnitResourceLogInfo.Resources[i][resourceType];
				}
			}
		}
		string text = "";
		string text2 = "";
		int num = (additionalGain ? 1 : 0);
		if (hexUnitResourceLogInfo.Hexes.Count != 0)
		{
			foreach (ResourceType resourceType3 in dictionary.Keys)
			{
				if (text2.Length != 0)
				{
					if (num == this.gainPerLine)
					{
						text2 += Environment.NewLine;
						num = 0;
					}
					else
					{
						text2 += " ";
					}
				}
				text2 = string.Concat(new object[]
				{
					text2,
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper(resourceType3.ToString())),
					" ",
					dictionary[resourceType3]
				});
				num++;
			}
			this.ShowMarkers(hexUnitResourceLogInfo.Hexes);
			if (!additionalGain && text2.Length != 0)
			{
				text = this.Gained;
			}
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			if (additionalGain)
			{
				return array;
			}
			text = this.Gained;
			text2 = this.Nothing;
		}
		array[0] = text + text2;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, false);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0006CB8C File Offset: 0x0006AD8C
	private string[] ProduceReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		ProductionLogInfo productionLogInfo = logInfo as ProductionLogInfo;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		string text = "";
		string text2 = "";
		int num6 = 0;
		if (productionLogInfo.Hexes.Count > 0)
		{
			foreach (GameHex gameHex in productionLogInfo.Hexes.Keys)
			{
				HexType hexType = gameHex.hexType;
				switch (hexType)
				{
				case HexType.mountain:
					num4 += productionLogInfo.Hexes[gameHex];
					break;
				case HexType.forest:
					num2 += productionLogInfo.Hexes[gameHex];
					break;
				case (HexType)3:
					break;
				case HexType.farm:
					num3 += productionLogInfo.Hexes[gameHex];
					break;
				default:
					if (hexType != HexType.tundra)
					{
						if (hexType == HexType.village)
						{
							num5 += productionLogInfo.Hexes[gameHex];
						}
					}
					else
					{
						num += productionLogInfo.Hexes[gameHex];
					}
					break;
				}
			}
			if (num != 0)
			{
				text2 = string.Concat(new object[]
				{
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Oil")),
					" ",
					num,
					" "
				});
				num6++;
			}
			if (num4 != 0)
			{
				text2 = string.Concat(new object[]
				{
					text2,
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Metal")),
					" ",
					num4,
					" "
				});
				num6++;
				if (num6 == this.gainPerLine)
				{
					text2 += Environment.NewLine;
					num6 = 0;
				}
			}
			if (num3 != 0)
			{
				text2 = string.Concat(new object[]
				{
					text2,
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Food")),
					" ",
					num3,
					" "
				});
				num6++;
				if (num6 == this.gainPerLine)
				{
					text2 += Environment.NewLine;
					num6 = 0;
				}
			}
			if (num2 != 0)
			{
				text2 = string.Concat(new object[]
				{
					text2,
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Wood")),
					" ",
					num2,
					" "
				});
				num6++;
				if (num6 == this.gainPerLine)
				{
					text2 += Environment.NewLine;
					num6 = 0;
				}
			}
			if (num5 != 0)
			{
				text2 = string.Concat(new object[]
				{
					text2,
					this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Worker")),
					" ",
					num5,
					" "
				});
				num6++;
				if (num6 == this.gainPerLine)
				{
					text2 += Environment.NewLine;
					num6 = 0;
				}
			}
			if (!additionalGain && text2.Length != 0)
			{
				text = this.Gained;
			}
			this.ShowMarkers(new List<GameHex>(productionLogInfo.Hexes.Keys));
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			if (additionalGain)
			{
				return array;
			}
			text2 += this.Nothing;
		}
		array[0] = text + text2;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, text2.Length == 0);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0006CF1C File Offset: 0x0006B11C
	private string[] UpgradeReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		UpgradeLogInfo upgradeLogInfo = logInfo as UpgradeLogInfo;
		string text = "";
		string text2 = "";
		if (upgradeLogInfo.DownAction != PayType.Coin)
		{
			text = ScriptLocalization.Get("PlayerMat/Upgrade") + ":" + Environment.NewLine;
			text = string.Concat(new string[]
			{
				text,
				"<size=60%>",
				this.GetTMPSprite("UpgradeIncrease"),
				"</size> ",
				this.GetPlayerMatGainActionName(upgradeLogInfo.TopAction),
				Environment.NewLine
			});
			text = string.Concat(new string[]
			{
				text,
				"<size=60%>",
				this.GetTMPSprite("UpgradeDecrease"),
				"</size> ",
				this.GetPlayerMatPayActionName(upgradeLogInfo.Resource),
				Environment.NewLine
			});
			if (!additionalGain)
			{
				text2 += this.Gained;
			}
			text2 = text2 + " " + this.GetTMPSprite("Upgrade") + " 1 ";
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			text2 += ((!additionalGain) ? this.Gained : (this.Nothing ?? ""));
		}
		array[0] = text2;
		array[1] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, upgradeLogInfo.DownAction == PayType.Coin);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0006D098 File Offset: 0x0006B298
	private string[] DeployReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		DeployLogInfo deployLogInfo = logInfo as DeployLogInfo;
		string text = "";
		string text2 = "";
		string text3 = "";
		if (deployLogInfo.DeployedMech != null)
		{
			this.ShowOutlineOnUnits(new List<Unit> { deployLogInfo.DeployedMech });
			this.ShowMarkers(new List<GameHex> { deployLogInfo.Position });
			text = ScriptLocalization.Get("GameScene/Unlocked") + ":" + Environment.NewLine;
			Player playerByFaction = GameController.GameManager.GetPlayerByFaction(deployLogInfo.PlayerAssigned);
			if (playerByFaction.matFaction.SkillUnlocked[deployLogInfo.MechBonus])
			{
				if (deployLogInfo.MechBonus == 0)
				{
					text2 = this.GetTMPSprite("TextSymbols3", deployLogInfo.PlayerAssigned.ToString() + "Riverwalk");
				}
				else if (deployLogInfo.MechBonus == 3)
				{
					text2 = this.GetTMPSprite("TextSymbols3", "Speed");
				}
				else
				{
					text2 = this.GetTMPSprite("TextSymbols3", playerByFaction.matFaction.abilities[deployLogInfo.MechBonus].ToString());
				}
				text2 = text2 + " " + this.GetMechAbilityName(deployLogInfo.PlayerAssigned, deployLogInfo.MechBonus) + Environment.NewLine;
				text3 = ((!additionalGain) ? this.Gained : "") + " " + this.GetTMPSprite("Mech") + " 1 ";
			}
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			text3 = text3 + this.Gained + this.Nothing;
		}
		array[0] = text3;
		array[1] = text + text2;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, deployLogInfo.DeployedMech == null);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0006D288 File Offset: 0x0006B488
	private string[] BuildReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		BuildLogInfo buildLogInfo = logInfo as BuildLogInfo;
		string text = "";
		string text2 = "";
		string text3 = "";
		if (buildLogInfo.Position != null)
		{
			this.ShowMarkers(new List<GameHex> { buildLogInfo.Position });
			this.ShowOutlineOnBuilding(GameController.Instance.GetGameHexPresenter(buildLogInfo.Position).building.GetComponent<BuildingPresenter>());
			text = ScriptLocalization.Get("GameScene/Placed") + ":" + Environment.NewLine;
			switch (buildLogInfo.PlacedBuilding.buildingType)
			{
			case BuildingType.Mine:
				text2 = this.GetTMPSprite("TextSymbols3", "Mine");
				break;
			case BuildingType.Monument:
				text2 = this.GetTMPSprite("Popularity");
				break;
			case BuildingType.Armory:
				text2 = this.GetTMPSprite("Power");
				break;
			case BuildingType.Mill:
				text2 = this.GetTMPSprite("textSymbols2", "textSymbols2_11");
				break;
			}
			text2 = text2 + " " + this.GetBuildingName(buildLogInfo.PlacedBuilding.buildingType) + Environment.NewLine;
			text3 = string.Concat(new string[]
			{
				text3,
				(!additionalGain) ? this.Gained : "",
				" ",
				this.GetTMPSprite("Building"),
				" 1 "
			});
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			text3 = text3 + ((!additionalGain) ? this.Gained : "") + this.Nothing;
		}
		array[0] = text3;
		array[1] = text + text2;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, buildLogInfo.PlacedBuilding == null);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0006D45C File Offset: 0x0006B65C
	private string[] EnlistReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		EnlistLogInfo enlistLogInfo = logInfo as EnlistLogInfo;
		string text = "";
		string text2 = "";
		if (enlistLogInfo.TypeOfDownAction != DownActionType.Factory)
		{
			text = ScriptLocalization.Get("GameScene/OngoingBonusFrom") + ":" + Environment.NewLine;
			switch (enlistLogInfo.TypeOfDownAction)
			{
			case DownActionType.Upgrade:
				text += this.GetTMPSprite("Upgrade");
				break;
			case DownActionType.Deploy:
				text += this.GetTMPSprite("Mech");
				break;
			case DownActionType.Build:
				text += this.GetTMPSprite("Building");
				break;
			case DownActionType.Enlist:
				text += this.GetTMPSprite("Recruit");
				break;
			}
			text = text + " " + this.GetDownActionName(enlistLogInfo.TypeOfDownAction) + Environment.NewLine;
			text2 = string.Concat(new string[]
			{
				(!additionalGain) ? this.Gained : "",
				" " + this.GetTMPSprite("Recruit"),
				" 1 ",
				this.GetTMPSprite(enlistLogInfo.OneTimeBonus.ToString()),
				" 2 ",
				Environment.NewLine
			});
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			text2 = text2 + ((!additionalGain) ? this.Gained : "") + this.Nothing;
		}
		array[0] = text2;
		array[1] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, enlistLogInfo.TypeOfDownAction == DownActionType.Factory);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0006D614 File Offset: 0x0006B814
	private string[] ForceWorkersToRetreatReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		CombatLogInfo combatLogInfo = logInfo as CombatLogInfo;
		this.ShowOutlineOnUnits(combatLogInfo.Units);
		this.ShowMarkers(new List<GameHex> { combatLogInfo.Battlefield });
		string text = "";
		int num = 0;
		for (int i = 0; i < combatLogInfo.Units.Count; i++)
		{
			if (combatLogInfo.Units[i].Owner.matFaction.faction != combatLogInfo.PlayerAssigned)
			{
				num++;
			}
		}
		string text2 = string.Concat(new object[]
		{
			this.GetTMPSprite("TMPFactionsSpritesheet", combatLogInfo.Defeated.matFaction.faction.ToString() + "Logo"),
			" ",
			ScriptLocalization.Get("GameScene/Withdrawn"),
			" ",
			this.GetTMPSprite("Worker"),
			" ",
			num
		});
		if (combatLogInfo.LostPopularity > 0)
		{
			text = string.Concat(new object[]
			{
				Environment.NewLine,
				this.GetTMPSprite("TMPFactionsSpritesheet", combatLogInfo.Winner.matFaction.faction.ToString() + "Logo"),
				" ",
				ScriptLocalization.Get("GameScene/Lost"),
				" ",
				this.GetTMPSprite("Popularity"),
				" ",
				combatLogInfo.LostPopularity
			});
		}
		array[0] = text2 + text;
		return array;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0006D7D0 File Offset: 0x0006B9D0
	private string[] SpyPlayerReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		SneakPeakLogInfo sneakPeakLogInfo = logInfo as SneakPeakLogInfo;
		string text = "";
		if (sneakPeakLogInfo.PlayerAssigned != sneakPeakLogInfo.SpiedFaction)
		{
			text = string.Concat(new string[]
			{
				this.GetTMPSprite("TMPFactionsSpritesheet", logInfo.PlayerAssigned.ToString() + "Logo"),
				" ",
				this.GetFactionName(sneakPeakLogInfo.PlayerAssigned),
				" ",
				ScriptLocalization.Get("GameScene/Spied"),
				" ",
				this.GetTMPSprite("TMPFactionsSpritesheet", sneakPeakLogInfo.SpiedFaction.ToString() + "Logo"),
				" ",
				this.GetFactionName(sneakPeakLogInfo.SpiedFaction),
				".\n"
			});
		}
		array[0] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, true);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0006D8F0 File Offset: 0x0006BAF0
	private string[] FactoryCardReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		FactoryLogInfo factoryLogInfo = logInfo as FactoryLogInfo;
		this.ShowOutlineOnUnits(new List<Unit> { factoryLogInfo.Character });
		this.ShowMarkers(new List<GameHex> { GameController.GameManager.gameBoard.factory });
		string tmpsprite = this.GetTMPSprite("TextSymbols3", "FactoryCard");
		string text = ScriptLocalization.Get("GameScene/FactoryCardNumber");
		string text2 = " ";
		if (text.EndsWith("#"))
		{
			text2 = string.Empty;
		}
		array[0] = string.Concat(new object[]
		{
			tmpsprite,
			" ",
			text,
			text2,
			factoryLogInfo.GainedFactoryCard.CardId
		});
		return array;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0006D9C0 File Offset: 0x0006BBC0
	private string[] GainedStarReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		StarLogInfo starLogInfo = logInfo as StarLogInfo;
		string text = "";
		string text2 = "";
		switch (starLogInfo.GainedStar)
		{
		case StarType.Upgrades:
			text = this.GetTMPSprite("Upgrade");
			break;
		case StarType.Mechs:
			text = this.GetTMPSprite("Mech");
			break;
		case StarType.Structures:
			text = this.GetTMPSprite("Building");
			break;
		case StarType.Recruits:
			text = this.GetTMPSprite("Recruit");
			break;
		case StarType.Workers:
			text = this.GetTMPSprite("Worker");
			break;
		case StarType.Objective:
			text = this.GetTMPSprite("textSymbols2", "Objective");
			break;
		case StarType.Combat:
			text = this.GetTMPSprite("TextSymbols3", "CombatVictory");
			break;
		case StarType.Popularity:
			text = this.GetTMPSprite("Popularity");
			break;
		case StarType.Power:
			text = this.GetTMPSprite("Power");
			break;
		}
		text = string.Concat(new string[]
		{
			ScriptLocalization.Get("GameScene/Star"),
			":",
			Environment.NewLine,
			text,
			" ",
			ScriptLocalization.Get("GameScene/" + starLogInfo.GainedStar.ToString() + "Star"),
			Environment.NewLine
		});
		for (int i = 0; i < 6; i++)
		{
			if (i < starLogInfo.starsUnlocked)
			{
				text2 += this.GetTMPSprite("TextSymbols3", "StarUnlocked");
			}
			else
			{
				text2 += this.GetTMPSprite("TextSymbols3", "StarLocked");
			}
		}
		text2 = string.Concat(new string[]
		{
			ScriptLocalization.Get("GameScene/Progress"),
			":",
			Environment.NewLine,
			"<align=\"center\">",
			text2
		});
		array[0] = text + text2;
		return array;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0006DBA4 File Offset: 0x0006BDA4
	private string[] CombatReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		CombatLogInfo combatLogInfo = logInfo as CombatLogInfo;
		this.ShowOutlineOnUnits(combatLogInfo.Units);
		this.ShowMarkers(new List<GameHex> { combatLogInfo.Battlefield });
		Faction faction = combatLogInfo.Winner.matFaction.faction;
		Faction faction2 = combatLogInfo.Defeated.matFaction.faction;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		text = string.Concat(new object[]
		{
			"<size=120%>",
			this.GetTMPSprite("TMPFactionsSpritesheet", faction.ToString() + "Logo"),
			"</size> ",
			this.GetFactionName(faction),
			" ",
			combatLogInfo.WinnerPower.selectedPower + combatLogInfo.WinnerPower.cardsPower,
			" ",
			this.GetTMPSprite("TextSymbols3", "CombatVictory")
		});
		text2 = string.Concat(new object[]
		{
			"<size=120%>",
			this.GetTMPSprite("TMPFactionsSpritesheet", faction2.ToString() + "Logo"),
			"</size> ",
			this.GetFactionName(faction2),
			" ",
			combatLogInfo.DefeatedPower.selectedPower + combatLogInfo.DefeatedPower.cardsPower
		});
		if (combatLogInfo.WinnerAbilityUsed)
		{
			text3 = this.GetTMPSprite("TextSymbols3", combatLogInfo.Winner.matFaction.abilities[2].ToString()) + " " + this.GetMechAbilityName(combatLogInfo.Winner.matFaction.faction, 2) + Environment.NewLine;
		}
		if (combatLogInfo.WinnerPower.selectedPower != 0)
		{
			text5 = string.Concat(new object[]
			{
				text5,
				this.GetTMPSprite("Power"),
				" ",
				combatLogInfo.WinnerPower.selectedPower
			});
		}
		if (combatLogInfo.WinnerPower.selectedCards != null && combatLogInfo.WinnerPower.selectedCards.Count != 0)
		{
			text5 = text5 + Environment.NewLine + this.GetTMPSprite("CombatCard");
			int num = 0;
			foreach (CombatCard combatCard in combatLogInfo.WinnerPower.selectedCards)
			{
				text5 = text5 + " " + combatCard.CombatBonus;
				if (num != combatLogInfo.WinnerPower.selectedCards.Count - 1)
				{
					text5 += ", ";
				}
			}
		}
		if (combatLogInfo.LostPopularity > 0)
		{
			text5 = string.Concat(new object[]
			{
				text5,
				Environment.NewLine,
				ScriptLocalization.Get("GameScene/Lost"),
				" ",
				this.GetTMPSprite("Popularity"),
				" ",
				combatLogInfo.LostPopularity
			});
		}
		if (combatLogInfo.DefeatedAbilityUsed)
		{
			text4 = this.GetTMPSprite("TextSymbols3", combatLogInfo.Defeated.matFaction.abilities[2].ToString()) + " " + this.GetMechAbilityName(combatLogInfo.Defeated.matFaction.faction, 2) + Environment.NewLine;
		}
		if (combatLogInfo.DefeatedPower.selectedPower != 0)
		{
			text6 = string.Concat(new object[]
			{
				text6,
				this.GetTMPSprite("Power"),
				" ",
				combatLogInfo.DefeatedPower.selectedPower
			});
		}
		if (combatLogInfo.DefeatedPower.selectedCards != null && combatLogInfo.DefeatedPower.selectedCards.Count != 0)
		{
			text6 = text6 + Environment.NewLine + this.GetTMPSprite("CombatCard");
			int num2 = 0;
			foreach (CombatCard combatCard2 in combatLogInfo.DefeatedPower.selectedCards)
			{
				text6 = text6 + " " + combatCard2.CombatBonus;
				if (num2 != combatLogInfo.DefeatedPower.selectedCards.Count - 1)
				{
					text6 += ", ";
				}
			}
		}
		array[0] = string.Concat(new string[]
		{
			text,
			Environment.NewLine,
			text3,
			text5,
			Environment.NewLine,
			text2,
			Environment.NewLine,
			text4,
			text6
		});
		return array;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0006E0B8 File Offset: 0x0006C2B8
	private string[] FactoryActionReport(LogInfo logInfo)
	{
		string[] array = new string[] { this.Gained, "" };
		string gained = this.Gained;
		if (logInfo.AdditionalGain.Count == 0)
		{
			array[0] = array[0] + this.Nothing;
			return array;
		}
		int num = 0;
		foreach (LogInfo logInfo2 in logInfo.AdditionalGain)
		{
			string[] array2 = this.ReportParser(logInfo2, true);
			if (array2 != null && array2[0].Length > 0)
			{
				array[0] = array[0] + array2[0];
				num++;
				if (num == this.gainPerLine)
				{
					array[0] = array[0] + Environment.NewLine;
					num = 0;
				}
			}
		}
		return array;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0006E18C File Offset: 0x0006C38C
	private string[] GainWorkerReport(LogInfo logInfo, bool additionalGain = false)
	{
		string[] array = new string[] { "", "" };
		WorkerLogInfo workerLogInfo = logInfo as WorkerLogInfo;
		string text = "";
		if (workerLogInfo.Position != null)
		{
			if (!additionalGain)
			{
				text = this.Gained;
			}
			text = string.Concat(new object[]
			{
				text,
				this.GetTMPSprite(ActionLogInterpreter.FirstToUpper("Worker")),
				" ",
				workerLogInfo.WorkersAmount
			});
			this.ShowMarkers(new List<GameHex> { workerLogInfo.Position });
		}
		else if (logInfo.AdditionalGain.Count == 0)
		{
			text = this.Gained + this.Nothing;
		}
		array[0] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, workerLogInfo.Position == null);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0006E274 File Offset: 0x0006C474
	private string[] TokenActionReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		TokenActionLogInfo tokenActionLogInfo = logInfo as TokenActionLogInfo;
		string text = "";
		switch (tokenActionLogInfo.action)
		{
		case TokenActionType.Placed:
			text = ScriptLocalization.Get("GameScene/Placed");
			break;
		case TokenActionType.Armed:
			text = ScriptLocalization.Get("GameScene/Armed");
			break;
		case TokenActionType.Triggered:
			text = ScriptLocalization.Get("GameScene/Triggered");
			break;
		}
		text += ":\n";
		if (tokenActionLogInfo.token is FlagToken)
		{
			text = text + this.GetTMPSprite("TextSymbols3", "FlagFront") + " " + ScriptLocalization.Get("GameScene/Flag");
		}
		else if (tokenActionLogInfo.token is TrapToken)
		{
			if (tokenActionLogInfo.action != TokenActionType.Triggered)
			{
				text = text + this.GetTMPSprite("TextSymbols3", "TrapBack") + " " + ScriptLocalization.Get("GameScene/Trap");
			}
			else
			{
				TrapToken trapToken = tokenActionLogInfo.token as TrapToken;
				switch (trapToken.Penalty)
				{
				case PayType.Coin:
					text = string.Concat(new string[]
					{
						text,
						this.GetTMPSprite("TextSymbols3", "TrapBack"),
						" ",
						ScriptLocalization.Get("GameScene/Trap"),
						"\n",
						ActionLogInterpreter.FirstToUpper(ScriptLocalization.Get("GameScene/Lost")),
						":\n",
						this.GetTMPSprite("Coin") + " " + trapToken.Amount.ToString()
					});
					break;
				case PayType.Popularity:
					text = string.Concat(new string[]
					{
						text,
						this.GetTMPSprite("TextSymbols3", "TrapBack"),
						" ",
						ScriptLocalization.Get("GameScene/Trap"),
						"\n",
						ActionLogInterpreter.FirstToUpper(ScriptLocalization.Get("GameScene/Lost")),
						":\n",
						this.GetTMPSprite("Popularity") + " " + trapToken.Amount.ToString()
					});
					break;
				case PayType.Power:
					text = string.Concat(new string[]
					{
						text,
						this.GetTMPSprite("TextSymbols3", "TrapBack"),
						" ",
						ScriptLocalization.Get("GameScene/Trap"),
						"\n",
						ActionLogInterpreter.FirstToUpper(ScriptLocalization.Get("GameScene/Lost")),
						":\n",
						this.GetTMPSprite("Power") + " " + trapToken.Amount.ToString()
					});
					break;
				case PayType.CombatCard:
					text = string.Concat(new string[]
					{
						text,
						this.GetTMPSprite("TextSymbols3", "TrapBack"),
						" ",
						ScriptLocalization.Get("GameScene/Trap"),
						"\n",
						ActionLogInterpreter.FirstToUpper(ScriptLocalization.Get("GameScene/Lost")),
						":\n",
						this.GetTMPSprite("CombatCard") + " " + trapToken.Amount.ToString()
					});
					break;
				}
			}
		}
		this.ShowOutlineOnUnits(new List<Unit> { tokenActionLogInfo.unit });
		this.ShowMarkers(new List<GameHex> { tokenActionLogInfo.token.Position });
		array[0] = text;
		return array;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0006E5F0 File Offset: 0x0006C7F0
	private string[] PassCoinsActionReport(LogInfo logInfo)
	{
		string[] array = new string[] { "", "" };
		PassCoinLogInfo passCoinLogInfo = logInfo as PassCoinLogInfo;
		string text = string.Concat(new object[]
		{
			this.GetTMPSprite("TMPFactionsSpritesheet", passCoinLogInfo.from.ToString() + "Logo"),
			" ",
			this.GetFactionName(passCoinLogInfo.from),
			"\n",
			"=(",
			this.GetTMPSprite("Coin"),
			" ",
			passCoinLogInfo.amount,
			")=>\n",
			this.GetTMPSprite("TMPFactionsSpritesheet", passCoinLogInfo.to.ToString() + "Logo"),
			" ",
			this.GetFactionName(passCoinLogInfo.to),
			"\n"
		});
		array[0] = text;
		string[] array2 = this.AdditionalGainReportFromLog(logInfo, true);
		array[0] = array[0] + array2[0];
		array[1] = array[1] + array2[1];
		return array;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0006E720 File Offset: 0x0006C920
	private string[] AdditionalGainReportFromLog(LogInfo logInfo, bool doesNotStartWithGainString = true)
	{
		string[] array = new string[] { "", "" };
		if (logInfo.AdditionalGain.Count == 0)
		{
			return array;
		}
		array[0] = (doesNotStartWithGainString ? this.Gained : "");
		int num = 0;
		if (!doesNotStartWithGainString && logInfo.Type != LogInfoType.Enlist)
		{
			num = 1;
		}
		bool flag = false;
		for (int i = 0; i < logInfo.AdditionalGain.Count; i++)
		{
			string[] array2 = this.ReportParser(logInfo.AdditionalGain[i], true);
			if (array2 != null && array2[0].Length > 0)
			{
				flag = true;
				array[0] = array[0] + array2[0];
				num++;
				array[1] = array[1] + array2[1];
				if (num == this.gainPerLine && i != logInfo.AdditionalGain.Count - 1)
				{
					array[0] = array[0] + Environment.NewLine;
					num = 0;
				}
			}
		}
		if (!flag)
		{
			string[] array3 = array;
			int num2 = 0;
			array3[num2] += this.Nothing;
		}
		return array;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0006E820 File Offset: 0x0006CA20
	public void ShowOutlineOnUnits(List<Unit> unitsWithOutline)
	{
		if (this.movePresenter.HaveAction())
		{
			this.movePresenter.HideUnitsOutline();
		}
		else
		{
			this.movePresenter.ClearLastClickedUnit();
		}
		foreach (Unit unit in this.unitsWithOutline)
		{
			if (unit != null)
			{
				this.GetUnitPresenter(unit).OutlineActivation(false, 0, true);
			}
		}
		this.unitsWithOutline.Clear();
		foreach (Unit unit2 in unitsWithOutline)
		{
			this.unitsWithOutline.Add(unit2);
			if (unit2 != null)
			{
				this.GetUnitPresenter(unit2).SetFocus(true, 0);
			}
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0006E904 File Offset: 0x0006CB04
	public void HideOutlineOnUnits()
	{
		foreach (Unit unit in this.unitsWithOutline)
		{
			if (unit != null)
			{
				this.GetUnitPresenter(unit).OutlineActivation(false, 0, true);
			}
		}
		if (this.movePresenter.HaveAction())
		{
			this.movePresenter.ShowUnitsOutline();
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002BB02 File Offset: 0x00029D02
	public void ShowOutlineOnBuilding(BuildingPresenter building)
	{
		this.buildingWithOutline = building;
		if (this.buildingWithOutline == null)
		{
			return;
		}
		this.buildingWithOutline.ActivateOutline(true);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002BB26 File Offset: 0x00029D26
	public void HideOutlineOnBuilding()
	{
		if (this.buildingWithOutline == null)
		{
			return;
		}
		this.buildingWithOutline.ActivateOutline(false);
		this.buildingWithOutline = null;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0006E97C File Offset: 0x0006CB7C
	public void ShowMarkers(List<GameHex> hexes)
	{
		this.ChangeActionPresentersState(false);
		this.hexesHighlight.Clear();
		foreach (GameHex gameHex in hexes)
		{
			this.hexesHighlight.Add(this.GetGameHexPresenter(gameHex.posX, gameHex.posY));
			((FlatGameHexPresenter)this.GetGameHexPresenter(gameHex.posX, gameHex.posY)).layerGroup.transform.GetChild(0).GetComponent<SpriteRenderer>().DOKill(true);
			((FlatGameHexPresenter)this.GetGameHexPresenter(gameHex.posX, gameHex.posY)).layerGroup.transform.GetChild(1).GetComponent<SpriteRenderer>().DOKill(true);
			((FlatGameHexPresenter)this.GetGameHexPresenter(gameHex.posX, gameHex.posY)).layerGroup.transform.GetChild(2).GetComponent<SpriteRenderer>().DOKill(true);
			((FlatGameHexPresenter)this.GetGameHexPresenter(gameHex.posX, gameHex.posY)).layerGroup.transform.GetChild(3).GetComponent<SpriteRenderer>().DOKill(true);
			this.GetGameHexPresenter(gameHex.posX, gameHex.posY).SetFocus(true, HexMarkers.MarkerType.Move, 0f, false);
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0006EAF0 File Offset: 0x0006CCF0
	public void HideMarkers()
	{
		foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesHighlight)
		{
			gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.Move, 0f, false);
		}
		this.ChangeActionPresentersState(true);
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0006EB50 File Offset: 0x0006CD50
	private void ChangeActionPresentersState(bool enableActionPresenters)
	{
		if (this.payResourcePresenter.HaveAction())
		{
			this.payResourcePresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.movePresenter.HaveAction())
		{
			if (enableActionPresenters && GameController.GameManager.moveManager.GetSelectedUnit() != null)
			{
				this.movePresenter.ShowPossibleMoves(true);
			}
			else
			{
				this.movePresenter.HidePossibleMoves();
			}
		}
		else if (this.tradePresenter.HaveAction())
		{
			this.tradePresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.producePresenter.HaveAction())
		{
			this.producePresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.deployPresenter.HaveAction())
		{
			this.deployPresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.buildPresenter.HaveAction())
		{
			this.buildPresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.deployPresenter.HaveAction())
		{
			this.producePresenter.EnableVisuals(enableActionPresenters);
		}
		else if (this.gainWorkerPresenter.HaveAction())
		{
			this.gainWorkerPresenter.EnableVisuals(enableActionPresenters);
		}
		this.SetCombatMarkersState(enableActionPresenters);
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0006EC68 File Offset: 0x0006CE68
	private void SetCombatMarkersState(bool enabled)
	{
		List<GameHex> battlefields = GameController.GameManager.combatManager.GetBattlefields();
		if (battlefields.Count == 0)
		{
			return;
		}
		foreach (GameHex gameHex in battlefields)
		{
			GameController.Instance.GetGameHexPresenter(gameHex).SetFocus(enabled, HexMarkers.MarkerType.Battle, 0f, false);
		}
	}

	// Token: 0x04000510 RID: 1296
	public LineRenderer pathLine;

	// Token: 0x04000511 RID: 1297
	public GameObject hint;

	// Token: 0x04000512 RID: 1298
	public PayResourcePresenter payResourcePresenter;

	// Token: 0x04000513 RID: 1299
	public MovePresenter movePresenter;

	// Token: 0x04000514 RID: 1300
	public TradePresenter tradePresenter;

	// Token: 0x04000515 RID: 1301
	public DeployPresenter deployPresenter;

	// Token: 0x04000516 RID: 1302
	public BuildPresenter buildPresenter;

	// Token: 0x04000517 RID: 1303
	public ProducePresenter producePresenter;

	// Token: 0x04000518 RID: 1304
	public GainWorkerPresenter gainWorkerPresenter;

	// Token: 0x04000519 RID: 1305
	public bool isFullyVisible = true;

	// Token: 0x0400051A RID: 1306
	public List<Color> factionColors;

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private Canvas parentCanvas;

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private RectTransform hintRectTransform;

	// Token: 0x0400051D RID: 1309
	private const string TMP_SPRITE_START = "<sprite name=\"";

	// Token: 0x0400051E RID: 1310
	private const string TMP_SPRITE_END = "\">";

	// Token: 0x0400051F RID: 1311
	private const string TMP_SPRITE = "<sprite=\"";

	// Token: 0x04000520 RID: 1312
	private const string TMP_NAME = "\" name=\"";

	// Token: 0x04000521 RID: 1313
	private const string SYMBOL_PACK_2 = "textSymbols2";

	// Token: 0x04000522 RID: 1314
	private const string SYMBOL_PACK_3 = "TextSymbols3";

	// Token: 0x04000523 RID: 1315
	private const string FACTION_ICONS = "TMPFactionsSpritesheet";

	// Token: 0x04000524 RID: 1316
	private List<Unit> unitsWithOutline = new List<Unit>();

	// Token: 0x04000525 RID: 1317
	private List<Scythe.BoardPresenter.GameHexPresenter> hexesHighlight = new List<Scythe.BoardPresenter.GameHexPresenter>();

	// Token: 0x04000526 RID: 1318
	private BuildingPresenter buildingWithOutline;

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private float hintHeightOffset;

	// Token: 0x04000528 RID: 1320
	private int gainPerLine = 2;
}
